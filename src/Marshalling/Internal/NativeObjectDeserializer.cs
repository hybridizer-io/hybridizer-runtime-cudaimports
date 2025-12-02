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
        internal abstract class NativeObjectDeserializer : FieldVisitor
        {
            protected NativeSerializerState serState;
            protected AbstractObjectVisiter deserializer;
            protected BinaryReader br;

            protected NativeObjectDeserializer(NativeSerializerState state, AbstractObjectVisiter deser)
            {
                serState = state;
                deserializer = deser;
            }

            internal override IntPtr AllocateObject(object param)
            {
                return IntPtr.Zero;
            }

            internal override void CopyObject(object param, IntPtr dev)
            {
            }

            protected override void HandleVTable(long o)
            {
            }

            protected override void HandlePaddingByte(long count)
            {
                br.BaseStream.Position += count;
            }

            protected override void HandleOverride(byte[] data)
            {
                br.ReadBytes(data.Length);
            }

            private static byte[] ReadBytes(BinaryReader reader, long totalBytesToRead)
            {
                List<byte> byteList = new List<byte>();
                const int bufferSize = int.MaxValue;
                long bytesRead = 0;

                while (bytesRead < totalBytesToRead)
                {
                    int bytesToReadNow = (int)Math.Min(bufferSize, totalBytesToRead - bytesRead);
                    byte[] buffer = reader.ReadBytes(bytesToReadNow);
                    
                    if (buffer.Length == 0)
                        break;

                    byteList.AddRange(buffer);
                    bytesRead += buffer.Length;
                }

                return byteList.ToArray();
            }


            protected override void HandlePrimitive(FieldTools.FieldDeclaration fd, object param)
            {
                var declaringType = fd.Info.DeclaringType;
                var isNullableField = declaringType.IsGenericType && declaringType.GetGenericTypeDefinition() == typeof(Nullable<>);
                if (isNullableField)
                {
                    ReadBytes(br, FieldTools.SizeOf(fd.Info.FieldType));
                    return;                        
                }

                if (fd.Count > 1)
                {
                    // This happens with fixed buffers
                    var value = fd.Info.GetValue(param);
                    GCHandle h = GCHandle.Alloc(value, GCHandleType.Pinned);
                    byte[] b = ReadBytes(br, fd.ByteCount);
                    Marshal.Copy(b, 0, h.AddrOfPinnedObject(), b.Length);
                    h.Free();
                    fd.Info.SetValue(param, value);
                    return;
                }

                Boolean failOnUnpad = !param.GetType().IsValueType;
                FieldInfo key = fd.Info;
                if (key.FieldType == typeof(bool)) 
                    key.SetValue(param, br.ReadInt32() != 0);
                else if (key.FieldType == typeof(int))
                    key.SetValue(param, br.ReadInt32());
                else if (key.FieldType == typeof(byte))
                    key.SetValue(param, br.ReadByte());
                else if (key.FieldType == typeof(short))
                    key.SetValue(param, br.ReadInt16());
                else if (key.FieldType == typeof(uint))
                    key.SetValue(param, br.ReadUInt32());
                else if (key.FieldType == typeof(float))
                    key.SetValue(param, br.ReadSingle());
                else if (key.FieldType == typeof(double))
                {
                    Unpad64(br, failOnUnpad);
                    key.SetValue(param, br.ReadDouble());
                }
                else if (key.FieldType == typeof(long))
                {
                    Unpad64(br, failOnUnpad);
                    key.SetValue(param, br.ReadInt64());
                }
                else if (key.FieldType == typeof(ulong))
                {
                    Unpad64(br, failOnUnpad);
                    key.SetValue(param, br.ReadUInt64());
                }
                else
                {
                    if (key.FieldType != typeof(IntPtr))
                        throw new NotImplementedException();
                    HandlePtr(fd, param, IntPtr.Zero);
                }
            }

            protected override void HandleOverridenPtr(FieldTools.FieldDeclaration key, object o, IntPtr devPtr)
            {
                br.ReadInt64(); // We need to ignore the corresponding bytes
                if (CudaRuntimeProperties.UseHybridArrays)
                {
                    br.ReadInt64(); // Ignore length / lowerbound
                }
            }

            protected override void HandlePtr(FieldTools.FieldDeclaration key, object param, IntPtr p, bool isResidentArray = false)
            {
                IntPtr ptr;
                Unpad64(br);
                ptr = new IntPtr(br.ReadInt64());
                key.Info.SetValue(param, ptr);

                // If wrapping arrays, write the length of the array and pad to 64 bits
                if (key.FieldType.IsArray && CudaRuntimeProperties.UseHybridArrays)
                {
                    
                    int rank = 1;
                    if (!isResidentArray)
                    {
                        var ar = key.Info.GetValue(param) as Array;
                        if (ar != null) rank = ar.Rank;
                    }
                    

                    // Read length / lowerbounds (and ignore values)
                    for (int i = 0; i < rank; ++i)
                        br.ReadInt64();
                }
            }

            protected override void HandleObject(FieldTools.FieldDeclaration key, object param, IntPtr p)
            {

                object target = key.Info.GetValue(param);
                Boolean failOnUnpad = !param.GetType().IsValueType;
                if (!Attribute.IsDefined(key.Info, typeof(SharedMemoryAttribute)))
                {
                    long readInt64 = Unpad64(br, false).ReadInt64();
                    IntPtr devP = new IntPtr(readInt64);
                    IntPtr unused = deserializer.VisitObject(target, devP);
                    if (CudaRuntimeProperties.UseHybridArrays && target != null && target is Array)
                    {
                        // Advance in buffer to get rid of array dimensions
                        for (int i = 0; i < (target as Array).Rank; ++i)
                            br.ReadInt64();
                    }
                }
                else
                {
                    serState.RemoveObject(target);
                }
            }

            protected override void HandleValueType(FieldTools.FieldDeclaration key, object param,
                Dictionary<FieldInfo, byte[]> overrides)
            {
                var p = key.Info.GetValue(param);
                if (key.Info.FieldType.IsEnum)
                {
                    if (Enum.GetUnderlyingType(key.FieldType) == typeof(int))
                        key.Info.SetValue(param, br.ReadInt32());
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(uint))
                        key.Info.SetValue(param, br.ReadUInt32());
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(long)) // TODO : verify padding
                        key.Info.SetValue(param, br.ReadInt64());
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(ulong))
                        key.Info.SetValue(param, br.ReadUInt64());
                    else
                        throw new ApplicationException("enum underlying type not implemented");
                }
                else
                {
                    VisitFields(p, serState.nativePtrConverter.GetTypeInfo(key.FieldType), overrides);
                    key.Info.SetValue(param, p);
                }
            }

            protected override void HandleDelegate(FieldTools.FieldDeclaration key, object param, IntPtr p)
            {
                if (key.Info.GetValue(param) != null)
                {
                    Delegate del = key.Info.GetValue(param) as Delegate;
                    var targetP = new IntPtr(br.ReadInt64());
                    var funcP = new IntPtr(br.ReadInt64());
                    var vectFuncP = new IntPtr(br.ReadInt64());
                    // WHY?? delegate target should be copied back as an object -- we should not copy it as a the delegate target
                    //if (serState.cleanUpNativeData)
                    //    deserializer.VisitObject(del.Target, targetP);
                }
                else
                {
                    br.ReadInt64(); br.ReadInt64(); br.ReadInt64();
                }
            }

            protected override void HandleEndOfObject(object param)
            {
                Unpad64(br, false);
                serState.RemoveObject(param);
            }
        }
    }
}