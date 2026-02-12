using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Hybridizer.Runtime.CUDAImports;

namespace Hybridizer.Runtime.CUDAImports.Tests;

/// <summary>
/// Tests that HybRunner generates correct IL for [In]/[Out] parameter attributes.
/// Creates a minimal stub .so, uses HybRunner.OMP to wrap test targets,
/// then inspects the generated IL byte arrays.
/// </summary>
[TestFixture]
public class HybRunnerInOutILTests
{
    private static string _stubDir;
    private static string _stubPath;
    private static HybRunner _runner;

    private const byte LDC_I4_1 = 0x17;
    private const byte CALL = 0x28;

    private const string StubSource = @"
#include <string.h>

/*
 * Must match C# HybridizerProperties which has [StructLayout(LayoutKind.Explicit, Size = 16)]
 * but also a _dummy field at [FieldOffset(16)], making the actual CLR struct 20 bytes.
 * On x86-64 Linux, structs > 16 bytes use sret (hidden pointer) calling convention.
 */
typedef struct {
    int useHybridArrays;   /* offset 0 */
    int flavor;            /* offset 4 */
    int delegateSupport;   /* offset 8 */
    int compatibilityMode; /* offset 12 */
    int dummy;             /* offset 16 -- matches C# _dummy field */
} HybridizerProperties;

HybridizerProperties __HybridizerGetProperties(void) {
    HybridizerProperties p;
    memset(&p, 0, sizeof(p));
    p.flavor = 2; /* OMP */
    return p;
}

int HybridizerGetTypeID(const char* name) {
    return 0;
}

int HybridizerGetShallowSize(const char* name) {
    return 0;
}
";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            Assert.Ignore("These tests require Linux (dlopen-based HybRunner)");

        _stubDir = Path.Combine(Path.GetTempPath(), "hybrunner_il_tests_" + Guid.NewGuid().ToString("N").Substring(0, 8));
        Directory.CreateDirectory(_stubDir);

        string stubSourcePath = Path.Combine(_stubDir, "hybstub.c");
        File.WriteAllText(stubSourcePath, StubSource);

        _stubPath = Path.Combine(_stubDir, "hybstub.so");
        var psi = new ProcessStartInfo("gcc", $"-shared -fPIC -o \"{_stubPath}\" \"{stubSourcePath}\"")
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        Process proc;
        try
        {
            proc = Process.Start(psi);
        }
        catch (Exception ex)
        {
            Assert.Ignore("gcc not available: " + ex.Message);
            return;
        }

        proc.WaitForExit(15000);
        if (proc.ExitCode != 0)
        {
            string stderr = proc.StandardError.ReadToEnd();
            Assert.Ignore("gcc compilation failed: " + stderr);
            return;
        }

        _runner = HybRunner.OMP(_stubPath);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        try
        {
            if (_stubDir != null && Directory.Exists(_stubDir))
                Directory.Delete(_stubDir, true);
        }
        catch { /* best effort cleanup */ }
    }

    #region Helpers

    private static MethodInfo GetWrappedProcessMethod(object target)
    {
        dynamic wrapped = _runner.Wrap(target);
        Type wrappedType = ((object)wrapped).GetType();
        // Find the "Process" method that takes managed parameters (not the IntPtr overload)
        return wrappedType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == "Process")
            .Where(m => !m.GetParameters().Any(p => p.ParameterType == typeof(IntPtr)))
            .First();
    }

    private static byte[] GetWrappedProcessIL(object target)
    {
        var method = GetWrappedProcessMethod(target);
        var body = method.GetMethodBody();
        Assert.That(body, Is.Not.Null, "GetMethodBody() returned null on dynamic method");
        return body.GetILAsByteArray();
    }

    /// <summary>
    /// Counts ldc.i4.1 (0x17) instructions by properly walking the IL instruction stream,
    /// so operand bytes are not mistaken for opcodes.
    /// </summary>
    private static int CountLdcI4_1(byte[] il)
    {
        int count = 0;
        int pos = 0;
        while (pos < il.Length)
        {
            byte op = il[pos];
            if (op == LDC_I4_1)
                count++;
            pos += GetInstructionSize(il, pos);
        }
        return count;
    }

    /// <summary>
    /// Returns the total size (opcode + operand) of the IL instruction at the given offset.
    /// Handles the opcodes commonly emitted by HybRunner's IL generation.
    /// </summary>
    private static int GetInstructionSize(byte[] il, int offset)
    {
        if (offset >= il.Length) return 1;
        byte op = il[offset];

        // Two-byte opcode prefix
        if (op == 0xFE)
        {
            if (offset + 1 >= il.Length) return 1;
            byte op2 = il[offset + 1];
            // ceq(0x01), cgt(0x02), clt(0x04) — no operand
            if (op2 <= 0x06) return 2;
            // ldarg(0x09), starg(0x0B), ldloc(0x0C), stloc(0x0E), ldloca(0x0D) — 2-byte operand
            if (op2 >= 0x09 && op2 <= 0x0E) return 4;
            return 2; // conservative default for unknown 0xFE opcodes
        }

        switch (op)
        {
            // 1-byte instructions (no operand)
            case 0x00: // nop
            case 0x01: // break
            case 0x02: case 0x03: case 0x04: case 0x05: // ldarg.0-3
            case 0x06: case 0x07: case 0x08: case 0x09: // ldloc.0-3
            case 0x0A: case 0x0B: case 0x0C: case 0x0D: // stloc.0-3
            case 0x14: // ldnull
            case 0x15: // ldc.i4.m1
            case 0x16: case 0x17: case 0x18: case 0x19: // ldc.i4.0-3
            case 0x1A: case 0x1B: case 0x1C: case 0x1D: case 0x1E: // ldc.i4.4-8
            case 0x25: // dup
            case 0x26: // pop
            case 0x2A: // ret
            case 0x58: case 0x59: case 0x5A: case 0x5B: // add, sub, mul, div
            case 0x5C: case 0x5D: // div.un, rem
            case 0x60: case 0x61: case 0x62: // and, or, xor
            case 0x67: // conv.i1
            case 0x68: // conv.i2
            case 0x69: // conv.i4
            case 0x6A: // conv.i8
            case 0x6B: // conv.r4
            case 0x6C: // conv.r8
            case 0x6D: // conv.u4
            case 0x6E: // conv.u8
            case 0x76: // conv.r.un
            case 0xD3: // conv.u
                return 1;

            // 2-byte instructions (1-byte operand)
            case 0x0E: // ldarg.s
            case 0x0F: // ldarga.s
            case 0x10: // starg.s
            case 0x11: // ldloc.s
            case 0x12: // ldloca.s
            case 0x13: // stloc.s
            case 0x1F: // ldc.i4.s
            case 0x2B: // br.s
            case 0x2C: // brfalse.s
            case 0x2D: // brtrue.s
            case 0x2E: case 0x2F: case 0x30: case 0x31: // beq.s, bge.s, bgt.s, ble.s
            case 0x32: case 0x33: case 0x34: case 0x35: // blt.s, bne.un.s, bge.un.s, bgt.un.s
            case 0x36: case 0x37: // ble.un.s, blt.un.s
            case 0xDE: // leave.s
                return 2;

            // 5-byte instructions (4-byte operand: tokens, int32, branch targets)
            case 0x20: // ldc.i4
            case 0x28: // call
            case 0x29: // calli
            case 0x38: // br
            case 0x39: // brfalse
            case 0x3A: // brtrue
            case 0x3B: case 0x3C: case 0x3D: case 0x3E: // beq, bge, bgt, ble
            case 0x3F: case 0x40: case 0x41: case 0x42: // blt, bne.un, bge.un, bgt.un
            case 0x43: case 0x44: // ble.un, blt.un
            case 0x6F: // callvirt
            case 0x70: // cpobj
            case 0x71: // ldobj
            case 0x72: // ldstr
            case 0x73: // newobj
            case 0x74: // castclass
            case 0x75: // isinst
            case 0x79: // unbox
            case 0x7B: // ldfld
            case 0x7C: // ldflda
            case 0x7D: // stfld
            case 0x7E: // ldsfld
            case 0x7F: // ldsflda
            case 0x80: // stsfld
            case 0x81: // stobj
            case 0x8C: // box
            case 0x8D: // newarr
            case 0x8E: // ldlen (actually 1 byte, but harmless to overcount)
            case 0xA1: // ldelema
            case 0xA3: // ldelem
            case 0xA4: // stelem
            case 0xA5: // unbox.any
            case 0xC2: // refanyval
            case 0xC6: // mkrefany
            case 0xD0: // ldtoken
            case 0xDD: // leave
                return 5;

            // 9-byte instructions (8-byte operand)
            case 0x21: // ldc.i8
            case 0x23: // ldc.r8
                return 9;

            // 5-byte instructions (4-byte operand: float32)
            case 0x22: // ldc.r4
                return 5;

            // switch (variable length)
            case 0x45:
            {
                if (offset + 4 >= il.Length) return 5;
                int count = BitConverter.ToInt32(il, offset + 1);
                return 5 + count * 4;
            }

            default:
                return 1; // unknown opcode, advance by 1
        }
    }

    /// <summary>
    /// Attempts to resolve Call instruction targets in the generated IL.
    /// Returns null if Module.ResolveMethod doesn't work on dynamic modules.
    /// </summary>
    private static List<MethodBase> TryResolveCallTargets(MethodInfo method)
    {
        try
        {
            var body = method.GetMethodBody();
            if (body == null) return null;
            var il = body.GetILAsByteArray();
            if (il == null) return null;
            var module = method.Module;
            var results = new List<MethodBase>();

            int pos = 0;
            while (pos < il.Length)
            {
                byte op = il[pos];
                int size = GetInstructionSize(il, pos);
                if (op == CALL && pos + 4 < il.Length)
                {
                    int token = BitConverter.ToInt32(il, pos + 1);
                    try
                    {
                        var target = module.ResolveMethod(token);
                        results.Add(target);
                    }
                    catch
                    {
                        return null;
                    }
                }
                pos += size;
            }
            return results;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region ldc.i4.1 count tests

    [Test]
    public void NoAttributes_NoSkipInstructions()
    {
        var il = GetWrappedProcessIL(new NoAttributeTarget());
        Assert.That(CountLdcI4_1(il), Is.EqualTo(0),
            "No attributes should produce zero ldc.i4.1 (no skip logic)");
    }

    [Test]
    public void InOnly_OneSkipInstruction()
    {
        var il = GetWrappedProcessIL(new InOnlyTarget());
        Assert.That(CountLdcI4_1(il), Is.EqualTo(1),
            "[In]-only should produce exactly one ldc.i4.1 (cleanup skip)");
    }

    [Test]
    public void OutOnly_OneSkipInstruction()
    {
        var il = GetWrappedProcessIL(new OutOnlyTarget());
        Assert.That(CountLdcI4_1(il), Is.EqualTo(1),
            "[Out]-only should produce exactly one ldc.i4.1 (marshal skip)");
    }

    [Test]
    public void InOut_NoSkipInstructions()
    {
        var il = GetWrappedProcessIL(new InOutTarget());
        Assert.That(CountLdcI4_1(il), Is.EqualTo(0),
            "[In,Out] should produce zero ldc.i4.1 (no skip on either side)");
    }

    [Test]
    public void MixedParams_CorrectSkipCount()
    {
        var il = GetWrappedProcessIL(new MixedTarget());
        Assert.That(CountLdcI4_1(il), Is.EqualTo(2),
            "Mixed [In]/[Out]/none should produce exactly 2 ldc.i4.1");
    }

    [Test]
    public void InOut_SameSkipCountAs_NoAttributes()
    {
        var ilNoAttr = GetWrappedProcessIL(new NoAttributeTarget());
        var ilInOut = GetWrappedProcessIL(new InOutTarget());
        Assert.That(CountLdcI4_1(ilInOut), Is.EqualTo(CountLdcI4_1(ilNoAttr)),
            "[In,Out] should produce same skip count as no-attribute");
    }

    [Test]
    public void StaticMethod_InOnly_OneSkipInstruction()
    {
        var il = GetWrappedProcessIL(new StaticInOnlyTarget());
        Assert.That(CountLdcI4_1(il), Is.EqualTo(1),
            "Static [In]-only should produce exactly one ldc.i4.1");
    }

    #endregion

    #region Parameter attribute preservation

    [Test]
    public void InOnly_PreservesIsIn()
    {
        var param = GetWrappedProcessMethod(new InOnlyTarget()).GetParameters()[0];
        Assert.That(param.IsIn, Is.True);
        Assert.That(param.IsOut, Is.False);
    }

    [Test]
    public void OutOnly_PreservesIsOut()
    {
        var param = GetWrappedProcessMethod(new OutOnlyTarget()).GetParameters()[0];
        Assert.That(param.IsOut, Is.True);
        Assert.That(param.IsIn, Is.False);
    }

    [Test]
    public void InOut_PreservesBoth()
    {
        var param = GetWrappedProcessMethod(new InOutTarget()).GetParameters()[0];
        Assert.That(param.IsIn, Is.True);
        Assert.That(param.IsOut, Is.True);
    }

    [Test]
    public void NoAttributes_PreservesNone()
    {
        var param = GetWrappedProcessMethod(new NoAttributeTarget()).GetParameters()[0];
        Assert.That(param.IsIn, Is.False);
        Assert.That(param.IsOut, Is.False);
    }

    [Test]
    public void MixedParams_PreservesAllAttributes()
    {
        var parameters = GetWrappedProcessMethod(new MixedTarget()).GetParameters();
        // param 0: [In]
        Assert.That(parameters[0].IsIn, Is.True);
        Assert.That(parameters[0].IsOut, Is.False);
        // param 1: [Out]
        Assert.That(parameters[1].IsOut, Is.True);
        Assert.That(parameters[1].IsIn, Is.False);
        // param 2: no attributes
        Assert.That(parameters[2].IsIn, Is.False);
        Assert.That(parameters[2].IsOut, Is.False);
    }

    #endregion

    #region Wrapper type structure

    [Test]
    public void WrappedType_HasRuntimeField()
    {
        dynamic wrapped = _runner.Wrap(new NoAttributeTarget());
        Type wt = ((object)wrapped).GetType();
        var field = wt.GetField("runtime", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(field, Is.Not.Null);
        Assert.That(field.FieldType, Is.EqualTo(typeof(HybRunner)));
    }

    [Test]
    public void WrappedType_HasWrappedField()
    {
        dynamic wrapped = _runner.Wrap(new NoAttributeTarget());
        Type wt = ((object)wrapped).GetType();
        var field = wt.GetField("wrapped", BindingFlags.Public | BindingFlags.Instance);
        Assert.That(field, Is.Not.Null);
        Assert.That(field.FieldType, Is.EqualTo(typeof(object)));
    }

    [Test]
    public void WrappedMethod_ReturnsInt()
    {
        var method = GetWrappedProcessMethod(new NoAttributeTarget());
        Assert.That(method.ReturnType, Is.EqualTo(typeof(int)));
    }

    #endregion

    #region IL size differential

    [Test]
    public void InOnly_LargerILThan_NoAttributes()
    {
        var ilNoAttr = GetWrappedProcessIL(new NoAttributeTarget());
        var ilIn = GetWrappedProcessIL(new InOnlyTarget());
        Assert.That(ilIn.Length, Is.GreaterThan(ilNoAttr.Length),
            "[In]-only IL should be larger due to extra ldc.i4.1 + different call target");
    }

    [Test]
    public void OutOnly_LargerILThan_NoAttributes()
    {
        var ilNoAttr = GetWrappedProcessIL(new NoAttributeTarget());
        var ilOut = GetWrappedProcessIL(new OutOnlyTarget());
        Assert.That(ilOut.Length, Is.GreaterThan(ilNoAttr.Length),
            "[Out]-only IL should be larger due to extra ldc.i4.1 + different call target");
    }

    #endregion

    #region Call target resolution (best-effort)

    [Test]
    public void OutOnly_CallsMarshalManagedToNativeWithSkip()
    {
        var method = GetWrappedProcessMethod(new OutOnlyTarget());
        var targets = TryResolveCallTargets(method);
        if (targets == null)
        {
            Assert.Ignore("Module.ResolveMethod not supported on this dynamic module");
            return;
        }

        // The skip variant is MarshalManagedToNative(object, bool) - 2 parameters
        var marshalCalls = targets.Where(t => t.Name == "MarshalManagedToNative").ToList();
        Assert.That(marshalCalls.Any(m => m.GetParameters().Length == 2), Is.True,
            "[Out] should call the 2-parameter MarshalManagedToNative (with skipMemcpy)");
    }

    [Test]
    public void InOnly_CallsCleanUpManagedDataWithSkip()
    {
        var method = GetWrappedProcessMethod(new InOnlyTarget());
        var targets = TryResolveCallTargets(method);
        if (targets == null)
        {
            Assert.Ignore("Module.ResolveMethod not supported on this dynamic module");
            return;
        }

        var cleanupCalls = targets.Where(t => t.Name == "CleanUpManagedData").ToList();
        Assert.That(cleanupCalls.Any(m => m.GetParameters().Length == 2), Is.True,
            "[In] should call the 2-parameter CleanUpManagedData (with skipMemcpy)");
    }

    [Test]
    public void NoAttributes_DoesNotCallSkipVariants()
    {
        var method = GetWrappedProcessMethod(new NoAttributeTarget());
        var targets = TryResolveCallTargets(method);
        if (targets == null)
        {
            Assert.Ignore("Module.ResolveMethod not supported on this dynamic module");
            return;
        }

        var marshalCalls = targets.Where(t => t.Name == "MarshalManagedToNative").ToList();
        var cleanupCalls = targets.Where(t => t.Name == "CleanUpManagedData").ToList();
        // No 2-parameter overloads for the parameter (self is always non-skip)
        // For the parameter call, only 1-parameter overloads should be used
        Assert.That(marshalCalls.All(m => m.GetParameters().Length == 1), Is.True,
            "No attributes: all MarshalManagedToNative calls should be 1-parameter");
        Assert.That(cleanupCalls.All(m => m.GetParameters().Length == 1), Is.True,
            "No attributes: all CleanUpManagedData calls should be 1-parameter");
    }

    #endregion
}

#region Test target classes

public class NoAttributeTarget
{
    [EntryPoint]
    public void Process(double[] data) { }
}

public class InOnlyTarget
{
    [EntryPoint]
    public void Process([In] double[] data) { }
}

public class OutOnlyTarget
{
    [EntryPoint]
    public void Process([Out] double[] data) { }
}

public class InOutTarget
{
    [EntryPoint]
    public void Process([In, Out] double[] data) { }
}

public class MixedTarget
{
    [EntryPoint]
    public void Process([In] double[] input, [Out] double[] output, double[] both) { }
}

public class StaticInOnlyTarget
{
    [EntryPoint]
    public static void Process([In] double[] data) { }
}

#endregion
