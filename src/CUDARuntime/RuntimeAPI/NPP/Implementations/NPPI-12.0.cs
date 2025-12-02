using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Hybridizer.Runtime.CUDAImports
{
    class NPPI_12_0 : INPPI
    {
        const string dll_name = "nppif64_12.dll";

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkers_8u32u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, byte nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkers_8u32u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, byte nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkers_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkers_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkersUF_32u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, NppiNorm eNorm, IntPtr pBuffer, NppStreamContext ctx);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkersUF_32u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, NppiNorm eNorm, IntPtr pBuffer);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkersUF_16u32u_C1R(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkersUF_16u32u_C1R_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabelsGetBufferSize_32u8u_C1R(int maxLabel, out int bufferSize);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabelsGetBufferSize_32u_C1R(int nStartingNumber, out int bufferSize);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabelsGetBufferSize_16u_C1R(int maxLabel, out int bufferSize);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkersGetBufferSize_8u32u_C1R(NppiSize oSizeROI, out int bufferSize);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkersGetBufferSize_16u_C1R(NppiSize oSizeROI, out int bufferSize);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiLabelMarkersUFGetBufferSize_32u_C1R(NppiSize oSizeROI, out int bufferSize);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabels_32u8u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabels_32u8u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabels_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabels_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabels_32u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiCompressMarkerLabels_32u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiBoundSegments_8u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, byte nBorderVal);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiBoundSegments_8u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, byte nBorderVal, NppStreamContext ctx);


        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiBoundSegments_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nBorderVal);

        [DllImport(dll_name, CallingConvention = CallingConvention.Cdecl)]
        extern static NppStatus nppiBoundSegments_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nBorderVal, NppStreamContext ctx);


        public NppStatus BoundSegments_8u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, byte nBorderVal)
        {
            return nppiBoundSegments_8u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal);
        }

        public NppStatus BoundSegments_8u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, byte nBorderVal, NppStreamContext ctx)
        {
            return nppiBoundSegments_8u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal, ctx);
        }

        public NppStatus BoundSegments_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nBorderVal)
        {
            return nppiBoundSegments_16u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal);
        }
        public NppStatus BoundSegments_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nBorderVal, NppStreamContext ctx)
        {
            return nppiBoundSegments_16u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nBorderVal, ctx);
        }

        public NppStatus LabelMarkersGetBufferSize_8u32u_C1R(NppiSize oSizeROI, out int nBufferSize)
        {
            return nppiLabelMarkersGetBufferSize_8u32u_C1R(oSizeROI, out nBufferSize);
        }

        public NppStatus CompressMarkerLabelsGetBufferSize_32u_C1R(int nStartingNumber, out int nBufferSize)
        {
            return nppiCompressMarkerLabelsGetBufferSize_32u_C1R(nStartingNumber, out nBufferSize);
        }

        public NppStatus LabelMarkersGetBufferSize_16u_C1R(NppiSize oSizeROI, out int nBufferSize)
        {
            return nppiLabelMarkersGetBufferSize_16u_C1R(oSizeROI, out nBufferSize);
        }

        public NppStatus LabelMarkersUFGetBufferSize_32u_C1R(NppiSize oSizeROI, out int nBufferSize)
        {
            return nppiLabelMarkersUFGetBufferSize_32u_C1R(oSizeROI, out nBufferSize);
        }

        public NppStatus CompressMarkerLabelsGetBufferSize_32u8u_C1R(int maxLabel, out int bufferSize)
        {
            return nppiCompressMarkerLabelsGetBufferSize_32u8u_C1R(maxLabel, out bufferSize);
        }

        public NppStatus CompressMarkerLabelsGetBufferSize_16u_C1R(int maxLabel, out int bufferSize)
        {
            return nppiCompressMarkerLabelsGetBufferSize_16u_C1R(maxLabel, out bufferSize);
        }

        public NppStatus CompressMarkerLabels_32u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return nppiCompressMarkerLabels_32u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer, ctx);
        }

        public NppStatus CompressMarkerLabels_32u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer)
        {
            return nppiCompressMarkerLabels_32u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer);
        }

        public NppStatus CompressMarkerLabels_32u8u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer)
        {
            return nppiCompressMarkerLabels_32u8u_C1R(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer);
        }

        public NppStatus CompressMarkerLabels_32u8u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return nppiCompressMarkerLabels_32u8u_C1R_Ctx(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer, ctx);
        }


        public NppStatus CompressMarkerLabels_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer)
        {
            return nppiCompressMarkerLabels_16u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer);
        }
        public NppStatus CompressMarkerLabels_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, int nStartingNumber, out int pNewNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return nppiCompressMarkerLabels_16u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nStartingNumber, out pNewNumber, pBuffer, ctx);
        }


        public NppStatus LabelMarkers_8u32u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, byte nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer)
        {
            return nppiLabelMarkers_8u32u_C1R(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer);
        }
        public NppStatus LabelMarkers_8u32u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, byte nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return nppiLabelMarkers_8u32u_C1R_Ctx(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer, ctx);
        }

        public NppStatus LabelMarkers_16u_C1IR(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer)
        {
            return nppiLabelMarkers_16u_C1IR(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer);
        }

        public NppStatus LabelMarkers_16u_C1IR_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return nppiLabelMarkers_16u_C1IR_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer, ctx);
        }


        public NppStatus LabelMarkersUF_16u32u_C1R(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer)
        {
            return nppiLabelMarkersUF_16u32u_C1R(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer);
        }

        public NppStatus LabelMarkersUF_32u_C1R_Ctx(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, NppiNorm eNorm, IntPtr pBuffer, NppStreamContext ctx)
        {
            return nppiLabelMarkersUF_32u_C1R_Ctx(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, eNorm, pBuffer, ctx);
        }

        public NppStatus LabelMarkersUF_32u_C1R(IntPtr pSrc, int nSrcStep, IntPtr pDst, int nDstStep, NppiSize oSizeROI, NppiNorm eNorm, IntPtr pBuffer)
        {
            return nppiLabelMarkersUF_32u_C1R(pSrc, nSrcStep, pDst, nDstStep, oSizeROI, eNorm, pBuffer);
        }

        public NppStatus LabelMarkersUF_16u32u_C1R_Ctx(IntPtr pSrcDst, int nSrcDstStep, NppiSize oSizeROI, ushort nMinVal, NppiNorm eNorm, out int pNumber, IntPtr pBuffer, NppStreamContext ctx)
        {
            return nppiLabelMarkersUF_16u32u_C1R_Ctx(pSrcDst, nSrcDstStep, oSizeROI, nMinVal, eNorm, out pNumber, pBuffer, ctx);
        }
    }
}