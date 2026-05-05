using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Hybridizer.Runtime.CUDAImports
{

    /// <summary>
    /// driver API
    /// </summary>
    public class driver
    {
        // Synthetic library name. The static constructor registers a resolver
        // (via System.Runtime.InteropServices.NativeLibrary, available at
        // runtime on .NET 5+) that maps it to the real binary per OS:
        //  - Windows: "nvcuda.dll"
        //  - Linux:   "libcuda.so.1"
        // On older runtimes (where NativeLibrary is unavailable) the default
        // loader falls back to searching for a file named "nvcuda" or
        // "libnvcuda.so", and consumers are expected to provide it via PATH /
        // LD_LIBRARY_PATH.
        const string dllPath = "nvcuda";

        static driver()
        {
            TryRegisterCudaDriverResolver();
        }

        private static void TryRegisterCudaDriverResolver()
        {
            try
            {
                var nlType = Type.GetType("System.Runtime.InteropServices.NativeLibrary, System.Runtime.InteropServices")
                          ?? Type.GetType("System.Runtime.InteropServices.NativeLibrary, System.Private.CoreLib")
                          ?? Type.GetType("System.Runtime.InteropServices.NativeLibrary");
                if (nlType == null) return;

                var resolverType = nlType.Assembly.GetType("System.Runtime.InteropServices.DllImportResolver");
                if (resolverType == null) return;

                var setResolver = nlType.GetMethod(
                    "SetDllImportResolver",
                    BindingFlags.Public | BindingFlags.Static,
                    null,
                    new[] { typeof(Assembly), resolverType },
                    null);
                if (setResolver == null) return;

                var resolveMethod = typeof(driver).GetMethod(
                    nameof(ResolveCudaDriver),
                    BindingFlags.NonPublic | BindingFlags.Static);
                var del = Delegate.CreateDelegate(resolverType, resolveMethod);
                setResolver.Invoke(null, new object[] { typeof(driver).Assembly, del });
            }
            catch
            {
                // Older runtime without NativeLibrary; fall back to the default loader.
            }
        }

        private static IntPtr ResolveCudaDriver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName != dllPath && libraryName != "nvcuda.dll") return IntPtr.Zero;
            string target = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "libcuda.so.1" : "nvcuda.dll";
            try
            {
                var nlType = Type.GetType("System.Runtime.InteropServices.NativeLibrary, System.Runtime.InteropServices")
                          ?? Type.GetType("System.Runtime.InteropServices.NativeLibrary, System.Private.CoreLib")
                          ?? Type.GetType("System.Runtime.InteropServices.NativeLibrary");
                var load = nlType?.GetMethod("Load", new[] { typeof(string) });
                return (IntPtr)(load?.Invoke(null, new object[] { target }) ?? IntPtr.Zero);
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        
        [DllImport(dllPath, CharSet = CharSet.Ansi)] static extern CUresult cuCtxGetCurrent(out CUcontext ctx);
        /// <summary>
        /// Returns the CUDA context bound to the calling CPU thread.
        /// Returns in \p* pctx the CUDA context bound to the calling CPU thread.
        /// If no context is bound to the calling CPU thread then \p* pctx is
        /// set to NULL and <see cref="CUresult.CUDA_SUCCESS" /> is returned.
        /// </summary>
        public static CUresult GetContext(out CUcontext ctx) { return cuCtxGetCurrent(out ctx); }


        /// <summary>
        /// check curesult against error and display stacktrace and error string
        /// </summary>
        /// <param name="err"></param>
        /// <param name="abort"></param>
        public static void ERROR_CHECK(CUresult err, bool abort = true)
        {
            if (err != CUresult.CUDA_SUCCESS)
            {
                var callStack = new StackFrame(1, true);
                string errorString;
                GetErrorString(err, out errorString);
                Console.Error.WriteLine("DRIVER API ERROR at {0}[{1}] : {2}", callStack.GetFileName(), callStack.GetFileLineNumber(), errorString);
                if (abort)
                {
                    Environment.Exit((int)err);
                }
            }
        }

        #region nvrtc
        [DllImport(dllPath)]
        static extern CUresult cuModuleLoadDataEx(out CUmodule module, IntPtr image, uint numOptions, IntPtr /* CUjit_option* */ options, IntPtr /* void** */ optionValues);

        /// <summary>
        /// Load a module's data with options. 
        /// </summary>
        /// <param name="module">Returned module </param>
        /// <param name="image">Module data to load</param>
        /// <param name="numOptions">Number of options</param>
        /// <param name="options">Options for JIT</param>
        /// <param name="optionValues">Option values for JIT</param>
        /// <returns></returns>
        public static CUresult ModuleLoadDataEx(out CUmodule module, IntPtr image, uint numOptions, IntPtr /* CUjit_option* */ options, IntPtr /* void** */ optionValues)
        {
            return cuModuleLoadDataEx(out module, image, numOptions, options, optionValues);
        }

        /// <summary>
        /// generates cubin from ptx
        /// </summary>
        /// <param name="ptx"></param>
        /// <param name="cubin"></param>
        /// <param name="cubinSize"></param>
        /// <param name="info_log"></param>
        /// <param name="error_log"></param>
        /// <returns></returns>
        public static CUresult GenerateCubin(string[] ptx, out IntPtr cubin, out size_t cubinSize, out string info_log, out string error_log)
        {
            List<GCHandle> arrayHandles = new List<GCHandle>();

            int logsize = 32 * 1024;
            byte[] error_log_bytes = new byte[logsize];
            byte[] info_log_bytes = new byte[logsize];

            CUdevice device;
            driver.DeviceGet(out device, 0);
            int major, minor;
            driver.DeviceGetAttribute(out major, CUdevice_attribute.CU_DEVICE_ATTRIBUTE_COMPUTE_CAPABILITY_MAJOR, device);
            driver.DeviceGetAttribute(out minor, CUdevice_attribute.CU_DEVICE_ATTRIBUTE_COMPUTE_CAPABILITY_MINOR, device);
            int computeTarget = 10 * major + minor;

            CUjit_option[] options = new CUjit_option[7];
            IntPtr[] optionValues = new IntPtr[7];

            float[] wallTime = new float[1];

            arrayHandles.Add(GCHandle.Alloc(wallTime, GCHandleType.Pinned));
            arrayHandles.Add(GCHandle.Alloc(info_log_bytes, GCHandleType.Pinned));
            arrayHandles.Add(GCHandle.Alloc(error_log_bytes, GCHandleType.Pinned));

            options[0] = CUjit_option.CU_JIT_WALL_TIME;
            optionValues[0] = Marshal.UnsafeAddrOfPinnedArrayElement(wallTime, 0);
            options[1] = CUjit_option.CU_JIT_INFO_LOG_BUFFER;
            optionValues[1] = Marshal.UnsafeAddrOfPinnedArrayElement(info_log_bytes, 0);
            options[2] = CUjit_option.CU_JIT_INFO_LOG_BUFFER_SIZE_BYTES;
            optionValues[2] = new IntPtr(logsize);
            options[3] = CUjit_option.CU_JIT_ERROR_LOG_BUFFER;
            optionValues[3] = Marshal.UnsafeAddrOfPinnedArrayElement(error_log_bytes, 0);
            options[4] = CUjit_option.CU_JIT_ERROR_LOG_BUFFER_SIZE_BYTES;
            optionValues[4] = new IntPtr(logsize);
            options[5] = CUjit_option.CU_JIT_LOG_VERBOSE;
            optionValues[5] = new IntPtr(1);
            options[6] = CUjit_option.CU_JIT_TARGET;
            optionValues[6] = new IntPtr(computeTarget);

            CUlinkState state;

            CUresult cures = driver.LinkCreate(7u, options, optionValues, out state);
            if (cures != CUresult.CUDA_SUCCESS)
            {
                info_log = Encoding.ASCII.GetString(info_log_bytes);
                error_log = "ERROR AT LINK CREATE : " + Encoding.ASCII.GetString(error_log_bytes);
                cubin = IntPtr.Zero;
                cubinSize = 0;
                return cures;
            }

            for (int k = 0; k < ptx.Length; ++k)
            {
                byte[] PTXchars = ASCIIEncoding.ASCII.GetBytes(ptx[k]);
                arrayHandles.Add(GCHandle.Alloc(PTXchars, GCHandleType.Pinned));

                cures = driver.LinkAddData(state, CUjitInputType.CU_JIT_INPUT_PTX,
                    Marshal.UnsafeAddrOfPinnedArrayElement(PTXchars, 0), PTXchars.Length - 1,
                    null, 0, null, null);
                if (cures != CUresult.CUDA_SUCCESS)
                {
                    info_log = Encoding.ASCII.GetString(info_log_bytes);
                    error_log = "ERROR AT LINK ADD DATA : " + Encoding.ASCII.GetString(error_log_bytes);
                    cubin = IntPtr.Zero;
                    cubinSize = 0;
                    return cures;
                }
            }

            cures = driver.LinkComplete(state, out cubin, out cubinSize);
            if (cures != CUresult.CUDA_SUCCESS)
            {
                info_log = Encoding.ASCII.GetString(info_log_bytes);
                error_log = "ERROR AT LINK COMPLETE : " + Encoding.ASCII.GetString(error_log_bytes);
                return cures;
            }

            info_log = "Compilation OK -- cubin generated";
            error_log = "";
            return CUresult.CUDA_SUCCESS;
        }


        [DllImport(dllPath)]
        static extern CUresult cuModuleLoadData(out CUmodule module, IntPtr image);

        /// <summary>
        /// Load a module's data. 
        /// </summary>
        /// <param name="module">Returned module</param>
        /// <param name="image">Module data to load</param>
        /// <returns></returns>
        public static CUresult ModuleLoadData(out CUmodule module, IntPtr image)
        {
            return cuModuleLoadData(out module, image);
        }
        
        [DllImport(dllPath, EntryPoint = "cuModuleGetGlobal_v2", CharSet = CharSet.Ansi)]
        static extern CUresult cuModuleGetGlobal(out IntPtr devptr, out size_t size, CUmodule module, [MarshalAs(UnmanagedType.LPStr)] string symbol);

        /// <summary>
        /// Returns a global pointer from a module.
        /// </summary>
        /// <param name="devptr">Returned global device pointer</param>
        /// <param name="size">Returned global size in bytes</param>
        /// <param name="module">Module to retrieve global from</param>
        /// <param name="symbol">Name of global to retrieve</param>
        /// <returns></returns>
        public static CUresult ModuleGetGlobal(out IntPtr devptr, out size_t size, CUmodule module, string symbol)
        {
            return cuModuleGetGlobal(out devptr, out size, module, symbol);
        }
        
        [DllImport(dllPath, EntryPoint = "cuModuleGetFunction", CharSet = CharSet.Ansi)]
        static extern CUresult cuModuleGetFunction(out CUfunction func, CUmodule module, [MarshalAs(UnmanagedType.LPStr)] string symbol);

        /// <summary>
        /// Returns a global pointer from a module.
        /// </summary>
        public static CUresult ModuleGetFunction(out CUfunction func, CUmodule module, string symbol)
        {
            return cuModuleGetFunction(out func, module, symbol);
        }
        
        [DllImport(dllPath, EntryPoint = "cuLaunchKernel", CharSet = CharSet.Ansi)]
        static extern CUresult cuLaunchKernel(CUfunction func, int gX, int gY, int gZ, int bX, int bY, int bZ, int shared, CUstream stream, IntPtr parameters, IntPtr extra);

        /// <summary>
        /// Launches a CUDA function. 
        /// </summary>
        /// <param name="func">Kernel to launch</param>
        /// <param name="gX">Width of grid in blocks</param>
        /// <param name="gY">Height of grid in blocks</param>
        /// <param name="gZ">Depth of grid in blocks</param>
        /// <param name="bX">X dimension of each thread block</param>
        /// <param name="bY">Y dimension of each thread block</param>
        /// <param name="bZ">Z dimension of each thread block</param>
        /// <param name="shared">Dynamic shared-memory size per thread block in bytes</param>
        /// <param name="stream">Stream identifier</param>
        /// <param name="parameters">Array of pointers to kernel parameters</param>
        /// <param name="extra">Extra options</param>
        /// <returns></returns>
        public static CUresult LaunchKernel(CUfunction func, int gX, int gY, int gZ, int bX, int bY, int bZ, int shared, CUstream stream, IntPtr parameters, IntPtr extra)
        {
            return cuLaunchKernel(func, gX, gY, gZ, bX, bY, bZ, shared, stream, parameters, extra);
        }

        [DllImport(dllPath, EntryPoint = "cuLinkCreate_v2", CharSet = CharSet.Ansi)]
        static extern CUresult cuLinkCreate(uint numOptions, CUjit_option[] options, IntPtr[] optionValues, out CUlinkState stateOut);

        /// <summary>
        /// Creates a pending JIT linker invocation. 
        /// </summary>
        /// <param name="numOptions">Size of options arrays</param>
        /// <param name="options">Array of linker and compiler options</param>
        /// <param name="optionValues">Array of option values, each cast to void *</param>
        /// <param name="stateOut">On success, this will contain a CUlinkState to specify and complete this action</param>
        /// <returns></returns>
        public static CUresult LinkCreate(uint numOptions, CUjit_option[] options, IntPtr[] optionValues, out CUlinkState stateOut)
        {
            return cuLinkCreate(numOptions, options, optionValues, out stateOut);
        }
        
        [DllImport(dllPath, EntryPoint = "cuLinkAddData_v2", CharSet = CharSet.Ansi)]
        static extern CUresult cuLinkAddData(CUlinkState state, CUjitInputType type, IntPtr data, size_t size, string name,
                        uint numOptions, CUjit_option[] options, IntPtr[] optionValues);

        /// <summary>
        /// Add an input to a pending linker invocation.
        /// </summary>
        /// <param name="state">A pending linker action.</param>
        /// <param name="type">The type of the input data.</param>
        /// <param name="data">The input data. PTX must be NULL-terminated.</param>
        /// <param name="size">The length of the input data.</param>
        /// <param name="name">An optional name for this input in log messages. </param>
        /// <param name="numOptions">Size of options.</param>
        /// <param name="options">Options to be applied only for this input (overrides options from cuLinkCreate</param>
        /// <param name="optionValues">Array of option values, each cast to void *</param>
        /// <returns></returns>
        public static CUresult LinkAddData(CUlinkState state, CUjitInputType type, IntPtr data, size_t size, string name,
                        uint numOptions, CUjit_option[] options, IntPtr[] optionValues)
        {
            return cuLinkAddData(state, type, data, size, name, numOptions, options, optionValues);
        }
        
        [DllImport(dllPath, CharSet = CharSet.Ansi)]
        static extern CUresult cuLinkComplete(CUlinkState state, out IntPtr cubinOut, out size_t sizeOut);

        /// <summary>
        /// Complete a pending linker invocation.
        /// </summary>
        /// <param name="state">A pending linker invocation</param>
        /// <param name="cubinOut">On success, this will point to the output image</param>
        /// <param name="sizeOut">Optional parameter to receive the size of the generated image</param>
        /// <returns></returns>
        public static CUresult LinkComplete(CUlinkState state, out IntPtr cubinOut, out size_t sizeOut)
        {
            return cuLinkComplete(state, out cubinOut, out sizeOut);
        }

        #endregion

        #region initialization
        
        [DllImport(dllPath, CharSet = CharSet.Ansi)] static extern CUresult cuInit(uint Flags);

        /// <summary>
        /// Initialize the CUDA driver API. 
        /// </summary>
        /// <param name="Flags"> Initialization flag for CUDA.</param>
        /// <returns></returns>
        /// <remarks>Initializes the driver API and must be called before any other function from the driver API. Currently, the Flags parameter must be 0. If cuInit() has not been called, any function from the driver API will return <see cref="CUresult.CUDA_ERROR_NOT_INITIALIZED"/> . </remarks>
        public static CUresult Init(uint Flags)
        {
            return cuInit(Flags);
        }
        #endregion

        #region version management
        [DllImport(dllPath, CharSet = CharSet.Ansi)] static extern CUresult cuDriverGetVersion(out int driverVersion);

        /// <summary>
        /// Returns the latest CUDA version supported by driver.
        /// </summary>
        /// <param name="driverVersion">Returns the CUDA driver version</param>
        /// <returns></returns>
        public static CUresult DriverGetVersion(out int driverVersion)
        {
            return cuDriverGetVersion(out driverVersion);
        }

        #endregion

        #region device management

        [DllImport(dllPath, CharSet = CharSet.Ansi)] static extern CUresult cuDeviceGetCount(out int count);

        /// <summary>
        /// Returns the number of compute-capable devices.
        /// </summary>
        /// <param name="count">Returned number of compute-capable devices</param>
        /// <returns></returns>
        public static CUresult DeviceGetCount(out int count)
        {
            return cuDeviceGetCount(out count);
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi)] static extern CUresult cuDeviceGet(out CUdevice device, int ordinal);

        /// <summary>
        /// Returns a handle to a compute device.
        /// </summary>
        /// <param name="device">Returned device handle</param>
        /// <param name="ordinal">Device number to get handle for</param>
        /// <returns></returns>
        public static CUresult DeviceGet(out CUdevice device, int ordinal)
        {
            return cuDeviceGet(out device, ordinal);
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi)] static extern CUresult cuDeviceGetAttribute(out int pi, CUdevice_attribute attrib, CUdevice dev);

        /// <summary>
        /// Returns information about the device
        /// </summary>
        /// <param name="pi">Returned device attribute value</param>
        /// <param name="attrib">Device attribute to query </param>
        /// <param name="device">Device handle</param>
        /// <returns></returns>
        public static CUresult DeviceGetAttribute(out int pi, CUdevice_attribute attrib, CUdevice device)
        {
            return cuDeviceGetAttribute(out pi, attrib, device);
        }

        #endregion

        #region memory management
        [DllImport(dllPath, EntryPoint = "cuMemcpyDtoH_v2", CharSet = CharSet.Ansi)]
        static extern CUresult cuMemcpyDtoH(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostPtr"></param>
        /// <param name="devPtr"></param>
        /// <param name="sizeInBytes"></param>
        /// <returns></returns>
        public static CUresult MemcpyDtoH(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes)
        {
            return cuMemcpyDtoH(hostPtr, devPtr, sizeInBytes);
        }

        [DllImport(dllPath, EntryPoint = "cuMemcpyDtoHAsync_v2", CharSet = CharSet.Ansi)]
        static extern CUresult cuMemcpyDtoHAsync(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes, CUstream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostPtr"></param>
        /// <param name="devPtr"></param>
        /// <param name="sizeInBytes"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static CUresult MemcpyDtoHAsync(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes, CUstream stream)
        {
            return cuMemcpyDtoHAsync(hostPtr, devPtr, sizeInBytes, stream);
        }

        [DllImport(dllPath, EntryPoint = "cuMemcpyHtoDAsync_v2", CharSet = CharSet.Ansi)]
        static extern CUresult cuMemcpyHtoDAsync(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes, CUstream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostPtr"></param>
        /// <param name="devPtr"></param>
        /// <param name="sizeInBytes"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static CUresult MemcpyHtoDAsync(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes, CUstream stream)
        {
            return cuMemcpyHtoDAsync(hostPtr, devPtr, sizeInBytes, stream);
        }

        [DllImport(dllPath, EntryPoint = "cuMemcpyDtoDAsync_v2", CharSet = CharSet.Ansi)]
        static extern CUresult cuMemcpyDtoDAsync(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes, CUstream stream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostPtr"></param>
        /// <param name="devPtr"></param>
        /// <param name="sizeInBytes"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static CUresult MemcpyDtoDAsync(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes, CUstream stream)
        {
            return cuMemcpyDtoDAsync(hostPtr, devPtr, sizeInBytes, stream);
        }

        /// <summary>
        /// copy data async in stream 
        /// </summary>
        /// <param name="hostPtr"></param>
        /// <param name="devPtr"></param>
        /// <param name="sizeInBytes"></param>
        /// <param name="kind">supported : DtoD, DtoH, HtoD</param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static CUresult MemcpyAsync(IntPtr hostPtr, IntPtr devPtr, size_t sizeInBytes, cudaMemcpyKind kind, CUstream stream)
        {
            switch(kind)
            {
                case cudaMemcpyKind.cudaMemcpyDeviceToDevice:
                    return cuMemcpyDtoDAsync(hostPtr, devPtr, sizeInBytes, stream);
                case cudaMemcpyKind.cudaMemcpyHostToDevice:
                    return cuMemcpyHtoDAsync(hostPtr, devPtr, sizeInBytes, stream);
                case cudaMemcpyKind.cudaMemcpyDeviceToHost:
                    return cuMemcpyDtoHAsync(hostPtr, devPtr, sizeInBytes, stream);
                default:
                    throw new NotImplementedException();
            }
        }

        [DllImport(dllPath, EntryPoint = "cuMemsetD8Async", CharSet = CharSet.Ansi)]
        static extern CUresult cuMemsetD8Async(IntPtr devPtr, byte val, size_t numElements, CUstream stream);

        /// <summary>
        /// 
        /// </summary>
        public static CUresult MemsetD8Async(IntPtr devPtr, byte val, size_t numElements, CUstream stream)
        {
            return cuMemsetD8Async(devPtr, val, numElements, stream);
        }
        #endregion

        #region error management

        [DllImport(dllPath, CharSet = CharSet.Ansi, EntryPoint = "cuGetErrorName")]
        static extern CUresult cuGetErrorName(CUresult err, out IntPtr result);

        /// <summary>
        /// Gets the string representation of an error code enum name. 
        /// </summary>
        /// <param name="err">Error code to convert to string</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CUresult GetErrorName(CUresult err, out string result)
        {
            IntPtr tmp;
            var err2 = cuGetErrorName(err, out tmp);
            result = Marshal.PtrToStringAnsi(tmp);
            return err2;
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi, EntryPoint = "cuGetErrorString")]
        static extern CUresult cuGetErrorString(CUresult err, out IntPtr result);

        /// <summary>
        /// Gets the string description of an error code
        /// </summary>
        /// <param name="err">Error code to convert to string</param>
        /// <param name="result">Address of the string pointer.</param>
        /// <returns></returns>
        public static CUresult GetErrorString(CUresult err, out string result)
        {
            IntPtr tmp;
            var err2 = cuGetErrorString(err, out tmp);
            result = Marshal.PtrToStringAnsi(tmp);
            return err2;
        }

        #endregion

        #region stream management
        [DllImport(dllPath, CharSet = CharSet.Ansi)]
        static extern CUresult cuStreamCreate(out CUstream stream, CUstream_flags flags);

        /// <summary>
        /// Create a stream. 
        /// </summary>
        /// <param name="stream">Returned newly created stream</param>
        /// <param name="flags">Parameters for stream creation</param>
        /// <returns></returns>
        public static CUresult StreamCreate(out CUstream stream, CUstream_flags flags)
        {
            return cuStreamCreate(out stream, flags);
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi)]
        static extern CUresult cuStreamDestroy(CUstream stream);

        /// <summary>
        /// Destroys a stream. 
        /// </summary>
        /// <param name="stream">stream</param>
        /// <returns></returns>
        public static CUresult StreamDestroy(CUstream stream)
        {
            return cuStreamDestroy(stream);
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi)]
        static extern CUresult cuStreamSynchronize(CUstream stream);

        /// <summary>
        /// Sync a stream. 
        /// </summary>
        /// <param name="stream">stream</param>
        /// <returns></returns>
        public static CUresult StreamSynchronize(CUstream stream)
        {
            return cuStreamSynchronize(stream);
        }
        #endregion

        #region event management
        [DllImport(dllPath, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        static extern CUresult cuEventCreate(out CUevent evt, CUevent_flags flags);

        /// <summary>
        /// create event
        /// </summary>
        public static CUresult EventCreate(out CUevent evt, CUevent_flags flags = CUevent_flags.CU_EVENT_DEFAULT)
        {
            return cuEventCreate(out evt, flags);
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        static extern CUresult cuEventDestroy(CUevent evt);

        /// <summary>
        /// destroy event
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public static CUresult EventDestroy(CUevent evt)
        {
            return cuEventDestroy(evt);
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        static extern CUresult cuEventSynchronize(CUevent evt);

        /// <summary>
        /// sync event
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public static CUresult EventSynchronize(CUevent evt)
        {
            return cuEventSynchronize(evt);
        }

        [DllImport(dllPath, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        static extern CUresult cuEventRecord(CUevent evt, CUstream stream);

        /// <summary>
        /// sync event
        /// </summary>
        public static CUresult EventRecord(CUevent evt, CUstream stream)
        {
            return cuEventRecord(evt, stream);
        }
        #endregion
    }
}
