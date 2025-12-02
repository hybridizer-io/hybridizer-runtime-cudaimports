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
    /// <summary>
    /// Handle conversion of managed code types (via the fullname) to native typeIds
    /// </summary>
    internal class NativePtrConverter
    {
        public const string NativeFunctionGetType = "HybridizerGetTypeID";
        public const string NativeFunctionGetTypeFromId = "HybridizerGetTypeFromID";
        public const string NativeFunctionGetShallowSize = "HybridizerGetShallowSize";
        public const string HybridizerCopyToSymbol = "HybridizerCopyToSymbol";        
        public const string NativeFunctionHybridizerGetProperties = "__HybridizerGetProperties";
        public const string NativeFunctionHybridizerGetFunctionPointer = "HybridizerGetFunctionPointer";
        public const string NativeFunctionHybridizerGetVectorizedActionFunctionPointer = "HybridizerGetVectorizedActionFunctionPointer";


        #region private fields
        private readonly SafeDictionary<string, IntPtr> _cache = new SafeDictionary<string, IntPtr>();
        private readonly SafeDictionary<string, IntPtr> _vectorizedCache = new SafeDictionary<string, IntPtr>();
        private readonly SafeDictionary<Type, TypeInfo> _typeInfoCache = new SafeDictionary<Type, TypeInfo>();
        private HybridizedLibrary[] _libraries;
        private HybridizerFlavor _flavor;
        #endregion

        #region private methods
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int HybridizerGetTypeIdDelegate([MarshalAs(UnmanagedType.LPStr)] string fullTypeName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int HybridizerGetShallowSizeDelegate([MarshalAs(UnmanagedType.LPStr)] string fullTypeName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr HybridizerGetFunctionPointerDelegate([MarshalAs(UnmanagedType.LPStr)] string fullMethodName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr HybridizerGetVectorizedActionFunctionPointerDelegate([MarshalAs(UnmanagedType.LPStr)] string fullMethodName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr HybridizerCopyToSymbolDelegate([MarshalAs(UnmanagedType.LPStr)] string targetSymbol, IntPtr source, IntPtr size, IntPtr offset, IntPtr module);

        internal class HybridizedLibrary
        {
            public HybridizedLibrary(string dllPath)
            {
                DllPath = dllPath;
            }

            internal string DllPath { get; private set; }
            internal HybridizerGetTypeIdDelegate GetTypeId { get; set; }
            internal HybridizerGetFunctionPointerDelegate GetFunctionPointer{ get; set; }
            internal HybridizerGetVectorizedActionFunctionPointerDelegate GetVectorizedActionFunctionPointer { get; set; }
            internal HybridizerGetShallowSizeDelegate GetShallowSize { get; set; }
            internal HybridizerCopyToSymbolDelegate HybridizerCopyToSymbol { get; set; }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate HybridizerProperties HybridizerGetProperties();
        private NativePtrConverter(HybridizerFlavor flavor, HybridizedLibrary[] libraries)
        {
            this._flavor = flavor;
            this._libraries = libraries;
        }

        public HybridizerFlavor Flavor
        {
            get { return _flavor; }
        }

        #endregion

        internal static NativePtrConverter Create(HybridizerFlavor flavor, IEnumerable<string> libraryFileNames)
        {
            var libraries = new List<HybridizedLibrary>();
            string filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var fileInfo = new FileInfo(filePath);
            var directoryInfo = fileInfo.Directory;
            foreach (string cudaDll in libraryFileNames)
            {
                var dllFile = new FileInfo(string.Format("{0}\\{1}", directoryInfo.FullName, cudaDll));
                if (!dllFile.Exists)
                    continue;
                HybridizedLibrary lib = RegisterDLL(dllFile, flavor);
                if (lib != null)
                    libraries.Add(lib);
            }
            var res = new NativePtrConverter(flavor, libraries.ToArray());
            return res;
        }

        internal static HybridizedLibrary RegisterDLL(FileInfo fileInfo, HybridizerFlavor flavor)
        {
            IntPtr loadLibrary = KernelInteropTools.LoadLibrary(fileInfo.FullName);
            if (loadLibrary.Equals(IntPtr.Zero))
            {
                var directory = Directory.GetCurrentDirectory();
                throw new Exception(String.Format("Library pointer is null libName:{0} Directory:{1}", fileInfo.FullName, directory));
            }
            IntPtr procAddress = KernelInteropTools.GetProcAddress(loadLibrary, NativeFunctionGetType);
            if (procAddress.Equals(IntPtr.Zero))
                throw new Exception(String.Format("ProcAddress is null ProcName:{0}", NativeFunctionGetType));
            HybridizerGetTypeIdDelegate del = (HybridizerGetTypeIdDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(HybridizerGetTypeIdDelegate));

            IntPtr sizeProcAddress = KernelInteropTools.GetProcAddress(loadLibrary, NativeFunctionGetShallowSize);
            if (sizeProcAddress.Equals(IntPtr.Zero))
                throw new Exception(String.Format("sizeProcAddress is null ProcName:{0}", NativeFunctionGetShallowSize));
            HybridizerGetShallowSizeDelegate sizeDel = (HybridizerGetShallowSizeDelegate)Marshal.GetDelegateForFunctionPointer(sizeProcAddress, typeof(HybridizerGetShallowSizeDelegate));

            // Retrieve from hybridized code whether arrays are simple pointers (no length) or wrapped into a structure
            procAddress = KernelInteropTools.GetProcAddress(loadLibrary, NativeFunctionHybridizerGetProperties);
            if (!procAddress.Equals(IntPtr.Zero))
            {
                HybridizerGetProperties d =
                    (HybridizerGetProperties)
                        Marshal.GetDelegateForFunctionPointer(procAddress, typeof (HybridizerGetProperties));
                HybridizerProperties props = d.Invoke();
                if (props.Flavor != flavor)
                    throw new Exception(String.Format("Invalid flavor of library {0} (expected {1})", props.Flavor, flavor));

                var useHybridArrays = props.UseHybridArrays != 0;

                if (CudaRuntimeProperties.UseHybridArrays && !useHybridArrays)
                {
                    //throw new ApplicationException(string.Format("Invalid value for UseHybridArray in dll {0} - you cannot mix DLLs with and without hybrid array support", cudaDll));
                }
                if (!CudaRuntimeProperties.UseHybridArrays) CudaRuntimeProperties.UseHybridArrays = useHybridArrays;
            }


            procAddress = KernelInteropTools.GetProcAddress(loadLibrary, NativeFunctionHybridizerGetFunctionPointer);
            HybridizerGetFunctionPointerDelegate fpDel = null;

            if (!procAddress.Equals(IntPtr.Zero))
                fpDel = (HybridizerGetFunctionPointerDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(HybridizerGetFunctionPointerDelegate));
            procAddress = KernelInteropTools.GetProcAddress(loadLibrary, NativeFunctionHybridizerGetVectorizedActionFunctionPointer);
            HybridizerGetVectorizedActionFunctionPointerDelegate fpDel2 = null;

            if (!procAddress.Equals(IntPtr.Zero))
                fpDel2 = (HybridizerGetVectorizedActionFunctionPointerDelegate)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(HybridizerGetVectorizedActionFunctionPointerDelegate));
            HybridizedLibrary res = new HybridizedLibrary(fileInfo.FullName);
            res.GetTypeId = del;
            res.GetFunctionPointer = fpDel;
            res.GetVectorizedActionFunctionPointer = fpDel2;
            res.GetShallowSize = sizeDel;

            IntPtr hybridizerCopyToSymbolAddress = KernelInteropTools.GetProcAddress(loadLibrary, HybridizerCopyToSymbol);
            if (!hybridizerCopyToSymbolAddress.Equals(IntPtr.Zero))
            {
                HybridizerCopyToSymbolDelegate copyToSymbloDel = (HybridizerCopyToSymbolDelegate)Marshal.GetDelegateForFunctionPointer(hybridizerCopyToSymbolAddress, typeof(HybridizerCopyToSymbolDelegate));
                res.HybridizerCopyToSymbol = copyToSymbloDel;
            }

            if (cuda.s_VERBOSITY == cuda.VERBOSITY.Verbose)
                Console.Out.WriteLine("[INFO] : Registered DLL {0}", fileInfo.FullName);

            return res;
        }

        internal bool RegisterDLL(string dllName)
        {
            FileInfo dllFile = new FileInfo(dllName);
            if (!dllFile.Exists)
            {
                var filePath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                var fileInfo = new FileInfo(filePath);
                var directoryInfo = fileInfo.Directory;
                dllFile = new FileInfo(string.Format("{0}\\{1}", directoryInfo.FullName, dllName));
                if (!dllFile.Exists)
                    return false;
            }

            HybridizedLibrary del = RegisterDLL(dllFile, Flavor);
            if (del != null)
            {
                var list = new List<HybridizedLibrary>(_libraries);
                list.Add(del);
                _libraries = list.ToArray();
                _cache.Clear();
                _typeInfoCache.Clear();
                return true;
            }
            return false;
        }

        internal IntPtr Convert(Type type)
        {
            return Convert(NamingTools.QualifiedTypeName(type));
        }

        internal IntPtr Convert(string fullName)
        {
            IntPtr result;
            if (_cache.TryGetValue(fullName, out result))
                return result;
            lock (this) 
            {
                foreach (var library in _libraries)
                {
                    var res = (int)library.GetTypeId.DynamicInvoke(fullName);
                    if (res != 0)
                    {
                        result = new IntPtr(res);
                        _cache.Add(fullName, result);
                        return result;
                    }
                }
#if DEBUG
                Console.WriteLine("Marshalling a type that has no typeid: {0} (maybe OK)", fullName);
#endif
                return IntPtr.Zero;
            }
        }

        internal IntPtr GetFunctionPointer(MethodBase method)
        {
            var mName = NamingTools.GetEncodedSignature(method);
            IntPtr result;
            if (_cache.TryGetValue(mName, out result))
                return result;
            lock (this)
            {
                if (_cache.TryGetValue(mName, out result))
                    return result;
                foreach (var library in _libraries)
                {
                    if (library.GetFunctionPointer != null)
                    {
                        var dynamicInvoke = library.GetFunctionPointer.DynamicInvoke(mName);
                        IntPtr res = (IntPtr)dynamicInvoke;
                        if (res != IntPtr.Zero)
                        {
                            _cache.Add(mName, res);
                            return res;
                        }
                    }
                }
                // This happens only when serializing delegates, arriving here means that the delegate method could not be found
                throw new ApplicationException("Could not get function pointer for method " + method + " - symbol looked for : " + mName);
            }
        }

        internal IntPtr GetVectorizedActionFunctionPointer(MethodBase method)
        {
            var mName = NamingTools.GetEncodedSignature(method);
            IntPtr result;
            //if ("Altimeshx46QuantFinancex46LongstaffSchwartzx46LongstaffSchwartzx60Altimeshx46QuantFinancex46Modelsx46Conceptsx46LogBasedSpotx60Altimeshx46QuantFinancex46Modelsx46Conceptsx46ArrayDoubleSpotx62x44Altimeshx46QuantFinancex46Modelsx46Conceptsx46ConstantRateDiscountingx44Altimeshx46QuantFinancex46LongstaffSchwartzx46AmericanCallx60Altimeshx46QuantFinancex46Modelsx46Conceptsx46LogBasedSpotx60Altimeshx46QuantFinancex46Modelsx46Conceptsx46ArrayDoubleSpotx62x62x44Altimeshx46QuantFinancex46LongstaffSchwartzx46QuadraticUnivariateRegressorStructx44Altimeshx46QuantFinancex46Schedulingx46SingleBlockParallelExecutorx62_applyRegressor_XHybridizerx46Runtimex46CUDAImportsx46alignedindex" == mName)
            //    return IntPtr.Zero;
            if (_vectorizedCache.TryGetValue(mName, out result))
                return result;
            lock (this)
            {
                if (_vectorizedCache.TryGetValue(mName, out result))
                    return result;
                foreach (var library in _libraries)
                {
                    if (library.GetVectorizedActionFunctionPointer != null)
                    {
                        var dynamicInvoke = library.GetVectorizedActionFunctionPointer.DynamicInvoke(mName);
                        IntPtr res = (IntPtr)dynamicInvoke;
                        if (res != IntPtr.Zero)
                        {
                            if (res.ToInt64() < 0xFL)
                                Console.WriteLine("Error while getting vectorized function pointer, invalid result {0:X}", res);

                            _vectorizedCache.Add(mName, res);
                            return res;
                        }
                    }
                }
                // This happens only when serializing delegates, arriving here means that the delegate method could not be found
                //throw new ApplicationException("Could not get function pointer for method " + method + " - symbol looked for : " + mName);
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// INTERNAL METHOD
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TypeInfo GetTypeInfo(Type type)
        {
            TypeInfo res;
            if (_typeInfoCache.TryGetValue(type, out res)) return res;
            lock (this)
            {
                string fullName = NamingTools.QualifiedTypeName(type);
                long shallowSize = 0;
                foreach (var library in _libraries)
                {
                    shallowSize = System.Convert.ToInt64(library.GetShallowSize.DynamicInvoke(fullName));
                    if (shallowSize != 0) break;
                }
                if (shallowSize == 0)
                    shallowSize = FieldTools.SizeOf(type, 0);
#if DEBUG
                else
                {
                    var computedSize = FieldTools.SizeOf(type, 0);
                    if (shallowSize != computedSize)
                        throw new ApplicationException("Computed size for " + fullName + ": " + computedSize + " != " + shallowSize);
                    
                }
#endif
                long typeId = 0;
                if (!type.IsValueType)
                    typeId = Convert(fullName).ToInt64();
                res = new TypeInfo(type, typeId, FieldTools.OrderedFields(type), shallowSize);
#if DEBUG
                Console.WriteLine("TypeInfo for {0}", fullName);
                Console.WriteLine(res);
#endif
                _typeInfoCache[type] = res;
            }
            return res;
        }

        public void CopyToSymbol(string symbol, IntPtr src, IntPtr size, IntPtr offset, IntPtr module)
        {
            foreach (var library in _libraries)
            {
                library.HybridizerCopyToSymbol.Invoke(symbol, src, size, offset, module);
            }
        }

        internal void SetCustomMarshaler(Type type, IHybCustomMarshaler cm)
        {
            TypeInfo res = GetTypeInfo(type);
            res.CustomMarshaler = cm;
            lock (this)
            {
                _typeInfoCache[type] = res;
            }
        }
    }
}