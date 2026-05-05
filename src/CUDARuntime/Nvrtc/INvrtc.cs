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
    /// nvrts interface
    /// </summary>
    public interface INvrtc
    {
        /// <summary>
        /// destroy program
        /// </summary>
        /// <param name="prog"></param>
        /// <returns></returns>
        nvrtcResult DestroyProgram(ref nvrtcProgram prog);
        /// <summary>
        /// create program
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="cudaSource"></param>
        /// <param name="cudaSourceName"></param>
        /// <param name="headers"></param>
        /// <param name="headerNames"></param>
        /// <returns></returns>
        nvrtcResult CreateProgram(out nvrtcProgram prog, string cudaSource, string cudaSourceName,
                            string[] headers, string[] headerNames);
        /// <summary>
        /// compile program
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        nvrtcResult CompileProgram(nvrtcProgram prog, string[] options);
        /// <summary>
        /// get logs
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        nvrtcResult GetProgramLog(nvrtcProgram prog, out string log);
        /// <summary>
        /// get ptx
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        nvrtcResult GetPTX(nvrtcProgram prog, out string log);
        /// <summary>
        /// get version
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <returns></returns>
        nvrtcResult Version(out int major, out int minor);
        /// <summary>
        /// get CUBIN (native GPU binary, available since CUDA 11.1)
        /// </summary>
        /// <param name="prog"></param>
        /// <param name="cubin"></param>
        /// <returns></returns>
        nvrtcResult GetCUBIN(nvrtcProgram prog, out byte[] cubin);
    }
}
