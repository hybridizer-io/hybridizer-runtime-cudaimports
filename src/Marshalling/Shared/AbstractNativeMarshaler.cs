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
    /// <summary>
    /// Base class for NativeMarshaler
    /// </summary>
    public abstract class AbstractNativeMarshaler : ICustomMarshaler
    {
        #region private fields

        internal readonly NativeSerializerState state;
        /// <summary>
        /// cleans up native data
        /// </summary>
        public bool cleanUpNativeData
        {
            get { return state.cleanUpNativeData; }
            set { state.cleanUpNativeData = value; }
        }

        /// <summary>
        /// is cleanup native data?
        /// </summary>
        /// <returns></returns>
        public bool IsCleanUpNativeData()
        {
            return cleanUpNativeData;
        }

        #endregion

        internal AbstractNativeMarshaler(NativeSerializerState state) 
        {
            this.state = state;
            this.state.Marshaler = this;
        }

        internal AbstractNativeMarshaler()
        {
        }

        /// <summary></summary>
        public string CreatingThreadId { get { return state.CreatingThreadId; } }

        #region ICustomMarshaler implementation
        /// <summary>
        /// Marshals Native To Managed
        /// </summary>
        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        [StructLayout(LayoutKind.Explicit, Size = 8)]
        struct PrimitiveTypesUnion
        {
            [FieldOffset(0)]
            public char _char;
            [FieldOffset(0)]
            public bool _bool;
            [FieldOffset(0)]
            public byte _byte;
            [FieldOffset(0)]
            public sbyte _sbyte;
            [FieldOffset(0)]
            public short _short;
            [FieldOffset(0)]
            public ushort _ushort;
            [FieldOffset(0)]
            public int _int;
            [FieldOffset(0)]
            public uint _uint;
            [FieldOffset(0)]
            public long _long;
            [FieldOffset(0)]
            public ulong _ulong;
            [FieldOffset(0)]
            public float _float;
            [FieldOffset(0)]
            public double _double;
            [FieldOffset(0)]
            public UIntPtr _uintPtr;

            [FieldOffset(0)]
            public IntPtr _intPtr;

            /// <summary>
            /// converts a primitive type to intrptr
            /// </summary>
            /// <param name="o"></param>
            /// <returns>zero if type is not primitive</returns>
            public static PrimitiveTypesUnion Build(object o)
            {
                PrimitiveTypesUnion result = new PrimitiveTypesUnion { _intPtr = IntPtr.Zero };
                   Type t = o.GetType();
                if(t == typeof(bool))
                    result._bool = (bool)o;
                else if (t == typeof(char))
                    result._char = (char)o;
                else if (t == typeof(byte))
                    result._byte = (byte)o;
                else if (t == typeof(sbyte))
                    result._sbyte = (sbyte)o;
                else if (t == typeof(short))
                    result._short = (short)o;
                else if (t == typeof(ushort))
                    result._ushort = (ushort)o;
                else if (t == typeof(int))
                    result._int = (int)o;
                else if (t == typeof(long))
                    result._long = (long)o;
                else if (t == typeof(ulong))
                    result._ulong = (ulong)o;
                else if (t == typeof(IntPtr))
                    result._intPtr = (IntPtr)o;
                else if (t == typeof(UIntPtr))
                    result._uintPtr = (UIntPtr)o;
                else if (t == typeof(float))
                    result._float = (float)o;
                else if (t == typeof(double))
                    result._double = (double)o;
                return result;
            }
        }

        /// <summary>
        /// Marshals Managed to Native
        /// </summary>
        public virtual IntPtr MarshalManagedToNative(object managedObj)
        {
            return MarshalManagedToNative(managedObj, false);
        }

        /// <summary>
        /// Marshals Managed to Native, optionally skipping memcpy (allocation only)
        /// </summary>
        public virtual IntPtr MarshalManagedToNative(object managedObj, bool skipMemcpy)
        {
            if (managedObj == null)
                return IntPtr.Zero;
            Stopwatch sw = null;
            if (Debugger.IsAttached)
            {
                sw = new Stopwatch();
                sw.Start();
            }

            if(managedObj.GetType().IsPrimitive)
            {
                PrimitiveTypesUnion union = PrimitiveTypesUnion.Build(managedObj);
                return union._intPtr;
            }

            IntPtr nativePtr = state.MarshalNonPrimitiveParameter(managedObj.GetType(), managedObj, skipMemcpy);
            if (Debugger.IsAttached)
            {
                sw.Stop();
                Logger.WriteLine("Marshalling took {0} ms, dst {1:X}", sw.ElapsedMilliseconds, nativePtr.ToInt64());
            }
            return nativePtr;
        }

        /// <summary>
        /// cleanup native data
        /// </summary>
        public void CleanUpNativeData(IntPtr pNativeData)
        {
            CleanUpNativeData(pNativeData, false);
        }

        /// <summary>
        /// cleanup native data, optionally skipping memcpy (free only)
        /// </summary>
        public void CleanUpNativeData(IntPtr pNativeData, bool skipMemcpy)
        {
            if (!cleanUpNativeData) return;
            state.RemoveNative(pNativeData, skipMemcpy);
        }

        /// <summary>
        /// cleanup managed data
        /// </summary>
        public void CleanUpManagedData(object managedObj)
        {
            CleanUpManagedData(managedObj, false);
        }

        /// <summary>
        /// cleanup managed data, optionally skipping memcpy (free only)
        /// </summary>
        public void CleanUpManagedData(object managedObj, bool skipMemcpy)
        {
            state.UpdateManagedData(managedObj, skipMemcpy);
        }

        /// <summary>
        /// Get native data size
        /// </summary>
        /// <returns></returns>
        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Returns the number of objects that have been marshaled
        /// </summary>
        public int NbelementsInGhost
        {
            get
            {
                return state.NbElements;
            }
        }

        /// <summary>
        /// Forces freeing all native allocated memory
        /// </summary>
        public void Free()
        {
            state.Free();
        }

        /// <summary>
        /// Register a dll that contains types to be used in marshaling
        /// </summary>
        /// <param name="filename"></param>
        public virtual bool RegisterDLL(string filename)
        {
            return state.RegisterDLL(filename);
        }

        /// <summary>
        /// Is state clean?
        /// </summary>
        public bool IsClean()
        {
            return state.IsClean();
        }

#pragma warning disable 1591
        protected void AutoRegister(Assembly asm, Type marshallerType)
        {
            IList<string> toBeRegistered = new List<string>();
            Type[] asmTypes = null;
            try
            {
                asmTypes = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }
            if (asmTypes == null) return;
            foreach (Type t in asmTypes)
            {
                if (t == null) continue;
                foreach (MethodInfo mi in t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    try
                    {
                        object[] att = mi.GetCustomAttributes(typeof(DllImportAttribute), true);
                        if (att.Length == 0) continue;
                        DllImportAttribute dia = att[0] as DllImportAttribute;
                        if (dia == null) continue; // should never happend...
                        // we have a DLL import attribute => check if method uses the marshaller
                        foreach (ParameterInfo pi in mi.GetParameters())
                        {
                            object[] patt = pi.GetCustomAttributes(typeof(MarshalAsAttribute), true);
                            if (patt.Length == 0) continue;
                            MarshalAsAttribute maa = patt[0] as MarshalAsAttribute;
                            if (maa == null) continue;
                            if (maa.MarshalTypeRef == marshallerType)
                            {
                                // using this parameter
                                if (!toBeRegistered.Contains(dia.Value)) toBeRegistered.Add(dia.Value);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // don't fail that way on auto-register
                        continue;
                    }
                }
            }
            foreach (string asmName in toBeRegistered)
            {
                try
                {
                    RegisterDLL(asmName);
                }
                catch (Exception)
                {
                    // maybe, autoregister is used and some DllImports in the assembly are not used/available
                    continue;
                }
                
            }
        }

        public HandleDelegate GetHandleDelegate(Delegate deleg)
        {
            Delegate del = deleg as Delegate;
            HandleDelegate res = new HandleDelegate();
            res.Marshalled = MarshalManagedToNative(del.Target);
            res.FuncPtr = state.nativePtrConverter.GetFunctionPointer(del.Method);
			res._isStaticFunc = false;
            return res;
        }
#pragma warning restore 1591

        /// <summary>
        /// copies memory to symbol
        /// </summary>
        public void CopyToSymbol(string targetSymbol, IntPtr src, IntPtr size, IntPtr offset, IntPtr module)
        {
            state.nativePtrConverter.CopyToSymbol(targetSymbol, src, size, offset, module);
        }

        /// <summary>
        /// current flavor
        /// </summary>
        public HybridizerFlavor Flavor { get { return state.nativePtrConverter.Flavor ; }}

        /// <summary>
        /// sets a custom marshaler to a type
        /// </summary>
        public void SetCustomMarshaler(Type type, IHybCustomMarshaler cm)
        {
            state.nativePtrConverter.SetCustomMarshaler(type, cm);
        }
    }
}