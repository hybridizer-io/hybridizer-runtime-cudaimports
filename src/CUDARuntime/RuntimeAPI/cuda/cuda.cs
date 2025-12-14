/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using static Hybridizer.Runtime.CUDAImports.CudaImplem;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// CUDA runtime API wrapper
    /// </summary>
    [Guid("553B61F4-8D61-4570-8791-B666857F987A")]
    public unsafe class cuda
    {
        internal static T StructConvert<T>(object o) where T : new()
        {
            Type t = o.GetType();
            object res = new T();
            var tf = t.GetFields();
            foreach (FieldInfo fi in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public))
            {
                if (t.GetField(fi.Name, BindingFlags.Instance | BindingFlags.Public) != null)
                    fi.SetValue(res, t.GetField(fi.Name).GetValue(o));
            }
            return (T)res;
        }

        /// <summary>
        /// Device id that represents the CPU
        /// </summary>
        [IntrinsicConstant("cudaCpuDeviceId")]
        public const int CpuDeviceId = -1;

        /// <summary>
        /// Device id that represents an invalid device
        /// </summary>
        [IntrinsicConstant("cudaInvalidDeviceId")]
        public const int InvalidDeviceId = -2;

        /// <summary>
        /// verbosity
        /// </summary>
        public enum VERBOSITY
        {
            /// <summary>
            /// none
            /// </summary>
            None,
            /// <summary>
            /// verbose
            /// </summary>
            Verbose
        }

        /// <summary>
        /// log verbosity
        /// </summary>
        public static VERBOSITY s_VERBOSITY = VERBOSITY.None;

        private static readonly Dictionary<string, string[]> CUDA_DLLS = new Dictionary<string, string[]>
        {
            { "110", new[] { RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Cuda_64_110_linux.CUDARTDLL : Cuda_64_110_windows.CUDARTDLL } },
            { "114", new[] { RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Cuda_64_114_linux.CUDARTDLL : Cuda_64_114_windows.CUDARTDLL } },
            { "120", new[] { RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Cuda_64_120_linux.CUDARTDLL : Cuda_64_120_windows.CUDARTDLL } },
            { "124", new[] { RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Cuda_64_124_linux.CUDARTDLL : Cuda_64_124_windows.CUDARTDLL } },
            { "126", new[] { RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Cuda_64_126_linux.CUDARTDLL : Cuda_64_126_windows.CUDARTDLL } },
            { "130", new[] { RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Cuda_64_130_linux.CUDARTDLL : Cuda_64_130_windows.CUDARTDLL } },
            { "131", new[] { RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? Cuda_64_131_linux.CUDARTDLL : Cuda_64_131_windows.CUDARTDLL } }
        };

        /// <summary>
        /// current instance
        /// </summary>
        public static ICuda instance { get; set; }

        /// <summary>
        /// returns the cuda runtime version
        /// if cudaimports is built with /p:DefineConstants="HYBRIDIZER_CUDA_VERSION_xx", we take that value first
        /// then we try to find it in application settings with key "hybridizer.cudaruntimeversion"
        /// finally from environment "HYBRIDIZER_CUDA_VERSION"
        /// if all fails, we default on "80"
        /// </summary>
        /// <returns></returns>
        public static string GetCudaVersion()
        {
            // Is any known cuda DLL already loaded
            foreach (var k in CUDA_DLLS.Keys)
                foreach (var dllName in CUDA_DLLS[k])
                    foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
                        if (module.ModuleName == dllName) 
                            return k;
            
            string cudaVersion = String.Empty;
#if HYBRIDIZER_CUDA_VERSION_131
            cudaVersion = "131";
#elif HYBRIDIZER_CUDA_VERSION_130
            cudaVersion = "130";
#elif HYBRIDIZER_CUDA_VERSION_126
            cudaVersion = "126";
#elif HYBRIDIZER_CUDA_VERSION_124
            cudaVersion = "124";
#elif HYBRIDIZER_CUDA_VERSION_114
            cudaVersion = "114";
#elif HYBRIDIZER_CUDA_VERSION_110
            cudaVersion = "110";
#endif
            try
            {
                // if (String.IsNullOrWhiteSpace(cudaVersion))
                // {
                //     cudaVersion = ConfigurationManager.AppSettings["hybridizer.cudaruntimeversion"];
                // }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Cannot read cuda version from application config file - Exception: {0}", e);
            }
            if (String.IsNullOrWhiteSpace(cudaVersion))
            {
                try
                {
                    cudaVersion = Environment.GetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION");
                }
                catch(Exception e)
                {
                    Console.Error.WriteLine("Cannot read cuda version from environment variable - Exception: {0}", e);
                }
            }

            // Otherwise default to latest version
            if (String.IsNullOrWhiteSpace(cudaVersion)) 
                cudaVersion = "131";
            cudaVersion = cudaVersion.Replace(".", ""); // Remove all dots ("7.5" will be understood "75")
            
            if (s_VERBOSITY != VERBOSITY.None)
            {
                Console.WriteLine("Using CUDA {0}", cudaVersion);
            }
            return cudaVersion;
        }

        static cuda()
        {
            // read verbosity from file
            string cudaVersion = GetCudaVersion();
            
            if (IntPtr.Size != 8)
                throw new NotSupportedException("cuda dropped 32 bits support since version 11");
            switch (cudaVersion)
            {
                case "110":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new Cuda_64_110_linux() : new Cuda_64_110_windows();
                    break;
                case "114":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new Cuda_64_114_linux() : new Cuda_64_114_windows();
                    break;
                case "120":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new Cuda_64_120_linux() : new Cuda_64_120_windows();
                    break;
                case "124":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new Cuda_64_124_linux() : new Cuda_64_124_windows();
                    break;
                case "126":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new Cuda_64_126_linux() : new Cuda_64_126_windows();
                    break;
                case "130":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new Cuda_64_130_linux() : new Cuda_64_130_windows();
                    break;
                case "131":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new Cuda_64_131_linux() : new Cuda_64_131_windows();
                    break;
                default:
                    throw new ApplicationException(string.Format("Unsupported version of Cuda {0}", cudaVersion));
            }
        }

#region Device Management
        /// <summary>
        /// Set a list of devices that can be used for CUDA
        /// </summary>
        /// <param name="devs"></param>
        /// <returns></returns>
        public static cudaError_t SetValidDevices(int[] devs) { return instance.SetValidDevices(devs); }
        /// <summary>
        /// Select compute-device which best matches criteria. Select compute-device which best matches criteria. 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static cudaError_t ChooseDevice(out int device, ref cudaDeviceProp prop) { return instance.ChooseDevice(out device, ref prop); }
        /// <summary>
        /// Returns information about the device. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="attr"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        [IntrinsicFunction("cudaDeviceGetAttribute")]
        public static cudaError_t DeviceGetAttribute(out int value, cudaDeviceAttr attr, int device) { return instance.DeviceGetAttribute(out value, attr, device); } 
        /// <summary>
        /// Returns a handle to a compute device. 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="pciBusId"></param>
        /// <returns></returns>
        public static cudaError_t DeviceGetByPCIBusId(out int device, string pciBusId) { return instance.DeviceGetByPCIBusId (out device, pciBusId); } 
        /// <summary>
        /// Returns the preferred cache configuration for the current device. 
        /// </summary>
        /// <param name="pCacheConfig"></param>
        /// <returns></returns>
        [IntrinsicFunction("cudaDeviceGetCacheConfig")]
        public static cudaError_t DeviceGetCacheConfig(IntPtr /* cudaFuncCache ** */ pCacheConfig) { return instance.DeviceGetCacheConfig(pCacheConfig); } 
        /// <summary>
        /// Returns resource limits. 
        /// </summary>
        /// <param name="pValue"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [IntrinsicFunction("cudaDeviceGetLimit")]
        public static cudaError_t DeviceGetLimit(out size_t pValue, cudaLimit limit) { return instance.DeviceGetLimit(out pValue, limit); }
        /// <summary>
        /// Returns a PCI Bus Id string for the device. 
        /// </summary>
        /// <param name="pciBusId"></param>
        /// <param name="len"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static cudaError_t DeviceGetPCIBusId(StringBuilder pciBusId, int len, int device) { return instance.DeviceGetPCIBusId(pciBusId, len, device); }
        /// <summary>
        /// Returns the shared memory configuration for the current device. 
        /// </summary>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        [IntrinsicFunction("cudaDeviceGetSharedMemConfig")]
        public static cudaError_t DeviceGetSharedMemConfig(IntPtr /* cudaSharedMemConfig** */ pConfig) { return instance.DeviceGetSharedMemConfig(pConfig); }
        /// <summary>
        /// Destroy all allocations and reset all state on the current device in the current process. 
        /// </summary>
        public static cudaError_t DeviceReset() { return instance.DeviceReset(); }
        /// <summary>
        /// Sets the preferred cache configuration for the current device. 
        /// </summary>
        public static cudaError_t DeviceSetCacheConfig(cudaFuncCache cacheConfig) { return instance.DeviceSetCacheConfig(cacheConfig); }
        /// <summary>
        /// Set resource limits.
        /// </summary>
        public static cudaError_t DeviceSetLimit(cudaLimit limit, size_t value) { return instance.DeviceSetLimit(limit, value); }
        /// <summary>
        /// Sets the shared memory configuration for the current device. 
        /// </summary>
        public static cudaError_t DeviceSetSharedMemConfig(cudaSharedMemConfig config) { return instance.DeviceSetSharedMemConfig(config); }
        /// <summary>
        /// Wait for compute device to finish. 
        /// </summary>
        [IntrinsicFunction("cudaDeviceSynchronize")]
        public static cudaError_t DeviceSynchronize() { return instance.DeviceSynchronize(); }
        /// <summary>
        /// Returns which device is currently being used. 
        /// </summary>
        [IntrinsicFunction("cudaGetDevice")]
        public static cudaError_t GetDevice(out int device) { return instance.GetDevice(out device); }
        /// <summary>
        /// Returns the number of compute-capable devices.
        /// </summary>
        [IntrinsicFunction("cudaGetDeviceCount")]
        public static cudaError_t GetDeviceCount(out int count) { return instance.GetDeviceCount(out count); }
        /// <summary>
        /// Returns information about the compute-device. 
        /// </summary>
        public static cudaError_t GetDeviceProperties(out cudaDeviceProp prop, int device) { return instance.GetDeviceProperties(out prop, device); }
        /// <summary>
        /// Close memory mapped with <see cref="IpcOpenMemHandle"></see>
        /// </summary>
        public static cudaError_t IpcCloseMemHandle(IntPtr devPtr) { return instance.IpcCloseMemHandle(devPtr); }
        /// <summary>
        /// Gets an interprocess handle for a previously allocated event. 
        /// </summary>
        public static cudaError_t IpcGetEventHandle(out cudaIpcEventHandle_t handle, cudaEvent_t evt) { return instance.IpcGetEventHandle(out handle, evt); }
        /// <summary>
        /// Gets an interprocess memory handle for an existing device memory allocation
        /// </summary>
        public static cudaError_t IpcGetMemHandle(out cudaIpcMemHandle_t handle, IntPtr devPtr) { return instance.IpcGetMemHandle(out handle, devPtr); }
        /// <summary>
        /// Opens an interprocess event handle for use in the current process. 
        /// </summary>
        public static cudaError_t IpcOpenEventHandle(out cudaEvent_t evt, cudaIpcEventHandle_t handle) { return instance.IpcOpenEventHandle(out evt, handle); }
        /// <summary>
        /// Opens an interprocess memory handle exported from another process and returns a device pointer usable in the local process.
        /// </summary>
        public static cudaError_t IpcOpenMemHandle(out IntPtr devPtr, cudaIpcMemHandle_t handle, uint flags) { return instance.IpcOpenMemHandle(out devPtr, handle, flags); }
        /// <summary>
        /// Set device to be used for GPU executions. 
        /// </summary>
        public static cudaError_t SetDevice(int dev) { return instance.SetDevice(dev); }
        /// <summary>
        /// Sets flags to be used for device executions. 
        /// </summary>
        public static cudaError_t SetDeviceFlags(deviceFlags flags) { return instance.SetDeviceFlags(flags); }

        /// <summary>
        /// Queries attributes of the link between two devices.
        /// </summary>
        /// <param name="value">Returned value of the requested attribute </param>
        /// <param name="attr"></param>
        /// <param name="srcDevice">The source device of the target link. </param>
        /// <param name="dstDevice">The destination device of the target link.</param>
        /// <returns></returns>
        public static cudaError_t DeviceGetP2PAttribute(out int value, cudaDeviceP2PAttr attr, int srcDevice, int dstDevice) { return instance.DeviceGetP2PAttribute(out value, attr, srcDevice, dstDevice); }

        /// <summary>
        /// Returns numerical values that correspond to the least and greatest stream priorities. 
        /// 
        /// Returns in *leastPriority and *greatestPriority the numerical values that correspond to the least and 
        /// greatest stream priorities respectively. Stream priorities follow a convention where lower numbers imply 
        /// greater priorities. The range of meaningful stream priorities is given by [*greatestPriority, *leastPriority].
        /// If the user attempts to create a stream with a priority value that is outside the the meaningful range as
        /// specified by this API, the priority is automatically clamped down or up to either *leastPriority or
        /// *greatestPriority respectively. See cudaStreamCreateWithPriority for details on creating a priority stream.
        /// A NULL may be passed in for *leastPriority or *greatestPriority if the value is not desired. 
        /// 
        /// This function will return '0' in both *leastPriority and *greatestPriority if the current context's device 
        /// does not support stream priorities (see cudaDeviceGetAttribute). 
        /// </summary>
        /// <param name="leastPriority">Pointer to an int in which the numerical value for least stream priority is returned </param>
        /// <param name="greatestPriority">Pointer to an int in which the numerical value for greatest stream priority is returned</param>
        /// 
        /// <returns></returns>
        public static cudaError_t DeviceGetStreamPriorityRange(out int leastPriority, out int greatestPriority) { return instance.DeviceGetStreamPriorityRange(out leastPriority, out greatestPriority); }

        /// <summary>
        /// Gets the flags for the current device. 
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static cudaError_t GetDeviceFlags(out uint flags) { return instance.GetDeviceFlags(out flags); }

#endregion

#region ErrorHandling

        /// <summary>
        /// Returns the string representation of an error code enum name
        /// </summary>
        [IntrinsicFunction("cudaGetErrorName")]
        public static string GetErrorName(cudaError_t err) { return instance.GetErrorName(err); }
        /// <summary>
        /// Returns the description string for an error code
        /// </summary>
        [IntrinsicFunction("cudaGetErrorString")]
        public static string GetErrorString(cudaError_t err) { return instance.GetErrorString(err); }
        /// <summary>
        /// Returns the last error from a runtime call
        /// </summary>
        [IntrinsicFunction("cudaGetLastError")]
        public static cudaError_t GetLastError() { return instance.GetLastError(); }
        /// <summary>
        /// Returns the last error from a runtime call
        /// </summary>
        [IntrinsicFunction("cudaGetPeekAtLastError")]
        public static cudaError_t GetPeekAtLastError() { return instance.GetPeekAtLastError(); }

#endregion

#region Thread Management

        /// <summary>
        /// Exit and clean up from CUDA launches
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static cudaError_t ThreadExit() { return instance.ThreadExit(); }
        /// <summary>
        /// Returns resource limits
        /// </summary>
        [Obsolete]
        public static cudaError_t ThreadGetLimit(out size_t value, cudaLimit limit) { return instance.ThreadGetLimit(out value, limit); }
        /// <summary>
        /// Set resource limits
        /// </summary>
        [Obsolete]
        public static cudaError_t ThreadSetLimit(cudaLimit limit, size_t value) { return instance.ThreadSetLimit(limit, value); }
        /// <summary>
        /// Wait for compute device to finish
        /// </summary>
        [Obsolete]
        public static cudaError_t ThreadSynchronize() { return instance.ThreadSynchronize(); }

#endregion

#region Stream Management

        /// <summary>
        /// Create an asynchronous stream
        /// </summary>
        /// <remarks> <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__STREAM.html#group__CUDART__STREAM"/></remarks>
        /// <param name="stream">Pointer to new stream identifier</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        public static cudaError_t StreamCreate(out cudaStream_t stream) { return instance.StreamCreate(out stream); }
        /// <summary>
        /// Destroy cuda steam
        /// </summary>
        /// <remarks> <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__STREAM.html#group__CUDART__STREAM"/></remarks>
        /// <param name="stream">Stream identifier</param>
        /// <returns>cudaSuccess, cudaErrorInvalidResourceHandle</returns>
        [IntrinsicFunction("cudaStreamDestroy")]
        public static cudaError_t StreamDestroy(cudaStream_t stream) { return instance.StreamDestroy(stream); }
        /// <summary>
        /// Queries an asynchronous stream for completion status
        /// </summary>
        public static cudaError_t StreamQuery(cudaStream_t stream) { return instance.StreamQuery(stream); }
        /// <summary>
        /// Synchronize cuda steam
        /// </summary>
        /// <remarks> <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__STREAM.html#group__CUDART__STREAM"/></remarks>
        /// <param name="stream">Stream identifier</param>
        /// <returns>cudaSuccess, cudaErrorInvalidResourceHandle</returns>
        public static cudaError_t StreamSynchronize(cudaStream_t stream) { return instance.StreamSynchronize(stream); }
        /// <summary>
        /// Attach memory to a stream asynchronously
        /// </summary>
        public static cudaError_t StreamAttachMemAsync(cudaStream_t stream, IntPtr devPtr, size_t length, uint flags) { return instance.StreamAttachMemAsync(stream, devPtr, length, flags); }
        /// <summary>
        /// Create an asynchronous stream
        /// </summary>
        [IntrinsicFunction("cudaStreamCreateWithFlags")]
        public static cudaError_t StreamCreateWithFlags(out cudaStream_t pStream, uint flags) { return instance.StreamCreateWithFlags(out pStream, flags); }
        /// <summary>
        /// Create an asynchronous stream with the specified priority
        /// </summary>
        public static cudaError_t StreamCreateWithPriority(out cudaStream_t pStream, uint flags, int priority) { return instance.StreamCreateWithPriority(out pStream, flags, priority); }
        /// <summary>
        /// Query the flags of a stream
        /// </summary>
        public static cudaError_t StreamGetFlags(cudaStream_t hStream, out uint flags) { return instance.StreamGetFlags(hStream, out flags); }
        /// <summary>
        /// Make a compute stream wait on an event
        /// </summary>
        [IntrinsicFunction("cudaStreamWaitEvent")]
        public static cudaError_t StreamWaitEvent(cudaStream_t stream, cudaEvent_t evt, uint flags) { return instance.StreamWaitEvent(stream, evt, flags); }
        /// <summary>
        /// Query the priority of a stream
        /// </summary>
        public static cudaError_t StreamGetPriority(cudaStream_t hStream, out int priority) { return instance.StreamGetPriority(hStream, out priority); }

#endregion

#region Event Management

        /// <summary>
        /// Creates an event object
        /// </summary>
        public static cudaError_t EventCreate(out cudaEvent_t evt) { return instance.EventCreate(out evt); }
        /// <summary>
        /// Creates an event object with the specified flags
        /// </summary>
        [IntrinsicFunction("cudaCreateWithFlags")]
        public static cudaError_t EventCreateWithFlags(out cudaEvent_t evt, cudaEventFlags flags) { return instance.EventCreateWithFlags(out evt, flags); }
        /// <summary>
        /// Destroys an event object
        /// </summary>
        [IntrinsicFunction("cudaEventDestroy")]
        public static cudaError_t EventDestroy(cudaEvent_t evt) { return instance.EventDestroy(evt); }
        /// <summary>
        /// Computes the elapsed time between events
        /// </summary>
        public static cudaError_t EventElapsedTime(out float ms, cudaEvent_t start, cudaEvent_t stop) { return instance.EventElapsedTime(out ms, start, stop); }
        /// <summary>
        /// Queries an event's status
        /// </summary>
        public static cudaError_t EventQuery(cudaEvent_t evt) { return instance.EventQuery(evt); }
        /// <summary>
        /// Records an event
        /// </summary>
        [IntrinsicFunction("cudaEventRecord")]
        public static cudaError_t EventRecord(cudaEvent_t evt, cudaStream_t stream) { return instance.EventRecord(evt, stream); }
        /// <summary>
        /// Waits for an event to complete
        /// </summary>
        [IntrinsicFunction("cudaEventSynchronize")]
        public static cudaError_t EventSynchronize(cudaEvent_t evt) { return instance.EventSynchronize(evt); }

#endregion

#region Execution Control
        /// <summary>
        /// Configure a device-launch
        /// </summary>
        [Obsolete("Deprecated method", false)]
        public static cudaError_t ConfigureCall(dim3 gridDim, dim3 blockDim, size_t sharedMemory, cudaStream_t stream) { return instance.ConfigureCall(gridDim, blockDim, sharedMemory, stream); }
        /// <summary>
        /// Launches a device function. 
        /// </summary>
        [Obsolete("Deprecated method", false)]
        public static cudaError_t Launch(string func) { return instance.Launch(func); }
        /// <summary>
        /// Configure a device launch. 
        /// </summary>
        [Obsolete("Deprecated method", false)]
        public static cudaError_t SetupArgument(IntPtr arg, size_t size, size_t offset) { return instance.SetupArgument(arg, size, offset); }

        /// <summary>
        /// Find out attributes for a given function. 
        /// </summary>
        [IntrinsicFunction("cudaFuncGetAttributes")]
        public static cudaError_t FuncGetAttributes(out cudaFuncAttributes attr, string func) { return instance.FuncGetAttributes(out attr, func); }

        /// <summary>
        /// Sets the preferred cache configuration for a device function. 
        /// </summary>
        public static cudaError_t FuncSetCacheConfig(IntPtr func, cudaFuncCache cacheConfig) { return instance.FuncSetCacheConfig(func, cacheConfig); }
        /// <summary>
        /// Sets the shared memory configuration for a device function. 
        /// </summary>
        public static cudaError_t FuncSetSharedMemConfig(IntPtr func, cudaSharedMemConfig config) { return instance.FuncSetSharedMemConfig(func, config); }
        /// <summary>
        /// Converts a double argument after execution on a device. 
        /// </summary>
        [Obsolete]
        public static cudaError_t SetDoubleForDevice(ref double d) { return instance.SetDoubleForDevice(ref d); }
        /// <summary>
        /// Configure a device launch. 
        /// </summary>
        [Obsolete]
        public static cudaError_t SetDoubleForHost(ref double d) { return instance.SetDoubleForHost(ref d); }
        /// <summary>
        /// Launches a device function. 
        /// </summary>
        public static cudaError_t LaunchKernel(IntPtr func, dim3 gridDim, dim3 blockDim, IntPtr args, size_t sharedMem, cudaStream_t stream) { return instance.LaunchKernel(func, gridDim, blockDim, args, sharedMem, stream); }

#endregion

#region Memory Management

        /// <summary>
        /// Gets info about the specified cudaArray
        /// </summary>
        /// <param name="desc">Returned array type </param>
        /// <param name="extent">Returned array shape. 2D arrays will have depth of zero </param>
        /// <param name="flags">Returned array flags </param>
        /// <param name="array">The cudaArray to get info for</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        [IntrinsicFunction("cudaArrayGetInfo")]
        public static cudaError_t ArrayGetInfo(out cudaChannelFormatDesc desc, out cudaFuncAttributes extent, out uint flags, cudaArray_t array) { return instance.ArrayGetInfo(out desc, out extent, out flags, array); }
        /// <summary>
        /// Frees memory on the device
        /// </summary>
        [IntrinsicFunction("cudaFree")]
        public static cudaError_t Free (IntPtr dev) { return instance.Free (dev) ; }
        /// <summary>
        /// Frees memory from device
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        [IntrinsicFunction("cudaFree")]
        public static cudaError_t Free(void* dev) { return instance.Free(new IntPtr(dev)); }
        /// <summary>
        /// Frees an array on the device
        /// </summary>
        public static cudaError_t FreeArray (cudaArray_t arr) { return instance.FreeArray (arr) ; }
        /// <summary>
        ///  Frees page-locked memory
        /// </summary>
        public static cudaError_t FreeHost (IntPtr ptr) { return instance.FreeHost (ptr) ; }
        /// <summary>
        /// Frees page-locked memory
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static cudaError_t FreeHost (void* ptr) { return instance.FreeHost(new IntPtr(ptr)); }
        /// <summary>
        /// Frees a mipmapped array on the device
        /// </summary>
        public static cudaError_t FreeMipmappedArray(cudaMipmappedArray_t mipmappedArray) { return instance.FreeMipmappedArray(mipmappedArray); }
        /// <summary>
        /// Gets a mipmap level of a CUDA mipmapped array
        /// </summary>
        public static cudaError_t GetMipmappedArrayLevel(out cudaArray_t levelArray, cudaMipmappedArray_const_t mipmappedArray, uint level) { return instance.GetMipmappedArrayLevel(out levelArray, mipmappedArray, level); }
        /// <summary>
        /// Finds the address associated with a CUDA symbol
        /// </summary>
        public static cudaError_t GetSymbolAddress(out IntPtr devPtr, string symbol) { return instance.GetSymbolAddress(out devPtr, symbol); }
        /// <summary>
        /// Finds the address associated with a CUDA symbol
        /// </summary>
        public static cudaError_t GetSymbolAddress(out void* devPtr, string symbol) { IntPtr res; cudaError_t cuer = instance.GetSymbolAddress(out res, symbol); devPtr = res.ToPointer(); return cuer; }
        /// <summary>
        /// Finds the size of the object associated with a CUDA symbol
        /// </summary>
        public static cudaError_t GetSymbolSize (out size_t size, string symbol) { return instance.GetSymbolSize (out size, symbol) ; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t HostAlloc (out IntPtr ptr, size_t size, cudaHostAllocFlags flags) { return instance.HostAlloc (out ptr, size, flags) ; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t HostAlloc (out void* ptr, size_t size, cudaHostAllocFlags flags) { IntPtr res; cudaError_t cuer = instance.HostAlloc(out res, size, flags); ptr = res.ToPointer(); return cuer; }
        /// <summary>
        /// asses back device pointer of mapped host memory allocated by cudaHostAlloc or registered by cudaHostRegister
        /// </summary>
        public static cudaError_t HostGetDevicePointer (out IntPtr pdev, IntPtr phost, cudaGetDevicePointerFlags flags) { return instance.HostGetDevicePointer (out pdev, phost, flags) ; }
        /// <summary>
        /// asses back device pointer of mapped host memory allocated by cudaHostAlloc or registered by cudaHostRegister
        /// </summary>
        public static cudaError_t HostGetDevicePointer(out void* pdev, IntPtr phost, cudaGetDevicePointerFlags flags) { IntPtr res; cudaError_t cuer = instance.HostGetDevicePointer(out res, phost, flags); pdev = res.ToPointer(); return cuer; }
        /// <summary>
        /// Passes back flags used to allocate pinned host memory allocated by cudaHostAlloc
        /// </summary>
        public static cudaError_t HostGetFlags (out cudaHostAllocFlags flags, IntPtr phost) { return instance.HostGetFlags (out flags, phost) ; }
        /// <summary>
        /// Passes back flags used to allocate pinned host memory allocated by cudaHostAlloc
        /// </summary>
        public static cudaError_t HostGetFlags (out cudaHostAllocFlags flags, void* phost) { return instance.HostGetFlags(out flags, new IntPtr(phost)); }
        /// <summary>
        /// Registers an existing host memory range for use by CUDA
        /// </summary>
        public static cudaError_t HostRegister(IntPtr ptr, size_t size, uint flags) { return instance.HostRegister(ptr, size, flags); }
        /// <summary>
        /// Registers an existing host memory range for use by CUDA
        /// </summary>
        public static cudaError_t HostRegister(void* ptr, size_t size, uint flags) { return instance.HostRegister(new IntPtr(ptr), size, flags); }
        /// <summary>
        /// Unregisters a memory range that was registered with cudaHostRegister
        /// </summary>
        public static cudaError_t HostUnregister(IntPtr ptr) { return instance.HostUnregister(ptr); }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out IntPtr dev, size_t size) { return instance.Malloc(out dev, size); }

        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out void* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = res.ToPointer(); return cuer; }

        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out int* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (int*)res.ToPointer(); return cuer; }
        
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out float* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (float*)res.ToPointer(); return cuer; }
        
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out double* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (double*)res.ToPointer(); return cuer; }

        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out uint* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (uint*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out long* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (long*)res.ToPointer(); return cuer; }

        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out ulong* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (ulong*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out short* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (short*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out ushort* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (ushort*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out byte* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (byte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        [IntrinsicFunction("cudaMalloc")]
        public static cudaError_t Malloc(out sbyte* dev, size_t size) { IntPtr res; cudaError_t cuer = instance.Malloc(out res, size); dev = (sbyte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        public static cudaError_t Malloc3D (ref cudaPitchedPtr ptr, cudaFuncAttributes extent) { return instance.Malloc3D (ref ptr, extent) ; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        public static cudaError_t Malloc3DArray (out cudaArray_t arr, ref cudaChannelFormatDesc chan, cudaFuncAttributes extent, cudaMallocArrayFlags flags) { return instance.Malloc3DArray (out arr, ref chan, extent, flags) ; }
        /// <summary>
        /// Allocates memory on the device
        /// </summary>
        public static cudaError_t MallocArray (out cudaArray_t arr, ref cudaChannelFormatDesc chan, size_t width, size_t height, cudaMallocArrayFlags flags) { return instance.MallocArray (out arr, ref chan, width, height, flags) ; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost (out IntPtr ptr, size_t size) { return instance.MallocHost (out ptr, size) ; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out void* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out int* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (int*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out uint* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (uint*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out long* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (long*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out ulong* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (ulong*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out float* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (float*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out double* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (double*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out byte* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (byte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out sbyte* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (sbyte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out short* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (short*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates page-locked memory on the host
        /// </summary>
        public static cudaError_t MallocHost(out ushort* ptr, size_t size) { IntPtr res; cudaError_t cuer = instance.MallocHost(out res, size); ptr = (ushort*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out IntPtr devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { return instance.MallocManaged(out devPtr, size, flags); }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out void* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out int* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (int*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out uint* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (uint*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out long* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (long*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out ulong* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (ulong*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out float* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (float*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out double* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (double*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out byte* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (byte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out sbyte* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (sbyte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out short* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (short*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates memory that will be automatically managed by the Unified Memory system
        /// </summary>
        public static cudaError_t MallocManaged(out ushort* devPtr, size_t size, uint flags = (uint)cudaMemAttach.cudaMemAttachGlobal) { IntPtr res; cudaError_t cuer = instance.MallocManaged(out res, size, flags); devPtr = (ushort*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocate a mipmapped array on the device
        /// </summary>
        public static cudaError_t MallocMipmappedArray(out cudaMipmappedArray_t mipmappedArray, ref cudaChannelFormatDesc desc, cudaFuncAttributes extent, uint numLevels, uint flags = 0) { return instance.MallocMipmappedArray(out mipmappedArray, ref desc, extent, numLevels, flags); }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out IntPtr devptr, out size_t pitch, size_t width, size_t height) { return instance.MallocPitch(out devptr, out pitch, width, height); }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out void* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out int* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (int*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out uint* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (uint*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out long* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (long*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out ulong* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (ulong*)res.ToPointer(); return cuer; }

        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>        public static cudaError_t MallocPitch(out float* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (float*)res.ToPointer(); return cuer; }
        public static cudaError_t MallocPitch(out double* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (double*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out byte* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (byte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out sbyte* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (sbyte*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out short* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (short*)res.ToPointer(); return cuer; }
        /// <summary>
        /// Allocates pitched memory on the device
        /// </summary>
        public static cudaError_t MallocPitch(out ushort* devptr, out size_t pitch, size_t width, size_t height) { IntPtr res; cudaError_t cuer = instance.MallocPitch(out res, out pitch, width, height); devptr = (ushort*)res.ToPointer(); return cuer; }
        /// <summary>
        ///  Advise about the usage of a given memory range
        /// </summary>
        public static cudaError_t MemAdvise(IntPtr devptr, size_t count, cudaMemmoryAdvise advice, int device) { return instance.MemAdvise(devptr, count, advice, device); }
        /// <summary>
        ///  Advise about the usage of a given memory range
        /// </summary>
        public static cudaError_t MemAdvise(void* devptr, size_t count, cudaMemmoryAdvise advice, int device) { return instance.MemAdvise(new IntPtr(devptr), count, advice, device); }
        /// <summary>
        /// Gets free and total device memory
        /// </summary>
        public static cudaError_t MemGetInfo(out size_t free, out size_t total) { return instance.MemGetInfo(out free, out total); }
        /// <summary>
        ///  Prefetches memory to the specified destination device
        /// </summary>
        public static cudaError_t MemPrefetchAsync (IntPtr devptr, size_t count, int dstDevice, cudaStream_t stream) { return instance.MemPrefetchAsync(devptr, count, dstDevice, stream) ; }
        /// <summary>
        ///  Prefetches memory to the specified destination device
        /// </summary>
        public static cudaError_t MemPrefetchAsync(void* devptr, size_t count, int dstDevice, cudaStream_t stream) { return instance.MemPrefetchAsync(new IntPtr(devptr), count, dstDevice, stream); }
        /// <summary>
        ///  Prefetches memory to the specified destination device
        /// </summary>
        public static cudaError_t MemPrefetchAsync(IntPtr devptr, size_t count, int dstDevice) { return instance.MemPrefetchAsync(devptr, count, dstDevice, cudaStream_t.NO_STREAM); }
        /// <summary>
        ///  Prefetches memory to the specified destination device
        /// </summary>
        public static cudaError_t MemPrefetchAsync(void* devptr, size_t count, int dstDevice) { return instance.MemPrefetchAsync(new IntPtr(devptr), count, dstDevice, cudaStream_t.NO_STREAM); }
        /// <summary>
        /// Copies data between host and device
        /// <param name="dest">destination pointer</param>
        /// <param name="kind">direction of memcopy</param>
        /// <param name="size">number of bytes to copy</param>
        /// <param name="src">source pointer</param>
        /// </summary>
        public static cudaError_t Memcpy(IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind) { return instance.Memcpy(dest, src, size, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy(void* dest, IntPtr src, size_t size, cudaMemcpyKind kind) { return instance.Memcpy(new IntPtr(dest), src, size, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy(IntPtr dest, void* src, size_t size, cudaMemcpyKind kind) { return instance.Memcpy(dest, new IntPtr(src), size, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy(void* dest, void* src, size_t size, cudaMemcpyKind kind) { return instance.Memcpy(new IntPtr(dest), new IntPtr(src), size, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2D (IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2D (dest, dpitch, src, spitch, width, height, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2D(void* dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2D(new IntPtr(dest), dpitch, src, spitch, width, height, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2D(IntPtr dest, size_t dpitch, void* src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2D(dest, dpitch, new IntPtr(src), spitch, width, height, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2D(void* dest, size_t dpitch, void* src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2D(new IntPtr(dest), dpitch, new IntPtr(src), spitch, width, height, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DArrayToArray (cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2DArrayToArray (dest, wOffsetDest, hOffsetDest, src, wOffsetSrc, hOffsetSrc, width, height, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpy2DAsync")]
        public static cudaError_t Memcpy2DAsync (IntPtr dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DAsync (dest, dpitch, src, spitch, width, height, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpy2DAsync")]
        public static cudaError_t Memcpy2DAsync(void* dest, size_t dpitch, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DAsync(new IntPtr(dest), dpitch, src, spitch, width, height, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpy2DAsync")]
        public static cudaError_t Memcpy2DAsync(IntPtr dest, size_t dpitch, void* src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DAsync(dest, dpitch, new IntPtr(src), spitch, width, height, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpy2DAsync")]
        public static cudaError_t Memcpy2DAsync(void* dest, size_t dpitch, void* src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DAsync(new IntPtr(dest), dpitch, new IntPtr(src), spitch, width, height, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DFromArray (IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2DFromArray (dest, dpitch, src, wOffset, hOffset, width, height, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DFromArray(void* dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2DFromArray(new IntPtr(dest), dpitch, src, wOffset, hOffset, width, height, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DFromArrayAsync (IntPtr dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DFromArrayAsync (dest, dpitch, src, wOffset, hOffset, width, height, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DFromArrayAsync(void* dest, size_t dpitch, cudaArray_t src, size_t wOffset, size_t hOffset, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DFromArrayAsync(new IntPtr(dest), dpitch, src, wOffset, hOffset, width, height, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DToArray (cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2DToArray (dest, wOffsetDest, hOffsetDest, src, spitch, width, height, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DToArray(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, void* src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind) { return instance.Memcpy2DToArray(dest, wOffsetDest, hOffsetDest, new IntPtr(src), spitch, width, height, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DToArrayAsync (cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, IntPtr src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DToArrayAsync (dest, wOffsetDest, hOffsetDest, src, spitch, width, height, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy2DToArrayAsync(cudaArray_t dest, size_t wOffsetDest, size_t hOffsetDest, void* src, size_t spitch, size_t width, size_t height, cudaMemcpyKind kind, cudaStream_t stream) { return instance.Memcpy2DToArrayAsync(dest, wOffsetDest, hOffsetDest, new IntPtr(src), spitch, width, height, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t Memcpy3D (ref cudaMemcpy3DParms par) { return instance.Memcpy3D (ref par) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpy3DAsync")]
        public static cudaError_t Memcpy3DAsync (ref cudaMemcpy3DParms par, cudaStream_t stream) { return instance.Memcpy3DAsync (ref par, stream) ; }
        /// <summary>
        /// Copies memory between devices
        /// </summary>
        public static cudaError_t Memcpy3DPeer(ref cudaMemcpy3DPeerParms par) { return instance.Memcpy3DPeer(ref par); }
        /// <summary>
        /// Copies memory between devices
        /// </summary>
        public static cudaError_t Memcpy3DPeerAsync(ref cudaMemcpy3DPeerParms par, cudaStream_t stream) { return instance.Memcpy3DPeerAsync(ref par, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyArrayToArray (cudaArray_t dest, size_t wOffsetDst, size_t hOffsetDst, cudaArray_t src, size_t wOffsetSrc, size_t hOffsetSrc, size_t count, cudaMemcpyKind kind) { return instance.MemcpyArrayToArray (dest, wOffsetDst, hOffsetDst, src, wOffsetSrc, hOffsetSrc, count, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpyAsync")]
        public static cudaError_t MemcpyAsync (IntPtr dest, IntPtr src, size_t size, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyAsync (dest, src, size, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpyAsync")]
        public static cudaError_t MemcpyAsync(void* dest, IntPtr src, size_t size, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyAsync(new IntPtr(dest), src, size, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpyAsync")]
        public static cudaError_t MemcpyAsync(IntPtr dest, void* src, size_t size, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyAsync(dest, new IntPtr(src), size, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        [IntrinsicFunction("cudaMemcpyAsync")]
        public static cudaError_t MemcpyAsync(void* dest, void* src, size_t size, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyAsync(new IntPtr(dest), new IntPtr(src), size, kind, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyFromArray (IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind) { return instance.MemcpyFromArray (dest, src, wOffset, hOffset, count, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyFromArray(void* dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind) { return instance.MemcpyFromArray(new IntPtr(dest), src, wOffset, hOffset, count, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyFromArrayAsync (IntPtr dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyFromArrayAsync (dest, src, wOffset, hOffset, count, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyFromArrayAsync(void* dest, cudaArray_t src, size_t wOffset, size_t hOffset, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyFromArrayAsync(new IntPtr(dest), src, wOffset, hOffset, count, kind, stream); }
        /// <summary>
        /// Copies data from the given symbol on the device
        /// </summary>
        public static cudaError_t MemcpyFromSymbol (IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind) { return instance.MemcpyFromSymbol (dest, symbol, count, offset, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyFromSymbol(void* dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind) { return instance.MemcpyFromSymbol(new IntPtr(dest), symbol, count, offset, kind); }
        /// <summary>
        /// Copies data from the given symbol on the device
        /// </summary>
        public static cudaError_t MemcpyFromSymbolAsync (IntPtr dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyFromSymbolAsync (dest, symbol, count, offset, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyFromSymbolAsync(void* dest, string symbol, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyFromSymbolAsync(new IntPtr(dest), symbol, count, offset, kind, stream); }
        /// <summary>
        /// Copies memory between devices
        /// </summary>
        public static cudaError_t MemcpyPeer(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count) { return instance.MemcpyPeer(dest, dstDevice, src, srcDevice, count); }
        /// <summary>
        /// Copies memory between devices
        /// </summary>
        public static cudaError_t MemcpyPeer(void* dest, int dstDevice, IntPtr src, int srcDevice, size_t count) { return instance.MemcpyPeer(new IntPtr(dest), dstDevice, src, srcDevice, count); }
        /// <summary>
        /// Copies memory between devices
        /// </summary>
        public static cudaError_t MemcpyPeer(IntPtr dest, int dstDevice, void* src, int srcDevice, size_t count) { return instance.MemcpyPeer(dest, dstDevice, new IntPtr(src), srcDevice, count); }
        /// <summary>
        /// Copies memory between devices
        /// </summary>
        public static cudaError_t MemcpyPeer(void* dest, int dstDevice, void* src, int srcDevice, size_t count) { return instance.MemcpyPeer(new IntPtr(dest), dstDevice, new IntPtr(src), srcDevice, count); }
        /// <summary>
        /// Copies memory between devices
        /// </summary>
        public static cudaError_t MemcpyPeerAsync(IntPtr dest, int dstDevice, IntPtr src, int srcDevice, size_t count, cudaStream_t stream) { return instance.MemcpyPeerAsync(dest, dstDevice, src, srcDevice, count, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyPeerAsync(void* dest, int dstDevice, IntPtr src, int srcDevice, size_t count, cudaStream_t stream) { return instance.MemcpyPeerAsync(new IntPtr(dest), dstDevice, src, srcDevice, count, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyPeerAsync(IntPtr dest, int dstDevice, void* src, int srcDevice, size_t count, cudaStream_t stream) { return instance.MemcpyPeerAsync(dest, dstDevice, new IntPtr(src), srcDevice, count, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyPeerAsync(void* dest, int dstDevice, void* src, int srcDevice, size_t count, cudaStream_t stream) { return instance.MemcpyPeerAsync(new IntPtr(dest), dstDevice, new IntPtr(src), srcDevice, count, stream); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyToArray(cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind) { return instance.MemcpyToArray(dest, wOffset, hOffset, src, count, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyToArray(cudaArray_t dest, size_t wOffset, size_t hOffset, void* src, size_t count, cudaMemcpyKind kind) { return instance.MemcpyToArray(dest, wOffset, hOffset, new IntPtr(src), count, kind); }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyToArrayAsync (cudaArray_t dest, size_t wOffset, size_t hOffset, IntPtr src, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyToArrayAsync (dest, wOffset, hOffset, src, count, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyToArrayAsync(cudaArray_t dest, size_t wOffset, size_t hOffset, void* src, size_t count, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyToArrayAsync(dest, wOffset, hOffset, new IntPtr(src), count, kind, stream); }
        /// <summary>
        /// Copies data to the given symbol on the device
        /// </summary>
        public static cudaError_t MemcpyToSymbol (string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind) { return instance.MemcpyToSymbol (symbol, src, count, offset, kind) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyToSymbol(string symbol, void* src, size_t count, size_t offset, cudaMemcpyKind kind) { return instance.MemcpyToSymbol(symbol, new IntPtr(src), count, offset, kind); }
        /// <summary>
        /// Copies data to the given symbol on the device
        /// </summary>
        public static cudaError_t MemcpyToSymbolAsync (string symbol, IntPtr src, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyToSymbolAsync (symbol, src, count, offset, kind, stream) ; }
        /// <summary>
        /// Copies data between host and device
        /// </summary>
        public static cudaError_t MemcpyToSymbolAsync(string symbol, void* src, size_t count, size_t offset, cudaMemcpyKind kind, cudaStream_t stream) { return instance.MemcpyToSymbolAsync(symbol, new IntPtr(src), count, offset, kind, stream); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        public static cudaError_t Memset (IntPtr devPtr, int value, size_t count) { return instance.Memset (devPtr, value, count) ; }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        public static cudaError_t Memset(void* devPtr, int value, size_t count) { return instance.Memset(new IntPtr(devPtr), value, count); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        public static cudaError_t Memset2D(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height) { return instance.Memset2D(devPtr, pitch, value, width, height); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        public static cudaError_t Memset2D(void* devPtr, size_t pitch, int value, size_t width, size_t height) { return instance.Memset2D(new IntPtr(devPtr), pitch, value, width, height); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        [IntrinsicFunction("cudaMemset2DAsync")]
        public static cudaError_t Memset2DAsync(IntPtr devPtr, size_t pitch, int value, size_t width, size_t height, cudaStream_t stream) { return instance.Memset2DAsync(devPtr, pitch, value, width, height, stream); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        public static cudaError_t Memset2DAsync(void* devPtr, size_t pitch, int value, size_t width, size_t height, cudaStream_t stream) { return instance.Memset2DAsync(new IntPtr(devPtr), pitch, value, width, height, stream); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        public static cudaError_t Memset3D(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent) { return instance.Memset3D(devPtr, value, extent); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        [IntrinsicFunction("cudaMemset3DAsync")]
        public static cudaError_t Memset3DAsync(cudaPitchedPtr devPtr, int value, cudaFuncAttributes extent, cudaStream_t stream) { return instance.Memset3DAsync(devPtr, value, extent, stream); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        [IntrinsicFunction("cudaMemsetAsync")]
        public static cudaError_t MemsetAsync(IntPtr devPtr, int value, size_t count, cudaStream_t stream) { return instance.MemsetAsync(devPtr, value, count, stream); }
        /// <summary>
        /// Initializes or sets device memory to a value
        /// </summary>
        /// 
        [IntrinsicFunction("cudaMemsetAsync")]
        public static cudaError_t MemsetAsync(void* devPtr, int value, size_t count, cudaStream_t stream) { return instance.MemsetAsync(new IntPtr(devPtr), value, count, stream); }

#endregion

#region Surface Management

        /// <summary>
        /// Creates a surface object and returns it in pSurfObject. pResDesc describes the data to perform
        /// surface load/stores on. cudaResourceDesc::resType must be cudaResourceTypeArray and 
        /// cudaResourceDesc::res::array::array must be set to a valid CUDA array handle. 
        /// 
        /// Surface objects are only supported on devices of compute capability 3.0 or higher.Additionally,
        /// a surface object is an opaque value, and, as such, should only be accessed through CUDA API
        /// calls. 
        /// 
        /// See <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__SURFACE__OBJECT.html#group__CUDART__SURFACE__OBJECT">nvidia documentation</see> 
        /// </summary>
        /// <param name="surface">Surface object to create </param>
        /// <param name="resDesc">Resource descriptor</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        public static cudaError_t CreateSurfaceObject(out cudaSurfaceObject_t surface, ref cudaResourceDesc resDesc) { return instance.CreateSurfaceObject(out surface, ref resDesc); }
        /// <summary>
        /// Destroys the surface object specified by surfObject. 
        /// 
        /// See <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__SURFACE__OBJECT.html#group__CUDART__SURFACE__OBJECT">nvidia documentation</see> 
        /// </summary>
        /// <param name="surface">Surface object to destroy</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        public static cudaError_t DestroySurfaceObject(cudaSurfaceObject_t surface) { return instance.DestroySurfaceObject(surface); }

        /// <summary>
        /// Returns a surface object's resource descriptor Returns the resource descriptor for the surface
        /// object specified by surfObject. 
        /// </summary>
        /// <param name="resDesc">Resource descriptor </param>
        /// <param name="surface">Surface object</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        public static cudaError_t GetSurfaceObjectResourceDesc(out cudaResourceDesc resDesc, cudaSurfaceObject_t surface) { return instance.GetSurfaceObjectResourceDesc(out resDesc, surface); }

#endregion

#region Texture Management

        /// <summary>
        /// Creates a texture object and returns it in pTexObject. pResDesc describes the data to texture
        /// from. pTexDesc describes how the data should be sampled. pResViewDesc is an optional argument
        /// that specifies an alternate format for the data described by pResDesc, and also describes the
        /// subresource region to restrict access to when texturing. pResViewDesc can only be specified if
        /// the type of resource is a CUDA array or a CUDA mipmapped array. 
        /// 
        /// Texture objects are only supported on devices of compute capability 3.0 or higher.Additionally,
        /// a texture object is an opaque value, and, as such, should only be accessed through CUDA API
        /// calls. 
        /// 
        /// See <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__TEXTURE__OBJECT.html#group__CUDART__TEXTURE__OBJECT">nvidia documentation</see>
        /// </summary>
        /// <param name="texture">Texture object to create </param>
        /// <param name="resDesc">Resource descriptor </param>
        /// <param name="texDesc">Texture descriptor </param>
        /// <param name="ResViewDesc">Resource view descriptor</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        public static cudaError_t CreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc, ref cudaResourceViewDesc ResViewDesc) { return instance.CreateTextureObject(out texture, ref resDesc, ref texDesc, ref ResViewDesc); }
        /// <summary>
        /// Creates a texture object and returns it in pTexObject. pResDesc describes the data to texture
        /// from. pTexDesc describes how the data should be sampled. pResViewDesc is an optional argument
        /// that specifies an alternate format for the data described by pResDesc, and also describes the
        /// subresource region to restrict access to when texturing. pResViewDesc can only be specified if
        /// the type of resource is a CUDA array or a CUDA mipmapped array. 
        /// 
        /// Texture objects are only supported on devices of compute capability 3.0 or higher.Additionally,
        /// a texture object is an opaque value, and, as such, should only be accessed through CUDA API
        /// calls. 
        /// 
        /// See <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__TEXTURE__OBJECT.html#group__CUDART__TEXTURE__OBJECT">nvidia documentation</see>
        /// </summary>
        /// <param name="texture">Texture object to create </param>
        /// <param name="resDesc">Resource descriptor </param>
        /// <param name="texDesc">Texture descriptor </param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        public static cudaError_t CreateTextureObject(out cudaTextureObject_t texture, ref cudaResourceDesc resDesc, ref cudaTextureDesc texDesc) { return instance.CreateTextureObject(out texture, ref resDesc, ref texDesc); }
        /// <summary>
        /// Destroys the texture object specified by texObject. 
        /// 
        /// See <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__TEXTURE__OBJECT.html#group__CUDART__TEXTURE__OBJECT">nvidia documentation</see>
        /// </summary>
        /// <param name="texture">Texture object to destroy</param>
        /// <returns></returns>
        public static cudaError_t DestroyTextureObject(cudaTextureObject_t texture) { return instance.DestroyTextureObject(texture); }

        /// <summary>
        /// Returns the resource descriptor for the texture object specified by texObject. 
        /// See <see href="http://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__TEXTURE__OBJECT.html#group__CUDART__TEXTURE__OBJECT">nvidia documentation</see>
        /// </summary>
        /// <param name="resDesc">Resource descriptor </param>
        /// <param name="texture">Texture object</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue</returns>
        public static cudaError_t GetTextureObjectResourceDesc(out cudaResourceDesc resDesc, cudaTextureObject_t texture) { return instance.GetTextureObjectResourceDesc(out resDesc, texture); }

#endregion

#region OPENGL interop
        /// <summary>
        /// Registers a buffer object for access by CUDA. 
        /// </summary>
        /// <param name="buffer">Buffer object ID to register</param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__OPENGL__DEPRECATED.html#group__CUDART__OPENGL__DEPRECATED_1gb835a92a340e999f4eaa55a8d57e122c">nvidia documentation</see>
        public static cudaError_t GLRegisterBufferObject(uint buffer) { return instance.GLRegisterBufferObject(buffer); }
        /// <summary>
        /// Registers an OpenGL buffer object. 
        /// </summary>
        /// <param name="pCudaResource"> Pointer to the returned object handle </param>
        /// <param name="buffer">name of buffer object to be registered</param>
        /// <param name="Flags">Register flags</param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__OPENGL.html#group__CUDART__OPENGL_1g0fd33bea77ca7b1e69d1619caf44214b">nvidia documentation</see>
        public static cudaError_t GraphicsGLRegisterBuffer(out IntPtr pCudaResource, uint buffer, uint Flags) { return instance.GraphicsGLRegisterBuffer(out pCudaResource, buffer, Flags); }
        /// <summary>
        /// Unregisters a graphics resource for access by CUDA. 
        /// </summary>
        /// <param name="resource">Resource to unregister</param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__INTEROP.html#group__CUDART__INTEROP_1gc65d1f2900086747de1e57301d709940">nvidia documentation</see>
        public static cudaError_t GraphicsUnregisterResource(IntPtr resource) { return instance.GraphicsUnregisterResource(resource); }

        /// <summary>
        /// Unmaps a buffer object for access by CUDA. 
        /// </summary>
        /// <param name="buffer">Buffer object to unmap </param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__OPENGL__DEPRECATED.html#group__CUDART__OPENGL__DEPRECATED_1g5ce0566e8543a8c7677b619acfefd5b5">nvidia documentation</see>
        public static cudaError_t GLUnregisterBufferObject(uint buffer) { return instance.GLUnregisterBufferObject(buffer); }
        /// <summary>
        /// Get an device pointer through which to access a mapped graphics resource. 
        /// </summary>
        /// <param name="devPtr"> Returned pointer through which resource may be accessed </param>
        /// <param name="size"> Returned size of the buffer accessible starting at *devPtr</param>
        /// <param name="resource">Mapped resource to access</param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__INTEROP.html#group__CUDART__INTEROP_1ga36881081c8deb4df25c256158e1ac99">nvidia documentation</see>
        public static cudaError_t GraphicsResourceGetMappedPointer(out IntPtr devPtr, out size_t size, IntPtr resource) { return instance.GraphicsResourceGetMappedPointer(out devPtr, out size, resource); }
        /// <summary>
        /// Set usage flags for mapping a graphics resource. 
        /// </summary>
        /// <param name="resource">Registered resource to set flags for</param>
        /// <param name="flags"> Parameters for resource mapping</param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__INTEROP.html#group__CUDART__INTEROP_1g5f94a0043909fddc100ab5f0c2476b9f">nvidia documentation</see>
        public static cudaError_t GraphicsResourceSetMapFlags(IntPtr resource, uint flags) { return instance.GraphicsResourceSetMapFlags(resource, flags); }
        /// <summary>
        /// Map graphics resources for access by CUDA. 
        /// </summary>
        /// <param name="count"> Number of resources to map </param>
        /// <param name="resources">Resources to map for CUDA </param>
        /// <param name="stream">Stream for synchronization</param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__INTEROP.html#group__CUDART__INTEROP_1gad8fbe74d02adefb8e7efb4971ee6322">nvidia documentation</see>
        public static cudaError_t GraphicsMapResources(int count, IntPtr[] resources, cudaStream_t stream) { return instance.GraphicsMapResources(count, resources, stream); }
        /// <summary>
        /// Unmap graphics resources. 
        /// </summary>
        /// <param name="count"> Number of resources to map </param>
        /// <param name="resources">Resources to map for CUDA </param>
        /// <param name="stream">Stream for synchronization</param>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__INTEROP.html#group__CUDART__INTEROP_1g11988ab4431b11ddb7cbde7aedb60491">nvidia documentation</see>
        public static cudaError_t GraphicsUnmapResources(int count, IntPtr[] resources, cudaStream_t stream) { return instance.GraphicsUnmapResources(count, resources, stream); }

        /// <summary>
        /// Register an OpenGL texture or renderbuffer object.
        /// </summary>
        /// <param name="cudaGraphicsResource">Pointer to the returned object handle </param>
        /// <param name="image">name of texture or renderbuffer object to be registered</param>
        /// <param name="target">Identifies the type of object specified by image</param>
        /// <param name="flags">Register flags</param>
        /// <returns>cudaErrorInvalidDevice, cudaErrorInvalidValue, cudaErrorInvalidResourceHandle, cudaErrorUnknown</returns>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__OPENGL.html#group__CUDART__OPENGL_1g80d12187ae7590807c7676697d9fe03d">nvidia documentation</see>
        public static cudaError_t GraphicsGLRegisterImage(out IntPtr cudaGraphicsResource, uint image, uint target, uint flags) { return instance.GraphicsGLRegisterImage(out cudaGraphicsResource, image, target, flags); }

        /// <summary>
        /// Get an array through which to access a subresource of a mapped graphics resource. 
        /// </summary>
        /// <param name="array">Returned array through which a subresource of resource may be accessed</param>
        /// <param name="resource"> Mapped resource to access </param>
        /// <param name="arrayIndex">Array index for array textures or cubemap face index as defined by cudaGraphicsCubeFace for cubemap textures for the subresource to access </param>
        /// <param name="mipLevel">Mipmap level for the subresource to access</param>
        /// <returns>cudaSuccess, cudaErrorInvalidValue, cudaErrorInvalidResourceHandle, cudaErrorUnknown</returns>
        /// <see href="https://docs.nvidia.com/cuda/cuda-runtime-api/group__CUDART__INTEROP.html#group__CUDART__INTEROP_1g0dd6b5f024dfdcff5c28a08ef9958031">nvidia documentation</see>
        public static cudaError_t GraphicsSubResourceGetMappedArray(out cudaArray_t array, IntPtr resource, uint arrayIndex, uint mipLevel) { return instance.GraphicsSubResourceGetMappedArray(out array, resource, arrayIndex, mipLevel); }

#endregion


        private static bool _isCudaAvailable;
        private static bool _cudaAvailableCalled = false;
        /// <summary>
        /// Check if CUDA is available
        /// </summary>        
        public static bool IsCudaAvailable()
        {
            if (!_cudaAvailableCalled)
            {
                _cudaAvailableCalled = true;
                try
                {
                    int count;
                    cuda.GetDeviceCount(out count);
                    _isCudaAvailable = count > 0 && cuda.GetLastError() == cudaError_t.cudaSuccess;
                    return _isCudaAvailable;
                }
                catch (Exception)
                {
                }
                _isCudaAvailable = false;
            }

            return _isCudaAvailable;
        }

        /// <summary>
        /// check and error
        /// </summary>
        /// <param name="err">error to check</param>
        /// <param name="abort">abort process if any error</param>
        public static void ERROR_CHECK(cudaError_t err, bool abort = true)
        {
            if (err != cudaError_t.cudaSuccess)
            {
                var callStack = new StackFrame(1, true);
                Console.Error.WriteLine("CUDA ERROR at {0}[{1}] : {2}", callStack.GetFileName(), callStack.GetFileLineNumber(), cuda.GetErrorString(err));
                if (abort)
                {
                    Environment.Exit((int) err);
                }
            }
        }
    }
}