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
        internal class AggregatedAllocator
        {
            struct AllocatedObject
            {
                internal long size;
                internal IntPtr ptr;

                internal AllocatedObject(long size, IntPtr ptr)
                {
                    this.size = size;
                    this.ptr = ptr;
                }
            }

            private CudaAggregatedSerializationState owner;

            /// <summary>
            /// Pointer to the start of the preallocated memory on the device
            /// </summary>
            private IntPtr baseDevicePtr;

            /// <summary>
            /// Current offset in the preallocated buffer (offset of the first free byte)
            /// </summary>
            long currentOffset;

            /// <summary>
            /// MaxSize of the preallocated buffer
            /// </summary>
            long aggregatedAllocatedSize;

            public long AllocatedSize {get { return aggregatedAllocatedSize; } }

            /// <summary>
            /// Set of all objects that have been copied to the preallocated buffer (direclty allocated objects are not in it)
            /// </summary>
            ICollection<object> allAggregatedObjects = new HashSet<object>();

            /// <summary>
            /// For each object allocated, keeps a link to the allocator which allocated it on the device
            /// </summary>
            static SafeDictionary<object, AggregatedAllocator> object2Allocator = new SafeDictionary<object, AggregatedAllocator>();

            /// <summary>
            /// All objects that have been directly allocated (without going to the preallocated buffer)
            /// </summary>
            IDictionary<object, AllocatedObject> directlyAllocatedObjects = new Dictionary<object, AllocatedObject>();

            internal static void RemoveObject(object o)
            {
                AggregatedAllocator all;
                if (object2Allocator.TryGetValue(o, out all))
                {
                    all.free(o);
                    object2Allocator.Remove(o);
                }
            }

            /// <summary>
            /// Buffer used to group all objects in host memory before copying to the device
            /// </summary>
            private byte[] _hostBufferArray;
            private IntPtr _hostBuffer = IntPtr.Zero;

            public long DeviceMemoryPressure
            {
                get
                {
                    return aggregatedAllocatedSize + (from a in directlyAllocatedObjects.Values select a.size).Sum();
                }
            }

            #region Async cuda malloc handling
            //private AsyncCudaMalloc asyncMalloc;
            //private IAsyncResult asyncResult;
            private GCHandle _hostBufferHandle;

            public IntPtr CudaMalloc(long size)
            {
                IntPtr dev;
#if DEBUG_ALLOC
                Logger.WriteLine("Aggregated cuda malloc {0}", size);
#endif
                if (owner.cuda.Malloc(out dev, size) != cudaError_t.cudaSuccess)
                    throw new ApplicationException(String.Format("CUDA Error {0} ({2}) Allocating {1} bytes", owner.cuda.GetPeekAtLastError(), size, owner.cuda.GetErrorString(owner.cuda.GetPeekAtLastError())));
                return dev;
            }

            public delegate IntPtr AsyncCudaMalloc(long size);
            #endregion

            private IntPtr getBasePtr()
            {
                return baseDevicePtr;
            }

            public IntPtr allocate(object o, long size)
            {
                if (size > MAX_SIZE_FOR_AGGREGATION)
                {
                    IntPtr dev;
                    if (owner.cuda.Malloc(out dev, size) != cudaError_t.cudaSuccess)
                        throw new ApplicationException(String.Format("CUDA Error {0} Allocating {1} bytes", owner.cuda.GetLastError(), size));
#if DEBUG_ALLOC
                    Logger.WriteLine("Allocating direct {0:X} -- {1}", dev.ToInt64(), o.GetType().FullName);
#endif
                    directlyAllocatedObjects[o] = new AllocatedObject(size, dev);
                    object2Allocator[o] = this;
                    return dev;
                }

                if (size > (aggregatedAllocatedSize - currentOffset))
                {
                    throw new ApplicationException("Not enough memory preallocated");
                }
                IntPtr res = new IntPtr(getBasePtr().ToInt64() + currentOffset);
                currentOffset += size;
                if (currentOffset % 8 != 0) currentOffset += 8 - currentOffset % 8; // Make sure next allocation is padded to 8 bytes
                allAggregatedObjects.Add(o);
                object2Allocator[o] = this;
#if DEBUG_ALLOC
                Logger.WriteLine("Allocating aggregated {0:X} -- {1} -- size {2}", res.ToInt64(), o.GetType().FullName, size);
#endif
                return res;
            }

            public void free(object o)
            {
                if (allAggregatedObjects.Contains(o))
                {
                    allAggregatedObjects.Remove(o);
                    if (allAggregatedObjects.Count == 0)
                    {
                        owner.CudaFree(getBasePtr());
                        baseDevicePtr = IntPtr.Zero;
                        aggregatedAllocatedSize = 0;
                        currentOffset = 0;
                    }
                }
                else
                {
                    AllocatedObject ao;
                    if (directlyAllocatedObjects.TryGetValue(o, out ao))
                    {
                        owner.CudaFree(ao.ptr);
                        directlyAllocatedObjects.Remove(o);
                    }
                }

                if (allAggregatedObjects.Count == 0 && directlyAllocatedObjects.Count == 0)
                {
                    owner.allAllocators.Remove(this);
                }
            }

            internal AggregatedAllocator(CudaAggregatedSerializationState state, long size)
            {
                owner = state;
                owner.allAllocators.Add(this);
                // Create the delegate.
                baseDevicePtr = CudaMalloc(size);
                aggregatedAllocatedSize = size;
                if (size > int.MaxValue) throw new ApplicationException(string.Format("Trying to allocate on host {0} bytes, which is bigger than {1} max int value", aggregatedAllocatedSize, int.MaxValue));

                _hostBufferArray = new byte[aggregatedAllocatedSize];
                _hostBufferHandle = GCHandle.Alloc(_hostBufferArray, GCHandleType.Pinned);
                _hostBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(_hostBufferArray, 0);
            }

            internal void checkEmpty()
            {
                if (getBasePtr() != IntPtr.Zero || allAggregatedObjects.Count != 0 ||
                    directlyAllocatedObjects.Count != 0)
                {
                    throw new ApplicationException("Allocator should be empty and it is not");
                }
            }

            public void cpy(IntPtr dev, Array param, uint size)
            {
                if (size == 0) return;
                if (size > MAX_SIZE_FOR_AGGREGATION)
                {
                    GCHandle gcHandle = GCHandle.Alloc(param, GCHandleType.Pinned);
                    IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(param as Array, 0);
                    owner.CudaMemCopy(dev, src, size, cudaMemcpyKind.cudaMemcpyHostToDevice, gcHandle);
                }
                else
                {
                    // Local host memcpy only here
                    long offset = dev.ToInt64() - getBasePtr().ToInt64();
                    GCHandle gcHandle = GCHandle.Alloc(param, GCHandleType.Pinned);
                    IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(param as Array, 0);
                    unsafe
                    {
                        // host->host copy of the pinned source array into the aggregated staging
                        // buffer. Buffer.MemoryCopy is cross-platform; this was previously a
                        // Windows-only msvcrt.dll!memcpy P/Invoke that threw DllNotFoundException
                        // on Linux/macOS.
                        byte* dst = (byte*)_hostBuffer.ToPointer() + offset;
                        Buffer.MemoryCopy((byte*)src.ToPointer(), dst, aggregatedAllocatedSize - offset, size);
                    }
                    gcHandle.Free();
                }
            }

            public BinaryWriter getWriter(IntPtr dest, long size)
            {
                bool buffer = size <= MAX_SIZE_FOR_AGGREGATION;
                if (buffer)
                {
                    unsafe
                    {
                        Int64 offset = dest.ToInt64() - getBasePtr().ToInt64();
                        var b = new IntPtr(_hostBuffer.ToInt64() + offset);
                        var ms = new UnmanagedMemoryStream((byte*)b.ToPointer(), 0, size, FileAccess.ReadWrite);
                        return new BinaryWriter(ms);
                    }
                }
                throw new ApplicationException("Trying to get a buffer writer on an object that should be serialized directly");
            }

            internal void copyBuffer()
            {
                GCHandle handle = GCHandle.Alloc(this);
                owner.CudaMemCopyRegistered(getBasePtr(), _hostBuffer, aggregatedAllocatedSize, cudaMemcpyKind.cudaMemcpyHostToDevice, handle);
            }

            public void freeBuffer()
            {
                _hostBufferHandle.Free();
                _hostBuffer = IntPtr.Zero;
                _hostBufferArray = null;
            }
        }
    }
}