/* (c) ALTIMESH 2019 -- all rights reserved */
using Altimesh.Hybridizer.Runtime;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// jitted cuda module
    /// </summary>
    public class JittedModule
    {
        private IntPtr _cubin;
        // ReSharper disable once NotAccessedField.Local
        private size_t _cubinsize;

        private CUmodule _module = new CUmodule(IntPtr.Zero);

        /// <summary>
        /// module
        /// </summary>
        public CUmodule Module
        {
            // ReSharper disable once ArrangeAccessorOwnerBody
            get { return _module; }
        } 
        /// <summary>
        /// load from cubin
        /// </summary>
        /// <param name="cubin"></param>
        /// <param name="cubinSize"></param>
        /// <returns></returns>
        public CUresult Load(IntPtr cubin, size_t cubinSize)
        {
            _cubin = cubin;
            _cubinsize = cubinSize;
            return driver.ModuleLoadData(out _module, _cubin);
        }

        #region get kernel 
        /// <summary>
        /// get kernel from name
        /// </summary>
        /// <param name="kernelname"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public CUresult GetFunctionPointer(string kernelname, out IntPtr result)
        {
            IntPtr dFuncptrvar;
            size_t sz;
            var cures = driver.ModuleGetGlobal(out dFuncptrvar, out sz, _module, "d_" + kernelname);
            if (cures != CUresult.CUDA_SUCCESS)
            {
                Console.Error.WriteLine("{0}", cures);
                result = IntPtr.Zero;
                return cures;
            }
            IntPtr[] dKernel = new IntPtr[1];
            GCHandle gch = GCHandle.Alloc(dKernel, GCHandleType.Pinned);
            try
            {
                cures = driver.MemcpyDtoH(Marshal.UnsafeAddrOfPinnedArrayElement(dKernel, 0), dFuncptrvar, sz);
                if (cures != CUresult.CUDA_SUCCESS)
                {
                    Console.Error.WriteLine("{0}", cures);
                    result = IntPtr.Zero;
                    return cures;
                }
                result = dKernel[0];
                return CUresult.CUDA_SUCCESS;
            }
            finally
            {
                gch.Free();
            }
        }

        /// <summary>
        /// get entry point from name (legacy API: discards CUresult)
        /// </summary>
        /// <param name="kernelname"></param>
        /// <returns></returns>
        public CUfunction GetEntryPoint(string kernelname)
        {
            TryGetEntryPoint(kernelname, out CUfunction func);
            return func;
        }

        /// <summary>
        /// get entry point from name and return the underlying CUresult so
        /// callers can react to a missing symbol or unloaded module.
        /// </summary>
        /// <param name="kernelname"></param>
        /// <param name="func">resolved kernel handle, or default if the call failed</param>
        /// <returns>CUresult from the underlying cuModuleGetFunction call</returns>
        public CUresult TryGetEntryPoint(string kernelname, out CUfunction func)
        {
            return driver.ModuleGetFunction(out func, _module, kernelname);
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint(Action action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1>(Action<T1> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2>(Action<T1, T2> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }

        /// <summary>
        /// get cuda entrypoint corresponding to C# method
        /// </summary>
        public CUfunction GetEntryPoint<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        {
            return GetEntryPoint(NamingTools.GetEntryPointSymbol(action));
        }
        #endregion
         
        #region launch kernel

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel(Action entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1>(Action<T1> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2>(Action<T1, T2> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3>(Action<T1, T2, T3> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3, T4>(Action<T1, T2, T3, T4> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        /// <summary>
        /// launch kernel (driver API)
        /// </summary>
        public CUresult LaunchKernel<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> entryPoint, dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, params object[] args)
        {
            CUfunction func = GetEntryPoint(entryPoint);
            return BaseLaunchKernel(gridDim, blockDim, sharedMem, stream, args, func);
        }

        static readonly Dictionary<Type, int> ParameterAlignment = new Dictionary<Type, int>
        {
            { typeof(char), 2 },
            { typeof(byte), 1 },
            { typeof(sbyte), 1 },
            { typeof(short), 2 },
            { typeof(ushort), 2 },
            { typeof(bool), 4 },
            { typeof(int), 4 },
            { typeof(uint), 4 },
            { typeof(long), 8 },
            { typeof(ulong), 8 },
            { typeof(float), 4 },
            { typeof(double), 8 },
            { typeof(IntPtr), 8 },
            { typeof(UIntPtr), 8 }
        };

        private static int __alignof(object t)
        {
            return ParameterAlignment[t.GetType()];
        }

        private static void ALIGN_UP(ref int offset, int alignment)
        {
            offset = ((offset) + (alignment) - 1) & ~((alignment) - 1);
        }

        private static unsafe void ADD_TO_PARAM_BUFFER(byte* ptr, ref int bufferSize, object o)
        {
            ALIGN_UP(ref bufferSize, __alignof(o));
            var t = o.GetType();
            if(t == typeof(char))
            {
                *((char*)(ptr + bufferSize)) = ((char)o);
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(sbyte))
            {
                *((sbyte*)(ptr + bufferSize)) = ((sbyte)o);
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(byte))
            {
                *(ptr + bufferSize) = ((byte)o);
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(bool))
            {
                *(int*)(ptr + bufferSize) = (bool)o ? 1 : 0;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(short))
            {
                *(short*)(ptr + bufferSize) = (short)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(ushort))
            {
                *(ushort*)(ptr + bufferSize) = (ushort)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(int))
            {
                *(int*)(ptr + bufferSize) = (int)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(uint))
            {
                *(uint*)(ptr + bufferSize) = (uint)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(long))
            {
                *(long*)(ptr + bufferSize) = (long)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(ulong))
            {
                *(ulong*)(ptr + bufferSize) = (ulong)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(float))
            {
                *(float*)(ptr + bufferSize) = (float)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(double))
            {
                *(double*)(ptr + bufferSize) = (double)o;
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(IntPtr))
            {
                *(long*)(ptr + bufferSize) = ((IntPtr)o).ToInt64();
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else if (t == typeof(UIntPtr))
            {
                *(ulong*)(ptr + bufferSize) = ((UIntPtr)o).ToUInt64();
                bufferSize += ParameterAlignment[t]; // size == alignment
            }
            else
            {
                throw new ArgumentException("argument passed is not primitive :: " + o.GetType());
            }
        }

        private static unsafe byte[] Convert(object[] o, out int bufferSize)
        {
            byte[] buffer = new byte[1024]; // max arg size
            bufferSize = 0;
            fixed(byte* ptr = buffer)
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < o.Length; ++i)
                {
                    ADD_TO_PARAM_BUFFER(ptr, ref bufferSize, o[i]);
                }
            }

            return buffer;
        }

        /// <summary>
        /// Launch a kernel (driver API)
        /// </summary>
        /// <param name="gridDim"></param>
        /// <param name="blockDim"></param>
        /// <param name="sharedMem"></param>
        /// <param name="stream"></param>
        /// <param name="args">all arguments must be IntPtr or primitive types</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static CUresult BaseLaunchKernel(dim3 gridDim, dim3 blockDim, int sharedMem, CUstream stream, object[] args, CUfunction func)
        {
            int[] runtime = new int[256];
            var runtimeHandle = GCHandle.Alloc(runtime, GCHandleType.Pinned);
            IntPtr dRuntime = runtimeHandle.AddrOfPinnedObject();
            List<object> completeArgList = new List<object> { dRuntime };
            completeArgList.AddRange(args);
            int argSize;
            byte[] packedArgs = Convert(completeArgList.ToArray(), out argSize);
            
            var handleSize = GCHandle.Alloc(argSize, GCHandleType.Pinned);
            IntPtr[] extra = {
                new IntPtr(1L), Marshal.UnsafeAddrOfPinnedArrayElement(packedArgs,0), 
                new IntPtr(2L), handleSize.AddrOfPinnedObject(),
                new IntPtr(0L)
            };

            var handle2 = GCHandle.Alloc(extra, GCHandleType.Pinned);
            // Hybridizer kernels reserve 1024 bytes of dynamic shared memory for the runtime
            // (the int[256] runtime buffer pinned above is consumed device-side); user-requested
            // sharedMem is added on top. Do not lower this constant without coordinating with the
            // generated kernel ABI.
            var result = driver.LaunchKernel(func, gridDim.x, gridDim.y, gridDim.z, blockDim.x, blockDim.y, blockDim.z, 1024 + sharedMem, stream, IntPtr.Zero, Marshal.UnsafeAddrOfPinnedArrayElement(extra,0));

            handle2.Free();
            handleSize.Free();
            runtimeHandle.Free();
            return result;
        }
        #endregion
    }
}
