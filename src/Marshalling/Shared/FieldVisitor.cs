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
    abstract class FieldVisitor
    {
        internal void VisitFields(object param, TypeInfo typeInfo, Dictionary<FieldInfo, byte[]> overrides)
        {
            foreach (FieldTools.FieldDeclaration key in typeInfo.fields)
            {
                HandleField(typeInfo, key, param, overrides);
            }

            HandleEndOfObject(param);
        }

        void HandleField(TypeInfo typeInfo, FieldTools.FieldDeclaration key, object param, Dictionary<FieldInfo, byte[]> overrides)
        {
            bool hasOverride = overrides.Count > 0;
            if (typeInfo.type.IsGenericType && typeInfo.type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (key.Name == "hasValue")
                {
                    HandlePrimitive(key, param);
                    return;
                }
                if (key.Name == "value")
                {
                    HandlePrimitive(key, param);
                    return;
                }
            }
            switch (key.Type)
            {
                case FieldTools.FieldDeclaration.FieldDeclarationType.VTABLE:
                    HandleVTable(typeInfo.typeId);
                    break;
                case FieldTools.FieldDeclaration.FieldDeclarationType.PADDING:
                    HandlePaddingByte(key.Count);
                    break;
                default:
                    if (key.UnionSubFields != null && key.UnionSubFields.Count > 0)
                    {
                        long max = long.MinValue;
                        int maxindex = 0;
                        int fdindex = 0;
                        foreach (var subfd in key.UnionSubFields)
                        {
                            long subfdsize = FieldTools.SizeOf(subfd.FieldType);
                            if (subfdsize > max)
                            {
                                max = subfdsize;
                                maxindex = fdindex;
                            }
                            ++fdindex;
                        }

                        HandleField(typeInfo, key.UnionSubFields[maxindex], param, overrides);
                    }
                    else if (hasOverride && overrides.ContainsKey(key.Info))
                    {
                        byte[] data = overrides[key.Info];

                        if (key.FieldType.IsArray || key.FieldType.IsClass || key.FieldType.IsInterface)
                        {
                            var r = new BinaryReader(new MemoryStream(data));
                            IntPtr devPtr = new IntPtr(r.ReadInt64());
                            HandleOverridenPtr(key, param, devPtr);
                        }
                        else
                        {
                            HandleOverride(data);
                        }
                    }
                    else if (key.FieldType.IsPrimitive)
                    {
                        HandlePrimitive(key, param);
                    }
                    else if (key.FieldType.IsValueType)
                    {
                        HandleValueType(key, param, overrides);
                    }
                    else
                    {
                        if (!key.FieldType.IsArray && !key.FieldType.IsClass && !key.FieldType.IsInterface)
                            throw new NotImplementedException();
                        if (typeof(Delegate).IsAssignableFrom(key.FieldType))
                            HandleDelegate(key, param, IntPtr.Zero);
                        else
                            HandleObject(key, param, IntPtr.Zero);
                    }
                    break;
            }
        }

        protected virtual void HandleOverridenPtr(FieldTools.FieldDeclaration key, object o, IntPtr devPtr)
        {
            HandlePtr(key, o, devPtr, true);
        }


        public struct PendingDelegateTarget
        {
            public BinaryWriter binaryWriter;
            public long streamPosition;
        }

        public HashSet<PendingDelegateTarget> pendingDelegateTargets = new HashSet<PendingDelegateTarget>();

        protected abstract void HandleVTable(long o);
        protected abstract void HandlePaddingByte(long count);
        protected abstract void HandleOverride(byte[] data);
        protected abstract void HandlePrimitive(FieldTools.FieldDeclaration key, object o);
        protected abstract void HandleObject(FieldTools.FieldDeclaration key, object param, IntPtr p);
        protected abstract void HandleDelegate(FieldTools.FieldDeclaration key, object param, IntPtr p);
        protected abstract void HandlePtr(FieldTools.FieldDeclaration key, object param, IntPtr o, bool isResidentArray = false);
        protected abstract void HandleValueType(FieldTools.FieldDeclaration key, object o, Dictionary<FieldInfo, byte[]> overrides);
        protected abstract void HandleEndOfObject(object param);
        internal abstract IntPtr AllocateObject(object param);
        internal abstract void CopyObject(object param, IntPtr dev);
        internal abstract void start(object param, Type type, IntPtr da);
    }
}