/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Runtime.InteropServices;
using static Hybridizer.Runtime.CUDAImports.cufftImplem;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// cufft wrapper
    /// Complete documentation <see href="https://docs.nvidia.com/cuda/cufft/index.html">here</see>
    /// </summary>
    public class cufft
    {
        static ICUFFT instance = new CUFFT_64_75();

        static cufft()
        {
            string cudaVersion = cuda.GetCudaVersion();
            switch (cudaVersion)
            {
                case "50":
                    instance = (IntPtr.Size == 8) ? (ICUFFT)new CUFFT_64_50() : (ICUFFT)new CUFFT_32_50();
                    break;
                case "60":
                    instance = (IntPtr.Size == 8) ? (ICUFFT)new CUFFT_64_60() : (ICUFFT)new CUFFT_32_60();
                    break;
                case "65":
                    instance = (IntPtr.Size == 8) ? (ICUFFT)new CUFFT_64_65() : (ICUFFT)new CUFFT_32_65();
                    break;
                case "75":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 7.5");
                    instance = new CUFFT_64_75();
                    break;
                case "80":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 8.0");
                    instance = new CUFFT_64_80();
                    break;
                case "90":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 9.0");
                    instance = new CUFFT_64_90();
                    break;
                case "91":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 9.1");
                    instance = new CUFFT_64_91();
                    break;
                case "92":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 9.2");
                    instance = new CUFFT_64_92();
                    break;
                case "100":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 10.0");
                    instance = new CUFFT_64_100();
                    break;
                case "101":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 10.1");
                    instance = new CUFFT_64_101();
                    break;
                case "110":
                case "114":
                case "116":
                case "117":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 11");
                    instance = new CUFFT_64_110();
                    break;
                case "120":
                case "121":
                case "122":
                    if (IntPtr.Size != 8) throw new ApplicationException("32bits version of CUFFT does not exist for CUDA 11");
                    instance = new CUFFT_64_120();
                    break;
                default:
                    throw new ApplicationException(string.Format("Unknown version of Cuda {0}", cudaVersion));
            }
        }

        /// <summary>
        /// 
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html#function-cufftplanmany">online</see>
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="rank"></param>
        /// <param name="n"></param>
        /// <param name="inembed"></param>
        /// <param name="istride"></param>
        /// <param name="idist"></param>
        /// <param name="onembed"></param>
        /// <param name="ostride"></param>
        /// <param name="odist"></param>
        /// <param name="type"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static cufftResult PlanMany(out cufftHandle plan, int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch) { return instance.PlanMany(out plan, rank, n, inembed, istride, idist, onembed, ostride, odist, type, batch); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html#function-cufftplanmany">online</see>
        /// </summary>
        public static cufftResult PlanMany(out cufftHandle plan, int rank, int[] n, int[] inembed, int istride, int idist, int[] onembed, int ostride, int odist, cufftType type, int batch) 
        {
            GCHandle n_gch = GCHandle.Alloc(n, GCHandleType.Pinned);
            GCHandle onembed_gch = GCHandle.Alloc(onembed, GCHandleType.Pinned);
            GCHandle inembed_gch = GCHandle.Alloc(inembed, GCHandleType.Pinned);
            cufftResult res = instance.PlanMany(out plan, rank, Marshal.UnsafeAddrOfPinnedArrayElement(n, 0), Marshal.UnsafeAddrOfPinnedArrayElement(inembed, 0), istride, idist, Marshal.UnsafeAddrOfPinnedArrayElement(onembed, 0), ostride, odist, type, batch);
            n_gch.Free();
            onembed_gch.Free();
            inembed_gch.Free();
            return res;
        }

        /// <summary>
        /// <see href="https://docs.nvidia.com/cuda/cufft/index.html#function-cufftplan1d">online</see>
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="nx"></param>
        /// <param name="type"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        public static cufftResult Plan1d(out cufftHandle plan, int nx, cufftType type, int batch) { return instance.Plan1d(out plan, nx, type, batch); }
        /// <summary>
        /// 
        /// <see href="https://docs.nvidia.com/cuda/cufft/index.html#function-cufftplan2d">online</see>
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static cufftResult Plan2d(out cufftHandle plan, int nx, int ny, cufftType type) { return instance.Plan2d(out plan, nx, ny, type); }
        /// <summary>
        /// 
        /// <see href="https://docs.nvidia.com/cuda/cufft/index.html#function-cufftplan3d">online</see>
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <param name="nz"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static cufftResult Plan3d(out cufftHandle plan, int nx, int ny, int nz, cufftType type) { return instance.Plan3d(out plan, nx, ny, nz, type); }
        /// <summary>
        /// 
        /// <see href="">onlone</see>
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public static cufftResult Destroy(cufftHandle plan) { return instance.Destroy(plan); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecC2C(cufftHandle plan, IntPtr idata, IntPtr odata, int direction) { return instance.ExecC2C(plan, idata, odata, direction); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecZ2Z(cufftHandle plan, IntPtr idata, IntPtr odata, int direction) { return instance.ExecZ2Z(plan, idata, odata, direction); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecR2C(cufftHandle plan, IntPtr idata, IntPtr odata) { return instance.ExecR2C(plan, idata, odata); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecD2Z(cufftHandle plan, IntPtr idata, IntPtr odata) { return instance.ExecD2Z(plan, idata, odata); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecC2R(cufftHandle plan, IntPtr idata, IntPtr odata) { return instance.ExecC2R(plan, idata, odata); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecZ2D(cufftHandle plan, IntPtr idata, IntPtr odata) { return instance.ExecZ2D(plan, idata, odata); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult SetStream(cufftHandle plan, cudaStream_t stream) { return instance.SetStream(plan, stream); }
        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult SetCompatibilityMode(cufftHandle plan, cufftCompatibility mode) { return instance.SetCompatibilityMode(plan, mode); }

        /// <summary>
        /// float type
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="plan"></param>
        /// <param name="idata"></param>
        /// <param name="odata"></param>
        /// <returns></returns>
        public static cufftResult ExecR2C<R,C>(cufftHandle plan, R[] idata, C[] odata)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecR2C(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// double type
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <typeparam name="Z"></typeparam>
        /// <param name="plan"></param>
        /// <param name="idata"></param>
        /// <param name="odata"></param>
        /// <returns></returns>
        public static cufftResult ExecD2Z<D, Z>(cufftHandle plan, D[] idata, Z[] odata)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecD2Z(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecC2C<C>(cufftHandle plan, C[] idata, C[] odata, int direction)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecC2C(plan, dev_idata, dev_odata, direction);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecZ2Z<Z>(cufftHandle plan, Z[] idata, Z[] odata, int direction)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecZ2Z(plan, dev_idata, dev_odata, direction);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// float type
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="plan"></param>
        /// <param name="idata"></param>
        /// <param name="odata"></param>
        /// <returns></returns>
        public static cufftResult ExecC2R<R,C>(cufftHandle plan, C[] idata, R[] odata)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecC2R(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// double type
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <typeparam name="Z"></typeparam>
        /// <param name="plan"></param>
        /// <param name="idata"></param>
        /// <param name="odata"></param>
        /// <returns></returns>
        public static cufftResult ExecZ2D<D, Z>(cufftHandle plan, Z[] idata, D[] odata)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecZ2D(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecR2C(cufftHandle plan, float[] idata, float2[] odata) 
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecR2C(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecD2Z(cufftHandle plan, double[] idata, double2[] odata)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecD2Z(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecC2C(cufftHandle plan, float2[] idata, float2[] odata, int direction)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecC2C(plan, dev_idata, dev_odata, direction);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecZ2Z(cufftHandle plan, double2[] idata, double2[] odata, int direction)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecZ2Z(plan, dev_idata, dev_odata, direction);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecC2R(cufftHandle plan, float2[] idata, float[] odata)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecC2R(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// <see href="http://docs.nvidia.com/cuda/cufft/index.html">online</see>
        /// </summary>
        public static cufftResult ExecZ2D(cufftHandle plan, double2[] idata, double[] odata)
        {
            IntPtr dev_idata = CudaMarshaler.Instance.MarshalManagedToNative(idata);
            IntPtr dev_odata = CudaMarshaler.Instance.MarshalManagedToNative(odata);
            cufftResult res = instance.ExecZ2D(plan, dev_idata, dev_odata);
            cuda.DeviceSynchronize();
            CudaMarshaler.Instance.CleanUpNativeData(dev_idata);
            CudaMarshaler.Instance.CleanUpNativeData(dev_odata);
            return res;
        }

        /// <summary>
        /// Creates only an opaque handle, and allocates small data structures on the host
        /// <see href="https://docs.nvidia.com/cuda/archive/9.2/cufft/index.html#function-cufftcreate">online</see> 
        /// </summary>
        /// <param name="plan">Pointer to a cufftHandle object </param>
        /// <returns></returns>
        public static cufftResult Create(out cufftHandle plan) { return instance.Create(out plan); }

        /// <summary>
        /// During plan execution, cuFFT requires a work area for temporary storage of intermediate results. This call returns an estimate for the size of the work area required, given the specified parameters, and assuming default plan settings. 
        /// <see href="https://docs.nvidia.com/cuda/archive/9.2/cufft/index.html#function-cufftestimate1d">online</see> 
        /// </summary>
        /// <param name="nx">The transform size in the x dimension (number of rows)</param>
        /// <param name="type">The transform data type (e.g., <see cref="cufftType.CUFFT_C2R"/> for single precision complex to real) </param>
        /// <param name="batch">Number of transforms of size nx. Please consider using <see cref="EstimateMany"/> for multiple transforms.</param>
        /// <param name="workSize">Pointer to the size, in bytes, of the work space.</param>
        /// <returns></returns>
        public static cufftResult Estimate1d(int nx, cufftType type, int batch, out size_t workSize) { return instance.Estimate1d(nx, type, batch, out workSize); }
      
        /// <summary>
        /// During plan execution, cuFFT requires a work area for temporary storage of intermediate results. This call returns an estimate for the size of the work area required, given the specified parameters, and assuming default plan settings. 
        /// </summary>
        /// <param name="nx">The transform size in the x dimension (number of rows)</param>
        /// <param name="ny">The transform size in the y dimension (number of columns)</param>
        /// <param name="type">The transform data type (e.g., <see cref="cufftType.CUFFT_C2R"/> for single precision complex to real) </param>
        /// <param name="workSize">Pointer to the size, in bytes, of the work space.</param>
        /// <returns></returns>
        public static cufftResult Estimate2d(int nx, int ny, cufftType type, out size_t workSize) { return instance.Estimate2d(nx, ny, type, out workSize); }
      
        /// <summary>
        /// During plan execution, cuFFT requires a work area for temporary storage of intermediate results. This call returns an estimate for the size of the work area required, given the specified parameters, and assuming default plan settings. 
        /// </summary>
        /// <param name="nx">The transform size in the x dimension</param>
        /// <param name="ny">The transform size in the y dimension</param>
        /// <param name="nz">The transform size in the z dimension</param>
        /// <param name="type">The transform data type (e.g., <see cref="cufftType.CUFFT_C2R"/> for single precision complex to real) </param>
        /// <param name="workSize">Pointer to the size, in bytes, of the work space.</param>
        /// <returns></returns>
        public static cufftResult Estimate3d(int nx, int ny, int nz, cufftType type, out size_t workSize) { return instance.Estimate3d(nx, ny, nz, type, out workSize); }
     
        /// <summary>
        /// During plan execution, cuFFT requires a work area for temporary storage of intermediate results. This call returns an estimate for the size of the work area required, given the specified parameters, and assuming default plan settings.
        /// The cufftEstimateMany() API supports more complicated input and output data layouts via the advanced data layout parameters: inembed, istride, idist, onembed, ostride, and odist.
        /// All arrays are assumed to be in CPU memory.
        /// </summary>
        /// <param name="rank">Dimensionality of the transform (1, 2, or 3)</param>
        /// <param name="n">Array of size rank, describing the size of each dimension </param>
        /// <param name="inembed">Pointer of size rank that indicates the storage dimensions of the input data in memory. If set to NULL all other advanced data layout parameters are ignored.</param>
        /// <param name="istride">Indicates the distance between two successive input elements in the least significant (i.e., innermost) dimension</param>
        /// <param name="idist">Indicates the distance between the first element of two consecutive signals in a batch of the input data</param>
        /// <param name="onembed">Pointer of size rank that indicates the storage dimensions of the output data in memory. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="ostride">Indicates the distance between two successive output elements in the output array in the least significant (i.e., innermost) dimension </param>
        /// <param name="odist">Indicates the distance between the first element of two consecutive signals in a batch of the output data</param>
        /// <param name="type">The transform data type (e.g., CUFFT_R2C for single precision real to complex) </param>
        /// <param name="batch">Batch size for this transform</param>
        /// <param name="workSize">Pointer to the size, in bytes, of the work space.</param>
        /// <returns></returns>
        public static cufftResult EstimateMany(int rank, int[] n, int[] inembed, int istride, int idist, int[] onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize)
        {
            var n_handle = GCHandle.Alloc(n, GCHandleType.Pinned);
            var inembed_handle = GCHandle.Alloc(inembed, GCHandleType.Pinned);
            var onembed_handle = GCHandle.Alloc(onembed, GCHandleType.Pinned);
            IntPtr d_n = n_handle.AddrOfPinnedObject();
            IntPtr d_inembed = inembed_handle.AddrOfPinnedObject();
            IntPtr d_onembed = onembed_handle.AddrOfPinnedObject();
            cufftResult result = instance.EstimateMany(rank, d_n, d_inembed, istride, idist, d_onembed, ostride, odist, type, batch, out workSize);
            cuda.DeviceSynchronize();
            n_handle.Free();
            inembed_handle.Free();
            onembed_handle.Free();
            return result;
        }
      
        /// <summary>
        /// Return in *value the number for the property described by type of the dynamically linked CUFFT library. 
        /// </summary>
        /// <param name="type">CUDA library property</param>
        /// <param name="val">Contains the integer value for the requested property</param>
        /// <returns></returns>
        public static cufftResult GetProperty(libraryPropertyType_t type, out int val) { return instance.GetProperty(type, out val); }
      
        /// <summary>
        /// Once plan generation has been done, either with the original API or the extensible API, this call returns the actual size of the work area required to support the plan. 
        /// Callers who choose to manage work area allocation within their application must use this call after plan generation, and after any cufftSet*() calls subsequent to plan generation, if those calls might alter the required work space size. 
        /// </summary>
        /// <param name="handle">cufftHandle returned by <see cref="Create"/></param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult GetSize(cufftHandle handle, out size_t workSize) { return instance.GetSize(handle, out workSize); }
    
        /// <summary>
        /// Gives a more accurate estimate of the work area size required for a plan than the <see cref="Estimate1d"/> routine as they take into account any plan settings that may have been made.
        /// As discussed in the section <see href="https://docs.nvidia.com/cuda/archive/9.2/cufft/index.html#work-estimate">cuFFT Estimated Size of Work Area</see>, the workSize value(s) returned may be conservative especially for values of n that are not multiples of powers of 2, 3, 5 and 7.
        /// </summary>
        /// <param name="handle">cufftHandle returned by cufftCreate</param>
        /// <param name="nx">The transform size (e.g. 256 for a 256-point FFT)</param>
        /// <param name="type">The transform data type (e.g., CUFFT_C2C for single precision complex to complex) </param>
        /// <param name="batch">Number of transforms of size nx. Please consider using cufftGetSizeMany for multiple transforms. </param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult GetSize1d(cufftHandle handle, int nx, cufftType type, int batch, out size_t workSize) { return instance.GetSize1d(handle, nx, type, batch, out workSize); }
     
        /// <summary>
        /// This call gives a more accurate estimate of the work area size required for a plan than <see cref="Estimate2d"></see>, given the specified parameters, and taking into account any plan settings that may have been made. 
        /// </summary>
        /// <param name="handle">cufftHandle returned by cufftCreate</param>
        /// <param name="nx">The transform size in the x dimension (number of rows) </param>
        /// <param name="ny">The transform size in the y dimension (number of columns) </param>
        /// <param name="type">The transform data type (e.g., CUFFT_C2R for single precision complex to real) </param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult GetSize2d(cufftHandle handle, int nx, int ny, cufftType type, out size_t workSize) { return instance.GetSize2d(handle, nx, ny, type, out workSize); }
    
        /// <summary>
        /// This call gives a more accurate estimate of the work area size required for a plan than <see cref="Estimate3d"></see>, given the specified parameters, and taking into account any plan settings that may have been made. 
        /// </summary>
        /// <param name="handle">cufftHandle returned by cufftCreate</param>
        /// <param name="nx">The transform size in the x dimension </param>
        /// <param name="ny">The transform size in the y dimension </param>
        /// <param name="nz">The transform size in the z dimension </param>
        /// <param name="type">The transform data type (e.g., CUFFT_R2C for single precision real to complex) </param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult GetSize3d(cufftHandle handle, int nx, int ny, int nz, cufftType type, out size_t workSize) { return instance.GetSize3d(handle, nx, ny, nz, type, out workSize); }
   
        /// <summary>
        /// This call gives a more accurate estimate of the work area size required for a plan than <see cref="EstimateMany"/>, given the specified parameters, and taking into account any plan settings that may have been made. 
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="rank">Dimensionality of the transform (1, 2, or 3)</param>
        /// <param name="n">Array of size rank, describing the size of each dimension </param>
        /// <param name="inembed">Pointer of size rank that indicates the storage dimensions of the input data in memory. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="istride">Indicates the distance between two successive input elements in the least significant (i.e., innermost) dimension</param>
        /// <param name="idist">Indicates the distance between the first element of two consecutive signals in a batch of the input data</param>
        /// <param name="onembed">Pointer of size rank that indicates the storage dimensions of the output data in memory. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="ostride">Indicates the distance between two successive output elements in the output array in the least significant (i.e., innermost) dimension </param>
        /// <param name="odist">Indicates the distance between the first element of two consecutive signals in a batch of the output data</param>
        /// <param name="type">The transform data type (e.g., CUFFT_R2C for single precision real to complex) </param>
        /// <param name="batch">Batch size for this transform</param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult GetSizeMany(cufftHandle plan, int rank, int[] n, int[] inembed, int istride, int idist, int[] onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize)
        {
            var n_handle = GCHandle.Alloc(n, GCHandleType.Pinned);
            var inembed_handle = GCHandle.Alloc(inembed, GCHandleType.Pinned);
            var onembed_handle = GCHandle.Alloc(onembed, GCHandleType.Pinned);
            IntPtr d_n = n_handle.AddrOfPinnedObject();
            IntPtr d_inembed = inembed_handle.AddrOfPinnedObject();
            IntPtr d_onembed = onembed_handle.AddrOfPinnedObject();
            cufftResult result = instance.GetSizeMany(plan, rank, d_n, d_inembed, istride, idist, d_onembed, ostride, odist, type, batch, out workSize);
            cuda.DeviceSynchronize();
            n_handle.Free();
            inembed_handle.Free();
            onembed_handle.Free();
            return result;
        }
       
        /// <summary>
        /// This call gives a more accurate estimate of the work area size required for a plan than cufftEstimateSizeMany(), given the specified parameters, and taking into account any plan settings that may have been made.
        /// This API is identical to cufftMakePlanMany except that the arguments specifying sizes and strides are 64 bit integers.This API makes very large transforms possible. cuFFT includes kernels that use 32 bit indexes, and kernels that use 64 bit indexes. cuFFT planning selects 32 bit kernels whenever possible to avoid any overhead due to 64 bit arithmetic.
        /// All sizes and types of transform are supported by this interface, with two exceptions.For transforms whose total size exceeds 4G elements, the dimensions specified in the array n must be factorable into primes that are less than or equal to 127. For real to complex and complex to real transforms whose total size exceeds 2G elements, the fastest changing dimension must be even.
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="rank">Dimensionality of the transform (1, 2, or 3)</param>
        /// <param name="n">Array of size rank, describing the size of each dimension </param>
        /// <param name="inembed">Pointer of size rank that indicates the storage dimensions of the input data in memory. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="istride">Indicates the distance between two successive input elements in the least significant (i.e., innermost) dimension</param>
        /// <param name="idist">Indicates the distance between the first element of two consecutive signals in a batch of the input data</param>
        /// <param name="onembed">Pointer of size rank that indicates the storage dimensions of the output data in memory. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="ostride">Indicates the distance between two successive output elements in the output array in the least significant (i.e., innermost) dimension </param>
        /// <param name="odist">Indicates the distance between the first element of two consecutive signals in a batch of the output data</param>
        /// <param name="type">The transform data type (e.g., CUFFT_R2C for single precision real to complex) </param>
        /// <param name="batch">Batch size for this transform</param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult GetSizeMany64(cufftHandle plan, int rank, long[] n, long[] inembed, long istride, long idist, long[] onembed, long ostride, long odist, cufftType type, long batch, out size_t workSize)
        {
            var n_handle = GCHandle.Alloc(n, GCHandleType.Pinned);
            var inembed_handle = GCHandle.Alloc(inembed, GCHandleType.Pinned);
            var onembed_handle = GCHandle.Alloc(onembed, GCHandleType.Pinned);
            IntPtr d_n = n_handle.AddrOfPinnedObject();
            IntPtr d_inembed = inembed_handle.AddrOfPinnedObject();
            IntPtr d_onembed = onembed_handle.AddrOfPinnedObject();
            cufftResult result = instance.GetSizeMany64(plan, rank, d_n, d_inembed, istride, idist, d_onembed, ostride, odist, type, batch, out workSize);
            cuda.DeviceSynchronize();
            n_handle.Free();
            inembed_handle.Free();
            onembed_handle.Free();
            return result;
        }
      
        /// <summary>
        /// Returns the version number of cuFFT.
        /// </summary>
        /// <param name="version">Pointer to the version number</param>
        /// <returns></returns>
        public static cufftResult GetVersion(out int version) { return instance.GetVersion(out version); }
       
        /// <summary>
        /// Following a call to cufftCreate() makes a 1D FFT plan configuration for a specified signal size and data type. The batch input parameter tells cuFFT how many 1D transforms to configure. 
        /// If cufftXtSetGPUs() was called prior to this call with multiple GPUs, then workSize will contain multiple sizes. See sections on multiple GPUs for more details. 
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="nx">The transform size (e.g. 256 for a 256-point FFT). For multiple GPUs, this must be a power of 2.</param>
        /// <param name="type">The transform data type (e.g., CUFFT_C2C for single precision complex to complex). For multiple GPUs this must be a complex to complex transform. </param>
        /// <param name="batch">Number of transforms of size nx. Please consider using cufftMakePlanMany for multiple transforms. </param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult MakePlan1d(cufftHandle plan, int nx, cufftType type, int batch, out size_t workSize) { return instance.MakePlan1d(plan, nx, type, batch, out workSize); }
       
        /// <summary>
        /// Following a call to cufftCreate() makes a 2D FFT plan configuration according to specified signal sizes and data type. 
        /// If cufftXtSetGPUs() was called prior to this call with multiple GPUs, then workSize will contain multiple sizes. See sections on multiple GPUs for more details. 
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="nx">The transform size in the x dimension. This is slowest changing dimension of a transform (strided in memory). For multiple GPUs, this must be factorable into primes less than or equal to 127.</param>
        /// <param name="ny">The transform size in the y dimension. This is fastest changing dimension of a transform (contiguous in memory). For 2 GPUs, this must be factorable into primes less than or equal to 127.</param>
        /// <param name="type">The transform data type (e.g., CUFFT_C2R for single precision complex to real). For multiple GPUs this must be a complex to complex transform. </param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult MakePlan2d(cufftHandle plan, int nx, int ny, cufftType type, out size_t workSize) { return instance.MakePlan2d(plan, nx, ny, type, out workSize); }

        /// <summary>
        /// Following a call to cufftCreate() makes a 3D FFT plan configuration according to specified signal sizes and data type. This function is the same as cufftPlan2d() except that it takes a third size parameter nz. 
        /// If cufftXtSetGPUs() was called prior to this call with multiple GPUs, then workSize will contain multiple sizes. See sections on multiple GPUs for more details. 
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="nx">The transform size in the x dimension. This is slowest changing dimension of a transform (strided in memory). For multiple GPUs, this must be factorable into primes less than or equal to 127. </param>
        /// <param name="ny">The transform size in the y dimension. For multiple GPUs, this must be factorable into primes less than or equal to 127. </param>
        /// <param name="nz">The transform size in the z dimension. This is fastest changing dimension of a transform (contiguous in memory). For multiple GPUs, this must be factorable into primes less than or equal to 127. </param>
        /// <param name="type">The transform data type (e.g., CUFFT_R2C for single precision real to complex). For multiple GPUs this must be a complex to complex transform. </param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult MakePlan3d(cufftHandle plan, int nx, int ny, int nz, cufftType type, out size_t workSize) { return instance.MakePlan3d(plan, nx, ny, nz, type, out workSize); }

        /// <summary>
        /// Following a call to cufftCreate() makes a FFT plan configuration of dimension rank, with sizes specified in the array n. The batch input parameter tells cuFFT how many transforms to configure. With this function, batched plans of 1, 2, or 3 dimensions may be created. 
        /// The cufftPlanMany() API supports more complicated input and output data layouts via the advanced data layout parameters: inembed, istride, idist, onembed, ostride, and odist. 
        /// If inembed and onembed are set to NULL, all other stride information is ignored, and default strides are used. The default assumes contiguous data arrays. 
        /// If cufftXtSetGPUs() was called prior to this call with multiple GPUs, then workSize will contain multiple sizes. See sections on multiple GPUs for more details. 
        /// All arrays are assumed to be in CPU memory.
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="rank">Dimensionality of the transform (1, 2, or 3)</param>
        /// <param name="n">Array of size rank, describing the size of each dimension, n[0] being the size of the outermost and n[rank-1] innermost (contiguous) dimension of a transform. For multiple GPUs and rank equal to 1, the sizes must be a power of 2. For multiple GPUs and rank equal to 2 or 3, the sizes must be factorable into primes less than or equal to 127. </param>
        /// <param name="inembed">Pointer of size rank that indicates the storage dimensions of the input data in memory, inembed[0] being the storage dimension of the innermost dimension. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="istride">Indicates the distance between two successive input elements in the least significant (i.e., innermost) dimension</param>
        /// <param name="idist">Indicates the distance between the first element of two consecutive signals in a batch of the input data</param>
        /// <param name="onembed">Pointer of size rank that indicates the storage dimensions of the output data in memory, inembed[0] being the storage dimension of the innermost dimension. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="ostride">Indicates the distance between two successive output elements in the output array in the least significant (i.e., innermost) dimension </param>
        /// <param name="odist">Indicates the distance between the first element of two consecutive signals in a batch of the output data</param>
        /// <param name="type">The transform data type (e.g., CUFFT_R2C for single precision real to complex). For 2 GPUs this must be a complex to complex transform. </param>
        /// <param name="batch">Batch size for this transform</param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult MakePlanMany(cufftHandle plan, int rank, int[] n, int[] inembed, int istride, int idist, int[] onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize)
        {
            var n_handle = GCHandle.Alloc(n, GCHandleType.Pinned);
            var inembed_handle = GCHandle.Alloc(inembed, GCHandleType.Pinned);
            var onembed_handle = GCHandle.Alloc(onembed, GCHandleType.Pinned);
            IntPtr d_n = n_handle.AddrOfPinnedObject();
            IntPtr d_inembed = inembed_handle.AddrOfPinnedObject();
            IntPtr d_onembed = onembed_handle.AddrOfPinnedObject();
            cufftResult result = instance.MakePlanMany(plan, rank, d_n, d_inembed, istride, idist, d_onembed, ostride, odist, type, batch, out workSize);
            cuda.DeviceSynchronize();
            n_handle.Free();
            inembed_handle.Free();
            onembed_handle.Free();
            return result;
        }
        
        /// <summary>
        /// Following a call to cufftCreate() makes a FFT plan configuration of dimension rank, with sizes specified in the array n. The batch input parameter tells cuFFT how many transforms to configure. With this function, batched plans of 1, 2, or 3 dimensions may be created. 
        /// The cufftPlanMany() API supports more complicated input and output data layouts via the advanced data layout parameters: inembed, istride, idist, onembed, ostride, and odist. 
        /// If inembed and onembed are set to NULL, all other stride information is ignored, and default strides are used. The default assumes contiguous data arrays. 
        /// If cufftXtSetGPUs() was called prior to this call with multiple GPUs, then workSize will contain multiple sizes. See sections on multiple GPUs for more details. 
        /// All arrays are assumed to be in CPU memory.
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="rank">Dimensionality of the transform (1, 2, or 3)</param>
        /// <param name="n">Array of size rank, describing the size of each dimension, n[0] being the size of the outermost and n[rank-1] innermost (contiguous) dimension of a transform. For multiple GPUs and rank equal to 1, the sizes must be a power of 2. For multiple GPUs and rank equal to 2 or 3, the sizes must be factorable into primes less than or equal to 127. </param>
        /// <param name="inembed">Pointer of size rank that indicates the storage dimensions of the input data in memory, inembed[0] being the storage dimension of the innermost dimension. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="istride">Indicates the distance between two successive input elements in the least significant (i.e., innermost) dimension</param>
        /// <param name="idist">Indicates the distance between the first element of two consecutive signals in a batch of the input data</param>
        /// <param name="onembed">Pointer of size rank that indicates the storage dimensions of the output data in memory, inembed[0] being the storage dimension of the innermost dimension. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="ostride">Indicates the distance between two successive output elements in the output array in the least significant (i.e., innermost) dimension </param>
        /// <param name="odist">Indicates the distance between the first element of two consecutive signals in a batch of the output data</param>
        /// <param name="type">The transform data type (e.g., CUFFT_R2C for single precision real to complex). For 2 GPUs this must be a complex to complex transform. </param>
        /// <param name="batch">Batch size for this transform</param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <returns></returns>
        public static cufftResult MakePlanMany64(cufftHandle plan, int rank, long[] n, long[] inembed, long istride, long idist, long[] onembed, long ostride, long odist, cufftType type, long batch, out size_t workSize)
        {
            var n_handle = GCHandle.Alloc(n, GCHandleType.Pinned);
            var inembed_handle = GCHandle.Alloc(inembed, GCHandleType.Pinned);
            var onembed_handle = GCHandle.Alloc(onembed, GCHandleType.Pinned);
            IntPtr d_n = n_handle.AddrOfPinnedObject();
            IntPtr d_inembed = inembed_handle.AddrOfPinnedObject();
            IntPtr d_onembed = onembed_handle.AddrOfPinnedObject();
            cufftResult result = instance.MakePlanMany64(plan, rank, d_n, d_inembed, istride, idist, d_onembed, ostride, odist, type, batch, out workSize);
            cuda.DeviceSynchronize();
            n_handle.Free();
            inembed_handle.Free();
            onembed_handle.Free();
            return result;
        }

        /// <summary>
        /// cufftSetAutoAllocation() indicates that the caller intends to allocate and manage work areas for plans that have been generated. 
        /// cuFFT default behavior is to allocate the work area at plan generation time. 
        /// If cufftSetAutoAllocation() has been called with autoAllocate set to 0 ("false") prior to one of the cufftMakePlan*() calls, cuFFT does not allocate the work area. 
        /// This is the preferred sequence for callers wishing to manage work area allocation. 
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate.</param>
        /// <param name="autoAllocate">allocate </param>
        /// <returns></returns>
        public static cufftResult SetAutoAllocate(cufftHandle plan, int autoAllocate) { return instance.SetAutoAllocate(plan, autoAllocate); }

        /// <summary>
        /// cufftSetWorkArea() overrides the work area pointer associated with a plan. 
        /// If the work area was auto-allocated, cuFFT frees the auto-allocated space. 
        /// The cufftExecute*() calls assume that the work area pointer is valid and that it points to a contiguous region in device memory that does not overlap with any other work area. 
        /// If this is not the case, results are indeterminate. 
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="workArea">Pointer to workArea. For multiple GPUs, multiple work area pointers must be given.</param>
        /// <returns></returns>
        public static cufftResult SetWorkArea(cufftHandle plan, IntPtr workArea) { return instance.SetWorkArea(plan, workArea); }

        /// <summary>
        /// Following a call to cufftCreate() makes an FFT plan configuration of dimension rank, with sizes specified in the array n. The batch input parameter tells cuFFT how many transforms to configure. With this function, batched plans of 1, 2, or 3 dimensions may be created. 
        /// Type specifiers inputtype, outputtype and executiontype dictate type and precision of transform to be performed. Not all combinations of parameters are supported. Currently all three parameters need to match precision. Parameters inputtype and outputtype need to match transform type complex-to-complex, real-to-complex or complex-to-real. Parameter executiontype needs to match precision and be of a complex type. Example: for a 16 bit real-to-complex transform parameters inputtype, outputtype and executiontype would have values of CUDA_R_16F, CUDA_C_16F and CUDA_C_16F respectively. 
        /// The cufftXtMakePlanMany() API supports more complicated input and output data layouts via the advanced data layout parameters: inembed, istride, idist, onembed, ostride, and odist. 
        /// If inembed and onembed are set to NULL, all other stride information is ignored, and default strides are used. The default assumes contiguous data arrays. 
        /// If cufftXtSetGPUs() was called prior to this call with multiple GPUs, then workSize will contain multiple sizes. See sections on multiple GPUs for more details. 
        /// All arrays are assumed to be in CPU memory.
        /// </summary>
        /// <param name="plan">cufftHandle returned by cufftCreate</param>
        /// <param name="rank">Dimensionality of the transform (1, 2, or 3)</param>
        /// <param name="n">Array of size rank, describing the size of each dimension, n[0] being the size of the innermost deminsion. For multiple GPUs and rank equal to 1, the sizes must be a power of 2. For multiple GPUs and rank equal to 2 or 3, the sizes must be factorable into primes less than or equal to 127. </param>
        /// <param name="inembed">Pointer of size rank that indicates the storage dimensions of the input data in memory, inembed[0] being the storage dimension of the innermost dimension. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="istride">Indicates the distance between two successive input elements in the least significant (i.e., innermost) dimension</param>
        /// <param name="idist">Indicates the distance between the first element of two consecutive signals in a batch of the input data</param>
        /// <param name="inputtype">Type of input data.</param>
        /// <param name="onembed">Pointer of size rank that indicates the storage dimensions of the output data in memory, inembed[0] being the storage dimension of the innermost dimension. If set to NULL all other advanced data layout parameters are ignored. </param>
        /// <param name="ostride">Indicates the distance between two successive output elements in the output array in the least significant (i.e., innermost) dimension </param>
        /// <param name="odist">Indicates the distance between the first element of two consecutive signals in a batch of the output data</param>
        /// <param name="outputtype">Type of output data.</param>
        /// <param name="batch">Batch size for this transform</param>
        /// <param name="workSize">Pointer to the size(s), in bytes, of the work areas. For example for two GPUs worksize must be declared to have two elements.</param>
        /// <param name="executiontype">Type of data to be used for computations.</param>
        /// <returns></returns>
        public static cufftResult XtMakePlanMany(cufftHandle plan, int rank, long[] n, long[] inembed, long istride, long idist, cudaDataType_t inputtype, long[] onembed, long ostride, long odist, cudaDataType_t outputtype, long batch, out size_t workSize, cudaDataType_t executiontype)
        {

            var n_handle = GCHandle.Alloc(n, GCHandleType.Pinned);
            var inembed_handle = GCHandle.Alloc(inembed, GCHandleType.Pinned);
            var onembed_handle = GCHandle.Alloc(onembed, GCHandleType.Pinned);
            IntPtr d_n = n_handle.AddrOfPinnedObject();
            IntPtr d_inembed = inembed_handle.AddrOfPinnedObject();
            IntPtr d_onembed = onembed_handle.AddrOfPinnedObject();
            cufftResult result = instance.XtMakePlanMany(plan, rank, d_n, d_inembed, istride, idist, inputtype, d_onembed, ostride, odist, outputtype, batch, out workSize, executiontype);
            cuda.DeviceSynchronize();
            n_handle.Free();
            inembed_handle.Free();
            onembed_handle.Free();
            return result;
        }
    }
}