using System;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Hybridizer.Runtime.CUDAImports;

namespace Hybridizer.Runtime.CUDAImports.Tests;

#pragma warning disable CS0649 // the structs below are only described, never filled

/// <summary>
/// Issue #349: the layout FieldTools computes for a value type must not
/// depend on the host. It used to ask the interop marshaller for field
/// offsets, which only answers for a struct holding an array or an object
/// reference where COM is available (Windows). Elsewhere the type came out
/// with no field and a size of 0.
/// </summary>
[NonParallelizable]
public class FieldToolsLayoutTests
{
    [StructLayout(LayoutKind.Explicit)]
    private struct ExplicitWithArray
    {
        [FieldOffset(0)] public double[] _data;
        [FieldOffset(8)] public IntPtr _devicePtr;
        [FieldOffset(16)] public int _dimX;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct ExplicitBlittable
    {
        [FieldOffset(0)] public double _value;
        [FieldOffset(8)] public IntPtr _devicePtr;
        [FieldOffset(16)] public int _dimX;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct ExplicitDeclaredOutOfOrder
    {
        [FieldOffset(8)] public int second;
        [FieldOffset(0)] public double first;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct ExplicitUnion
    {
        [FieldOffset(0)] public int asInt;
        [FieldOffset(0)] public float asFloat;
        [FieldOffset(4)] public int tail;
    }

    private struct SequentialWithArray
    {
        public double[] data;
        public int count;
    }

    private struct SequentialWithObject
    {
        public int count;
        public object payload;
    }

    private struct SequentialBlittable
    {
        public double x;
        public int count;
    }

    private bool _useHybridArrays;

    [SetUp]
    public void SetUp()
    {
        _useHybridArrays = CudaRuntimeProperties.UseHybridArrays;
        CudaRuntimeProperties.UseHybridArrays = false;
    }

    [TearDown]
    public void TearDown() => CudaRuntimeProperties.UseHybridArrays = _useHybridArrays;

    private static FieldTools.FieldDeclaration[] SingleGroup(Type t)
    {
        var groups = FieldTools.GetFields(t);
        Assert.That(groups, Has.Count.EqualTo(1), t.Name + " must be described by one group of fields");
        return groups[0];
    }

    private static string[] DataFieldNames(Type t) =>
        SingleGroup(t)
            .Where(d => d.Type == FieldTools.FieldDeclaration.FieldDeclarationType.DATA)
            .Select(d => d.Name)
            .ToArray();

    [Test]
    public void ExplicitLayoutWithArrayField_KeepsItsFieldsAndSize()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DataFieldNames(typeof(ExplicitWithArray)), Is.EqualTo(new[] { "_data", "_devicePtr", "_dimX" }));
            Assert.That(FieldTools.SizeOf(typeof(ExplicitWithArray)), Is.EqualTo(24));
        });
    }

    [Test]
    public void ExplicitLayoutWithArrayField_MatchesItsBlittableTwin()
    {
        Assert.That(FieldTools.SizeOf(typeof(ExplicitWithArray)), Is.EqualTo(FieldTools.SizeOf(typeof(ExplicitBlittable))));
    }

    [Test]
    public void SequentialLayoutWithArrayField_KeepsItsFieldsAndSize()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DataFieldNames(typeof(SequentialWithArray)), Is.EqualTo(new[] { "data", "count" }));
            Assert.That(FieldTools.SizeOf(typeof(SequentialWithArray)), Is.EqualTo(16));
        });
    }

    [Test]
    public void SequentialLayoutWithObjectField_KeepsItsFieldsAndSize()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DataFieldNames(typeof(SequentialWithObject)), Is.EqualTo(new[] { "count", "payload" }));
            Assert.That(FieldTools.SizeOf(typeof(SequentialWithObject)), Is.EqualTo(16));
        });
    }

    // The three tests below already hold on every host: they pin the
    // behaviour the fix for #349 must leave alone.

    [Test]
    public void SequentialBlittable_KeepsDeclarationOrder()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DataFieldNames(typeof(SequentialBlittable)), Is.EqualTo(new[] { "x", "count" }));
            Assert.That(FieldTools.SizeOf(typeof(SequentialBlittable)), Is.EqualTo(16));
        });
    }

    [Test]
    public void ExplicitLayout_OrdersFieldsByOffsetNotByDeclaration()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DataFieldNames(typeof(ExplicitDeclaredOutOfOrder)), Is.EqualTo(new[] { "first", "second" }));
            Assert.That(FieldTools.SizeOf(typeof(ExplicitDeclaredOutOfOrder)), Is.EqualTo(16));
        });
    }

    [Test]
    public void ExplicitLayout_FieldsAtTheSameOffsetFormAUnion()
    {
        var data = SingleGroup(typeof(ExplicitUnion))
            .Where(d => d.Type == FieldTools.FieldDeclaration.FieldDeclarationType.DATA)
            .ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(data, Has.Length.EqualTo(2));
            Assert.That(data[0].UnionSubFields.Select(f => f.Name), Is.EquivalentTo(new[] { "asInt", "asFloat" }));
            Assert.That(data[1].Name, Is.EqualTo("tail"));
        });
    }
}
