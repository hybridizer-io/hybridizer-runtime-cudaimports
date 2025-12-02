/* (c) ALTIMESH 2018 -- all rights reserved */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
    #region COMMON structures

    /// <summary>
    /// Static class to guide work distribution on device for the Thread part : Index
    /// (maps on vector unit index for the vectorized flavors)
    /// </summary>
    [Guid("58611C9B-09E0-4CAB-80E3-E37632F96E4A")]
    public static class threadIdxX64
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static long x { [IntrinsicConstant("__hybridizer_threadIdxXX64")] get { return 0; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static long y { [IntrinsicConstant("__hybridizer_threadIdxYX64")] get { return 0; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static long z { [IntrinsicConstant("__hybridizer_threadIdxZX64")] get { return 0; } }
    }

    /// <summary>
    /// Static class to guide work distribution on device for the Thread part : Index
    /// (maps on vector unit index for the vectorized flavors)
    /// </summary>
    [Guid("8D26DA49-C533-4076-A3DE-629A5A513F3D")]
    public static class threadIdx
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static int x { [IntrinsicConstant("__hybridizer_threadIdxX")] get { return 0; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static int y { [IntrinsicConstant("__hybridizer_threadIdxY")] get { return 0; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static int z { [IntrinsicConstant("__hybridizer_threadIdxZ")] get { return 0; } }
    }

    /// <summary>
    /// Static class to guide work distribution on device for the Block part : Index
    /// </summary>
    [Guid("C1C64E1B-A7A1-4BE4-98C5-0B3DA6B65E24")]
    public static class blockIdxX64
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static long x { [IntrinsicConstant("__hybridizer_blockIdxXX64")] get { return 0; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static long y { [IntrinsicConstant("__hybridizer_blockIdxYX64")] get { return 0; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static long z { [IntrinsicConstant("__hybridizer_blockIdxZX64")] get { return 0; } }
    }

    /// <summary>
    /// Static class to guide work distribution on device for the Block part : Index
    /// </summary>
    [Guid("DBFDA0FB-3EAF-410F-8417-0969B36FE00B")]
    public static class blockIdx
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static int x { [IntrinsicConstant("__hybridizer_blockIdxX")] get { return 0; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static int y { [IntrinsicConstant("__hybridizer_blockIdxY")] get { return 0; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static int z { [IntrinsicConstant("__hybridizer_blockIdxZ")] get { return 0; } }
    }

    /// <summary>
    /// Static class to guide work distribution on device for the Thread part : Dimension
    /// (maps on vector unit index for the vectorized flavors)
    /// </summary>
    [Guid("7F53B70A-455F-423C-9DC1-579415F6BB3E")]
    public static class blockDimX64
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static long x { [IntrinsicConstant("__hybridizer_blockDimXX64")] get { return 1; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static long y { [IntrinsicConstant("__hybridizer_blockDimYX64")] get { return 1; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static long z { [IntrinsicConstant("__hybridizer_blockDimZX64")] get { return 1; } }
    }

    /// <summary>
    /// Static class to guide work distribution on device for the Thread part : Dimension
    /// (maps on vector unit index for the vectorized flavors)
    /// </summary>
    [Guid("ABAD87BA-63A6-44EC-BEE5-BB7A074BC165")]
    public static class blockDim
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static int x { [IntrinsicConstant("__hybridizer_blockDimX")] get { return 1; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static int y { [IntrinsicConstant("__hybridizer_blockDimY")] get { return 1; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static int z { [IntrinsicConstant("__hybridizer_blockDimZ")] get { return 1; } }
    }

    /// <summary>
    /// Static class to guide work distribution on device for the Block part : Dimension
    /// </summary>
    [Guid("1362A3C1-510B-429D-8ACA-DB44CF6FE9E1")]
    public static class gridDimX64
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static long x { [IntrinsicConstant("__hybridizer_gridDimXX64")] get { return 1; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static long y { [IntrinsicConstant("__hybridizer_gridDimYX64")] get { return 1; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static long z { [IntrinsicConstant("__hybridizer_gridDimZX64")] get { return 1; } }
    }
    
    /// <summary>
    /// Static class to guide work distribution on device for the Block part : Dimension
    /// </summary>
    [Guid("33B890E0-28DC-4896-B43D-17E03AEA997D")]
    public static class gridDim
    {
        /// <summary>
        /// X component (lowest weight)
        /// </summary>
        public static int x { [IntrinsicConstant("__hybridizer_gridDimX")] get { return 1; } }
        /// <summary>
        /// Y component
        /// </summary>
        public static int y { [IntrinsicConstant("__hybridizer_gridDimY")] get { return 1; } }
        /// <summary>
        /// Z component (highest weight)
        /// </summary>
        public static int z { [IntrinsicConstant("__hybridizer_gridDimZ")] get { return 1; } }
    }

    /// <summary>
    /// CUDA intrinsics
    /// </summary>
    [Guid("511A9122-F19A-479B-839C-604F8309F168")]
    public static class CUDAIntrinsics
    {
        /// <summary>
        /// synchronize all threads of a block
        /// </summary>
        [IntrinsicFunction("__syncthreads"), HybridCompletionDescription("[CUDA] - synchronize all threads of a block")]
        public static void __syncthreads() { }
    }

    #endregion

    #region VISUAL STUDIO EDITOR

#pragma warning disable 1591
    [Guid("85F8F214-9335-4989-B9D7-92FD87685EDC"), AttributeUsage(AttributeTargets.Method)]
    public class HybridCompletionDescriptionAttribute : Attribute
    {
        string _desc;

        public string Description { get { return _desc; } set { _desc = value; } }
        
        public HybridCompletionDescriptionAttribute() { }

        public HybridCompletionDescriptionAttribute(string desc) { _desc = desc; }

    }
#pragma warning restore 1591

    #endregion

    #region BASIC features
    /// <summary>
    /// internal
    /// </summary>
    [Guid("D1296FFD-4FFC-4B44-B0DB-739EE974FD99")]
    public class SharedMemoryAttribute : Attribute
    {
    }

    /// <summary>
    /// Entry point method
    /// called from host and executed on device
    /// </summary>
    [Guid("25320F68-311E-43BA-B8E9-160D18A6E974")]
    public class EntryPointAttribute : KernelAttribute
    {
        uint _sharedSize = 8192;
        bool _omitSelf = false;
        /// <summary>
        /// obsolete
        /// </summary>
        [Obsolete]
        public bool OmitSelf { get { return _omitSelf; } set { _omitSelf = value; } }
        /// <summary>
        /// reserved
        /// </summary>
        public uint SharedSize { get { return _sharedSize; } set { _sharedSize = value; } }
        /// <summary>
        /// Using Nvrtc for just-in-time compilation
        /// </summary>
        public bool Nvrtc { get; set; }
        /// <summary>
        /// default constructor
        /// </summary>
        public EntryPointAttribute() : this("") { }
        /// <summary>
        /// constructor with name override
        /// </summary>
        /// <param name="name"></param>
        public EntryPointAttribute(string name) : base(name) { _omitSelf = false; }
        /// <summary>
        /// obsolete
        /// </summary>
        [Obsolete]
        public EntryPointAttribute(string name, bool omitSelf) : base(name) { _omitSelf = omitSelf; }
    }

    /// <summary>
    /// register type as target for a is/as cast
    /// </summary>
    [Guid("EB07926D-2C41-4C3A-952D-5156BBDA36F1")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class TypeConversionTargetAttribute : Attribute
    { }

    /// <summary>
    /// register generic type specialization
    /// </summary>
    [Guid("72C3F9A5-E032-44D2-94D6-3F4C55464265")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true)]
    public class RegisterTypeSpecializationAttribute : Attribute
    {
        /// <summary>
        /// type to register
        /// </summary>
        public Type Specialize { get; set; }
    }

    /// <summary>
    /// EntryPoint can be called from device function, spawning dynamic parallelism
    /// </summary>
    [Guid("98266B90-7609-4561-9C59-CB702FFE9888")]
    public class CUDADynamicParallelismAttribute : Attribute
    {
        /// <summary>
        /// grid dimension at launch
        /// </summary>
        public string GridDim { get; set; }
        /// <summary>
        /// block dimension at launch
        /// </summary>
        public string BlockDim { get; set; }
        /// <summary>
        /// shared memory size
        /// </summary>
        public string Shared { get; set; }
        /// <summary>
        /// stream identifier
        /// </summary>
        public string Stream { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public CUDADynamicParallelismAttribute() { GridDim = "1"; BlockDim = "1"; Shared = "0"; Stream = "0"; }
        /// <summary>
        /// full constructor
        /// </summary>
        public CUDADynamicParallelismAttribute(string gridDim, string blockDim, string shared, string stream = "0")
        {
            GridDim = gridDim;
            BlockDim = blockDim;
            Shared = shared;
            Stream = stream;
        }
    }

    /// <summary>
    /// Launch bounds provided to __global__ function
    /// Hints to compiler to optimize register pressure
    /// Complete documentation <see href="https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#launch-bounds">here</see>
    /// </summary>
    [Guid("02B1D7F2-C269-46D1-BB96-46F15FCF0474")]
    public class LaunchBoundsAttribute : Attribute
    {
        private int maxThreadsPerBlock;
        private int minBlocksPerMultiprocessor;

        /// <summary>
        /// maximum threads per block
        /// </summary>
        public int MaxThreadsPerBlock
        {
            get { return maxThreadsPerBlock; }
            set { maxThreadsPerBlock = value; }
        }

        /// <summary>
        /// minimum blocks per multiprocessor
        /// </summary>
        public int MinBlocksPerMultiprocessor
        {
            get { return minBlocksPerMultiprocessor; }
            set { minBlocksPerMultiprocessor = value; }
        }

        /// <summary>
        /// full constructor
        /// </summary>
        public LaunchBoundsAttribute(int maxThreadsPerBlock, int minBlocksPerMultiprocessor)
        {
            this.maxThreadsPerBlock = maxThreadsPerBlock;
            this.minBlocksPerMultiprocessor = minBlocksPerMultiprocessor;
        }

        /// <summary>
        /// default consructor
        /// </summary>
        public LaunchBoundsAttribute()
        {
        }
    }

    /// <summary>
    /// A function running on the device
    /// </summary>
    /// <example>
    /// <code>
    /// [Kernel]
    /// int f (int p) 
    /// {
    ///     return p * p ;
    /// }
    /// </code>
    /// </example>
    [Guid("DBDAF16B-754B-4F87-902F-A1449C6E729A")]
    public class KernelAttribute : Attribute
    {
        string _name;

        /// <summary>
        /// optional override for native generated name
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// reserved
        /// </summary>
        public string[] Profiles { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public KernelAttribute() { }

        /// <summary>
        /// constructor from name
        /// </summary>
        /// <param name="name"></param>
        public KernelAttribute(string name) { _name = name; }
    }


    /// <summary>
    /// Orevents inlining in generated code (speed-up compilation + saves register)
    /// Use with care, as it can alse dramatically slow down the generated code
    /// </summary>
    [Guid("30B4A486-60CB-42DB-98A0-A4233700E233")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property)]
    public class HybNoInlineAttribute: Attribute { }

    /// <summary>
    /// Do not hybridize
    /// Likely because C# code contains non supported code constructs
    /// </summary>
    /// <example>
    /// <code>
    ///  class A
    ///  {
    ///    int x;
    ///    [HybridizerIgnore]
    ///    Dictionary&lt;int, float&gt; somecache;
    ///  }
    /// </code>
    /// </example>
    [Guid("3B81B49D-9BBC-41D0-B367-88ABDC405174")]
    public class HybridizerIgnoreAttribute : Attribute
    {
        List<string> _flavors = new List<string>();
        /// <summary>
        /// list of flavors for which code should be ignored (default : all)
        /// </summary>
        public List<string> Flavors { get { return _flavors; } set { _flavors = value; } }
        /// <summary>
        /// default constructor
        /// </summary>
        public HybridizerIgnoreAttribute() { }
        /// <summary>
        /// Ignore for a specific flavor 
        /// </summary>
        public HybridizerIgnoreAttribute(string flavor) { _flavors = flavor.Split(new[] { ' ', ',', ';' }).ToList(); }
        /// <summary>
        /// Ignor for a list of flavors
        /// </summary>
        /// <param name="flavors"></param>
        public HybridizerIgnoreAttribute(params HybridizerFlavor[] flavors)
        {
            _flavors = flavors.Select(x => x.ToString()).ToList();
        }
        /// <summary>
        /// internal
        /// </summary>
        public bool IsIgnored(string s)
        {
            return Flavors.Count == 0 || Flavors.Contains(s, StringComparer.InvariantCultureIgnoreCase);
        }

        public bool AreIgnored(IEnumerable<string> values)
        {
            return values.Where(IsIgnored).Any();
        }
    }

    /// <summary>
    /// internal - base type for all intrinsics attribute
    /// <see cref="IntrinsicFunctionAttribute"/> 
    /// <seealso cref="IntrinsicTypeAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public abstract class IntrinsicAttribute : KernelAttribute
    {
        /// <summary>
        /// internal
        /// </summary>
        public abstract bool IsFunction { get; set; }

        /// <summary>
        /// internal
        /// </summary>
        public int Flavor { get; set; }

        /// <summary>
        /// internal
        /// </summary>
        public IntrinsicAttribute() { }

        /// <summary>
        /// internal
        /// </summary>
        /// <param name="name"></param>
        public IntrinsicAttribute(string name) : base(name) { }
    }

    /// <summary>
    /// Functions marked as intrinsic -- user shall provide an implementation
    /// </summary>
    /// <example>
    /// <code>
    /// [IntrinsicFunction("::cosf")]
    /// public static float Cosf(float x) {
    ///     return (float) Math.Cos((double) x);
    /// }
    /// </code>
    /// </example>
    [Guid("6F292943-53BC-4456-8068-6D5CF8D97433")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = true)]
    public class IntrinsicFunctionAttribute : IntrinsicAttribute
    {
        bool _generateSource;
        private bool _naked;
        private bool _member;

        /// <summary>
        /// Is function static or member function
        /// </summary>
        public bool IsMember { get { return _member; } set { _member = value; } }

        /// <summary>
        /// Is function -- always true
        /// </summary>
        public override bool IsFunction { get { return true; } set { } }

        /// <summary>
        /// is function naked?
        /// </summary>
        public bool IsNaked { get { return _naked; } set { _naked = value; } }

        /// <summary>
        /// internal
        /// </summary>
        public bool GenerateSource { get { return _generateSource; } set { _generateSource = value; } }

        /// <summary>
        /// default constructor
        /// </summary>
        public IntrinsicFunctionAttribute() { }

        /// <summary>
        /// name constructor
        /// </summary>
        /// <param name="name">name of native function to be used</param>
        public IntrinsicFunctionAttribute(string name) : base(name) { }

        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="name">name of native function to be used</param>
        /// <param name="flavor">flavor</param>
        public IntrinsicFunctionAttribute(string name, int flavor)
            : base(name)
        {
            Flavor = flavor;
        }
    }

    /// <summary>
    /// rename intrinsic function -- allows dotnet property to be named differently from native version
    /// </summary>
    [Guid("F796E790-522B-4ECE-A25F-07BF145A938A")]
    public class IntrinsicRenameAttribute : IntrinsicAttribute
    {
        /// <summary>
        /// is function ? 
        /// </summary>
        public override bool IsFunction { get { return false; } set { } }
        /// <summary>
        /// default constructor
        /// </summary>
        public IntrinsicRenameAttribute() : base() { }
        /// <summary>
        /// constructor from name
        /// </summary>
        /// <param name="name">native name</param>
        public IntrinsicRenameAttribute(string name) : base(name) { }
    }

    /// <summary>
    /// Variables of this type can only be assigned once (at their declaration)
    /// The developer is responsible to ensure it's actually the case
    /// </summary>
    [Guid("13C2E66D-9965-4718-90A3-C284D0712A82")]
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Interface)]
    public class SingleStaticAssignmentAttribute : Attribute { }

    /// <summary>
    /// Types marked as intrinsic -- user shall provide an implementation
    /// </summary>
    /// <example>
    /// <code>
    /// [IntrinsicType("half2")]
    /// struct float2 {
    ///    ...
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="IntrinsicIncludeAttribute"/>
    [Guid("AFF70C75-30C4-4598-88D2-74CD3C212111")]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class IntrinsicTypeAttribute : IntrinsicAttribute
    {
        /// <summary>
        /// is function ? -- always false
        /// </summary>
        public override bool IsFunction { get { return false; } set { } }

        /// <summary>
        /// AVX specific -- is type vectorizable?
        /// </summary>
        public bool NotVectorizable { get; set; }

        /// <summary>
        /// Provide vectorized type if type is vectorizable
        /// </summary>
        public Type VectorizedType { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public IntrinsicTypeAttribute() { }

        /// <summary>
        /// constructor for name
        /// </summary>
        /// <param name="name">name of native type to be used</param>
        public IntrinsicTypeAttribute(string name) : base(name) { }

        /// <summary>
        /// handle this type as a value type (not a pointer)
        /// </summary>
        public bool HandleAsValueType { get; set; }

        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="name">name of native type to be used</param>
        /// <param name="flavor">flavor</param>
        public IntrinsicTypeAttribute(string name, int flavor)
            : base(name)
        {
            Flavor = flavor;
        }
    }

#pragma warning disable 1591
    /// <summary>
    /// Types that can be newed on the device using hybridizer::allocate&lt;T&gt;()
    /// </summary>
    [Guid("B75F9B6D-0A26-46F9-9142-5B98B7314535")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AllocatableTypeAttribute : IntrinsicAttribute
    {
        public override bool IsFunction { get { return false; } set { } }
        public bool NotVectorizable { get; set; }
        public string AllocatorTemplateName { get; set; }
        public AllocatableTypeAttribute(string name) { AllocatorTemplateName = name; }
        public AllocatableTypeAttribute() { AllocatorTemplateName = "hybridizer::defaultallocator"; }
    }

    [Guid("33AF19F3-A2C0-48DD-828C-0A594788250C")]
    public class IntrinsicPrimitiveAttribute : IntrinsicAttribute
    {
        public override bool IsFunction { get { return false; } set { } }
        public IntrinsicPrimitiveAttribute() { }
        public IntrinsicPrimitiveAttribute(string name) : base(name) { }
    }
#pragma warning restore 1591

    /// <summary>
    /// Compile time constant
    /// </summary>
    [Guid("6F69FCF7-D388-4ED1-9C9C-458C2C2A67C3")]
    public class IntrinsicConstantAttribute : IntrinsicAttribute
    {
        /// <summary>
        /// is function ? -- always false
        /// </summary>
        public override bool IsFunction { get { return false; } set { } }

        /// <summary>
        /// default constructor
        /// </summary>
        public IntrinsicConstantAttribute() { }

        /// <summary>
        /// name constructor
        /// </summary>
        /// <param name="name">name of constant value to be used</param>
        public IntrinsicConstantAttribute(string name) : base(name) { }
    }

    /// <summary>
    /// Register class as a template concept
    /// </summary>
    [Guid("B6337182-7B1C-4DB5-B04E-7ABA795CD46F")]
    public class HybridTemplateConceptAttribute : Attribute
    {
    }

    /// <summary>
    /// type to specialize template concept
    /// </summary>
    [Guid("C99230C6-B39C-4A07-AA00-AC23885C1FA5")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,
                   AllowMultiple = true)]
    public class HybridRegisterTemplateAttribute : Attribute
    {
        /// <summary>
        /// type to specialize with
        /// </summary>
        public Type Specialize { get; set; }
    }

    /// <summary>
    /// Use this attribute to customize the typeId of one type
    /// </summary>
    [Guid("989DE99F-1C18-4E7F-9509-D4D0A70B9FA7")]
    public class TypeIdAttribute : Attribute
    {
        private string _typeId;
        /// <summary>
        /// typeid to be used
        /// </summary>
        new public string TypeId { get { return _typeId; } set { _typeId = value; } }

        /// <summary>
        /// default constructor
        /// </summary>
        public TypeIdAttribute()
        {
        }

        /// <summary>
        /// full constructor
        /// </summary>
        public TypeIdAttribute(string typeId)
        {
            _typeId = typeId;
        }
    }

#pragma warning disable 1591
    [Guid("7460008E-FAF1-4A6D-8784-5FA3058A61E6")]
    public class KnownReturnTypeAttribute : Attribute
    {
        public Type ReturnType { get; set; }
        public KnownReturnTypeAttribute()
        {
        }
        public KnownReturnTypeAttribute(Type t)
        {
            this.ReturnType = t;
        }
    }

    [Guid("E5CA6846-7A9A-40B6-8715-57B698693C6E")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class HybridizerTemplatizeArgumentAttribute : Attribute { }
#pragma warning restore 1591
    #endregion

    #region MEMORY

    /// <summary>
    /// constant memory location
    /// <seealso cref="HybridConstantAttribute"/>
    /// </summary>
    public enum ConstantLocation
    {
        /// <summary>
        /// Constant memory
        /// </summary>
        ConstantMemory,
        /// <summary>
        /// global memory
        /// </summary>
        GlobalMemory,
        /// <summary>
        /// reserved
        /// </summary>
        LocalDeclaration
    }

    /// <summary>
    /// Defines a constant value
    /// </summary>
    /// <example>
    /// <code>
    /// public class ConstantsDeclaration {
    ///     [HybridConstant(Location = ConstantLocation.ConstantMemory)]
    ///     public static float[] data = { -2.0F, -1.0F, 0.0F, 1.0F, 2.0F };
    /// }
    /// </code>
    /// Complete example <see href="https://github.com/altimesh/hybridizer-basic-samples/blob/master/HybridizerBasicSamples_CUDA100/5.CUDA%20runtime/ConstantMemory/ConstantMemory/Program.cs">on github</see>
    /// </example>
    [Guid("6FD77541-3CF0-4BD0-ACD0-A7DC051AD9CE")]
    public class HybridConstantAttribute : Attribute
    {
        private ConstantLocation _location;
        /// <summary>
        /// memory location to be used
        /// <see cref="ConstantLocation"/>
        /// </summary>
        public ConstantLocation Location { get { return _location; } set { _location = value; } }

        private int _maxSize = -1;

        /// <summary>
        /// Maximum size (in elements) of this constant array
        /// </summary>
        public int MaxSize { get { return _maxSize; } set { _maxSize = value; } }

        private string _symbol;
        /// <summary>
        /// Symbol to be used in native code - be carefull to avoid naming conflicts as no mangling is applied
        /// </summary>
        public string Symbol { get { return _symbol; } set { _symbol = value; } }

        /// <summary>
        /// Instead of a constant memory variable, a macro define will b egenerated.
        /// Please note that this can only work when the value is known at compile time.
        /// </summary>
        public bool IsDefine { get; set; }

        /// <summary>
        /// default constructor (GlobalMemory)
        /// </summary>
        public HybridConstantAttribute()
        {
            _location = ConstantLocation.GlobalMemory;
        }

        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="location"></param>
        public HybridConstantAttribute(ConstantLocation location)
        {
            _location = location;
        }
    }

#pragma warning disable 1591
    [Guid("92A2755E-651C-4EFB-B2EF-AD5D2D30408C")]
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class ReadOnlyAttribute : Attribute
    {
    }

    [Guid("DA37DA90-BE0C-4318-A34F-1136D50036EF")]
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class WriteOnlyAttribute : Attribute
    {
    }
#pragma warning restore 1591

    #endregion

    #region INCLUDES
    /// <summary>
    /// intrinsic include -- CUDA specific 
    /// <seealso cref="IntrinsicIncludeAttribute"/>
    /// </summary>
    [Guid("4BE24B23-57BC-476C-8D1E-4389F7B68227")]
    public class IntrinsicIncludeCUDAAttribute : Attribute
    {
#pragma warning disable 1591
        string _filename;
        public string FileName { get { return _filename; } set { _filename = value; } }
        bool _itdin;
        public bool IsTypeDeclaredInHeader { get { return _itdin; } set { _itdin = value; } }
        public IntrinsicIncludeCUDAAttribute() { }
        public IntrinsicIncludeCUDAAttribute(string filename) { _filename = filename; }
#pragma warning restore 1591
    }

    /// <summary>
    /// Force include of some native header
    /// </summary>
    /// <example>
    /// <code>
    /// [IntrinsicInclude("&lt;cuda_fp16.h&gt;")]
    /// [IntrinsicType("half2")]
    /// struct half2 {
    ///  ... 
    /// }
    /// </code>
    /// </example>
    [Guid("6409211D-61C5-4629-90BB-8BDB6E44694B"), AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = true)]
    public class IntrinsicIncludeAttribute : IntrinsicAttribute
    {
        string _filename;
        /// <summary>
        /// file name to include
        /// </summary>
        public string FileName { get { return _filename; } set { _filename = value; } }
        /// <summary>
        ///  is function ? always false
        /// </summary>
        public override bool IsFunction { get { return false; } set { } }
        bool _itdin;
        /// <summary>
        /// is type declared in header?
        /// </summary>
        public bool IsTypeDeclaredInHeader { get { return _itdin; } set { _itdin = value; } }
        /// <summary>
        /// default constructor
        /// </summary>
        public IntrinsicIncludeAttribute() { }
        /// <summary>
        /// full constructor
        /// </summary>
        public IntrinsicIncludeAttribute(string filename) { _filename = filename; }
    }
    #endregion

    #region MARSHALLING

    /// <summary>
    /// Assembly attribute providing name of generated satellite library
    /// </summary>
    [Guid("DB47194F-E116-409A-88F8-634AC19E4A15")]
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class HybRunnerDefaultSatelliteNameAttribute : Attribute
    {
        private string _name;
        /// <summary>
        /// Name of satellite library
        /// </summary>
        /// <example>
        /// <code>
        /// // allow parameterless constructor for HybRunner
        /// [assembly: HybRunnerDefaultSatelliteName("HybridizerSample15_CUDA.dll")]
        /// // allowing HybRunner runner = HybRunner.Cuda(); 
        /// </code>
        /// </example>
        public string Name { get { return _name; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public HybRunnerDefaultSatelliteNameAttribute(string name)
        {
            _name = name;
        }
    }

    /// <summary>
    /// Resident array -- user must manually control memory location
    /// </summary>
    [Guid("A44E17E1-D5CF-4B50-8E40-ABF9C18DCB74")]
    public interface IResidentArray
    {
        /// <summary>
        /// memory status
        /// </summary>
        ResidentArrayStatus Status { get; set; }

        /// <summary>
        /// device pointer
        /// </summary>
        IntPtr DevicePointer { get; }

        /// <summary>
        /// host pointer
        /// </summary>
        IntPtr HostPointer { get; }

        /// <summary>
        /// moves memory to host
        /// </summary>
        void RefreshHost();

        /// <summary>
        /// moves memory to device
        /// </summary>
        void RefreshDevice();
    }

    /// <summary>
    /// custom marshaler
    /// </summary>
    [Guid("7B4E8768-BB70-4088-ABC1-DBBA55F370F5")]
    public interface ICustomMarshalled
    {
        /// <summary>
        /// marshal to native
        /// </summary>
        void MarshalTo(BinaryWriter bw, HybridizerFlavor flavor);

        /// <summary>
        /// UnMarshal from native
        /// </summary>
        void UnmarshalFrom(BinaryReader br, HybridizerFlavor flavor);
    }

    /// <summary>
    /// custom marshaler
    /// </summary>
    [Guid("9E018A3A-4C04-4276-B072-D48981C31A12")]
    public interface IHybCustomMarshaler
    {
        /// <summary>
        /// marshal to native
        /// </summary>
        void MarshalTo(object value, BinaryWriter bw, AbstractNativeMarshaler marshaler);

        /// <summary>
        /// UnMarshal from native
        /// </summary>
        void UnmarshalFrom(object value, BinaryReader br, AbstractNativeMarshaler marshaler);

        /// <summary>
        /// provides size
        /// </summary>
        long SizeOf(object customMarshalled);
    }

    /// <summary>
    /// size of marshalled structure
    /// </summary>
    /// <remarks>
    /// Mandatory when type implements <see cref="ICustomMarshalled" />
    /// </remarks>
    [Guid("19B45042-6B32-420C-AEB2-2ACE70D23531")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ICustomMarshalledSize : Attribute
    {
        /// <summary>
        /// size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public ICustomMarshalledSize() { }

        /// <summary>
        /// full constructor
        /// </summary>
        public ICustomMarshalledSize(int size)
        {
            Size = size;
        }
    }

    /// <summary>
    /// Resident data interface
    /// </summary>
    [Guid("619C83F7-2415-47D3-9A0B-0611B0478FD7")]
    public interface IResidentData : IResidentArray
    {
        /// <summary>
        /// Total size in bytes
        /// </summary>
        long Size { get; }
    }


    /// <summary>
    /// Memory status of resident array
    /// <see cref="IResidentArray"/>
    /// </summary>
    [Guid("BD8F8FC3-29A8-4FA8-A5CF-8D6A42A3EFFB")]
    public enum ResidentArrayStatus
    {
        /// <summary>
        /// device and host are up-to-date
        /// </summary>
        NoAction,
        /// <summary>
        /// Host memory has changed and not propagated to device
        /// </summary>
        DeviceNeedsRefresh,
        /// <summary>
        /// device memory has changed and not propagated to host
        /// </summary>
        HostNeedsRefresh,
    }

    /// <summary>
    /// </summary>
    [Guid("AF21F70B-A761-4C48-A882-806465214394")]
    public class ResidentArrayHostAttribute : Attribute
    {
    }

    #endregion

    #region OPTIX

    /// <summary>
    /// Kernel is an optix shader - special treatment required
    /// <see href="https://docs.nvidia.com/gameworks/content/gameworkslibrary/optix/optix_v4_0.htm"/>
    /// </summary>
    [Guid("CE5F18FA-4477-4836-A38C-BE3E7CD8B9DD")]
    public class OptixShaderAttribute : EntryPointAttribute
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public OptixShaderAttribute() : base() { }
        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="name"></param>
        public OptixShaderAttribute(string name) : base(name) { }
    }

    #endregion

    #region LLVM
    /// <summary>
    /// Use to wrap a constant array
    /// <example>
    /// public static int[] aa = { 1, 2 };
    /// [HybridizerNativeFieldProxy("aa")]
    /// public static int* a;
    /// </example>
    /// </summary>
    [Guid("958B5745-84C2-4124-8A85-0A6F899C6450")]
    public class HybridizerNativeFieldProxyAttribute : Attribute
    {
        /// <summary>
        /// name of underlying array
        /// </summary>
        public string UnderlyingArrayName { get; set; }
        /// <summary>
        /// default constructor
        /// </summary>
        public HybridizerNativeFieldProxyAttribute() { }
        /// <summary>
        /// full constructor
        /// </summary>
        public HybridizerNativeFieldProxyAttribute(string underlyingArrayName)
        {
            UnderlyingArrayName = underlyingArrayName;
        }
    }

    /// <summary>
    /// LLVM as input only
    /// With this attribute, no code will be generated
    /// Provided .Net method will be used instead in generated assembly
    /// </summary>
    [Guid("4782C5C4-4DED-4AD5-A3DF-0DFF3CB2A2C5")]
    public class BuiltinMethodAttribute : Attribute
    {
        /// <summary>
        /// native method name
        /// </summary>
        public string NativeName { get; set; }
        /// <summary>
        /// default constructor
        /// </summary>
        public BuiltinMethodAttribute() { NativeName = null; }
        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="name"></param>
        public BuiltinMethodAttribute(string name) { NativeName = name; }
    }

    /// <summary>
    /// LLVM as input only
    /// Mark a parameter as const
    /// </summary>
    [Guid("A9D84C49-CE4E-44B4-9430-9118720E5A64")]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ConstAttribute : Attribute
    {
    }

    /// <summary>
    /// LLVM as input only
    /// Native symbol shoud use provided .Net method
    /// </summary>
    /// <example>
    /// <code>
    /// [IntrinsicFunction(IsNaked=true, Name="__threadfence_block"), NativeImportSymbol("__threadfence_block")]
    /// public static void __threadfence_block()
    /// {
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="IntrinsicIncludeAttribute"/>
    [Guid("48220DAE-4FF4-4BCF-8FF5-69890EA76E13")]
    [AttributeUsage(AttributeTargets.Method)]
    public class NativeImportSymbolAttribute : Attribute
    {
        /// <summary>
        /// native symbol
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// </summary>
        public bool IsOverride { get; set; }

        /// <summary>
        /// </summary>
        public NativeImportSymbolAttribute() { }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="symbol"></param>
        public NativeImportSymbolAttribute(string symbol)
        {
            Symbol = symbol;
        }

        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="isOverride"></param>
        public NativeImportSymbolAttribute(string symbol, bool isOverride)
        {
            Symbol = symbol;
            IsOverride = isOverride;
        }
    }

#pragma warning disable 1591
    [Guid("3804A1E3-E03D-4A2E-B2BA-0BF08F75469F")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NativeImportHeaderAttribute : Attribute
    {
        public string Header { get; set; }

        public NativeImportHeaderAttribute() { }
        public NativeImportHeaderAttribute(string header)
        {
            Header = header;
        }
    }
#pragma warning restore 1591

    #endregion

    #region OMP AVX and PHI

    /// <summary>
    ///  force vectorization of method parameter
    /// </summary>
    [Guid("A9547890-397B-4D19-B1BD-0D6975F62222")]
    public class HybridVectorizerAttribute : Attribute
    {
    }

    /// <summary>
    /// intrinsic include -- OMP specific
    /// <seealso cref="IntrinsicIncludeAttribute"/>
    /// </summary>
    [Guid("7DB811ED-A881-43D7-A77C-A6468FD2CB07")]
    public class IntrinsicIncludeOMPAttribute : Attribute
    {
#pragma warning disable 1591
        string _filename;
        public string FileName { get { return _filename; } set { _filename = value; } }
        bool _itdin;
        public bool IsTypeDeclaredInHeader { get { return _itdin; } set { _itdin = value; } }
        public IntrinsicIncludeOMPAttribute() { }
        public IntrinsicIncludeOMPAttribute(string filename) { _filename = filename; }
#pragma warning restore 1591
    }

    /// <summary>
    /// intrinsic include -- AVX specific
    /// <seealso cref="IntrinsicIncludeAttribute"/>
    /// </summary>
    [Guid("FF753B09-C978-46D0-A123-A1F40532D123")]
    public class IntrinsicIncludePhiAttribute : Attribute
    {
#pragma warning disable 1591
        string _filename;
        public string FileName { get { return _filename; } set { _filename = value; } }
        public IntrinsicIncludePhiAttribute() { }
        public IntrinsicIncludePhiAttribute(string filename) { _filename = filename; }
#pragma warning restore 1591
    }



    /// <summary>
    /// INTERNAL TYPE
    /// </summary>
    [Guid("5E2494A7-CEB2-42B4-B6BB-EFE6B08176CA")]
    public struct VectorizerMask
    {
    }

    /// <summary>
    /// vectorization hint for return type
    /// </summary>
    [Guid("80F6B6AD-F592-42E4-A7A4-EC121403D5AD")]
    public enum VectorizerIntrinsicReturn : int
    {
        /// <summary>
        /// unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// return type is same as argument
        /// </summary>
        Transitive,
        /// <summary>
        /// return type has same vectorization pattern as argument (float -> int becomes float&lt;&gt; -&gt; int&lt;&gt;)
        /// </summary>
        VectorTransitive,
        /// <summary>
        /// return type is unchanged whatsoever (reducers for example)
        /// </summary>
        Unchanged,
        /// <summary>
        /// return type is always vectorized
        /// </summary>
        Vectorized,   
    }

    /// <summary>
    /// obsolete
    /// <see cref="ReturnTypeInferenceAttribute"/>
    /// </summary>
    [Guid("AE20EFB0-273F-4DF1-B939-8452BAFCCE6C")]
    [Obsolete("Typo in this attributes type name, use ReturnTypeInferenceAttribute instead")]
    public class ReturnTypeInferrenceAttribute : Attribute
    {
#pragma warning disable 1591
        int _index = 0;
        public int Index { get { return _index; } set { _index = value; } }
        public VectorizerIntrinsicReturn Return { get; set; }
        public ReturnTypeInferrenceAttribute() { }
        public ReturnTypeInferrenceAttribute(VectorizerIntrinsicReturn ret) { Return = ret; }
#pragma warning restore 1591
    }

    /// <summary>
    /// hint for return type vectorization
    /// </summary>
    [Guid("AE20EFB0-273F-4DF1-B939-8452BAFCCE6C")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true)]
    public class ReturnTypeInferenceAttribute : Attribute
    {
        int _index = 0;
        /// <summary>
        /// internal
        /// </summary>
        public int Index { get { return _index; } set { _index = value; } }
        /// <summary>
        /// vectorization type
        /// </summary>
        public VectorizerIntrinsicReturn Return { get; set; }
        /// <summary>
        /// default constructor
        /// </summary>
        public ReturnTypeInferenceAttribute() { }
        /// <summary>
        /// </summary>
        public ReturnTypeInferenceAttribute(VectorizerIntrinsicReturn ret) { Return = ret; }
        /// <summary>
        /// </summary>
        public ReturnTypeInferenceAttribute(VectorizerIntrinsicReturn ret, int index)
        {
            Return = ret;
            Index = index;
        }

        /// <summary>
        /// </summary>
        public ReturnTypeInferenceAttribute(int ret, int index)
        {
            Return = (VectorizerIntrinsicReturn)ret;
            Index = index;
        }
    }

    /// <summary>
    /// Naked function : no memory operation
    /// Allows optimizations
    /// </summary>
    [Guid("E91E5D25-EFB1-4AE3-90B9-A2F33127B0EE")]
    public class HybridNakedFunctionAttribute : Attribute
    {
    }

    /// <summary>
    /// Arithmetic function : no memory operations and no branches
    /// Allows aggressive optimizations
    /// </summary>
    [Guid("103631AE-32AC-4E25-AFD9-94AFB7EBF904")]
    public class HybridArithmeticFunctionAttribute : Attribute
    {
    }

    /// <summary>
    /// internal
    /// </summary>
    [Guid("B0B5537C-5E68-49FF-9B61-7481EA07958E")]
    public class HybridHybOpFunctionAttribute : Attribute
    {
    }

    /// <summary>
    /// Fallback when vectorization fails
    /// Method signature is vectorized, but implementation is serial
    /// </summary>
    [Guid("8123D16F-93F8-423F-8573-55AACCBB70D5")]
    public class SerialVectorizeAttribute : Attribute
    {
    }

    /// <summary>
    /// dot no vectorize
    /// </summary>
    [Guid("02196684-8AC0-4A18-94CA-E3F6C38DECDD")]
    public class DontVectorizeAttribute : Attribute
    {
    }

    #endregion

    #region StackAlloc Behavior

    /// <summary>
    /// Stack allocation behavior
    /// </summary>
    public enum StackAllocBehaviorEnum
    {
        /// <summary>
        /// each vector lane has an instance of the stack allocation 
        /// </summary>
        PerLane,
        /// <summary>
        /// stack allocation is share amongst lanes
        /// </summary>
        BlockShared,
        /// <summary>
        /// if not specified
        /// </summary>
        Default = PerLane 
    }

    /// <summary>
    /// specifies stack allocation behavior
    /// </summary>
    [Guid("FD3A08B9-4322-419F-A456-169C3249E11A")]
    public class StackAllocBehaviorAttribute : Attribute
    {
        /// <summary>
        /// behavior
        /// </summary>
        public StackAllocBehaviorEnum Behavior { get; set; }
        /// <summary>
        /// constructor
        /// </summary>
        public StackAllocBehaviorAttribute() { Behavior = StackAllocBehaviorEnum.Default; }
    }

    #endregion

#pragma warning disable 1591
    #region SPLITTER

    [Guid("18353160-09EA-47ED-B128-2B77A68753BA")]
    public class HybridizerForceIgnoreAttribute : Attribute
    {
    }

    [Guid("0A131C43-BC38-4159-BA69-A405EC808DA5")]
    public class HeapifyLocalsAttribute : Attribute
    {
    }

    #endregion
    #region JAVA

    [Guid("83CA44E1-92CA-478B-8520-D3DBBAE50F48")]
    public class HybridMappedJavaTypeAttribute : Attribute
    {
        private string _mappedType;
        public string MappedType { get { return _mappedType; } set { _mappedType = value; } }

        public HybridMappedJavaTypeAttribute() { }

        public HybridMappedJavaTypeAttribute(string mappedType)
        {
            _mappedType = mappedType;
        }
    }

    [Guid("4F583484-17FC-4987-B5B7-95D98184227A")]
    [AttributeUsage(AttributeTargets.Method)]
    public class HybridJavaImplementationAttribute : Attribute
    {
        private string _javaCode;
        public string JavaCode { get { return _javaCode; } set { _javaCode = value; } }

        public HybridJavaImplementationAttribute() { }

        public HybridJavaImplementationAttribute(string javaCode)
        {
            _javaCode = javaCode;
        }
    }

    #endregion
#pragma warning restore 1591

    #region HVL

    /// <summary>
    /// HVL only
    /// Lambdas must have a unique identifier to be used in HVL
    /// </summary>
    [Guid("A8D0758A-2DA7-4664-840C-0BCAE1430689")]
    public class HybridLambdaIdentifierAttribute : Attribute
    {
        /// <summary>
        /// identifier
        /// </summary>
        /// <seealso cref="System.Guid"/>
        public Guid guid { get; set; }
        /// <summary>
        /// default constructor
        /// </summary>
        public HybridLambdaIdentifierAttribute()
        {
            guid = new Guid();
        }

        /// <summary>
        /// full constructor
        /// </summary>
        /// <param name="gid">must be a valid GUID</param>
        public HybridLambdaIdentifierAttribute(string gid)
        {
            guid = Guid.Parse(gid);
        }
    }

    #endregion
}
