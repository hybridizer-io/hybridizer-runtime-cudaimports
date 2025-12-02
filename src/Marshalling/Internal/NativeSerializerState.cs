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
    internal abstract partial class NativeSerializerState
    {
        #region state fields

        internal readonly IDictionary<object, IntPtr> ghosts = new Dictionary<object, IntPtr>();
        internal readonly IDictionary<Object, GCHandle> directlyMappedArrayHandles = new Dictionary<Object, GCHandle>();
        internal readonly IDictionary<Object, IntPtr> directlyMappedArrayPtr = new Dictionary<Object, IntPtr>();
        internal readonly IDictionary<Object, byte[]> nonBlittableArrays = new Dictionary<Object, byte[]>();

        internal readonly NativePtrConverter nativePtrConverter;

        protected AbstractObjectVisiter serializer;
        protected AbstractObjectVisiter deserializer;

        public bool cleanUpNativeData
        {
            get;
            set;
        }

        #endregion

        #region Common methods

        public virtual bool IsClean()
        {
            return ghosts.Count == 0;
        }

        internal int NbElements
        {
            get { return ghosts.Count; }
        }

        internal NativeSerializerState(NativePtrConverter ptrConverter)
        {
            nativePtrConverter = ptrConverter;
            cleanUpNativeData = true;
            this.CreatingThreadId = Thread.CurrentThread.Name;
        }

        internal virtual IntPtr MarshalManagedToNative(object param)
        {
            return serializer.InitialVisit(param);
        }

        internal void UpdateManagedData(object param)
        {
            try
            {
                deserializer.InitialVisit(param);
            }
            catch (Exception ex)
            {
                Logger.Out.WriteLine(ex);
                throw;
            }
        }

        internal virtual void RemoveNative(IntPtr native)
        {
            // Console.WriteLine("Remove native thread:{0} marshaller:{1}", Thread.CurrentThread.Name, this.CreatingThreadId);
            
            try
            {
                object managed = null;
                foreach (KeyValuePair<object, IntPtr> entry in ghosts)
                {
                    if (entry.Value.Equals(native))
                    {
                        managed = entry.Key;
                        break;
                    }
                }
                if (managed == null)
                {
                    foreach (KeyValuePair<Object, IntPtr> entry in directlyMappedArrayPtr)
                    {
                        if (entry.Value.Equals(native))
                        {
                            managed = entry.Key;
                            break;
                        }
                    }
                }
                if (managed == null) return;

                deserializer.InitialVisit(managed);
            }
            catch (Exception ex)
            {
                Logger.Out.WriteLine(ex);
                throw ex;
            }
        }

        public string CreatingThreadId { get; set; }

        internal virtual void Free()
        {
            foreach (var p in ghosts.Keys.ToArray())
            {
                RemoveObject(p);
            }
            ghosts.Clear();
        }

        internal IntPtr MarshalNonPrimitiveParameter(Type t, object param)
        {
            try
            {
                if (!IsTypeSupported(t))
                    throw new NotSupportedException(string.Format("Type {0} is not supported for marshalling", t.FullName));
                if (param == null)
                    return IntPtr.Zero;
                return MarshalManagedToNative(param);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private static bool IsTypeSupported(Type t)
        {
            return !t.IsPrimitive && (t.IsValueType || t.IsArray || t.IsClass || t.IsInterface || typeof(ICustomMarshalled).IsAssignableFrom(t));
        }

        internal void Dispose()
        {
            foreach (object key in ghosts.Keys)
            {
                IntPtr da = ghosts[key];
                UpdateManagedData(key);
                RemoveObject(key);
            }
        }

        internal bool RegisterDLL(string filename)
        {
            return nativePtrConverter.RegisterDLL(filename);
        }

        internal virtual void RemoveObject(object p)
        {
            if (p != null) ghosts.Remove(p);
        }

        internal static void WriteHybArrayDimensions(Array array, int rank, BinaryWriter bw)
        {
            if (array != null)
            {
                // Write dimensions
                for (int i = 0; i < rank; ++i)
                    bw.Write((int)array.GetLength(i));

                // Write lower bounds
                for (int i = 0; i < rank; ++i)
                    bw.Write((int)array.GetLowerBound(i));
            } else {
                for (int i = 0; i < rank; ++i)
                    bw.Write((int)0);
                for (int i = 0; i < rank; ++i)
                    bw.Write((int)0);
            }
        }

        #endregion

        #region Binary utilities
        protected static BinaryReader Unpad64(BinaryReader br, Boolean fail = true)
        {
            return Unpad(br, 8, fail);
        }

        protected static BinaryWriter Pad64(BinaryWriter bw)
        {
            return Pad(bw, 8);
        }

        protected static BinaryReader Unpad32(BinaryReader br, Boolean fail = true)
        {
            return Unpad(br, 4, fail);
        }

        protected static BinaryWriter Pad32(BinaryWriter bw)
        {
            return Pad(bw, 4);
        }

        private static BinaryReader Unpad(BinaryReader br, int size, Boolean fail = true)
        {
            if ((br.BaseStream.Position % size) != 0)
            {
                if (fail)
                {
                    throw new ApplicationException("INTERNAL ERROR : padding should be taken care of in fields ordering");
                }
                else
                {
                    br.ReadBytes((int)(size - (br.BaseStream.Position % size)));
                }
            }
            return br;
        }

        private static BinaryWriter Pad(BinaryWriter bw, int size)
        {
            var p = bw.BaseStream.Position;
            var m = p % size;
            if (m != 0)
                bw.BaseStream.Position = p + size - m;
            return bw;
        }


        protected static uint SizeOfArrayElt(Type t)
        {
            if (t.IsValueType)
                return (uint)FieldTools.SizeOf(t);

            return (uint)IntPtr.Size;
        }

        protected static uint GetElementCount(Array a)
        {
            int rank = a.Rank;
            long num = 1L;
            for (int dimension = 0; dimension < rank; ++dimension)
                num *= a.GetLength(dimension);
            return (uint)num;
        }

        public static FieldTools.FieldDeclaration[] OrderedFields(Type t)
        {
            return FieldTools.OrderedFields(t);
        }
        #endregion

        internal AbstractNativeMarshaler Marshaler { get; set; }
    }
}