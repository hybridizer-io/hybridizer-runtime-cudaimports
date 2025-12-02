using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Hybridizer.Runtime.CUDAImports
{
    public static class TdrDetection
    {
        static ITdrRegistries _tdrRegistries = TdrRegistriesFactory.GetInstance();

        static bool _IsTdrEnabledRead = false;
        static bool _TdrDelayRead = false;
        static bool _TdrEnabled = false;
        static int _TdrDelay;

        public static bool IsTdrEnabled()
        {
            if (!_IsTdrEnabledRead)
            {
                _IsTdrEnabledRead = true;
                _TdrEnabled = _tdrRegistries.TdrLevel > 0;
            }

            return _TdrEnabled;
        }

        public static int TdrDelay()
        {
            if (!_TdrDelayRead)
            {
                _TdrDelayRead = true;
                if (!IsTdrEnabled())
                {
                    _TdrDelay = -1;
                }
                else
                {
                    _TdrDelay = _tdrRegistries.TdrDelay;
                }
            }

            return _TdrDelay;
        }
    }

    internal interface ITdrRegistries
    {
        int TdrLevel { get; }
        int TdrDelay { get; }
    }

    internal static class TdrRegistriesFactory
    {
        static ITdrRegistries _instance;
        static TdrRegistriesFactory()
        {
            _instance = null;
        }

        public static ITdrRegistries GetInstance()
        {
            if (_instance == null)
            {
                if (Type.GetType("Mono.Runtime") != null)
                {
                    _instance = new LinuxTdrRegistries();
                }
                else
                {
                    _instance = new WindowsTdrRegistries();
                }
            }

            return _instance;
        }
    }

    internal class LinuxTdrRegistries : ITdrRegistries
    {
        public int TdrLevel
        {
            // TODO
            get { return 0; }
        }

        public int TdrDelay
        {
            // TODO
            get { return int.MaxValue; }
        }
    }

    internal class WindowsTdrRegistries : ITdrRegistries
    {
        Assembly registryAssembly;

        public WindowsTdrRegistries()
        {
            if (IntPtr.Size != 8)
            {
                throw new PlatformNotSupportedException("Hybridizer dropped 32 bits support (as always should and will in desktop/server environments)");
            }

            using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hybridizer.Runtime.CUDAImports.Microsoft.Win32.Registry.dll"))
            {
                byte[] bytes = new byte[resource.Length];
                resource.Read(bytes, 0, bytes.Length);
                registryAssembly = Assembly.Load(bytes);
            }
        }

        public int TdrLevel
        {
            get
            {
                return getRegistryKey("SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers", "TdrLevel");
            }
        }

        public int TdrDelay
        {
            get
            {
                return getRegistryKey( "SYSTEM\\CurrentControlSet\\Control\\GraphicsDrivers", "TdrDelay");
            }
        }

        private int getRegistryKey(string path, string name) 
        {
            Type registryType = registryAssembly.GetType("Microsoft.Win32.Registry");
            Type registryKeyType = registryAssembly.GetType("Microsoft.Win32.Registry");
            registryType.TypeInitializer.Invoke(null, null);
            using (var localMachine = registryType
                .GetField("LocalMachine", BindingFlags.Static | BindingFlags.Public)
                .GetValue(null) as IDisposable)
            {
                using (var baseKey = registryKeyType
                    .GetMethod("OpenSubKey", new Type[] { typeof(string), typeof(bool) })
                    .Invoke(registryKeyType, new object[] { path, false }) as IDisposable)
                {
                    if(baseKey != null)
                    {
                        var keyValue = registryKeyType
                            .GetMethod("GetValue", new Type[] { typeof(string) })
                            .Invoke(baseKey, new object[] { name });
                        if(keyValue is int)
                        {
                            return (int) keyValue;
                        }
                    }

                    return -1;
                }
            }
            //using (RegistryKey baseKey = RegistryKey.OpenBaseKey(localMachine, is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32))
            //{
            //    using (RegistryKey subKey = baseKey.OpenSubKey(path, false))
            //    {
            //        if (subKey != null)
            //        {
            //            object keyValue = subKey.GetValue(name);
            //            if (keyValue is int)
            //            {
            //                return (int)keyValue;
            //            }
            //        }

            //        return -1;
            //    }
            //}
        }
    }
}
