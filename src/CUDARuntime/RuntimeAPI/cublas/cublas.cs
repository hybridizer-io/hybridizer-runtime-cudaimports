/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Runtime.InteropServices;
using System.Text;
using static Hybridizer.Runtime.CUDAImports.cublasImplem;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// cuBLAS mapping
    /// </summary>
    public class cublas
    {
        // TODO : depend on configuration
        static ICUBLAS instance  { get; set; }

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
        /// builds a cublas instance with requested cuda version (5.0 to 9.1)
        /// </summary>
        public cublas()
        {
            string cudaVersion = GetCudaVersion();
            switch (cudaVersion)
            {
                case "50":
                    instance = (IntPtr.Size == 8) ? new CUBLAS_64_50() : (ICUBLAS)new CUBLAS_32_50();
                    break;
                case "55":
                    instance = (IntPtr.Size == 8) ? new CUBLAS_64_55() : (ICUBLAS)new CUBLAS_32_55();
                    break;
                case "60":
                    instance = (IntPtr.Size == 8) ? new CUBLAS_64_60() : (ICUBLAS)new CUBLAS_32_60();
                    break;
                case "65":
                    instance = (IntPtr.Size == 8) ? new CUBLAS_64_65() : (ICUBLAS)new CUBLAS_32_65();
                    break;
                case "70":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_70();
                    else
                        throw new NotSupportedException("cublas 7.0 dropped 32 bits support");
                    break;
                case "75":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_75();
                    else
                        throw new NotSupportedException("cublas 7.5 dropped 32 bits support");
                    break;
                case "80":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_80();
                    else
                        throw new NotSupportedException("cublas 8.0 dropped 32 bits support");
                    break;
                case "90":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_90();
                    else
                        throw new NotSupportedException("cublas 9.0 dropped 32 bits support");
                    break;
                case "91":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_91();
                    else
                        throw new NotSupportedException("cublas 9.1 dropped 32 bits support");
                    break;
                case "92":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_92();
                    else
                        throw new NotSupportedException("cublas 9.2 dropped 32 bits support");
                    break;
                case "100":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_100();
                    else
                        throw new NotSupportedException("cublas 10.0 dropped 32 bits support");
                    break;
                case "101":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_101();
                    else
                        throw new NotSupportedException("cublas 10.1 dropped 32 bits support");
                    break;
                case "110":
                case "114":
                case "116":
                case "117":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_110();
                    else
                        throw new NotSupportedException("cublas 11 dropped 32 bits support");
                    break;
                case "120":
                case "121":
                case "122":
                    if (IntPtr.Size == 8)
                        instance = new CUBLAS_64_120();
                    else
                        throw new NotSupportedException("cublas 11 dropped 32 bits support");
                    break;
                default:
                    throw new ApplicationException(string.Format("Unknown version of Cuda {0}", cudaVersion));
            }
        }

        #region Helper Functions

        /// <summary>
        /// This function initializes the CUBLAS library and creates a handle to an opaque structure holding the CUBLAS library context. It allocates hardware resources on the host and device and must be called prior to making any other CUBLAS library calls. The CUBLAS library context is tied to the current CUDA device. To use the library on multiple devices, one CUBLAS handle needs to be created for each device. Furthermore, for a given device, multiple CUBLAS handles with different configuration can be created. Because cublasCreate allocates some internal resources and the release of those resources by calling cublasDestroy will implicitly call cublasDeviceSynchronize, it is recommended to minimize the number of cublasCreate/cublasDestroy occurences. For multi-threaded applications that use the same device from different threads, the recommended programming model is to create one CUBLAS handle per thread and use that CUBLAS handle for the entire life of the thread. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublascreate">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public cublasStatus_t Create(out cublasHandle_t handle) { return instance.Create(out handle); }
        /// <summary>
        /// This function releases hardware resources used by the CUBLAS library. This function is usually the last call with a particular handle to the CUBLAS library. Because cublasCreate allocates some internal resources and the release of those resources by calling cublasDestroy will implicitly call cublasDeviceSynchronize, it is recommended to minimize the number of cublasCreate/cublasDestroy occurences. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasdestroy">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public cublasStatus_t Destroy(cublasHandle_t handle) { return instance.Destroy(handle); }
        /// <summary>
        /// This function returns the version number of the cuBLAS library.
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetversion">nvidia documentation</see>
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public cublasStatus_t GetVersion(out int version) { return instance.GetVersion(out version); }
        /// <summary>
        /// This function sets the cuBLAS library stream, which will be used to execute all subsequent calls to the cuBLAS library functions. If the cuBLAS library stream is not set, all kernels use the defaultNULL stream. In particular, this routine can be used to change the stream between kernel launches and then to reset the cuBLAS library stream back to NULL. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublassetstream">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="streamId"></param>
        /// <returns></returns>
        public cublasStatus_t SetStream(cublasHandle_t handle, cudaStream_t streamId) { return instance.SetStream(handle, streamId); }
        /// <summary>
        /// This function gets the cuBLAS library stream, which is being used to execute all calls to the cuBLAS library functions. If the cuBLAS library stream is not set, all kernels use the defaultNULL stream. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetstream">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="streamId"></param>
        /// <returns></returns>
        public cublasStatus_t GetStream(cublasHandle_t handle, out cudaStream_t streamId) { return instance.GetStream(handle, out streamId); }
        /// <summary>
        /// This function sets the pointer mode used by the cuBLAS library. The default is for the values to be passed by reference on the host. Please see the section on the cublasPointerMode_t type for more details. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublassetpointermode">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public cublasStatus_t SetPointerMode(cublasHandle_t handle, cublasPointerMode_t mode) { return instance.SetPointerMode(handle, mode); }
        /// <summary>
        /// This function obtains the pointer mode used by the cuBLAS library. Please see the section on the cublasPointerMode_t type for more details. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetpointermode">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public cublasStatus_t GetPointerMode(cublasHandle_t handle, out cublasPointerMode_t mode) { return instance.GetPointerMode(handle, out mode); }
        /// <summary>
        /// This function copies n elements from a vector x in host memory space to a vector y in GPU memory space. Elements in both vectors are assumed to have a size of elemSize bytes. The storage spacing between consecutive elements is given by incx for the source vector x and by incy for the destination vector y. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublassetvector">nvidia documentation</see>
        /// </summary>
        /// <param name="n"></param>
        /// <param name="elemSize"></param>
        /// <param name="x"></param>
        /// <param name="incx"></param>
        /// <param name="y"></param>
        /// <param name="incy"></param>
        /// <returns></returns>
        public cublasStatus_t SetVector(int n, int elemSize, IntPtr x, int incx, IntPtr y, int incy) { return instance.SetVector(n, elemSize, x, incx, y, incy); }
        /// <summary>
        /// This function copies n elements from a vector x in GPU memory space to a vector y in host memory space. Elements in both vectors are assumed to have a size of elemSize bytes. The storage spacing between consecutive elements is given by incx for the source vector and incy for the destination vector y. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetvector">nvidia documentation</see>
        /// </summary>
        /// <param name="n"></param>
        /// <param name="elemSize"></param>
        /// <param name="x"></param>
        /// <param name="incx"></param>
        /// <param name="y"></param>
        /// <param name="incy"></param>
        /// <returns></returns>
        public cublasStatus_t GetVector(int n, int elemSize, IntPtr x, int incx, IntPtr y, int incy) { return instance.GetVector(n, elemSize, x, incx, y, incy); }
        /// <summary>
        /// This function copies a tile of rows x cols elements from a matrix A in GPU memory space to a matrix B in host memory space. It is assumed that each element requires storage of elemSize bytes and that both matrices are stored in column-major format, with the leading dimension of the source matrix A and destination matrix B given in lda and ldb, respectively. The leading dimension indicates the number of rows of the allocated matrix, even if only a submatrix of it is being used. In general, A is a device pointer that points to an object, or part of an object, that was allocated in GPU memory space via cublasAlloc(). 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetmatrix">nvidia documentation</see>
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="elemSize"></param>
        /// <param name="A"></param>
        /// <param name="lda"></param>
        /// <param name="B"></param>
        /// <param name="ldb"></param>
        /// <returns></returns>
        public cublasStatus_t GetMatrix(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb) { return instance.GetMatrix(rows, cols, elemSize, A, lda, B, ldb); }
        /// <summary>
        /// This function copies a tile of rows x cols elements from a matrix A in host memory space to a matrix B in GPU memory space. It is assumed that each element requires storage of elemSize bytes and that both matrices are stored in column-major format, with the leading dimension of the source matrix A and destination matrix B given in lda and ldb, respectively. The leading dimension indicates the number of rows of the allocated matrix, even if only a submatrix of it is being used. In general, B is a device pointer that points to an object, or part of an object, that was allocated in GPU memory space via cublasAlloc(). 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublassetmatrix">nvidia documentation</see>
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="elemSize"></param>
        /// <param name="A"></param>
        /// <param name="lda"></param>
        /// <param name="B"></param>
        /// <param name="ldb"></param>
        /// <returns></returns>
        public cublasStatus_t SetMatrix(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb) { return instance.SetMatrix(rows, cols, elemSize, A, lda, B, ldb); }
        /// <summary>
        /// This function has the same functionality as cublasSetVector(), with the exception that the data transfer is done asynchronously (with respect to the host) using the given CUDA™ stream parameter. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublassetvectorasync">nvidia documentation</see>
        /// </summary>
        /// <param name="n"></param>
        /// <param name="elemSize"></param>
        /// <param name="hostPtr"></param>
        /// <param name="incx"></param>
        /// <param name="devicePtr"></param>
        /// <param name="incy"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public cublasStatus_t SetVectorAsync(int n, int elemSize, IntPtr hostPtr, int incx, IntPtr devicePtr, int incy, cudaStream_t stream) { return instance.SetVectorAsync(n, elemSize, hostPtr, incx, devicePtr, incy, stream); }
        /// <summary>
        ///  This function has the same functionality as cublasGetVector(), with the exception that the data transfer is done asynchronously (with respect to the host) using the given CUDA™ stream parameter. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetvectorasync">nvidia documentation</see>
        /// </summary>
        /// <param name="n"></param>
        /// <param name="elemSize"></param>
        /// <param name="devicePtr"></param>
        /// <param name="incx"></param>
        /// <param name="hostPtr"></param>
        /// <param name="incy"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public cublasStatus_t GetVectorAsync(int n, int elemSize, IntPtr devicePtr, int incx, IntPtr hostPtr, int incy, cudaStream_t stream) { return instance.GetVectorAsync(n, elemSize, hostPtr, incx, devicePtr, incy, stream); }
        /// <summary>
        /// This function has the same functionality as cublasSetMatrix(), with the exception that the data transfer is done asynchronously (with respect to the host) using the given CUDA™ stream parameter. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublassetmatrixasync">nvidia documentation</see>
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="elemSize"></param>
        /// <param name="A"></param>
        /// <param name="lda"></param>
        /// <param name="B"></param>
        /// <param name="ldb"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public cublasStatus_t SetMatrixAsync(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb, cudaStream_t stream) { return instance.SetMatrixAsync (rows, cols, elemSize, A, lda, B, ldb, stream); }
        /// <summary>
        /// This function has the same functionality as cublasGetMatrix(), with the exception that the data transfer is done asynchronously (with respect to the host) using the given CUDA™ stream parameter. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetmatrixasync">nvidia documentation</see>
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="elemSize"></param>
        /// <param name="A"></param>
        /// <param name="lda"></param>
        /// <param name="B"></param>
        /// <param name="ldb"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public cublasStatus_t GetMatrixAsync(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb, cudaStream_t stream) { return instance.GetMatrixAsync(rows, cols, elemSize, A, lda, B, ldb, stream); }
        /// <summary>
        /// Some routines like cublas&lt;t&gt;symv and cublas&lt;t&gt;hemv have an alternate implementation that use atomics to cumulate results. This implementation is generally significantly faster but can generate results that are not strictly identical from one run to the others. Mathematically, those different results are not significant but when debugging those differences can be prejudicial. 
        /// This function allows or disallows the usage of atomics in the cuBLAS library for all routines which have an alternate implementation. When not explicitly specified in the documentation of any cuBLAS routine, it means that this routine does not have an alternate implementation that use atomics. When atomics mode is disabled, each cuBLAS routine should produce the same results from one run to the other when called with identical parameters on the same Hardware. 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublassetatomicsmode">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public cublasStatus_t SetAtomicsMode(cublasHandle_t handle, cublasAtomicsMode_t mode) { return instance.SetAtomicsMode(handle, mode); }
        /// <summary>
        /// This function queries the atomic mode of a specific cuBLAS context.
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublasgetatomicsmode">nvidia documentation</see>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public cublasStatus_t GetAtomicsMode(cublasHandle_t handle, out cublasAtomicsMode_t mode) { return instance.GetAtomicsMode(handle, out mode); }

        #endregion

        #region Level-1 Functions

        /// <summary>
        /// This function finds the (smallest) index of the element of the maximum magnitude. 
        /// </summary>
        public cublasStatus_t Isamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Isamax(handle, n, x, incx, result); }
        /// <summary>
        /// This function finds the (smallest) index of the element of the maximum magnitude. 
        /// </summary>
        public cublasStatus_t Idamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Idamax(handle, n, x, incx, result); }
        /// <summary>
        /// This function finds the (smallest) index of the element of the maximum magnitude. 
        /// </summary>
        public cublasStatus_t Icamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Icamax(handle, n, x, incx, result); }
        /// <summary>
        /// This function finds the (smallest) index of the element of the maximum magnitude. 
        /// </summary>
        public cublasStatus_t Izamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Izamax(handle, n, x, incx, result); }

        /// <summary>
        /// This function finds the (smallest) index of the element of the minimum magnitude
        /// </summary>
        public cublasStatus_t Isamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Isamin(handle, n, x, incx, result); }
        /// <summary>
        /// This function finds the (smallest) index of the element of the minimum magnitude
        /// </summary>
        public cublasStatus_t Idamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Idamin(handle, n, x, incx, result); }
        /// <summary>
        /// This function finds the (smallest) index of the element of the minimum magnitude
        /// </summary>
        public cublasStatus_t Icamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Icamin(handle, n, x, incx, result); }
        /// <summary>
        /// This function finds the (smallest) index of the element of the minimum magnitude
        /// </summary>
        public cublasStatus_t Izamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Izamin(handle, n, x, incx, result); }

        /// <summary>
        /// This function computes the sum of the absolute values of the elements of vector x
        /// </summary>
        public cublasStatus_t Sasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Sasum(handle, n, x, incx, result); }
        /// <summary>
        /// This function computes the sum of the absolute values of the elements of vector x
        /// </summary>
        public cublasStatus_t Dasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Dasum(handle, n, x, incx, result); }
        /// <summary>
        /// This function computes the sum of the absolute values of the elements of vector x
        /// </summary>
        public cublasStatus_t Scasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Scasum(handle, n, x, incx, result); }
        /// <summary>
        /// This function computes the sum of the absolute values of the elements of vector x
        /// </summary>
        public cublasStatus_t Dzasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Dzasum(handle, n, x, incx, result); }

        /// <summary>
        /// This function multiplies the vector x by the scalar α and adds it to the vector y overwriting the latest vector with the result
        /// </summary>
        public cublasStatus_t Saxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return instance.Saxpy(handle, n, alpha, x, incx, y, incy); }
        /// <summary>
        /// This function multiplies the vector x by the scalar α and adds it to the vector y overwriting the latest vector with the result
        /// </summary>
        public cublasStatus_t Daxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return instance.Daxpy(handle, n, alpha, x, incx, y, incy); }
        /// <summary>
        /// This function multiplies the vector x by the scalar α and adds it to the vector y overwriting the latest vector with the result
        /// </summary>
        public cublasStatus_t Caxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return instance.Caxpy(handle, n, alpha, x, incx, y, incy); }
        /// <summary>
        /// This function multiplies the vector x by the scalar α and adds it to the vector y overwriting the latest vector with the result
        /// </summary>
        public cublasStatus_t Zaxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return instance.Zaxpy(handle, n, alpha, x, incx, y, incy); }

        /// <summary>
        /// This function copies the vector x into the vector y.
        /// </summary>
        public cublasStatus_t Scopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Scopy(handle, n, x, incx, y, incy); }
        /// <summary>
        /// This function copies the vector x into the vector y.
        /// </summary>
        public cublasStatus_t Dcopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Dcopy(handle, n, x, incx, y, incy); }
        /// <summary>
        /// This function copies the vector x into the vector y.
        /// </summary>
        public cublasStatus_t Ccopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Ccopy(handle, n, x, incx, y, incy); }
        /// <summary>
        /// This function copies the vector x into the vector y.
        /// </summary>
        public cublasStatus_t Zcopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Zcopy(handle, n, x, incx, y, incy); }

        /// <summary>
        /// This function computes the dot product of vectors x and y
        /// </summary>
        public cublasStatus_t Sdot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return instance.Sdot(handle, n, x, incx, y, incy, result); }
        /// <summary>
        /// This function computes the dot product of vectors x and y
        /// </summary>
        public cublasStatus_t Ddot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return instance.Ddot(handle, n, x, incx, y, incy, result); }
        /// <summary>
        /// This function computes the dot product of vectors x and y
        /// </summary>
        public cublasStatus_t Cdotu(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return instance.Cdotu(handle, n, x, incx, y, incy, result); }
        /// <summary>
        /// This function computes the dot product of vectors x and y
        /// </summary>
        public cublasStatus_t Cdotc(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return instance.Cdotc(handle, n, x, incx, y, incy, result); }
        /// <summary>
        /// This function computes the dot product of vectors x and y
        /// </summary>
        public cublasStatus_t Zdotu(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return instance.Zdotu(handle, n, x, incx, y, incy, result); }
        /// <summary>
        /// This function computes the dot product of vectors x and y
        /// </summary>
        public cublasStatus_t Zdotc(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return instance.Zdotc(handle, n, x, incx, y, incy, result); }

        /// <summary>
        /// This function computes the Euclidean norm of the vector x
        /// </summary>
        public cublasStatus_t Snrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Snrm2(handle, n, x, incx, result); }
        /// <summary>
        /// This function computes the Euclidean norm of the vector x
        /// </summary>
        public cublasStatus_t Dnrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Dnrm2(handle, n, x, incx, result); }
        /// <summary>
        /// This function computes the Euclidean norm of the vector x
        /// </summary>
        public cublasStatus_t Scnrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Scnrm2(handle, n, x, incx, result); }
        /// <summary>
        /// This function computes the Euclidean norm of the vector x
        /// </summary>
        public cublasStatus_t Dznrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return instance.Dznrm2(handle, n, x, incx, result); }

        /// <summary>
        /// This function applies Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Srot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return instance.Srot(handle, n, x, incx, y, incy, c, s); }
        /// <summary>
        /// This function applies Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Drot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return instance.Drot(handle, n, x, incx, y, incy, c, s); }
        /// <summary>
        /// This function applies Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Crot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return instance.Crot(handle, n, x, incx, y, incy, c, s); }
        /// <summary>
        /// This function applies Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Csrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return instance.Csrot(handle, n, x, incx, y, incy, c, s); }
        /// <summary>
        /// This function applies Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Zrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return instance.Zrot(handle, n, x, incx, y, incy, c, s); }
        /// <summary>
        /// This function applies Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Zdrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return instance.Zdrot(handle, n, x, incx, y, incy, c, s); }

        /// <summary>
        /// This function constructs the Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Srotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return instance.Srotg(handle, a, b, c, s); }
        /// <summary>
        /// This function constructs the Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Drotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return instance.Drotg(handle, a, b, c, s); }
        /// <summary>
        /// This function constructs the Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Crotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return instance.Crotg(handle, a, b, c, s); }
        /// <summary>
        /// This function constructs the Givens rotation matrix (c, s; -s, c)
        /// </summary>
        public cublasStatus_t Zrotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return instance.Zrotg(handle, a, b, c, s); }

        /// <summary>
        /// This function applies the modified Givens transformation (h11, h12; h21, h22)
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-rotm">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Srotm(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr param) { return instance.Srotm(handle, n, x, incx, y, incy, param); }
        /// <summary>
        /// This function applies the modified Givens transformation (h11, h12; h21, h22)
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-rotm">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Drotm(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr param) { return instance.Drotm(handle, n, x, incx, y, incy, param); }

        /// <summary>
        /// This function constructs the modified Givens transformation (h11, h12; h21, h22)
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-rotmg">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Srotmg(cublasHandle_t handle, IntPtr d1, IntPtr d2, IntPtr x1, IntPtr y1, IntPtr param) { return instance.Srotmg(handle, d1, d2, x1, y1, param); }
        /// <summary>
        /// This function constructs the modified Givens transformation (h11, h12; h21, h22)
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-rotmg">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Drotmg(cublasHandle_t handle, IntPtr d1, IntPtr d2, IntPtr x1, IntPtr y1, IntPtr param) { return instance.Drotmg(handle, d1, d2, x1, y1, param); }

        /// <summary>
        /// This function scales the vector x by the scalar α and overwrites it with the result
        /// </summary>
        public cublasStatus_t Sscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return instance.Sscal(handle, n, alpha, x, incx); }
        /// <summary>
        /// This function scales the vector x by the scalar α and overwrites it with the result
        /// </summary>
        public cublasStatus_t Dscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return instance.Dscal(handle, n, alpha, x, incx); }
        /// <summary>
        /// This function scales the vector x by the scalar α and overwrites it with the result
        /// </summary>
        public cublasStatus_t Cscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return instance.Cscal(handle, n, alpha, x, incx); }
        /// <summary>
        /// This function scales the vector x by the scalar α and overwrites it with the result
        /// </summary>
        public cublasStatus_t Csscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return instance.Csscal(handle, n, alpha, x, incx); }
        /// <summary>
        /// This function scales the vector x by the scalar α and overwrites it with the result
        /// </summary>
        public cublasStatus_t Zscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return instance.Zscal(handle, n, alpha, x, incx); }
        /// <summary>
        /// This function scales the vector x by the scalar α and overwrites it with the result
        /// </summary>
        public cublasStatus_t Zdscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return instance.Zdscal(handle, n, alpha, x, incx); }

        /// <summary>
        /// This function interchanges the elements of vector x and y
        /// </summary>
        public cublasStatus_t Sswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Sswap(handle, n, x, incx, y, incy); }
        /// <summary>
        /// This function interchanges the elements of vector x and y
        /// </summary>
        public cublasStatus_t Dswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Dswap(handle, n, x, incx, y, incy); }
        /// <summary>
        /// This function interchanges the elements of vector x and y
        /// </summary>
        public cublasStatus_t Cswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Cswap(handle, n, x, incx, y, incy); }
        /// <summary>
        /// This function interchanges the elements of vector x and y
        /// </summary>
        public cublasStatus_t Zswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return instance.Zswap(handle, n, x, incx, y, incy); }

        #endregion

        #region Level-2 Functions

        /// <summary>
        /// This function performs the banded matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gbmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Sgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Sgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the banded matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gbmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Dgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the banded matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gbmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Cgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Cgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the banded matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gbmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Zgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Zgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }

        /// <summary>
        /// This function performs the matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gemv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Sgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Sgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gemv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Dgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gemv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Cgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Cgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the matrix-vector multiplication y = α op(A ) x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-gemv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Zgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Zgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }

        /// <summary>
        /// This function performs the rank-1 update A =  α x y^T + A if ger(),geru() is called  or  α x y^H + A if gerc() is called 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-ger">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Sger(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Sger(handle, m, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the rank-1 update A =  α x y^T + A if ger(),geru() is called  or  α x y^H + A if gerc() is called 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-ger">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dger(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Dger(handle, m, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the rank-1 update A =  α x y^T + A if ger(),geru() is called  or  α x y^H + A if gerc() is called 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-ger">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Cgeru(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Cgeru(handle, m, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the rank-1 update A =  α x y^T + A if ger(),geru() is called  or  α x y^H + A if gerc() is called 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-ger">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Cgerc(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Cgerc(handle, m, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the rank-1 update A =  α x y^T + A if ger(),geru() is called  or  α x y^H + A if gerc() is called 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-ger">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Zgeru(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Zgeru(handle, m, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the rank-1 update A =  α x y^T + A if ger(),geru() is called  or  α x y^H + A if gerc() is called 
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-ger">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Zgerc(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Zgerc(handle, m, n, alpha, x, incx, y, incy, A, lda); }

        /// <summary>
        /// This function performs the symmetric banded matrix-vector multiplication
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-sbmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Ssbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Ssbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the symmetric banded matrix-vector multiplication
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-sbmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dsbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Dsbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }

        /// <summary>
        /// This function performs the symmetric packed matrix-vector multiplication y = α A x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-spmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Sspmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Sspmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the symmetric packed matrix-vector multiplication y = α A x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-spmv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dspmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Dspmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }

        /// <summary>
        /// This function performs the packed symmetric rank-1 update A = α x x^T + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-spr">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Sspr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return instance.Sspr(handle, uplo, n, alpha, x, incx, AP); }
        /// <summary>
        /// This function performs the packed symmetric rank-1 update A = α x x^T + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-spr">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dspr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return instance.Dspr(handle, uplo, n, alpha, x, incx, AP); }


        /// <summary>
        /// This function performs the packed symmetric rank-2 update A = α(x y^T + y x^T) + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-spr2">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Sspr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return instance.Sspr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }
        /// <summary>
        /// This function performs the packed symmetric rank-2 update A = α(x y^T + y x^T) + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-spr2">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dspr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return instance.Dspr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }

        /// <summary>
        /// This function performs the symmetric matrix-vector multiplication. y = α A x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-symv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Ssymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Ssymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the symmetric matrix-vector multiplication. y = α A x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-symv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Dsymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the symmetric matrix-vector multiplication. y = α A x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-symv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Csymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Csymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the symmetric matrix-vector multiplication. y = α A x + β y
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-symv">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Zsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Zsymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }


        /// <summary>
        /// This function performs the symmetric rank-1 update A = α x x<sup>T</sup> + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Ssyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return instance.Ssyr(handle, uplo, n, alpha, x, incx, A, lda); }
        /// <summary>
        /// This function performs the symmetric rank-1 update A = α x x<sup>T</sup> + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return instance.Dsyr(handle, uplo, n, alpha, x, incx, A, lda); }
        /// <summary>
        /// This function performs the symmetric rank-1 update A = α x x<sup>T</sup> + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Csyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return instance.Csyr(handle, uplo, n, alpha, x, incx, A, lda); }
        /// <summary>
        /// This function performs the symmetric rank-1 update A = α x x<sup>T</sup> + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Zsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return instance.Zsyr(handle, uplo, n, alpha, x, incx, A, lda); }

        /// <summary>
        /// This function performs the symmetric rank-2 update A = α (x y<sup>T</sup> + y x <sup>T</sup>) + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr2">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Ssyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Ssyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the symmetric rank-2 update A = α (x y<sup>T</sup> + y x <sup>T</sup>) + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr2">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Dsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Dsyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the symmetric rank-2 update A = α (x y<sup>T</sup> + y x <sup>T</sup>) + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr2">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Csyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Csyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the symmetric rank-2 update A = α (x y<sup>T</sup> + y x <sup>T</sup>) + A
        /// Complete documentation on <see href="https://docs.nvidia.com/cuda/cublas/index.html#cublas-lt-t-gt-syr2">Nvidia documentation</see>
        /// </summary>
        public cublasStatus_t Zsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Zsyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }

        /// <summary>
        /// This function performs the triangular banded matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Stbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Stbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }
        /// <summary>
        /// This function performs the triangular banded matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Dtbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Dtbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }
        /// <summary>
        /// This function performs the triangular banded matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Ctbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ctbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }
        /// <summary>
        /// This function performs the triangular banded matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Ztbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ztbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

        /// <summary>
        /// This function solves the triangular banded linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Stbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Stbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }
        /// <summary>
        /// This function solves the triangular banded linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Dtbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Dtbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }
        /// <summary>
        /// This function solves the triangular banded linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Ctbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ctbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }
        /// <summary>
        /// This function solves the triangular banded linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Ztbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ztbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

        /// <summary>
        /// This function performs the triangular packed matrix-vector multiplication x = op (A) x 
        /// </summary>
        public cublasStatus_t Stpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Stpmv(handle, uplo, trans, diag, n, AP, x, incx); }
        /// <summary>
        /// This function performs the triangular packed matrix-vector multiplication x = op (A) x 
        /// </summary>
        public cublasStatus_t Dtpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Dtpmv(handle, uplo, trans, diag, n, AP, x, incx); }
        /// <summary>
        /// This function performs the triangular packed matrix-vector multiplication x = op (A) x 
        /// </summary>
        public cublasStatus_t Ctpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Ctpmv(handle, uplo, trans, diag, n, AP, x, incx); }
        /// <summary>
        /// This function performs the triangular packed matrix-vector multiplication x = op (A) x 
        /// </summary>
        public cublasStatus_t Ztpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Ztpmv(handle, uplo, trans, diag, n, AP, x, incx); }

        /// <summary>
        /// This function solves the packed triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Stpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Stpsv(handle, uplo, trans, diag, n, AP, x, incx); }
        /// <summary>
        /// This function solves the packed triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Dtpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Dtpsv(handle, uplo, trans, diag, n, AP, x, incx); }
        /// <summary>
        /// This function solves the packed triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Ctpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Ctpsv(handle, uplo, trans, diag, n, AP, x, incx); }
        /// <summary>
        /// This function solves the packed triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Ztpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return instance.Ztpsv(handle, uplo, trans, diag, n, AP, x, incx); }

        /// <summary>
        /// This function performs the triangular matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Strmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Strmv(handle, uplo, trans, diag, n, A, lda, x, incx); }
        /// <summary>
        /// This function performs the triangular matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Dtrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Dtrmv(handle, uplo, trans, diag, n, A, lda, x, incx); }
        /// <summary>
        /// This function performs the triangular matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Ctrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ctrmv(handle, uplo, trans, diag, n, A, lda, x, incx); }
        /// <summary>
        /// This function performs the triangular matrix-vector multiplication x = op(A) x
        /// </summary>
        public cublasStatus_t Ztrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ztrmv(handle, uplo, trans, diag, n, A, lda, x, incx); }

        /// <summary>
        /// This function solves the triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Strsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Strsv(handle, uplo, trans, diag, n, A, lda, x, incx); }
        /// <summary>
        /// This function solves the triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Dtrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Dtrsv(handle, uplo, trans, diag, n, A, lda, x, incx); }
        /// <summary>
        /// This function solves the triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Ctrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ctrsv(handle, uplo, trans, diag, n, A, lda, x, incx); }
        /// <summary>
        /// This function solves the triangular linear system with a single right-hand-side op(A) x = b
        /// </summary>
        public cublasStatus_t Ztrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return instance.Ztrsv(handle, uplo, trans, diag, n, A, lda, x, incx); }

        /// <summary>
        /// This function performs the Hermitian matrix-vector multiplication y = α A x + β y
        /// </summary>
        public cublasStatus_t Chemv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Chemv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the Hermitian matrix-vector multiplication y = α A x + β y
        /// </summary>
        public cublasStatus_t Zhemv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Zhemv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }

        /// <summary>
        /// This function performs the Hermitian banded matrix-vector multiplication y = α A x + β y
        /// </summary>
        public cublasStatus_t Chbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Chbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the Hermitian banded matrix-vector multiplication y = α A x + β y
        /// </summary>
        public cublasStatus_t Zhbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Zhbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }

        /// <summary>
        /// This function performs the Hermitian packed matrix-vector multiplication y = α A x + β y
        /// </summary>
        public cublasStatus_t Chpmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Chpmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }
        /// <summary>
        /// This function performs the Hermitian packed matrix-vector multiplication y = α A x + β y
        /// </summary>
        public cublasStatus_t Zhpmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return instance.Zhpmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }

        /// <summary>
        /// This function performs the Hermitian rank-1 update A = α x x<sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Cher(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return instance.Cher(handle, uplo, n, alpha, x, incx, A, lda); }
        /// <summary>
        /// This function performs the Hermitian rank-1 update A = α x x<sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Zher(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return instance.Zher(handle, uplo, n, alpha, x, incx, A, lda); }

        /// <summary>
        /// This function performs the Hermitian rank-2 update A = α x y<sup>H</sup> + conj(α) y x <sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Cher2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Cher2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }
        /// <summary>
        /// This function performs the Hermitian rank-2 update A = α x y<sup>H</sup> + conj(α) y x <sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Zher2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return instance.Zher2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }

        /// <summary>
        /// This function performs the packed Hermitian rank-1 update A = α x x<sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Chpr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return instance.Chpr(handle, uplo, n, alpha, x, incx, AP); }
        /// <summary>
        /// This function performs the packed Hermitian rank-1 update A = α x x<sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Zhpr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return instance.Zhpr(handle, uplo, n, alpha, x, incx, AP); }

        /// <summary>
        /// This function performs the packed Hermitian rank-2 update A = α x y<sup>H</sup> + conj(α) y x <sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Chpr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return instance.Chpr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }
        /// <summary>
        /// This function performs the packed Hermitian rank-2 update A = α x y<sup>H</sup> + conj(α) y x <sup>H</sup> + A
        /// </summary>
        public cublasStatus_t Zhpr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return instance.Zhpr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }

        #endregion

        #region Level-3 Functions
        /// <summary>
        /// This function performs the matrix-matrix multiplication C = α op(A ) op(B ) + β C
        /// </summary>
        public cublasStatus_t Sgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Sgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the matrix-matrix multiplication C = α op(A ) op(B ) + β C
        /// </summary>
        public cublasStatus_t Dgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Dgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the matrix-matrix multiplication C = α op(A ) op(B ) + β C
        /// </summary>
        public cublasStatus_t Cgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Cgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the matrix-matrix multiplication C = α op(A ) op(B ) + β C
        /// </summary>
        public cublasStatus_t Zgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Zgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

        /// <summary>
        /// This function performs the symmetric matrix-matrix multiplication C =  α A B + β C if  side == CUBLAS_SIDE_LEFT ;  α B A + β C if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Ssymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Ssymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric matrix-matrix multiplication C =  α A B + β C if  side == CUBLAS_SIDE_LEFT ;  α B A + β C if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Dsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Dsymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric matrix-matrix multiplication C =  α A B + β C if  side == CUBLAS_SIDE_LEFT ;  α B A + β C if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Csymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Csymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric matrix-matrix multiplication C =  α A B + β C if  side == CUBLAS_SIDE_LEFT ;  α B A + β C if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Zsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Zsymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }

        /// <summary>
        /// This function performs the symmetric rank- k update C = α op(A ) op(A )<sup>T</sup> + β C
        /// </summary>
        public cublasStatus_t Ssyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return instance.Ssyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric rank- k update C = α op(A ) op(A )<sup>T</sup> + β C
        /// </summary>
        public cublasStatus_t Dsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return instance.Dsyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric rank- k update C = α op(A ) op(A )<sup>T</sup> + β C
        /// </summary>
        public cublasStatus_t Csyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return instance.Csyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric rank- k update C = α op(A ) op(A )<sup>T</sup> + β C
        /// </summary>
        public cublasStatus_t Zsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return instance.Zsyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }

        /// <summary>
        /// This function performs the symmetric rank- 2 k update  C = α ( op ( A ) op ( B )<sup>T</sup> + op ( B ) op ( A )<sup>T</sup> ) + β C 
        /// </summary>
        public cublasStatus_t Ssyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Ssyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric rank- 2 k update  C = α ( op ( A ) op ( B )<sup>T</sup> + op ( B ) op ( A )<sup>T</sup> ) + β C 
        /// </summary>
        public cublasStatus_t Dsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Dsyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric rank- 2 k update  C = α ( op ( A ) op ( B )<sup>T</sup> + op ( B ) op ( A )<sup>T</sup> ) + β C 
        /// </summary>
        public cublasStatus_t Csyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Csyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the symmetric rank- 2 k update  C = α ( op ( A ) op ( B )<sup>T</sup> + op ( B ) op ( A )<sup>T</sup> ) + β C 
        /// </summary>
        public cublasStatus_t Zsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Zsyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

        /// <summary>
        /// This function performs the triangular matrix-matrix multiplication C =  α op ( A ) B if  side == CUBLAS_SIDE_LEFT  ;  α B op ( A ) if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Strmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return instance.Strmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }
        /// <summary>
        /// This function performs the triangular matrix-matrix multiplication C =  α op ( A ) B if  side == CUBLAS_SIDE_LEFT  ;  α B op ( A ) if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Dtrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return instance.Dtrmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }
        /// <summary>
        /// This function performs the triangular matrix-matrix multiplication C =  α op ( A ) B if  side == CUBLAS_SIDE_LEFT  ;  α B op ( A ) if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Ctrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return instance.Ctrmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }
        /// <summary>
        /// This function performs the triangular matrix-matrix multiplication C =  α op ( A ) B if  side == CUBLAS_SIDE_LEFT  ;  α B op ( A ) if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Ztrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return instance.Ztrmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }

        /// <summary>
        /// This function solves the triangular linear system with multiple right-hand-sides :  op ( A ) X = α B if  side == CUBLAS_SIDE_LEFT ;  X op ( A ) = α B if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Strsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return instance.Strsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }
        /// <summary>
        /// This function solves the triangular linear system with multiple right-hand-sides :  op ( A ) X = α B if  side == CUBLAS_SIDE_LEFT ;  X op ( A ) = α B if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Dtrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return instance.Dtrsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }
        /// <summary>
        /// This function solves the triangular linear system with multiple right-hand-sides :  op ( A ) X = α B if  side == CUBLAS_SIDE_LEFT ;  X op ( A ) = α B if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Ctrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return instance.Ctrsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }
        /// <summary>
        /// This function solves the triangular linear system with multiple right-hand-sides :  op ( A ) X = α B if  side == CUBLAS_SIDE_LEFT ;  X op ( A ) = α B if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Ztrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return instance.Ztrsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }

        /// <summary>
        /// This function performs the Hermitian matrix-matrix multiplication : C =  α A B + β C if  side == CUBLAS_SIDE_LEFT ;  α B A + β C if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Chemm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Chemm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the Hermitian matrix-matrix multiplication : C =  α A B + β C if  side == CUBLAS_SIDE_LEFT ;  α B A + β C if  side == CUBLAS_SIDE_RIGHT 
        /// </summary>
        public cublasStatus_t Zhemm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Zhemm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }

        /// <summary>
        /// This function performs the Hermitian rank-k update : C = α op ( A ) op ( A ) H + β C
        /// </summary>
        public cublasStatus_t Cherk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return instance.Cherk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }
        /// <summary>
        /// This function performs the Hermitian rank-k update : C = α op ( A ) op ( A ) H + β C
        /// </summary>
        public cublasStatus_t Zherk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return instance.Zherk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }

        /// <summary>
        /// This function performs the Hermitian rank-2 k update  C = α op ( A ) op ( B )<sup>H</sup> + conj(α) op ( B ) op ( A )<sup>H</sup> + β C 
        /// </summary>
        public cublasStatus_t Cher2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Cher2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }
        /// <summary>
        /// This function performs the Hermitian rank-2 k update  C = α op ( A ) op ( B )<sup>H</sup> + conj(α) op ( B ) op ( A )<sup>H</sup> + β C 
        /// </summary>
        public cublasStatus_t Zher2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return instance.Zher2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

        #endregion
    }
}