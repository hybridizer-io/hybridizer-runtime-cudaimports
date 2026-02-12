/* (c) ALTIMESH 2018 -- all rights reserved */
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
        protected class CUDAAggregatedSerializer : NativeSerializer
        {
            private CudaAggregatedSerializationState state;
            /// <summary>
            /// Associate each initial object to a global allocator
            /// </summary>

            internal CUDAAggregatedSerializer(CudaAggregatedSerializationState state)
                : base(state)
            {
                this.state = state;
            }

            public override IntPtr InitialVisit(object param, bool skipMemcpy = false)
            {
                if (param == null)
                    return IntPtr.Zero;

				if (serState.ghosts.ContainsKey(param))
					return serState.ghosts[param];

                state.InitializeStream();

                // Compute the size to be allocated on the device
                CUDAAggregatedSizeCalculator sizeCalculator = new CUDAAggregatedSizeCalculator(state);
                sizeCalculator.InitialVisit(param);
                // Allocate on the device
                var allocator = state.createAllocator(param, sizeCalculator.totalSize);
                //_allAllocators[new WeakReference(param)] = allocator;
                state._currentAllocator = allocator;
                IntPtr res = base.InitialVisit(param, skipMemcpy);
                if (!skipMemcpy)
                    allocator.copyBuffer();
                state.StreamSynchronize();
                allocator.freeBuffer();
                return res;
            }

            protected override IntPtr SerializeObjectArray(object param, uint size, bool skipMemcpy = false)
            {
                IntPtr[] numArray = DeepSerializeArray(param as Array, skipMemcpy);

                IntPtr dev = state._currentAllocator.allocate(param, size);
                if (!skipMemcpy)
                    state._currentAllocator.cpy(dev, numArray, size);
                return dev;
            }

            /// <summary>
            /// Serializes all the objects contained in <paramref name="ap"/>
            /// </summary>
            /// <param name="ap">Array of objects</param>
            /// <returns>an array of pointers pointing to native memory</returns>
            protected override IntPtr[] DeepSerializeArray(Array ap, bool skipMemcpy = false)
            {
                var numArray = new IntPtr[GetElementCount(ap)];
                int num1 = 0;
                foreach (object key in ap)
                {
                    if (key == null)
                    {
                        numArray[num1++] = IntPtr.Zero;
                    }
                    else if (serState.ghosts.ContainsKey(key))
                    {
                        numArray[num1++] = serState.ghosts[key];
                    }
                    else
                    {
                        IntPtr num2 = VisitObject(key, IntPtr.Zero, skipMemcpy);
                        numArray[num1++] = new IntPtr((long)num2);
                    }
                }
                return numArray;
            }

            protected override IntPtr BinaryCopyArray(object param, uint size, bool skipMemcpy = false)
            {
                IntPtr dev = state._currentAllocator.allocate(param, size);
                if (!skipMemcpy)
                    state._currentAllocator.cpy(dev, param as Array, size);
                return dev;
            }

            protected override IntPtr SerializeCustom(ICustomMarshalled customMarshalled, bool skipMemcpy = false)
            {
                throw new NotImplementedException();
            }

            internal override void Free(IntPtr ptr)
            {
                state.CudaFree(ptr);
            }

            protected override FieldVisitor CreateFieldVisitor()
            {
                return new CUDAAggregatedObjectSerializer(state, this);
            }

            protected override void SerializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides,
                IResidentArray residentArray)
            {
                if (residentArray.Status == ResidentArrayStatus.DeviceNeedsRefresh)
                    residentArray.RefreshDevice();
                residentArray.Status = ResidentArrayStatus.HostNeedsRefresh;
                IntPtr devicePointer = residentArray.DevicePointer;
                var memoryStream = new MemoryStream();
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(devicePointer.ToInt64());
                }

                bool found = false;
                foreach (FieldTools.FieldDeclaration key in OrderedFields(type))
                {
                    if (key.Info == null) continue;
                    if (Attribute.IsDefined(key.Info, typeof(ResidentArrayHostAttribute)))
                    {
                        overrides.Add(key.Info, memoryStream.ToArray());
                        found = true;
                    }
                }

                if (!found)
                    Logger.WriteLine(String.Format("Resident array <{0}> does not declare a ResidentArrayHost entry", type.FullName));
            }
        }
    }
}