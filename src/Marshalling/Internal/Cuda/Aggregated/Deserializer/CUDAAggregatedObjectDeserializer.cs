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
        private class CUDAAggregatedObjectDeserializer : NativeObjectDeserializer
        {
            private CudaAggregatedSerializationState state;
            internal CUDAAggregatedObjectDeserializer(CudaAggregatedSerializationState state, AbstractObjectVisiter deser)
                : base(state, deser)
            {
                this.state = state;
            }

            internal override void start(object param, Type type, IntPtr da, bool skipMemcpy = false)
            {
                uint size = (uint)FieldTools.SizeOf(type);
                long expected = serState.nativePtrConverter.Convert(type).ToInt64();

                var buffer = new byte[size];
                GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                state.CudaMemCopy(Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0), da, size,
                    cudaMemcpyKind.cudaMemcpyDeviceToHost, gcHandle, false, true);

                gcHandle.Free();

                var ms = new MemoryStream(buffer);
                br = new BinaryReader(ms);
                long typeId = br.ReadInt64();
                if (typeId != expected)
                {
                    throw new ApplicationException(String.Format("Incompatible types expecting {0} and found {1}", expected, typeId));
                }
            }
        }
    }
}