using System.Runtime.InteropServices;

namespace Hybridizer.Runtime.CUDAImports
{
    /// <summary>
    /// 2 32 bits float, packed
    /// </summary>
    [IntrinsicType("float2")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct float2
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
        /// conversion to signed 64 bits integer
        /// </summary>
        /// <param name="res"></param>
        public static explicit operator long(float2 res)
        {
            long* tmp = (long*)(&res);
            return *tmp;
        }

        /// <summary>
        /// conversion to int2
        /// </summary>
        public static explicit operator int2(float2 res)
        {
            int2* tmp = (int2*)(&res);
            return *tmp;
        }

        /// <summary>
        /// conversion to double
        /// </summary>
        public static explicit operator double(float2 res)
        {
            double* tmp = (double*)(&res);
            return *tmp;
        }

        /// <summary>
        /// conversion to short4
        /// </summary>
        public static explicit operator short4(float2 res)
        {
            short4* tmp = (short4*)(&res);
            return *tmp;
        }

        /// <summary>
        /// conversion to char8
        /// </summary>
        public static explicit operator char8(float2 res)
        {
            char8* tmp = (char8*)(&res);
            return *tmp;
        }

        /// <summary>
        /// constructor from 2 individual 32 bits floats
        /// </summary>
        public float2(float xx, float yy)
        {
            x = xx;
            y = yy;
        }

        /// <summary>
        /// make a float2 from 2 floats
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <returns></returns>
        [IntrinsicFunction("make_float2")]
        public static float2 make_float2(float xx, float yy)
        {
            return new float2(xx, yy);
        }

        /// <summary>
        /// constructor from int2
        /// </summary>
        public float2(int2 val)
        {
            x = (float)val.x;
            y = (float)val.y;
        }

        /// <summary>
        /// addition operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator+")]
        public static float2 operator +(float2 a, float2 b)
        {
            float2 res;
            res.x = a.x + b.x;
            res.y = a.y + b.y;
            return res;
        }

        /// <summary>
        /// addition operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator+")]
        public static float2 operator +(float a, float2 b)
        {
            float2 res;
            res.x = a + b.x;
            res.y = a + b.y;
            return res;
        }

        /// <summary>
        /// addition operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator+")]
        public static float2 operator +(float2 a, float b)
        {
            float2 res;
            res.x = a.x + b;
            res.y = a.y + b;
            return res;
        }

        /// <summary>
        /// substraction operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator-")]
        public static float2 operator -(float2 a, float2 b)
        {
            float2 res;
            res.x = a.x - b.x;
            res.y = a.y - b.y;
            return res;
        }

        /// <summary>
        /// substraction operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator-")]
        public static float2 operator -(float a, float2 b)
        {
            float2 res;
            res.x = a - b.x;
            res.y = a - b.y;
            return res;
        }

        /// <summary>
        /// substraction operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator-")]
        public static float2 operator -(float2 a, float b)
        {
            float2 res;
            res.x = a.x - b;
            res.y = a.y - b;
            return res;
        }

        /// <summary>
        /// multiplication operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator*")]
        public static float2 operator *(float2 a, float2 b)
        {
            float2 res;
            res.x = a.x * b.x;
            res.y = a.y * b.y;
            return res;
        }

        /// <summary>
        /// multiplication operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator*")]
        public static float2 operator *(float a, float2 b)
        {
            float2 res;
            res.x = a * b.x;
            res.y = a * b.y;
            return res;
        }

        /// <summary>
        /// multiplication operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator*")]
        public static float2 operator *(float2 a, float b)
        {
            float2 res;
            res.x = a.x * b;
            res.y = a.y * b;
            return res;
        }

        /// <summary>
        /// division operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator/")]
        public static float2 operator /(float2 a, float2 b)
        {
            float2 res;
            res.x = a.x / b.x;
            res.y = a.y / b.y;
            return res;
        }

        /// <summary>
        /// division operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator/")]
        public static float2 operator /(float a, float2 b)
        {
            float2 res;
            res.x = a / b.x;
            res.y = a / b.y;
            return res;
        }

        /// <summary>
        /// division operator
        /// </summary>
        [IntrinsicFunction(IsNaked = true, Name = "operator/")]
        public static float2 operator /(float2 a, float b)
        {
            float2 res;
            res.x = a.x / b;
            res.y = a.y / b;
            return res;
        }

        /// <summary>
        /// stores in memory
        /// </summary>
        /// <param name="ptr">destination pointer</param>
        /// <param name="val">value to store</param>
        /// <param name="alignment">has to be a compile time constant</param>
        [IntrinsicFunction(IsNaked = true, Name = "__hybridizer_store_float2")]
        public unsafe static void Store(float2* ptr, float2 val, int alignment)
        {
            *ptr = val;
        }

        /// <summary>
        /// loads from memory
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="alignment">has to be a compile time constant</param>
        [IntrinsicFunction(IsNaked = true, Name = "__hybridizer_load_float2")]
        public unsafe static float2 Load(float2* ptr, int alignment)
        {
            return *ptr;
        }
    }
}