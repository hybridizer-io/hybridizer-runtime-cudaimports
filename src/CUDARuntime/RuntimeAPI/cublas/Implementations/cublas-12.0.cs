/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// cuBLAS mapping
    /// </summary>
    internal partial class cublasImplem
    {
        // to check
        internal class CUBLAS_64_120 : ICUBLAS
        {
            public const string CUBLAS_DLL = "cublas64_11.dll";

            #region Helper Functions

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCreate")]
            public static extern cublasStatus_t cublasCreate(out cublasHandle_t handle);
            public cublasStatus_t Create(out cublasHandle_t handle) { return cublasCreate(out handle); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDestroy")]
            public static extern cublasStatus_t cublasDestroy(cublasHandle_t handle);
            public cublasStatus_t Destroy(cublasHandle_t handle) { return cublasDestroy(handle); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetVersion")]
            public static extern cublasStatus_t cublasGetVersion(cublasHandle_t handle, out int version);
            public cublasStatus_t GetVersion(cublasHandle_t handle, out int version) { return cublasGetVersion(handle, out version); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetProperty")]
            public static extern cublasStatus_t cublasGetProperty(libraryPropertyType_t type, out int property);
            public cublasStatus_t GetVersion(libraryPropertyType_t type, out int property) { return cublasGetProperty(type, out property); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetStream")]
            public static extern cublasStatus_t cublasSetStream(cublasHandle_t handle, cudaStream_t streamId);
            public cublasStatus_t SetStream(cublasHandle_t handle, cudaStream_t streamId) { return cublasSetStream(handle, streamId); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetStream")]
            public static extern cublasStatus_t cublasGetStream(cublasHandle_t handle, out cudaStream_t streamId);
            public cublasStatus_t GetStream(cublasHandle_t handle, out cudaStream_t streamId) { return cublasGetStream(handle, out streamId); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetPointerMode")]
            public static extern cublasStatus_t cublasGetPointerMode(cublasHandle_t handle, out cublasPointerMode_t mode);
            public cublasStatus_t GetPointerMode(cublasHandle_t handle, out cublasPointerMode_t mode) { return cublasGetPointerMode(handle, out mode); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetPointerMode")]
            public static extern cublasStatus_t cublasSetPointerMode(cublasHandle_t handle, cublasPointerMode_t mode);
            public cublasStatus_t SetPointerMode(cublasHandle_t handle, cublasPointerMode_t mode) { return cublasSetPointerMode(handle, mode); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetVector")]
            public static extern cublasStatus_t cublasSetVector(int n, int elemSize, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t SetVector(int n, int elemSize, IntPtr x, int incx, IntPtr y, int incy) { return cublasSetVector(n, elemSize, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetVector")]
            public static extern cublasStatus_t cublasGetVector(int n, int elemSize, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t GetVector(int n, int elemSize, IntPtr x, int incx, IntPtr y, int incy) { return cublasGetVector(n, elemSize, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetMatrix")]
            public static extern cublasStatus_t cublasSetMatrix(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb);
            public cublasStatus_t SetMatrix(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb) { return cublasSetMatrix(rows, cols, elemSize, A, lda, B, ldb); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetMatrix")]
            public static extern cublasStatus_t cublasGetMatrix(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb);
            public cublasStatus_t GetMatrix(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb) { return cublasGetMatrix(rows, cols, elemSize, A, lda, B, ldb); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetVectorAsync")]
            public static extern cublasStatus_t cublasSetVectorAsync(int n, int elemSize, IntPtr hostPtr, int incx, IntPtr devicePtr, int incy, cudaStream_t stream);
            public cublasStatus_t SetVectorAsync(int n, int elemSize, IntPtr hostPtr, int incx, IntPtr devicePtr, int incy, cudaStream_t stream) { return cublasSetVectorAsync(n, elemSize, hostPtr, incx, devicePtr, incy, stream); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetVectorAsync")]
            public static extern cublasStatus_t cublasGetVectorAsync(int n, int elemSize, IntPtr devicePtr, int incx, IntPtr hostPtr, int incy, cudaStream_t stream);
            public cublasStatus_t GetVectorAsync(int n, int elemSize, IntPtr devicePtr, int incx, IntPtr hostPtr, int incy, cudaStream_t stream) { return cublasGetVectorAsync(n, elemSize, devicePtr, incx, hostPtr, incy, stream); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetMatrixAsync")]
            public static extern cublasStatus_t cublasSetMatrixAsync(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb, cudaStream_t stream);
            public cublasStatus_t SetMatrixAsync(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb, cudaStream_t stream) { return cublasSetMatrixAsync(rows, cols, elemSize, A, lda, B, ldb, stream); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetMatrixAsync")]
            public static extern cublasStatus_t cublasGetMatrixAsync(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb, cudaStream_t stream);
            public cublasStatus_t GetMatrixAsync(int rows, int cols, int elemSize, IntPtr A, int lda, IntPtr B, int ldb, cudaStream_t stream) { return cublasGetMatrixAsync(rows, cols, elemSize, A, lda, B, ldb, stream); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetAtomicsMode")]
            public static extern cublasStatus_t cublasSetAtomicsMode(cublasHandle_t handle, cublasAtomicsMode_t mode);
            public cublasStatus_t SetAtomicsMode(cublasHandle_t handle, cublasAtomicsMode_t mode) { return cublasSetAtomicsMode(handle, mode); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetAtomicsMode")]
            public static extern cublasStatus_t cublasGetAtomicsMode(cublasHandle_t handle, out cublasAtomicsMode_t mode);
            public cublasStatus_t GetAtomicsMode(cublasHandle_t handle, out cublasAtomicsMode_t mode) { return cublasGetAtomicsMode(handle, out mode); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSetMathMode")]
            public static extern cublasStatus_t cublasSetMathMode(cublasHandle_t handle, cublasMath_t mode);
            public cublasStatus_t SetMathMode(cublasHandle_t handle, cublasMath_t mode) { return cublasSetMathMode(handle, mode); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasGetMathMode")]
            public static extern cublasStatus_t cublasGetMathMode(cublasHandle_t handle, out cublasMath_t mode);
            public cublasStatus_t GetMathMode(cublasHandle_t handle, out cublasMath_t mode) { return cublasGetMathMode(handle, out mode); }

            #endregion

            #region Level-1 Functions

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIsamax")]
            public static extern cublasStatus_t cublasIsamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Isamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIsamax(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIdamax")]
            public static extern cublasStatus_t cublasIdamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Idamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIdamax(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIcamax")]
            public static extern cublasStatus_t cublasIcamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Icamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIcamax(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIzamax")]
            public static extern cublasStatus_t cublasIzamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Izamax(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIzamax(handle, n, x, incx, result); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIsamin")]
            public static extern cublasStatus_t cublasIsamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Isamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIsamin(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIdamin")]
            public static extern cublasStatus_t cublasIdamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Idamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIdamin(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIcamin")]
            public static extern cublasStatus_t cublasIcamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Icamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIcamin(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasIzamin")]
            public static extern cublasStatus_t cublasIzamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Izamin(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasIzamin(handle, n, x, incx, result); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSasum")]
            public static extern cublasStatus_t cublasSasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Sasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasSasum(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDasum")]
            public static extern cublasStatus_t cublasDasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Dasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasSasum(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasScasum")]
            public static extern cublasStatus_t cublasScasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Scasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasScasum(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDzasum")]
            public static extern cublasStatus_t cublasDzasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Dzasum(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasDzasum(handle, n, x, incx, result); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSaxpy")]
            public static extern cublasStatus_t cublasSaxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Saxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return cublasSaxpy(handle, n, alpha, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDaxpy")]
            public static extern cublasStatus_t cublasDaxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Daxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return cublasDaxpy(handle, n, alpha, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCaxpy")]
            public static extern cublasStatus_t cublasCaxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Caxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return cublasCaxpy(handle, n, alpha, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZaxpy")]
            public static extern cublasStatus_t cublasZaxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Zaxpy(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy) { return cublasZaxpy(handle, n, alpha, x, incx, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasScopy")]
            public static extern cublasStatus_t cublasScopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Scopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasScopy(handle, n, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDcopy")]
            public static extern cublasStatus_t cublasDcopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Dcopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasDcopy(handle, n, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCcopy")]
            public static extern cublasStatus_t cublasCcopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Ccopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasCcopy(handle, n, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZcopy")]
            public static extern cublasStatus_t cublasZcopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Zcopy(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasZcopy(handle, n, x, incx, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSdot")]
            public static extern cublasStatus_t cublasSdot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result);
            public cublasStatus_t Sdot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return cublasSdot(handle, n, x, incx, y, incy, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDdot")]
            public static extern cublasStatus_t cublasDdot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result);
            public cublasStatus_t Ddot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return cublasDdot(handle, n, x, incx, y, incy, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCdotu")]
            public static extern cublasStatus_t cublasCdotu(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result);
            public cublasStatus_t Cdotu(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return cublasCdotu(handle, n, x, incx, y, incy, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCdotc")]
            public static extern cublasStatus_t cublasCdotc(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result);
            public cublasStatus_t Cdotc(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return cublasCdotc(handle, n, x, incx, y, incy, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZdotu")]
            public static extern cublasStatus_t cublasZdotu(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result);
            public cublasStatus_t Zdotu(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return cublasZdotu(handle, n, x, incx, y, incy, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZdotc")]
            public static extern cublasStatus_t cublasZdotc(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result);
            public cublasStatus_t Zdotc(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr result) { return cublasZdotc(handle, n, x, incx, y, incy, result); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSnrm2")]
            public static extern cublasStatus_t cublasSnrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Snrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasSnrm2(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDnrm2")]
            public static extern cublasStatus_t cublasDnrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Dnrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasDnrm2(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasScnrm2")]
            public static extern cublasStatus_t cublasScnrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Scnrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasScnrm2(handle, n, x, incx, result); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDznrm2")]
            public static extern cublasStatus_t cublasDznrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result);
            public cublasStatus_t Dznrm2(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr result) { return cublasDznrm2(handle, n, x, incx, result); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSrot")]
            public static extern cublasStatus_t cublasSrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s);
            public cublasStatus_t Srot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return cublasSrot(handle, n, x, incx, y, incy, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDrot")]
            public static extern cublasStatus_t cublasDrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s);
            public cublasStatus_t Drot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return cublasDrot(handle, n, x, incx, y, incy, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCrot")]
            public static extern cublasStatus_t cublasCrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s);
            public cublasStatus_t Crot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return cublasCrot(handle, n, x, incx, y, incy, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsrot")]
            public static extern cublasStatus_t cublasCsrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s);
            public cublasStatus_t Csrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return cublasCsrot(handle, n, x, incx, y, incy, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZrot")]
            public static extern cublasStatus_t cublasZrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s);
            public cublasStatus_t Zrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return cublasZrot(handle, n, x, incx, y, incy, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZdrot")]
            public static extern cublasStatus_t cublasZdrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s);
            public cublasStatus_t Zdrot(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr c, IntPtr s) { return cublasZdrot(handle, n, x, incx, y, incy, c, s); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSrotg")]
            public static extern cublasStatus_t cublasSrotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s);
            public cublasStatus_t Srotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return cublasSrotg(handle, a, b, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDrotg")]
            public static extern cublasStatus_t cublasDrotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s);
            public cublasStatus_t Drotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return cublasDrotg(handle, a, b, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCrotg")]
            public static extern cublasStatus_t cublasCrotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s);
            public cublasStatus_t Crotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return cublasCrotg(handle, a, b, c, s); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZrotg")]
            public static extern cublasStatus_t cublasZrotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s);
            public cublasStatus_t Zrotg(cublasHandle_t handle, IntPtr a, IntPtr b, IntPtr c, IntPtr s) { return cublasZrotg(handle, a, b, c, s); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSrotm")]
            public static extern cublasStatus_t cublasSrotm(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr param);
            public cublasStatus_t Srotm(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr param) { return cublasSrotm(handle, n, x, incx, y, incy, param); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDrotm")]
            public static extern cublasStatus_t cublasDrotm(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr param);
            public cublasStatus_t Drotm(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy, IntPtr param) { return cublasDrotm(handle, n, x, incx, y, incy, param); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSrotmg")]
            public static extern cublasStatus_t cublasSrotmg(cublasHandle_t handle, IntPtr d1, IntPtr d2, IntPtr x1, IntPtr y1, IntPtr param);
            public cublasStatus_t Srotmg(cublasHandle_t handle, IntPtr d1, IntPtr d2, IntPtr x1, IntPtr y1, IntPtr param) { return cublasSrotmg(handle, d1, d2, x1, y1, param); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDrotmg")]
            public static extern cublasStatus_t cublasDrotmg(cublasHandle_t handle, IntPtr d1, IntPtr d2, IntPtr x1, IntPtr y1, IntPtr param);
            public cublasStatus_t Drotmg(cublasHandle_t handle, IntPtr d1, IntPtr d2, IntPtr x1, IntPtr y1, IntPtr param) { return cublasDrotmg(handle, d1, d2, x1, y1, param); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSscal")]
            public static extern cublasStatus_t cublasSscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx);
            public cublasStatus_t Sscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return cublasSscal(handle, n, alpha, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDscal")]
            public static extern cublasStatus_t cublasDscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx);
            public cublasStatus_t Dscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return cublasDscal(handle, n, alpha, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCscal")]
            public static extern cublasStatus_t cublasCscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx);
            public cublasStatus_t Cscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return cublasCscal(handle, n, alpha, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsscal")]
            public static extern cublasStatus_t cublasCsscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx);
            public cublasStatus_t Csscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return cublasCsscal(handle, n, alpha, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZscal")]
            public static extern cublasStatus_t cublasZscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx);
            public cublasStatus_t Zscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return cublasZscal(handle, n, alpha, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZdscal")]
            public static extern cublasStatus_t cublasZdscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx);
            public cublasStatus_t Zdscal(cublasHandle_t handle, int n, IntPtr alpha, IntPtr x, int incx) { return cublasZdscal(handle, n, alpha, x, incx); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSswap")]
            public static extern cublasStatus_t cublasSswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Sswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasSswap(handle, n, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDswap")]
            public static extern cublasStatus_t cublasDswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Dswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasDswap(handle, n, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCswap")]
            public static extern cublasStatus_t cublasCswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Cswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasCswap(handle, n, x, incx, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZswap")]
            public static extern cublasStatus_t cublasZswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy);
            public cublasStatus_t Zswap(cublasHandle_t handle, int n, IntPtr x, int incx, IntPtr y, int incy) { return cublasZswap(handle, n, x, incx, y, incy); }

            #endregion

            #region Level-2 Functions

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSgbmv")]
            public static extern cublasStatus_t cublasSgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Sgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasSgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDgbmv")]
            public static extern cublasStatus_t cublasDgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Dgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasDgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCgbmv")]
            public static extern cublasStatus_t cublasCgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Cgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasCgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZgbmv")]
            public static extern cublasStatus_t cublasZgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Zgbmv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, int kl, int ku, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasZgbmv(handle, trans, m, n, kl, ku, alpha, A, lda, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSgemv")]
            public static extern cublasStatus_t cublasSgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Sgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasSgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDgemv_v2")]
            public static extern cublasStatus_t cublasDgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Dgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasDgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCgemv")]
            public static extern cublasStatus_t cublasCgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Cgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasCgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZgemv")]
            public static extern cublasStatus_t cublasZgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Zgemv(cublasHandle_t handle, cublasOperation_t trans, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasZgemv(handle, trans, m, n, alpha, A, lda, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSger")]
            public static extern cublasStatus_t cublasSger(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Sger(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasSger(handle, m, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDger")]
            public static extern cublasStatus_t cublasDger(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Dger(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasDger(handle, m, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCgeru")]
            public static extern cublasStatus_t cublasCgeru(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Cgeru(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasCgeru(handle, m, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCgerc")]
            public static extern cublasStatus_t cublasCgerc(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Cgerc(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasCgerc(handle, m, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZgeru")]
            public static extern cublasStatus_t cublasZgeru(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Zgeru(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasZgeru(handle, m, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZgerc")]
            public static extern cublasStatus_t cublasZgerc(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Zgerc(cublasHandle_t handle, int m, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasZgerc(handle, m, n, alpha, x, incx, y, incy, A, lda); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSsbmv")]
            public static extern cublasStatus_t cublasSsbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Ssbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasSsbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDsbmv")]
            public static extern cublasStatus_t cublasDsbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Dsbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasDsbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSspmv")]
            public static extern cublasStatus_t cublasSspmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Sspmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasSspmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDspmv")]
            public static extern cublasStatus_t cublasDspmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Dspmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasDspmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSspr")]
            public static extern cublasStatus_t cublasSspr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP);
            public cublasStatus_t Sspr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return cublasSspr(handle, uplo, n, alpha, x, incx, AP); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDspr")]
            public static extern cublasStatus_t cublasDspr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP);
            public cublasStatus_t Dspr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return cublasDspr(handle, uplo, n, alpha, x, incx, AP); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSspr2")]
            public static extern cublasStatus_t cublasSspr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP);
            public cublasStatus_t Sspr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return cublasSspr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDspr2")]
            public static extern cublasStatus_t cublasDspr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP);
            public cublasStatus_t Dspr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return cublasDspr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSsymv")]
            public static extern cublasStatus_t cublasSsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Ssymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasSsymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDsymv")]
            public static extern cublasStatus_t cublasDsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Dsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasDsymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsymv")]
            public static extern cublasStatus_t cublasCsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Csymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasCsymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZsymv")]
            public static extern cublasStatus_t cublasZsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Zsymv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasZsymv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSsyr")]
            public static extern cublasStatus_t cublasSsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda);
            public cublasStatus_t Ssyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return cublasSsyr(handle, uplo, n, alpha, x, incx, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDsyr")]
            public static extern cublasStatus_t cublasDsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda);
            public cublasStatus_t Dsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return cublasDsyr(handle, uplo, n, alpha, x, incx, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsyr")]
            public static extern cublasStatus_t cublasCsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda);
            public cublasStatus_t Csyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return cublasCsyr(handle, uplo, n, alpha, x, incx, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZsyr")]
            public static extern cublasStatus_t cublasZsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda);
            public cublasStatus_t Zsyr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return cublasZsyr(handle, uplo, n, alpha, x, incx, A, lda); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSsyr2")]
            public static extern cublasStatus_t cublasSsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Ssyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasSsyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDsyr2")]
            public static extern cublasStatus_t cublasDsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Dsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasDsyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsyr2")]
            public static extern cublasStatus_t cublasCsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Csyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasCsyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZsyr2")]
            public static extern cublasStatus_t cublasZsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Zsyr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasZsyr2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStbmv")]
            public static extern cublasStatus_t cublasStbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Stbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasStbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtbmv")]
            public static extern cublasStatus_t cublasDtbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Dtbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasDtbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtbmv")]
            public static extern cublasStatus_t cublasCtbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ctbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasCtbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtbmv")]
            public static extern cublasStatus_t cublasZtbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ztbmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasZtbmv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStbsv")]
            public static extern cublasStatus_t cublasStbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Stbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasStbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtbsv")]
            public static extern cublasStatus_t cublasDtbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Dtbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasDtbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtbsv")]
            public static extern cublasStatus_t cublasCtbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ctbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasCtbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtbsv")]
            public static extern cublasStatus_t cublasZtbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ztbsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, int k, IntPtr A, int lda, IntPtr x, int incx) { return cublasZtbsv(handle, uplo, trans, diag, n, k, A, lda, x, incx); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStpmv")]
            public static extern cublasStatus_t cublasStpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Stpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasStpmv(handle, uplo, trans, diag, n, AP, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtpmv")]
            public static extern cublasStatus_t cublasDtpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Dtpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasDtpmv(handle, uplo, trans, diag, n, AP, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtpmv")]
            public static extern cublasStatus_t cublasCtpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Ctpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasCtpmv(handle, uplo, trans, diag, n, AP, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtpmv")]
            public static extern cublasStatus_t cublasZtpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Ztpmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasZtpmv(handle, uplo, trans, diag, n, AP, x, incx); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStpsv")]
            public static extern cublasStatus_t cublasStpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Stpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasStpsv(handle, uplo, trans, diag, n, AP, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtpsv")]
            public static extern cublasStatus_t cublasDtpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Dtpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasDtpsv(handle, uplo, trans, diag, n, AP, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtpsv")]
            public static extern cublasStatus_t cublasCtpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Ctpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasCtpsv(handle, uplo, trans, diag, n, AP, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtpsv")]
            public static extern cublasStatus_t cublasZtpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx);
            public cublasStatus_t Ztpsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr AP, IntPtr x, int incx) { return cublasZtpsv(handle, uplo, trans, diag, n, AP, x, incx); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStrmv")]
            public static extern cublasStatus_t cublasStrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Strmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasStrmv(handle, uplo, trans, diag, n, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtrmv")]
            public static extern cublasStatus_t cublasDtrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Dtrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasDtrmv(handle, uplo, trans, diag, n, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtrmv")]
            public static extern cublasStatus_t cublasCtrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ctrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasCtrmv(handle, uplo, trans, diag, n, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtrmv")]
            public static extern cublasStatus_t cublasZtrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ztrmv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasZtrmv(handle, uplo, trans, diag, n, A, lda, x, incx); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStrsv")]
            public static extern cublasStatus_t cublasStrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Strsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasStrsv(handle, uplo, trans, diag, n, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtrsv")]
            public static extern cublasStatus_t cublasDtrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Dtrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasDtrsv(handle, uplo, trans, diag, n, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtrsv")]
            public static extern cublasStatus_t cublasCtrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ctrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasCtrsv(handle, uplo, trans, diag, n, A, lda, x, incx); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtrsv")]
            public static extern cublasStatus_t cublasZtrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx);
            public cublasStatus_t Ztrsv(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int n, IntPtr A, int lda, IntPtr x, int incx) { return cublasZtrsv(handle, uplo, trans, diag, n, A, lda, x, incx); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasChemv")]
            public static extern cublasStatus_t cublasChemv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Chemv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasChemv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZhemv")]
            public static extern cublasStatus_t cublasZhemv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Zhemv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasZhemv(handle, uplo, n, alpha, A, lda, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasChbmv")]
            public static extern cublasStatus_t cublasChbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Chbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasChbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZhbmv")]
            public static extern cublasStatus_t cublasZhbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Zhbmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasZhbmv(handle, uplo, n, k, alpha, A, lda, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasChpmv")]
            public static extern cublasStatus_t cublasChpmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Chpmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasChpmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZhpmv")]
            public static extern cublasStatus_t cublasZhpmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy);
            public cublasStatus_t Zhpmv(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr AP, IntPtr x, int incx, IntPtr beta, IntPtr y, int incy) { return cublasZhpmv(handle, uplo, n, alpha, AP, x, incx, beta, y, incy); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCher")]
            public static extern cublasStatus_t cublasCher(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda);
            public cublasStatus_t Cher(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return cublasCher(handle, uplo, n, alpha, x, incx, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZher")]
            public static extern cublasStatus_t cublasZher(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda);
            public cublasStatus_t Zher(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr A, int lda) { return cublasZher(handle, uplo, n, alpha, x, incx, A, lda); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCher2")]
            public static extern cublasStatus_t cublasCher2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Cher2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasCher2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZher2")]
            public static extern cublasStatus_t cublasZher2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda);
            public cublasStatus_t Zher2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr A, int lda) { return cublasZher2(handle, uplo, n, alpha, x, incx, y, incy, A, lda); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasChpr")]
            public static extern cublasStatus_t cublasChpr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP);
            public cublasStatus_t Chpr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return cublasChpr(handle, uplo, n, alpha, x, incx, AP); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZhpr")]
            public static extern cublasStatus_t cublasZhpr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP);
            public cublasStatus_t Zhpr(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr AP) { return cublasZhpr(handle, uplo, n, alpha, x, incx, AP); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasChpr2")]
            public static extern cublasStatus_t cublasChpr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP);
            public cublasStatus_t Chpr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return cublasChpr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZhpr2")]
            public static extern cublasStatus_t cublasZhpr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP);
            public cublasStatus_t Zhpr2(cublasHandle_t handle, cublasFillMode_t uplo, int n, IntPtr alpha, IntPtr x, int incx, IntPtr y, int incy, IntPtr AP) { return cublasZhpr2(handle, uplo, n, alpha, x, incx, y, incy, AP); }

            #endregion

            #region Level-3 Functions

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSgemm_v2")]
            public static extern cublasStatus_t cublasSgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Sgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasSgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDgemm_v2")]
            public static extern cublasStatus_t cublasDgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Dgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasDgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCgemm_v2")]
            public static extern cublasStatus_t cublasCgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Cgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasCgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZgemm_v2")]
            public static extern cublasStatus_t cublasZgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Zgemm(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasZgemm(handle, transa, transb, m, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            /*
            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSgemmBatched")]
            public static extern cublasStatus_t cublasSgemmBatched(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr Aarray, int lda, IntPtr Barray, int ldb, IntPtr beta, IntPtr Carray, int ldc, int batchCount);

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDgemmBatched")]
            public static extern cublasStatus_t cublasDgemmBatched(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr Aarray, int lda, IntPtr Barray, int ldb, IntPtr beta, IntPtr Carray, int ldc, int batchCount);

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCgemmBatched")]
            public static extern cublasStatus_t cublasCgemmBatched(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr Aarray, int lda, IntPtr Barray, int ldb, IntPtr beta, IntPtr Carray, int ldc, int batchCount);

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZgemmBatched")]
            public static extern cublasStatus_t cublasZgemmBatched(cublasHandle_t handle, cublasOperation_t transa, cublasOperation_t transb, int m, int n, int k, IntPtr alpha, IntPtr Aarray, int lda, IntPtr Barray, int ldb, IntPtr beta, IntPtr Carray, int ldc, int batchCount);
            // */

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSsymm")]
            public static extern cublasStatus_t cublasSsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Ssymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasSsymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDsymm")]
            public static extern cublasStatus_t cublasDsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Dsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasDsymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsymm")]
            public static extern cublasStatus_t cublasCsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Csymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasCsymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZsymm")]
            public static extern cublasStatus_t cublasZsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Zsymm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasZsymm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSsyrk")]
            public static extern cublasStatus_t cublasSsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Ssyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return cublasSsyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDsyrk")]
            public static extern cublasStatus_t cublasDsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Dsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return cublasDsyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsyrk")]
            public static extern cublasStatus_t cublasCsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Csyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return cublasCsyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZsyrk")]
            public static extern cublasStatus_t cublasZsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Zsyrk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return cublasZsyrk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasSsyr2k")]
            public static extern cublasStatus_t cublasSsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Ssyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasSsyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDsyr2k")]
            public static extern cublasStatus_t cublasDsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Dsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasDsyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCsyr2k")]
            public static extern cublasStatus_t cublasCsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Csyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasCsyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZsyr2k")]
            public static extern cublasStatus_t cublasZsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Zsyr2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasZsyr2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStrmm")]
            public static extern cublasStatus_t cublasStrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc);
            public cublasStatus_t Strmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return cublasStrmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtrmm")]
            public static extern cublasStatus_t cublasDtrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc);
            public cublasStatus_t Dtrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return cublasDtrmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtrmm")]
            public static extern cublasStatus_t cublasCtrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc);
            public cublasStatus_t Ctrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return cublasCtrmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtrmm")]
            public static extern cublasStatus_t cublasZtrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc);
            public cublasStatus_t Ztrmm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr C, int ldc) { return cublasZtrmm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb, C, ldc); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStrsm")]
            public static extern cublasStatus_t cublasStrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb);
            public cublasStatus_t Strsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return cublasStrsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtrsm")]
            public static extern cublasStatus_t cublasDtrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb);
            public cublasStatus_t Dtrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return cublasDtrsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtrsm")]
            public static extern cublasStatus_t cublasCtrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb);
            public cublasStatus_t Ctrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return cublasCtrsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtrsm")]
            public static extern cublasStatus_t cublasZtrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb);
            public cublasStatus_t Ztrsm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb) { return cublasZtrsm(handle, side, uplo, trans, diag, m, n, alpha, A, lda, B, ldb); }

            /*
            [DllImport(CUBLAS_DLL, EntryPoint = "cublasStrsmBatched")]
            public static extern cublasStatus_t cublasStrsmBatched(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, int batchCount);

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasDtrsmBatched")]
            public static extern cublasStatus_t cublasDtrsmBatched(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, int batchCount);

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCtrsmBatched")]
            public static extern cublasStatus_t cublasCtrsmBatched(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, int batchCount);

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZtrsmBatched")]
            public static extern cublasStatus_t cublasZtrsmBatched(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, cublasOperation_t trans, cublasDiagType_t diag, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, int batchCount);
            // */


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasChemm")]
            public static extern cublasStatus_t cublasChemm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Chemm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasChemm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZhemm")]
            public static extern cublasStatus_t cublasZhemm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Zhemm(cublasHandle_t handle, cublasSideMode_t side, cublasFillMode_t uplo, int m, int n, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasZhemm(handle, side, uplo, m, n, alpha, A, lda, B, ldb, beta, C, ldc); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCherk")]
            public static extern cublasStatus_t cublasCherk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Cherk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return cublasCherk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZherk")]
            public static extern cublasStatus_t cublasZherk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Zherk(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr beta, IntPtr C, int ldc) { return cublasZherk(handle, uplo, trans, n, k, alpha, A, lda, beta, C, ldc); }


            [DllImport(CUBLAS_DLL, EntryPoint = "cublasCher2k")]
            public static extern cublasStatus_t cublasCher2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Cher2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasCher2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            [DllImport(CUBLAS_DLL, EntryPoint = "cublasZher2k")]
            public static extern cublasStatus_t cublasZher2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc);
            public cublasStatus_t Zher2k(cublasHandle_t handle, cublasFillMode_t uplo, cublasOperation_t trans, int n, int k, IntPtr alpha, IntPtr A, int lda, IntPtr B, int ldb, IntPtr beta, IntPtr C, int ldc) { return cublasZher2k(handle, uplo, trans, n, k, alpha, A, lda, B, ldb, beta, C, ldc); }

            #endregion


            public cublasStatus_t GetVersion(out int version)
            {
                throw new NotImplementedException();
            }

            public cublasStatus_t GetProperty(libraryPropertyType_t type, out int property)
            {
                throw new NotImplementedException();
            }

            public cublasStatus_t GetMathMode(cublasHandle_t handle, cublasMath_t mode)
            {
                throw new NotImplementedException();
            }

            public cublasStatus_t SetMathMode(cublasHandle_t handle, out cublasMath_t mode)
            {
                throw new NotImplementedException();
            }
        }

    }
}