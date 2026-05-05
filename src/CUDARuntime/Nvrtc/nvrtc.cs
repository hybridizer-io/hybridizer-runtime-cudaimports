/* (c) ALTIMESH 2019 -- all rights reserved */
using Altimesh.Hybridizer.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// nvrtc
    /// </summary>
    public static class nvrtc
    {
        /// <summary>
        /// get cuda version
        /// </summary>
        /// <returns></returns>
        public static string GetCudaVersion()
        {
            // cuda.GetCudaVersion() always returns a non-null, dot-stripped version
            // string (defaulting to the latest supported CUDA), so we just relay it.
            return cuda.GetCudaVersion();
        }

        static INvrtc instance;

        static nvrtc()
        {
            instance = SelectInstance(GetCudaVersion());
        }

        /// <summary>
        /// Re-select the nvrtc backend based on the current cuda version.
        /// Called by <see cref="cuda.SetCudaVersion"/> so that swapping the
        /// runtime version after type-init also swaps nvrtc.
        /// </summary>
        internal static void Reinitialize()
        {
            instance = SelectInstance(GetCudaVersion());
        }

        internal static INvrtc SelectInstance(string cudaVersion)
        {
            bool linux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            switch(cudaVersion)
            {
                case "132": return linux ? (INvrtc)new nvrtc132_linux() : new nvrtc132_windows();
                case "131": return linux ? (INvrtc)new nvrtc131_linux() : new nvrtc131_windows();
                case "130": return linux ? (INvrtc)new nvrtc130_linux() : new nvrtc130_windows();
                case "126": return linux ? (INvrtc)new nvrtc126_linux() : new nvrtc126_windows();
                case "124": return linux ? (INvrtc)new nvrtc124_linux() : new nvrtc124_windows();
                case "120": return linux ? (INvrtc)new nvrtc120_linux() : new nvrtc120_windows();
                case "114": return linux ? (INvrtc)new nvrtc114_linux() : new nvrtc114_windows();
                case "110": return linux ? (INvrtc)new nvrtc110_linux() : new nvrtc110_windows();
                case "101": return new nvrtc101();
                case "100": return new nvrtc10();
                default:
                    throw new NotImplementedException(string.Format("nvrtc is not mapped for CUDA version {0}", cudaVersion));
            }
        }

        /// <summary>
        /// destroy program
        /// </summary>
        /// <param name="prog"></param>
        /// <returns></returns>
        public static nvrtcResult DestroyProgram(ref nvrtcProgram prog)
        {
            return instance.DestroyProgram(ref prog);
        }

        /// <summary>
        /// create program
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="cudaSource"></param>
        /// <param name="cudaSourceName"></param>
        /// <param name="headers"></param>
        /// <param name="headerNames"></param>
        /// <returns></returns>
        public static nvrtcResult CreateProgram(out nvrtcProgram prog, string cudaSource, string cudaSourceName, string[] headers, string[] headerNames)
        {
            return instance.CreateProgram(out prog, cudaSource, cudaSourceName, headers, headerNames);
        }

        /// <summary>
        /// compile program
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static nvrtcResult CompileProgram(nvrtcProgram prog, string[] options)
        {
            return instance.CompileProgram(prog, options);
        }

        /// <summary>
        /// get logs
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static nvrtcResult GetProgramLog(nvrtcProgram prog, out string log)
        {
            return instance.GetProgramLog(prog, out log);
        }

        /// <summary>
        /// get ptx
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="ptx"></param>
        /// <returns></returns>
        public static nvrtcResult GetPTX(nvrtcProgram prog, out string ptx)
        {
            return instance.GetPTX(prog, out ptx);
        }

        /// <summary>
        /// get version
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <returns></returns>
        public static nvrtcResult Version(out int major, out int minor)
        {
            return instance.Version(out major, out minor);
        }

        /// <summary>
        /// get CUBIN (native GPU binary, available since CUDA 11.1)
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="cubin"></param>
        /// <returns></returns>
        public static nvrtcResult GetCUBIN(nvrtcProgram prog, out byte[] cubin)
        {
            return instance.GetCUBIN(prog, out cubin);
        }

        /// <summary>
        /// generate ptx from cuda
        /// </summary>
        /// <param name="cuda"></param>
        /// <param name="options"></param>
        /// <param name="ptx"></param>
        /// <param name="log"></param>
        /// <param name="headerNames"></param>
        /// <param name="headerContents"></param>
        /// <returns></returns>
        public static nvrtcResult GeneratePTX(string cuda, string[] options, out string ptx, out string log, string[] headerNames = null, string[] headerContents = null)
        {
            // TODO: compile
            nvrtcProgram prog;
            var nvres = nvrtc.CreateProgram(out prog, cuda, null, headerContents, headerNames);

            if (nvres != nvrtcResult.NVRTC_SUCCESS)
            {
                string compileLog;
                nvrtc.GetProgramLog(prog, out compileLog);
                log = "Compilation error - log : " + Environment.NewLine + compileLog;
                ptx = "";
                return nvres;
            }

            nvrtcResult compil = nvrtc.CompileProgram(prog, options);

            if (compil == nvrtcResult.NVRTC_ERROR_COMPILATION)
            {
                string compileLog;
                nvrtc.GetProgramLog(prog, out compileLog);
                log = "Compilation error - log : " + Environment.NewLine + compileLog;
                ptx = "";
                return compil;
            }

            if (compil == nvrtcResult.NVRTC_ERROR_INVALID_OPTION)
            {
                string compileLog;
                nvrtc.GetProgramLog(prog, out compileLog);
                log = "Invalid option - log : " + Environment.NewLine + compileLog;
                ptx = "";
                return compil;
            }

            if (compil != nvrtcResult.NVRTC_SUCCESS)
            {
                string compileLog;
                nvrtc.GetProgramLog(prog, out compileLog);
                log = String.Format("{0} error - log : {1}", compil, Environment.NewLine + compileLog);
                ptx = "";
                return compil;
            }

            nvres = nvrtc.GetPTX(prog, out ptx);
            log = "Compilation OK -- PTX generated";
            return nvrtcResult.NVRTC_SUCCESS;
        }
    }
}
