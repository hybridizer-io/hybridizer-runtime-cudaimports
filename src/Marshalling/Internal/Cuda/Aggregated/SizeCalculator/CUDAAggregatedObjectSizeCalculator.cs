/* (c) ALTIMESH 2018 -- all rights reserved */
//#define DEBUG
//#define DEBUG_ALLOC
//#define DEBUG_MEMCPY

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security;
using System.Text;
using System.Threading;
using NamingTools = Altimesh.Hybridizer.Runtime.NamingTools;

namespace Hybridizer.Runtime.CUDAImports
{
    internal partial class CudaAggregatedSerializationState : CudaAbstractSerializationState
    {
        protected class CUDAAggregatedObjectSizeCalculator : NativeObjectSerializer
        {
            private CUDAAggregatedSizeCalculator s;
            internal CUDAAggregatedObjectSizeCalculator(CudaAggregatedSerializationState state, CUDAAggregatedSizeCalculator ser)
                : base(state, ser, false)
            {
                s = ser;
            }


            protected override void HandleVTable(long o)
            {
                s.addSize(8);
            }

            protected override void HandlePaddingByte(long count)
            {
                s.addSize(count);
            }

            protected override void HandleOverride(byte[] data)
            {
                s.addSize(data.Length);
            }

            protected override void HandlePrimitive(FieldTools.FieldDeclaration key, object param)
            {
                s.addSize(key.ByteCount);
                switch (key.TypeEnum)
                {
                    case FieldTools.FieldTypeEnum.BOOL:
                        //s.addSize(sizeof(int)); // Force bool to be serialized as ints
                        s.addSize(key.ByteCount);
                        break;
                    case FieldTools.FieldTypeEnum.BYTE:
                    case FieldTools.FieldTypeEnum.SHORT:
                    case FieldTools.FieldTypeEnum.INT:
                    case FieldTools.FieldTypeEnum.UINT:
                    case FieldTools.FieldTypeEnum.FLOAT:
                        s.addSize(key.ByteCount);
                        break;
                    case FieldTools.FieldTypeEnum.ULONG:
                    case FieldTools.FieldTypeEnum.DOUBLE:
                    case FieldTools.FieldTypeEnum.LONG:
                        s.pad64();
                        s.addSize(key.ByteCount);
                        break;
                    default:
                        if (key.FieldType != typeof(IntPtr))
                            throw new NotImplementedException();
                        // always write 8 bytes
                        HandlePtr(key, null, (IntPtr)key.Info.GetValue(param));
                        break;
                }
            }

            protected override void HandlePtr(FieldTools.FieldDeclaration key, object param, IntPtr p, bool isResidentArray = false)
            {
                if (key.FieldType.IsArray && CudaRuntimeProperties.UseHybridArrays)
                {
                    int rank = 1;
                    if (!isResidentArray)
                    {
                        Array ar = key.Info.GetValue(param) as Array;
                        rank = ar != null ? ar.Rank : 1;
                    }
                    
                    s.addSize(8 * rank);
                }
                s.addSize(8);
            }

            protected override void HandleObject(FieldTools.FieldDeclaration key, object param, IntPtr p)
            {
                IntPtr ptr = serializer.VisitObject(key.Info.GetValue(param), IntPtr.Zero);
                HandlePtr(key, param, ptr);
            }

            protected override void HandleValueType(FieldTools.FieldDeclaration key, object param,
                Dictionary<FieldInfo, byte[]> overrides)
            {
                object o = key.Info.GetValue(param);
                if (key.FieldType.IsEnum)
                {
                    if (Enum.GetUnderlyingType(key.FieldType) == typeof(int))
                        s.addSize(sizeof(int));
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(uint))
                        s.addSize(sizeof(uint));
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(long))
                        s.addSize(sizeof(long));
                    else if (Enum.GetUnderlyingType(key.FieldType) == typeof(ulong))
                        s.addSize(sizeof(ulong));
                    else
                        throw new ApplicationException("enum underlying type not implemented");
                }
                else if (o is ICustomMarshalled)
                {
                    serializer.VisitObject(o, IntPtr.Zero);
                }
                else
                {
                    VisitFields(o, serState.nativePtrConverter.GetTypeInfo(key.FieldType), overrides);
                }
            }

            protected override void HandleEndOfObject(object param)
            {
                s.pad64();
            }

            internal override IntPtr AllocateObject(object param)
            {
#if DEBUG
                Console.WriteLine("End of " + param.GetType().Name + ": " + s.totalSize);
#endif
                return IntPtr.Zero;
            }

            internal override void CopyObject(object param, IntPtr dev)
            {
            }
        }
    }
}