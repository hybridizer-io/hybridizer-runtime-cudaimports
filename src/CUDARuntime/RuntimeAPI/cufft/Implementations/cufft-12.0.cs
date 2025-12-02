/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// cufft wrapper
    /// Complete documentation <see href="https://docs.nvidia.com/cuda/cufft/index.html">here</see>
    /// </summary>
    internal partial class cufftImplem
    {
        internal class CUFFT_64_120 : ICUFFT
        {
            // no cuufft upgrade this time
            private const string CUFFT_DLL = "cufft64_10.dll";

            [DllImport(CUFFT_DLL, EntryPoint = "cufftCreate", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftCreate(out cufftHandle plan);
            public cufftResult Create(out cufftHandle plan) { return cufftCreate(out plan); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftDestroy", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftDestroy(cufftHandle plan);
            public cufftResult Destroy(cufftHandle plan) { return cufftDestroy(plan); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftEstimate1d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftEstimate1d(int nx, cufftType type, int batch, out size_t workSize);
            public cufftResult Estimate1d(int nx, cufftType type, int batch, out size_t workSize) { return cufftEstimate1d(nx, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftEstimate2d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftEstimate2d(int nx, int ny, cufftType type, out size_t workSize);
            public cufftResult Estimate2d(int nx, int ny, cufftType type, out size_t workSize) { return cufftEstimate2d(nx, ny, type, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftEstimate3d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftEstimate3d(int nx, int ny, int nz, cufftType type, out size_t workSize);
            public cufftResult Estimate3d(int nx, int ny, int nz, cufftType type, out size_t workSize) { return cufftEstimate3d(nx, ny, nz, type, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftEstimateMany", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftEstimateMany(int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize);
            public cufftResult EstimateMany(int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize) { return cufftEstimateMany(rank, n, inembed, istride, idist, onembed, ostride, odist, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftExecC2C", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftExecC2C(cufftHandle plan, IntPtr idata, IntPtr odata, int direction);
            public cufftResult ExecC2C(cufftHandle plan, IntPtr idata, IntPtr odata, int direction) { return cufftExecC2C(plan, idata, odata, direction); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftExecC2R", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftExecC2R(cufftHandle plan, IntPtr idata, IntPtr odata);
            public cufftResult ExecC2R(cufftHandle plan, IntPtr idata, IntPtr odata) { return cufftExecC2R(plan, idata, odata); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftExecD2Z", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftExecD2Z(cufftHandle plan, IntPtr idata, IntPtr odata);
            public cufftResult ExecD2Z(cufftHandle plan, IntPtr idata, IntPtr odata) { return cufftExecD2Z(plan, idata, odata); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftExecR2C", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftExecR2C(cufftHandle plan, IntPtr idata, IntPtr odata);
            public cufftResult ExecR2C(cufftHandle plan, IntPtr idata, IntPtr odata) { return cufftExecR2C(plan, idata, odata); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftExecZ2D", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftExecZ2D(cufftHandle plan, IntPtr idata, IntPtr odata);
            public cufftResult ExecZ2D(cufftHandle plan, IntPtr idata, IntPtr odata) { return cufftExecZ2D(plan, idata, odata); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftExecZ2Z", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftExecZ2Z(cufftHandle plan, IntPtr idata, IntPtr odata, int direction);
            public cufftResult ExecZ2Z(cufftHandle plan, IntPtr idata, IntPtr odata, int direction) { return cufftExecZ2Z(plan, idata, odata, direction); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetProperty", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetProperty(libraryPropertyType_t type, out int val);
            public cufftResult GetProperty(libraryPropertyType_t type, out int val) { return cufftGetProperty(type, out val); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetSize", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetSize(cufftHandle handle, out size_t workSize);
            public cufftResult GetSize(cufftHandle handle, out size_t workSize) { return cufftGetSize(handle, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetSize1d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetSize1d(cufftHandle handle, int nx, cufftType type, int batch, out size_t workSize);
            public cufftResult GetSize1d(cufftHandle handle, int nx, cufftType type, int batch, out size_t workSize) { return cufftGetSize1d(handle, nx, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetSize2d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetSize2d(cufftHandle handle, int nx, int ny, cufftType type, out size_t workSize);
            public cufftResult GetSize2d(cufftHandle handle, int nx, int ny, cufftType type, out size_t workSize) { return cufftGetSize2d(handle, nx, ny, type, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetSize3d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetSize3d(cufftHandle handle, int nx, int ny, int nz, cufftType type, out size_t workSize);
            public cufftResult GetSize3d(cufftHandle handle, int nx, int ny, int nz, cufftType type, out size_t workSize) { return cufftGetSize3d(handle, nx, ny, nz, type, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetSizeMany", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetSizeMany(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize);
            public cufftResult GetSizeMany(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize) { return cufftGetSizeMany(plan, rank, n, inembed, istride, idist, onembed, ostride, odist, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetSizeMany64", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetSizeMany64(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, long istride, long idist, IntPtr onembed, long ostride, long odist, cufftType type, long batch, out size_t workSize);
            public cufftResult GetSizeMany64(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, long istride, long idist, IntPtr onembed, long ostride, long odist, cufftType type, long batch, out size_t workSize) { return cufftGetSizeMany64(plan, rank, n, inembed, istride, idist, onembed, ostride, odist, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftGetVersion", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftGetVersion(out int version);
            public cufftResult GetVersion(out int version) { return cufftGetVersion(out version); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftMakePlan1d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftMakePlan1d(cufftHandle plan, int nx, cufftType type, int batch, out size_t workSize);
            public cufftResult MakePlan1d(cufftHandle plan, int nx, cufftType type, int batch, out size_t workSize) { return cufftMakePlan1d(plan, nx, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftMakePlan2d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftMakePlan2d(cufftHandle plan, int nx, int ny, cufftType type, out size_t workSize);
            public cufftResult MakePlan2d(cufftHandle plan, int nx, int ny, cufftType type, out size_t workSize) { return cufftMakePlan2d(plan, nx, ny, type, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftMakePlan3d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftMakePlan3d(cufftHandle plan, int nx, int ny, int nz, cufftType type, out size_t workSize);
            public cufftResult MakePlan3d(cufftHandle plan, int nx, int ny, int nz, cufftType type, out size_t workSize) { return cufftMakePlan3d(plan, nx, ny, nz, type, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftMakePlanMany", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftMakePlanMany(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize);
            public cufftResult MakePlanMany(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch, out size_t workSize) { return cufftMakePlanMany(plan, rank, n, inembed, istride, idist, onembed, ostride, odist, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftMakePlanMany64", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftMakePlanMany64(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, long istride, long idist, IntPtr onembed, long ostride, long odist, cufftType type, long batch, out size_t workSize);
            public cufftResult MakePlanMany64(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, long istride, long idist, IntPtr onembed, long ostride, long odist, cufftType type, long batch, out size_t workSize) { return cufftMakePlanMany64(plan, rank, n, inembed, istride, idist, onembed, ostride, odist, type, batch, out workSize); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftPlan1d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftPlan1d(out cufftHandle plan, int nx, cufftType type, int batch);
            public cufftResult Plan1d(out cufftHandle plan, int nx, cufftType type, int batch) { return cufftPlan1d(out plan, nx, type, batch); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftPlan2d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftPlan2d(out cufftHandle plan, int nx, int ny, cufftType type);
            public cufftResult Plan2d(out cufftHandle plan, int nx, int ny, cufftType type) { return cufftPlan2d(out plan, nx, ny, type); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftPlan3d", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftPlan3d(out cufftHandle plan, int nx, int ny, int nz, cufftType type);
            public cufftResult Plan3d(out cufftHandle plan, int nx, int ny, int nz, cufftType type) { return cufftPlan3d(out plan, nx, ny, nz, type); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftPlanMany", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftPlanMany(out cufftHandle plan, int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch);
            public cufftResult PlanMany(out cufftHandle plan, int rank, IntPtr n, IntPtr inembed, int istride, int idist, IntPtr onembed, int ostride, int odist, cufftType type, int batch) { return cufftPlanMany(out plan, rank, n, inembed, istride, idist, onembed, ostride, odist, type, batch); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftSetAutoAllocation", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftSetAutoAllocation(cufftHandle plan, int autoAllocate);
            public cufftResult SetAutoAllocate(cufftHandle plan, int autoAllocate) { return cufftSetAutoAllocation(plan, autoAllocate); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftSetStream", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftSetStream(cufftHandle plan, cudaStream_t stream);
            public cufftResult SetStream(cufftHandle plan, cudaStream_t stream) { return cufftSetStream(plan, stream); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftSetWorkArea", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftSetWorkArea(cufftHandle plan, IntPtr workArea);
            public cufftResult SetWorkArea(cufftHandle plan, IntPtr workArea) { return cufftSetWorkArea(plan, workArea); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftSetCompatibilityMode", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftSetCompatibilityMode(cufftHandle plan, cufftCompatibility mode);
            public cufftResult SetCompatibilityMode(cufftHandle plan, cufftCompatibility mode) { return cufftSetCompatibilityMode(plan, mode); }

            [DllImport(CUFFT_DLL, EntryPoint = "cufftXtMakePlanMany", CallingConvention = CallingConvention.StdCall)]
            private static extern cufftResult cufftXtMakePlanMany(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, long istride, long idist, cudaDataType_t inputtype, IntPtr onembed, long ostride, long odist, cudaDataType_t outputtype, long batch, out size_t workSize, cudaDataType_t executiontype);
            public cufftResult XtMakePlanMany(cufftHandle plan, int rank, IntPtr n, IntPtr inembed, long istride, long idist, cudaDataType_t inputtype, IntPtr onembed, long ostride, long odist, cudaDataType_t outputtype, long batch, out size_t workSize, cudaDataType_t executiontype) { return cufftXtMakePlanMany(plan, rank, n, inembed, istride, idist, inputtype, onembed, ostride, odist, outputtype, batch, out workSize, executiontype); }

            // TODO: cufftXt...
        }

    }
}