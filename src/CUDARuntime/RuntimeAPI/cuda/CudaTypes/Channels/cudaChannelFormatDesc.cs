using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// CUDA Channel format descriptor
    /// </summary>
    [IntrinsicType("cudaChannelFormatDesc")]
#if PLATFORM_X86
    [StructLayout(LayoutKind.Explicit, Size = 20)] 
#elif PLATFORM_X64
    [StructLayout(LayoutKind.Explicit, Size = 20)]
#else
#error Unsupported Platform
#endif
    public struct cudaChannelFormatDesc
    {
        /// <summary>
        /// x
        /// </summary>
        [FieldOffset(0)]
        public int x;
        /// <summary>
        /// y
        /// </summary>
        [FieldOffset(4)]
        public int y;
        /// <summary>
        /// z
        /// </summary>
        [FieldOffset(8)]
        public int z;
        /// <summary>
        /// w
        /// </summary>
        [FieldOffset(12)]
        public int w;
        /// <summary>
        /// Channel format kind
        /// </summary>
        [FieldOffset(16)]
        public cudaChannelFormatKind f;
    }
}