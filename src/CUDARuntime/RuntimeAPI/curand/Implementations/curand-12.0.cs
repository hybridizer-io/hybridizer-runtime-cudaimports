/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Runtime.InteropServices;

using size_t = System.UIntPtr ;

namespace Hybridizer.Runtime.CUDAImports
{
	 /// <summary>
    /// curand mapping
    /// Full documentation <see href="https://docs.nvidia.com/cuda/curand/index.html">here</see>
    /// </summary>
    #pragma warning disable 1591
    internal partial class curandImplem
    {
        [HybridizerIgnore("OMP,JAVA,CUDA")]
        internal class Curand64_120 : ICurand
        {
            public const string CURAND_DLL = "curand64_10.dll";

            [DllImport(CURAND_DLL, EntryPoint = "curandCreateGenerator")]
            private static extern curandStatus_t curandCreateGenerator_(out curandGenerator_t generator, curandRngType_t type);

            [DllImport(CURAND_DLL, EntryPoint = "curandGenerate")]
            private static extern curandStatus_t curandGenerate_(curandGenerator_t generator, IntPtr outputPtr, size_t num);

            [DllImport(CURAND_DLL, EntryPoint = "curandGenerateUniform")]
            private static extern curandStatus_t curandGenerateUniform_(curandGenerator_t generator, IntPtr outputPtr, size_t num);

            [DllImport(CURAND_DLL, EntryPoint = "curandGenerateUniform")]
            private static extern curandStatus_t curandGenerateNormal_(curandGenerator_t generator, IntPtr outputPtr, size_t n, float mean, float stddev);

            [DllImport(CURAND_DLL, EntryPoint = "curandGenerateLogNormal")]
            private static extern curandStatus_t curandGenerateLogNormal_(curandGenerator_t generator, IntPtr outputPtr, size_t n, float mean, float stddev);

            [DllImport(CURAND_DLL, EntryPoint = "curandGeneratePoisson")]
            private static extern curandStatus_t curandGeneratePoisson_(curandGenerator_t generator, IntPtr outputPtr, size_t n, double lambda);

            [DllImport(CURAND_DLL, EntryPoint = "curandGenerateUniformDouble")]
            private static extern curandStatus_t curandGenerateUniformDouble_(curandGenerator_t generator, IntPtr outputPtr, size_t num);

            [DllImport(CURAND_DLL, EntryPoint = "curandGenerateNormalDouble")]
            private static extern curandStatus_t curandGenerateNormalDouble_(curandGenerator_t generator, IntPtr outputPtr, size_t n, double mean, double stddev);

            [DllImport(CURAND_DLL, EntryPoint = "curandGenerateLogNormalDouble")]
            private static extern curandStatus_t curandGenerateLogNormalDouble_(curandGenerator_t generator, IntPtr outputPtr, size_t n, double mean, double stddev);

            [DllImport(CURAND_DLL, EntryPoint = "curandSetPseudoRandomGeneratorSeed")]
            private static extern curandStatus_t curandSetPseudoRandomGeneratorSeed_(curandGenerator_t prngGPU, UInt64 seed);

            [DllImport(CURAND_DLL, EntryPoint = "curandSetGeneratorOrdering")]
            private static extern curandStatus_t curandSetGeneratorOrdering_(curandGenerator_t prngGPU, curandOrdering_t seed);

            [DllImport(CURAND_DLL, EntryPoint = "curandSetGeneratorOffset")]
            private static extern curandStatus_t curandSetGeneratorOffset_(curandGenerator_t generator, UInt64 offset);


            public curandStatus_t curandSetGeneratorOffset(curandGenerator_t generator, ulong offset)
            {
                return curandSetGeneratorOffset_(generator, offset);
            }

            public curandStatus_t curandCreateGenerator(out curandGenerator_t generator, curandRngType_t type)
            {
                return curandCreateGenerator_(out generator, type);
            }

            public curandStatus_t curandGenerate(curandGenerator_t generator, IntPtr outputPtr, size_t num)
            {
                return curandGenerate_(generator, outputPtr, num);
            }

            public curandStatus_t curandGenerateUniform(curandGenerator_t generator, IntPtr outputPtr, size_t num)
            {
                return curandGenerateUniform_(generator, outputPtr, num);
            }

            public curandStatus_t curandGenerateNormal(curandGenerator_t generator, IntPtr outputPtr, size_t n,
                float mean, float stddev)
            {
                return curandGenerateNormal_(generator, outputPtr, n, mean, stddev);
            }

            public curandStatus_t curandGenerateLogNormal(curandGenerator_t generator, IntPtr outputPtr, size_t n,
                float mean, float stddev)
            {
                return curandGenerateLogNormal_(generator, outputPtr, n, mean, stddev);
            }

            public curandStatus_t curandGeneratePoisson(curandGenerator_t generator, IntPtr outputPtr, size_t n,
                double lambda)
            {
                return curandGeneratePoisson_(generator, outputPtr, n, lambda);
            }

            public curandStatus_t curandGenerateUniformDouble(curandGenerator_t generator, IntPtr outputPtr,
                size_t num)
            {
                return curandGenerateUniformDouble_(generator, outputPtr, num);
            }

            public curandStatus_t curandGenerateNormalDouble(curandGenerator_t generator, IntPtr outputPtr, size_t n, double mean, double stddev)
            {
                return curandGenerateNormalDouble_(generator, outputPtr, n, mean, stddev);
            }

            public curandStatus_t curandGenerateLogNormalDouble(curandGenerator_t generator, IntPtr outputPtr, size_t n, double mean, double stddev)
            {
                return curandGenerateLogNormalDouble_(generator, outputPtr, n, mean, stddev);
            }

            public curandStatus_t curandSetPseudoRandomGeneratorSeed(curandGenerator_t prngGPU, UInt64 seed)
            {
                return curandSetPseudoRandomGeneratorSeed_(prngGPU, seed);
            }

            public curandStatus_t curandSetGeneratorOrdering(curandGenerator_t prngGPU, curandOrdering_t order)
            {
                return curandSetGeneratorOrdering_(prngGPU, order);
            }
        }
    }
#pragma warning restore 1591
}
