using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// 4 32 bits floats
    /// </summary>
    [IntrinsicType("float4")]
    [IntrinsicPrimitive("float4")]
    [StructLayout(LayoutKind.Explicit)]
    public struct float4
    {
        /// <summary>
        /// x
        /// </summary>
        [FieldOffset(0)]
        public float x;
        /// <summary>
        /// y
        /// </summary>
        [FieldOffset(4)]
        public float y;
        /// <summary>
        /// z
        /// </summary>
        [FieldOffset(8)]
        public float z;
        /// <summary>
        /// w
        /// </summary>
        [FieldOffset(12)]
        public float w;

        /// <summary>
        /// copy constructor
        /// </summary>
        public float4(float4 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
            w = other.w;
        }

        /// <summary>
        /// constructor from components
        /// </summary>
        public float4(float xx, float yy, float zz, float ww)
        {
            x = xx;
            y = yy;
            z = zz;
            w = ww;
        }

        /// <summary>
        /// make a float4 from 4 floats
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="zz"></param>
        /// <param name="ww"></param>
        /// <returns></returns>
        [IntrinsicFunction("make_float4")]
        public static float4 make_float4(float xx, float yy, float zz, float ww)
        {
            return new float4(xx, yy, zz, ww);
        }

        /// <summary>
        /// addition operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator+")]
        public static float4 operator +(float4 a, float4 b)
        {
            float4 res;
            res.x = a.x + b.x;
            res.y = a.y + b.y;
            res.z = a.z + b.z;
            res.w = a.w + b.w;
            return res;
        }

        /// <summary>
        /// addition operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator+")]
        public static float4 operator +(float a, float4 b)
        {
            float4 res;
            res.x = a + b.x;
            res.y = a + b.y;
            res.z = a + b.z;
            res.w = a + b.w;
            return res;
        }

        /// <summary>
        /// addition operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator+")]
        public static float4 operator +(float4 a, float b)
        {
            float4 res;
            res.x = a.x + b;
            res.y = a.y + b;
            res.z = a.z + b;
            res.w = a.w + b;
            return res;
        }

        /// <summary>
        /// substraction operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator-")]
        public static float4 operator -(float4 a, float4 b)
        {
            float4 res;
            res.x = a.x - b.x;
            res.y = a.y - b.y;
            res.z = a.z - b.z;
            res.w = a.w - b.w;
            return res;
        }

        /// <summary>
        /// substraction operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator-")]
        public static float4 operator -(float a, float4 b)
        {
            float4 res;
            res.x = a - b.x;
            res.y = a - b.y;
            res.z = a - b.z;
            res.w = a - b.w;
            return res;
        }

        /// <summary>
        /// substraction operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator-")]
        public static float4 operator -(float4 a, float b)
        {
            float4 res;
            res.x = a.x - b;
            res.y = a.y - b;
            res.z = a.z - b;
            res.w = a.w - b;
            return res;
        }

        /// <summary>
        /// multiplication operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator*")]
        public static float4 operator *(float4 a, float4 b)
        {
            float4 res;
            res.x = a.x * b.x;
            res.y = a.y * b.y;
            res.z = a.z * b.z;
            res.w = a.w * b.w;
            return res;
        }

        /// <summary>
        /// multiplication operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator*")]
        public static float4 operator *(float a, float4 b)
        {
            float4 res;
            res.x = a * b.x;
            res.y = a * b.y;
            res.z = a * b.z;
            res.w = a * b.w;
            return res;
        }

        /// <summary>
        /// multiplication operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator*")]
        public static float4 operator *(float4 a, float b)
        {
            float4 res;
            res.x = a.x * b;
            res.y = a.y * b;
            res.z = a.z * b;
            res.w = a.w * b;
            return res;
        }

        /// <summary>
        /// division operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator/")]
        public static float4 operator /(float4 a, float4 b)
        {
            float4 res;
            res.x = a.x / b.x;
            res.y = a.y / b.y;
            res.z = a.z / b.z;
            res.w = a.w / b.w;
            return res;
        }

        /// <summary>
        /// division operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator/")]
        public static float4 operator /(float a, float4 b)
        {
            float4 res;
            res.x = a / b.x;
            res.y = a / b.y;
            res.z = a / b.z;
            res.w = a / b.w;
            return res;
        }

        /// <summary>
        /// division operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator/")]
        public static float4 operator /(float4 a, float b)
        {
            float4 res;
            res.x = a.x / b;
            res.y = a.y / b;
            res.z = a.z / b;
            res.w = a.w / b;
            return res;
        }

        /// <summary>
        /// Stores in memory
        /// </summary>
        /// <param name="ptr">destination pointer</param>
        /// <param name="val">value to store</param>
        /// <param name="alignment">has to be a compile time constant</param>
        [IntrinsicFunction(IsNaked = true, Name = "__hybridizer_store_float4")]
        public unsafe static void Store(float4* ptr, float4 val, int alignment)
        {
            *ptr = val;
        }

        /// <summary>
        /// loads from memory
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="alignment">has to be a compile time constant</param>
        [IntrinsicFunction(IsNaked = true, Name = "__hybridizer_load_float4")]
        public unsafe static float4 Load(float4* ptr, int alignment)
        {
            return *ptr;
        }
    }
}