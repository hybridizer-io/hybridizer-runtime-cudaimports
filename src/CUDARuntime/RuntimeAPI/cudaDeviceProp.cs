/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// CUDA device properties
    /// Complete documentation <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/structcudaDeviceProp.html">here</see>
    /// </summary>
    public struct cudaDeviceProp
    {
        /// <summary>
        /// ASCII string identifying device
        /// </summary>
        public char[] name;
        /// <summary>
        /// 16-byte unique identifier
        /// </summary>
        public char[] uuid;
		/// <summary>
		/// 8-byte locally unique identifier. Value is undefined on TCC and non-Windows platforms
		/// </summary>
		public char[] luid;
		/// <summary>
		/// LUID device node mask. Value is undefined on TCC and non-Windows platforms
		/// </summary>                
		public uint luidDeviceNodeMask;  
        /// <summary>
        /// Global memory available on device in bytes
        /// </summary>
        public size_t totalGlobalMem;            
        /// <summary>
        /// Shared memory available per block in bytes
        /// </summary>
        public size_t sharedMemPerBlock;         
        /// <summary>
        /// 32-bit registers available per block
        /// </summary>
        public int regsPerBlock;              
        /// <summary>
        /// Warp size in threads
        /// </summary>
        public int warpSize;                  
        /// <summary>
        /// Maximum pitch in bytes allowed by memory copies
        /// </summary>
        public size_t memPitch;                  
        /// <summary>
        /// Maximum number of threads per block
        /// </summary>
        public int maxThreadsPerBlock;        
        /// <summary>
        /// Maximum size of each dimension of a block
        /// </summary>
        public int[] maxThreadsDim;          
        /// <summary>
        /// Maximum size of each dimension of a grid
        /// </summary>
        public int[] maxGridSize;            
        /// <summary>
        /// Clock frequency in kilohertz
        /// </summary>
        public int clockRate;                 
        /// <summary>
        /// Constant memory available on device in bytes
        /// </summary>
        public size_t totalConstMem;             
        /// <summary>
        /// Major compute capability
        /// </summary>
        public int major;                     
        /// <summary>
        /// Minor compute capability
        /// </summary>
        public int minor;                     
        /// <summary>
        /// Alignment requirement for textures
        /// </summary>
        public size_t textureAlignment;          
        /// <summary>
        /// Pitch alignment requirement for texture references bound to pitched memory
        /// </summary>
        public size_t texturePitchAlignment;          
        /// <summary>
        /// Device can concurrently copy memory and execute a kernel
        /// </summary>
        public int deviceOverlap;             
        /// <summary>
        /// Number of multiprocessors on device
        /// </summary>
        public int multiProcessorCount;       
        /// <summary>
        /// Specified whether there is a run time limit on kernels
        /// </summary>
        public int kernelExecTimeoutEnabled;  
        /// <summary>
        /// Device is integrated as opposed to discrete
        /// </summary>
        public int integrated;                
        /// <summary>
        /// Device can map host memory with cudaHostAlloc/cudaHostGetDevicePointer
        /// </summary>
        public int canMapHostMemory;          
        /// <summary>
        /// Compute mode (See ::cudaComputeMode)
        /// </summary>
        public int computeMode;               
        /// <summary>
        /// Maximum 1D texture size
        /// </summary>
        public int maxTexture1D;
        /// <summary>
        /// Maximum size for 1D textures bound to linear memory
        /// </summary>
        public int maxTexture1DLinear;
        /// <summary>
        /// Maximum 2D texture dimensions
        /// </summary>
        public int[] maxTexture2D;           
        /// <summary>
        /// Maximum 2D texture dimensions
        /// </summary>
        public int[] maxTexture2DLinear;           
        /// <summary>
        /// Maximum 2D texture dimensions
        /// </summary>
        public int[] maxTexture2DGather;           
        /// <summary>
        /// Maximum 3D texture dimensions
        /// </summary>
        public int[] maxTexture3D;
        /// <summary>
        /// Maximum Cubemap texture dimensions
        /// </summary>
        public int maxTextureCubemap;
        /// <summary>
        /// Maximum 1D layered texture dimensions
        /// </summary>
        public int[] maxTexture1DLayered;
        /// <summary>
        /// Maximum 2D layered texture dimensions
        /// </summary>
        public int[] maxTexture2DLayered;
        /// <summary>
        /// Maximum Cubemap layered texture dimensions
        /// </summary>
        public int[] maxTextureCubemapLayered;

        /// <summary>
        /// Maximum 1D surface size
        /// </summary>
        public int maxSurface1D;              
        /// <summary>
        /// Maximum 2D surface size
        /// </summary>
        public int[] maxSurface2D;              
        /// <summary>
        /// Maximum 3D surface size
        /// </summary>
        public int[] maxSurface3D;              
        /// <summary>
        /// Maximum 1D layered surface size
        /// </summary>
        public int[] maxSurface1DLayered;              
        /// <summary>
        /// Maximum 2D layered surface size
        /// </summary>
        public int[] maxSurface2DLayered;              
        /// <summary>
        /// Maximum
        /// </summary>
        public int maxSurfaceCubemap;              
        /// <summary>
        /// Maximum
        /// </summary>
        public int[] maxSurfaceCubemapLayered;              
        /// <summary>
        /// Alignment requirements for surfaces
        /// </summary>
        public size_t surfaceAlignment;          
                                      
        // 5.5
        /// <summary>
        /// Device can possibly execute multiple kernels concurrently
        /// </summary>
        public int concurrentKernels;         
        /// <summary>
        /// Device has ECC support enabled
        /// </summary>
        public int ECCEnabled;                
        /// <summary>
        /// PCI bus ID of the device
        /// </summary>
        public int pciBusID;                  
        /// <summary>
        /// PCI device ID of the device
        /// </summary>
        public int pciDeviceID;
        /// <summary>
        /// PCI domain ID of the device
        /// </summary>
        public int pciDomainID;
        /// <summary>
        /// 1 if device is a Tesla device using TCC driver, 0 otherwise
        /// </summary>
        public int tccDriver;
        /// <summary>
        /// Number of asynchronous engines
        /// </summary>
        public int asyncEngineCount;
        /// <summary>
        /// Device shares a unified address space with the host
        /// </summary>
        public int unifiedAddressing;
        /// <summary>
        /// Peak memory clock frequency in kilohertz
        /// </summary>
        public int memoryClockRate;
        /// <summary>
        /// Global memory bus width in bits
        /// </summary>
        public int memoryBusWidth;
        /// <summary>
        /// Size of L2 cache in bytes
        /// </summary>
        public int l2CacheSize;
        /// <summary>
        /// Maximum resident threads per multiprocessor
        /// </summary>
        public int maxThreadsPerMultiProcessor;
        /// <summary>
        /// Device supports stream priorities
        /// </summary>
        public int streamPrioritiesSupported;

        // 6.0
        /// <summary>
        /// Device supports caching globals in L1
        /// </summary>
        public int globalL1CacheSupported;     
        /// <summary>
        /// Device supports caching locals in L1
        /// </summary>
        public int localL1CacheSupported;      
        /// <summary>
        /// Shared memory available per multiprocessor in bytes
        /// </summary>
        public size_t sharedMemPerMultiprocessor; 
        /// <summary>
        /// 32-bit registers available per multiprocessor
        /// </summary>
        public int regsPerMultiprocessor;      
        /// <summary>
        /// Device supports allocating managed memory on this system
        /// </summary>
        public int managedMemory;              
        /// <summary>
        /// Device is on a multi-GPU board
        /// </summary>
        public int isMultiGpuBoard;            
        /// <summary>
        /// Unique identifier for a group of devices on the same multi-GPU board
        /// </summary>
        public int multiGpuBoardGroupID;       

        /// 8.0
        /// <summary>
        /// Link between the device and the host supports native atomic operations
        /// </summary>
        public int    hostNativeAtomicSupported;  
        /// <summary>
        /// Ratio of single precision performance (in floating-point operations per second) to double precision performance
        /// </summary>
        public int    singleToDoublePrecisionPerfRatio; 
        /// <summary>
        /// Device supports coherently accessing pageable memory without calling cudaHostRegister on it
        /// </summary>
        public int    pageableMemoryAccess;       
        /// <summary>
        /// Device can coherently access managed memory concurrently with the CPU
        /// </summary>
        public int    concurrentManagedAccess;    

        /// 9.0 - 9.1
        /// <summary>
        /// Device supports Compute Preemption
        /// </summary>
        public int    computePreemptionSupported;        
        /// <summary>
        /// Device can access host registered memory at the same virtual address as the CPU
        /// </summary>
        public int    canUseHostPointerForRegisteredMem; 
        /// <summary>
        /// Device supports launching cooperative kernels via ::cudaLaunchCooperativeKernel
        /// </summary>
        public int    cooperativeLaunch;                 
        /// <summary>
        /// Device can participate in cooperative kernels launched via ::cudaLaunchCooperativeKernelMultiDevice
        /// </summary>
        public int    cooperativeMultiDeviceLaunch;      
        /// <summary>
        /// Per device maximum shared memory per block usable by special opt in
        /// </summary>
        public size_t sharedMemPerBlockOptin;
        /// <summary>
        /// Device accesses pageable memory via the host's page tables
        /// </summary>
        public int pageableMemoryAccessUsesHostPageTables;
        /// <summary>
        /// Host can directly access managed memory on the device without migration.
        /// </summary>
        public int directManagedMemAccessFromHost;

        // 11.0
        /// <summary>
        /// Device's maximum l2 persisting lines capacity setting in bytes
        /// </summary>
        public int persistingL2CacheMaxSize;
        /// <summary>
        /// Maximum number of resident blocks per multiprocessor
        /// </summary>
        public int maxBlocksPerMultiProcessor;
        /// <summary>
        /// The maximum value of ::cudaAccessPolicyWindow::num_bytes.
        /// </summary>
        public int accessPolicyMaxWindowSize;
        /// <summary>
        /// Shared memory reserved by CUDA driver per block in bytes
        /// </summary>
        public size_t reservedSharedMemPerBlock;

        // 12
        /// <summary>
        /// Device supports host memory registration via ::cudaHostRegister.
        /// </summary>
        public int hostRegisterSupported;
        /// <summary>
        /// 1 if the device supports sparse CUDA arrays and sparse CUDA mipmapped arrays, 0 otherwise
        /// </summary>
        public int sparseCudaArraySupported;
        /// <summary>
        ///  Device supports using the ::cudaHostRegister flag cudaHostRegisterReadOnly to register memory that must be mapped as read-only to the GPU
        /// </summary>
        public int hostRegisterReadOnlySupported;
        /// <summary>
        /// External timeline semaphore interop is supported on the device
        /// </summary>
        public int timelineSemaphoreInteropSupported;
        /// <summary>
        /// 1 if the device supports using the cudaMallocAsync and cudaMemPool family of APIs, 0 otherwise
        /// </summary>
        public int memoryPoolsSupported;
        /// <summary>
        /// 1 if the device supports GPUDirect RDMA APIs, 0 otherwise
        /// </summary>
        public int gpuDirectRDMASupported;
        /// <summary>
        /// Bitmask to be interpreted according to the ::cudaFlushGPUDirectRDMAWritesOptions enum
        /// </summary>
        public uint gpuDirectRDMAFlushWritesOptions;
        /// <summary>
        /// See the ::cudaGPUDirectRDMAWritesOrdering enum for numerical values
        /// </summary>
        public int gpuDirectRDMAWritesOrdering;
        /// <summary>
        /// Bitmask of handle types supported with mempool-based IPC
        /// </summary>
        public int memoryPoolSupportedHandleTypes;
        /// <summary>
        /// 1 if the device supports deferred mapping CUDA arrays and CUDA mipmapped arrays
        /// </summary>
        public int deferredMappingCudaArraySupported;
        /// <summary>
        /// Device supports IPC Events.
        /// </summary>
        public int ipcEventSupported;
        /// <summary>
        /// Indicates device supports cluster launch
        /// </summary>
        public int clusterLaunch;
        /// <summary>
        /// Indicates device supports unified pointers
        /// </summary>
        public int unifiedFunctionPointers;
        /// <summary>
        /// Reserved for future use
        /// </summary>
        public int[] reserved2;
        /// <summary>
        /// Reserved for future use
        /// </summary>
        public int[] reserved;

        /// <summary>
        /// get cuda core count
        /// </summary>
        /// <returns></returns>
        public int GetCudaCoresCount()
        {
            int cores = 0;
            int mp = multiProcessorCount;
            switch (major)
            {
                case 1:
                case 2:
                    Console.Error.WriteLine("Deprecated device");
                    break;
                case 3: // https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#compute-capability-3-0
                    return mp * 192;
                case 5: // https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#compute-capability-5-x
                    return mp * 128;
                case 6: // https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#compute-capability-6-x
                    if (minor == 0)
                        return 64 * mp;
                    else if (minor == 1 || minor == 2)
                        return 128 * mp;
                    else
                        Console.Error.WriteLine("Unknown device type");
                    break;
                case 7: // https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#compute-capability-7-x
                    return 64 * mp;
                case 8: // https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#compute-capability-8-x
                    if (minor == 0)
                    {
                        return 64 * mp;
                    }
                    else if (minor == 6 || minor == 7 || minor == 9)
                    {
                        return 128 * mp;
                    }
                    else
                        Console.Error.WriteLine("Unknown device type");
                    break;
                case 9: // https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#compute-capability-9-0
                    return 128 * mp;
                default:
                    Console.Error.WriteLine("Unknown device type");
                    break;
            }
            return cores;
        }
    }
}
