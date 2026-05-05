/* (c) ALTIMESH 2019 -- all rights reserved */
using Altimesh.Hybridizer.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// marshalling for array of strinfgs
    /// </summary>
    public class StringArrayMarshal : IDisposable
    {
        IntPtr[] ar;
        GCHandle arHandle;
        byte[][] charArrays;
        GCHandle[] handles;

        /// <summary>
        /// get intptr
        /// </summary>
        public IntPtr Ptr
        {
            get { if (ar == null) return IntPtr.Zero; else return Marshal.UnsafeAddrOfPinnedArrayElement(ar, 0); }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="data"></param>
        public StringArrayMarshal(string[] data)
        {
            // Treat null and zero-length identically: native callers expect a
            // null pointer when there are no entries.
            if (data == null || data.Length == 0)
            {
                ar = null;
                charArrays = null;
                handles = null;
                return;
            }

            charArrays = new byte[data.Length][];
            handles = new GCHandle[data.Length];
            ar = new IntPtr[data.Length];
            for (int k = 0; k < data.Length; ++k)
            {
                if (data[k] == null)
                {
                    // Null entries map to a null slot in the IntPtr array.
                    charArrays[k] = null;
                    ar[k] = IntPtr.Zero;
                    continue;
                }
                var bytes = ASCIIEncoding.ASCII.GetBytes(data[k]).ToList();
                bytes.Add(0);
                charArrays[k] = bytes.ToArray();
                handles[k] = GCHandle.Alloc(charArrays[k], GCHandleType.Pinned);
                ar[k] = Marshal.UnsafeAddrOfPinnedArrayElement(charArrays[k], 0);
            }
            arHandle = GCHandle.Alloc(ar, GCHandleType.Pinned);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (ar != null)
                {
                    if (arHandle.IsAllocated) arHandle.Free();
                    if (handles != null)
                    {
                        foreach (GCHandle handle in handles)
                            if (handle.IsAllocated) handle.Free();
                    }
                }
                ar = null;
                charArrays = null;
                handles = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// destructor
        /// </summary>
        ~StringArrayMarshal() {
          Dispose(false);
        }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
