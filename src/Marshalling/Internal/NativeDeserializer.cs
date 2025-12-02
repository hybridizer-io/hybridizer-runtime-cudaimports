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
        protected abstract class NativeDeserializer : AbstractObjectVisiter
        {
            internal NativeDeserializer(NativeSerializerState state)
                : base(state)
            {
            }

            public override IntPtr InitialVisit(object param)
            {
                IntPtr dev;
                if (param is Delegate) // Delegate as EntryPoint parameter
                {
                    if (serState.cleanUpNativeData)
                    {
                        Delegate del = param as Delegate;
                        InitialVisit(del.Target);
                    }
                    return IntPtr.Zero;
                }
                if (serState.ghosts.TryGetValue(param, out dev))
                {
                    return VisitObject(param, dev);
                }
                if (param is Array && CudaRuntimeProperties.UseHybridArrays)
                {
                    // Just free the CPU (managed) allocated buffer - no deserialization needed
                    GCHandle handle;
                    if (serState.directlyMappedArrayHandles.TryGetValue(param as Array, out handle))
                    {
                        handle.Free();
                        serState.directlyMappedArrayPtr.Remove(param as Array);
                        serState.directlyMappedArrayHandles.Remove(param as Array);
                    }
                }
                return IntPtr.Zero;
            }

            internal override IntPtr VisitObject(object param, IntPtr da)
            {
                VisitObjectInt(param, da);
                return IntPtr.Zero;
            }

            private void VisitObjectInt(object param, IntPtr da)
            {
                if (param == null || !serState.ghosts.ContainsKey(param))
                    return;
                Type type = param.GetType();
                if (type.IsArray)
                {
                    DeserializeArray(param, da, type);
                    serState.RemoveObject(param);
                }
                else if (type == typeof (string))
                {
                    serState.RemoveObject(param);
                }
                else
                {
                    if (!(type.IsClass || type.IsInterface || typeof(ICustomMarshalled).IsAssignableFrom(type)))
                        throw new NotImplementedException();
                    if (typeof (ICustomMarshalled).IsAssignableFrom(type))
                    {
                        // get size
                        long size = FieldTools.SizeOf(param.GetType());
                        byte[] data = new byte[size];
                        DeserializeRawData(data, da, size);
                        (param as ICustomMarshalled).UnmarshalFrom(new BinaryReader(new MemoryStream(data, false)), this.serState.nativePtrConverter.Flavor);
                        serState.RemoveObject(param);
                        return;
                    }
                    var overrides = new Dictionary<FieldInfo, byte[]>();
                    var residentArray = param as IResidentArray;
                    if (residentArray != null)
                    {
                        DeserializeResidentArray(type, overrides, residentArray);
                    }

                    FieldVisitor fv = CreateFieldVisitor();
                    TypeInfo typeInfo = serState.nativePtrConverter.GetTypeInfo(type);
                    fv.start(param, type, da);
                    fv.VisitFields(param, typeInfo, overrides);
                    IntPtr ptr = fv.AllocateObject(param);
                    fv.CopyObject(param, ptr);
                }
            }

            protected abstract void DeserializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides,
                IResidentArray residentArray);

            protected void DeserializeObjectArray(IntPtr[] handles, Array array)
            {
                int index = 0;
                foreach (object key in array)
                {
                    VisitObject(key, handles[index]);
                    ++index;
                }
            }

            protected abstract void DeserializeArray(object param, IntPtr da, Type type);

            protected abstract void DeserializeRawData(byte[] data, IntPtr da, long size);
        }
    }
}