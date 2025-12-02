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
        internal abstract class NativeObjectSerializer : FieldVisitor
        {
            protected NativeSerializerState serState;
            protected AbstractObjectVisiter serializer;
            protected BinaryWriter bw;
            protected MemoryStream ms;

            protected NativeObjectSerializer() : this(true) { }

            protected NativeObjectSerializer(bool initWriter)
            {
                if (initWriter)
                {
                    ms = new MemoryStream();
                    bw = new BinaryWriter(ms);
                }
            }

            protected NativeObjectSerializer(NativeSerializerState state, AbstractObjectVisiter ser)
                : this(state, ser, true) { }

            protected NativeObjectSerializer(NativeSerializerState state, AbstractObjectVisiter ser, bool initWriter)
                : this(initWriter)
            {
                serState = state;
                serializer = ser;
            }

            protected override void HandleVTable(long o)
            {
                Pad64(bw);
                bw.Write(o);
            }

            protected override void HandlePaddingByte(long count)
            {
                bw.Write(new byte[count]);
            }

            protected override void HandleOverride(byte[] data)
            {
                bw.Write(data);
            }

            protected override void HandlePrimitive(FieldTools.FieldDeclaration key, object param)
            {
                var declaringType = key.Info.DeclaringType;
                var isNullableField = declaringType.IsGenericType && declaringType.GetGenericTypeDefinition() == typeof(Nullable<>);

                object o;
                if (isNullableField && key.Name == "hasValue")
                    o = param != null;
                else if (isNullableField && key.Name == "value")
                    if (param == null)
                        o = Activator.CreateInstance(declaringType.GetGenericArguments()[0]);
                    else
                        o = param;
                else
                    o = key.Info.GetValue(param);

                if (key.Count > 1)
                {
                    // This happens with fixed buffers
                    GCHandle h = GCHandle.Alloc(o, GCHandleType.Pinned);
                    byte[] b = new byte[key.ByteCount];
                    Marshal.Copy(h.AddrOfPinnedObject(), b, 0, b.Length);
                    bw.Write(b);
                    h.Free();
                    return;
                }

                switch (key.TypeEnum)
                {
                    case FieldTools.FieldTypeEnum.BOOL:
                        bw.Write(((bool)o) ? 1 : 0);
                        break;
                    case FieldTools.FieldTypeEnum.BYTE:
                        bw.Write((byte)o);
                        break;
                    case FieldTools.FieldTypeEnum.SHORT:
                        bw.Write((short)o);
                        break;
                    case FieldTools.FieldTypeEnum.INT:
                        bw.Write((int)o);
                        break;
                    case FieldTools.FieldTypeEnum.UINT:
                        bw.Write((uint)o);
                        break;
                    case FieldTools.FieldTypeEnum.LONG:
                        Pad64(bw);
                        bw.Write((long)o);
                        break;
                    case FieldTools.FieldTypeEnum.ULONG:
                        Pad64(bw);
                        bw.Write((ulong)o);
                        break;
                    case FieldTools.FieldTypeEnum.DOUBLE:
                        Pad64(bw);
                        bw.Write((double)o);
                        break;
                    case FieldTools.FieldTypeEnum.FLOAT:
                        bw.Write((float)o);
                        break;
                    default:
                        if (key.FieldType != typeof(IntPtr))
                            throw new NotImplementedException();
                        // always write 8 bytes
                        HandlePtr(key, null, (IntPtr)o);
                        break;
                }
            }

            protected override void HandlePtr(FieldTools.FieldDeclaration key, object param, IntPtr p, bool isResidentArray = false)
            {
                Pad64(bw);
                bw.Write(p.ToInt64());

                // If wrapping arrays, write the length of the array and pad to 64 bits
                if (key.FieldType.IsArray && CudaRuntimeProperties.UseHybridArrays)
                {
                    if (isResidentArray)
                    {
                        bw.Write(1);
                        bw.Write(0);
                    }
                    else
                    {
                        Array array = key.Info.GetValue(param) as Array;
                        int rank = array == null ? 0 : array.Rank;
                        WriteHybArrayDimensions(array, rank, bw);
                    }
                }
            }

            protected override void HandleObject(FieldTools.FieldDeclaration key, object param, IntPtr p)
            {
                IntPtr ptr = serializer.VisitObject(key.Info.GetValue(param), IntPtr.Zero);
                HandlePtr(key, param, ptr);
            }
            
            protected override void HandleDelegate(FieldTools.FieldDeclaration key, object param, IntPtr p)
            {
                if(key.Info.GetValue(param) != null) {
                    Delegate del = key.Info.GetValue(param) as Delegate;
                    IntPtr ptr = IntPtr.Zero;
                    if (param != del.Target)
                    {
                        ptr = serializer.VisitObject(del.Target, IntPtr.Zero);
                    }
                    else
                    {
                        pendingDelegateTargets.Add(new PendingDelegateTarget { binaryWriter = bw, streamPosition = bw.BaseStream.Position });
                    }
                    IntPtr func = serState.nativePtrConverter.GetFunctionPointer(del.Method);
                    HandlePtr(key, param, ptr);
                    HandlePtr(key, param, func);
                    HandlePtr(key, param, del.Method.IsStatic ? new IntPtr(1) : IntPtr.Zero);
                }
                else
                {
                    HandlePtr(key, param, IntPtr.Zero);
                    HandlePtr(key, param, IntPtr.Zero);
                    HandlePtr(key, param, IntPtr.Zero);
                }
            }

            protected override void HandleValueType(FieldTools.FieldDeclaration key, object param,
                Dictionary<FieldInfo, byte[]> overrides)
            {
                object o = key.Info.GetValue(param);
                if (key.FieldType.IsEnum)
                {
                    if (Enum.GetUnderlyingType(key.FieldType) == typeof(int))
                        bw.Write((int)o);
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(uint))
                        bw.Write((uint)o);
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(long))
                        bw.Write((long)o);
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(ulong))
                        bw.Write((ulong)o);
                    else
                        throw new ApplicationException("enum underlying type not implemented");
                } 
                else if (typeof (ICustomMarshalled).IsAssignableFrom(key.FieldType))
                {
                    MarshalICustomMarshalled(o as ICustomMarshalled);
                }
                else
                {
                    VisitFields(o, serState.nativePtrConverter.GetTypeInfo(key.FieldType), overrides);
                }
            }

            internal void MarshalICustomMarshalled(ICustomMarshalled custom)
            {
                custom.MarshalTo(bw, serState.nativePtrConverter.Flavor);
            }

            protected override void HandleEndOfObject(object param)
            {
                Pad64(bw);
            }

            internal override void start(object param, Type type, IntPtr da)
            {
            }
        }
    }
}