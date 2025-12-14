using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Altimesh.Hybridizer.Runtime;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// This class allows to call hybridized methods without explicitly declaring native methods
    /// Usage: int res = HybRunner.Cuda(target).Distrib(gridDimX, blockDimX).methodName(args);
    /// </summary>
    public class HybRunner
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate string GetExceptionTypeNameDel(int errorCode);

        private delegate void ExceptionCallback(int code);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void RegisterCallbackDel(ExceptionCallback fn, cudaStream_t stream);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CheckExceptionsDel(cudaStream_t stream);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate HybridizerProperties HybridizerGetProperties();

        private delegate IntPtr MarshalManagedToNativeDel(object p);
        private delegate void CleanUpManagedDataDel(object p);
        private delegate HandleDelegate GetHandleDelegateDel(Delegate p);
        private delegate bool GetCleanUpManagedDataDel();


        private GetExceptionTypeNameDel _GetExceptionTypeNameDel;
        private RegisterCallbackDel _RegisterCallbackDel;
        private CheckExceptionsDel _CheckExceptionsDel;

        Type GetExceptionType(int errorCode)
        {
            switch (errorCode)
            {
                case -2:
                    return typeof(IndexOutOfRangeException);
                case -1:
                    return typeof(NullReferenceException);
                case 0:
                    return typeof(Exception);
                default:
                    string name = _GetExceptionTypeNameDel(errorCode);
                    return NamingTools.QualifiedType(name, null);
            }
        }

        void HandleException(int errorCode)
        {
            Type exType = GetExceptionType(errorCode);
            if (exType != null)
            {
                throw exType.GetConstructor(new Type[] {typeof(string)}).Invoke(new object[] {"Native exception of type " + errorCode}) as Exception;
            }
            throw new ApplicationException("Unknown native exception of type " + errorCode);
        }

        /// <summary>
        /// Check exceptions
        /// </summary>
        public void CheckExceptions()
        {
            if (_CheckExceptionsDel != null)
                _CheckExceptionsDel(_stream);
            else
                throw new ApplicationException("Exception support not properly setup - exceptions are not available");
        }

        private int _blockDimX = 1;
        private int _blockDimY = 1;
        private int _blockDimZ = 1;
        private int _gridDimX = 1;
        private int _gridDimY = 1;
        private int _shared;
        private IntPtr _dllPtr = IntPtr.Zero;
        private Stopwatch _stopWatchLastKernel = new Stopwatch();
        private HybridizerProperties _props;

        private cudaStream_t _stream = cudaStream_t.NO_STREAM;

        private readonly string _dllName;
        private readonly HybridizerFlavor _flavor = HybridizerFlavor.CUDA;

        private readonly AbstractNativeMarshaler _marshaller;

        #region Marshaller methods
        static MethodInfo marshalManagedToNative;
        static MethodInfo getHandleDelegate;
        static MethodInfo cleanUpManagedData;
        static MethodInfo getCleanUpManagedData;

        private void initDelegates()
        {
            MarshalManagedToNativeDel marshallDel = _marshaller.MarshalManagedToNative;
            CleanUpManagedDataDel cleanupDel = _marshaller.CleanUpManagedData;
            GetHandleDelegateDel getHandleDelegateDel = _marshaller.GetHandleDelegate;
            GetCleanUpManagedDataDel GetCleanUpManagedDataDel = _marshaller.IsCleanUpNativeData;

            marshalManagedToNative = marshallDel.Method;
            getHandleDelegate = getHandleDelegateDel.Method;
            cleanUpManagedData = cleanupDel.Method;
            getCleanUpManagedData = GetCleanUpManagedDataDel.Method;
        }
        #endregion
        

        AssemblyBuilder assembly;
        ModuleBuilder module;
        Dictionary<HybridizerFlavor, Dictionary<Type, Type>> wrappedTypes = new Dictionary<HybridizerFlavor, Dictionary<Type, Type>>();

        Type hyb_occupancy_del;
        MethodBuilder hyb_occupancy_del_invoke;
        
        private HybRunner(string dllName, HybridizerFlavor flavor, AbstractNativeMarshaler marshaller)
        {
            assembly = GetAssemblyBuilder("__Hybridizer_HybRunner_WrappedTypes");
            module = assembly.DefineDynamicModule("__Hybridizer_HybRunner_WrappedTypes.mod");
            // default dll name may be provided in assembly - if not exception
            if (dllName == null)
            {
                string defaultName = null ;
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Attribute att in asm.GetCustomAttributes(true))
                    {
                        if (typeof(HybRunnerDefaultSatelliteNameAttribute).GUID.Equals(att.GetType().GUID))
                        {
                            HybRunnerDefaultSatelliteNameAttribute name = att as HybRunnerDefaultSatelliteNameAttribute;
                            defaultName = name.Name;
                        }
                    }
                    if (defaultName != null)
                        break;
                }
                if (defaultName == null)
                    throw new ApplicationException("Cannot create an instance of HybRunner with parameterless constructor without HybRunnerDefaultSatelliteNameAttribute set");

                dllName = defaultName;
            }

            _dllName = dllName;
            _marshaller = marshaller;
            initDelegates();
            if (!File.Exists(dllName))
                Console.WriteLine("{0} does not exist", dllName);
            _dllPtr = KernelInteropTools.LoadLibrary(dllName);
            if (_dllPtr == IntPtr.Zero)
                throw new ApplicationException("Cannot load dll " + dllName);
            IntPtr procAddress = KernelInteropTools.GetProcAddress(_dllPtr, NativePtrConverter.NativeFunctionHybridizerGetProperties);

            if (procAddress == IntPtr.Zero)
                throw new ApplicationException("missing properties method " + dllName);
            HybridizerGetProperties d =
                (HybridizerGetProperties)
                    Marshal.GetDelegateForFunctionPointer(procAddress, typeof(HybridizerGetProperties));
            _props = d.Invoke();

            if (!_marshaller.RegisterDLL(dllName))
                throw new ApplicationException("dll not found " + dllName);
            _flavor = flavor;
            if (flavor == HybridizerFlavor.AVX || flavor == HybridizerFlavor.AVX512 || flavor == HybridizerFlavor.PHI)
                _blockDimX = 32;
            // deactivated as registries are a mess with different version of dotnet 
            //if (flavor == HybridizerFlavor.CUDA && TdrDetection.IsTdrEnabled() && TdrDetection.TdrDelay() > 0)
            //{
            //    Console.Out.WriteLine("[WARNING] : TDR mode is activated with a {0} seconds delay. Kernels taking more than that will timeout and driver will recover", TdrDetection.TdrDelay());
            //}
        }

        /// <summary>
        /// registers an an additional dll to marshaller
        /// </summary>
        /// <param name="fileName"></param>
        public void RegisterAdditionalDLL(string fileName)
        {
            _marshaller.RegisterDLL(fileName);
        }

        #region properties
        /// <summary>
        /// block dimension X
        /// </summary>
        public int BlockDimX
        {
            get { return _blockDimX; }
        }

        /// <summary>
        /// block dimension Y
        /// </summary>
        public int BlockDimY
        {
            get { return _blockDimY; }
        }

        /// <summary>
        /// block dimension Z
        /// </summary>
        public int BlockDimZ
        {
            get { return _blockDimZ; }
        }

        /// <summary>
        /// grid dimension X
        /// </summary>
        public int GridDimX
        {
            get { return _gridDimX; }
        }

        /// <summary>
        /// grid dimension Y
        /// </summary>
        public int GridDimY
        {
            get { return _gridDimY; }
        }

        /// <summary>
        /// Amount of shared memory
        /// </summary>
        public int Shared
        {
            get { return _shared; }
            set { _shared = value; }
        }

        /// <summary>
        /// Stream identifier
        /// </summary>
        public cudaStream_t Stream
        {
            get { return _stream; }
            set { _stream = value; }
        }

        /// <summary>
        /// Is using non-default stream?
        /// </summary>
        public bool UseStream
        {
            get { return _flavor == HybridizerFlavor.CUDA && !cudaStream_t.NO_STREAM.Equals(_stream); }
        }

        /// <summary>
        /// Does runner use grid synchronization (modern devices only)
        /// </summary>
        public bool UseGridSync { get; set; }

        /// <summary>
        /// sets grid sync to true or false
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">thrown if cuda version is &lt; 9.0</exception>
        public HybRunner SetGridSync(bool value)
        {
            if(value && int.Parse(GetCudaVersion()) < 90)
            {
                throw new ApplicationException("Grid synchronization is only available from CUDA 9.0");
            }

            UseGridSync = value;
            return this;
        }

        /// <summary>
        /// Gets CUDA version from app.config
        /// </summary>
        /// <returns></returns>
        public static string GetCudaVersion()
        {
            // If not, get the version configured in app.config
            string cudaVersion = cuda.GetCudaVersion();

            // Otherwise default to latest version
            if (cudaVersion == null) cudaVersion = "80";
            cudaVersion = cudaVersion.Replace(".", ""); // Remove all dots ("7.5" will be understood "75")

            return cudaVersion;
        }

        /// <summary>
        /// Marshaller attached
        /// </summary>
        public AbstractNativeMarshaler Marshaller
        {
            get { return _marshaller; }
        }

        /// <summary>
        /// Last kernel duration (from launch to synchronize)
        /// </summary>
        public Stopwatch LastKernelDuration { get { return _stopWatchLastKernel; } }

        #endregion

        private dynamic Native(object o)
        {
            var t = GetCachedWrappedType(o.GetType());
            var constructor = t.GetConstructor(new[] { typeof(HybRunner), typeof(object) });
            if (constructor != null)
            {
                object wrapped = constructor.Invoke(new[] { this, o });
                return wrapped;
            }
            throw new ApplicationException("Cannot find constructor");
        }

        private Type GetCachedWrappedType(Type originalType)
        {
            Type t;
            lock (wrappedTypes)
            {
                if (!wrappedTypes.ContainsKey(_flavor))
                    wrappedTypes[_flavor] = new Dictionary<Type, Type>();
                if (!wrappedTypes[_flavor].TryGetValue(originalType, out t))
                {
                    TypeBuilder tb = GenerateWrappedType(originalType);
                    t = tb.CreateTypeInfo();
                    wrappedTypes[_flavor][originalType] = t;
                }
            }
            return t;
        }

        /// <summary>
        /// Allows to call CUDA implementations of an object's entrypoints
        /// </summary>
        /// <param name="o">Object to be wrapped</param>
        /// <param name="dllName">name of the DLL containing the native method</param>
        /// <returns>A dynamic proxy to the object</returns>
        public static dynamic Cuda(object o, string dllName)
        {
            return Cuda(dllName).Native(o);
        }

        /// <summary>
        /// Wraps a dll into an HybRunner using CUDA, allowing to further set the work distribution parameter (see SetDistrib)
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static HybRunner Cuda(string dllName = null)
        {
            var hybRunner = new HybRunner(dllName, HybridizerFlavor.CUDA, CudaMarshaler.Instance);
            hybRunner.SetDistrib(1, 1, 128, 1, 1, 0);
            cudaDeviceProp prop;
            int deviceId;
            if (cuda.GetDevice(out deviceId) != cudaError_t.cudaSuccess)
            {
                // TODO: abort?
            }
            else
            {
                if (cuda.GetDeviceProperties(out prop, deviceId) != cudaError_t.cudaSuccess)
                {
                    Console.Error.WriteLine("[WARNING] cannot get device properties for device {0}", deviceId);
                    Console.Error.WriteLine("[WARNING] keeping default grid configuration as 1x128");
                }
                else
                {
                    hybRunner.SetDistrib(16 * prop.multiProcessorCount, 128);
                }
            }

            hybRunner.RegisterExceptionHandling();
            return hybRunner;
        }

        /// <summary>
        /// Wraps a dll into an HybRunner using CUDA flavor
        /// </summary>
        public static HybRunner Cuda(string dllName, cudaStream_t stream, CudaMarshaler marshaler)
        {
            var hybRunner = new HybRunner(dllName, HybridizerFlavor.CUDA, marshaler);
            hybRunner._stream = stream;
            hybRunner.RegisterExceptionHandling();
            return hybRunner;
        }

        private void RegisterExceptionHandling()
        {
            // Handling of exceptions, register callback
            IntPtr procAddress2 = KernelInteropTools.GetProcAddress(_dllPtr, NativePtrConverter.NativeFunctionGetTypeFromId);
            if (procAddress2 != IntPtr.Zero)
                _GetExceptionTypeNameDel = (GetExceptionTypeNameDel) Marshal.GetDelegateForFunctionPointer(procAddress2, typeof(GetExceptionTypeNameDel));
            IntPtr procAddress3 = KernelInteropTools.GetProcAddress(_dllPtr, "__hybridizer_register_exception_callback");
            if (procAddress3 != IntPtr.Zero)
                _RegisterCallbackDel = (RegisterCallbackDel) Marshal.GetDelegateForFunctionPointer(procAddress3, typeof(RegisterCallbackDel));
            if (_RegisterCallbackDel != null)
                _RegisterCallbackDel(this.HandleException,  _stream);
            IntPtr procAddress4 = KernelInteropTools.GetProcAddress(_dllPtr, "__hybridizer_checkexceptions");
            if (procAddress4 != IntPtr.Zero)
                _CheckExceptionsDel = (CheckExceptionsDel) Marshal.GetDelegateForFunctionPointer(procAddress4, typeof(CheckExceptionsDel));
        }

        /// <summary>
        /// Allows to call OMP implementations of an object's entrypoints
        /// </summary>
        /// <param name="o">Object to be wrapped</param>
        /// <param name="dllName">name of the DLL containing the native method</param>
        /// <returns>A dynamic proxy to the object</returns>
        public static dynamic OMP(object o, string dllName)
        {
            return OMP(dllName).Native(o);
        }

        /// <summary>
        /// Wraps a dll into an HybRunner using OMP flavor
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static HybRunner OMP(string dllName)
        {
            var result = new HybRunner(dllName, HybridizerFlavor.OMP, MainMemoryMarshaler.Create(HybridizerFlavor.OMP));
            result.SetDistrib(1, Environment.ProcessorCount);
            return result;
        }

        /// <summary>
        /// Allows to call AVX implementations of an object's entrypoints
        /// </summary>
        /// <param name="o">Object to be wrapped</param>
        /// <param name="dllName">name of the DLL containing the native method</param>
        /// <returns>A dynamic proxy to the object</returns>
        public static dynamic AVX(object o, string dllName)
        {
            return AVX(dllName).Native(o);
        }

        /// <summary>
        /// Wraps a dll into an HybRunner using AVX flavor
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static HybRunner AVX(string dllName = null)
        {
            return new HybRunner(dllName, HybridizerFlavor.AVX, MainMemoryMarshaler.Create(HybridizerFlavor.AVX));
        }

        /// <summary>
        /// Wraps a dll into an HybRunner using AVX512 flavor
        /// </summary>
        /// <param name="o"></param>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static dynamic AVX512(object o, string dllName)
        {
            return AVX512(dllName).Native(o);
        }

        /// <summary>
        /// Wraps a dll into an HybRunner using AVX512 flavor
        /// </summary>
        /// <param name="dllName"></param>
        /// <returns></returns>
        public static HybRunner AVX512(string dllName = null)
        {
            // keep avx here
            return new HybRunner(dllName, HybridizerFlavor.AVX, MainMemoryMarshaler.Create(HybridizerFlavor.AVX));
        }

		/// <summary>
		/// Instanciates a HybRunner with a flavor from an environment variable (%HYBRIDIZER_FLAVOR%) [CUDA if not present]
		/// possible values for %HYBRIDIZER_FLAVOR% : [CUDA, AVX, AVX2, AVX512, AUTOCPU]
		/// </summary>
		/// <returns>HybRunner instance</returns>
		public static HybRunner Env()
		{
			string env = Environment.GetEnvironmentVariable("HYBRIDIZER_FLAVOR");
			if(String.IsNullOrWhiteSpace(env))
			{
				env = "CUDA";
			}

			env = env.ToUpperInvariant();
			if (env == "AUTOCPU")
				return HybRunner.AutoCPU();
			foreach (string library in Directory.GetFiles(Environment.CurrentDirectory, "*.dll", SearchOption.TopDirectoryOnly))
			{
				if (library.EndsWith("_" + env + ".dll"))
				{
					if (env == "CUDA")
						return HybRunner.Cuda(library);
					else
						return HybRunner.AVX(library).SetDistrib(Environment.ProcessorCount, 32);
				}
			}

			throw new FileNotFoundException("cannot find generated library for flavor : " + env);
		}

        /// <summary>
        /// expects generated cudadll to be the only _CUDA.dll in the executing directory, which should be by default
        /// </summary>
        /// <returns></returns>
        public static HybRunner AutoCuda()
        {
            try {
                var dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
                
                foreach(var file in dir.GetFiles("*_CUDA.dll", SearchOption.TopDirectoryOnly))
                {
                    Console.WriteLine($"Found {file} as candidate for generated dll -- try to load");
                    return Cuda(file.FullName);
                }
            } 
            catch {}
            finally {
                throw new ApplicationException("cannot find any candidate generated CUDA dll -- specify path explicitely");
            }
        }

        /// <summary>
        /// Automatically detects processor features (flags) to load the appropriate satellite dll
        /// satellite dlls must have a name ending with the flavor (AVX/AVX2/AVX512)
        /// LINUX ONLY
        /// </summary>
        /// <returns>The appropriate HybRunner</returns>
        public static HybRunner AutoCPU()
        {
            if (!KernelInteropTools.IsLinux)
            {
                throw new NotImplementedException("auto cpu hybrunner is not yet implemented on windows");
            }

            string cpuInfo = LinuxKernelInteropTools.ExecuteBashCommand("cat /proc/cpuinfo 2>/dev/null | grep flags | head -1");
            string suffix;
            if (cpuInfo.Contains("avx512f"))
            {
                Logger.WriteLine("DETECTED AVX512");
                suffix = "AVX512";
            }
            else if (cpuInfo.Contains("avx2"))
            {
                Logger.WriteLine("DETECTED AVX2");
                suffix = "AVX2";
            }
            else if (cpuInfo.Contains("avx"))
            {
                Logger.WriteLine("DETECTED AVX");
                suffix = "AVX";
            }
            else
            {
                throw new ApplicationException("AVX is not supported on this machine -- aborting");
            }

            foreach (string library in Directory.GetFiles(Environment.CurrentDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                if(library.EndsWith("_" + suffix + ".dll"))
                {
                    return new HybRunner(library, HybridizerFlavor.AVX, MainMemoryMarshaler.Create(HybridizerFlavor.AVX)).SetDistrib(Environment.ProcessorCount, 32);
                }
            }

            throw new ApplicationException("no suitable dll found in directory : " + Environment.CurrentDirectory);
        }

        /// <summary>
        /// Wraps an object using the current flavor
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public dynamic Wrap(object o)
        {
            return Native(o);
        }

        /// <summary>
        /// Set cuda work distribution parameters
        /// </summary>
        public HybRunner SetDistrib(int gridDimX, int blockDimX)
        {
            SetDistrib(gridDimX, 1, blockDimX, 1, 1, _shared);
            return this;
        }

        /// <summary>
        /// Set cuda work distribution parameters
        /// </summary>
        /// <param name="grid">NOTE: grid.z is ignored !</param>
        /// <param name="block"></param>
        /// <returns></returns>
        public HybRunner SetDistrib(dim3 grid, dim3 block)
        {
            return SetDistrib(grid.x, grid.y, block.x, block.y, block.z, _shared);
        }

        /// <summary>
        /// Configures a launch (CUDA-only)
        /// </summary>
        /// <param name="gridDimX"></param>
        /// <param name="gridDimY"></param>
        /// <param name="blockDimX"></param>
        /// <param name="blockDimY"></param>
        /// <param name="blockDimZ"></param>
        /// <param name="shared"></param>
        /// <returns></returns>
        public HybRunner SetDistrib(int gridDimX, int gridDimY, int blockDimX, int blockDimY, int blockDimZ, int shared)
        {   
            if (_flavor == HybridizerFlavor.AVX && (gridDimY != 1 || blockDimX != 32 || blockDimY != 1 || blockDimZ != 1))
                throw new ApplicationException("Invalid work distributions parameters");

            _gridDimX = gridDimX;
            _gridDimY = gridDimY;
            _blockDimX = blockDimX;
            _blockDimY = blockDimY;
            _blockDimZ = blockDimZ;
            _shared = shared;
            return this;
        }

        /// <summary>
        /// Sets the shared memory size parameter for a launch (CUDA-only)
        /// </summary>
        /// <param name="shared"></param>
        /// <returns></returns>
        public HybRunner SetShared(int shared)
        {
            _shared = shared;
            return this;
        }

        private class GetCustomAttribute<T> where T : Attribute, new()
        {
            public static T ConvertToAPIAttribute(object orig)
            {
                var res = new T();
                foreach (PropertyInfo pi in orig.GetType().GetProperties())
                {
                    PropertyInfo lpi = typeof(T).GetProperty(pi.Name);
                    if (lpi == null) continue; // dont fail for properties of users.
                    if (lpi == null || lpi.GetSetMethod() == null)
                        continue;
                    lpi.GetSetMethod().Invoke(res, new object[] { pi.GetGetMethod().Invoke(orig, new object[] { }) });
                }
                return res;
            }
        }

        private Type GetActionType(int paramCount)
        {
            switch(paramCount)
            {
                case 0:
                    return typeof(Action);
                case 1:
                    return typeof(Action<>);
                case 2:
                    return typeof(Action<,>);
                case 3:
                    return typeof(Action<,,>);
                case 4:
                    return typeof(Action<,,,>);
                case 5:
                    return typeof(Action<,,,,>);
                case 6:
                    return typeof(Action<,,,,,>);
                case 7:
                    return typeof(Action<,,,,,,>);
                case 8:
                    return typeof(Action<,,,,,,,>);
                case 9:
                    return typeof(Action<,,,,,,,,>);
                case 10:
                    return typeof(Action<,,,,,,,,,>);
                case 11:
                    return typeof(Action<,,,,,,,,,,>);
                case 12:
                    return typeof(Action<,,,,,,,,,,,>);
                case 13:
                    return typeof(Action<,,,,,,,,,,,,>);
                case 14:
                    return typeof(Action<,,,,,,,,,,,,,>);
                case 15:
                    return typeof(Action<,,,,,,,,,,,,,,>);
                case 16:
                    return typeof(Action<,,,,,,,,,,,,,,,>);
                default:
                    throw new ApplicationException("too many parameters for kernel");
            }
        }

        private TypeBuilder GenerateWrappedType(Type t)
        {
            TypeBuilder typeBuilder = GetType(module, t.FullName + "_wrapped_" + _flavor);
            FieldInfo runtimeField = typeBuilder.DefineField("runtime", typeof(HybRunner), FieldAttributes.Public);
            FieldInfo wrappedObject = typeBuilder.DefineField("wrapped", typeof(object), FieldAttributes.Public);

            // Create a constructor that takes as argument a HybRunner object
            Type[] cParams = { typeof(HybRunner), typeof(object) };
            ConstructorBuilder cBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public |
                                                                        MethodAttributes.HideBySig |
                                                                        MethodAttributes.SpecialName |
                                                                        MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                cParams);

            ConstructorInfo conObj = typeof(object).GetConstructor(new Type[0]);
            if (conObj == null)
                throw new ApplicationException("Constructor not found");
            ILGenerator cil = cBuilder.GetILGenerator();
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Call, conObj);
            cil.Emit(OpCodes.Nop);
            cil.Emit(OpCodes.Nop);
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Ldarg_1);
            cil.Emit(OpCodes.Stfld, runtimeField);
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Ldarg_2);
            cil.Emit(OpCodes.Stfld, wrappedObject);
            cil.Emit(OpCodes.Ret);

            // Generate SetDistrib methods
            {
                {
                    MethodBuilder mb = typeBuilder.DefineMethod("SetDistrib", MethodAttributes.Public, typeBuilder, new Type[] { typeof(int), typeof(int) });
                    ILGenerator ilgen = mb.GetILGenerator();
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, runtimeField); // load the hybrunner instance attached to the wrapper
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Ldarg_2);
                    MethodInfo mi = typeof(HybRunner).GetMethod("SetDistrib", new Type[] { typeof(int), typeof(int) });
                    ilgen.Emit(OpCodes.Call, mi); // call corresponding setdistrib method
                                                  // SetDistrib returns HybRunner => trash it
                    ilgen.Emit(OpCodes.Pop);
                    ilgen.Emit(OpCodes.Ldarg_0); ilgen.Emit(OpCodes.Ret); // return this
                }
                {
                    MethodBuilder mb = typeBuilder.DefineMethod("SetDistrib", MethodAttributes.Public, typeBuilder, new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) });
                    ILGenerator ilgen = mb.GetILGenerator();
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, runtimeField); // load the hybrunner instance attached to the wrapper
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Ldarg_2);
                    ilgen.Emit(OpCodes.Ldarg_3);
                    ilgen.Emit(OpCodes.Ldarg, 4);
                    ilgen.Emit(OpCodes.Ldarg, 5);
                    ilgen.Emit(OpCodes.Ldarg, 6);
                    MethodInfo mi = typeof(HybRunner).GetMethod("SetDistrib", new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) });
                    ilgen.Emit(OpCodes.Call, mi); // call corresponding setdistrib method
                                                  // SetDistrib returns HybRunner => trash it
                    ilgen.Emit(OpCodes.Pop);
                    ilgen.Emit(OpCodes.Ldarg_0); ilgen.Emit(OpCodes.Ret); // return this
                }
                {
                    MethodBuilder mb = typeBuilder.DefineMethod("SetDistrib", MethodAttributes.Public, typeBuilder, new Type[] { typeof(dim3), typeof(dim3) });
                    ILGenerator ilgen = mb.GetILGenerator();
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, runtimeField); // load the hybrunner instance attached to the wrapper
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Ldarg_2);
                    MethodInfo mi = typeof(HybRunner).GetMethod("SetDistrib", new Type[] { typeof(dim3), typeof(dim3) });
                    ilgen.Emit(OpCodes.Call, mi); // call corresponding setdistrib method
                                                  // SetDistrib returns HybRunner => trash it
                    ilgen.Emit(OpCodes.Pop);
                    ilgen.Emit(OpCodes.Ldarg_0); ilgen.Emit(OpCodes.Ret); // return this
                }
            }


            // Generate SetStream methods
            {
                {
                    MethodBuilder mb = typeBuilder.DefineMethod("SetStream", MethodAttributes.Public, typeBuilder, new Type[] { typeof(cudaStream_t) });
                    ILGenerator ilgen = mb.GetILGenerator();
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, runtimeField); // load the hybrunner instance attached to the wrapper
                    ilgen.Emit(OpCodes.Ldarg_1);
                    MethodInfo mi = typeof(HybRunner).GetProperty("Stream").GetSetMethod();
                    ilgen.Emit(OpCodes.Call, mi); // call corresponding setdistrib method
                                                  // SetStream returns HybRunner => trash it
                    ilgen.Emit(OpCodes.Ldarg_0); ilgen.Emit(OpCodes.Ret); // return this
                }
            }

            // Generate methods for all entrypoints
            foreach (MethodInfo mi in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                string symbol = null;
                foreach (var att in mi.GetCustomAttributes(true))
                {

                    if (att.GetType().GUID == typeof(EntryPointAttribute).GUID)
                    {
                        EntryPointAttribute epa = null;
                        epa = GetCustomAttribute<EntryPointAttribute>.ConvertToAPIAttribute(att);
                        symbol = epa.Name;
                        if (symbol == null || symbol.Length == 0)
                            symbol = CMethodMangler.Mangle(mi);
                    }

                    if (symbol == null && _props.CompatibilityMode && att.GetType().GUID == typeof(KernelAttribute).GUID
                        && mi.ReturnType == typeof(void)
                        && !mi.DeclaringType.IsGenericType
                        && (mi.IsStatic || !mi.IsVirtual)
                        &&
                        !(mi.GetParameters()
                            .Any(
                                x =>
                                    x.ParameterType.IsByRef &&
                                    (x.ParameterType.GetElementType().IsValueType ||
                                     x.ParameterType.GetElementType().IsPrimitive)))
                    )
                    {
                        symbol = NamingTools.GetEncodedSignature(mi);
                    }
                }
                if (symbol != null)
                {
                    bool supportsOmp = true && _flavor == HybridizerFlavor.AVX;

                    MethodInfo toWrap = mi;
                    List<Type> nativeParameters = new List<Type>(); // Parameters of DLL exported function
                    List<Type> wrappedParams = new List<Type>(toWrap.GetParameters().Length); // Parameters of wrapped function
                    if(_flavor == HybridizerFlavor.OMP) {
                        nativeParameters.Add(typeof(int));
                    }
                    if (_marshaller is CudaMarshaler)
                    {
                        nativeParameters.Add(typeof(int)); // griddim_x
                        nativeParameters.Add(typeof(int)); // griddim_y
                        nativeParameters.Add(typeof(int)); // blockDim_x
                        nativeParameters.Add(typeof(int)); // blockdim_y
                        nativeParameters.Add(typeof(int)); // blockdim_z
                        nativeParameters.Add(typeof(int)); // shared
                    }

                    if (!toWrap.IsStatic)
                    {
                        nativeParameters.Add(typeof(IntPtr)); // self
                    }
                    for (int i = 0; i < toWrap.GetParameters().Length; i++)
                    {
                        ParameterInfo pi = toWrap.GetParameters()[i];
                        Type pt = pi.ParameterType;
                        if (pt.HasElementType && pt.GetElementType().IsValueType && pt.IsByRef)
                        {
                            // We have a native type by reference, pass it as a single element array
                            throw new ApplicationException("Value types by reference are not supported");
                        }
                        if (IsDelegateType(pt))
                        {
                            nativeParameters.Add(typeof(HandleDelegate));
                        }
                        else if (pt.IsClass || pt.IsInterface || typeof(ICustomMarshalled).IsAssignableFrom(pt))
                        {
                            nativeParameters.Add(typeof(IntPtr));
                        }
                        else
                        {
                            nativeParameters.Add(pt);
                        }
                        wrappedParams.Add(pt);
                    }

                    MethodBuilder nativeMethod = GetNativeMethod(typeBuilder, symbol + "_ExternCWrapper_" + _flavor, nativeParameters.ToArray());
                    MethodBuilder nativeMethodStream = null;
                    MethodBuilder nativeMethodGridSync = null;
                    MethodBuilder nativeMethodStreamGridSync = null;
                    MethodBuilder nativeMethodMaxBlocksPerSM = null;
                    if (_flavor == HybridizerFlavor.CUDA)
                    {
                        List<Type> streamParameters = new List<Type>(nativeParameters.Count + 1);
                        streamParameters.AddRange(nativeParameters);
                        streamParameters.Insert(6, typeof(cudaStream_t));
                        nativeMethodStream = GetNativeMethod(typeBuilder, symbol + "_ExternCWrapperStream_" + _flavor, streamParameters.ToArray());
                        nativeMethodGridSync = GetNativeMethod(typeBuilder, symbol + "_ExternCWrapperGridSync_" + _flavor, nativeParameters.ToArray());
                        nativeMethodStreamGridSync = GetNativeMethod(typeBuilder, symbol + "_ExternCWrapperStreamGridSync_" + _flavor, streamParameters.ToArray());

                        // generate occupancy calculator method
                        nativeMethodMaxBlocksPerSM = GetNativeMethod(typeBuilder, symbol + "_OccupancyCalculator_MaxActiveBlocksPerSM", new Type[] { typeof(int).MakeByRefType(), typeof(int), typeof(int) });
                        var paramBuilder = nativeMethodMaxBlocksPerSM.DefineParameter(1, ParameterAttributes.Out, "blocksPerSM");
                        //paramBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(OutAttribute).GetConstructor(new Type[0]), new object[0]));
                        nativeMethodMaxBlocksPerSM.DefineParameter(2, ParameterAttributes.None, "threadsPerBlock");
                        nativeMethodMaxBlocksPerSM.DefineParameter(3, ParameterAttributes.None, "sharedMemSize");
                    }

                    if (nativeMethod == null) continue; // Native method not found, skipping method

                    MethodBuilder nativeMethodOMP = null;
                    if (supportsOmp)
                    {
                        List<Type> ompParameters = new List<Type>(nativeParameters.Count + 1);
                        ompParameters.Add(typeof(int));
                        ompParameters.AddRange(nativeParameters);
                        nativeMethodOMP = GetNativeMethod(typeBuilder, symbol + "_ExternCWrapper_OMP_" + _flavor, ompParameters.ToArray());
                    }

                    bool generatePtrMethod = false;
                    List<Type> wrappedParamsPtr = new List<Type>();
                    foreach (var param in wrappedParams)
                    {
                        if (param.IsClass || param.IsInterface || param.IsArray || typeof(ICustomMarshalled).IsAssignableFrom(param))
                        {
                            wrappedParamsPtr.Add(typeof(IntPtr));
                            generatePtrMethod = true;
                        }
                        else
                        {
                            wrappedParamsPtr.Add(param);
                        }
                    }


                    MethodBuilder mb = typeBuilder.DefineMethod(toWrap.Name, MethodAttributes.Public, typeof(int), wrappedParams.ToArray());
                    MethodBuilder mbPtr = null;
                    if (generatePtrMethod)
                    {
                        mbPtr = typeBuilder.DefineMethod(toWrap.Name, MethodAttributes.Public, typeof(int), wrappedParamsPtr.ToArray());
                    }

                    // Copy parameter attributes (including, for instance, IsOut)
                    for (int i = 0; i < toWrap.GetParameters().Length; i++)
                    {
                        ParameterInfo pi = toWrap.GetParameters()[i];
                        mb.DefineParameter(pi.Position + 1, pi.Attributes, pi.Name);
                        if (generatePtrMethod && mbPtr != null)
                        {
                            mbPtr.DefineParameter(pi.Position + 1, pi.Attributes, pi.Name);
                        }
                    }

                    ILGenerator generator = mb.GetILGenerator();
                    ILGenerator generatorPtr = null;
                    if (generatePtrMethod && mbPtr != null)
                    {
                        generatorPtr = mbPtr.GetILGenerator();
                    }
                    // Call native method in IL...

                    LocalBuilder stopWatch = generator.DeclareLocal(typeof(Stopwatch));

                    EmitMethod(generator, runtimeField, supportsOmp, toWrap, wrappedObject, wrappedParams, nativeMethodOMP, nativeMethod, nativeMethodStream, nativeMethodStreamGridSync, nativeMethodGridSync, true);
                    if (generatePtrMethod && mbPtr != null && generatorPtr != null)
                    {
                        EmitMethod(generatorPtr, runtimeField, supportsOmp, toWrap, wrappedObject, wrappedParamsPtr, nativeMethodOMP, nativeMethod, nativeMethodStream, nativeMethodStreamGridSync, nativeMethodGridSync, false);
                    }
                }
            }


            if (_flavor == HybridizerFlavor.CUDA)
            {
                if (hyb_occupancy_del == null)
                {
                    TypeBuilder delBuilder = module.DefineType("__hyb_occupancy_del", TypeAttributes.Class | TypeAttributes.NotPublic | TypeAttributes.Sealed | TypeAttributes.AnsiClass | TypeAttributes.AutoClass, typeof(MulticastDelegate));
                    var cb = delBuilder.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName | MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(object), typeof(IntPtr) });
                    cb.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

                    hyb_occupancy_del_invoke = delBuilder.DefineMethod("Invoke", MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, CallingConventions.Standard, typeof(void), new Type[] { typeof(int).MakeByRefType(), typeof(int), typeof(int) });
                    hyb_occupancy_del_invoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
                    hyb_occupancy_del_invoke.DefineParameter(1, ParameterAttributes.Out, "a");

                    var binvoke = delBuilder.DefineMethod("BeginInvoke", MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, CallingConventions.Standard, typeof(IAsyncResult), new Type[] { typeof(int).MakeByRefType(), typeof(int), typeof(int), typeof(AsyncCallback), typeof(object) });
                    binvoke.DefineParameter(1, ParameterAttributes.Out, "a");
                    binvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);

                    var einvoke = delBuilder.DefineMethod("EndInvoke", MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, CallingConventions.Standard, typeof(void), new Type[] { typeof(int).MakeByRefType(), typeof(IAsyncResult) });
                    einvoke.SetImplementationFlags(MethodImplAttributes.Runtime | MethodImplAttributes.Managed);
                    hyb_occupancy_del = delBuilder.CreateTypeInfo();
                }

                for (int i = 0; i < 17; ++i)
                {
                    MethodBuilder maxBlocksPerSM = typeBuilder.DefineMethod("MaxBlocksPerSM", MethodAttributes.Public);
                    maxBlocksPerSM.SetReturnType(typeof(int));
                    string[] typeParameterNames = Enumerable.Range(0, i).Select(k => "T" + k).ToArray();
                    Type paramType = typeof(Action);
                    if (i > 0)
                    {
                        var parameterBuilder = maxBlocksPerSM.DefineGenericParameters(typeParameterNames);
                        paramType = GetActionType(i).MakeGenericType(parameterBuilder);
                        maxBlocksPerSM.SetParameters(new Type[] { paramType, typeof(int), typeof(int) });
                    }
                    else
                    {
                        maxBlocksPerSM.SetParameters(new Type[] { typeof(Action), typeof(int), typeof(int) });
                    }
                    maxBlocksPerSM.DefineParameter(1, ParameterAttributes.None, "DotNetEntryPoint");
                    maxBlocksPerSM.DefineParameter(2, ParameterAttributes.None, "threadsPerBlock");
                    maxBlocksPerSM.DefineParameter(3, ParameterAttributes.None, "sharedMemSize");
                    ILGenerator ilgen = maxBlocksPerSM.GetILGenerator();

                    var actionInfo = ilgen.DeclareLocal(typeof(MethodInfo));
                    var symbolInfo = ilgen.DeclareLocal(typeof(string));
                    var fullName = ilgen.DeclareLocal(typeof(string));
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Callvirt, typeof(Delegate).GetProperty("Method").GetGetMethod());
                    // TODO: handle name attributes on entry points

                    ilgen.Emit(OpCodes.Call, typeof(CMethodMangler).GetMethod("Mangle", new Type[] { typeof(MethodBase) }));
                    ilgen.Emit(OpCodes.Stloc, symbolInfo);

                    LocalBuilder dllHandle = ilgen.DeclareLocal(typeof(IntPtr));
                    LocalBuilder procHandle = ilgen.DeclareLocal(typeof(IntPtr));
                    ilgen.Emit(OpCodes.Ldstr, _dllName);
                    ilgen.Emit(OpCodes.Call, typeof(KernelInteropTools).GetMethod("LoadLibrary"));
                    ilgen.Emit(OpCodes.Stloc, dllHandle);

                    var delInfo = ilgen.DeclareLocal(hyb_occupancy_del);

                    ilgen.Emit(OpCodes.Ldstr, "{0}{1}");
                    ilgen.Emit(OpCodes.Ldloc, symbolInfo);
                    ilgen.Emit(OpCodes.Ldstr, "_OccupancyCalculator_MaxActiveBlocksPerSM");
                    ilgen.Emit(OpCodes.Call, typeof(string).GetMethod("Format", new Type[] { typeof(string), typeof(object), typeof(object) }));
                    ilgen.Emit(OpCodes.Stloc, fullName);

                    ilgen.Emit(OpCodes.Ldloc, dllHandle);
                    ilgen.Emit(OpCodes.Ldloc, fullName);
                    ilgen.Emit(OpCodes.Call, typeof(KernelInteropTools).GetMethod("GetProcAddress"));
                    ilgen.Emit(OpCodes.Stloc, procHandle);

                    ilgen.Emit(OpCodes.Ldloc, procHandle);
                    ilgen.Emit(OpCodes.Ldtoken, hyb_occupancy_del);
                    ilgen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
                    ilgen.Emit(OpCodes.Call, typeof(Marshal).GetMethod("GetDelegateForFunctionPointer", new Type[] { typeof(IntPtr), typeof(Type) }));
                    ilgen.Emit(OpCodes.Castclass, hyb_occupancy_del);
                    ilgen.Emit(OpCodes.Stloc, delInfo);

                    var result = ilgen.DeclareLocal(typeof(int));
                    ilgen.Emit(OpCodes.Ldloc, delInfo);
                    ilgen.Emit(OpCodes.Ldloca, result);
                    ilgen.Emit(OpCodes.Ldarg_2);
                    ilgen.Emit(OpCodes.Ldarg_3);
                    ilgen.Emit(OpCodes.Callvirt, hyb_occupancy_del_invoke);
                    ilgen.Emit(OpCodes.Ldloc, result);
                    ilgen.Emit(OpCodes.Ret);
                }
            }

            return typeBuilder;
        }

        private void EmitMethod(ILGenerator ilgen, FieldInfo runtimeField, bool supportsOmp, MethodInfo toWrap,
            FieldInfo wrappedObject, List<Type> wrappedParams, MethodBuilder nativeMetodOMP, 
            MethodBuilder nativeMetod, MethodBuilder nativeMethodStream, MethodBuilder nativeMethodStreamGridSync, MethodBuilder nativeMethodGridSync, 
            bool managedFallback)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            MethodInfo marshallerGetter = typeof(HybRunner).GetProperty("Marshaller", bindingFlags).GetGetMethod();
            Label endOfMethod = ilgen.DefineLabel();
            LocalBuilder result = ilgen.DeclareLocal(typeof(int));


            if (_marshaller is CudaMarshaler && managedFallback)
            {
                Label cudaLabel = ilgen.DefineLabel();
                ilgen.Emit(OpCodes.Call, typeof(cuda).GetMethod("IsCudaAvailable"));
                ilgen.Emit(OpCodes.Brtrue, cudaLabel);

                ilgen.EmitWriteLine("[WARNING] : no CUDA device detected -- running .Net code instead");

                int argIdx = 0;
                if (!toWrap.IsStatic)
                {
                    ilgen.Emit(OpCodes.Ldarg_0);
                }

                argIdx++;
                foreach (var t in wrappedParams)
                {
                    ilgen.Emit(OpCodes.Ldarg, argIdx++);
                }

                ilgen.Emit(OpCodes.Call, toWrap);
                ilgen.Emit(OpCodes.Ldc_I4_0);
                ilgen.Emit(OpCodes.Stloc, result);
                ilgen.Emit(OpCodes.Br, endOfMethod);

                ilgen.MarkLabel(cudaLabel);
            }
            
            if (supportsOmp)
            {
                LocalBuilder isOmpCall = ilgen.DeclareLocal(typeof(bool));
                Label noOmp = ilgen.DefineLabel();
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("GridDimX", bindingFlags).GetGetMethod());
                ilgen.Emit(OpCodes.Ldc_I4_1);
                ilgen.Emit(OpCodes.Cgt); // gridDimX, bool
                ilgen.Emit(OpCodes.Stloc, isOmpCall);
                ilgen.Emit(OpCodes.Ldloc, isOmpCall);
                ilgen.Emit(OpCodes.Brfalse, noOmp);

                // Emitting the omp version of the call
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call,
                    typeof(HybRunner).GetProperty("GridDimX", bindingFlags).GetGetMethod());
                AddParametersToStack(toWrap, ilgen, runtimeField, marshallerGetter, wrappedObject, wrappedParams);
                ilgen.Emit(OpCodes.Call, nativeMetodOMP);
                ilgen.Emit(OpCodes.Stloc, result);
                var endOfCall = ilgen.DefineLabel();
                ilgen.Emit(OpCodes.Br, endOfCall);

                ilgen.MarkLabel(noOmp);
                // The non OMP version of the call
                _stopWatchLastKernel.Start();

                CallNativeMethod(ilgen, runtimeField, bindingFlags, toWrap, 
                                marshallerGetter, wrappedObject, wrappedParams, 
                                nativeMetod, nativeMethodStream, nativeMethodStreamGridSync, nativeMethodGridSync, result);
                ilgen.MarkLabel(endOfCall);
            }
            else
            {
                CallNativeMethod(ilgen, runtimeField, bindingFlags, toWrap, 
                    marshallerGetter, wrappedObject, wrappedParams, 
                    nativeMetod, nativeMethodStream, nativeMethodStreamGridSync, nativeMethodGridSync, result);
            }

            {

                Label noCleanup = ilgen.DefineLabel();
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, marshallerGetter);
                ilgen.Emit(OpCodes.Call, getCleanUpManagedData);
                ilgen.Emit(OpCodes.Brfalse, noCleanup);
                // Cleanup - unmarshall parameters
                int argIdx = 0;
                if (!toWrap.IsStatic)
                {
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, runtimeField);
                    ilgen.Emit(OpCodes.Call, marshallerGetter);
                    ilgen.Emit(OpCodes.Ldarg_0); // Load this
                    ilgen.Emit(OpCodes.Ldfld, wrappedObject); // Unwrap
                    ilgen.Emit(OpCodes.Call, cleanUpManagedData);
                }
                argIdx++;

                foreach (Type pi in wrappedParams)
                {
                    if (pi.IsClass || pi.IsInterface)
                    {
                        // TODO: support manual cleanup here
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Ldfld, runtimeField);
                        ilgen.Emit(OpCodes.Call, marshallerGetter);
                        ilgen.Emit(OpCodes.Ldarg, argIdx);
                        ilgen.Emit(OpCodes.Call, cleanUpManagedData);
                    }
                    argIdx++;
                }

                ilgen.MarkLabel(noCleanup);
            }

            ilgen.MarkLabel(endOfMethod);
            ilgen.Emit(OpCodes.Ldloc, result);
            ilgen.Emit(OpCodes.Ret);
        }

        private void LoadCommonParameters(ILGenerator ilgen, FieldInfo runtimeField, BindingFlags bindingFlags)
        {
            if (_marshaller is CudaMarshaler)
            {
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("GridDimX", bindingFlags).GetGetMethod());
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("GridDimY", bindingFlags).GetGetMethod());
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("BlockDimX", bindingFlags).GetGetMethod());
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("BlockDimY", bindingFlags).GetGetMethod());
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("BlockDimZ", bindingFlags).GetGetMethod());
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("Shared", bindingFlags).GetGetMethod());
            }
            if(_flavor == HybridizerFlavor.OMP) {
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("BlockDimX", bindingFlags).GetGetMethod());
            }
        }

        private void CallNativeMethod(ILGenerator ilgen, FieldInfo runtimeField, BindingFlags bindingFlags, MethodInfo toWrap, MethodInfo marshallerGetter, FieldInfo wrappedObject, List<Type> wrappedParams, MethodBuilder nativeMetod, MethodBuilder nativeMethodStream, MethodBuilder nativeMethodStreamGridSync, MethodBuilder nativeMethodGridSync, LocalBuilder result)
        {
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldfld, runtimeField);
            ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("LastKernelDuration", bindingFlags).GetGetMethod());
            ilgen.Emit(OpCodes.Call, typeof(Stopwatch).GetMethod("Restart"));

            Label streamLabel = ilgen.DefineLabel();
            Label streamAndGridSyncLabel = ilgen.DefineLabel();
            Label gridSyncLabel = ilgen.DefineLabel();
            Label endOfStreamRegionLabel = ilgen.DefineLabel();

            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldfld, runtimeField);
            ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("UseStream").GetGetMethod());
            ilgen.Emit(OpCodes.Brtrue, streamLabel);

            LoadCommonParameters(ilgen, runtimeField, bindingFlags);
            AddParametersToStack(toWrap, ilgen, runtimeField, marshallerGetter, wrappedObject, wrappedParams);

            if (_marshaller is CudaMarshaler)
            {
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("UseGridSync").GetGetMethod());
                ilgen.Emit(OpCodes.Brtrue, gridSyncLabel);
                ilgen.Emit(OpCodes.Call, nativeMetod);
                ilgen.Emit(OpCodes.Stloc, result);
                ilgen.Emit(OpCodes.Br, endOfStreamRegionLabel);
                ilgen.MarkLabel(gridSyncLabel);
                ilgen.Emit(OpCodes.Call, nativeMethodGridSync);
                ilgen.Emit(OpCodes.Stloc, result);
                ilgen.Emit(OpCodes.Br, endOfStreamRegionLabel);
            }
            else
            {
                ilgen.Emit(OpCodes.Call, nativeMetod);
                ilgen.Emit(OpCodes.Stloc, result);
                ilgen.Emit(OpCodes.Br, endOfStreamRegionLabel);
            }


            ilgen.MarkLabel(streamLabel);

            if (_marshaller is CudaMarshaler)
            {
                LoadCommonParameters(ilgen, runtimeField, bindingFlags);
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("Stream").GetGetMethod());
                AddParametersToStack(toWrap, ilgen, runtimeField, marshallerGetter, wrappedObject, wrappedParams);

                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldfld, runtimeField);
                ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("UseGridSync").GetGetMethod());
                ilgen.Emit(OpCodes.Brtrue, streamAndGridSyncLabel);
                ilgen.Emit(OpCodes.Call, nativeMethodStream);
                ilgen.Emit(OpCodes.Stloc, result);
                ilgen.Emit(OpCodes.Br, endOfStreamRegionLabel);
                ilgen.MarkLabel(streamAndGridSyncLabel);
                ilgen.Emit(OpCodes.Call, nativeMethodStreamGridSync);
                ilgen.Emit(OpCodes.Stloc, result);
            }

            ilgen.MarkLabel(endOfStreamRegionLabel);

            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldfld, runtimeField);
            ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("LastKernelDuration", bindingFlags).GetGetMethod());
            ilgen.Emit(OpCodes.Call, typeof(Stopwatch).GetMethod("Stop"));
            // deactivated as registries are a mess with new dotnet versions (standard)
            //if (_marshaller is CudaMarshaler && TdrDetection.IsTdrEnabled())
            //{
            //    Label noTimeoutLabel = ilgen.DefineLabel();
            //    ilgen.Emit(OpCodes.Ldarg_0);
            //    ilgen.Emit(OpCodes.Ldfld, runtimeField);
            //    ilgen.Emit(OpCodes.Call, typeof(HybRunner).GetProperty("LastKernelDuration", bindingFlags).GetGetMethod());
            //    ilgen.Emit(OpCodes.Call, typeof(Stopwatch).GetProperty("ElapsedMilliseconds", bindingFlags).GetGetMethod());
            //    ilgen.Emit(OpCodes.Conv_R8);
            //    ilgen.Emit(OpCodes.Ldc_R8, TdrDetection.TdrDelay() * 1000.0);
            //    ilgen.Emit(OpCodes.Cgt);
            //    LocalBuilder tmp = ilgen.DeclareLocal(typeof(bool));
            //    ilgen.Emit(OpCodes.Stloc, tmp);
            //    ilgen.Emit(OpCodes.Ldloc, tmp);
            //    ilgen.Emit(OpCodes.Brfalse, noTimeoutLabel);
            //    ilgen.EmitWriteLine(String.Format("[WARNING] Last kernel took more than TDR delay : {0} seconds", TdrDetection.TdrDelay()));
            //    ilgen.MarkLabel(noTimeoutLabel);
            //}
        }

        private static bool IsDelegateType(Type type)
        {
            return typeof(Delegate).IsAssignableFrom(type);
        }

        private static void AddParametersToStack(MethodInfo toWrap, ILGenerator generator, FieldInfo runtimeField,
            MethodInfo marshallerGetter, FieldInfo wrappedObject, List<Type> wrappedParams)
        {
            int argIdx = 0;
            // marshal this
            if (!toWrap.IsStatic)
            {
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldfld, runtimeField);
                generator.Emit(OpCodes.Call, marshallerGetter);
                generator.Emit(OpCodes.Ldarg_0); // Load this
                generator.Emit(OpCodes.Ldfld, wrappedObject); // Unwrap
                generator.Emit(OpCodes.Call, marshalManagedToNative);
            }
            argIdx++;

            foreach (Type parameterType in wrappedParams)
            {
                if (IsDelegateType(parameterType))
                {
                    // Convert to HandleDelegate
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, runtimeField);
                    generator.Emit(OpCodes.Call, marshallerGetter);
                    generator.Emit(OpCodes.Ldarg, argIdx++);
                    generator.Emit(OpCodes.Call, getHandleDelegate);
                }
                else if (parameterType.IsClass || parameterType.IsInterface ||
                    (typeof(ICustomMarshalled).IsAssignableFrom(parameterType) && parameterType.IsValueType)
                    )
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, runtimeField);
                    generator.Emit(OpCodes.Call, marshallerGetter);
                    generator.Emit(OpCodes.Ldarg, argIdx++);
                    generator.Emit(OpCodes.Call, marshalManagedToNative);
                }
                else
                {
                    generator.Emit(OpCodes.Ldarg, argIdx++);
                }
            }
        }

        private static AssemblyBuilder GetAssemblyBuilder(string assemblyName)
        {
            AssemblyName aname = new AssemblyName(assemblyName);
            AppDomain currentDomain = AppDomain.CurrentDomain; // Thread.GetDomain();
            AssemblyBuilder builder = AssemblyBuilder.DefineDynamicAssembly(aname, AssemblyBuilderAccess.Run);
            return builder;
        }

        private static ModuleBuilder GetModule(AssemblyBuilder asmBuilder)
        {
            return asmBuilder.DefineDynamicModule("HybridizerWrapped");
        }

        private static TypeBuilder GetType(ModuleBuilder modBuilder, string className)
        {
            return modBuilder.DefineType(className, TypeAttributes.Public);
        }

        private MethodBuilder GetNativeMethod(TypeBuilder tb, String methodName, Type[] parameters)
        {
            //if (KernelInteropTools.GetProcAddress(_dllPtr, methodName) == IntPtr.Zero) 
            //    return null;

            MethodAttributes methodAttributes = MethodAttributes.Public | MethodAttributes.PinvokeImpl | MethodAttributes.Static;
            string symbolName = methodName;

            if (cuda.s_VERBOSITY == cuda.VERBOSITY.Verbose)
                Console.WriteLine("Mapped " + symbolName);

            CallingConventions callingConvention = CallingConventions.Standard;
            MethodBuilder result = tb.DefineMethod(symbolName, methodAttributes, callingConvention, typeof(int), parameters);
            var attrType = typeof(DllImportAttribute);
            var attrBuilder = new CustomAttributeBuilder(attrType.GetConstructor(new Type[] { typeof(string) }),
                new object[1] { _dllName });
                //new[]
                //{
                //    attrType.GetProperty("CallingConvention"),
                //    attrType.GetProperty("CharSet")
                //},
                //new object[]
                //{
                //    CallingConvention.Cdecl,
                //    CharSet.Ansi
                //});
            result.SetCustomAttribute(attrBuilder);
            return result;
        }
    }
}