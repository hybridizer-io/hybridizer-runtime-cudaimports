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
    internal abstract class CudaAbstractSerializationState : NativeSerializerState
    {
        public static int MIN_SIZE_FOR_HOST_REGISTER = 16384; // in bytes, objects bigger than this will be serialized independently

        internal ICudaMarshalling cuda;

        private cudaStream_t stream;
        private bool async = false; // If set to true, cuda memcpy will, to the extent of what is feasible, be asynchronous...
        private ICollection<GCHandle> allHandles;
        private ICollection<IntPtr> allDevicesPtrToFree;
        private ICollection<IntPtr> hostRegisteredPointers;

        public CudaAbstractSerializationState(NativePtrConverter ptrConverter, ICudaMarshalling cuda)
            : base(ptrConverter)
        {
            this.cuda = cuda;
            async = false;
        }

        public CudaAbstractSerializationState(NativePtrConverter ptrConverter, ICudaMarshalling cuda, cudaStream_t stream)
            : base(ptrConverter)
        {
            this.cuda = cuda;
            async = true;
            this.stream = stream;
        }

        internal void CudaMemCopyRegistered(IntPtr dst, IntPtr src, size_t size, cudaMemcpyKind kind, GCHandle handle)
        {
            if ((long) size == 0) return;
            if (async)
            {
                if (cuda.MemcpyAsync(dst, src, size, kind, stream) != cudaError_t.cudaSuccess)
                    throw new ApplicationException(String.Format("Error while copying data {0}", cuda.GetLastError()));
                allHandles.Add(handle);
            }
            else
            {
                if (cuda.Memcpy(dst, src, size, kind) != cudaError_t.cudaSuccess)
                    throw new ApplicationException(String.Format("Error while copying data {0}", cuda.GetLastError()));
                handle.Free();
            }
        }

        internal void CudaMemCopy(IntPtr dst, IntPtr src, size_t size, cudaMemcpyKind kind, GCHandle handle, bool freeSrc = false, bool synchronize = false)
        {
            if ((long) size == 0) return;
            IntPtr hostPtr = kind == cudaMemcpyKind.cudaMemcpyHostToDevice ? src : (kind == cudaMemcpyKind.cudaMemcpyDeviceToHost ? dst : IntPtr.Zero);
            bool registerHost = (Int64) size > MIN_SIZE_FOR_HOST_REGISTER;
            if (registerHost) cuda.HostRegister(hostPtr, size, 1);
            if (async)
            {
#if DEBUG_MEMCPY
                Logger.WriteLine("Copying from @{0:X} to @{1:X} {2} bytes", src.ToInt64(), dst.ToInt64(), size);
#endif
                if (cuda.MemcpyAsync(dst, src, size, kind, stream) != cudaError_t.cudaSuccess)
                    throw new ApplicationException(String.Format("Error while copying data {0}", cuda.GetLastError()));
                allHandles.Add(handle);
                if (registerHost) hostRegisteredPointers.Add(hostPtr);
                if (freeSrc)
                {
                    allDevicesPtrToFree.Add(src);
                }
                if (synchronize)
                {
                    cuda.StreamSynchronize(stream);
                }
            }
            else
            {
#if DEBUG_MEMCPY
                Logger.WriteLine("Copying from @{0:X} to @{1:X} {2} bytes", src.ToInt64(), dst.ToInt64(), size);
#endif
                if (src != IntPtr.Zero)
                {
                    cudaError_t err = cuda.Memcpy(dst, src, size, kind);
                    if (err != cudaError_t.cudaSuccess)
                    {
                        throw new ApplicationException( String.Format("Error while copying data {0} : {1}", err, cuda.GetErrorString(err)));
                    }
                }
                if (registerHost) cuda.HostUnregister(hostPtr);
                if (handle.IsAllocated) handle.Free();
                if (freeSrc)
                {                    
                    CudaFree(src);
                }
            }
        }

        internal void CudaFree(IntPtr src)
        {
#if DEBUG_ALLOC
                Logger.WriteLine("Freeing @{0:X}", src.ToInt64());
#endif

            cudaError_t error = cuda.Free(src);
            if (error != cudaError_t.cudaSuccess)
            {
                throw new ApplicationException(string.Format("Error {1} while freeing #{0:X}", src.ToInt64(), error));
            }
        }

        internal void InitializeStream()
        {
            if (async)
            {
                allHandles = new List<GCHandle>();
                allDevicesPtrToFree = new List<IntPtr>();
                hostRegisteredPointers = new List<IntPtr>();
            }
        }

        internal void StreamSynchronize()
        {
            if (!async || allHandles.Count == 0)  return;

            if (cuda.StreamSynchronize(stream) != cudaError_t.cudaSuccess)
            {
                Logger.WriteLine("Error while synchronizing stream");
            }

            ReleaseAsynchronousResources();
        }

        private void ReleaseAsynchronousResources()
        {
            // Then free all handles remaining
            if (allHandles != null)
            {
                foreach (GCHandle gcHandle in allHandles)
                {
                    try
                    {
                        gcHandle.Free();
                    }
                    catch (Exception)
                    {
                        Logger.WriteLine("Could not realease handle");
                    }
                }
                allHandles.Clear();
            }

            if (allDevicesPtrToFree != null)
            {
                foreach (var devPtr in allDevicesPtrToFree)
                {
                    CudaFree(devPtr);
                }
                allDevicesPtrToFree.Clear();
            }


            if (hostRegisteredPointers != null)
            {
                foreach (var hostPtr in hostRegisteredPointers)
                {
                    if (cuda.HostUnregister(hostPtr) != cudaError_t.cudaSuccess)
                    {
                        Logger.WriteLine("Error in cudaHostUnregister");
                    }
                }
                hostRegisteredPointers.Clear();
            }
        }

        internal override void Free()
        {
            base.Free();
            ReleaseAsynchronousResources();
        }
    }
}