/* (c) ALTIMESH 2026 -- all rights reserved */
//
// nvcuda::wmma bindings for Hybridizer.
//
// Phase 1 scope (see https://docs.nvidia.com/cuda/cuda-c-programming-guide/index.html#wmma):
//   * half input  (16,16,16), (32,8,16), (8,32,16)  -> {half, float} accumulators (sm_70+)
//   * tf32 input  (16,16,8)                         -> float accumulator           (sm_80+)
//   * double      (8,8,4)                           -> double accumulator          (sm_80+)
//
// Deferred to follow-up PRs:
//   * bfloat16 (needs a bfloat16 type added to CudaFP16.cs first)
//   * int8/uint8 with int accumulator
//   * subbyte (s4/u4) and binary (b1) -- deprecated in CUDA 13, explicitly out of scope.
//
// Per-element access `frag[i]` is provided via [IntrinsicFieldSubscript("x")],
// which the Hybridizer transcoder maps to (frag)->x[i] in the emitted C++.
// Useful for custom epilogues that touch fragment elements directly (e.g. a
// fused activation between mma_sync and store_matrix_sync).
//
using System;

#pragma warning disable 1591
namespace Hybridizer.Runtime.CUDAImports.wmma
{
    #region Tag types

    /// <summary>Marker type for fragment&lt;matrix_a, ...&gt;.</summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::matrix_a")]
    public struct matrix_a { }

    /// <summary>Marker type for fragment&lt;matrix_b, ...&gt;.</summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::matrix_b")]
    public struct matrix_b { }

    /// <summary>Marker type for fragment&lt;accumulator, ...&gt;.</summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::accumulator")]
    public struct accumulator { }

    /// <summary>Layout tag: row-major.</summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::row_major")]
    public struct row_major { }

    /// <summary>Layout tag: column-major.</summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::col_major")]
    public struct col_major { }

    /// <summary>TensorFloat-32 precision tag (sm_80+).</summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::precision::tf32")]
    public struct tf32 { }

    /// <summary>
    /// Runtime layout flag passed to load/store of accumulator fragments.
    /// </summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::layout_t")]
    public enum wmma_layout
    {
        [IntrinsicConstant("nvcuda::wmma::mem_row_major")] mem_row_major = 0,
        [IntrinsicConstant("nvcuda::wmma::mem_col_major")] mem_col_major = 1,
    }

    #endregion

    #region Fragment structs

    // ---------------------------------------------------------------------
    // Convention:
    //   frag_<role>_<M>x<N>x<K>_<T>[_<layout>]
    //     role   : a | b | acc
    //     M,N,K  : shape
    //     T      : half | float | tf32 | double  (input element type for a/b,
    //              accumulator element type for acc)
    //     layout : row | col   (matrix_a/b only; accumulator has no layout tag)
    //
    // All fragments are [SingleStaticAssignment]: the underlying C++ object is
    // warp-distributed state; re-binding the C# handle to a fresh value has no
    // sensible CUDA semantics.
    // ---------------------------------------------------------------------

    #region (16,16,16) half-input fragments

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 16, 16, 16, half, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_a_16x16x16_half_row
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 16, 16, 16, half, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_a_16x16x16_half_col
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 16, 16, 16, half, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_b_16x16x16_half_row
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 16, 16, 16, half, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_b_16x16x16_half_col
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 16, 16, 16, half>")]
    [SingleStaticAssignment]
    public struct frag_acc_16x16x16_half
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 16, 16, 16, float>")]
    [SingleStaticAssignment]
    public struct frag_acc_16x16x16_float
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    #endregion

    #region (32,8,16) half-input fragments

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 32, 8, 16, half, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_a_32x8x16_half_row
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 32, 8, 16, half, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_a_32x8x16_half_col
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 32, 8, 16, half, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_b_32x8x16_half_row
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 32, 8, 16, half, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_b_32x8x16_half_col
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 32, 8, 16, half>")]
    [SingleStaticAssignment]
    public struct frag_acc_32x8x16_half
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 32, 8, 16, float>")]
    [SingleStaticAssignment]
    public struct frag_acc_32x8x16_float
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    #endregion

    #region (8,32,16) half-input fragments

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 8, 32, 16, half, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_a_8x32x16_half_row
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 8, 32, 16, half, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_a_8x32x16_half_col
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 8, 32, 16, half, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_b_8x32x16_half_row
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 8, 32, 16, half, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_b_8x32x16_half_col
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 8, 32, 16, half>")]
    [SingleStaticAssignment]
    public struct frag_acc_8x32x16_half
    {
        public half this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return new half(); }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 8, 32, 16, float>")]
    [SingleStaticAssignment]
    public struct frag_acc_8x32x16_float
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    #endregion

    #region (16,16,8) tf32-input fragments  (sm_80+)

    // The element storage type for tf32 fragments is `float`; the tf32 tag
    // selects 19-bit mantissa interpretation inside the tensor core MMA.
    // load/store and fill_fragment therefore use `float` for value/pointer.

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 16, 16, 8, nvcuda::wmma::precision::tf32, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_a_16x16x8_tf32_row
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 16, 16, 8, nvcuda::wmma::precision::tf32, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_a_16x16x8_tf32_col
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 16, 16, 8, nvcuda::wmma::precision::tf32, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_b_16x16x8_tf32_row
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 16, 16, 8, nvcuda::wmma::precision::tf32, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_b_16x16x8_tf32_col
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 16, 16, 8, float>")]
    [SingleStaticAssignment]
    public struct frag_acc_16x16x8_float
    {
        public float this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0f; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    #endregion

    #region (8,8,4) double-input fragments  (sm_80+)

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 8, 8, 4, double, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_a_8x8x4_double_row
    {
        public double this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_a, 8, 8, 4, double, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_a_8x8x4_double_col
    {
        public double this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 8, 8, 4, double, nvcuda::wmma::row_major>")]
    [SingleStaticAssignment]
    public struct frag_b_8x8x4_double_row
    {
        public double this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::matrix_b, 8, 8, 4, double, nvcuda::wmma::col_major>")]
    [SingleStaticAssignment]
    public struct frag_b_8x8x4_double_col
    {
        public double this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    [IntrinsicIncludeCUDA("<mma.h>")]
    [IntrinsicType("nvcuda::wmma::fragment<nvcuda::wmma::accumulator, 8, 8, 4, double>")]
    [SingleStaticAssignment]
    public struct frag_acc_8x8x4_double
    {
        public double this[int i]
        {
            [IntrinsicFieldSubscript("x")]
            get { return 0.0; }
            [IntrinsicFieldSubscript("x")]
            set { }
        }
    }

    #endregion

    #endregion

    #region Operations

    /// <summary>
    /// Warp-collective WMMA operations. Every overload binds to a function in
    /// the <c>nvcuda::wmma</c> namespace; the C# call signature pins the
    /// fragment template specialization. These calls MUST be issued by all 32
    /// lanes of a warp.
    /// </summary>
    [IntrinsicIncludeCUDA("<mma.h>")]
    public static class op
    {
        #region fill_fragment

        // ---- (16,16,16) half-input ----
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_16x16x16_half_row f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_16x16x16_half_col f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_16x16x16_half_row f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_16x16x16_half_col f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_16x16x16_half f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_16x16x16_float f, float v) { }

        // ---- (32,8,16) half-input ----
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_32x8x16_half_row f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_32x8x16_half_col f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_32x8x16_half_row f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_32x8x16_half_col f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_32x8x16_half f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_32x8x16_float f, float v) { }

        // ---- (8,32,16) half-input ----
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_8x32x16_half_row f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_8x32x16_half_col f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_8x32x16_half_row f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_8x32x16_half_col f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_8x32x16_half f, half v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_8x32x16_float f, float v) { }

        // ---- (16,16,8) tf32 ----
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_16x16x8_tf32_row f, float v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_16x16x8_tf32_col f, float v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_16x16x8_tf32_row f, float v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_16x16x8_tf32_col f, float v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_16x16x8_float f, float v) { }

        // ---- (8,8,4) double ----
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_8x8x4_double_row f, double v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_a_8x8x4_double_col f, double v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_8x8x4_double_row f, double v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_b_8x8x4_double_col f, double v) { }
        [IntrinsicFunction("nvcuda::wmma::fill_fragment")]
        public static void fill_fragment([HandleByRef] ref frag_acc_8x8x4_double f, double v) { }

        #endregion

        #region load_matrix_sync (matrix_a / matrix_b -- no layout_t argument)

        // ---- (16,16,16) half ----
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_16x16x16_half_row f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_16x16x16_half_col f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_16x16x16_half_row f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_16x16x16_half_col f, half* mptr, uint ldm) { }

        // ---- (32,8,16) half ----
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_32x8x16_half_row f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_32x8x16_half_col f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_32x8x16_half_row f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_32x8x16_half_col f, half* mptr, uint ldm) { }

        // ---- (8,32,16) half ----
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_8x32x16_half_row f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_8x32x16_half_col f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_8x32x16_half_row f, half* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_8x32x16_half_col f, half* mptr, uint ldm) { }

        // ---- (16,16,8) tf32 (mptr is float*; storage is float, MMA reads as tf32) ----
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_16x16x8_tf32_row f, float* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_16x16x8_tf32_col f, float* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_16x16x8_tf32_row f, float* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_16x16x8_tf32_col f, float* mptr, uint ldm) { }

        // ---- (8,8,4) double ----
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_8x8x4_double_row f, double* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_a_8x8x4_double_col f, double* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_8x8x4_double_row f, double* mptr, uint ldm) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_b_8x8x4_double_col f, double* mptr, uint ldm) { }

        #endregion

        #region load_matrix_sync (accumulator -- takes layout_t)

        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_16x16x16_half  f, half*   mptr, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_16x16x16_float f, float*  mptr, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_32x8x16_half   f, half*   mptr, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_32x8x16_float  f, float*  mptr, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_8x32x16_half   f, half*   mptr, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_8x32x16_float  f, float*  mptr, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_16x16x8_float  f, float*  mptr, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::load_matrix_sync")]
        public static unsafe void load_matrix_sync([HandleByRef] ref frag_acc_8x8x4_double   f, double* mptr, uint ldm, wmma_layout layout) { }

        #endregion

        #region store_matrix_sync (accumulator only)

        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(half*   mptr, frag_acc_16x16x16_half  f, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(float*  mptr, frag_acc_16x16x16_float f, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(half*   mptr, frag_acc_32x8x16_half   f, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(float*  mptr, frag_acc_32x8x16_float  f, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(half*   mptr, frag_acc_8x32x16_half   f, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(float*  mptr, frag_acc_8x32x16_float  f, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(float*  mptr, frag_acc_16x16x8_float  f, uint ldm, wmma_layout layout) { }
        [IntrinsicFunction("nvcuda::wmma::store_matrix_sync")]
        public static unsafe void store_matrix_sync(double* mptr, frag_acc_8x8x4_double   f, uint ldm, wmma_layout layout) { }

        #endregion

        #region mma_sync

        // ---- (16,16,16) half-input, float acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_float d, frag_a_16x16x16_half_row a, frag_b_16x16x16_half_row b, frag_acc_16x16x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_float d, frag_a_16x16x16_half_row a, frag_b_16x16x16_half_col b, frag_acc_16x16x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_float d, frag_a_16x16x16_half_col a, frag_b_16x16x16_half_row b, frag_acc_16x16x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_float d, frag_a_16x16x16_half_col a, frag_b_16x16x16_half_col b, frag_acc_16x16x16_float c) { }

        // ---- (16,16,16) half-input, half acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_half d, frag_a_16x16x16_half_row a, frag_b_16x16x16_half_row b, frag_acc_16x16x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_half d, frag_a_16x16x16_half_row a, frag_b_16x16x16_half_col b, frag_acc_16x16x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_half d, frag_a_16x16x16_half_col a, frag_b_16x16x16_half_row b, frag_acc_16x16x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x16_half d, frag_a_16x16x16_half_col a, frag_b_16x16x16_half_col b, frag_acc_16x16x16_half c) { }

        // ---- (32,8,16) half-input, float acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_float d, frag_a_32x8x16_half_row a, frag_b_32x8x16_half_row b, frag_acc_32x8x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_float d, frag_a_32x8x16_half_row a, frag_b_32x8x16_half_col b, frag_acc_32x8x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_float d, frag_a_32x8x16_half_col a, frag_b_32x8x16_half_row b, frag_acc_32x8x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_float d, frag_a_32x8x16_half_col a, frag_b_32x8x16_half_col b, frag_acc_32x8x16_float c) { }

        // ---- (32,8,16) half-input, half acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_half d, frag_a_32x8x16_half_row a, frag_b_32x8x16_half_row b, frag_acc_32x8x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_half d, frag_a_32x8x16_half_row a, frag_b_32x8x16_half_col b, frag_acc_32x8x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_half d, frag_a_32x8x16_half_col a, frag_b_32x8x16_half_row b, frag_acc_32x8x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_32x8x16_half d, frag_a_32x8x16_half_col a, frag_b_32x8x16_half_col b, frag_acc_32x8x16_half c) { }

        // ---- (8,32,16) half-input, float acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_float d, frag_a_8x32x16_half_row a, frag_b_8x32x16_half_row b, frag_acc_8x32x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_float d, frag_a_8x32x16_half_row a, frag_b_8x32x16_half_col b, frag_acc_8x32x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_float d, frag_a_8x32x16_half_col a, frag_b_8x32x16_half_row b, frag_acc_8x32x16_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_float d, frag_a_8x32x16_half_col a, frag_b_8x32x16_half_col b, frag_acc_8x32x16_float c) { }

        // ---- (8,32,16) half-input, half acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_half d, frag_a_8x32x16_half_row a, frag_b_8x32x16_half_row b, frag_acc_8x32x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_half d, frag_a_8x32x16_half_row a, frag_b_8x32x16_half_col b, frag_acc_8x32x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_half d, frag_a_8x32x16_half_col a, frag_b_8x32x16_half_row b, frag_acc_8x32x16_half c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x32x16_half d, frag_a_8x32x16_half_col a, frag_b_8x32x16_half_col b, frag_acc_8x32x16_half c) { }

        // ---- (16,16,8) tf32, float acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x8_float d, frag_a_16x16x8_tf32_row a, frag_b_16x16x8_tf32_row b, frag_acc_16x16x8_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x8_float d, frag_a_16x16x8_tf32_row a, frag_b_16x16x8_tf32_col b, frag_acc_16x16x8_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x8_float d, frag_a_16x16x8_tf32_col a, frag_b_16x16x8_tf32_row b, frag_acc_16x16x8_float c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_16x16x8_float d, frag_a_16x16x8_tf32_col a, frag_b_16x16x8_tf32_col b, frag_acc_16x16x8_float c) { }

        // ---- (8,8,4) double, double acc ----
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x8x4_double d, frag_a_8x8x4_double_row a, frag_b_8x8x4_double_row b, frag_acc_8x8x4_double c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x8x4_double d, frag_a_8x8x4_double_row a, frag_b_8x8x4_double_col b, frag_acc_8x8x4_double c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x8x4_double d, frag_a_8x8x4_double_col a, frag_b_8x8x4_double_row b, frag_acc_8x8x4_double c) { }
        [IntrinsicFunction("nvcuda::wmma::mma_sync")]
        public static void mma_sync([HandleByRef] ref frag_acc_8x8x4_double d, frag_a_8x8x4_double_col a, frag_b_8x8x4_double_col b, frag_acc_8x8x4_double c) { }

        #endregion
    }

    #endregion
}
#pragma warning restore 1591
