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
        protected class CUDAAggregatedSizeCalculator : NativeSerializer
        {
            public long totalSize { get; set; }
            private ICollection<object> _allProcessedObjects = new HashSet<object>();
            private CudaAggregatedSerializationState state;
            internal CUDAAggregatedSizeCalculator(CudaAggregatedSerializationState state)
                : base(state)
            {
                this.state = state;
            }

            protected override void AddGhost(object param, IntPtr dev)
            {
                _allProcessedObjects.Add(param);
            }

            internal void pad64()
            {
                if (totalSize % 8 != 0)
                    totalSize += 8 - (totalSize % 8);
            }

            internal void addSize(long size)
            {
                totalSize += size;
            }

            protected override IntPtr SerializeObjectArray(object param, uint size, bool skipMemcpy = false)
            {
                DeepSerializeArray(param as Array);
                totalSize += size;
                pad64();
                return IntPtr.Zero;
            }

            /// <summary>
            /// Serializes all the objects contained in <paramref name="ap"/>
            /// </summary>
            /// <param name="ap">Array of objects</param>
            /// <returns>an array of pointers pointing to native memory</returns>
            protected override IntPtr[] DeepSerializeArray(Array ap, bool skipMemcpy = false)
            {
                foreach (object key in ap)
                {
                    if (!_allProcessedObjects.Contains(key))
                    {
                        VisitObject(key, IntPtr.Zero);
                    }
                }
                return null;
            }

            protected override IntPtr BinaryCopyArray(object param, uint size, bool skipMemcpy = false)
            {
                if (size < MAX_SIZE_FOR_AGGREGATION)
                {
                    totalSize += size;
                }
                pad64();
                return IntPtr.Zero;
            }

            protected override IntPtr SerializeCustom(ICustomMarshalled o, bool skipMemcpy = false)
            {
                totalSize += FieldTools.SizeOf(o.GetType());
                return IntPtr.Zero;
            }

            protected override IntPtr SerializeCustom(IHybCustomMarshaler marshaler, object customMarshalled, bool skipMemcpy = false)
            {
                totalSize += marshaler.SizeOf(customMarshalled);
                pad64();
                return IntPtr.Zero;
            }

            internal override void Free(IntPtr ptr)
            {
            }

            protected override FieldVisitor CreateFieldVisitor()
            {
                return new CUDAAggregatedObjectSizeCalculator(state, this);
            }

            protected override void SerializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides,
                IResidentArray residentArray)
            {

            }

            protected override IntPtr WrapArray(Array array, IntPtr res, bool skipMemcpy = false)
            {
                int rank = array.Rank;
                totalSize += rank*8;
                return IntPtr.Zero;
            }
        }
    }
}