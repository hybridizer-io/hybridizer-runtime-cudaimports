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
    #region Internal marshaling code
    #region Main Memory
    internal class MainMemorySerializationState : NativeSerializerState
    {
        /// <summary>
        /// Object to pointer on the device memory
        /// </summary>
        protected Dictionary<object, GCHandle> pinned_objects = new Dictionary<object, GCHandle>();
        protected Dictionary<object, byte[]> serialized_objects = new Dictionary<object, byte[]>();
        protected Dictionary<object, IntPtr[]> serialized_objects_arrays = new Dictionary<object, IntPtr[]>();

        internal MainMemorySerializationState(NativePtrConverter ptrConverter)
            : base(ptrConverter)
        {
            serializer = new MainMemorySerializer(this);
            deserializer = new MainMemoryDeserializer(this);
        }

        public override bool IsClean()
        {
            return base.IsClean() && pinned_objects.Count == 0;
        }

        internal override void RemoveObject(object param)
        {
            if (param == null) return;
            base.RemoveObject(param);
            GCHandle handle;
            if (pinned_objects.TryGetValue(param, out handle))
            {
                handle.Free();
                pinned_objects.Remove(param);
                serialized_objects.Remove(param);
                serialized_objects_arrays.Remove(param);
            }
        }

        #region Main Memory Serialization
        internal class MainMemorySerializer : NativeSerializer
        {
            private MainMemorySerializationState state;

            public MainMemorySerializer(MainMemorySerializationState state)
                : base(state)
            {
                this.state = state;
            }

            protected override IntPtr SerializeObjectArray(object param, uint size)
            {
                IntPtr[] numArray = DeepSerializeArray(param as Array);
                var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
                IntPtr dev = Marshal.UnsafeAddrOfPinnedArrayElement(numArray, 0);
                state.pinned_objects.Add(param, gcHandle);
                state.serialized_objects_arrays.Add(param, numArray);
                return dev;
            }

            /// <summary>
            /// Serializes all the objects contained in <paramref name="ap"/>
            /// </summary>
            /// <param name="ap">Array of objects</param>
            /// <returns>an array of pointers pointing to native memory</returns>
            protected override IntPtr[] DeepSerializeArray(Array ap)
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
                        IntPtr num2 = VisitObject(key, IntPtr.Zero);
                        numArray[num1++] = new IntPtr((long)num2);
                    }
                }
                return numArray;
            }

            protected override IntPtr BinaryCopyArray(object param, uint size)
            {
                bool blittable = true;
                GCHandle handle = new GCHandle();
                IntPtr dev = IntPtr.Zero;
                if (param != null && size > 0)
                {
                    // in case of non-blittable data -> marshal by hand
                    try
                    {
                        handle = GCHandle.Alloc(param, GCHandleType.Pinned);
                        dev = Marshal.UnsafeAddrOfPinnedArrayElement(param as Array, 0);
                    }
                    catch (ArgumentException)
                    {
                        blittable = false;
                    }

                    if (!blittable)
                    {
                        // in case of non-blittable data -> marshal by hand
                        byte[] data = SerializeNonBlittableArray(param as Array);
                        handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                        dev = Marshal.UnsafeAddrOfPinnedArrayElement(data as Array, 0);
                    }

                    // We just need to pin the memory to be able to use it in native code
                    state.pinned_objects.Add(param, handle);
                }
                return dev;
            }

            protected override IntPtr SerializeCustom(ICustomMarshalled customMarshalled)
            {
                MainMemoryObjectSerializer os = new MainMemoryObjectSerializer(state, state.serializer);
                os.MarshalICustomMarshalled(customMarshalled);
                IntPtr result = os.AllocateObject(customMarshalled);
                os.CopyObject(customMarshalled, result);
                return result;
            }

            internal override void Free(IntPtr ptr)
            {
                // Do nothing
                Logger.WriteLine("Free is not implemented, ignoring");
            }

            protected override FieldVisitor CreateFieldVisitor()
            {
                return new MainMemoryObjectSerializer(state, this);
            }

            protected override void SerializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides, IResidentArray residentArray)
            {
                residentArray.RefreshHost();
            }

        }

        internal class MainMemoryObjectSerializer : NativeObjectSerializer
        {
            private MainMemorySerializationState state;

            internal MainMemoryObjectSerializer(MainMemorySerializationState state, AbstractObjectVisiter ser)
                : base(state, ser)
            {
                this.state = state;
            }

            internal override IntPtr AllocateObject(object param)
            {
                IntPtr dev = IntPtr.Zero;
                try
                {
                    byte[] numArray = ms.ToArray();
                    var gcHandle = GCHandle.Alloc(numArray, GCHandleType.Pinned);
                    if (state.pinned_objects.ContainsKey(param))
                        throw new ApplicationException();       
                    state.pinned_objects.Add(param, gcHandle);
                    if (state.serialized_objects.ContainsKey(param))
                        throw new ApplicationException();
                    state.serialized_objects.Add(param, numArray);
                    dev = Marshal.UnsafeAddrOfPinnedArrayElement(numArray, 0);
                    if (state.ghosts.ContainsKey(param))
                        throw new ApplicationException();
                    state.ghosts.Add(param, dev);
                }
                catch (Exception)
                {
                    throw;
                }

                return dev;
            }

            internal override void CopyObject(object param, IntPtr dev)
            {
            }
        }

        #endregion

        #region Main Memory Deserialization
        private class MainMemoryDeserializer : NativeDeserializer
        {
            private MainMemorySerializationState state;

            public MainMemoryDeserializer(MainMemorySerializationState state)
                : base(state)
            {
                this.state = state;
            }

            protected override void DeserializeRawData(byte[] data, IntPtr da, long size)
            {
                if(size > int.MaxValue) {
                    Console.WriteLine("WARNING : cannot yet deserialize more than 2GB of raw data");
                }
                Marshal.Copy(da, data, 0, (int)size);
            }

            protected override void DeserializeArray(object param, IntPtr da, Type type)
            {
                if (type.GetElementType().IsPrimitive || type.GetElementType().IsValueType)
                {
                    // Nothing to do
                    try
                    {
                        GCHandle gcHandle = GCHandle.Alloc(param, GCHandleType.Pinned);
                        gcHandle.Free();
                    }
                    catch (ArgumentException)
                    {
                        byte[] buffer;
                        if (serState.nonBlittableArrays.TryGetValue(param as Array, out buffer))
                        {
                            var des = new NativeSerializer.NativeNonBlittableStructDeserializer(serState, this);
                            var fv = CreateFieldVisitor();
                            des.deserialize(param, type, fv, buffer);
                        }
                    }
                }
                else
                {
                    IntPtr[] handles = state.serialized_objects_arrays[param];
                    DeserializeObjectArray(handles, param as Array);
                }

                // Just unpin the array
                state.RemoveObject(param);
                GCHandle handle;
                if (state.pinned_objects.TryGetValue(param, out handle))
                {
                    handle.Free();
                    state.pinned_objects.Remove(param);
                    state.serialized_objects.Remove(param);
                }
#if DEBUG_ALLOC
                Logger.WriteLine("Object {0} deserialized from {1:X}", param, da.ToInt64());
#endif
            }

            internal override void Free(IntPtr ptr)
            {
                // Dont do anything
            }
            protected override FieldVisitor CreateFieldVisitor()
            {
                return new MainMemoryObjectDeserializer(state, this);
            }

            protected override void DeserializeResidentArray(Type type, Dictionary<FieldInfo, byte[]> overrides, IResidentArray residentArray)
            {
                // Do nothing
            }
        }

        protected class MainMemoryObjectDeserializer : NativeObjectDeserializer
        {
            private MainMemorySerializationState state;


            internal MainMemoryObjectDeserializer(MainMemorySerializationState state, AbstractObjectVisiter deser)
                : base(state, deser)
            {
                this.state = state;
            }

            internal override void start(object param, Type type, IntPtr da)
            {
                var buffer = state.serialized_objects[param];

                long expected = serState.nativePtrConverter.Convert(type).ToInt64();
                var ms = new MemoryStream(buffer);
                br = new BinaryReader(ms);
                long typeId = br.ReadInt64();
                if (typeId != expected)
                {
                    throw new ApplicationException(String.Format("Incompatible types expecting {0} and found {1}", expected, typeId));
                }
            }
        }
        #endregion
    }
    #endregion
    #endregion
}