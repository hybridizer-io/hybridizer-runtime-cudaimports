using System;
using NUnit.Framework;
using Hybridizer.Runtime.CUDAImports;

namespace Hybridizer.Runtime.CUDAImports.Tests;

public class MarshallerTests
{
    private MainMemoryMarshaler marshaller;

    [SetUp]
    public void SetUp() => marshaller = MainMemoryMarshaler.Create(HybridizerFlavor.OMP);

    [TearDown]
    public void TearDown()
    {
        marshaller?.Free();
        Assert.That(marshaller?.IsClean(), Is.True, "Native allocations remain after cleanup.");
    }

    [Test]
    public unsafe void TestMarshalClassWithNumericAlignedFields() {
        MyClassAlignedFields instance = new()
        {
            d1 = 12.0,
            d2 = 42.0
        };

        IntPtr ptr = marshaller.MarshalManagedToNative(instance);
        int* native = (int*) ptr;
        
        // vtable
        Assert.That(((double*)ptr)[0], Is.EqualTo(0));
        Assert.That(((double*)ptr)[1], Is.EqualTo(12.0));
        Assert.That(((double*)ptr)[2], Is.EqualTo(42.0));
    }

    IntPtr Wrap(MyStructUnalignedFields s, MainMemoryMarshaler marshaller) {
        return marshaller.MarshalManagedToNative(s);
    }

    [Test]
    public unsafe void TestMarshalStructWithNumericUnAlignedFields() {
        MyStructUnalignedFields instance = new()
        {
            d1 = 12.0,
            d2 = 3,
            d3 = 42.0
        };

        IntPtr ptr = Wrap(instance, marshaller);
        int* native = (int*) ptr;
       
        Assert.That(((double*)ptr)[0], Is.EqualTo(12.0));
        Assert.That(((int*)ptr)[2], Is.EqualTo(3));
        // should save 4 bytes for padding after 'unaligned' field
        Assert.That(((double*)ptr)[2], Is.EqualTo(42.0));

    }

    [Test]
    public unsafe void TestMarshalArrayOfNullableInt() {
        int?[] a = [1, null, 2, null, 3, null];
        int* native = (int*) marshaller.MarshalManagedToNative(a);
        int[] ret = new int[12];
        for(int i = 0; i < 12; ++i) {
            ret[i] = native[i];
        }
        // 1
        Assert.That(ret[0], Is.EqualTo(1));
        Assert.That(ret[1], Is.EqualTo(1));
        // 2
        Assert.That(ret[2], Is.EqualTo(0));
        Assert.That(ret[3], Is.EqualTo(0));
        // 3
        Assert.That(ret[4], Is.EqualTo(1));
        Assert.That(ret[5], Is.EqualTo(2));
        // 4
        Assert.That(ret[6], Is.EqualTo(0));
        Assert.That(ret[7], Is.EqualTo(0));
        // 5
        Assert.That(ret[8], Is.EqualTo(1));
        Assert.That(ret[9], Is.EqualTo(3));
        // 6
        Assert.That(ret[10], Is.EqualTo(0));
        Assert.That(ret[11], Is.EqualTo(0));
    }

    [Test]
    public unsafe void TestMarshalClassWithNumericAlignedFields2() {
        MyClassAlignedFields2 instance = new()
        {
            d1 = 12.0,
            d2 = 42.0,
            d3 = 3
        };

        IntPtr ptr = marshaller.MarshalManagedToNative(instance);
        int* native = (int*) ptr;
        
        // vtable
        Assert.That(((double*)ptr)[0], Is.EqualTo(0));
        // d1
        Assert.That(((double*)ptr)[1], Is.EqualTo(12.0));
        // d2
        Assert.That(((double*)ptr)[2], Is.EqualTo(42.0));
        // d3
        Assert.That(((int*)ptr)[6], Is.EqualTo(3));

    }

    [Test, Category("Stress"), Explicit("Requires HYBRIDIZER_RUN_STRESS=1 and an explicit memory budget.")]
    public void AllocateLarge()
    {
        Assert.That(Environment.GetEnvironmentVariable("HYBRIDIZER_RUN_STRESS"), Is.EqualTo("1"),
            "Use tools/run-tests.py --suite stress --stress-doubles N.");
        Assert.That(long.TryParse(Environment.GetEnvironmentVariable("HYBRIDIZER_STRESS_DOUBLES"), out var count),
            Is.True, "Specify HYBRIDIZER_STRESS_DOUBLES.");
        Assert.That(count, Is.InRange(1L, 1_000_000_000L));
        TestContext.Out.WriteLine($"Stress budget: {count} doubles, at least {count * 16L} bytes managed + native.");
        double[] a = new double[count];
        IntPtr pi = marshaller.MarshalManagedToNative(a);
        Assert.That(pi, Is.Not.EqualTo(IntPtr.Zero));
    }

}

// fields are alphabetically ordered
public class MyClassAlignedFields
{
    public double d1;
    public double d2;
}

public struct MyStructUnalignedFields
{
    public double d1;
    public int d2;
    public double d3;
}

public class MyClassAlignedFields2
{
    public double d1;
    public double d2;
    public int d3;
}
