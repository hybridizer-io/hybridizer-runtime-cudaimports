/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


#pragma warning disable 1591
namespace Hybridizer.Runtime.CUDAImports
{
    public class FieldTools
    {
        public enum FieldTypeEnum { BOOL, BYTE, SHORT, INT, UINT, LONG, ULONG, FLOAT, DOUBLE, OTHER }
        private static IDictionary<Type, IList<FieldDeclaration[]>> _declarationCache = new Dictionary<Type, IList<FieldDeclaration[]>>();
        private static IDictionary<Type, FieldTypeEnum> _typeToEnum = new Dictionary<Type, FieldTypeEnum>();

        private static SafeDictionary<Type, FieldDeclaration[]> _orderedFieldsCache = new SafeDictionary<Type, FieldDeclaration[]>();
        private static SafeDictionary<Type, long> _sizeCache = new SafeDictionary<Type, long>();


        private static Type[] funcTypes = new Type[]
        {
            typeof(Func<>),
            typeof(Func<,>),
            typeof(Func<,,>),
            typeof(Func<,,,>),
            typeof(Func<,,,,>),
            typeof(Func<,,,,,>),
            typeof(Func<,,,,,,>),
            typeof(Func<,,,,,,,>),
            typeof(Func<,,,,,,,,>),
            typeof(Func<,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,>),
            typeof(Func<,,,,,,,,,,,,,,,>),
        };

        private static Type[] actionTypes = new Type[]
        {
            typeof(Action<>),
            typeof(Action<,>),
            typeof(Action<,,>),
            typeof(Action<,,,>),
            typeof(Action<,,,,>),
            typeof(Action<,,,,,>),
            typeof(Action<,,,,,,>),
            typeof(Action<,,,,,,,>),
            typeof(Action<,,,,,,,,>),
            typeof(Action<,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,,,>),
            typeof(Action<,,,,,,,,,,,,,,,>),
        };

        static FieldTools()
        {
            _typeToEnum.Add(typeof(bool), FieldTypeEnum.BOOL);
            _typeToEnum.Add(typeof(byte), FieldTypeEnum.BYTE);
            _typeToEnum.Add(typeof(short), FieldTypeEnum.SHORT);
            _typeToEnum.Add(typeof(int), FieldTypeEnum.INT);
            _typeToEnum.Add(typeof(uint), FieldTypeEnum.UINT);
            _typeToEnum.Add(typeof(long), FieldTypeEnum.LONG);
            _typeToEnum.Add(typeof(ulong), FieldTypeEnum.ULONG);
            _typeToEnum.Add(typeof(float), FieldTypeEnum.FLOAT);
            _typeToEnum.Add(typeof(double), FieldTypeEnum.DOUBLE);
        }

        public static bool HasVTable(Type t)
        {
            if (t.IsValueType) return false;
            if (t.IsPrimitive) return false;
            return true;
        }

        public struct FieldDeclaration
        {
            public enum FieldDeclarationType { VTABLE, PADDING, DATA }
            // might be null for VTABLE
            public string Name { get; set; }
            // might be null for VTABLE or Pointers
            public Type FieldType { get; set; }

            public FieldTypeEnum TypeEnum { get; set; }
            // might be null for VTABLE or PADDING
            public FieldInfo Info { get; set; }

            public long Count { get; set; }
            public FieldDeclarationType Type { get; set; }

            public List<FieldDeclaration> UnionSubFields { get; set; }
            
            public static FieldDeclaration MakeUnion(List<string> names, List<Type> fieldTypes, List<FieldInfo> infos, List<FieldDeclarationType> types, List<int> counts)
            {
                FieldDeclaration result = new FieldDeclaration();
                result.UnionSubFields = new List<FieldDeclaration>();
                for(int k = 0; k < names.Count; ++k)
                {
                    result.UnionSubFields.Add(new FieldDeclaration(names[k], fieldTypes[k], infos[k], types[k], counts[k]));
                }

                result.Type = FieldDeclarationType.DATA;

                return result;
            }

            public FieldDeclaration(string name, Type fieldType, FieldInfo info, FieldDeclarationType type = FieldDeclarationType.DATA, long count = 1)
                : this()
            {
                if (info != null)
                {
                    FixedBufferAttribute attr = GetCustomAttribute<FixedBufferAttribute>(info);
                    if (attr != null)
                    {
                        fieldType = attr.ElementType;
                        count = attr.Length;
                    }
                    else
                    {
                        // Heuristic to detect fixedbuffers when FixedBuffer attribute is missing (seems to be the case when the field is not public with Mono)
                        CompilerGeneratedAttribute compilerGenerated = GetCustomAttribute<CompilerGeneratedAttribute>(info.FieldType);
                        if (compilerGenerated != null)
                        {
                            var fixedEltField = info.FieldType.GetField("FixedElementField");
                            if (fixedEltField != null)
                            {
                                fieldType = fixedEltField.FieldType;
                                count = Marshal.SizeOf(info.FieldType)/SizeOf(fieldType);
                            }
                        }
                        
                    }
                }
                Name = name; FieldType = fieldType; Info = info;
                FieldTypeEnum te;
                if (fieldType == null || !_typeToEnum.TryGetValue(fieldType, out te))
                {
                    te = FieldTypeEnum.OTHER;
                }
                TypeEnum = te;
                Count = count;
                Type = type;
            }

            public override string ToString()
            {
                return String.Format("{0} : {1}[{2}] - TypeEnam={3}, Type={4}, ByteCount={5}", Name, FieldType, Count, TypeEnum, Type, ByteCount);
            }

            public long ByteCount 
            {
                get
                {
                    if (Type == FieldDeclarationType.PADDING)
                    {
                        switch (TypeEnum)
                        {
                            case FieldTypeEnum.BYTE:
                                return Count;
                            case FieldTypeEnum.DOUBLE:
                                return Count*8;
                            case FieldTypeEnum.FLOAT:
                                return Count*4;
                            case FieldTypeEnum.INT:
                                return Count*4;
                            case FieldTypeEnum.LONG:
                                return Count*8;
                            case FieldTypeEnum.SHORT:
                                return Count*2;
                            case FieldTypeEnum.UINT:
                                return Count*4;
                            case FieldTypeEnum.ULONG:
                                return Count*8;
                            case FieldTypeEnum.OTHER:
                                return Count;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    else if (Type == FieldDeclarationType.VTABLE || FieldType == null)
                        return 8;
                    else
                    {
                        return Count*FieldTools.SizeOf(FieldType);
                    }
                }
            }
        }

        public static long SizeOf(Type t, long offset = 0, long count = 1)
        {
            if (offset == 0 && count == 1)
            {
                long cacheResult;
                if (_sizeCache.TryGetValue(t, out cacheResult))
                {
                    return cacheResult;
                }
                // FDU TODO : CANNOT CACHE RESULT FOR DIFFERENT OFFSETS !!!!
                cacheResult = SizeOf_(t, offset, count);
                _sizeCache[t] = cacheResult;
                // FDU TODO : DON'T RETURN ??
            }
            return SizeOf_(t, offset, count);
        }

        private static long SizeOf_(Type t, long offset, long count)
        {
            if (t.GUID == Guid.Parse("506315CE-E8F4-46A3-AE9F-A1A950A8FD4C")) // half
                return 2;
            if (t.GUID == Guid.Parse("FB1994C2-5E2A-4748-B745-C09CD7AD9C06")) // half2
                return 4;
            if (t.GUID == Guid.Parse("FB1994C2-5E2A-4748-B745-C09CD7AD9C06")) // half8
                return 16;
            if (t.IsPrimitive)
            {
                if (t == typeof(IntPtr)) return 8 * count;
                if (t == typeof(UIntPtr)) return 8 * count;
                long s = Marshal.SizeOf(t) * count;
                //if ((s == 8) && (offset % 8) != 0)
                //{
                //    // Pad to 8 bytes
                //    s += 8 - (offset % 8);
                //}

                return s;
            }
            else if (t.IsEnum)
            {
                long s = Marshal.SizeOf(Enum.GetUnderlyingType(t)) * count;
                if ((s == 8) && (offset % 8) != 0)
                {
                    // Pad to 8 bytes
                    s += 8 - (offset % 8);
                }
                return s;
            }
            else if (t == typeof(string))
            {
                return 8;
            }
            else if (typeof(Delegate).IsAssignableFrom(t))
            {
                return 24 * count; // 8 bytes for the instance + 8 bytes for the function pointer + 8 bytes for the _isStatic bool (including padding)
            }

            var interfaces = t.GetInterfaces();
            foreach (Type i in interfaces)
            {
                if(i.GUID == Guid.Parse("7B4E8768-BB70-4088-ABC1-DBBA55F370F5")) // ICustomMarshalled 
                {
                    foreach (var attribute in t.GetCustomAttributes(true))
                    {
                        if (attribute.GetType().GUID == Guid.Parse("19B45042-6B32-420C-AEB2-2ACE70D23531")) // ICustomMarshalledSize
                        {
                            long size = Convert.ToInt64(attribute.GetType().GetProperty("Size").GetGetMethod().Invoke(attribute, new object[0]));
                            return size * count;
                        }
                    }

                    throw new ApplicationException("ICustomMarshalled types must have a ICustomMarshalledSize attribute");
                }
            }

            IList<FieldDeclaration[]> fields = GetFields(t);
            long result = 0;
            foreach (FieldDeclaration[] fdl in fields)
            {
                // pad to eight bytes -> there seems to be an issue here
                // if (((result + offset) % 8) != 0)
                //     result += 8 - (result % 8);
                foreach (FieldDeclaration fd in fdl)
                {
                    result = SizeofField(t, offset, result, fd);
                }

                result *= count;
            }
            return result;
        }

        private static long SizeofField(Type t, long offset, long result, FieldDeclaration fd)
        {
            switch (fd.Type)
            {
                case FieldDeclaration.FieldDeclarationType.VTABLE:
                    result += 8;
                    break;
                case FieldDeclaration.FieldDeclarationType.PADDING:
                    result += fd.Count;
                    break;
                default:
                    if(fd.UnionSubFields != null && fd.UnionSubFields.Count > 0)
                    {
                        long max = long.MinValue;
                        int maxindex = 0;
                        int fdindex = 0;
                        foreach (var subfd in fd.UnionSubFields)
                        {
                            long subfdsize = FieldTools.SizeOf(subfd.FieldType);
                            if (subfdsize > max)
                            {
                                max = subfdsize;
                                maxindex = fdindex;
                            }
                            ++fdindex;
                        }
                        return SizeofField(t, offset, result, fd.UnionSubFields[maxindex]);
                    }
                    else if (fd.FieldType.IsArray && CudaRuntimeProperties.UseHybridArrays)
                    {
                        long arrayRank = fd.FieldType.GetArrayRank();
                        result += 8 + arrayRank * 8;
                        // result += 16;
                    }
                    else if (typeof(Delegate).IsAssignableFrom(fd.FieldType))
                    {
                        result += 24;
                    }
                    else if (fd.FieldType.IsClass || fd.FieldType.IsInterface)
                        result += 8; // All pointers are now 64 bits
                    else
                    {
                        var sfd = SizeOf(fd.FieldType, result + offset, fd.Count);
                        if (t.IsGenericType)
                        {
                            var gfd = t.GetGenericTypeDefinition().GetField(fd.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            if (gfd != null && gfd.FieldType.IsGenericParameter)
                            {
                                // add padding for generic fields
                                while (sfd % 8 != 0)
                                {
                                    sfd += 1;
                                }
                            }
                        }
                        result += sfd;
                    }
                    break;
            }

            return result;
        }

        public static IList<FieldDeclaration[]> GetFields(Type t)
        {
            lock (_declarationCache) // Ensure that only one thread accesses this part
            {
                IList<FieldDeclaration[]> cacheResult = null;
                if (_declarationCache.TryGetValue(t, out cacheResult))
                    return new List<FieldDeclaration[]>(cacheResult);
                // TODO : support for fixed arrays ?

                bool serializeAllFields = t.IsValueType || t == typeof(ValueType) || t.IsExplicitLayout;
                var fis = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance).ToList();
                if (fis.Any(IsIgnore))
                    serializeAllFields = false;

				// FDU: @AON
				if (((t.IsGenericTypeDefinition) || (t.IsGenericType)))
					if(t.GetGenericTypeDefinition() != typeof(Nullable<>))
						serializeAllFields = false;
				// end FDU

				if (serializeAllFields)
                {
                    if (t.IsEnum)
                        throw new ApplicationException();
                    // sort by offset
                    SortedDictionary<int, List<FieldInfo>> fields = new SortedDictionary<int, List<FieldInfo>>();
                   
                    foreach (FieldInfo f in fis)
                    {
                        int offset = 0;
                        try
                        {
                            offset = Marshal.OffsetOf(t, f.Name).ToInt32();
                        }
                        catch (ArgumentException)
                        {
                            // This happens on DateTime, so if there is only one field, suppose that offset is 0
                            if (fis.Count > 1)
                            {
                                Console.Out.WriteLine("Type {0} will not be marshalable using default marshaler", t.FullName);
                                return new List<FieldDeclaration[]>();
                            }
                        }
                        if (fields.ContainsKey(offset))
                        {
                            fields[offset].Add(f);
                        }
                        else
                        {
                            fields.Add(offset, new List<FieldInfo> { f });
                        }
                    }
                    // build declarations
                    int paddingIndex = 0;
                    IList<FieldDeclaration> result = new List<FieldDeclaration>();
                    if (HasVTable(t) && t != typeof(ValueType))
                    {
                        result.Add(new FieldDeclaration("$__VTABLE", null, null, FieldDeclaration.FieldDeclarationType.VTABLE));
                    }
                    long size = 0;
                    foreach (List<FieldInfo> allFields in fields.Values)
                    {
                        if(allFields.Count == 0)
                        {
                            Console.Error.WriteLine("empty field list -- ignore");
                            continue;
                        }
                        else if(allFields.Count == 1)
                        {
                            var fi = allFields.First();
                            var fieldSize = SizeOf(fi.FieldType);
                            var padding = Math.Min(fieldSize, 8);
                            if (padding > 0 && size % padding > 0)
                            {
                                var count = padding - size % padding;
                                result.Add(new FieldDeclaration(String.Format("__hybridizer_padding_{0}", paddingIndex++), typeof(byte), null, FieldDeclaration.FieldDeclarationType.PADDING, count));
                                size += count;
                            }
                            result.Add(new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            size += fieldSize;
                        }
                        else
                        {
                            var maxSize = long.MinValue;
                            foreach (var fi in allFields)
                            {
                                var fieldSize = SizeOf(fi.FieldType);
                                if (fieldSize > maxSize)
                                    maxSize = fieldSize;
                            }
                            result.Add(FieldDeclaration.MakeUnion(allFields.Select(fi => fi.Name).ToList(), allFields.Select(fi => fi.FieldType).ToList(), allFields, allFields.Select(fi => FieldDeclaration.FieldDeclarationType.DATA).ToList(), Enumerable.Repeat(1, allFields.Count).ToList()));

                            var padding = Math.Min(maxSize, 8);
                            if (padding > 0 && size % padding > 0)
                            {
                                var count = padding - size % padding;
                                result.Add(new FieldDeclaration(String.Format("__hybridizer_padding_{0}", paddingIndex++), typeof(byte), null, FieldDeclaration.FieldDeclarationType.PADDING, count));
                                size += count;
                            }
                        }
                    }

                    // Finally pad to 8 -- if not an intrinsic type
                    var attributes = t.GetCustomAttributesData();
                    if (!attributes.Any(att => att.Constructor.DeclaringType.GUID == Guid.Parse("AFF70C75-30C4-4598-88D2-74CD3C212111") /*"IntrinsicTypeAttribute-30C4-4598-88D2-74CD3C212111"*/ || att.Constructor.DeclaringType.GUID == Guid.Parse("33AF19F3-A2C0-48DD-828C-0A594788250C") /*IntrinsicPrimitiveAttribute*/))
                    {
                        if (size % 8 != 0)
                            result.Add(new FieldDeclaration(String.Format("__hybridizer_padding_{0}", paddingIndex++), typeof(byte), null, FieldDeclaration.FieldDeclarationType.PADDING, 8 - size % 8));
                    }

                    cacheResult = new List<FieldDeclaration[]>(new FieldDeclaration[][] { result.ToArray() });

                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                        cacheResult = new List<FieldDeclaration[]>(new FieldDeclaration[][] { result.Reverse().ToArray() });

                    if (!_declarationCache.ContainsKey(t))
                        _declarationCache.Add(t, cacheResult);
                    else
                        VerifyAlike(cacheResult, _declarationCache[t]);
                    return cacheResult;
                }
                else
                {
                    int paddingIndex = 0;

                    IList<FieldDeclaration[]> result = new List<FieldDeclaration[]>();
                    bool declareVTable = HasVTable(t);
                    if (t.BaseType != null && t.BaseType == typeof(Exception))
                    {
                        declareVTable = false;
                        result.Add(new FieldDeclaration[] {new FieldDeclaration("_message", typeof(string), t.BaseType.GetField("message"))});
  
                    } 
                    else if ((t.BaseType != null) && (t.BaseType != typeof(object)))
                    {
                        // clone !!!
                        result = new List<FieldDeclaration[]>(GetFields(t.BaseType));
                        declareVTable = false;
                    }
                    List<FieldDeclaration> current = new List<FieldDeclaration>();
                    // 1. declare vtable
                    if (declareVTable)
                        current.Add(new FieldDeclaration("$__VTABLE", null, null, FieldDeclaration.FieldDeclarationType.VTABLE));

                    // 1.5. get all generic types
                    SortedDictionary<string, FieldDeclaration> generics = new SortedDictionary<string, FieldDeclaration>();
                    if (t.IsGenericType)
                    {
                        var gtd = t.GetGenericTypeDefinition();
                        var gtdGenericFields = gtd.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance).Where(x => ! IsIgnore(x) && x.FieldType.IsGenericParameter);
                        foreach (var fiGen in gtdGenericFields)
                        {
                            var fi = t.GetField(fiGen.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                            generics.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                        }

                    }
                    current.AddRange(generics.Values);
                    fis.RemoveAll(x => generics.Any(y => y.Value.Info == x));

                    // 2. get all structures
                    SortedDictionary<string, FieldDeclaration> structures = new SortedDictionary<string, FieldDeclaration>();
                    foreach (FieldInfo fi in fis)
                    {
                        if (IsIgnore(fi)) continue;
                        if (!IsSpecialClassConsideredAsStruct(fi.FieldType))
                        {
                            if (fi.FieldType.IsClass || fi.FieldType.IsInterface) continue;
                            if (fi.FieldType.IsPrimitive || fi.FieldType.IsEnum) continue;
                        }
                        if (IsSpecialClassConsideredAsStruct(fi.FieldType) || fi.FieldType.IsValueType)
                        {
                            structures.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            long size = SizeOf(fi.FieldType);
                            if ( (size % 8) != 0)
                            {
                                long pCount = 8 - (size % 8);
                                structures.Add(fi.Name + "##padding", new FieldDeclaration(String.Format("__hybridizer_padding_{0}", paddingIndex), null, null, FieldDeclaration.FieldDeclarationType.PADDING, pCount));
                                ++paddingIndex;
                            }

                            continue;
                        }
                        throw new ApplicationException("INTERNAL ERROR");
                    }
                    current.AddRange(structures.Values);
                    fis.RemoveAll(x => structures.Any(y => y.Value.Info == x));

                    // 3. get all classes -- to be declared 8-bytes aligned
                    SortedDictionary<string, FieldDeclaration> classesAndPtrs = new SortedDictionary<string, FieldDeclaration>();
                    foreach (FieldInfo fi in fis)
                    {
                        if (IsIgnore(fi)) continue;
                        if (fi.FieldType == typeof(IntPtr) || fi.FieldType == typeof(UIntPtr))
                        {
                            classesAndPtrs.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            continue;
                        }
                        if (fi.FieldType.IsValueType) continue;
                        if (fi.FieldType.IsPrimitive || fi.FieldType.IsEnum) continue;
                        if (fi.FieldType.IsClass || fi.FieldType.IsInterface)
                        {
                            classesAndPtrs.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            continue;
                        }
                        throw new ApplicationException("INTERNAL ERROR");
                    }
                    current.AddRange(classesAndPtrs.Values);
                    fis.RemoveAll(x => classesAndPtrs.Any(y => y.Value.Info == x));

                    // 4. get all primitives - by size
                    SortedDictionary<string, FieldDeclaration> primitives8bytes = new SortedDictionary<string, FieldDeclaration>();
                    SortedDictionary<string, FieldDeclaration> primitives4bytes = new SortedDictionary<string, FieldDeclaration>();
                    SortedDictionary<string, FieldDeclaration> primitives2bytes = new SortedDictionary<string, FieldDeclaration>();
                    SortedDictionary<string, FieldDeclaration> primitives1byte = new SortedDictionary<string, FieldDeclaration>();
                    foreach (FieldInfo fi in fis)
                    {
                        if (IsIgnore(fi)) continue;
                        if (fi.FieldType.IsClass || fi.FieldType.IsInterface) continue;
                        if (fi.FieldType.IsPrimitive)
                        {
                            if (Marshal.SizeOf(fi.FieldType) == 8)
                                primitives8bytes.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            else if (Marshal.SizeOf(fi.FieldType) == 4)
                                primitives4bytes.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            else if (Marshal.SizeOf(fi.FieldType) == 2)
                                primitives2bytes.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            else if (Marshal.SizeOf(fi.FieldType) == 1)
                                primitives1byte.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                            else throw new ApplicationException("INTERNAL ERROR");
                            continue;
                        }
                        if (fi.FieldType.IsEnum)
                        {
                            if (fi.FieldType.IsEnum)
                            {
                                var ut = Enum.GetUnderlyingType(fi.FieldType);
                                if (Marshal.SizeOf(ut) == 8)
                                    primitives8bytes.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                                else if (Marshal.SizeOf(ut) == 4)
                                    primitives4bytes.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                                else if (Marshal.SizeOf(ut) == 2)
                                    primitives2bytes.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                                else if (Marshal.SizeOf(ut) == 1)
                                    primitives1byte.Add(fi.Name, new FieldDeclaration(fi.Name, fi.FieldType, fi));
                                else throw new ApplicationException("INTERNAL ERROR");
                                continue;
                            }
                        }
                        if (fi.FieldType.IsValueType) continue;
                        throw new ApplicationException("INTERNAL ERROR");
                    }
                    current.AddRange(primitives8bytes.Values);
                    current.AddRange(primitives4bytes.Values);
                    current.AddRange(primitives2bytes.Values);
                    current.AddRange(primitives1byte.Values);

                    fis.RemoveAll(x => primitives8bytes.Any(y => y.Value.Info == x));
                    fis.RemoveAll(x => primitives4bytes.Any(y => y.Value.Info == x));
                    fis.RemoveAll(x => primitives2bytes.Any(y => y.Value.Info == x));
                    fis.RemoveAll(x => primitives1byte.Any(y => y.Value.Info == x));

                    // pad ?
                    int primitivePadderSize =
                        (4 * primitives4bytes.Values.Count) +
                        (2 * primitives2bytes.Values.Count) +
                        (1 * primitives1byte.Values.Count);
                    while ((primitivePadderSize % 8) != 0)
                    {
                        int pCount = 8 - (primitivePadderSize % 8);
                        current.Add(new FieldDeclaration(String.Format("__hybridizer_padding_{0}", paddingIndex), null, null, FieldDeclaration.FieldDeclarationType.PADDING, pCount));
                        ++paddingIndex;
                        primitivePadderSize += pCount;
                    }
                    // store fields
                    result.Add(current.ToArray());

                    // done !
                    if (!_declarationCache.ContainsKey(t))
                        _declarationCache.Add(t, result);
                    else
                        VerifyAlike(result, _declarationCache[t]);
                    return result;
                }
            }
        }

        private static bool IsIgnore(FieldInfo fi)
        {
            Guid ignoreGuid = Guid.Parse("3B81B49D-9BBC-41D0-B367-88ABDC405174");
            try
            {
                if (fi.GetCustomAttributes(true).FirstOrDefault(attr => attr.GetType().GUID == ignoreGuid) != null)
                    return true;
                if (fi.IsDefined(typeof(CompilerGeneratedAttribute), true))
                {
                    var match = Regex.Match(fi.Name, "<(.+?)>k__BackingField");
                    if (match.Success)
                    {
                        var propertyName = match.Groups[1].Value;
                        // ReSharper disable once PossibleNullReferenceException
                        var autoProperty = fi.DeclaringType.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                        if (autoProperty != null)
                        {
                            if (autoProperty.GetCustomAttributes(true).FirstOrDefault(attr => attr.GetType().GUID == ignoreGuid) != null)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch { return false; }
        }

        private static Dictionary<MemberInfo, Attribute> getCustomAttribute_cache = new Dictionary<MemberInfo, Attribute>();

        private static T GetCustomAttribute<T>(MemberInfo mi) where T : Attribute
        {
            if (getCustomAttribute_cache.ContainsKey(mi))
            {
                return (T) getCustomAttribute_cache[mi];
            }

            var catts = mi.GetCustomAttributesData();

            T res = null;
            if (catts.Count > 0)
            {
                var epas =
                    catts.Where(x => x.Constructor.DeclaringType.GUID == typeof(T).GUID);
                if (epas.Count() > 0)
                {
                    var epa = epas.FirstOrDefault();
                    if (epa.ConstructorArguments != null)
                    {
                        List<object>args = new List<object>();
                        List<Type> types = new List<Type>();
                        foreach (var argument in epa.ConstructorArguments)
                        {
                            args.Add(argument.Value);
                            types.Add(argument.ArgumentType);
                        }
                        var constructorInfo = typeof(T).GetConstructor(types.ToArray());
                        if (constructorInfo == null)
                        {
                            return null;
                        }
                        res = constructorInfo.Invoke(args.ToArray()) as T;
                    }
                    if (res == null)
                        throw new ApplicationException("Invalid attribute");
                    if (epa.NamedArguments != null)
                    {
                        foreach (var namedArg in epa.NamedArguments)
                        {
                            typeof(T).GetProperty(namedArg.MemberInfo.Name).SetValue(res, namedArg.TypedValue.Value, null);
                        }
                    }
                }
            }

            getCustomAttribute_cache.Add(mi, res);
            return res;
        }

        public static FieldDeclaration[] OrderedFields(Type t)
        {
            FieldDeclaration[] cacheResult = null;
            if (_orderedFieldsCache.TryGetValue(t, out cacheResult))
                return cacheResult;

            List<FieldDeclaration> tmpRes;
            if (t == null)
            {
                tmpRes = new List<FieldDeclaration>(0);
            }
            else if (t == typeof(object))
            {
                tmpRes = new List<FieldDeclaration>();
                tmpRes.Add(new FieldDeclaration(null, null, null, FieldDeclaration.FieldDeclarationType.VTABLE));
            }
            else
            {
                tmpRes = new List<FieldDeclaration>();
                foreach (FieldDeclaration[] fieldGroup in GetFields(t))
                {
                    tmpRes.AddRange(fieldGroup);
                }
            }
            cacheResult = tmpRes.ToArray();
            _orderedFieldsCache[t] = cacheResult;
            return cacheResult;
        }


        private static bool VerifyAlike(IList<FieldDeclaration[]> result, IList<FieldDeclaration[]> list)
        {
            if (result.Count != list.Count)
                return false;
            for (int k = 0; k < result.Count; ++k)
            {
                if (!VerifyAlike(result[k], list[k]))
                    return false;
            }
            return true;
        }

        private static bool VerifyAlike(FieldDeclaration[] fieldDeclaration, FieldDeclaration[] fieldDeclaration_2)
        {
            if (fieldDeclaration.Length != fieldDeclaration_2.Length)
                return false;
            for (int k = 0; k < fieldDeclaration.Length; ++k)
            {
                if (!Equals(fieldDeclaration[k], fieldDeclaration_2[k]))
                    return false;
            }
            return true;
        }

        public static bool IsSpecialClassConsideredAsStruct(Type t)
        {
            if (t == null)
                return false;

            if (GetCustomAttribute<CompilerGeneratedAttribute>(t) != null)
                return true;
            if (t.IsGenericType && actionTypes.Contains(t.GetGenericTypeDefinition()))
                return true;
            if (t.IsGenericType && funcTypes.Contains(t.GetGenericTypeDefinition()))
                return true;
            if (typeof(Delegate).IsAssignableFrom(t))
                return true;
            return false;
        }
    }

    public class SafeDictionary<TKey, TValue>
    {
        private readonly object _Padlock = new object();
        private readonly Dictionary<TKey, TValue> _Dictionary = new Dictionary<TKey, TValue>();

        public TValue this[TKey key]
        {
            get
            {
                lock (_Padlock)
                {
                    return _Dictionary[key];
                }
            }

            set
            {
                lock (_Padlock)
                {
                    _Dictionary[key] = value;
                }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_Padlock)
            {
                return _Dictionary.TryGetValue(key, out value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_Padlock)
            {
                _Dictionary.Add(key, value);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_Padlock)
            {
                return _Dictionary.Remove(key);
            }
        }

        public void Clear()
        {
            lock (_Padlock)
            {
                _Dictionary.Clear();
            }
        }
    }

    public struct CudaRuntimeProperties
    {
        public static bool UseHybridArrays { get; set; }
    }
}
#pragma warning restore 1591