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
    internal partial class CudaAggregatedSerializationState : CudaAbstractSerializationState
    {
        protected class CUDAAggregatedObjectSerializer : NativeObjectSerializer
        {
            private CudaAggregatedSerializationState state;
            private IntPtr directlyWrittenToBuffer = IntPtr.Zero;

            internal CUDAAggregatedObjectSerializer(CudaAggregatedSerializationState state, CUDAAggregatedSerializer ser)
                : base(state, ser, false)
            {
                this.state = state;
            }

            internal override void start(object param, Type type, IntPtr da, bool skipMemcpy = false)
            {
                base.start(param, type, da, skipMemcpy);
                long size = serState.nativePtrConverter.GetTypeInfo(param.GetType()).size;
                if (size <= MAX_SIZE_FOR_AGGREGATION)
                {
                    directlyWrittenToBuffer = state._currentAllocator.allocate(param, size);
                    bw = state._currentAllocator.getWriter(directlyWrittenToBuffer, size);
                    ms = null;
                }
            }

            internal override IntPtr AllocateObject(object param)
            {
                IntPtr dev;
                if (directlyWrittenToBuffer != IntPtr.Zero)
                    dev = directlyWrittenToBuffer;
                else
                {
                    var size = (uint)ms.Length;

                    dev = state._currentAllocator.allocate(param, size);
#if DEBUG_ALLOC
                Logger.WriteLine("Allocated {1} bytes @{0:X} -- {2}", dev.ToInt64(), size, param.GetType().FullName);
#endif
                }
                return dev;
            }

            internal override void CopyObject(object param, IntPtr dev, bool skipMemcpy = false)
            {
                if (!skipMemcpy && directlyWrittenToBuffer == IntPtr.Zero)
                {
                    byte[] numArray = ms.ToArray();
                    var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
                    IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(numArray, 0);
                    state.CudaMemCopy(dev, src, (uint)numArray.Length, cudaMemcpyKind.cudaMemcpyHostToDevice, gcHandle);
                }
                state.ghosts.Add(param, dev);
            }
        }
    }
}