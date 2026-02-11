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
        internal abstract class AbstractObjectVisiter
        {
            protected NativeSerializerState serState;

            internal abstract IntPtr VisitObject(object o, IntPtr da, bool skipMemcpy = false);
            protected abstract FieldVisitor CreateFieldVisitor();

            protected AbstractObjectVisiter(NativeSerializerState state)
            {
                serState = state;
            }

            internal abstract void Free(IntPtr ptr);

            /// <summary>
            /// Initial point of entry when serializing / deserializing a whole object tree
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public abstract IntPtr InitialVisit(object param, bool skipMemcpy = false);
        }
    }
}