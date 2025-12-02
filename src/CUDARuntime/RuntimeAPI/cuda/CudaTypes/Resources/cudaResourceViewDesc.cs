using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// CUDA resource view descriptor
    /// </summary>
    [IntrinsicType("cudaResourceViewDesc")]
    [StructLayout(LayoutKind.Explicit, Size = 48)]
    public struct cudaResourceViewDesc
    {
        /// <summary>
        /// Resource view format
        /// </summary>
        [FieldOffset(0)]
        cudaResourceViewFormat format;
        /// <summary>
        /// Width of the resource view
        /// </summary>
        [FieldOffset(8)]
        size_t width;
        /// <summary>
        /// Height of the resource view
        /// </summary>
        [FieldOffset(16)]
        size_t height;
        /// <summary>
        /// Depth of the resource view
        /// </summary>
        [FieldOffset(24)]
        size_t depth;
        /// <summary>
        /// First defined mipmap level
        /// </summary>
        [FieldOffset(32)]
        uint firstMipmapLevel;
        /// <summary>
        /// Last defined mipmap level
        /// </summary>
        [FieldOffset(36)]
        uint lastMipmapLevel;
        /// <summary>
        /// First layer index
        /// </summary>
        [FieldOffset(40)]
        uint firstLayer;
        /// <summary>
        /// Last layer index
        /// </summary>
        [FieldOffset(44)]
        uint lastlayer;
    }
}