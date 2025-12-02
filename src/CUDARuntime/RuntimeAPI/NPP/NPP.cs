using System;
namespace Hybridizer.Runtime.CUDAImports
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class NPPI
    {
        static INPPI instance { get; set; }

        static NPPI()
        {
            var version = GetCudaVersion();
            switch (version)
            {
                case "122":
                case "121":
                case "120":
                    if (IntPtr.Size == 8)
                        instance = new NPPI_12_0();
                    else
                        throw new NotSupportedException("nppi 11 dropped 32 bits support");
                    break;
                case "117":
                case "116":
                case "114":
                case "110":
                    if (IntPtr.Size == 8)
                        instance = new NPPI_11_0();
                    else
                        throw new NotSupportedException("nppi 11 dropped 32 bits support");
                    break;
                case "101":
                    if (IntPtr.Size == 8)
                        instance = new NPPI_10_1();
                    else
                        throw new NotSupportedException("cublas 10.1 dropped 32 bits support");
                    break;
                default:
                    throw new NotImplementedException("NPP is only supported for CUDA 10.1");
            }
        }

        [IntrinsicFunction("nppiBoundSegments_8u_C1IR")]
        public static NppStatus BoundSegments_8u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, byte nBorderVal)
        {
            return instance.BoundSegments_8u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal);
        }

        [IntrinsicFunction("nppiBoundSegments_8u_C1IR_Ctx")]
        public static NppStatus BoundSegments_8u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, byte nBorderVal, NppStreamContext ctx)
        {
            return instance.BoundSegments_8u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal, ctx);
        }


        [IntrinsicFunction("nppiBoundSegments_16u_C1IR")]
        public static NppStatus BoundSegments_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nBorderVal)
        {
            return instance.BoundSegments_16u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal);
        }

        [IntrinsicFunction("nppiBoundSegments_16u_C1IR_Ctx")]
        public static NppStatus BoundSegments_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nBorderVal, NppStreamContext ctx)
        {
            return instance.BoundSegments_16u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal, ctx);
        }


        [IntrinsicFunction("nppiCompressMarkerLabels_32u8u_C1R")]
        public static NppStatus CompressMarkerLabels_32u8u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer)
        {
            return instance.CompressMarkerLabels_32u8u_C1R(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer);
        }

        [IntrinsicFunction("nppiCompressMarkerLabels_32u8u_C1R_Ctx")]
        public static NppStatus CompressMarkerLabels_32u8u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return instance.CompressMarkerLabels_32u8u_C1R_Ctx(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer, ctx);
        }


        [IntrinsicFunction("nppiCompressMarkerLabels_16u_C1IR")]
        public static NppStatus CompressMarkerLabels_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer)
        {
            return instance.CompressMarkerLabels_16u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer);
        }

        [IntrinsicFunction("nppiCompressMarkerLabels_16u_C1IR_Ctx")]
        public static NppStatus CompressMarkerLabels_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return instance.CompressMarkerLabels_16u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer, ctx);
        }

        [IntrinsicFunction("nppiCompressMarkerLabels_32u_C1IR_Ctx")]
        public static NppStatus CompressMarkerLabels_32u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return instance.CompressMarkerLabels_32u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer, ctx);
        }

        [IntrinsicFunction("nppiCompressMarkerLabels_32u_C1IR")]
        public static NppStatus CompressMarkerLabels_32u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer)
        {
            return instance.CompressMarkerLabels_32u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer);
        }


        [IntrinsicFunction("nppiLabelMarkers_8u32u_C1R")]
        public static NppStatus LabelMarkers_8u32u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, byte nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer)
        {
            return instance.LabelMarkers_8u32u_C1R(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer);
        }

        [IntrinsicFunction("nppiLabelMarkers_8u32u_C1R_Ctx")]
        public static NppStatus LabelMarkers_8u32u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, byte nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return instance.LabelMarkers_8u32u_C1R_Ctx(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer, ctx);
        }


        [IntrinsicFunction("nppiLabelMarkersUF_16u32u_C1R")]
        public static NppStatus LabelMarkersUF_16u32u_C1R(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer)
        {
            return instance.LabelMarkersUF_16u32u_C1R(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer);
        }

        [IntrinsicFunction("nppiLabelMarkersUF_16u32u_C1R_Ctx")]
        public static NppStatus LabelMarkersUF_16u32u_C1R_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return instance.LabelMarkersUF_16u32u_C1R_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer, ctx);
        }

        [IntrinsicFunction("nppiLabelMarkersUF_32u_C1R_Ctx")]
        public static NppStatus LabelMarkersUF_32u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, NppiNorm eNorm, IntPtr pBuffer, NppStreamContext ctx)
        {
            return instance.LabelMarkersUF_32u_C1R_Ctx(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, eNorm, pBuffer, ctx);
        }

        [IntrinsicFunction("nppiLabelMarkersUF_32u_C1R")]
        public static NppStatus LabelMarkersUF_32u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, NppiNorm eNorm, IntPtr pBuffer)
        {
            return instance.LabelMarkersUF_32u_C1R(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, eNorm, pBuffer);
        }

        [IntrinsicFunction("nppiLabelMarkers_16u_C1IR")]
        public static NppStatus LabelMarkers_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer)
        {
            return instance.LabelMarkers_16u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer);
        }

        [IntrinsicFunction("nppiLabelMarkers_16u_C1IR_Ctx")]
        public static NppStatus LabelMarkers_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return instance.LabelMarkers_16u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer, ctx);
        }

        [IntrinsicFunction("nppiCompressMarkerLabelsGetBufferSize_32u8u_C1R")]
        public static NppStatus CompressMarkerLabelsGetBufferSize_32u8u_C1R(int maxLabel, out int bufferSize)
        {
            return instance.CompressMarkerLabelsGetBufferSize_32u8u_C1R(maxLabel, out bufferSize);
        }

        [IntrinsicFunction("nppiCompressMarkerLabelsGetBufferSize_16u_C1R")]
        public static NppStatus CompressMarkerLabelsGetBufferSize_16u_C1R(int maxLabel, out int bufferSize)
        {
            return instance.CompressMarkerLabelsGetBufferSize_16u_C1R(maxLabel, out bufferSize);
        }

        [IntrinsicFunction("nppiLabelMarkersGetBufferSize_8u32u_C1R")]
        public static NppStatus LabelMarkersGetBufferSize_8u32u_C1R(NppiSize oSizeROI, out int nBufferSize)
        {
            return instance.LabelMarkersGetBufferSize_8u32u_C1R(oSizeROI, out nBufferSize);
        }

        [IntrinsicFunction("nppiCompressMarkerLabelsGetBufferSize_32u_C1R")]
        public static NppStatus CompressMarkerLabelsGetBufferSize_32u_C1R(int nStartingNumber, out int nBufferSize)
        {
            return instance.CompressMarkerLabelsGetBufferSize_32u_C1R(nStartingNumber, out nBufferSize);
        }

        [IntrinsicFunction("nppiLabelMarkersGetBufferSize_16u_C1R")]
        public static NppStatus LabelMarkersGetBufferSize_16u_C1R(NppiSize oSizeROI, out int nBufferSize)
        {
            return instance.LabelMarkersGetBufferSize_16u_C1R(oSizeROI, out nBufferSize);
        }

        [IntrinsicFunction("nppiLabelMarkersGetBufferSize_32u_C1R")]
        public static NppStatus LabelMarkersUFGetBufferSize_32u_C1R(NppiSize oSizeROI, out int nBufferSize)
        {
            return instance.LabelMarkersUFGetBufferSize_32u_C1R(oSizeROI, out nBufferSize);
        }

        /// <summary>
        /// checks for error
        /// </summary>
        /// <param name="status"></param>
        /// <param name="abort"></param>
        public static void ERROR_CHECK(NppStatus status, bool abort = true)
        {
            if(status != NppStatus.SUCCESS)
            {
                Console.Error.WriteLine("NPP ERROR {0}...", Enum.GetName(typeof(NppStatus), status));
                if (abort)
                    Environment.Exit(6); // abort
            }
        }

        static string GetCudaVersion()
        {
            // If not, get the version configured in app.config
            string cudaVersion = cuda.GetCudaVersion();

            // Otherwise default to latest version
            if (cudaVersion == null) cudaVersion = "80";
            cudaVersion = cudaVersion.Replace(".", ""); // Remove all dots ("7.5" will be understood "75")

            return cudaVersion;
        }
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

}
