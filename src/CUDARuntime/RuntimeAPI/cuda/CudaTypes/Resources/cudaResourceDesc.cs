using System.Runtime.InteropServices;
using System;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// CUDA resource descriptor
    /// </summary>
    [IntrinsicType("cudaResourceDesc")]
#if PLATFORM_X86
    [StructLayout(LayoutKind.Explicit, Size = 40)] 
#elif PLATFORM_X64
    [StructLayout(LayoutKind.Explicit, Size = 64)]
#else
#error Unsupported Platform
#endif
    public struct cudaResourceDesc
    {
#if PLATFORM_X86
        [FieldOffset(0)]
        public cudaResourceType resType;
        [FieldOffset(4)]
        public cudaArray_t arrayStruct;
        [FieldOffset(4)]
        public cudaMipmappedArray_t mipmap;

        [StructLayout(LayoutKind.Explicit, Size = 28)]
        public struct cudaResourceDesc_linear
        {
            [FieldOffset(0)]
            public IntPtr devPtr;
            [FieldOffset(4)]
            public cudaChannelFormatDesc desc ;
            [FieldOffset(24)]
            public size_t sizeInBytes;
        }
        [FieldOffset(4)]
        public cudaResourceDesc_linear linear ;

        [StructLayout(LayoutKind.Explicit, Size = 36)]
        public struct cudaResourceDesc_pitch2D
        {
            [FieldOffset(0)]
            public IntPtr devPtr;
            [FieldOffset(4)]
            public cudaChannelFormatDesc desc ;
            [FieldOffset(24)]
            public size_t width;
            [FieldOffset(28)]
            public size_t height;
            [FieldOffset(32)]
            public size_t pitchInBytes;
        }
        [FieldOffset(4)]
        public cudaResourceDesc_pitch2D pitch2D;
#elif PLATFORM_X64
        /// <summary>
        /// Resource type
        /// </summary>
        [FieldOffset(0)]
        public cudaResourceType resType;
        /// <summary>
        /// CUDA array
        /// </summary>
        [FieldOffset(8)]
        public cudaArray_t arrayStruct;
        /// <summary>
        /// CUDA mipmapped array
        /// </summary>
        [FieldOffset(8)]
        public cudaMipmappedArray_t mipmap;

        /// <summary>
        /// linear
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 40)]
        public struct cudaResourceDesc_linear
        {
            /// <summary>
            /// device pointer
            /// </summary>
            [FieldOffset(0)]
            public IntPtr devPtr;
            /// <summary>
            /// Channel descriptor
            /// </summary>
            [FieldOffset(8)]
            public cudaChannelFormatDesc desc;
            /// <summary>
            /// Size in bytes
            /// </summary>
            [FieldOffset(32)]
            public size_t sizeInBytes;
        }
        /// <summary>
        /// linear
        /// </summary>
        [FieldOffset(8)]
        public cudaResourceDesc_linear linear;

        /// <summary>
        /// pitch2D
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 56)]
        public struct cudaResourceDesc_pitch2D
        {
            /// <summary>
            /// Device pointer
            /// </summary>
            [FieldOffset(0)]
            public IntPtr devPtr;
            /// <summary>
            /// Channel descriptor
            /// </summary>
            [FieldOffset(8)]
            public cudaChannelFormatDesc desc;
            /// <summary>
            /// Width of the array in elements
            /// </summary>
            [FieldOffset(32)]
            public size_t width;
            /// <summary>
            /// Height of the array in elements
            /// </summary>
            [FieldOffset(40)]
            public size_t height;
            /// <summary>
            /// Pitch between two rows in bytes
            /// </summary>
            [FieldOffset(48)]
            public size_t pitchInBytes;
        }
        /// <summary>
        /// pitch2D
        /// </summary>
        [FieldOffset(8)]
        public cudaResourceDesc_pitch2D pitch2D;
#else
#error Unsupported Platform
#endif
    }
}