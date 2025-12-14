/* (c) ALTIMESH 2022 -- all rights reserved */
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
    /// CUDA runtime API wrapper
    /// </summary>
    internal unsafe partial class CudaImplem
    {
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		public struct cudaDeviceProp_131
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			public char[] name;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public char[] uuid;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public char[] luid;
			public uint luidDeviceNodeMask;
			public size_t totalGlobalMem;
			public size_t sharedMemPerBlock;
			public int regsPerBlock;
			public int warpSize;
			public size_t memPitch;
			public int maxThreadsPerBlock;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxThreadsDim;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxGridSize;
			public size_t totalConstMem;
			public int major;
			public int minor;
			public size_t textureAlignment;
			public size_t texturePitchAlignment;
			public int multiProcessorCount;
			public int integrated;
			public int canMapHostMemory;
			public int maxTexture1D;
			public int maxTexture1DMipmap;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxTexture2D;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxTexture2DMipmap;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxTexture2DLinear;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxTexture2DGather;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxTexture3D;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxTexture3DAlt;
			public int maxTextureCubemap;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxTexture1DLayered;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxTexture2DLayered;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxTextureCubemapLayered;
			public int maxSurface1D;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxSurface2D;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxSurface3D;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxSurface1DLayered;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public int[] maxSurface2DLayered;
			public int maxSurfaceCubemap;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
			public int[] maxSurfaceCubemapLayered;
			size_t surfaceAlignment;
			public int concurrentKernels;
			public int ECCEnabled;
			public int pciBusID;
			public int pciDeviceID;
			public int pciDomainID;
			public int tccDriver;
			public int asyncEngineCount;
			public int unifiedAddressing;
			public int memoryBusWidth;
			public int l2CacheSize;
            public int persistingL2CacheMaxSize;
            public int maxThreadsPerMultiProcessor;
			public int streamPrioritiesSupported;
			public int globalL1CacheSupported;
			public int localL1CacheSupported;
			public size_t sharedMemPerMultiprocessor;
			public int regsPerMultiprocessor;
			public int managedMemory;
			public int isMultiGpuBoard;
			public int multiGpuBoardGroupID;
			public int hostNativeAtomicSupported;
			public int singleToDoublePrecisionPerfRatio;
			public int pageableMemoryAccess;
			public int concurrentManagedAccess;
			public int computePreemptionSupported;
			public int canUseHostPointerForRegisteredMem;
			public int cooperativeLaunch;
			public size_t sharedMemPerBlockOptin;
			public int pageableMemoryAccessUsesHostPageTables;
			public int directManagedMemAccessFromHost;
            public int maxBlocksPerMultiProcessor; 
            public int accessPolicyMaxWindowSize;  
            public size_t reservedSharedMemPerBlock;
            public int hostRegisterSupported; 
            public int sparseCudaArraySupported; 
            public int hostRegisterReadOnlySupported;
            public int timelineSemaphoreInteropSupported; 
            public int memoryPoolsSupported;    
            public int gpuDirectRDMASupported;   
            public uint gpuDirectRDMAFlushWritesOptions;
            public int gpuDirectRDMAWritesOrdering;
            public int memoryPoolSupportedHandleTypes;
            public int deferredMappingCudaArraySupported;
            public int ipcEventSupported;       
            public int clusterLaunch;
            public int unifiedFunctionPointers;
            
            public int deviceNumaConfig;           /**< NUMA configuration of a device: value is of type ::cudaDeviceNumaConfig enum */
            public int deviceNumaId;               /**< NUMA node ID of the GPU memory */
            public int mpsEnabled;                 /**< Indicates if contexts created on this device will be shared via MPS */
            public int hostNumaId;                 /**< NUMA ID of the host node closest to the device or -1 when system does not support NUMA */
            public uint gpuPciDeviceID; /**< The combined 16-bit PCI device ID and 16-bit PCI vendor ID */
            public int gpuPciSubsystemID; /**< The combined 16-bit PCI subsystem ID and 16-bit PCI subsystem vendor ID */
            public int hostNumaMultinodeIpcSupported; /**< 1 if the device supports HostNuma location IPC between nodes in a multi-node system. */
            
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
            public int[] reserved;              
        }

        internal class Cuda_64_131_linux : ICuda
        {
            internal const string CUDARTDLL = "/usr/local/cuda-13.1/lib64/libcudart.so";

            #region Device Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceReset")]
            public static extern cudaError_t cudaDeviceReset();
            public cudaError_t DeviceReset() { return cudaDeviceReset(); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDevice")]
            public static extern cudaError_t cudaGetDevice(out int dev);
            public cudaError_t GetDevice(out int dev) { return cudaGetDevice(out dev); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDevice")]
            public static extern cudaError_t cudaSetDevice(int dev);
            public cudaError_t SetDevice(int dev) { return cudaSetDevice(dev); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDeviceCount")]
            public static extern cudaError_t cudaGetDeviceCount(out int devCount);
            public cudaError_t GetDeviceCount(out int devCount) { return cudaGetDeviceCount(out devCount); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDeviceProperties")]
            public static extern cudaError_t cudaGetDeviceProperties(out cudaDeviceProp_131 props, int device);
            public cudaError_t GetDeviceProperties(out cudaDeviceProp props, int device)
            {
                cudaDeviceProp_131 res;
                cudaError_t er = cudaGetDeviceProperties(out res, device);
                props = cuda.StructConvert<cudaDeviceProp>(res);
                return er;
            }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDeviceFlags")]
            public static extern cudaError_t cudaGetDeviceFlags(out uint flags);
            public cudaError_t GetDeviceFlags(out uint flags) { return cudaGetDeviceFlags(out flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDeviceFlags")]
            public static extern cudaError_t cudaSetDeviceFlags(deviceFlags flags);
            public cudaError_t SetDeviceFlags(deviceFlags flags) { return cudaSetDeviceFlags(flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetValidDevices")]
            private unsafe static extern cudaError_t cudaSetValidDevices(int* ptr, int len);

            public unsafe cudaError_t SetValidDevices(int[] devs)
            {
                cudaError_t res = cudaError_t.cudaSuccess;
                fixed (int* ptr = devs)
                {
                    res = cudaSetValidDevices(ptr, devs.Length);
                }
                return res;
            }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaChooseDevice")]
            public static extern cudaError_t cudaChooseDevice(out int device, ref cudaDeviceProp_131 prop);
            public cudaError_t ChooseDevice(out int device, ref cudaDeviceProp prop)
            {
                cudaDeviceProp_131 res;
                res = cuda.StructConvert<cudaDeviceProp_131>(prop);
                return cudaChooseDevice(out device, ref res);
            }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetAttribute")]
            public static extern cudaError_t cudaDeviceGetAttribute(out int value, cudaDeviceAttr attr, int device);
            public cudaError_t DeviceGetAttribute(out int value, cudaDeviceAttr attr, int device) { return cudaDeviceGetAttribute(out value, attr, device); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetByPCIBusId")]
            public static extern cudaError_t cudaDeviceGetByPCIBusId(out int device, string pciBusId);
            public cudaError_t DeviceGetByPCIBusId(out int device, string pciBusId) { return cudaDeviceGetByPCIBusId(out device, pciBusId); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetCacheConfig")]
            public static extern cudaError_t cudaDeviceGetCacheConfig(IntPtr pCacheConfig);
            public cudaError_t DeviceGetCacheConfig(IntPtr pCacheConfig) { return cudaDeviceGetCacheConfig(pCacheConfig); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetLimit")]
            public static extern cudaError_t cudaDeviceGetLimit(out size_t pValue, cudaLimit limit);
            public cudaError_t DeviceGetLimit(out size_t pValue, cudaLimit limit) { return cudaDeviceGetLimit(out pValue, limit); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetPCIBusId")]
            public static extern cudaError_t cudaDeviceGetPCIBusId(StringBuilder pciBusId, int len, int device);
            public cudaError_t DeviceGetPCIBusId(StringBuilder pciBusId, int len, int device) { return cudaDeviceGetPCIBusId(pciBusId, len, device); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetSharedMemConfig")]
            public static extern cudaError_t cudaDeviceGetSharedMemConfig(IntPtr pConfig);
            public cudaError_t DeviceGetSharedMemConfig(IntPtr pConfig) { return cudaDeviceGetSharedMemConfig(pConfig); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSetCacheConfig")]
            public static extern cudaError_t cudaDeviceSetCacheConfig(cudaFuncCache cacheConfig);
            public cudaError_t DeviceSetCacheConfig(cudaFuncCache cacheConfig) { return cudaDeviceSetCacheConfig(cacheConfig); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSetLimit")]
            public static extern cudaError_t cudaDeviceSetLimit(cudaLimit limit, size_t value);
            public cudaError_t DeviceSetLimit(cudaLimit limit, size_t value) { return cudaDeviceSetLimit(limit, value); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSetSharedMemConfig")]
            public static extern cudaError_t cudaDeviceSetSharedMemConfig(cudaSharedMemConfig config);
            public cudaError_t DeviceSetSharedMemConfig(cudaSharedMemConfig config) { return cudaDeviceSetSharedMemConfig(config); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSynchronize")]
            public static extern cudaError_t cudaDeviceSynchronize();
            public cudaError_t DeviceSynchronize() { return cudaDeviceSynchronize(); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcCloseMemHandle")]
            public static extern cudaError_t cudaIpcCloseMemHandle(IntPtr devPtr);
            public cudaError_t IpcCloseMemHandle(IntPtr devPtr) { return cudaIpcCloseMemHandle(devPtr); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcGetEventHandle")]
            public static extern cudaError_t cudaIpcGetEventHandle(out cudaIpcEventHandle_t handle, cudaEvent_t evt);
            public cudaError_t IpcGetEventHandle(out cudaIpcEventHandle_t handle, cudaEvent_t evt) { return cudaIpcGetEventHandle(out handle, evt); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcGetMemHandle")]
            public static extern cudaError_t cudaIpcGetMemHandle(out cudaIpcMemHandle_t handle, IntPtr devPtr);
            public cudaError_t IpcGetMemHandle(out cudaIpcMemHandle_t handle, IntPtr devPtr) { return cudaIpcGetMemHandle(out handle, devPtr); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcOpenEventHandle")]
            public static extern cudaError_t cudaIpcOpenEventHandle(out cudaEvent_t evt, cudaIpcEventHandle_t handle);
            public cudaError_t IpcOpenEventHandle(out cudaEvent_t evt, cudaIpcEventHandle_t handle) { return cudaIpcOpenEventHandle(out evt, handle); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcOpenMemHandle")]
            public static extern cudaError_t cudaIpcOpenMemHandle(out IntPtr devPtr, cudaIpcMemHandle_t handle, uint flags);
            public cudaError_t IpcOpenMemHandle(out IntPtr devPtr, cudaIpcMemHandle_t handle, uint flags) { return cudaIpcOpenMemHandle(out devPtr, handle, flags); }

            // new to 8.0
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetP2PAttribute")]
            public static extern cudaError_t cudaDeviceGetP2PAttribute(out int value, cudaDeviceP2PAttr attr, int srcDevice, int dstDevice);
            public cudaError_t DeviceGetP2PAttribute(out int value, cudaDeviceP2PAttr attr, int srcDevice, int dstDevice) { return cudaDeviceGetP2PAttribute(out value, attr, srcDevice, dstDevice); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetStreamPriorityRange")]
            public static extern cudaError_t cudaDeviceGetStreamPriorityRange(out int leastPriority, out int greatestPriority);
            public cudaError_t DeviceGetStreamPriorityRange(out int leastPriority, out int greatestPriority) { return cudaDeviceGetStreamPriorityRange(out leastPriority, out greatestPriority); }

            #endregion

            #region ErrorHandling

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetErrorName")]
            public static extern IntPtr cudaGetErrorName(cudaError_t err);
            public string GetErrorName(cudaError_t err) { return Marshal.PtrToStringAnsi(cudaGetErrorName(err)); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetErrorString")]
            public static extern IntPtr cudaGetErrorString(cudaError_t err);
            public string GetErrorString(cudaError_t err) { return Marshal.PtrToStringAnsi(cudaGetErrorString(err)); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetLastError")]
            public static extern cudaError_t cudaGetLastError();
            public cudaError_t GetLastError() { return cudaGetLastError(); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaPeekAtLastError")]
            public static extern cudaError_t cudaGetPeekAtLastError();
            public cudaError_t GetPeekAtLastError() { return cudaGetPeekAtLastError(); }

            #endregion

            #region Thread Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadExit")]
            public static extern cudaError_t cudaThreadExit();

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadGetLimit")]
            public static extern cudaError_t cudaThreadGetLimit(out size_t value, cudaLimit limit);

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadSetLimit")]
            public static extern cudaError_t cudaThreadSetLimit(cudaLimit limit, size_t value);

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadSynchronize")]
            public static extern cudaError_t cudaThreadSynchronize();

            public cudaError_t ThreadExit() { return cudaThreadExit(); }
            public cudaError_t ThreadGetLimit(out size_t value, cudaLimit limit) { return cudaThreadGetLimit(out value, limit); }
            public cudaError_t ThreadSetLimit(cudaLimit limit, size_t value) { return cudaThreadSetLimit(limit, value); }
            public cudaError_t ThreadSynchronize() { return cudaThreadSynchronize(); }

            #endregion

            #region Stream Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamCreate")]
            public static extern cudaError_t cudaStreamCreate(out cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamDestroy")]
            public static extern cudaError_t cudaStreamDestroy(cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamQuery")]
            public static extern cudaError_t cudaStreamQuery(cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamSynchronize")]
            public static extern cudaError_t cudaStreamSynchronize(cudaStream_t stream);

            public cudaError_t StreamCreate(out cudaStream_t stream) { return cudaStreamCreate(out stream); }
            public cudaError_t StreamDestroy(cudaStream_t stream) { return cudaStreamDestroy(stream); }
            public cudaError_t StreamQuery(cudaStream_t stream) { return cudaStreamQuery(stream); }
            public cudaError_t StreamSynchronize(cudaStream_t stream) { return cudaStreamSynchronize(stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamAddCallback")]
            public static extern cudaError_t cudaStreamAddCallback(cudaStream_t stream, cudaStreamCallback_t callback, IntPtr userData, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamAttachMemAsync")]
            public static extern cudaError_t cudaStreamAttachMemAsync(cudaStream_t stream, IntPtr devPtr, size_t length, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamCreateWithFlags")]
            public static extern cudaError_t cudaStreamCreateWithFlags(out cudaStream_t pStream, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamCreateWithPriority")]
            public static extern cudaError_t cudaStreamCreateWithPriority(out cudaStream_t pStream, uint flags, int priority);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamGetFlags")]
            public static extern cudaError_t cudaStreamGetFlags(cudaStream_t hStream, out uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamGetPriority")]
            public static extern cudaError_t cudaStreamGetPriority(cudaStream_t hStream, out int priority);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamWaitEvent")]
            public static extern cudaError_t cudaStreamWaitEvent(cudaStream_t stream, cudaEvent_t evt, uint flags);

            public cudaError_t StreamAddCallback(cudaStream_t stream, cudaStreamCallback_t callback, IntPtr userData, uint flags) { return StreamAddCallback(stream, callback, userData, flags); }
            public cudaError_t StreamAttachMemAsync(cudaStream_t stream, IntPtr devPtr, size_t length, uint flags) { return cudaStreamAttachMemAsync(stream, devPtr, length, flags); }
            public cudaError_t StreamCreateWithFlags(out cudaStream_t pStream, uint flags) { return cudaStreamCreateWithFlags(out pStream, flags); }
            public cudaError_t StreamCreateWithPriority(out cudaStream_t pStream, uint flags, int priority) { return cudaStreamCreateWithPriority(out pStream, flags, priority); }
            public cudaError_t StreamGetFlags(cudaStream_t hStream, out uint flags) { return cudaStreamGetFlags(hStream, out flags); }
            public cudaError_t StreamGetPriority(cudaStream_t hStream, out int priority) { return cudaStreamGetPriority(hStream, out priority); }
            public cudaError_t StreamWaitEvent(cudaStream_t stream, cudaEvent_t evt, uint flags) { return cudaStreamWaitEvent(stream, evt, flags); }

            #endregion

            #region Event Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventCreate")]
            public static extern cudaError_t cudaEventCreate(out cudaEvent_t evt);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventCreateWithFlags")]
            public static extern cudaError_t cudaEventCreateWithFlags(out cudaEvent_t evt, cudaEventFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventDestroy")]
            public static extern cudaError_t cudaEventDestroy(cudaEvent_t evt);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventElapsedTime")]
            public static extern cudaError_t cudaEventElapsedTime(out float ms, cudaEvent_t start, cudaEvent_t stop);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventQuery")]
            public static extern cudaError_t cudaEventQuery(cudaEvent_t evt);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventRecord")]
            public static extern cudaError_t cudaEventRecord(cudaEvent_t evt, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventSynchronize")]
            public static extern cudaError_t cudaEventSynchronize(cudaEvent_t evt);

            public cudaError_t EventCreate(out cudaEvent_t evt) { return cudaEventCreate(out evt); }
            public cudaError_t EventCreateWithFlags(out cudaEvent_t evt, cudaEventFlags flags) { return cudaEventCreateWithFlags(out evt, flags); }
            public cudaError_t EventDestroy(cudaEvent_t evt) { return cudaEventDestroy(evt); }
            public cudaError_t EventElapsedTime(out float ms, cudaEvent_t start, cudaEvent_t stop) { return cudaEventElapsedTime(out ms, start, stop); }
            public cudaError_t EventQuery(cudaEvent_t evt) { return cudaEventQuery(evt); }
            public cudaError_t EventRecord(cudaEvent_t evt, cudaStream_t stream) { return cudaEventRecord(evt, stream); }
            public cudaError_t EventSynchronize(cudaEvent_t evt) { return cudaEventSynchronize(evt); }

            #endregion

            #region Execution Control

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaConfigureCall")]
            public static extern cudaError_t cudaConfigureCall(dim3 gridDim, dim3 blockDim, size_t sharedMemory, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFuncGetAttributes")]
            public static extern cudaError_t cudaFuncGetAttributes(out cudaFuncAttributes attr, string func);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFuncSetCacheConfig")]
            public static extern cudaError_t cudaFuncSetCacheConfig(IntPtr func, cudaFuncCache cacheConfig);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaLaunch")]
            public static extern cudaError_t cudaLaunch(string func);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDoubleForDevice")]
            public static extern cudaError_t cudaSetDoubleForDevice(ref double d);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDoubleForHost")]
            public static extern cudaError_t cudaSetDoubleForHost(ref double d);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetupArgument")]
            public static extern cudaError_t cudaSetupArgument(IntPtr arg, size_t size, size_t offset);

            public cudaError_t ConfigureCall(dim3 gridDim, dim3 blockDim, size_t sharedMemory, cudaStream_t stream) { return cudaConfigureCall(gridDim, blockDim, sharedMemory, stream); }
            public cudaError_t FuncGetAttributes(out cudaFuncAttributes attr, string func) { return cudaFuncGetAttributes(out attr, func); }
            public cudaError_t FuncSetCacheConfig(IntPtr func, cudaFuncCache cacheConfig) { return cudaFuncSetCacheConfig(func, cacheConfig); }
            public cudaError_t Launch(string func) { return cudaLaunch(func); }
            public cudaError_t SetDoubleForDevice(ref double d) { return cudaSetDoubleForDevice(ref d); }
            public cudaError_t SetDoubleForHost(ref double d) { return cudaSetDoubleForHost(ref d); }
            public cudaError_t SetupArgument(IntPtr arg, size_t size, size_t offset) { return cudaSetupArgument(arg, size, offset); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFuncSetSharedMemConfig")]
            public static extern cudaError_t cudaFuncSetSharedMemConfig(IntPtr func, cudaSharedMemConfig config);

            public cudaError_t FuncSetSharedMemConfig(IntPtr func, cudaSharedMemConfig config) { return cudaFuncSetSharedMemConfig(func, config); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaLaunchKernel")]
            public static extern cudaError_t cudaLaunchKernel(IntPtr func, dim3 gridDim, dim3 blockDim, IntPtr args, size_t sharedMem, cudaStream_t stream);
            public cudaError_t LaunchKernel(IntPtr func, dim3 gridDim, dim3 blockDim, IntPtr args, size_t sharedMem, cudaStream_t stream) { return cudaLaunchKernel(func, gridDim, blockDim, args, sharedMem, stream); }

            #endregion

            #region Occupancy

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaOccupancyMaxActiveBlocksPerMultiprocessor ")]
            public static extern cudaError_t cudaOccupancyMaxActiveBlocksPerMultiprocessor(out int numBlocks, IntPtr function, int blockSize, size_t dynamicSharedSize);

            public cudaError_t OccupancyMaxActiveBlocksPerMultiprocessor(out int numBlocks, IntPtr function, int blockSize, size_t dynamicSharedSize)
            {
                return cudaOccupancyMaxActiveBlocksPerMultiprocessor(out numBlocks, function, blockSize, dynamicSharedSize);
            }

            #endregion

            #region Memory Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFree")]
            public static extern cudaError_t cudaFree(IntPtr dev);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeArray")]
            public static extern cudaError_t cudaFreeArray(cudaArray_t arr);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeHost")]
            public static extern cudaError_t cudaFreeHost(IntPtr ptr);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetSymbolAddress")]
            public static extern cudaError_t cudaGetSymbolAddress(out IntPtr devPtr, string symbol);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetSymbolSize")]
            public static extern cudaError_t cudaGetSymbolSize(out size_t size, string symbol);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostAlloc")]
            public static extern cudaError_t cudaHostAlloc(out IntPtr ptr, size_t size, cudaHostAllocFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostGetDevicePointer")]
            public static extern cudaError_t cudaHostGetDevicePointer(out IntPtr pdev, IntPtr phost, cudaGetDevicePointerFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostGetFlags")]
            public static extern cudaError_t cudaHostGetFlags(out cudaHostAllocFlags flags, IntPtr phost);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostRegister")]
            public static extern cudaError_t cudaHostRegister(IntPtr ptr, size_t size, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostUnregister")]
            public static extern cudaError_t cudaHostUnregister(IntPtr ptr);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMalloc")]
            public static extern cudaError_t cudaMalloc(out IntPtr dev, size_t size);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMalloc3D")]
            public static extern cudaError_t cudaMalloc3D(ref cudaPitchedPtr ptr, cudaFuncAttributes extent);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMalloc3DArray")]
            public static extern cudaError_t cudaMalloc3DArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, cudaFuncAttributes extent, cudaMallocArrayFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocArray")]
            public static extern cudaError_t cudaMallocArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, size_t width, size_t height, cudaMallocArrayFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocHost")]
            public static extern cudaError_t cudaMallocHost(out IntPtr ptr, size_t size);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocPitch")]
            public static extern cudaError_t cudaMallocPitch(out IntPtr dptr, out size_t pitch, size_t width, size_t height);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy")]
            public static extern cudaError_t cudaMemcpy(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2D")]
            public static extern cudaError_t cudaMemcpy2D(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DArrayToArray")]
            public static extern cudaError_t cudaMemcpy2DArrayToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DAsync")]
            public static extern cudaError_t cudaMemcpy2DAsync(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DFromArray")]
            public static extern cudaError_t cudaMemcpy2DFromArray(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DFromArrayAsync")]
            public static extern cudaError_t cudaMemcpy2DFromArrayAsync(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DToArray")]
            public static extern cudaError_t cudaMemcpy2DToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DToArrayAsync")]
            public static extern cudaError_t cudaMemcpy2DToArrayAsync(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3D")]
            public static extern cudaError_t cudaMemcpy3D(ref cudaMemcpy3DParms par);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3DAsync")]
            public static extern cudaError_t cudaMemcpy3DAsync(ref cudaMemcpy3DParms par, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyArrayToArray")]
            public static extern cudaError_t cudaMemcpyArrayToArray(cudaArray_t dest, size_t wOffsetDst, size_t hOffsetDst, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t count, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyAsync")]
            public static extern cudaError_t cudaMemcpyAsync(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromArray")]
            public static extern cudaError_t cudaMemcpyFromArray(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromArrayAsync")]
            public static extern cudaError_t cudaMemcpyFromArrayAsync(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromSymbol")]
            public static extern cudaError_t cudaMemcpyFromSymbol(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromSymbolAsync")]
            public static extern cudaError_t cudaMemcpyFromSymbolAsync(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToArray")]
            public static extern cudaError_t cudaMemcpyToArray(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToArrayAsync")]
            public static extern cudaError_t cudaMemcpyToArrayAsync(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToSymbol")]
            public static extern cudaError_t cudaMemcpyToSymbol(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToSymbolAsync")]
            public static extern cudaError_t cudaMemcpyToSymbolAsync(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemGetInfo")]
            public static extern cudaError_t cudaMemGetInfo(out size_t free, out size_t total);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset")]
            public static extern cudaError_t cudaMemset(IntPtr devPtr, int value, size_t count);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset2D")]
            public static extern cudaError_t cudaMemset2D(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset3D")]
            public static extern cudaError_t cudaMemset3D(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent);

            public cudaError_t Free(IntPtr dev) { return cudaFree(dev); }
            public cudaError_t FreeArray(cudaArray_t arr) { return cudaFreeArray(arr); }
            public cudaError_t FreeHost(IntPtr ptr) { return cudaFreeHost(ptr); }
            public cudaError_t GetSymbolAddress(out IntPtr devPtr, string symbol) { return cudaGetSymbolAddress(out devPtr, symbol); }
            public cudaError_t GetSymbolSize(out size_t size, string symbol) { return cudaGetSymbolSize(out size, symbol); }
            public cudaError_t HostAlloc(out IntPtr ptr, size_t size, cudaHostAllocFlags flags) { return cudaHostAlloc(out ptr, size, flags); }
            public cudaError_t HostGetDevicePointer(out IntPtr pdev, IntPtr phost, cudaGetDevicePointerFlags flags) { return cudaHostGetDevicePointer(out pdev, phost, flags); }
            public cudaError_t HostGetFlags(out cudaHostAllocFlags flags, IntPtr phost) { return cudaHostGetFlags(out flags, phost); }
            public cudaError_t HostRegister(IntPtr ptr, size_t size, uint flags) { return cudaHostRegister(ptr, size, flags); }
            public cudaError_t HostUnregister(IntPtr ptr) { return cudaHostUnregister(ptr); }
            public cudaError_t Malloc(out IntPtr dev, size_t size) { return cudaMalloc(out dev, size); }
            public cudaError_t Malloc3D(ref cudaPitchedPtr ptr, cudaFuncAttributes extent) { return cudaMalloc3D(ref ptr, extent); }
            public cudaError_t Malloc3DArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, cudaFuncAttributes extent, cudaMallocArrayFlags flags) { return cudaMalloc3DArray(out arr, ref chan, extent, flags); }
            public cudaError_t MallocArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, size_t width, size_t height, cudaMallocArrayFlags flags) { return cudaMallocArray(out arr, ref chan, width, height, flags); }
            public cudaError_t MallocHost(out IntPtr ptr, size_t size) { return cudaMallocHost(out ptr, size); }
            public cudaError_t MallocPitch(out IntPtr dptr, out size_t pitch, size_t width, size_t height) { return cudaMallocPitch(out dptr, out pitch, width, height); }
            public cudaError_t Memcpy(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind) { return cudaMemcpy(dest, src, size, kind); }
            public cudaError_t Memcpy2D(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2D(dest, dpitch, src, spitch, width, height, kind); }
            public cudaError_t Memcpy2DArrayToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2DArrayToArray(dest, wOffsetDest, hOffsetDest, src, wOffsetSrc, hOffsetSrc, width, height, kind); }
            public cudaError_t Memcpy2DAsync(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpy2DAsync(dest, dpitch, src, spitch, width, height, kind, stream); }
            public cudaError_t Memcpy2DFromArray(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2DFromArray(dest, dpitch, src, wOffset, hOffset, width, height, kind); }
            public cudaError_t Memcpy2DFromArrayAsync(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpy2DFromArrayAsync(dest, dpitch, src, wOffset, hOffset, width, height, kind, stream); }
            public cudaError_t Memcpy2DToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2DToArray(dest, wOffsetDest, hOffsetDest, src, spitch, width, height, kind); }
            public cudaError_t Memcpy2DToArrayAsync(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpy2DToArrayAsync(dest, wOffsetDest, hOffsetDest, src, spitch, width, height, kind, stream); }
            public cudaError_t Memcpy3D(ref cudaMemcpy3DParms par) { return cudaMemcpy3D(ref par); }
            public cudaError_t Memcpy3DAsync(ref cudaMemcpy3DParms par, cudaStream_t stream) { return cudaMemcpy3DAsync(ref par, stream); }
            public cudaError_t MemcpyArrayToArray(cudaArray_t dest, size_t wOffsetDst, size_t hOffsetDst, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t count, cudaMemcpyKind kind) { return cudaMemcpyArrayToArray(dest, wOffsetDst, hOffsetDst, src, wOffsetSrc, hOffsetSrc, count, kind); }
            public cudaError_t MemcpyAsync(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyAsync(dest, src, size, kind, stream); }
            public cudaError_t MemcpyFromArray(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind) { return cudaMemcpyFromArray(dest, src, wOffset, hOffset, count, kind); }
            public cudaError_t MemcpyFromArrayAsync(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyFromArrayAsync(dest, src, wOffset, hOffset, count, kind, stream); }
            public cudaError_t MemcpyFromSymbol(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind) { return cudaMemcpyFromSymbol(dest, symbol, count, offset, kind); }
            public cudaError_t MemcpyFromSymbolAsync(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyFromSymbolAsync(dest, symbol, count, offset, kind, stream); }
            public cudaError_t MemcpyToArray(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind) { return cudaMemcpyToArray(dest, wOffset, hOffset, src, count, kind); }
            public cudaError_t MemcpyToArrayAsync(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyToArrayAsync(dest, wOffset, hOffset, src, count, kind, stream); }
            public cudaError_t MemcpyToSymbol(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind) { return cudaMemcpyToSymbol(symbol, src, count, offset, kind); }
            public cudaError_t MemcpyToSymbolAsync(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyToSymbolAsync(symbol, src, count, offset, kind, stream); }
            public cudaError_t MemGetInfo(out size_t free, out size_t total) { return cudaMemGetInfo(out free, out total); }
            public cudaError_t Memset(IntPtr devPtr, int value, size_t count) { return cudaMemset(devPtr, value, count); }
            public cudaError_t Memset2D(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height) { return cudaMemset2D(devPtr, pitch, value, width, height); }
            public cudaError_t Memset3D(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent) { return cudaMemset3D(devPtr, value, extent); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaArrayGetInfo")]
            static extern cudaError_t cudaArrayGetInfo(out cudaChannelFormatDesc desc, out cudaFuncAttributes extent, out uint flags, cudaArray_t array); // Gets info about the specified cudaArray. 
            public cudaError_t ArrayGetInfo(out cudaChannelFormatDesc desc, out cudaFuncAttributes extent, out uint flags, cudaArray_t array)
            { return cudaArrayGetInfo(out desc, out extent, out flags, array); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeMipmappedArray")]
            static extern cudaError_t cudaFreeMipmappedArray(cudaMipmappedArray_t mipmappedArray); // Frees a mipmapped array on the device. 
            public cudaError_t FreeMipmappedArray(cudaMipmappedArray_t mipmappedArray) { return cudaFreeMipmappedArray(mipmappedArray); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeMipmappedArray")]
            static extern cudaError_t cudaGetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_t mipmappedArray, uint level); // Gets a mipmap level of a CUDA mipmapped array. 
            public cudaError_t GetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_t mipmappedArray, uint level) { return cudaGetMipmappedArrayLevel(out levelArray, mipmappedArray, level); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocManaged")]
            static extern cudaError_t cudaMallocManaged(out IntPtr devPtr, size_t size, uint flags); // Allocates memory that will be automatically managed by the Unified Memory system. 
            public cudaError_t MallocManaged(out IntPtr devPtr, size_t size, uint flags) { return cudaMallocManaged(out devPtr, size, flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocMipmappedArray")]
            static extern cudaError_t cudaMallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, int numLevels, uint flags); // Allocate a mipmapped array on the device. 
            public cudaError_t MallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, int numLevels, uint flags = 0)
            { return cudaMallocMipmappedArray(out mipmappedArray, ref desc, extent, numLevels, flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3DPeer")]
            static extern cudaError_t cudaMemcpy3DPeer(ref cudaMemcpy3DPeerParms p); // Copies memory between devices. 
            public cudaError_t Memcpy3DPeer(ref cudaMemcpy3DPeerParms p) { return cudaMemcpy3DPeer(ref p); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3DPeerAsync")]
            static extern cudaError_t cudaMemcpy3DPeerAsync(ref cudaMemcpy3DPeerParms p, cudaStream_t stream); // Copies memory between devices asynchronously. 
            public cudaError_t Memcpy3DPeerAsync(ref cudaMemcpy3DPeerParms p, cudaStream_t stream = new cudaStream_t()) { return cudaMemcpy3DPeerAsync(ref p, stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemsetAsync")]
            public static extern cudaError_t cudaMemsetAsync(IntPtr devPtr, int value, size_t count, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset2DAsync")]
            public static extern cudaError_t cudaMemset2DAsync(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset3DAsync")]
            public static extern cudaError_t cudaMemset3DAsync(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent, cudaStream_t stream);

            public cudaError_t MemsetAsync(IntPtr devPtr, int value, size_t count, cudaStream_t stream = new cudaStream_t()) { return cudaMemsetAsync(devPtr, value, count, stream); }
            public cudaError_t Memset2DAsync(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height, cudaStream_t stream = new cudaStream_t()) { return cudaMemset2DAsync(devPtr, pitch, value, width, height, stream); }
            public cudaError_t Memset3DAsync(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent, cudaStream_t stream = new cudaStream_t()) { return cudaMemset3DAsync(devPtr, value, extent, stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemPrefetchAsync")]
            public static extern cudaError_t cudaMemPrefetchAsync(IntPtr devptr, size_t count, int dstDevice, cudaStream_t stream);
            public cudaError_t MemPrefetchAsync(IntPtr devptr, size_t count, int dstDevice, cudaStream_t stream) { return cudaMemPrefetchAsync(devptr, count, dstDevice, stream); }
            public cudaError_t MemPrefetchAsync(IntPtr devptr, size_t count, int dstDevice) { return cudaMemPrefetchAsync(devptr, count, dstDevice, cudaStream_t.NO_STREAM); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyPeerAsync")]
            static extern cudaError_t cudaMemcpyPeerAsync(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count, cudaStream_t stream);
            public cudaError_t MemcpyPeerAsync(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count, cudaStream_t stream) { return cudaMemcpyPeerAsync(dest, dstDevice, src, srcDevice, count, stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyPeer")]
            static extern cudaError_t cudaMemcpyPeer(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count);
            public cudaError_t MemcpyPeer(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count) { return cudaMemcpyPeer(dest, dstDevice, src, srcDevice, count); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemAdvise")]
            static extern cudaError_t cudaMemAdvise(IntPtr devptr, size_t count, cudaMemmoryAdvise advice, int device);
            public cudaError_t MemAdvise(IntPtr devptr, size_t count, cudaMemmoryAdvise advice, int device) { return cudaMemAdvise(devptr, count, advice, device); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocMipmappedArray")]
            static extern cudaError_t cudaMallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, uint numLevels, uint flags);
            public cudaError_t MallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, uint numLevels, uint flags = 0) { return cudaMallocMipmappedArray(out mipmappedArray, ref desc, extent, numLevels, flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetMipmappedArrayLevel")]
            static extern cudaError_t cudaGetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_const_t mipmappedArray, uint level);
            public cudaError_t GetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_const_t mipmappedArray, uint level) { return cudaGetMipmappedArrayLevel(out levelArray, mipmappedArray, level); }


            /*
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "__make_cudaExtent")]
            static extern 
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "__make_cudaExtent")]
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "__make_cudaExtent")]

            public cudaExtent make_cudaExtent(size_t w, size_t h, size_t d) { return __make_cudaExtent(w, h, d); }
            public cudaExtent make_cudaPitchedPtr(IntPtr d, size_t p, size_t xsz, size_t ysz) { return __make_cudaPitchedPtr(d, p, xsz, ysz); }
            public cudaExtent make_cudaPos(size_t x, size_t y, size_t z) { return __make_cudaPos(x, y, z); }
            */
            #endregion

            #region Surface Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaCreateSurfaceObject")]
            public static extern cudaError_t cudaCreateSurfaceObject(out cudaSurfaceObject_t surface, ref cudaResourceDesc resDesc);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDestroySurfaceObject")]
            public static extern cudaError_t cudaDestroySurfaceObject(cudaSurfaceObject_t surface);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetSurfaceObjectResourceDesc")]
            public static extern cudaError_t cudaGetSurfaceObjectResourceDesc(out cudaResourceDesc resDesc, cudaSurfaceObject_t surface);

            public cudaError_t CreateSurfaceObject(out cudaSurfaceObject_t surface, ref cudaResourceDesc resDesc) { return cudaCreateSurfaceObject(out surface, ref resDesc); }
            public cudaError_t DestroySurfaceObject(cudaSurfaceObject_t surface) { return cudaDestroySurfaceObject(surface); }
            public cudaError_t GetSurfaceObjectResourceDesc(out cudaResourceDesc resDesc, cudaSurfaceObject_t surface) { return cudaGetSurfaceObjectResourceDesc(out resDesc, surface); }

            #endregion

            #region Texture Management


            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaCreateTextureObject")]
            public static extern cudaError_t cudaCreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc, ref cudaResourceViewDesc ResViewDesc);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaCreateTextureObject")]
            public static extern cudaError_t cudaCreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc, IntPtr ResViewDesc);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDestroyTextureObject")]
            public static extern cudaError_t cudaDestroyTextureObject(cudaTextureObject_t texture);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetTextureObjectResourceDesc")]
            public static extern cudaError_t cudaGetTextureObjectResourceDesc(out cudaResourceDesc resDesc, cudaTextureObject_t texture);

            public cudaError_t CreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc, ref cudaResourceViewDesc ResViewDesc) { return cudaCreateTextureObject(out texture, ref resDesc, ref texDesc, ref ResViewDesc); }
            public cudaError_t CreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc) { return cudaCreateTextureObject(out texture, ref resDesc, ref texDesc, IntPtr.Zero); }
            public cudaError_t DestroyTextureObject(cudaTextureObject_t texture) { return cudaDestroyTextureObject(texture); }
            public cudaError_t GetTextureObjectResourceDesc(out cudaResourceDesc resDesc, cudaTextureObject_t texture) { return cudaGetTextureObjectResourceDesc(out resDesc, texture); }

            #endregion

            #region Driver Interop Features

            public static class Interop
            {
                public struct CUmodule
                {
#pragma warning disable 0169
                    IntPtr mod;
#pragma warning restore 0169
                }

                public struct CUfunction
                {
#pragma warning disable 0169
                    IntPtr mod;
#pragma warning restore 0169
                }

                [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cuModuleLoadData")]
                public static extern cudaError_t cuModuleLoadData(out CUmodule module, IntPtr target);

                [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cuModuleGetFunction")]
                public static extern cudaError_t cuModuleGetFunction(out CUfunction function, CUmodule module, string name);
            }

            #endregion
            #region OpenGL interop


            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGLRegisterBufferObject(uint buffer);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsGLRegisterBuffer(out IntPtr pCudaResource, uint buffer, uint Flags);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsUnregisterResource(IntPtr resource);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGLUnregisterBufferObject(uint buffer);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsResourceGetMappedPointer(out IntPtr devPtr, out size_t size, IntPtr resource);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsResourceSetMapFlags(IntPtr resource, uint flags);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsMapResources(int count, IntPtr[] resources, cudaStream_t stream);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsUnmapResources(int count, IntPtr[] resources, cudaStream_t stream);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsGLRegisterImage(out IntPtr cudaGraphicsResource, uint image, uint target, uint flags);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsSubResourceGetMappedArray(out cudaArray_t array, IntPtr resource, uint arrayIndex, uint mipLevel);

            public cudaError_t GraphicsGLRegisterImage(out IntPtr cudaGraphicsResource, uint image, uint target, uint flags)
            {
                return cudaGraphicsGLRegisterImage(out cudaGraphicsResource, image, target, flags);
            }

            public cudaError_t GraphicsSubResourceGetMappedArray(out cudaArray_t array, IntPtr resource, uint arrayIndex, uint mipLevel)
            {
                return cudaGraphicsSubResourceGetMappedArray(out array, resource, arrayIndex, mipLevel);
            }

            public cudaError_t GLRegisterBufferObject(uint buffer)
            {
                return cudaGLRegisterBufferObject(buffer);
            }

            public cudaError_t GraphicsGLRegisterBuffer(out IntPtr pCudaResource, uint buffer, uint Flags)
            {
                return cudaGraphicsGLRegisterBuffer(out pCudaResource, buffer, Flags);
            }

            public cudaError_t GraphicsUnregisterResource(IntPtr resource)
            {
                return cudaGraphicsUnregisterResource(resource);
            }

            public cudaError_t GLUnregisterBufferObject(uint buffer)
            {
                return cudaGLUnregisterBufferObject(buffer);
            }

            public cudaError_t GraphicsResourceGetMappedPointer(out IntPtr devPtr, out size_t size, IntPtr resource)
            {
                return cudaGraphicsResourceGetMappedPointer(out devPtr, out size, resource);
            }

            public cudaError_t GraphicsResourceSetMapFlags(IntPtr resource, uint flags)
            {
                return cudaGraphicsResourceSetMapFlags(resource, flags);
            }

            public cudaError_t GraphicsMapResources(int count, IntPtr[] resources, cudaStream_t stream)
            {
                return cudaGraphicsMapResources(count, resources, stream);
            }

            public cudaError_t GraphicsUnmapResources(int count, IntPtr[] resources, cudaStream_t stream)
            {
                return cudaGraphicsUnmapResources(count, resources, stream);
            }
            #endregion

        }
    
    
        internal class Cuda_64_131_windows : ICuda
        {
            internal const string CUDARTDLL = "cudart64_13.dll";

            #region Device Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceReset")]
            public static extern cudaError_t cudaDeviceReset();
            public cudaError_t DeviceReset() { return cudaDeviceReset(); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDevice")]
            public static extern cudaError_t cudaGetDevice(out int dev);
            public cudaError_t GetDevice(out int dev) { return cudaGetDevice(out dev); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDevice")]
            public static extern cudaError_t cudaSetDevice(int dev);
            public cudaError_t SetDevice(int dev) { return cudaSetDevice(dev); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDeviceCount")]
            public static extern cudaError_t cudaGetDeviceCount(out int devCount);
            public cudaError_t GetDeviceCount(out int devCount) { return cudaGetDeviceCount(out devCount); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDeviceProperties")]
            public static extern cudaError_t cudaGetDeviceProperties(out cudaDeviceProp_131 props, int device);
            public cudaError_t GetDeviceProperties(out cudaDeviceProp props, int device)
            {
                cudaDeviceProp_131 res;
                cudaError_t er = cudaGetDeviceProperties(out res, device);
                props = cuda.StructConvert<cudaDeviceProp>(res);
                return er;
            }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetDeviceFlags")]
            public static extern cudaError_t cudaGetDeviceFlags(out uint flags);
            public cudaError_t GetDeviceFlags(out uint flags) { return cudaGetDeviceFlags(out flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDeviceFlags")]
            public static extern cudaError_t cudaSetDeviceFlags(deviceFlags flags);
            public cudaError_t SetDeviceFlags(deviceFlags flags) { return cudaSetDeviceFlags(flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetValidDevices")]
            private unsafe static extern cudaError_t cudaSetValidDevices(int* ptr, int len);

            public unsafe cudaError_t SetValidDevices(int[] devs)
            {
                cudaError_t res = cudaError_t.cudaSuccess;
                fixed (int* ptr = devs)
                {
                    res = cudaSetValidDevices(ptr, devs.Length);
                }
                return res;
            }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaChooseDevice")]
            public static extern cudaError_t cudaChooseDevice(out int device, ref cudaDeviceProp_131 prop);
            public cudaError_t ChooseDevice(out int device, ref cudaDeviceProp prop)
            {
                cudaDeviceProp_131 res;
                res = cuda.StructConvert<cudaDeviceProp_131>(prop);
                return cudaChooseDevice(out device, ref res);
            }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetAttribute")]
            public static extern cudaError_t cudaDeviceGetAttribute(out int value, cudaDeviceAttr attr, int device);
            public cudaError_t DeviceGetAttribute(out int value, cudaDeviceAttr attr, int device) { return cudaDeviceGetAttribute(out value, attr, device); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetByPCIBusId")]
            public static extern cudaError_t cudaDeviceGetByPCIBusId(out int device, string pciBusId);
            public cudaError_t DeviceGetByPCIBusId(out int device, string pciBusId) { return cudaDeviceGetByPCIBusId(out device, pciBusId); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetCacheConfig")]
            public static extern cudaError_t cudaDeviceGetCacheConfig(IntPtr pCacheConfig);
            public cudaError_t DeviceGetCacheConfig(IntPtr pCacheConfig) { return cudaDeviceGetCacheConfig(pCacheConfig); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetLimit")]
            public static extern cudaError_t cudaDeviceGetLimit(out size_t pValue, cudaLimit limit);
            public cudaError_t DeviceGetLimit(out size_t pValue, cudaLimit limit) { return cudaDeviceGetLimit(out pValue, limit); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetPCIBusId")]
            public static extern cudaError_t cudaDeviceGetPCIBusId(StringBuilder pciBusId, int len, int device);
            public cudaError_t DeviceGetPCIBusId(StringBuilder pciBusId, int len, int device) { return cudaDeviceGetPCIBusId(pciBusId, len, device); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetSharedMemConfig")]
            public static extern cudaError_t cudaDeviceGetSharedMemConfig(IntPtr pConfig);
            public cudaError_t DeviceGetSharedMemConfig(IntPtr pConfig) { return cudaDeviceGetSharedMemConfig(pConfig); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSetCacheConfig")]
            public static extern cudaError_t cudaDeviceSetCacheConfig(cudaFuncCache cacheConfig);
            public cudaError_t DeviceSetCacheConfig(cudaFuncCache cacheConfig) { return cudaDeviceSetCacheConfig(cacheConfig); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSetLimit")]
            public static extern cudaError_t cudaDeviceSetLimit(cudaLimit limit, size_t value);
            public cudaError_t DeviceSetLimit(cudaLimit limit, size_t value) { return cudaDeviceSetLimit(limit, value); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSetSharedMemConfig")]
            public static extern cudaError_t cudaDeviceSetSharedMemConfig(cudaSharedMemConfig config);
            public cudaError_t DeviceSetSharedMemConfig(cudaSharedMemConfig config) { return cudaDeviceSetSharedMemConfig(config); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceSynchronize")]
            public static extern cudaError_t cudaDeviceSynchronize();
            public cudaError_t DeviceSynchronize() { return cudaDeviceSynchronize(); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcCloseMemHandle")]
            public static extern cudaError_t cudaIpcCloseMemHandle(IntPtr devPtr);
            public cudaError_t IpcCloseMemHandle(IntPtr devPtr) { return cudaIpcCloseMemHandle(devPtr); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcGetEventHandle")]
            public static extern cudaError_t cudaIpcGetEventHandle(out cudaIpcEventHandle_t handle, cudaEvent_t evt);
            public cudaError_t IpcGetEventHandle(out cudaIpcEventHandle_t handle, cudaEvent_t evt) { return cudaIpcGetEventHandle(out handle, evt); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcGetMemHandle")]
            public static extern cudaError_t cudaIpcGetMemHandle(out cudaIpcMemHandle_t handle, IntPtr devPtr);
            public cudaError_t IpcGetMemHandle(out cudaIpcMemHandle_t handle, IntPtr devPtr) { return cudaIpcGetMemHandle(out handle, devPtr); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcOpenEventHandle")]
            public static extern cudaError_t cudaIpcOpenEventHandle(out cudaEvent_t evt, cudaIpcEventHandle_t handle);
            public cudaError_t IpcOpenEventHandle(out cudaEvent_t evt, cudaIpcEventHandle_t handle) { return cudaIpcOpenEventHandle(out evt, handle); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaIpcOpenMemHandle")]
            public static extern cudaError_t cudaIpcOpenMemHandle(out IntPtr devPtr, cudaIpcMemHandle_t handle, uint flags);
            public cudaError_t IpcOpenMemHandle(out IntPtr devPtr, cudaIpcMemHandle_t handle, uint flags) { return cudaIpcOpenMemHandle(out devPtr, handle, flags); }

            // new to 8.0
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetP2PAttribute")]
            public static extern cudaError_t cudaDeviceGetP2PAttribute(out int value, cudaDeviceP2PAttr attr, int srcDevice, int dstDevice);
            public cudaError_t DeviceGetP2PAttribute(out int value, cudaDeviceP2PAttr attr, int srcDevice, int dstDevice) { return cudaDeviceGetP2PAttribute(out value, attr, srcDevice, dstDevice); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDeviceGetStreamPriorityRange")]
            public static extern cudaError_t cudaDeviceGetStreamPriorityRange(out int leastPriority, out int greatestPriority);
            public cudaError_t DeviceGetStreamPriorityRange(out int leastPriority, out int greatestPriority) { return cudaDeviceGetStreamPriorityRange(out leastPriority, out greatestPriority); }

            #endregion

            #region ErrorHandling

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetErrorName")]
            public static extern IntPtr cudaGetErrorName(cudaError_t err);
            public string GetErrorName(cudaError_t err) { return Marshal.PtrToStringAnsi(cudaGetErrorName(err)); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetErrorString")]
            public static extern IntPtr cudaGetErrorString(cudaError_t err);
            public string GetErrorString(cudaError_t err) { return Marshal.PtrToStringAnsi(cudaGetErrorString(err)); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetLastError")]
            public static extern cudaError_t cudaGetLastError();
            public cudaError_t GetLastError() { return cudaGetLastError(); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaPeekAtLastError")]
            public static extern cudaError_t cudaGetPeekAtLastError();
            public cudaError_t GetPeekAtLastError() { return cudaGetPeekAtLastError(); }

            #endregion

            #region Thread Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadExit")]
            public static extern cudaError_t cudaThreadExit();

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadGetLimit")]
            public static extern cudaError_t cudaThreadGetLimit(out size_t value, cudaLimit limit);

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadSetLimit")]
            public static extern cudaError_t cudaThreadSetLimit(cudaLimit limit, size_t value);

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaThreadSynchronize")]
            public static extern cudaError_t cudaThreadSynchronize();

            public cudaError_t ThreadExit() { return cudaThreadExit(); }
            public cudaError_t ThreadGetLimit(out size_t value, cudaLimit limit) { return cudaThreadGetLimit(out value, limit); }
            public cudaError_t ThreadSetLimit(cudaLimit limit, size_t value) { return cudaThreadSetLimit(limit, value); }
            public cudaError_t ThreadSynchronize() { return cudaThreadSynchronize(); }

            #endregion

            #region Stream Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamCreate")]
            public static extern cudaError_t cudaStreamCreate(out cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamDestroy")]
            public static extern cudaError_t cudaStreamDestroy(cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamQuery")]
            public static extern cudaError_t cudaStreamQuery(cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamSynchronize")]
            public static extern cudaError_t cudaStreamSynchronize(cudaStream_t stream);

            public cudaError_t StreamCreate(out cudaStream_t stream) { return cudaStreamCreate(out stream); }
            public cudaError_t StreamDestroy(cudaStream_t stream) { return cudaStreamDestroy(stream); }
            public cudaError_t StreamQuery(cudaStream_t stream) { return cudaStreamQuery(stream); }
            public cudaError_t StreamSynchronize(cudaStream_t stream) { return cudaStreamSynchronize(stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamAddCallback")]
            public static extern cudaError_t cudaStreamAddCallback(cudaStream_t stream, cudaStreamCallback_t callback, IntPtr userData, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamAttachMemAsync")]
            public static extern cudaError_t cudaStreamAttachMemAsync(cudaStream_t stream, IntPtr devPtr, size_t length, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamCreateWithFlags")]
            public static extern cudaError_t cudaStreamCreateWithFlags(out cudaStream_t pStream, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamCreateWithPriority")]
            public static extern cudaError_t cudaStreamCreateWithPriority(out cudaStream_t pStream, uint flags, int priority);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamGetFlags")]
            public static extern cudaError_t cudaStreamGetFlags(cudaStream_t hStream, out uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamGetPriority")]
            public static extern cudaError_t cudaStreamGetPriority(cudaStream_t hStream, out int priority);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaStreamWaitEvent")]
            public static extern cudaError_t cudaStreamWaitEvent(cudaStream_t stream, cudaEvent_t evt, uint flags);

            public cudaError_t StreamAddCallback(cudaStream_t stream, cudaStreamCallback_t callback, IntPtr userData, uint flags) { return StreamAddCallback(stream, callback, userData, flags); }
            public cudaError_t StreamAttachMemAsync(cudaStream_t stream, IntPtr devPtr, size_t length, uint flags) { return cudaStreamAttachMemAsync(stream, devPtr, length, flags); }
            public cudaError_t StreamCreateWithFlags(out cudaStream_t pStream, uint flags) { return cudaStreamCreateWithFlags(out pStream, flags); }
            public cudaError_t StreamCreateWithPriority(out cudaStream_t pStream, uint flags, int priority) { return cudaStreamCreateWithPriority(out pStream, flags, priority); }
            public cudaError_t StreamGetFlags(cudaStream_t hStream, out uint flags) { return cudaStreamGetFlags(hStream, out flags); }
            public cudaError_t StreamGetPriority(cudaStream_t hStream, out int priority) { return cudaStreamGetPriority(hStream, out priority); }
            public cudaError_t StreamWaitEvent(cudaStream_t stream, cudaEvent_t evt, uint flags) { return cudaStreamWaitEvent(stream, evt, flags); }

            #endregion

            #region Event Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventCreate")]
            public static extern cudaError_t cudaEventCreate(out cudaEvent_t evt);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventCreateWithFlags")]
            public static extern cudaError_t cudaEventCreateWithFlags(out cudaEvent_t evt, cudaEventFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventDestroy")]
            public static extern cudaError_t cudaEventDestroy(cudaEvent_t evt);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventElapsedTime")]
            public static extern cudaError_t cudaEventElapsedTime(out float ms, cudaEvent_t start, cudaEvent_t stop);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventQuery")]
            public static extern cudaError_t cudaEventQuery(cudaEvent_t evt);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventRecord")]
            public static extern cudaError_t cudaEventRecord(cudaEvent_t evt, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaEventSynchronize")]
            public static extern cudaError_t cudaEventSynchronize(cudaEvent_t evt);

            public cudaError_t EventCreate(out cudaEvent_t evt) { return cudaEventCreate(out evt); }
            public cudaError_t EventCreateWithFlags(out cudaEvent_t evt, cudaEventFlags flags) { return cudaEventCreateWithFlags(out evt, flags); }
            public cudaError_t EventDestroy(cudaEvent_t evt) { return cudaEventDestroy(evt); }
            public cudaError_t EventElapsedTime(out float ms, cudaEvent_t start, cudaEvent_t stop) { return cudaEventElapsedTime(out ms, start, stop); }
            public cudaError_t EventQuery(cudaEvent_t evt) { return cudaEventQuery(evt); }
            public cudaError_t EventRecord(cudaEvent_t evt, cudaStream_t stream) { return cudaEventRecord(evt, stream); }
            public cudaError_t EventSynchronize(cudaEvent_t evt) { return cudaEventSynchronize(evt); }

            #endregion

            #region Execution Control

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaConfigureCall")]
            public static extern cudaError_t cudaConfigureCall(dim3 gridDim, dim3 blockDim, size_t sharedMemory, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFuncGetAttributes")]
            public static extern cudaError_t cudaFuncGetAttributes(out cudaFuncAttributes attr, string func);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFuncSetCacheConfig")]
            public static extern cudaError_t cudaFuncSetCacheConfig(IntPtr func, cudaFuncCache cacheConfig);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaLaunch")]
            public static extern cudaError_t cudaLaunch(string func);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDoubleForDevice")]
            public static extern cudaError_t cudaSetDoubleForDevice(ref double d);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetDoubleForHost")]
            public static extern cudaError_t cudaSetDoubleForHost(ref double d);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaSetupArgument")]
            public static extern cudaError_t cudaSetupArgument(IntPtr arg, size_t size, size_t offset);

            public cudaError_t ConfigureCall(dim3 gridDim, dim3 blockDim, size_t sharedMemory, cudaStream_t stream) { return cudaConfigureCall(gridDim, blockDim, sharedMemory, stream); }
            public cudaError_t FuncGetAttributes(out cudaFuncAttributes attr, string func) { return cudaFuncGetAttributes(out attr, func); }
            public cudaError_t FuncSetCacheConfig(IntPtr func, cudaFuncCache cacheConfig) { return cudaFuncSetCacheConfig(func, cacheConfig); }
            public cudaError_t Launch(string func) { return cudaLaunch(func); }
            public cudaError_t SetDoubleForDevice(ref double d) { return cudaSetDoubleForDevice(ref d); }
            public cudaError_t SetDoubleForHost(ref double d) { return cudaSetDoubleForHost(ref d); }
            public cudaError_t SetupArgument(IntPtr arg, size_t size, size_t offset) { return cudaSetupArgument(arg, size, offset); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFuncSetSharedMemConfig")]
            public static extern cudaError_t cudaFuncSetSharedMemConfig(IntPtr func, cudaSharedMemConfig config);

            public cudaError_t FuncSetSharedMemConfig(IntPtr func, cudaSharedMemConfig config) { return cudaFuncSetSharedMemConfig(func, config); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaLaunchKernel")]
            public static extern cudaError_t cudaLaunchKernel(IntPtr func, dim3 gridDim, dim3 blockDim, IntPtr args, size_t sharedMem, cudaStream_t stream);
            public cudaError_t LaunchKernel(IntPtr func, dim3 gridDim, dim3 blockDim, IntPtr args, size_t sharedMem, cudaStream_t stream) { return cudaLaunchKernel(func, gridDim, blockDim, args, sharedMem, stream); }

            #endregion

            #region Occupancy

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaOccupancyMaxActiveBlocksPerMultiprocessor ")]
            public static extern cudaError_t cudaOccupancyMaxActiveBlocksPerMultiprocessor(out int numBlocks, IntPtr function, int blockSize, size_t dynamicSharedSize);

            public cudaError_t OccupancyMaxActiveBlocksPerMultiprocessor(out int numBlocks, IntPtr function, int blockSize, size_t dynamicSharedSize)
            {
                return cudaOccupancyMaxActiveBlocksPerMultiprocessor(out numBlocks, function, blockSize, dynamicSharedSize);
            }

            #endregion

            #region Memory Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFree")]
            public static extern cudaError_t cudaFree(IntPtr dev);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeArray")]
            public static extern cudaError_t cudaFreeArray(cudaArray_t arr);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeHost")]
            public static extern cudaError_t cudaFreeHost(IntPtr ptr);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetSymbolAddress")]
            public static extern cudaError_t cudaGetSymbolAddress(out IntPtr devPtr, string symbol);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetSymbolSize")]
            public static extern cudaError_t cudaGetSymbolSize(out size_t size, string symbol);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostAlloc")]
            public static extern cudaError_t cudaHostAlloc(out IntPtr ptr, size_t size, cudaHostAllocFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostGetDevicePointer")]
            public static extern cudaError_t cudaHostGetDevicePointer(out IntPtr pdev, IntPtr phost, cudaGetDevicePointerFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostGetFlags")]
            public static extern cudaError_t cudaHostGetFlags(out cudaHostAllocFlags flags, IntPtr phost);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostRegister")]
            public static extern cudaError_t cudaHostRegister(IntPtr ptr, size_t size, uint flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaHostUnregister")]
            public static extern cudaError_t cudaHostUnregister(IntPtr ptr);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMalloc")]
            public static extern cudaError_t cudaMalloc(out IntPtr dev, size_t size);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMalloc3D")]
            public static extern cudaError_t cudaMalloc3D(ref cudaPitchedPtr ptr, cudaFuncAttributes extent);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMalloc3DArray")]
            public static extern cudaError_t cudaMalloc3DArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, cudaFuncAttributes extent, cudaMallocArrayFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocArray")]
            public static extern cudaError_t cudaMallocArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, size_t width, size_t height, cudaMallocArrayFlags flags);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocHost")]
            public static extern cudaError_t cudaMallocHost(out IntPtr ptr, size_t size);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocPitch")]
            public static extern cudaError_t cudaMallocPitch(out IntPtr dptr, out size_t pitch, size_t width, size_t height);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy")]
            public static extern cudaError_t cudaMemcpy(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2D")]
            public static extern cudaError_t cudaMemcpy2D(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DArrayToArray")]
            public static extern cudaError_t cudaMemcpy2DArrayToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DAsync")]
            public static extern cudaError_t cudaMemcpy2DAsync(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DFromArray")]
            public static extern cudaError_t cudaMemcpy2DFromArray(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DFromArrayAsync")]
            public static extern cudaError_t cudaMemcpy2DFromArrayAsync(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DToArray")]
            public static extern cudaError_t cudaMemcpy2DToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy2DToArrayAsync")]
            public static extern cudaError_t cudaMemcpy2DToArrayAsync(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3D")]
            public static extern cudaError_t cudaMemcpy3D(ref cudaMemcpy3DParms par);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3DAsync")]
            public static extern cudaError_t cudaMemcpy3DAsync(ref cudaMemcpy3DParms par, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyArrayToArray")]
            public static extern cudaError_t cudaMemcpyArrayToArray(cudaArray_t dest, size_t wOffsetDst, size_t hOffsetDst, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t count, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyAsync")]
            public static extern cudaError_t cudaMemcpyAsync(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromArray")]
            public static extern cudaError_t cudaMemcpyFromArray(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromArrayAsync")]
            public static extern cudaError_t cudaMemcpyFromArrayAsync(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromSymbol")]
            public static extern cudaError_t cudaMemcpyFromSymbol(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyFromSymbolAsync")]
            public static extern cudaError_t cudaMemcpyFromSymbolAsync(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToArray")]
            public static extern cudaError_t cudaMemcpyToArray(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToArrayAsync")]
            public static extern cudaError_t cudaMemcpyToArrayAsync(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToSymbol")]
            public static extern cudaError_t cudaMemcpyToSymbol(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyToSymbolAsync")]
            public static extern cudaError_t cudaMemcpyToSymbolAsync(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemGetInfo")]
            public static extern cudaError_t cudaMemGetInfo(out size_t free, out size_t total);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset")]
            public static extern cudaError_t cudaMemset(IntPtr devPtr, int value, size_t count);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset2D")]
            public static extern cudaError_t cudaMemset2D(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset3D")]
            public static extern cudaError_t cudaMemset3D(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent);

            public cudaError_t Free(IntPtr dev) { return cudaFree(dev); }
            public cudaError_t FreeArray(cudaArray_t arr) { return cudaFreeArray(arr); }
            public cudaError_t FreeHost(IntPtr ptr) { return cudaFreeHost(ptr); }
            public cudaError_t GetSymbolAddress(out IntPtr devPtr, string symbol) { return cudaGetSymbolAddress(out devPtr, symbol); }
            public cudaError_t GetSymbolSize(out size_t size, string symbol) { return cudaGetSymbolSize(out size, symbol); }
            public cudaError_t HostAlloc(out IntPtr ptr, size_t size, cudaHostAllocFlags flags) { return cudaHostAlloc(out ptr, size, flags); }
            public cudaError_t HostGetDevicePointer(out IntPtr pdev, IntPtr phost, cudaGetDevicePointerFlags flags) { return cudaHostGetDevicePointer(out pdev, phost, flags); }
            public cudaError_t HostGetFlags(out cudaHostAllocFlags flags, IntPtr phost) { return cudaHostGetFlags(out flags, phost); }
            public cudaError_t HostRegister(IntPtr ptr, size_t size, uint flags) { return cudaHostRegister(ptr, size, flags); }
            public cudaError_t HostUnregister(IntPtr ptr) { return cudaHostUnregister(ptr); }
            public cudaError_t Malloc(out IntPtr dev, size_t size) { return cudaMalloc(out dev, size); }
            public cudaError_t Malloc3D(ref cudaPitchedPtr ptr, cudaFuncAttributes extent) { return cudaMalloc3D(ref ptr, extent); }
            public cudaError_t Malloc3DArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, cudaFuncAttributes extent, cudaMallocArrayFlags flags) { return cudaMalloc3DArray(out arr, ref chan, extent, flags); }
            public cudaError_t MallocArray(out cudaArray_t arr, ref cudaChannelFormatDesc chan, size_t width, size_t height, cudaMallocArrayFlags flags) { return cudaMallocArray(out arr, ref chan, width, height, flags); }
            public cudaError_t MallocHost(out IntPtr ptr, size_t size) { return cudaMallocHost(out ptr, size); }
            public cudaError_t MallocPitch(out IntPtr dptr, out size_t pitch, size_t width, size_t height) { return cudaMallocPitch(out dptr, out pitch, width, height); }
            public cudaError_t Memcpy(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind) { return cudaMemcpy(dest, src, size, kind); }
            public cudaError_t Memcpy2D(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2D(dest, dpitch, src, spitch, width, height, kind); }
            public cudaError_t Memcpy2DArrayToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2DArrayToArray(dest, wOffsetDest, hOffsetDest, src, wOffsetSrc, hOffsetSrc, width, height, kind); }
            public cudaError_t Memcpy2DAsync(IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpy2DAsync(dest, dpitch, src, spitch, width, height, kind, stream); }
            public cudaError_t Memcpy2DFromArray(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2DFromArray(dest, dpitch, src, wOffset, hOffset, width, height, kind); }
            public cudaError_t Memcpy2DFromArrayAsync(IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpy2DFromArrayAsync(dest, dpitch, src, wOffset, hOffset, width, height, kind, stream); }
            public cudaError_t Memcpy2DToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return cudaMemcpy2DToArray(dest, wOffsetDest, hOffsetDest, src, spitch, width, height, kind); }
            public cudaError_t Memcpy2DToArrayAsync(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpy2DToArrayAsync(dest, wOffsetDest, hOffsetDest, src, spitch, width, height, kind, stream); }
            public cudaError_t Memcpy3D(ref cudaMemcpy3DParms par) { return cudaMemcpy3D(ref par); }
            public cudaError_t Memcpy3DAsync(ref cudaMemcpy3DParms par, cudaStream_t stream) { return cudaMemcpy3DAsync(ref par, stream); }
            public cudaError_t MemcpyArrayToArray(cudaArray_t dest, size_t wOffsetDst, size_t hOffsetDst, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t count, cudaMemcpyKind kind) { return cudaMemcpyArrayToArray(dest, wOffsetDst, hOffsetDst, src, wOffsetSrc, hOffsetSrc, count, kind); }
            public cudaError_t MemcpyAsync(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyAsync(dest, src, size, kind, stream); }
            public cudaError_t MemcpyFromArray(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind) { return cudaMemcpyFromArray(dest, src, wOffset, hOffset, count, kind); }
            public cudaError_t MemcpyFromArrayAsync(IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyFromArrayAsync(dest, src, wOffset, hOffset, count, kind, stream); }
            public cudaError_t MemcpyFromSymbol(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind) { return cudaMemcpyFromSymbol(dest, symbol, count, offset, kind); }
            public cudaError_t MemcpyFromSymbolAsync(IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyFromSymbolAsync(dest, symbol, count, offset, kind, stream); }
            public cudaError_t MemcpyToArray(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind) { return cudaMemcpyToArray(dest, wOffset, hOffset, src, count, kind); }
            public cudaError_t MemcpyToArrayAsync(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyToArrayAsync(dest, wOffset, hOffset, src, count, kind, stream); }
            public cudaError_t MemcpyToSymbol(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind) { return cudaMemcpyToSymbol(symbol, src, count, offset, kind); }
            public cudaError_t MemcpyToSymbolAsync(string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return cudaMemcpyToSymbolAsync(symbol, src, count, offset, kind, stream); }
            public cudaError_t MemGetInfo(out size_t free, out size_t total) { return cudaMemGetInfo(out free, out total); }
            public cudaError_t Memset(IntPtr devPtr, int value, size_t count) { return cudaMemset(devPtr, value, count); }
            public cudaError_t Memset2D(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height) { return cudaMemset2D(devPtr, pitch, value, width, height); }
            public cudaError_t Memset3D(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent) { return cudaMemset3D(devPtr, value, extent); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaArrayGetInfo")]
            static extern cudaError_t cudaArrayGetInfo(out cudaChannelFormatDesc desc, out cudaFuncAttributes extent, out uint flags, cudaArray_t array); // Gets info about the specified cudaArray. 
            public cudaError_t ArrayGetInfo(out cudaChannelFormatDesc desc, out cudaFuncAttributes extent, out uint flags, cudaArray_t array)
            { return cudaArrayGetInfo(out desc, out extent, out flags, array); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeMipmappedArray")]
            static extern cudaError_t cudaFreeMipmappedArray(cudaMipmappedArray_t mipmappedArray); // Frees a mipmapped array on the device. 
            public cudaError_t FreeMipmappedArray(cudaMipmappedArray_t mipmappedArray) { return cudaFreeMipmappedArray(mipmappedArray); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaFreeMipmappedArray")]
            static extern cudaError_t cudaGetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_t mipmappedArray, uint level); // Gets a mipmap level of a CUDA mipmapped array. 
            public cudaError_t GetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_t mipmappedArray, uint level) { return cudaGetMipmappedArrayLevel(out levelArray, mipmappedArray, level); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocManaged")]
            static extern cudaError_t cudaMallocManaged(out IntPtr devPtr, size_t size, uint flags); // Allocates memory that will be automatically managed by the Unified Memory system. 
            public cudaError_t MallocManaged(out IntPtr devPtr, size_t size, uint flags) { return cudaMallocManaged(out devPtr, size, flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocMipmappedArray")]
            static extern cudaError_t cudaMallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, int numLevels, uint flags); // Allocate a mipmapped array on the device. 
            public cudaError_t MallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, int numLevels, uint flags = 0)
            { return cudaMallocMipmappedArray(out mipmappedArray, ref desc, extent, numLevels, flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3DPeer")]
            static extern cudaError_t cudaMemcpy3DPeer(ref cudaMemcpy3DPeerParms p); // Copies memory between devices. 
            public cudaError_t Memcpy3DPeer(ref cudaMemcpy3DPeerParms p) { return cudaMemcpy3DPeer(ref p); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpy3DPeerAsync")]
            static extern cudaError_t cudaMemcpy3DPeerAsync(ref cudaMemcpy3DPeerParms p, cudaStream_t stream); // Copies memory between devices asynchronously. 
            public cudaError_t Memcpy3DPeerAsync(ref cudaMemcpy3DPeerParms p, cudaStream_t stream = new cudaStream_t()) { return cudaMemcpy3DPeerAsync(ref p, stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemsetAsync")]
            public static extern cudaError_t cudaMemsetAsync(IntPtr devPtr, int value, size_t count, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset2DAsync")]
            public static extern cudaError_t cudaMemset2DAsync(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height, cudaStream_t stream);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemset3DAsync")]
            public static extern cudaError_t cudaMemset3DAsync(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent, cudaStream_t stream);

            public cudaError_t MemsetAsync(IntPtr devPtr, int value, size_t count, cudaStream_t stream = new cudaStream_t()) { return cudaMemsetAsync(devPtr, value, count, stream); }
            public cudaError_t Memset2DAsync(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height, cudaStream_t stream = new cudaStream_t()) { return cudaMemset2DAsync(devPtr, pitch, value, width, height, stream); }
            public cudaError_t Memset3DAsync(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent, cudaStream_t stream = new cudaStream_t()) { return cudaMemset3DAsync(devPtr, value, extent, stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemPrefetchAsync")]
            public static extern cudaError_t cudaMemPrefetchAsync(IntPtr devptr, size_t count, int dstDevice, cudaStream_t stream);
            public cudaError_t MemPrefetchAsync(IntPtr devptr, size_t count, int dstDevice, cudaStream_t stream) { return cudaMemPrefetchAsync(devptr, count, dstDevice, stream); }
            public cudaError_t MemPrefetchAsync(IntPtr devptr, size_t count, int dstDevice) { return cudaMemPrefetchAsync(devptr, count, dstDevice, cudaStream_t.NO_STREAM); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyPeerAsync")]
            static extern cudaError_t cudaMemcpyPeerAsync(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count, cudaStream_t stream);
            public cudaError_t MemcpyPeerAsync(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count, cudaStream_t stream) { return cudaMemcpyPeerAsync(dest, dstDevice, src, srcDevice, count, stream); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemcpyPeer")]
            static extern cudaError_t cudaMemcpyPeer(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count);
            public cudaError_t MemcpyPeer(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count) { return cudaMemcpyPeer(dest, dstDevice, src, srcDevice, count); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMemAdvise")]
            static extern cudaError_t cudaMemAdvise(IntPtr devptr, size_t count, cudaMemmoryAdvise advice, int device);
            public cudaError_t MemAdvise(IntPtr devptr, size_t count, cudaMemmoryAdvise advice, int device) { return cudaMemAdvise(devptr, count, advice, device); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaMallocMipmappedArray")]
            static extern cudaError_t cudaMallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, uint numLevels, uint flags);
            public cudaError_t MallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, uint numLevels, uint flags = 0) { return cudaMallocMipmappedArray(out mipmappedArray, ref desc, extent, numLevels, flags); }

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetMipmappedArrayLevel")]
            static extern cudaError_t cudaGetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_const_t mipmappedArray, uint level);
            public cudaError_t GetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_const_t mipmappedArray, uint level) { return cudaGetMipmappedArrayLevel(out levelArray, mipmappedArray, level); }


            /*
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "__make_cudaExtent")]
            static extern 
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "__make_cudaExtent")]
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "__make_cudaExtent")]

            public cudaExtent make_cudaExtent(size_t w, size_t h, size_t d) { return __make_cudaExtent(w, h, d); }
            public cudaExtent make_cudaPitchedPtr(IntPtr d, size_t p, size_t xsz, size_t ysz) { return __make_cudaPitchedPtr(d, p, xsz, ysz); }
            public cudaExtent make_cudaPos(size_t x, size_t y, size_t z) { return __make_cudaPos(x, y, z); }
            */
            #endregion

            #region Surface Management

            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaCreateSurfaceObject")]
            public static extern cudaError_t cudaCreateSurfaceObject(out cudaSurfaceObject_t surface, ref cudaResourceDesc resDesc);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDestroySurfaceObject")]
            public static extern cudaError_t cudaDestroySurfaceObject(cudaSurfaceObject_t surface);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetSurfaceObjectResourceDesc")]
            public static extern cudaError_t cudaGetSurfaceObjectResourceDesc(out cudaResourceDesc resDesc, cudaSurfaceObject_t surface);

            public cudaError_t CreateSurfaceObject(out cudaSurfaceObject_t surface, ref cudaResourceDesc resDesc) { return cudaCreateSurfaceObject(out surface, ref resDesc); }
            public cudaError_t DestroySurfaceObject(cudaSurfaceObject_t surface) { return cudaDestroySurfaceObject(surface); }
            public cudaError_t GetSurfaceObjectResourceDesc(out cudaResourceDesc resDesc, cudaSurfaceObject_t surface) { return cudaGetSurfaceObjectResourceDesc(out resDesc, surface); }

            #endregion

            #region Texture Management


            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaCreateTextureObject")]
            public static extern cudaError_t cudaCreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc, ref cudaResourceViewDesc ResViewDesc);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaCreateTextureObject")]
            public static extern cudaError_t cudaCreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc, IntPtr ResViewDesc);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaDestroyTextureObject")]
            public static extern cudaError_t cudaDestroyTextureObject(cudaTextureObject_t texture);
            [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cudaGetTextureObjectResourceDesc")]
            public static extern cudaError_t cudaGetTextureObjectResourceDesc(out cudaResourceDesc resDesc, cudaTextureObject_t texture);

            public cudaError_t CreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc, ref cudaResourceViewDesc ResViewDesc) { return cudaCreateTextureObject(out texture, ref resDesc, ref texDesc, ref ResViewDesc); }
            public cudaError_t CreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc) { return cudaCreateTextureObject(out texture, ref resDesc, ref texDesc, IntPtr.Zero); }
            public cudaError_t DestroyTextureObject(cudaTextureObject_t texture) { return cudaDestroyTextureObject(texture); }
            public cudaError_t GetTextureObjectResourceDesc(out cudaResourceDesc resDesc, cudaTextureObject_t texture) { return cudaGetTextureObjectResourceDesc(out resDesc, texture); }

            #endregion

            #region Driver Interop Features

            public static class Interop
            {
                public struct CUmodule
                {
#pragma warning disable 0169
                    IntPtr mod;
#pragma warning restore 0169
                }

                public struct CUfunction
                {
#pragma warning disable 0169
                    IntPtr mod;
#pragma warning restore 0169
                }

                [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cuModuleLoadData")]
                public static extern cudaError_t cuModuleLoadData(out CUmodule module, IntPtr target);

                [DllImport(CUDARTDLL, CharSet = CharSet.Ansi, EntryPoint = "cuModuleGetFunction")]
                public static extern cudaError_t cuModuleGetFunction(out CUfunction function, CUmodule module, string name);
            }

            #endregion
            #region OpenGL interop


            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGLRegisterBufferObject(uint buffer);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsGLRegisterBuffer(out IntPtr pCudaResource, uint buffer, uint Flags);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsUnregisterResource(IntPtr resource);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGLUnregisterBufferObject(uint buffer);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsResourceGetMappedPointer(out IntPtr devPtr, out size_t size, IntPtr resource);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsResourceSetMapFlags(IntPtr resource, uint flags);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsMapResources(int count, IntPtr[] resources, cudaStream_t stream);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsUnmapResources(int count, IntPtr[] resources, cudaStream_t stream);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsGLRegisterImage(out IntPtr cudaGraphicsResource, uint image, uint target, uint flags);

            [DllImport(CUDARTDLL, CallingConvention = CallingConvention.Cdecl)]
            public static extern cudaError_t cudaGraphicsSubResourceGetMappedArray(out cudaArray_t array, IntPtr resource, uint arrayIndex, uint mipLevel);

            public cudaError_t GraphicsGLRegisterImage(out IntPtr cudaGraphicsResource, uint image, uint target, uint flags)
            {
                return cudaGraphicsGLRegisterImage(out cudaGraphicsResource, image, target, flags);
            }

            public cudaError_t GraphicsSubResourceGetMappedArray(out cudaArray_t array, IntPtr resource, uint arrayIndex, uint mipLevel)
            {
                return cudaGraphicsSubResourceGetMappedArray(out array, resource, arrayIndex, mipLevel);
            }

            public cudaError_t GLRegisterBufferObject(uint buffer)
            {
                return cudaGLRegisterBufferObject(buffer);
            }

            public cudaError_t GraphicsGLRegisterBuffer(out IntPtr pCudaResource, uint buffer, uint Flags)
            {
                return cudaGraphicsGLRegisterBuffer(out pCudaResource, buffer, Flags);
            }

            public cudaError_t GraphicsUnregisterResource(IntPtr resource)
            {
                return cudaGraphicsUnregisterResource(resource);
            }

            public cudaError_t GLUnregisterBufferObject(uint buffer)
            {
                return cudaGLUnregisterBufferObject(buffer);
            }

            public cudaError_t GraphicsResourceGetMappedPointer(out IntPtr devPtr, out size_t size, IntPtr resource)
            {
                return cudaGraphicsResourceGetMappedPointer(out devPtr, out size, resource);
            }

            public cudaError_t GraphicsResourceSetMapFlags(IntPtr resource, uint flags)
            {
                return cudaGraphicsResourceSetMapFlags(resource, flags);
            }

            public cudaError_t GraphicsMapResources(int count, IntPtr[] resources, cudaStream_t stream)
            {
                return cudaGraphicsMapResources(count, resources, stream);
            }

            public cudaError_t GraphicsUnmapResources(int count, IntPtr[] resources, cudaStream_t stream)
            {
                return cudaGraphicsUnmapResources(count, resources, stream);
            }
            #endregion

        }
    
    
    }
}
