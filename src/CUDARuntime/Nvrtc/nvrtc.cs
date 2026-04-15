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
            // If not, get the version configured in app.config
            string cudaVersion = cuda.GetCudaVersion();

            // Otherwise default to latest version
            if (cudaVersion == null) cudaVersion = "80";
            cudaVersion = cudaVersion.Replace(".", ""); // Remove all dots ("7.5" will be understood "75")

            return cudaVersion;
        }

        static INvrtc instance;

        static nvrtc()
        {
            string cudaVersion = GetCudaVersion();
            switch(cudaVersion)
            {
                case "131":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? (INvrtc)new nvrtc131_linux() : new nvrtc131_windows();
                    break;
                case "130":
                    instance = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? (INvrtc)new nvrtc130_linux() : new nvrtc130_windows();
                    break;
                case "101":
                    instance = new nvrtc101();
                    break;
                case "100":
                    instance = new nvrtc10();
                    break;
                case "110":
                case "114":
                case "120":
                case "124":
                case "126":
                    throw new NotImplementedException(string.Format("nvrtc is not yet mapped for CUDA {0} -- use 13.0+ or 10.x", cudaVersion));
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
