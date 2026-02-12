/* (c) ALTIMESH 2018 -- all rights reserved */
//#define DEBUG
//#define DEBUG_ALLOC
//#define DEBUG_MEMCPY

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security;
using System.Text;
using System.Threading;
using NamingTools = Altimesh.Hybridizer.Runtime.NamingTools;

namespace Hybridizer.Runtime.CUDAImports
{
    internal abstract partial class NativeSerializerState
    {
        internal abstract class NativeSerializer : AbstractObjectVisiter
        {
            protected NativeSerializer(NativeSerializerState state)
                : base(state)
            {
            }

            public static int[] ArrayMultiIndex(Array ar, int k)
            {
                if (ar.Rank == 1) return new int[]{k} ;
                throw new ApplicationException();
                /*
                // TODO : test this part (ordering of indices wrt rank ?) !
                List<int> result = new List<int>();
                int index = k;
                for (int j = 0; j < ar.Rank; ++j)
                {
                    result.Add(index % ar.GetLength(j));
                    index /= ar.GetLength(j);
                }
                return result.ToArray();
                 * */
            }

            public class NativeNonBlittableStructSerializer : NativeObjectSerializer
            {
                public NativeNonBlittableStructSerializer(NativeSerializerState state, AbstractObjectVisiter ser)
                    : base(state, ser)
                { }

                internal override IntPtr AllocateObject(object param)
                {
                    return IntPtr.Zero ;
                }

                internal override void CopyObject(object param, IntPtr dev, bool skipMemcpy = false)
                {
                }

                public byte[] Finish()
                {
                    return this.ms.ToArray();
                }
            }

            internal class NativeNonBlittableStructDeserializer : NativeObjectDeserializer
            {
                public NativeNonBlittableStructDeserializer(NativeSerializerState state, AbstractObjectVisiter ser)
                    : base(state, ser)
                { }

                internal override void start(object param, Type type, IntPtr da, bool skipMemcpy = false)
                {
                }

                public void deserialize(object param, Type type, FieldVisitor fv, byte[] buffer)
                {
                    this.br = new BinaryReader(new MemoryStream(buffer));
                    Array ar = param as Array;
                    Type eltType = ar.GetType().GetElementType();
                    TypeInfo typeInfo = serState.nativePtrConverter.GetTypeInfo(eltType);
                    Dictionary<FieldInfo, byte[]> emptyOverrides = new Dictionary<FieldInfo, byte[]>();
                    for (int i = 0; i < ar.Length; ++i)
                    {
                        var elt = ar.GetValue(i);
                        start(elt, eltType, IntPtr.Zero);
                        VisitFields(elt, typeInfo, emptyOverrides);
                        AllocateObject(elt);
                        ar.SetValue(elt, i);
                        serState.nonBlittableArrays.Remove(ar);
;                    }
                }
            }

            public byte[] SerializeNonBlittableArray(Array ar)
            {
                NativeNonBlittableStructSerializer nos = new NativeNonBlittableStructSerializer(this.serState, this);
                // TODO : optimize this, and handle overrides
                TypeInfo ti = this.serState.nativePtrConverter.GetTypeInfo(ar.GetType().GetElementType());
                for (int k = 0; k < ar.Length; ++k)
                {
                    object val = ar.GetValue(ArrayMultiIndex(ar, k));
                    nos.VisitFields(val, ti, new Dictionary<FieldInfo, byte[]> { });
                }
                byte[] bytes = nos.Finish();
                serState.nonBlittableArrays[ar] = bytes;
                return bytes;
            }

            public override IntPtr InitialVisit(object param, bool skipMemcpy = false)
            {
                if (typeof(ICustomMarshalled).IsAssignableFrom(param.GetType()) && param.GetType().IsValueType)
                {
                    ICustomMarshalled icm = param as ICustomMarshalled;
                    var buffer = new byte[FieldTools.SizeOf(param.GetType())];
                    var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    icm.MarshalTo(new BinaryWriter(new MemoryStream(buffer)), serState.nativePtrConverter.Flavor);
                    serState.directlyMappedArrayHandles[param] = handle;
                    IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                    serState.directlyMappedArrayPtr[param] = ptr;
                    return ptr;
                }
                else if (param is Delegate) // Delegate as EntryPoint parameter
                {
                    Delegate del = param as Delegate;
                    IntPtr target = serState.serializer.VisitObject(del.Target, IntPtr.Zero, skipMemcpy);
                    IntPtr fPtr = serState.nativePtrConverter.GetFunctionPointer(del.Method);

                    byte[] buffer = new byte[24];
                    var bw = new BinaryWriter(new MemoryStream(buffer));
                    bw.Write(target.ToInt64());
                    bw.Write(fPtr.ToInt64());
					// TODO: handle statid delegates
					bw.Write(IntPtr.Zero.ToInt64());
                    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    serState.directlyMappedArrayHandles[del] = handle;
                    IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                    serState.directlyMappedArrayPtr[del] = ptr;
                    return ptr;
                }
                IntPtr res = VisitObject(param, IntPtr.Zero, skipMemcpy);
                if (param is Array && CudaRuntimeProperties.UseHybridArrays)
                {
                    // Pass a ptr to a hybarray struct in CPU memory
                    // This is a specific mode for arrays passed as Wrapper parameter (not kernel !) and only when using hybarrays
                    return WrapArray(param as Array, res);
                }
                return res;
            }

            protected virtual IntPtr WrapArray(Array array, IntPtr res, bool skipMemcpy = false)
            {
                int rank = array.Rank;
                byte[] buffer = new byte[8 + 8 * rank]; // 8 bytes for the ptr + 8 bytes per dimension (length and lowerbound)
                var bw = new BinaryWriter(new MemoryStream(buffer));
                bw.Write(res.ToInt64());
                WriteHybArrayDimensions(array, rank, bw);
                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                serState.directlyMappedArrayHandles[array] = handle;
                IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                serState.directlyMappedArrayPtr[array] = ptr;
                return ptr;
            }
            
            internal unsafe override IntPtr VisitObject(object param, IntPtr da, bool skipMemcpy = false)
            {
                if (param == null)
                    return IntPtr.Zero;
                if (serState.ghosts.ContainsKey(param))
                    return serState.ghosts[param];
                Type type = param.GetType();
                if (typeof (ICustomMarshalled).IsAssignableFrom(type))
                {
                    IntPtr result = SerializeCustom(param as ICustomMarshalled, skipMemcpy);
                    AddGhost(param, result);
                    return result;
                }
                if (type.IsArray || type == typeof(string))
                {
                    IntPtr dev;
                    Array array;
                    if (type.IsArray)
                        array = param as Array;
                    else
                        array = Encoding.ASCII.GetBytes(param as string + "\0");
                    uint elementCount = GetElementCount(array);
                    uint size = SizeOfArrayElt(array.GetType().GetElementType()) * elementCount;
                    if (array.GetType().GetElementType().IsPrimitive || array.GetType().GetElementType().IsValueType)
                    {
                        dev = BinaryCopyArray(array, size, skipMemcpy);
                    }
                    else
                    {
                        dev = SerializeObjectArray(array, size, skipMemcpy);
                    }

                    AddGhost(param, dev);
#if DEBUG_ALLOC
                    Logger.WriteLine("Array {0} serialized to {1:X}", param, dev.ToInt64());
#endif
                    return dev;
                }
                else if (typeof(Delegate).IsAssignableFrom(type))
                {
                    Delegate del = param as Delegate;
                    if (del == null)
                        throw new ApplicationException("INTERNAL ERROR - expecting delegate");
                    // delegates are arrays of two intptr
                    IntPtr target = serState.serializer.VisitObject(del.Target, IntPtr.Zero, skipMemcpy);
                    IntPtr fPtr = serState.nativePtrConverter.GetFunctionPointer(del.Method);
                    IntPtr dev = BinaryCopyArray(new long[]{target.ToInt64(), fPtr.ToInt64(), 0L}, SizeOfArrayElt(typeof(long)) * 3, skipMemcpy) ;
                    AddGhost(param, dev);
                    return dev;
                }
                else
                {
                    if (!(type.IsClass || type.IsInterface || (type.IsValueType && !type.IsPrimitive)))
                        throw new NotImplementedException();
                    if (type == typeof(Pointer))
                    {
                        Pointer p = (Pointer)param;
                        return (IntPtr)Pointer.Unbox(p);
                    }
                    var overrides = new Dictionary<FieldInfo, byte[]>();
                    var residentArray = param as IResidentArray;
                    if (residentArray != null)
                    {
                        SerializeResidentArray(type, overrides, residentArray);
                    }

                    TypeInfo ti = serState.nativePtrConverter.GetTypeInfo(type);
                    if (ti.CustomMarshaler != null)
                    {
                        return SerializeCustom(ti.CustomMarshaler, param, skipMemcpy);
                    }

                    FieldVisitor fv = CreateFieldVisitor();
                    fv.start(param, type, da, skipMemcpy);
                    fv.VisitFields(param, ti, overrides, skipMemcpy);
                    IntPtr dev = fv.AllocateObject(param);
                    foreach (var pending in fv.pendingDelegateTargets)
                    {
                        var oldPosition = pending.binaryWriter.BaseStream.Position;
                        pending.binaryWriter.Seek((int)pending.streamPosition, SeekOrigin.Begin);
                        Pad64(pending.binaryWriter);
                        pending.binaryWriter.Write(dev.ToInt64());
                        pending.binaryWriter.Seek((int)oldPosition, SeekOrigin.Begin);
                    }
                    fv.CopyObject(param, dev, skipMemcpy);
#if DEBUG_ALLOC
                    Logger.WriteLine("Object {0} serialized to {1:X}", param, dev.ToInt64());
#endif
                    return dev;
                }
            }

            protected virtual void AddGhost(object param, IntPtr dev)
            {
                serState.ghosts.Add(param, dev);
            }

            protected abstract void SerializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides,
                IResidentArray residentArray);

            protected abstract IntPtr SerializeObjectArray(object param, uint size, bool skipMemcpy = false);
            protected abstract IntPtr[] DeepSerializeArray(Array ap, bool skipMemcpy = false);
            protected abstract IntPtr BinaryCopyArray(object param, uint size, bool skipMemcpy = false);
            protected abstract IntPtr SerializeCustom(ICustomMarshalled customMarshalled, bool skipMemcpy = false);
            protected virtual IntPtr SerializeCustom(IHybCustomMarshaler marshaler, object customMarshalled, bool skipMemcpy = false)
            {
                var size = marshaler.SizeOf(customMarshalled);
                var buffer = new byte[size];
                var memoryStream = new MemoryStream(buffer);
                var br = new BinaryWriter(memoryStream);
                marshaler.MarshalTo(customMarshalled, br, serState.Marshaler);
                return BinaryCopyArray(buffer, (uint)size, skipMemcpy);
            }
        }
    }
}