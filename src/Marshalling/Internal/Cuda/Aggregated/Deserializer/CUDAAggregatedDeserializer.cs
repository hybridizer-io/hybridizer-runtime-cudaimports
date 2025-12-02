/* (c) ALTIMESH 2018 -- all rights reserved */
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
    internal partial class CudaAggregatedSerializationState : CudaAbstractSerializationState
    {
        private class CUDAAggregatedDeserializer : NativeDeserializer
        {
            private CudaAggregatedSerializationState state;
            public CUDAAggregatedDeserializer(CudaAggregatedSerializationState state)
                : base(state)
            {
                this.state = state;
            }

            public override IntPtr InitialVisit(object param)
            {
                state.InitializeStream();
                IntPtr res = base.InitialVisit(param);
                state.StreamSynchronize();
                return res;

            }

            protected override void DeserializeRawData(byte[] data, IntPtr da, long size)
            {
                var gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                state.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), da, size, cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle, false, true);
            }

            protected override void DeserializeArray(object param, IntPtr da, Type type)
            {
                uint elementCount = GetElementCount(param as Array);
                uint size = SizeOfArrayElt(type.GetElementType()) * elementCount;
                if (type.GetElementType().IsPrimitive || type.GetElementType().IsValueType)
                {
                    var gcHandle = GCHandle.Alloc(param, GCHandleType.Pinned);
                    state.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(param as Array, 0), da, size, cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle);
                }
                else
                {
                    var handles = new IntPtr[elementCount];
                    var gcHandle = GCHandle.Alloc(handles, GCHandleType.Pinned);
                    state.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(handles as Array, 0), da, size, cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle, false, true);

                    gcHandle.Free();
                    DeserializeObjectArray(handles, param as Array);
                }
            }

            internal override void Free(IntPtr ptr)
            {
                // Dont do anything
            }

            protected override FieldVisitor CreateFieldVisitor()
            {
                return new CUDAAggregatedObjectDeserializer(state, this);
            }

            protected override void DeserializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides,
                IResidentArray residentArray)
            {
                var savedStatus = residentArray.Status;
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
                residentArray.Status = savedStatus; // Make sure the status has not changed
            }

        }
    }
}