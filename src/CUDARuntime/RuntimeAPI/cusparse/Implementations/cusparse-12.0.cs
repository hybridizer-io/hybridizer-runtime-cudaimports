/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Runtime.InteropServices;
#pragma warning disable 0169
namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// Complete documentation <see href="https://docs.nvidia.com/cuda/cusparse/index.html">here</see>
    /// </summary>
#pragma warning disable 1591
    public unsafe class CUSPARSE_64_120
    {
        public const string CUSPARSE_DLL = "cusparse64_12.dll";

        #region /* --- Helper functions --- */

        /* CUSPARSE initialization and managment routines */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreate(out cusparseHandle_t handle);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroy(cusparseHandle_t handle);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseGetVersion(cusparseHandle_t handle, out int version);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSetStream(cusparseHandle_t handle, cudaStream_t streamId);

        /* CUSPARSE type creation, destruction, set and get routines */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseGetPointerMode(cusparseHandle_t handle,
            out cusparsePointerMode_t mode);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSetPointerMode(cusparseHandle_t handle,
            cusparsePointerMode_t mode);

        /* sparse matrix descriptor */
        /* When the matrix descriptor is created, its fields are initialized to: 
            CUSPARSE_MATRIX_TYPE_GENERAL
            CUSPARSE_INDEX_BASE_ZERO
            All other fields are uninitialized
        */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateMatDescr(out cusparseMatDescr_t descrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyMatDescr(cusparseMatDescr_t descrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSetMatType(cusparseMatDescr_t descrA,
            cusparseMatrixType_t type);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseMatrixType_t cusparseGetMatType(cusparseMatDescr_t descrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSetMatFillMode(cusparseMatDescr_t descrA,
            cusparseFillMode_t fillMode);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseFillMode_t cusparseGetMatFillMode(cusparseMatDescr_t descrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSetMatDiagType(cusparseMatDescr_t descrA,
            cusparseDiagType_t diagType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseDiagType_t cusparseGetMatDiagType(cusparseMatDescr_t descrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSetMatIndexBase(cusparseMatDescr_t descrA,
            cusparseIndexBase_t indexBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseIndexBase_t cusparseGetMatIndexBase(cusparseMatDescr_t descrA);

        /* sparse triangular solve and incomplete-LU and Cholesky (algorithm 1) */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateSolveAnalysisInfo(out cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroySolveAnalysisInfo(cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseGetLevelInfo(cusparseHandle_t handle,
            cusparseSolveAnalysisInfo_t info,
            int* nlevels,
            int** levelPtr,
            int** levelInd);

        /* sparse triangular solve (algorithm 2) */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateCsrsv2Info(out csrsv2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyCsrsv2Info(csrsv2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateCsrsm2Info(out csrsm2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyCsrsm2Info(csrsm2Info_t info);

        /* incomplete Cholesky (algorithm 2)*/
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateCsric02Info(out csric02Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyCsric02Info(csric02Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateBsric02Info(out bsric02Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyBsric02Info(bsric02Info_t info);

        /* incomplete LU (algorithm 2) */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateCsrilu02Info(out csrilu02Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyCsrilu02Info(csrilu02Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateBsrilu02Info(out bsrilu02Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyBsrilu02Info(bsrilu02Info_t info);

        /* block-CSR triangular solve (algorithm 2) */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateBsrsv2Info(out bsrsv2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyBsrsv2Info(bsrsv2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateBsrsm2Info(out bsrsm2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyBsrsm2Info(bsrsm2Info_t info);

        /* hybrid (HYB) format */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateHybMat(out cusparseHybMat_t hybA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyHybMat(cusparseHybMat_t hybA);

        /* sorting information */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateCsru2csrInfo(out csru2csrInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyCsru2csrInfo(csru2csrInfo_t info);

        /* coloring info */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateColorInfo(out cusparseColorInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyColorInfo(cusparseColorInfo_t info);

        #endregion

        #region /* --- Sparse Level 1 routines --- */

        /* Description: Addition of a scalar multiple of a sparse vector x  
            and a dense vector y. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSaxpyi(cusparseHandle_t handle,
            int nnz,
            float* alpha,
            float* xVal,
            int* xInd,
            float* y,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDaxpyi(cusparseHandle_t handle,
            int nnz,
            double* alpha,
            double* xVal,
            int* xInd,
            double* y,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCaxpyi(cusparseHandle_t handle,
            int nnz,
            cuComplex* alpha,
            cuComplex* xVal,
            int* xInd,
            cuComplex* y,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZaxpyi(cusparseHandle_t handle,
            int nnz,
            cuDoubleComplex* alpha,
            cuDoubleComplex* xVal,
            int* xInd,
            cuDoubleComplex* y,
            cusparseIndexBase_t idxBase);

        /* Description: dot product of a sparse vector x and a dense vector y. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSdoti(cusparseHandle_t handle,
            int nnz,
            float* xVal,
            int* xInd,
            float* y,
            float* resultDevHostPtr,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDdoti(cusparseHandle_t handle,
            int nnz,
            double* xVal,
            int* xInd,
            double* y,
            double* resultDevHostPtr,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCdoti(cusparseHandle_t handle,
            int nnz,
            cuComplex* xVal,
            int* xInd,
            cuComplex* y,
            cuComplex* resultDevHostPtr,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZdoti(cusparseHandle_t handle,
            int nnz,
            cuDoubleComplex* xVal,
            int* xInd,
            cuDoubleComplex* y,
            cuDoubleComplex* resultDevHostPtr,
            cusparseIndexBase_t idxBase);

        /* Description: dot product of complex conjugate of a sparse vector x
        and a dense vector y. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCdotci(cusparseHandle_t handle,
            int nnz,
            cuComplex* xVal,
            int* xInd,
            cuComplex* y,
            cuComplex* resultDevHostPtr,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZdotci(cusparseHandle_t handle,
            int nnz,
            cuDoubleComplex* xVal,
            int* xInd,
            cuDoubleComplex* y,
            cuDoubleComplex* resultDevHostPtr,
            cusparseIndexBase_t idxBase);


        /* Description: Gather of non-zero elements from dense vector y into 
        sparse vector x. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgthr(cusparseHandle_t handle,
            int nnz,
            float* y,
            float* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgthr(cusparseHandle_t handle,
            int nnz,
            double* y,
            double* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgthr(cusparseHandle_t handle,
            int nnz,
            cuComplex* y,
            cuComplex* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgthr(cusparseHandle_t handle,
            int nnz,
            cuDoubleComplex* y,
            cuDoubleComplex* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        /* Description: Gather of non-zero elements from desne vector y into 
        sparse vector x (also replacing these elements in y by zeros). */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgthrz(cusparseHandle_t handle,
            int nnz,
            float* y,
            float* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgthrz(cusparseHandle_t handle,
            int nnz,
            double* y,
            double* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgthrz(cusparseHandle_t handle,
            int nnz,
            cuComplex* y,
            cuComplex* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgthrz(cusparseHandle_t handle,
            int nnz,
            cuDoubleComplex* y,
            cuDoubleComplex* xVal,
            int* xInd,
            cusparseIndexBase_t idxBase);

        /* Description: Scatter of elements of the sparse vector x into 
        dense vector y. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSsctr(cusparseHandle_t handle,
            int nnz,
            float* xVal,
            int* xInd,
            float* y,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDsctr(cusparseHandle_t handle,
            int nnz,
            double* xVal,
            int* xInd,
            double* y,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCsctr(cusparseHandle_t handle,
            int nnz,
            cuComplex* xVal,
            int* xInd,
            cuComplex* y,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZsctr(cusparseHandle_t handle,
            int nnz,
            cuDoubleComplex* xVal,
            int* xInd,
            cuDoubleComplex* y,
            cusparseIndexBase_t idxBase);

        /* Description: Givens rotation, where c and s are cosine and sine, 
        x and y are sparse and dense vectors, respectively. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSroti(cusparseHandle_t handle,
            int nnz,
            float* xVal,
            int* xInd,
            float* y,
            float* c,
            float* s,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDroti(cusparseHandle_t handle,
            int nnz,
            double* xVal,
            int* xInd,
            double* y,
            double* c,
            double* s,
            cusparseIndexBase_t idxBase);

        #endregion

        #region /* --- Sparse Level 2 routines --- */

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgemvi(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            float* alpha, /* host or device pointer */
            float* A,
            int lda,
            int nnz,
            float* xVal,
            int* xInd,
            float* beta, /* host or device pointer */
            float* y,
            cusparseIndexBase_t idxBase,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgemvi_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            int* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgemvi(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            double* alpha, /* host or device pointer */
            double* A,
            int lda,
            int nnz,
            double* xVal,
            int* xInd,
            double* beta, /* host or device pointer */
            double* y,
            cusparseIndexBase_t idxBase,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgemvi_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            int* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgemvi(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            cuComplex* alpha, /* host or device pointer */
            cuComplex* A,
            int lda,
            int nnz,
            cuComplex* xVal,
            int* xInd,
            cuComplex* beta, /* host or device pointer */
            cuComplex* y,
            cusparseIndexBase_t idxBase,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgemvi_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            int* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgemvi(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            cuDoubleComplex* alpha, /* host or device pointer */
            cuDoubleComplex* A,
            int lda,
            int nnz,
            cuDoubleComplex* xVal,
            int* xInd,
            cuDoubleComplex* beta, /* host or device pointer */
            cuDoubleComplex* y,
            cusparseIndexBase_t idxBase,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgemvi_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            int* pBufferSize);


        /* Description: Matrix-vector multiplication  y = alpha * op(A) * x  + beta * y, 
        where A is a sparse matrix in CSR storage format, x and y are dense vectors. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* x,
            float* beta,
            float* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* x,
            double* beta,
            double* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* x,
            cuComplex* beta,
            cuComplex* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int nnz,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* x,
            cuDoubleComplex* beta,
            cuDoubleComplex* y);

        /* Description: Matrix-vector multiplication  y = alpha * op(A) * x  + beta * y, 
        where A is a sparse matrix in HYB storage format, x and y are dense vectors. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseShybmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            float* alpha,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            float* x,
            float* beta,
            float* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDhybmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            double* alpha,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            double* x,
            double* beta,
            double* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseChybmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuComplex* x,
            cuComplex* beta,
            cuComplex* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZhybmv(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuDoubleComplex* x,
            cuDoubleComplex* beta,
            cuDoubleComplex* y);

        /* Description: Matrix-vector multiplication  y = alpha * op(A) * x  + beta * y, 
        where A is a sparse matrix in BSR storage format, x and y are dense vectors. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nb,
            int nnzb,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            float* x,
            float* beta,
            float* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nb,
            int nnzb,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            double* x,
            double* beta,
            double* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nb,
            int nnzb,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cuComplex* x,
            cuComplex* beta,
            cuComplex* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nb,
            int nnzb,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cuDoubleComplex* x,
            cuDoubleComplex* beta,
            cuDoubleComplex* y);

        /* Description: Matrix-vector multiplication  y = alpha * op(A) * x  + beta * y, 
        where A is a sparse matrix in extended BSR storage format, x and y are dense 
        vectors. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrxmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int sizeOfMask,
            int mb,
            int nb,
            int nnzb,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedMaskPtrA,
            int* bsrSortedRowPtrA,
            int* bsrSortedEndPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            float* x,
            float* beta,
            float* y);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrxmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int sizeOfMask,
            int mb,
            int nb,
            int nnzb,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedMaskPtrA,
            int* bsrSortedRowPtrA,
            int* bsrSortedEndPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            double* x,
            double* beta,
            double* y);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrxmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int sizeOfMask,
            int mb,
            int nb,
            int nnzb,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedMaskPtrA,
            int* bsrSortedRowPtrA,
            int* bsrSortedEndPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cuComplex* x,
            cuComplex* beta,
            cuComplex* y);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrxmv(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int sizeOfMask,
            int mb,
            int nb,
            int nnzb,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedMaskPtrA,
            int* bsrSortedRowPtrA,
            int* bsrSortedEndPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cuDoubleComplex* x,
            cuDoubleComplex* beta,
            cuDoubleComplex* y);

        /* Description: Solution of triangular linear system op(A) * x = alpha * f, 
        where A is a sparse matrix in CSR storage format, rhs f and solution x 
        are dense vectors. This routine implements algorithm 1 for the solve. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsv_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            float* f,
            float* x);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsv_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            double* f,
            double* x);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsv_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            cuComplex* f,
            cuComplex* x);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsv_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            cuDoubleComplex* f,
            cuDoubleComplex* x);

        /* Description: Solution of triangular linear system op(A) * x = alpha * f, 
        where A is a sparse matrix in CSR storage format, rhs f and solution y 
        are dense vectors. This routine implements algorithm 1 for this problem. 
        Also, it provides a utility function to query size of buffer used. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrsv2_zeroPivot(cusparseHandle_t handle,
            csrsv2Info_t info,
            int* position);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            size_t* pBufferSize);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsv2_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsv2_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsv2_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsv2_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsv2_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            float* f,
            float* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsv2_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            double* f,
            double* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsv2_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            cuComplex* f,
            cuComplex* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsv2_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrsv2Info_t info,
            cuDoubleComplex* f,
            cuDoubleComplex* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        /* Description: Solution of triangular linear system op(A) * x = alpha * f, 
        where A is a sparse matrix in block-CSR storage format, rhs f and solution y 
        are dense vectors. This routine implements algorithm 2 for this problem. 
        Also, it provides a utility function to query size of buffer used. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXbsrsv2_zeroPivot(cusparseHandle_t handle,
            bsrsv2Info_t info,
            int* position);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsv2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            bsrsv2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            bsrsv2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            bsrsv2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsv2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            bsrsv2Info_t info,
            size_t* pBufferSize);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsv2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsv2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsv2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsv2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsv2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            float* f,
            float* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsv2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            double* f,
            double* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsv2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            cuComplex* f,
            cuComplex* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsv2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            int mb,
            int nnzb,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            bsrsv2Info_t info,
            cuDoubleComplex* f,
            cuDoubleComplex* x,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        /* Description: Solution of triangular linear system op(A) * x = alpha * f, 
        where A is a sparse matrix in HYB storage format, rhs f and solution x 
        are dense vectors. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseShybsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDhybsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseChybsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZhybsv_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseShybsv_solve(cusparseHandle_t handle,
            cusparseOperation_t trans,
            float* alpha,
            cusparseMatDescr_t descra,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info,
            float* f,
            float* x);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseChybsv_solve(cusparseHandle_t handle,
            cusparseOperation_t trans,
            cuComplex* alpha,
            cusparseMatDescr_t descra,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info,
            cuComplex* f,
            cuComplex* x);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDhybsv_solve(cusparseHandle_t handle,
            cusparseOperation_t trans,
            double* alpha,
            cusparseMatDescr_t descra,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info,
            double* f,
            double* x);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZhybsv_solve(cusparseHandle_t handle,
            cusparseOperation_t trans,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descra,
            cusparseHybMat_t hybA,
            cusparseSolveAnalysisInfo_t info,
            cuDoubleComplex* f,
            cuDoubleComplex* x);

        #endregion

        #region /* --- Sparse Level 3 routines --- */

        /* Description: sparse - dense matrix multiplication C = alpha * op(A) * B  + beta * C, 
where A is a sparse matrix in CSR format, B and C are dense tall matrices.  */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrmm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int k,
            int nnz,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* B,
            int ldb,
            float* beta,
            float* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrmm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int k,
            int nnz,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* B,
            int ldb,
            double* beta,
            double* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrmm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int k,
            int nnz,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* B,
            int ldb,
            cuComplex* beta,
            cuComplex* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrmm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            int k,
            int nnz,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* B,
            int ldb,
            cuDoubleComplex* beta,
            cuDoubleComplex* C,
            int ldc);

        /* Description: sparse - dense matrix multiplication C = alpha * op(A) * B  + beta * C, 
            where A is a sparse matrix in CSR format, B and C are dense tall matrices.
            This routine allows transposition of matrix B, which may improve performance. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrmm2(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            int nnz,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* B,
            int ldb,
            float* beta,
            float* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrmm2(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            int nnz,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* B,
            int ldb,
            double* beta,
            double* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrmm2(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            int nnz,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* B,
            int ldb,
            cuComplex* beta,
            cuComplex* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrmm2(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            int nnz,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* B,
            int ldb,
            cuDoubleComplex* beta,
            cuDoubleComplex* C,
            int ldc);

        /* Description: sparse - dense matrix multiplication C = alpha * op(A) * B  + beta * C, 
            where A is a sparse matrix in block-CSR format, B and C are dense tall matrices.
            This routine allows transposition of matrix B, which may improve performance. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrmm(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int kb,
            int nnzb,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            float* B,
            int ldb,
            float* beta,
            float* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrmm(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int kb,
            int nnzb,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            double* B,
            int ldb,
            double* beta,
            double* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrmm(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int kb,
            int nnzb,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            cuComplex* B,
            int ldb,
            cuComplex* beta,
            cuComplex* C,
            int ldc);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrmm(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int kb,
            int nnzb,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockSize,
            cuDoubleComplex* B,
            int ldb,
            cuDoubleComplex* beta,
            cuDoubleComplex* C,
            int ldc);

        /* Description: Solution of triangular linear system op(A) * X = alpha * F, 
            with multiple right-hand-sides, where A is a sparse matrix in CSR storage 
            format, rhs F and solution X are dense tall matrices. 
            This routine implements algorithm 1 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsm_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsm_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsm_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsm_analysis(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsm_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            float* F,
            int ldf,
            float* X,
            int ldx);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsm_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            double* F,
            int ldf,
            double* X,
            int ldx);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsm_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            cuComplex* F,
            int ldf,
            cuComplex* X,
            int ldx);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsm_solve(cusparseHandle_t handle,
            cusparseOperation_t transA,
            int m,
            int n,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info,
            cuDoubleComplex* F,
            int ldf,
            cuDoubleComplex* X,
            int ldx);

        /* Description: Solution of triangular linear system op(A) * op(X) = alpha * op(B), 
            where A is a sparse matrix in CSR storage 
            format, B and solution X are right-hand-side matrix
            This routine implements algorithm 2 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrsm2_zeroPivot(cusparseHandle_t handle,
            csrsm2Info_t info,
            int* position);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsm2_bufferSizeExt(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsm2_bufferSizeExt(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsm2_bufferSizeExt(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsm2_bufferSizeExt(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            size_t* pBufferSize);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsm2_analysis(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsm2_analysis(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsm2_analysis(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsm2_analysis(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrsm2_solve(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrsm2_solve(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrsm2_solve(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrsm2_solve(cusparseHandle_t handle,
            int algo,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int nrhs,
            int nnz,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* B,
            int ldb,
            csrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        /* Description: Solution of triangular linear system op(A) * X = alpha * F, 
            with multiple right-hand-sides, where A is a sparse matrix in CSR storage 
            format, rhs F and solution X are dense tall matrices.
            This routine implements algorithm 2 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXbsrsm2_zeroPivot(cusparseHandle_t handle,
            bsrsm2Info_t info,
            int* position);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsm2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsm2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsm2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsm2_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            int* pBufferSizeInBytes);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsm2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsm2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsm2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsm2_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            size_t* pBufferSize);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsm2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsm2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsm2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsm2_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrsm2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            float* alpha,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            float* F,
            int ldf,
            float* X,
            int ldx,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrsm2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            double* alpha,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            double* F,
            int ldf,
            double* X,
            int ldx,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrsm2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            cuComplex* F,
            int ldf,
            cuComplex* X,
            int ldx,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrsm2_solve(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            cusparseOperation_t transA,
            cusparseOperation_t transXY,
            int mb,
            int n,
            int nnzb,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrsm2Info_t info,
            cuDoubleComplex* F,
            int ldf,
            cuDoubleComplex* X,
            int ldx,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        /* --- Preconditioners --- */

        /* Description: Compute the incomplete-LU factorization with 0 fill-in (ILU0)
            of the matrix A stored in CSR format based on the information in the opaque 
            structure info that was obtained from the analysis phase (csrsv_analysis). 
            This routine implements algorithm 1 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrilu0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            float* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrilu0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            double* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrilu0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrilu0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        /* Description: Compute the incomplete-LU factorization with 0 fill-in (ILU0)
            of the matrix A stored in CSR format based on the information in the opaque 
            structure info that was obtained from the analysis phase (csrsv2_analysis).
            This routine implements algorithm 2 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrilu02_numericBoost(cusparseHandle_t handle,
            csrilu02Info_t info,
            int enable_boost,
            double* tol,
            float* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrilu02_numericBoost(cusparseHandle_t handle,
            csrilu02Info_t info,
            int enable_boost,
            double* tol,
            double* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrilu02_numericBoost(cusparseHandle_t handle,
            csrilu02Info_t info,
            int enable_boost,
            double* tol,
            cuComplex* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrilu02_numericBoost(cusparseHandle_t handle,
            csrilu02Info_t info,
            int enable_boost,
            double* tol,
            cuDoubleComplex* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrilu02_zeroPivot(cusparseHandle_t handle,
            csrilu02Info_t info,
            int* position);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrilu02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrilu02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrilu02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrilu02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrilu02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csrilu02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrilu02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csrilu02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrilu02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csrilu02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrilu02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csrilu02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrilu02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrilu02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrilu02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrilu02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrilu02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrilu02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrilu02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrilu02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        /* Description: Compute the incomplete-LU factorization with 0 fill-in (ILU0)
            of the matrix A stored in block-CSR format based on the information in the opaque 
            structure info that was obtained from the analysis phase (bsrsv2_analysis).
            This routine implements algorithm 2 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrilu02_numericBoost(cusparseHandle_t handle,
            bsrilu02Info_t info,
            int enable_boost,
            double* tol,
            float* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrilu02_numericBoost(cusparseHandle_t handle,
            bsrilu02Info_t info,
            int enable_boost,
            double* tol,
            double* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrilu02_numericBoost(cusparseHandle_t handle,
            bsrilu02Info_t info,
            int enable_boost,
            double* tol,
            cuComplex* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrilu02_numericBoost(cusparseHandle_t handle,
            bsrilu02Info_t info,
            int enable_boost,
            double* tol,
            cuDoubleComplex* boost_val);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXbsrilu02_zeroPivot(cusparseHandle_t handle,
            bsrilu02Info_t info,
            int* position);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrilu02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrilu02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrilu02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrilu02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrilu02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrilu02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrilu02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrilu02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrilu02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrilu02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrilu02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsrilu02Info_t info,
            size_t* pBufferSize);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrilu02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrilu02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrilu02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrilu02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsrilu02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descra,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsrilu02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descra,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsrilu02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descra,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsrilu02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descra,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsrilu02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        /* Description: Compute the incomplete-Cholesky factorization with 0 fill-in (IC0)
            of the matrix A stored in CSR format based on the information in the opaque 
            structure info that was obtained from the analysis phase (csrsv_analysis). 
            This routine implements algorithm 1 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsric0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            float* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsric0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            double* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsric0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsric0(cusparseHandle_t handle,
            cusparseOperation_t trans,
            int m,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA_ValM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseSolveAnalysisInfo_t info);

        /* Description: Compute the incomplete-Cholesky factorization with 0 fill-in (IC0)
            of the matrix A stored in CSR format based on the information in the opaque 
            structure info that was obtained from the analysis phase (csrsv2_analysis). 
            This routine implements algorithm 2 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsric02_zeroPivot(cusparseHandle_t handle,
            csric02Info_t info,
            int* position);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsric02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsric02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsric02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsric02_bufferSize(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsric02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csric02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsric02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csric02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsric02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csric02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsric02_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            csric02Info_t info,
            size_t* pBufferSize);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsric02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsric02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsric02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsric02_analysis(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsric02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsric02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsric02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsric02(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA_valM,
            /* matrix A values are updated inplace 
                to be the preconditioner M values */
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            csric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        /* Description: Compute the incomplete-Cholesky factorization with 0 fill-in (IC0)
            of the matrix A stored in block-CSR format based on the information in the opaque 
            structure info that was obtained from the analysis phase (bsrsv2_analysis). 
            This routine implements algorithm 1 for this problem. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXbsric02_zeroPivot(cusparseHandle_t handle,
            bsric02Info_t info,
            int* position);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsric02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsric02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsric02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsric02_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsric02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsric02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsric02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsric02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsric02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsric02Info_t info,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsric02_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockSize,
            bsric02Info_t info,
            size_t* pBufferSize);



        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsric02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pInputBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsric02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pInputBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsric02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pInputBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsric02_analysis(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pInputBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsric02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsric02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsric02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsric02(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int blockDim,
            bsric02Info_t info,
            cusparseSolvePolicy_t policy,
            void* pBuffer);


        /* Description: Solution of tridiagonal linear system A * X = F, 
            with multiple right-hand-sides. The coefficient matrix A is 
            composed of lower (dl), main (d) and upper (du) diagonals, and 
            the right-hand-sides F are overwritten with the solution X. 
            These routine use pivoting. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgtsv(cusparseHandle_t handle,
            int m,
            int n,
            float* dl,
            float* d,
            float* du,
            float* B,
            int ldb);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgtsv(cusparseHandle_t handle,
            int m,
            int n,
            double* dl,
            double* d,
            double* du,
            double* B,
            int ldb);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgtsv(cusparseHandle_t handle,
            int m,
            int n,
            cuComplex* dl,
            cuComplex* d,
            cuComplex* du,
            cuComplex* B,
            int ldb);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgtsv(cusparseHandle_t handle,
            int m,
            int n,
            cuDoubleComplex* dl,
            cuDoubleComplex* d,
            cuDoubleComplex* du,
            cuDoubleComplex* B,
            int ldb);

        /* Description: Solution of tridiagonal linear system A * X = F, 
            with multiple right-hand-sides. The coefficient matrix A is 
            composed of lower (dl), main (d) and upper (du) diagonals, and 
            the right-hand-sides F are overwritten with the solution X. 
            These routine does not use pivoting. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgtsv_nopivot(cusparseHandle_t handle,
            int m,
            int n,
            float* dl,
            float* d,
            float* du,
            float* B,
            int ldb);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgtsv_nopivot(cusparseHandle_t handle,
            int m,
            int n,
            double* dl,
            double* d,
            double* du,
            double* B,
            int ldb);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgtsv_nopivot(cusparseHandle_t handle,
            int m,
            int n,
            cuComplex* dl,
            cuComplex* d,
            cuComplex* du,
            cuComplex* B,
            int ldb);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgtsv_nopivot(cusparseHandle_t handle,
            int m,
            int n,
            cuDoubleComplex* dl,
            cuDoubleComplex* d,
            cuDoubleComplex* du,
            cuDoubleComplex* B,
            int ldb);

        /* Description: Solution of a set of tridiagonal linear systems 
            A_{i} * x_{i} = f_{i} for i=1,...,batchCount. The coefficient 
            matrices A_{i} are composed of lower (dl), main (d) and upper (du) 
            diagonals and stored separated by a batchStride. Also, the 
            right-hand-sides/solutions f_{i}/x_{i} are separated by a batchStride. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgtsvStridedBatch(cusparseHandle_t handle,
            int m,
            float* dl,
            float* d,
            float* du,
            float* x,
            int batchCount,
            int batchStride);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgtsvStridedBatch(cusparseHandle_t handle,
            int m,
            double* dl,
            double* d,
            double* du,
            double* x,
            int batchCount,
            int batchStride);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgtsvStridedBatch(cusparseHandle_t handle,
            int m,
            cuComplex* dl,
            cuComplex* d,
            cuComplex* du,
            cuComplex* x,
            int batchCount,
            int batchStride);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgtsvStridedBatch(cusparseHandle_t handle,
            int m,
            cuDoubleComplex* dl,
            cuDoubleComplex* d,
            cuDoubleComplex* du,
            cuDoubleComplex* x,
            int batchCount,
            int batchStride);

        #endregion

        #region /* --- Sparse Level 4 routines --- */

        /* Description: Compute sparse - sparse matrix multiplication for matrices 
stored in CSR format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrgemmNnz(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            int* csrSortedRowPtrC,
            int* nnzTotalDevHostPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrgemm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            cusparseMatDescr_t descrA,
            int nnzA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            float* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            float* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrgemm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            cusparseMatDescr_t descrA,
            int nnzA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            double* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            double* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrgemm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrgemm(cusparseHandle_t handle,
            cusparseOperation_t transA,
            cusparseOperation_t transB,
            int m,
            int n,
            int k,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuDoubleComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        /* Description: Compute sparse - sparse matrix multiplication for matrices 
            stored in CSR format. */

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateCsrgemm2Info(out csrgemm2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDestroyCsrgemm2Info(csrgemm2Info_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrgemm2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            float* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            float* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            csrgemm2Info_t info,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrgemm2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            double* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            double* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            csrgemm2Info_t info,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrgemm2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cuComplex* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            csrgemm2Info_t info,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrgemm2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cuDoubleComplex* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            csrgemm2Info_t info,
            size_t* pBufferSizeInBytes);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrgemm2Nnz(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrD,
            int nnzD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            cusparseMatDescr_t descrC,
            int* csrSortedRowPtrC,
            int* nnzTotalDevHostPtr,
            csrgemm2Info_t info,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrgemm2(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            float* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            float* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            float* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            float* csrSortedValD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            cusparseMatDescr_t descrC,
            float* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            csrgemm2Info_t info,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrgemm2(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            double* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            double* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            double* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            double* csrSortedValD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            cusparseMatDescr_t descrC,
            double* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            csrgemm2Info_t info,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrgemm2(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cuComplex* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            cuComplex* csrSortedValD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            cusparseMatDescr_t descrC,
            cuComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            csrgemm2Info_t info,
            void* pBuffer);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrgemm2(cusparseHandle_t handle,
            int m,
            int n,
            int k,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuDoubleComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cuDoubleComplex* beta,
            cusparseMatDescr_t descrD,
            int nnzD,
            cuDoubleComplex* csrSortedValD,
            int* csrSortedRowPtrD,
            int* csrSortedColIndD,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            csrgemm2Info_t info,
            void* pBuffer);


        /* Description: Compute sparse - sparse matrix addition of matrices 
            stored in CSR format */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrgeamNnz(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            int* csrSortedRowPtrC,
            int* nnzTotalDevHostPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrgeam(cusparseHandle_t handle,
            int m,
            int n,
            float* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            float* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            float* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrgeam(cusparseHandle_t handle,
            int m,
            int n,
            double* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            double* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            double* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrgeam(cusparseHandle_t handle,
            int m,
            int n,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrgeam(cusparseHandle_t handle,
            int m,
            int n,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuDoubleComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        /* Description: This function performs following sparse matrix-matrix operation
                alpha * A + beta * B */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrgeam2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            float* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            float* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            float* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrgeam2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            double* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            double* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            double* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrgeam2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrgeam2_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuDoubleComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            size_t* pBufferSizeInBytes);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrgeam2Nnz(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            int nnzA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrB,
            int nnzB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            int* csrSortedRowPtrC,
            int* nnzTotalDevHostPtr,
            void* workspace);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrgeam2(cusparseHandle_t handle,
            int m,
            int n,
            float* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            float* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            float* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrgeam2(cusparseHandle_t handle,
            int m,
            int n,
            double* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            double* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            double* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrgeam2(cusparseHandle_t handle,
            int m,
            int n,
            cuComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrgeam2(cusparseHandle_t handle,
            int m,
            int n,
            cuDoubleComplex* alpha,
            cusparseMatDescr_t descrA,
            int nnzA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* beta,
            cusparseMatDescr_t descrB,
            int nnzB,
            cuDoubleComplex* csrSortedValB,
            int* csrSortedRowPtrB,
            int* csrSortedColIndB,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC,
            void* pBuffer);

        /* --- Sparse Matrix Reorderings --- */

        /* Description: Find an approximate coloring of a matrix stored in CSR format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsrcolor(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* fractionToColor,
            int* ncolors,
            int* coloring,
            int* reordering,
            cusparseColorInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsrcolor(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* fractionToColor,
            int* ncolors,
            int* coloring,
            int* reordering,
            cusparseColorInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsrcolor(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* fractionToColor,
            int* ncolors,
            int* coloring,
            int* reordering,
            cusparseColorInfo_t info);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsrcolor(cusparseHandle_t handle,
            int m,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* fractionToColor,
            int* ncolors,
            int* coloring,
            int* reordering,
            cusparseColorInfo_t info);

        /* --- Sparse Format Conversion --- */

        /* Description: This routine finds the total number of non-zero elements and 
            the number of non-zero elements per row or column in the dense matrix A. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSnnz(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* A,
            int lda,
            int* nnzPerRowCol,
            int* nnzTotalDevHostPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDnnz(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* A,
            int lda,
            int* nnzPerRowCol,
            int* nnzTotalDevHostPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCnnz(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* A,
            int lda,
            int* nnzPerRowCol,
            int* nnzTotalDevHostPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZnnz(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* A,
            int lda,
            int* nnzPerRowCol,
            int* nnzTotalDevHostPtr);

        /* Description: This routine converts a dense matrix to a sparse matrix 
            in the CSR storage format, using the information computed by the 
            nnz routine. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSdense2csr(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* A,
            int lda,
            int* nnzPerRow,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDdense2csr(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* A,
            int lda,
            int* nnzPerRow,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCdense2csr(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* A,
            int lda,
            int* nnzPerRow,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZdense2csr(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* A,
            int lda,
            int* nnzPerRow,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        /* Description: This routine converts a sparse matrix in CSR storage format
            to a dense matrix. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            float* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            double* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuComplex* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cuDoubleComplex* A,
            int lda);

        /* Description: This routine converts a dense matrix to a sparse matrix 
            in the CSC storage format, using the information computed by the 
            nnz routine. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSdense2csc(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* A,
            int lda,
            int* nnzPerCol,
            float* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDdense2csc(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* A,
            int lda,
            int* nnzPerCol,
            double* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCdense2csc(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* A,
            int lda,
            int* nnzPerCol,
            cuComplex* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZdense2csc(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* A,
            int lda,
            int* nnzPerCol,
            cuDoubleComplex* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA);

        /* Description: This routine converts a sparse matrix in CSC storage format
            to a dense matrix. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsc2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            float* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsc2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            double* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsc2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            cuComplex* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsc2dense(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            cuDoubleComplex* A,
            int lda);

        /* Description: This routine compresses the indecis of rows or columns.
            It can be interpreted as a conversion from COO to CSR sparse storage
            format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcoo2csr(cusparseHandle_t handle,
            int* cooRowInd,
            int nnz,
            int m,
            int* csrSortedRowPtr,
            cusparseIndexBase_t idxBase);

        /* Description: This routine uncompresses the indecis of rows or columns.
            It can be interpreted as a conversion from CSR to COO sparse storage
            format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsr2coo(cusparseHandle_t handle,
            int* csrSortedRowPtr,
            int nnz,
            int m,
            int* cooRowInd,
            cusparseIndexBase_t idxBase);

        /* Description: This routine converts a matrix from CSR to CSC sparse 
            storage format. The resulting matrix can be re-interpreted as a 
            transpose of the original matrix in CSR storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2csc(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            float* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            float* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2csc(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            double* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            double* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2csc(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cuComplex* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            cuComplex* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t idxBase);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2csc(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cuDoubleComplex* csrSortedVal,
            int* csrSortedRowPtr,
            int* csrSortedColInd,
            cuDoubleComplex* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t idxBase);

        /* Description: This routine converts a dense matrix to a sparse matrix 
            in HYB storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSdense2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* A,
            int lda,
            int* nnzPerRow,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDdense2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* A,
            int lda,
            int* nnzPerRow,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCdense2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* A,
            int lda,
            int* nnzPerRow,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZdense2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* A,
            int lda,
            int* nnzPerRow,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        /* Description: This routine converts a sparse matrix in HYB storage format
            to a dense matrix. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseShyb2dense(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            float* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDhyb2dense(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            double* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseChyb2dense(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuComplex* A,
            int lda);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZhyb2dense(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuDoubleComplex* A,
            int lda);

        /* Description: This routine converts a sparse matrix in CSR storage format
            to a sparse matrix in HYB storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        /* Description: This routine converts a sparse matrix in HYB storage format
            to a sparse matrix in CSR storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseShyb2csr(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDhyb2csr(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseChyb2csr(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZhyb2csr(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA);

        /* Description: This routine converts a sparse matrix in CSC storage format
            to a sparse matrix in HYB storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsc2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsc2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsc2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsc2hyb(cusparseHandle_t handle,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* cscSortedValA,
            int* cscSortedRowIndA,
            int* cscSortedColPtrA,
            cusparseHybMat_t hybA,
            int userEllWidth,
            cusparseHybPartition_t partitionType);

        /* Description: This routine converts a sparse matrix in HYB storage format
            to a sparse matrix in CSC storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseShyb2csc(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            float* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDhyb2csc(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            double* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseChyb2csc(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuComplex* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZhyb2csc(cusparseHandle_t handle,
            cusparseMatDescr_t descrA,
            cusparseHybMat_t hybA,
            cuDoubleComplex* cscSortedVal,
            int* cscSortedRowInd,
            int* cscSortedColPtr);

        /* Description: This routine converts a sparse matrix in CSR storage format
            to a sparse matrix in block-CSR storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsr2bsrNnz(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            int* bsrSortedRowPtrC,
            int* nnzTotalDevHostPtr);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2bsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            float* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2bsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            double* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2bsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            cuComplex* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2bsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC);

        /* Description: This routine converts a sparse matrix in block-CSR storage format
            to a sparse matrix in CSR storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSbsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            float* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDbsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            double* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCbsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            cuComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZbsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int blockDim,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        /* Description: This routine converts a sparse matrix in general block-CSR storage format
            to a sparse matrix in general block-CSC storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgebsr2gebsc_bufferSize(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgebsr2gebsc_bufferSize(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgebsr2gebsc_bufferSize(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgebsr2gebsc_bufferSize(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgebsr2gebsc_bufferSizeExt(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgebsr2gebsc_bufferSizeExt(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgebsr2gebsc_bufferSizeExt(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgebsr2gebsc_bufferSizeExt(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgebsr2gebsc(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            float* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            float* bscVal,
            int* bscRowInd,
            int* bscColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t baseIdx,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgebsr2gebsc(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            double* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            double* bscVal,
            int* bscRowInd,
            int* bscColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t baseIdx,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgebsr2gebsc(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            cuComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            cuComplex* bscVal,
            int* bscRowInd,
            int* bscColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t baseIdx,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgebsr2gebsc(cusparseHandle_t handle,
            int mb,
            int nb,
            int nnzb,
            cuDoubleComplex* bsrSortedVal,
            int* bsrSortedRowPtr,
            int* bsrSortedColInd,
            int rowBlockDim,
            int colBlockDim,
            cuDoubleComplex* bscVal,
            int* bscRowInd,
            int* bscColPtr,
            cusparseAction_t copyValues,
            cusparseIndexBase_t baseIdx,
            void* pBuffer);

        /* Description: This routine converts a sparse matrix in general block-CSR storage format
            to a sparse matrix in CSR storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXgebsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            cusparseMatDescr_t descrC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgebsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            cusparseMatDescr_t descrC,
            float* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgebsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            cusparseMatDescr_t descrC,
            double* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgebsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            cusparseMatDescr_t descrC,
            cuComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgebsr2csr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* csrSortedValC,
            int* csrSortedRowPtrC,
            int* csrSortedColIndC);

        /* Description: This routine converts a sparse matrix in CSR storage format
            to a sparse matrix in general block-CSR storage format. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            int rowBlockDim,
            int colBlockDim,
            size_t* pBufferSize);



        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsr2gebsrNnz(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrC,
            int* bsrSortedRowPtrC,
            int rowBlockDim,
            int colBlockDim,
            int* nnzTotalDevHostPtr,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            float* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrC,
            float* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDim,
            int colBlockDim,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            double* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrC,
            double* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDim,
            int colBlockDim,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrC,
            cuComplex* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDim,
            int colBlockDim,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int m,
            int n,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrSortedValA,
            int* csrSortedRowPtrA,
            int* csrSortedColIndA,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDim,
            int colBlockDim,
            void* pBuffer);

        /* Description: This routine converts a sparse matrix in general block-CSR storage format
            to a sparse matrix in general block-CSR storage format with different block size. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgebsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgebsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgebsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            int* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgebsr2gebsr_bufferSize(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            int* pBufferSizeInBytes);


        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgebsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgebsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgebsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            size_t* pBufferSize);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgebsr2gebsr_bufferSizeExt(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            int rowBlockDimC,
            int colBlockDimC,
            size_t* pBufferSize);



        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXgebsr2gebsrNnz(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            cusparseMatDescr_t descrC,
            int* bsrSortedRowPtrC,
            int rowBlockDimC,
            int colBlockDimC,
            int* nnzTotalDevHostPtr,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseSgebsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            float* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            cusparseMatDescr_t descrC,
            float* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDimC,
            int colBlockDimC,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDgebsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            double* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            cusparseMatDescr_t descrC,
            double* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDimC,
            int colBlockDimC,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCgebsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            cusparseMatDescr_t descrC,
            cuComplex* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDimC,
            int colBlockDimC,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZgebsr2gebsr(cusparseHandle_t handle,
            cusparseDirection_t dirA,
            int mb,
            int nb,
            int nnzb,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* bsrSortedValA,
            int* bsrSortedRowPtrA,
            int* bsrSortedColIndA,
            int rowBlockDimA,
            int colBlockDimA,
            cusparseMatDescr_t descrC,
            cuDoubleComplex* bsrSortedValC,
            int* bsrSortedRowPtrC,
            int* bsrSortedColIndC,
            int rowBlockDimC,
            int colBlockDimC,
            void* pBuffer);

        #endregion

        #region /* --- Sparse Matrix Sorting --- */

        /* Description: Create a identity sequence p=[0,1,...,n-1]. */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCreateIdentityPermutation(cusparseHandle_t handle,
            int n,
            int* p);

        /* Description: Sort sparse matrix stored in COO format */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcoosort_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            int* cooRowsA,
            int* cooColsA,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcoosortByRow(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            int* cooRowsA,
            int* cooColsA,
            int* P,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcoosortByColumn(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            int* cooRowsA,
            int* cooColsA,
            int* P,
            void* pBuffer);

        /* Description: Sort sparse matrix stored in CSR format */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrsort_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            int* csrRowPtrA,
            int* csrColIndA,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcsrsort(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            int* csrRowPtrA,
            int* csrColIndA,
            int* P,
            void* pBuffer);

        /* Description: Sort sparse matrix stored in CSC format */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcscsort_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            int* cscColPtrA,
            int* cscRowIndA,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseXcscsort(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            int* cscColPtrA,
            int* cscRowIndA,
            int* P,
            void* pBuffer);

        /* Description: Wrapper that sorts sparse matrix stored in CSR format 
            (without exposing the permutation). */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsru2csr_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            float* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsru2csr_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            double* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsru2csr_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cuComplex* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsru2csr_bufferSizeExt(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cuDoubleComplex* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            size_t* pBufferSizeInBytes);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsru2csr(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsru2csr(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsru2csr(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsru2csr(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        /* Description: Wrapper that un-sorts sparse matrix stored in CSR format 
            (without exposing the permutation). */
        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseScsr2csru(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            float* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseDcsr2csru(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            double* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseCcsr2csru(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            cuComplex* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        [DllImport(CUSPARSE_DLL)]
        public static extern cusparseStatus_t cusparseZcsr2csru(cusparseHandle_t handle,
            int m,
            int n,
            int nnz,
            cusparseMatDescr_t descrA,
            cuDoubleComplex* csrVal,
            int* csrRowPtr,
            int* csrColInd,
            csru2csrInfo_t info,
            void* pBuffer);

        #endregion
    }
#pragma warning restore 1591
}
#pragma warning restore 0169