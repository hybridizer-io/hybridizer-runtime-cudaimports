/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
#pragma warning disable 1591
    public interface IKernelInteropTools
    {
        IntPtr LoadLibrary(string libName);
        IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);
    }

    internal class Win32KernelInteropTools : IKernelInteropTools
    {
        [DllImport("kernel32.dll", EntryPoint="LoadLibrary", SetLastError = true)]
        public static extern IntPtr InnerLoadLibrary(string libName);


        [DllImport("kernel32.dll", EntryPoint="GetProcAddress")]
        public static extern IntPtr InnerGetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);

        public IntPtr LoadLibrary(string libName)
        {
            var ptr = InnerLoadLibrary(libName);
            var error = Marshal.GetLastWin32Error();
            if (ptr == IntPtr.Zero && error != 0)
                throw new ApplicationException("Dll load error when loading " + libName + ": " + error);
            return ptr;
        }

        public IntPtr GetProcAddress(IntPtr hModule, string procName)
        {
            return InnerGetProcAddress(hModule, procName);
        }
    }

    internal class LinuxKernelInteropTools : IKernelInteropTools
    {
        public IntPtr LoadLibrary(string fileName)
        {
            return dlopen(fileName, RTLD_LAZY);
        }
        
        /// <summary>
        /// executes a bash command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string ExecuteBashCommand(string command)
        {
            command = command.Replace("\"", "\"\"");
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            proc.WaitForExit();

            return proc.StandardOutput.ReadToEnd();
        }

        public void FreeLibrary(IntPtr handle)
        {
            dlclose(handle);
        }

        public IntPtr GetProcAddress(IntPtr dllHandle, string name)
        {
            // clear previous errors if any
            dlerror();
            var res = dlsym(dllHandle, name);
            var errPtr = dlerror();
            if (errPtr != IntPtr.Zero)
            {
                throw new Exception("dlsym: " + Marshal.PtrToStringAnsi(errPtr));
            }
            return res;
        }

        const int RTLD_LAZY = 1;
        const int RTLD_NOW = 2;
        [DllImport("libdl.so", CharSet=CharSet.Ansi)]
        private static extern IntPtr dlopen(String fileName, int flags);
        [DllImport("libdl.so")]
        private static extern IntPtr dlsym(IntPtr handle, String symbol);
        [DllImport("libdl.so")]
        private static extern int dlclose(IntPtr handle);
        [DllImport("libdl.so")]
        private static extern IntPtr dlerror();
    }

    public class KernelInteropTools
    {
        static IKernelInteropTools instance;

        /// <summary>
        /// return true if environment is linux. 
        /// </summary>
        /// <see href="http://www.mono-project.com/docs/faq/technical/#how-to-detect-the-execution-platform"></see>
        public static bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        static KernelInteropTools()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                instance = new LinuxKernelInteropTools();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                instance = new Win32KernelInteropTools();
            }
            else
                throw new NotImplementedException("not supported OS");
        }

        public static IntPtr LoadLibrary(string libName)
        {
            return instance.LoadLibrary(libName);
        }

        public static IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName)
        {
            try
            {
                return instance.GetProcAddress(hModule, procName);
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }
    }
#pragma warning restore 1591
}
