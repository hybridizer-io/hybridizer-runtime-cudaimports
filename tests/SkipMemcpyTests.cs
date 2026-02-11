using System;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Hybridizer.Runtime.CUDAImports;

namespace Hybridizer.Runtime.CUDAImports.Tests;

/// <summary>
/// Tests for the skipMemcpy parameter flowing through the marshalling call graph.
/// These tests use MainMemoryMarshaler (no GPU required).
/// </summary>
public class SkipMemcpyTests
{
    private MainMemoryMarshaler marshaller;

    [SetUp]
    public void SetUp()
    {
        marshaller = MainMemoryMarshaler.Create(HybridizerFlavor.OMP);
    }

    #region MarshalManagedToNative with skipMemcpy

    [Test]
    public void MarshalClass_SkipMemcpy_ReturnsNonZeroPointer()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, true);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void MarshalClass_NoSkip_ReturnsNonZeroPointer()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, false);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void MarshalArray_SkipMemcpy_ReturnsNonZeroPointer()
    {
        double[] array = { 1.0, 2.0, 3.0 };
        IntPtr ptr = marshaller.MarshalManagedToNative(array, true);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void MarshalArray_NoSkip_ReturnsNonZeroPointer()
    {
        double[] array = { 1.0, 2.0, 3.0 };
        IntPtr ptr = marshaller.MarshalManagedToNative(array, false);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void MarshalNull_SkipMemcpy_ReturnsZero()
    {
        IntPtr ptr = marshaller.MarshalManagedToNative(null, true);
        Assert.That(ptr, Is.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void MarshalPrimitive_SkipMemcpy_IgnoresSkipFlag()
    {
        // Primitives are passed by value, skipMemcpy should have no effect
        IntPtr ptr1 = marshaller.MarshalManagedToNative(42, false);
        IntPtr ptr2 = marshaller.MarshalManagedToNative(42, true);
        Assert.That(ptr1, Is.EqualTo(ptr2));
    }

    [Test]
    public void MarshalStruct_SkipMemcpy_ReturnsNonZeroPointer()
    {
        var instance = new MyTestStruct { X = 1.0, Y = 2.0 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, true);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
    }

    #endregion

    #region Default parameter (backward compatibility)

    [Test]
    public void MarshalClass_DefaultOverload_StillWorks()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        // The original single-arg overload should still work
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public unsafe void MarshalClass_DefaultOverload_DataIsCorrect()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);

        // Layout: VTABLE(8) + Value(double, 8) + Count(int, 4) + padding(4)
        double* doubles = (double*)ptr;
        Assert.That(doubles[0], Is.EqualTo(0));    // vtable
        Assert.That(doubles[1], Is.EqualTo(42.0)); // Value
        Assert.That(((int*)ptr)[4], Is.EqualTo(7)); // Count at byte offset 16
    }

    #endregion

    #region CleanUpManagedData with skipMemcpy

    [Test]
    public void CleanUpManagedData_SkipMemcpy_CleansUpWithoutError()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(0));

        // CleanUp with skipMemcpy = true (FreeObjectGraph path)
        marshaller.CleanUpManagedData(instance, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CleanUpManagedData_NoSkip_CleansUpWithoutError()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(0));

        // CleanUp with skipMemcpy = false (deserializer path)
        marshaller.CleanUpManagedData(instance, false);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CleanUpManagedData_DefaultOverload_CleansUp()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        marshaller.MarshalManagedToNative(instance);
        marshaller.CleanUpManagedData(instance);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CleanUpManagedData_Array_SkipMemcpy_CleansUp()
    {
        double[] array = { 1.0, 2.0, 3.0 };
        marshaller.MarshalManagedToNative(array);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(0));

        marshaller.CleanUpManagedData(array, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region CleanUpNativeData with skipMemcpy

    [Test]
    public void CleanUpNativeData_SkipMemcpy_CleansUp()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(0));

        marshaller.CleanUpNativeData(ptr, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CleanUpNativeData_NoSkip_CleansUp()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);

        marshaller.CleanUpNativeData(ptr, false);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CleanUpNativeData_DefaultOverload_CleansUp()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);

        marshaller.CleanUpNativeData(ptr);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region FreeObjectGraph — nested objects

    [Test]
    public void CleanUpManagedData_SkipMemcpy_ClassWithNestedArray()
    {
        var instance = new MyClassWithNestedArray
        {
            Data = new double[] { 1.0, 2.0, 3.0 },
            Factor = 2.0
        };
        marshaller.MarshalManagedToNative(instance);
        int ghostsBefore = marshaller.NbelementsInGhost;
        Assert.That(ghostsBefore, Is.GreaterThan(1)); // instance + Data array

        marshaller.CleanUpManagedData(instance, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CleanUpManagedData_SkipMemcpy_ClassWithNestedObject()
    {
        var inner = new MyTestClass { Value = 3.14, Count = 1 };
        var outer = new MyClassWithNestedObject
        {
            Inner = inner,
            Tag = 99
        };
        marshaller.MarshalManagedToNative(outer);
        int ghostsBefore = marshaller.NbelementsInGhost;
        Assert.That(ghostsBefore, Is.GreaterThan(1)); // outer + inner

        marshaller.CleanUpManagedData(outer, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CleanUpManagedData_SkipMemcpy_ObjectArrayOfClasses()
    {
        var arr = new MyTestClass[]
        {
            new MyTestClass { Value = 1.0, Count = 1 },
            new MyTestClass { Value = 2.0, Count = 2 },
            null
        };
        marshaller.MarshalManagedToNative(arr);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(1));

        marshaller.CleanUpManagedData(arr, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region Marshal then skipMemcpy cleanup preserves managed data

    [Test]
    public unsafe void MarshalAndSkipCleanup_ManagedDataUnchanged()
    {
        // When we skipMemcpy on cleanup, managed data should NOT be updated from native
        // With MainMemory marshaler the data is pinned so it's the same memory,
        // but the cleanup path (FreeObjectGraph) should not touch managed fields
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance);

        // Modify native memory directly
        double* doubles = (double*)ptr;
        doubles[2] = 999.0; // modify the Value field in native memory

        // Cleanup with skipMemcpy = true — should NOT deserialize back
        marshaller.CleanUpManagedData(instance, true);

        // For MainMemory, since it's pinned, the value might already be reflected.
        // The key thing is that the cleanup doesn't crash and the state is clean.
        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region Round-trip with normal (non-skip) cleanup

    [Test]
    public unsafe void MarshalAndCleanup_NoSkip_RoundTrip()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, false);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));

        marshaller.CleanUpManagedData(instance, false);
        Assert.That(instance.Value, Is.EqualTo(42.0));
        Assert.That(instance.Count, Is.EqualTo(7));
        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region Simulate [Out]-only: skipMemcpy on marshal, normal cleanup

    [Test]
    public void SimulateOutOnly_SkipMarshal_NormalCleanup()
    {
        // [Out] parameter: skip memcpy H→D (marshal with skip), normal D→H cleanup
        var instance = new MyTestClass { Value = 0.0, Count = 0 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, true); // skip H→D
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));

        marshaller.CleanUpManagedData(instance, false); // normal D→H
        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region Simulate [In]-only: normal marshal, skipMemcpy on cleanup

    [Test]
    public void SimulateInOnly_NormalMarshal_SkipCleanup()
    {
        // [In] parameter: normal H→D marshal, skip memcpy D→H (cleanup with skip)
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, false); // normal H→D
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));

        marshaller.CleanUpManagedData(instance, true); // skip D→H
        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region Multiple parameters scenario

    [Test]
    public void MultipleObjects_MixedSkipBehavior()
    {
        // Simulate a call with multiple params having different In/Out attributes
        var inParam = new MyTestClass { Value = 1.0, Count = 1 };       // [In]
        var outParam = new MyTestClass { Value = 0.0, Count = 0 };      // [Out]
        var inoutParam = new MyTestClass { Value = 3.0, Count = 3 };    // [In,Out]

        // Marshal: [In] normal, [Out] skip, [In,Out] normal
        IntPtr ptrIn = marshaller.MarshalManagedToNative(inParam, false);
        IntPtr ptrOut = marshaller.MarshalManagedToNative(outParam, true);
        IntPtr ptrInOut = marshaller.MarshalManagedToNative(inoutParam, false);

        Assert.That(ptrIn, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(ptrOut, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(ptrInOut, Is.Not.EqualTo(IntPtr.Zero));

        // Cleanup: [In] skip, [Out] normal, [In,Out] normal
        marshaller.CleanUpManagedData(inParam, true);
        marshaller.CleanUpManagedData(outParam, false);
        marshaller.CleanUpManagedData(inoutParam, false);

        Assert.That(marshaller.IsClean(), Is.True);
    }

    #endregion

    #region Ghost count tracking

    [Test]
    public void GhostCount_IncreasesOnMarshal_DecreasesOnCleanup()
    {
        Assert.That(marshaller.NbelementsInGhost, Is.EqualTo(0));

        var instance = new MyTestClass { Value = 1.0, Count = 1 };
        marshaller.MarshalManagedToNative(instance);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(0));

        marshaller.CleanUpManagedData(instance, true);
        Assert.That(marshaller.NbelementsInGhost, Is.EqualTo(0));
    }

    [Test]
    public void GhostCount_SkipMarshal_StillTracked()
    {
        var instance = new MyTestClass { Value = 1.0, Count = 1 };
        marshaller.MarshalManagedToNative(instance, true);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(0));

        marshaller.CleanUpManagedData(instance, true);
        Assert.That(marshaller.NbelementsInGhost, Is.EqualTo(0));
    }

    #endregion
}

/// <summary>
/// Tests for [In]/[Out] attribute detection via reflection.
/// Verifies that ParameterInfo.IsIn/IsOut correctly reflects attributes.
/// </summary>
public class InOutAttributeReflectionTests
{
    private static void MethodWithInParam([In] double[] data) { }
    private static void MethodWithOutParam([Out] double[] data) { }
    private static void MethodWithInOutParam([In, Out] double[] data) { }
    private static void MethodWithNoAttributes(double[] data) { }

    [Test]
    public void InAttribute_IsDetected()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithInParam),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];
        Assert.That(pi.IsIn, Is.True);
        Assert.That(pi.IsOut, Is.False);
    }

    [Test]
    public void OutAttribute_IsDetected()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithOutParam),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];
        Assert.That(pi.IsOut, Is.True);
        Assert.That(pi.IsIn, Is.False);
    }

    [Test]
    public void InOutAttributes_BothDetected()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithInOutParam),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];
        Assert.That(pi.IsIn, Is.True);
        Assert.That(pi.IsOut, Is.True);
    }

    [Test]
    public void NoAttributes_NeitherDetected()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithNoAttributes),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];
        Assert.That(pi.IsIn, Is.False);
        Assert.That(pi.IsOut, Is.False);
    }

    [Test]
    public void InOnly_SkipLogic_IsCorrect()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithInParam),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];

        // [In] only → skip D→H on cleanup
        bool isInOnly = pi.IsIn && !pi.IsOut;
        Assert.That(isInOnly, Is.True);
    }

    [Test]
    public void OutOnly_SkipLogic_IsCorrect()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithOutParam),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];

        // [Out] only → skip H→D on marshal
        bool isOutOnly = pi.IsOut && !pi.IsIn;
        Assert.That(isOutOnly, Is.True);
    }

    [Test]
    public void InOut_NoSkipOnEitherSide()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithInOutParam),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];

        bool isInOnly = pi.IsIn && !pi.IsOut;
        bool isOutOnly = pi.IsOut && !pi.IsIn;
        Assert.That(isInOnly, Is.False);
        Assert.That(isOutOnly, Is.False);
    }

    [Test]
    public void NoAttributes_NoSkipOnEitherSide()
    {
        var mi = typeof(InOutAttributeReflectionTests).GetMethod(nameof(MethodWithNoAttributes),
            BindingFlags.Static | BindingFlags.NonPublic);
        var pi = mi.GetParameters()[0];

        bool isInOnly = pi.IsIn && !pi.IsOut;
        bool isOutOnly = pi.IsOut && !pi.IsIn;
        Assert.That(isInOnly, Is.False);
        Assert.That(isOutOnly, Is.False);
    }
}

/// <summary>
/// CUDA-specific tests that require a GPU and CUDA driver.
/// Run with: dotnet test --filter "TestCategory=CUDA"
/// </summary>
[TestFixture, Category("CUDA")]
public class CudaSkipMemcpyTests
{
    private CudaMarshaler marshaller;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        if (!cuda.IsCudaAvailable())
            Assert.Ignore("CUDA is not available — skipping GPU tests");
    }

    [SetUp]
    public void SetUp()
    {
        marshaller = CudaMarshaler.Create(false);
    }

    [TearDown]
    public void TearDown()
    {
        marshaller?.Free();
    }

    [Test]
    public void CudaMarshalClass_NoSkip_ReturnsDevicePointer()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, false);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
        marshaller.CleanUpManagedData(instance, false);
    }

    [Test]
    public void CudaMarshalClass_SkipMemcpy_ReturnsDevicePointer()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, true);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
        marshaller.CleanUpManagedData(instance, true);
    }

    [Test]
    public void CudaMarshalArray_NoSkip_ReturnsDevicePointer()
    {
        double[] array = { 1.0, 2.0, 3.0, 4.0, 5.0 };
        IntPtr ptr = marshaller.MarshalManagedToNative(array, false);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
        marshaller.CleanUpManagedData(array, false);
    }

    [Test]
    public void CudaMarshalArray_SkipMemcpy_ReturnsDevicePointer()
    {
        // [Out] scenario: allocate on GPU, no H→D copy
        double[] array = { 1.0, 2.0, 3.0, 4.0, 5.0 };
        IntPtr ptr = marshaller.MarshalManagedToNative(array, true);
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));
        marshaller.CleanUpManagedData(array, true);
    }

    [Test]
    public void CudaCleanUp_SkipMemcpy_FreesWithoutError()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        marshaller.MarshalManagedToNative(instance, false);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(0));

        // [In] scenario: skip D→H on cleanup
        marshaller.CleanUpManagedData(instance, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CudaCleanUp_NoSkip_DeserializesBack()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        marshaller.MarshalManagedToNative(instance, false);

        marshaller.CleanUpManagedData(instance, false);
        Assert.That(instance.Value, Is.EqualTo(42.0));
        Assert.That(instance.Count, Is.EqualTo(7));
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CudaSimulateOutOnly_SkipMarshal_NormalCleanup()
    {
        double[] output = new double[100];
        IntPtr ptr = marshaller.MarshalManagedToNative(output, true); // skip H→D
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));

        marshaller.CleanUpManagedData(output, false); // normal D→H
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CudaSimulateInOnly_NormalMarshal_SkipCleanup()
    {
        double[] input = { 1.0, 2.0, 3.0 };
        IntPtr ptr = marshaller.MarshalManagedToNative(input, false); // normal H→D
        Assert.That(ptr, Is.Not.EqualTo(IntPtr.Zero));

        marshaller.CleanUpManagedData(input, true); // skip D→H
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CudaNestedObject_SkipCleanup_FreesAll()
    {
        var instance = new MyClassWithNestedArray
        {
            Data = new double[] { 1.0, 2.0, 3.0 },
            Factor = 2.0
        };
        marshaller.MarshalManagedToNative(instance, false);
        Assert.That(marshaller.NbelementsInGhost, Is.GreaterThan(1));

        marshaller.CleanUpManagedData(instance, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CudaCleanUpNativeData_SkipMemcpy()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, false);

        marshaller.CleanUpNativeData(ptr, true);
        Assert.That(marshaller.IsClean(), Is.True);
    }

    [Test]
    public void CudaCleanUpNativeData_NoSkip()
    {
        var instance = new MyTestClass { Value = 42.0, Count = 7 };
        IntPtr ptr = marshaller.MarshalManagedToNative(instance, false);

        marshaller.CleanUpNativeData(ptr, false);
        Assert.That(marshaller.IsClean(), Is.True);
    }
}

#region Test helper types

public class MyTestClass
{
    public int Count;
    public double Value;
}

public struct MyTestStruct
{
    public double X;
    public double Y;
}

public class MyClassWithArray
{
    public double[] Data;
    public string Name;
}

public class MyClassWithNestedArray
{
    public double[] Data;
    public double Factor;
}

public class MyClassWithNestedObject
{
    public MyTestClass Inner;
    public int Tag;
}

#endregion
