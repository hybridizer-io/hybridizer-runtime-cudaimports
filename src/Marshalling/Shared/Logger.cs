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
    internal class Logger
    {
        static TextWriter w;
        private static bool debug = Debugger.IsAttached;

        // this does not exist on linux -- should never be used
        // [DllImport("kernel32.dll")]
        // static extern void OutputDebugString(string lpOutputString);

        static Logger()
        {
            if (debug)
            {
                w = new DebugWriter();
            }
            else
            {
                w = Console.Out;
            }
        }

        public static TextWriter Out
        {
            get
            {
                return w;
            }
        }

        public static void WriteLine(string message, params object[] variableArguments)
        {
            Console.Out.WriteLine(string.Format(message, variableArguments));
        }
    }
}