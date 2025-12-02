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
    internal class CudaSerializationState : CudaAbstractSerializationState
    {
        internal CudaSerializationState(NativePtrConverter ptrConverter, ICudaMarshalling cuda)
            : base(ptrConverter, cuda)
        {
            serializer = new CUDASerializer(this);
            deserializer = new CUDADeserializer(this);
        }
        
        internal CudaSerializationState(NativePtrConverter ptrConverter, ICudaMarshalling cuda, cudaStream_t stream)
            : base(ptrConverter, cuda, stream)
        {
            serializer = new CUDASerializer(this);
            deserializer = new CUDADeserializer(this);
        }

        #region CUDA Serialization

        internal override void RemoveNative(IntPtr native)
        {
            cudaError_t cuer = cuda.GetLastError();
            if (cuer != cudaError_t.cudaSuccess)
            {
                throw new ApplicationException(String.Format("CUDA error {0} - unmarshalling impossible", cuer));
            }
            base.RemoveNative(native);
        }

        internal override void RemoveObject(object p)
        {
            if (p == null) return;
            IntPtr dev;
            if (ghosts.TryGetValue(p, out dev))
            {
                CudaFree(dev);
            }
            base.RemoveObject(p);
        }

        protected class CUDASerializer : NativeSerializer
        {
            private CudaAbstractSerializationState CudaSerState {get { return (CudaAbstractSerializationState) serState; }}

            internal CUDASerializer(NativeSerializerState state)
                : base(state)
            {
            }

            public override IntPtr InitialVisit(object param)
            {
                CudaSerState.InitializeStream();
                IntPtr res = base.InitialVisit(param);
                CudaSerState.StreamSynchronize();
                return res;
            }

            protected override IntPtr SerializeObjectArray(object param, uint size)
            {
                IntPtr dev;
                IntPtr[] numArray = DeepSerializeArray(param as Array);
                var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
                if (CudaSerState.cuda.Malloc(out dev, size) != cudaError_t.cudaSuccess)
                    Logger.WriteLine("CUDA Error {0} Allocating {1} bytes", CudaSerState.cuda.GetLastError(), size);
#if DEBUG_ALLOC
                Logger.WriteLine("Allocated {1} bytes @{0:X} -- {2}", dev.ToInt64(), size, param.GetType().FullName);
#endif
                IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(numArray, 0);

                CudaSerState.CudaMemCopy(dev, src, size, cudaMemcpyKind.cudaMemcpyHostToDevice, gcHandle);
                return dev;
            }

            /// <summary>
            /// Serializes all the objects contained in <paramref name="ap"/>
            /// </summary>
            /// <param name="ap">Array of objects</param>
            /// <returns>an array of pointers pointing to native memory</returns>
            protected override IntPtr[] DeepSerializeArray(Array ap)
            {
                var numArray = new IntPtr[GetElementCount(ap)];
                int num1 = 0;
                foreach (object key in ap)
                {
                    if (key == null)
                    {
                        numArray[num1++] = IntPtr.Zero;
                    }
                    else
                    {
                        if (serState.ghosts.ContainsKey(key))
                        {
                            numArray[num1++] = serState.ghosts[key];
                        }
                        else
                        {
                            IntPtr num2 = VisitObject(key, IntPtr.Zero);
                            numArray[num1++] = new IntPtr((long)num2);
                        }
                    }
                }
                return numArray;
            }

            protected override IntPtr BinaryCopyArray(object param, uint size)
            {
                IntPtr dev;
                IntPtr src = IntPtr.Zero;
                bool blittable = true;
                GCHandle handle = new GCHandle();
                // in case of non-blittable data -> marshal by hand
                try
                {
                    handle = GCHandle.Alloc(param, GCHandleType.Pinned);
                    src = Marshal.UnsafeAddrOfPinnedArrayElement(param as Array, 0);
                }
                catch (ArgumentException)
                {
                    blittable = false;
                }

                if (!blittable)
                {
                    // in case of non-blittable data -> marshal by hand
                    byte[] data = SerializeNonBlittableArray(param as Array) ;
                    handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    src = Marshal.UnsafeAddrOfPinnedArrayElement(data as Array, 0);
                }

                if (CudaSerState.cuda.Malloc(out dev, size) != cudaError_t.cudaSuccess)
                {
                    string message = string.Format("CUDA Error {0} Allocating {1} bytes", CudaSerState.cuda.GetLastError(), size);
                    Logger.WriteLine(message);
                    throw new ApplicationException(message);
                }
                if (dev == IntPtr.Zero && !(param is Array && (param as Array).Length ==0))
                {
                    string message = string.Format("CUDA Error Allocating {0} bytes -- NO ERROR BUT RETURNED ZERO !!!", size);
                    Logger.WriteLine(message);
                    throw new ApplicationException(message);
                }
#if DEBUG_ALLOC
                Logger.WriteLine("Allocated {1} bytes @{0:X} -- {2}", dev.ToInt64(), size, param.GetType().FullName);
#endif

                CudaSerState.CudaMemCopy(dev, src, size, cudaMemcpyKind.cudaMemcpyHostToDevice, handle);
                
                return dev;
            }

            protected override IntPtr SerializeCustom(ICustomMarshalled customMarshalled)
            {
                var size = FieldTools.SizeOf(customMarshalled.GetType());
                var buffer = new byte[size];    
                var memoryStream = new MemoryStream(buffer);
                var br = new BinaryWriter(memoryStream);
                customMarshalled.MarshalTo(br, serState.nativePtrConverter.Flavor);
                return BinaryCopyArray(buffer, (uint)size);
            }

            internal override void Free(IntPtr ptr)
            {
                CudaSerState.CudaFree(ptr);
            }

            protected override FieldVisitor CreateFieldVisitor()
            {
                return new CUDAObjectSerializer(serState, this);
            }

            protected override void SerializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides,
                IResidentArray residentArray)
            {
                if (residentArray.Status == ResidentArrayStatus.DeviceNeedsRefresh)
                    residentArray.RefreshDevice();
                residentArray.Status = ResidentArrayStatus.HostNeedsRefresh;
                IntPtr devicePointer = residentArray.DevicePointer;
                var memoryStream = new MemoryStream();
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(devicePointer.ToInt64());
                }

                bool found = false;
                foreach (FieldTools.FieldDeclaration key in OrderedFields(type))
                {
                    if (key.Info == null) continue;
                    if (Attribute.IsDefined(key.Info, typeof(ResidentArrayHostAttribute)))
                    {
                        overrides.Add(key.Info, memoryStream.ToArray());
                        found = true;
                    }
                }

                if (!found)
                    Logger.WriteLine(String.Format(
                        "Resident array <{0}> does not declare a ResidentArrayHost entry", type.FullName));
            }
        }

        protected class CUDAObjectSerializer : NativeObjectSerializer
        {
            private CudaAbstractSerializationState CudaSerState { get { return (CudaAbstractSerializationState)serState; } }

            internal CUDAObjectSerializer(NativeSerializerState state, AbstractObjectVisiter ser)
                : base(state, ser)
            {
            }

            internal override IntPtr AllocateObject(object param)
            {
                IntPtr dev;
                var size = (uint)ms.Length;
                if (CudaSerState.cuda.Malloc(out dev, size) != cudaError_t.cudaSuccess)
                    Logger.WriteLine("CUDA Error {0} Allocating {1} bytes", CudaSerState.cuda.GetLastError(), size);
#if DEBUG_ALLOC
                Logger.WriteLine("Allocated {1} bytes @{0:X} -- {2}", dev.ToInt64(), size, param.GetType().FullName);
#endif
                serState.ghosts.Add(param, dev);
                return dev;
            }

            internal override void CopyObject(object param, IntPtr dev)
            {
                var size = (uint)ms.Length;
                byte[] numArray = ms.ToArray();
                var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
                IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(numArray, 0);
                CudaSerState.CudaMemCopy(dev, src, size, cudaMemcpyKind.cudaMemcpyHostToDevice, gcHandle);
            }
        }

        #endregion

        #region CUDA Deserialization

        private class CUDADeserializer : NativeDeserializer
        {
            private CudaAbstractSerializationState CudaSerState { get { return (CudaAbstractSerializationState)serState; } }

            public CUDADeserializer(NativeSerializerState state)
                : base(state)
            {
            }

            protected override void DeserializeRawData(byte[] data, IntPtr da, long size)
            {
                GCHandle gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                CudaSerState.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), da, size,
                            cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle);
            }

            protected override void DeserializeArray(object param, IntPtr da, Type type)
            {
                uint elementCount = GetElementCount(param as Array);
                int size = (int) (SizeOfArrayElt(type.GetElementType()) * elementCount);
                if (type.GetElementType().IsPrimitive || type.GetElementType().IsValueType)
                {
                    try
                    {
                        GCHandle gcHandle = GCHandle.Alloc(param, GCHandleType.Pinned);
                        CudaSerState.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(param as Array, 0), da, size,
                            cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle);
                    }
                    catch (ArgumentException)
                    {
                        // Not blittable
                        byte[] buffer;
                        if (serState.nonBlittableArrays.TryGetValue(param as Array, out buffer))
                        {
                            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                            CudaSerState.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), da, size, 
                                cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle);

                            var des = new NativeSerializer.NativeNonBlittableStructDeserializer(serState, this);
                            var fv = CreateFieldVisitor();
                            des.deserialize(param, type, fv, buffer);
                        }
                    }
                }
                else
                {
                    var handles = new IntPtr[elementCount];
                    var gcHandle = GCHandle.Alloc(handles, GCHandleType.Pinned);

                    // We explicitely use a synchronous copy here because we need the result before continuing deserializing objects in the array
                    CudaSerState.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(handles, 0), da, size,
                        cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle);

                    CudaSerState.RemoveObject(param);
                    DeserializeObjectArray(handles, param as Array);
                }
            }

            internal override void Free(IntPtr ptr)
            {
                // Dont do anything
            }

            protected override FieldVisitor CreateFieldVisitor()
            {
                return new CUDAObjectDeserializer(CudaSerState, this);
            }

            protected override void DeserializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides,
                IResidentArray residentArray)
            {
                IntPtr devicePointer = residentArray.DevicePointer;
                var memoryStream = new MemoryStream();
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(devicePointer.ToInt64());
                }
                foreach (FieldTools.FieldDeclaration key in OrderedFields(type))
                {
                    if (key.Info == null) continue;
                    if (Attribute.IsDefined(key.Info, typeof(ResidentArrayHostAttribute)))
                        overrides.Add(key.Info, memoryStream.ToArray());
                }
            }

            public override IntPtr InitialVisit(object param)
            {
                if (CUDAImports.cuda.GetPeekAtLastError() != cudaError_t.cudaSuccess)
                    throw new ApplicationException("CUDA error occured before deserialization (most probably during kernel call): " + CUDAImports.cuda.GetErrorString(CUDAImports.cuda.GetPeekAtLastError()));
                CudaSerState.InitializeStream();
                IntPtr res = base.InitialVisit(param);
                CudaSerState.StreamSynchronize();
                return res;
            }
        }

        private class CUDAObjectDeserializer : NativeObjectDeserializer
        {
            internal CUDAObjectDeserializer(CudaAbstractSerializationState state, AbstractObjectVisiter deser)
                : base(state, deser)
            {
            }

            private CudaAbstractSerializationState CudaSerState { get { return (CudaAbstractSerializationState)serState; } }

            internal override void start(object param, Type type, IntPtr da)
            {
                uint size = (uint)FieldTools.SizeOf(type);
                long expected = serState.nativePtrConverter.Convert(type).ToInt64();

                var buffer = new byte[size];
                GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                // We explicitely use a synchronous copy here because we need the result before continuing deserializing
                CudaSerState.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), da, size,
                    cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle, false, true);

                var ms = new MemoryStream(buffer);
                br = new BinaryReader(ms);
                long typeId = br.ReadInt64();
                if (typeId != expected)
                {
                    throw new ApplicationException(String.Format("Incompatible types expecting {0} and found {1}", expected, typeId));
                }
            }
        }

        #endregion
    }
}