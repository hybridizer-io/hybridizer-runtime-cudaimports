/* (c) ALTIMESH 2022 -- all rights reserved */
using Altimesh.Hybridizer.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Hybridizer.Runtime.CUDAImports
{
    internal class nvrtc130 : INvrtc
    {
        const string DLL_NAME = "nvrtc";

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        private static extern nvrtcResult nvrtcCreateProgram(out nvrtcProgram prog,
            [MarshalAs(UnmanagedType.LPStr)] string cudaSource,
            [MarshalAs(UnmanagedType.LPStr)] string cudaSourceName, int numHeader, IntPtr headers, IntPtr headerNames);

        public nvrtcResult CreateProgram(out nvrtcProgram prog, string cudaSource, string cudaSourceName, string[] headers, string[] headerNames)
        {
            int num = 0;
            if ((headers != null) && (headerNames != null))
                num = Math.Min(headers.Length, headerNames.Length);
            using (StringArrayMarshal cstr_headers = new StringArrayMarshal(headers))
            {
                using (StringArrayMarshal cstr_headerNames = new StringArrayMarshal(headerNames))
                {
                    return nvrtcCreateProgram(out prog, cudaSource, cudaSourceName, num, cstr_headers.Ptr, cstr_headerNames.Ptr);
                }
            }
        }

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcCompileProgram(
            nvrtcProgram prog,
            int numOptions,
            IntPtr options);

        public nvrtcResult CompileProgram(nvrtcProgram prog, string[] options)
        {
            int num = options.Length;
            using (StringArrayMarshal cstr_options = new StringArrayMarshal(options))
            {
                return nvrtcCompileProgram(prog, num, cstr_options.Ptr);
            }
        }

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcDestroyProgram(ref nvrtcProgram prog);

        public nvrtcResult DestroyProgram(ref nvrtcProgram prog)
        {
            return nvrtcDestroyProgram(ref prog);
        }

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcGetProgramLogSize(
            nvrtcProgram prog,
            out ulong logSize);

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcGetProgramLog(
            nvrtcProgram prog,
            IntPtr log);

        public nvrtcResult GetProgramLog(nvrtcProgram prog, out string log)
        {
            log = string.Empty;
            ulong logsize;
            nvrtcResult res = nvrtcGetProgramLogSize(prog, out logsize);
            if (res != nvrtcResult.NVRTC_SUCCESS) return res;
            byte[] data = new byte[logsize];
            GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
            res = nvrtcGetProgramLog(prog, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));
            gch.Free();
            log = ASCIIEncoding.ASCII.GetString(data);
            return res;
        }

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcGetPTXSize(
            nvrtcProgram prog,
            out ulong ptxSize);

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcGetPTX(
            nvrtcProgram prog,
            IntPtr ptx);

        public nvrtcResult GetPTX(nvrtcProgram prog, out string ptx)
        {
            ptx = string.Empty;
            ulong logsize;
            nvrtcResult res = nvrtcGetPTXSize(prog, out logsize);
            if (res != nvrtcResult.NVRTC_SUCCESS) return res;
            byte[] data = new byte[logsize];
            GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
            res = nvrtcGetPTX(prog, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));
            gch.Free();
            ptx = ASCIIEncoding.ASCII.GetString(data);
            return res;
        }

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcVersion(out int major, out int minor);

        public nvrtcResult Version(out int major, out int minor)
        {
            return nvrtcVersion(out major, out minor);
        }

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcGetCUBINSize(
            nvrtcProgram prog,
            out ulong cubinSize);

        [DllImport(DLL_NAME)]
        public static extern nvrtcResult nvrtcGetCUBIN(
            nvrtcProgram prog,
            IntPtr cubin);

        public nvrtcResult GetCUBIN(nvrtcProgram prog, out byte[] cubin)
        {
            cubin = null;
            ulong cubinSize;
            nvrtcResult res = nvrtcGetCUBINSize(prog, out cubinSize);
            if (res != nvrtcResult.NVRTC_SUCCESS) return res;
            cubin = new byte[cubinSize];
            GCHandle gch = GCHandle.Alloc(cubin, GCHandleType.Pinned);
            res = nvrtcGetCUBIN(prog, Marshal.UnsafeAddrOfPinnedArrayElement(cubin, 0));
            gch.Free();
            return res;
        }
    }
}
