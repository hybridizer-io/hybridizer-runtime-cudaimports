using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// CUDA Channel format descriptor
    /// </summary>
    [IntrinsicType("cudaChannelFormatDesc")]
    [StructLayout(LayoutKind.Explicit, Size = 20)]
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