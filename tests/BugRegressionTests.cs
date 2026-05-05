using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Hybridizer.Runtime.CUDAImports.Tests;

/// <summary>
/// Regression tests for issues #25-#39 against
/// hybridizer-io/hybridizer-runtime-cudaimports.
///
/// Tests are TDD-style: they describe the desired behavior and currently fail
/// against the buggy code. Each test is annotated with the matching issue.
/// </summary>
public class BugRegressionTests
{
    private static string RepoRoot
    {
        get
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Hybridizer.Runtime.CUDAImports.sln")))
                dir = dir.Parent;
            if (dir == null) throw new InvalidOperationException("Could not locate repo root from " + AppContext.BaseDirectory);
            return dir.FullName;
        }
    }

    private static IEnumerable<string> CudaImplFiles =>
        Directory.GetFiles(Path.Combine(RepoRoot, "src", "CUDARuntime", "RuntimeAPI", "cuda", "Implementations"), "cuda-*.cs");

    private static IEnumerable<string> NvrtcImplFiles =>
        Directory.GetFiles(Path.Combine(RepoRoot, "src", "CUDARuntime", "Nvrtc", "Implementations"), "nvrtc-*.cs");

    private static Assembly CudaAssembly => typeof(cuda).Assembly;

    private static IEnumerable<Type> CudaImplTypes =>
        CudaAssembly.GetTypes().Where(t => t.Name.StartsWith("Cuda_64_") && !t.IsAbstract);

    private static IEnumerable<Type> NvrtcImplTypes =>
        CudaAssembly.GetTypes().Where(t => typeof(INvrtc).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

    /// <summary>
    /// Walk all DllImport-decorated extern methods on a type. DllImport is a
    /// pseudo-custom attribute, so we read it via GetCustomAttribute.
    /// </summary>
    private static IEnumerable<(MethodInfo method, DllImportAttribute attr)> DllImports(Type t)
    {
        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        foreach (var m in t.GetMethods(flags))
        {
            var a = m.GetCustomAttribute<DllImportAttribute>();
            if (a != null) yield return (m, a);
        }
    }

    // -----------------------------------------------------------------------
    // Issue #25 — StreamAddCallback infinite recursion
    // -----------------------------------------------------------------------

    [Test, Description("Issue #25: StreamAddCallback wrapper must call cudaStreamAddCallback, not itself.")]
    public void StreamAddCallback_WrapperMustNotRecurse()
    {
        var offenders = new List<string>();
        foreach (var file in CudaImplFiles)
        {
            var text = File.ReadAllText(file);
            // The bug: `return StreamAddCallback(stream, callback, userData, flags);`
            // The fix would be `return cudaStreamAddCallback(...)` — i.e. the
            // PInvoke shim, never the wrapper itself.
            int idx = 0;
            while ((idx = text.IndexOf("return StreamAddCallback(", idx, StringComparison.Ordinal)) >= 0)
            {
                offenders.Add($"{Path.GetFileName(file)}: '{text.Substring(idx, Math.Min(60, text.Length - idx))}...'");
                idx += 1;
            }
        }
        Assert.That(offenders, Is.Empty,
            "StreamAddCallback wrapper recurses into itself in: " + Environment.NewLine + string.Join(Environment.NewLine, offenders));
    }

    // -----------------------------------------------------------------------
    // Issue #26 — cudaGetMipmappedArrayLevel must not be bound to cudaFreeMipmappedArray
    // -----------------------------------------------------------------------

    [Test, Description("Issue #26: every PInvoke named cudaGetMipmappedArrayLevel must use that exact EntryPoint.")]
    public void GetMipmappedArrayLevel_EntryPointIsCorrect()
    {
        var offenders = new List<string>();
        foreach (var t in CudaImplTypes)
        {
            foreach (var (method, attr) in DllImports(t))
            {
                if (method.Name != "cudaGetMipmappedArrayLevel") continue;
                var ep = attr.EntryPoint ?? method.Name;
                if (ep != "cudaGetMipmappedArrayLevel")
                    offenders.Add($"{t.Name}.{method.Name}: EntryPoint='{ep}'");
            }
        }
        Assert.That(offenders, Is.Empty,
            "cudaGetMipmappedArrayLevel is bound to the wrong native symbol in:" + Environment.NewLine
            + string.Join(Environment.NewLine, offenders));
    }

    // -----------------------------------------------------------------------
    // Issue #27 — no DllImport EntryPoint should have surrounding whitespace
    // -----------------------------------------------------------------------

    [Test, Description("Issue #27: DllImport EntryPoint strings must not have leading or trailing whitespace.")]
    public void DllImport_EntryPointsAreTrimmed()
    {
        var offenders = new List<string>();
        foreach (var t in CudaImplTypes.Concat(NvrtcImplTypes).Concat(new[] { typeof(driver) }))
        {
            foreach (var (method, attr) in DllImports(t))
            {
                var ep = attr.EntryPoint;
                if (ep == null) continue;
                if (ep != ep.Trim())
                    offenders.Add($"{t.Name}.{method.Name}: EntryPoint='{ep}'");
            }
        }
        Assert.That(offenders, Is.Empty,
            "DllImport EntryPoint(s) have stray whitespace:" + Environment.NewLine
            + string.Join(Environment.NewLine, offenders));
    }

    // -----------------------------------------------------------------------
    // Issue #28 — Driver API must not hard-code a Windows DLL name
    // -----------------------------------------------------------------------

    [Test, Description("Issue #28: on Linux, the Driver API DllImports must not reference nvcuda.dll.")]
    [Platform(Include = "Linux")]
    public void DriverApi_DoesNotHardcodeWindowsDll_OnLinux()
    {
        var offenders = new List<string>();
        foreach (var (method, attr) in DllImports(typeof(driver)))
        {
            if (string.Equals(attr.Value, "nvcuda.dll", StringComparison.OrdinalIgnoreCase))
                offenders.Add($"driver.{method.Name}: dll='{attr.Value}'");
        }
        Assert.That(offenders, Is.Empty,
            "Driver API references the Windows-only DLL on Linux. Expected libcuda.so.1 (or an OS-specific impl)." + Environment.NewLine
            + string.Join(Environment.NewLine, offenders));
    }

    // -----------------------------------------------------------------------
    // Issue #29 — duplicate `typeof(byte)` branch
    // -----------------------------------------------------------------------

    [Test, Description("Issue #29: JittedModule.ADD_TO_PARAM_BUFFER must not have a duplicate typeof(byte) branch.")]
    public void JittedModule_NoDuplicateByteBranch()
    {
        var path = Path.Combine(RepoRoot, "src", "CUDARuntime", "Nvrtc", "JittedModule.cs");
        var text = File.ReadAllText(path);
        // Count distinct occurrences of `t == typeof(byte)` inside ADD_TO_PARAM_BUFFER.
        int start = text.IndexOf("ADD_TO_PARAM_BUFFER", StringComparison.Ordinal);
        int end = text.IndexOf("private static unsafe byte[] Convert", StringComparison.Ordinal);
        Assert.That(start, Is.GreaterThanOrEqualTo(0), "couldn't locate ADD_TO_PARAM_BUFFER");
        Assert.That(end, Is.GreaterThan(start), "couldn't locate Convert helper after ADD_TO_PARAM_BUFFER");
        var body = text.Substring(start, end - start);
        int occurrences = 0;
        int idx = 0;
        while ((idx = body.IndexOf("t == typeof(byte)", idx, StringComparison.Ordinal)) >= 0)
        {
            occurrences++;
            idx += 1;
        }
        Assert.That(occurrences, Is.LessThanOrEqualTo(1),
            $"ADD_TO_PARAM_BUFFER contains {occurrences} branches for typeof(byte); only one is reachable.");
    }

    // -----------------------------------------------------------------------
    // Issue #30 — nvrtc impls must exist for every supported cuda version
    // -----------------------------------------------------------------------

    [TestCase("110"), TestCase("114"), TestCase("120"), TestCase("124"), TestCase("126"), TestCase("130"), TestCase("131"),
     Description("Issue #30: an INvrtc implementation should exist for every supported cuda runtime version.")]
    public void Nvrtc_ImplExistsForVersion(string version)
    {
        var found = NvrtcImplTypes.Any(t => t.Name.Contains(version));
        Assert.That(found, Is.True, $"No INvrtc implementation found for cuda version '{version}'.");
    }

    // -----------------------------------------------------------------------
    // Issue #32 — SetCudaVersion must actually rebind the active backend
    // -----------------------------------------------------------------------

    [Test, Description("Issue #32: SetCudaVersion must take effect even after the cuda type has been initialized.")]
    public void SetCudaVersion_RebindsInstance()
    {
        // Save & restore shared state to keep this test isolated from the
        // existing CudaVersionTests fixture (which doesn't reset _cudaversion).
        var versionField = typeof(cuda).GetField("_cudaversion", BindingFlags.Static | BindingFlags.NonPublic);
        var savedVersion = versionField?.GetValue(null);
        var savedInstance = cuda.instance;
        try
        {
            _ = cuda.GetCudaVersion();
            var before = cuda.instance.GetType().Name;

            // Pick a target version that differs from whatever the default selected.
            string target = before.Contains("130") ? "13.1" : "13.0";
            cuda.SetCudaVersion(target);

            var after = cuda.instance.GetType().Name;
            var expectedFragment = target.Replace(".", "");
            Assert.That(after, Does.Contain(expectedFragment),
                $"After SetCudaVersion(\"{target}\"), instance is still {after} (was {before}).");
        }
        finally
        {
            versionField?.SetValue(null, savedVersion);
            cuda.instance = savedInstance;
        }
    }

    // -----------------------------------------------------------------------
    // Issue #33 — StringArrayMarshal on empty array should report IntPtr.Zero
    // -----------------------------------------------------------------------

    [Test, Description("Issue #33: StringArrayMarshal(new string[0]) must surface as IntPtr.Zero, mirroring the null case.")]
    public void StringArrayMarshal_EmptyArray()
    {
        IntPtr ptr = IntPtr.Zero;
        Assert.DoesNotThrow(() =>
        {
            using var m = new StringArrayMarshal(Array.Empty<string>());
            ptr = m.Ptr;
        });
        // An empty option set should be indistinguishable from null at the
        // native boundary — nvrtcCompileProgram(prog, 0, options) is a valid
        // call when options is NULL but undefined when options is a pinned
        // pointer to a zero-length array.
        Assert.That(ptr, Is.EqualTo(IntPtr.Zero));
    }

    // -----------------------------------------------------------------------
    // Issue #34 — StringArrayMarshal must tolerate null entries
    // -----------------------------------------------------------------------

    [Test, Description("Issue #34: StringArrayMarshal must not crash when an element of the input array is null.")]
    public void StringArrayMarshal_NullEntry()
    {
        Assert.DoesNotThrow(() =>
        {
            using var m = new StringArrayMarshal(new[] { "a", null, "b" });
            _ = m.Ptr;
        });
    }

    // -----------------------------------------------------------------------
    // Issue #36 — Linux DLL paths must not be hard-coded under /usr/local
    // -----------------------------------------------------------------------

    [Test, Description("Issue #36: Linux DllImports should reference sonames, not absolute paths under /usr/local/cuda-X.Y.")]
    [Platform(Include = "Linux")]
    public void LinuxDlls_AreNotHardcodedAbsolutePaths()
    {
        var offenders = new List<string>();
        foreach (var t in CudaImplTypes.Concat(NvrtcImplTypes))
        {
            // Only inspect Linux variants; Windows ones ride PATH and are fine.
            if (!t.Name.EndsWith("_linux", StringComparison.Ordinal)) continue;
            foreach (var (method, attr) in DllImports(t))
            {
                var dll = attr.Value;
                if (dll != null && dll.StartsWith("/usr/local/", StringComparison.Ordinal))
                    offenders.Add($"{t.Name}.{method.Name}: dll='{dll}'");
            }
        }
        Assert.That(offenders, Is.Empty,
            "Linux DLL paths should be sonames (e.g. libcudart.so.13), not absolute /usr/local paths." + Environment.NewLine
            + string.Join(Environment.NewLine, offenders.Take(10)) + (offenders.Count > 10 ? Environment.NewLine + $"...and {offenders.Count - 10} more" : ""));
    }

    // -----------------------------------------------------------------------
    // Issue #37 — surfaceAlignment field must be public (so StructConvert sees it)
    // -----------------------------------------------------------------------

    [TestCase("cudaDeviceProp_130"), TestCase("cudaDeviceProp_131"),
     Description("Issue #37: surfaceAlignment must be a public field for StructConvert to copy it.")]
    public void CudaDeviceProp_SurfaceAlignmentIsPublic(string structName)
    {
        var t = CudaAssembly.GetTypes().FirstOrDefault(x => x.Name == structName);
        Assert.That(t, Is.Not.Null, $"struct {structName} not found in assembly");
        var field = t!.GetField("surfaceAlignment", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        Assert.That(field, Is.Not.Null, $"{structName}.surfaceAlignment not found");
        Assert.That(field!.IsPublic, Is.True,
            $"{structName}.surfaceAlignment must be public (StructConvert iterates BindingFlags.Public only).");
    }

    // -----------------------------------------------------------------------
    // Issue #31 — dead "80" default in nvrtc.GetCudaVersion
    // -----------------------------------------------------------------------

    [Test, Description("Issue #31: the unreachable cudaVersion=\"80\" fallback in nvrtc.cs should be removed.")]
    public void Nvrtc_GetCudaVersion_NoDeadEightyDefault()
    {
        var path = Path.Combine(RepoRoot, "src", "CUDARuntime", "Nvrtc", "nvrtc.cs");
        var text = File.ReadAllText(path);
        Assert.That(text, Does.Not.Contain("cudaVersion = \"80\""),
            "Dead branch `cudaVersion = \"80\"` is still present in nvrtc.cs (cuda.GetCudaVersion never returns null).");
    }

    // -----------------------------------------------------------------------
    // Issue #35 — GCHandle.Alloc must be paired with try/finally in nvrtc Get* methods
    // -----------------------------------------------------------------------

    [Test, Description("Issue #35: GetProgramLog / GetPTX / GetCUBIN must guard their pinned GCHandle with try/finally.")]
    public void Nvrtc_GetMethods_PinHandleInTryFinally()
    {
        // Heuristic: in each nvrtc impl source file, count occurrences of the
        // pin-then-free pattern outside try/finally blocks.
        // The buggy form is:
        //     GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
        //     ...
        //     gch.Free();
        // The fixed form moves the body into try { ... } finally { gch.Free(); }.
        var offenders = new List<string>();
        foreach (var file in NvrtcImplFiles)
        {
            var text = File.ReadAllText(file);
            // Walk method-by-method by splitting on "public nvrtcResult Get".
            foreach (var methodName in new[] { "GetProgramLog", "GetPTX", "GetCUBIN" })
            {
                int sigStart = text.IndexOf("public nvrtcResult " + methodName + "(", StringComparison.Ordinal);
                while (sigStart >= 0)
                {
                    // crude balanced-brace scan to extract the method body
                    int braceStart = text.IndexOf('{', sigStart);
                    if (braceStart < 0) break;
                    int depth = 0; int i = braceStart;
                    while (i < text.Length)
                    {
                        if (text[i] == '{') depth++;
                        else if (text[i] == '}') { depth--; if (depth == 0) break; }
                        i++;
                    }
                    var body = text.Substring(braceStart, Math.Min(i - braceStart + 1, text.Length - braceStart));
                    bool hasAlloc = body.Contains("GCHandle.Alloc(");
                    bool hasTryFinally = body.Contains("try") && body.Contains("finally");
                    if (hasAlloc && !hasTryFinally)
                        offenders.Add($"{Path.GetFileName(file)}::{methodName}");
                    sigStart = text.IndexOf("public nvrtcResult " + methodName + "(", i, StringComparison.Ordinal);
                }
            }
        }
        Assert.That(offenders, Is.Empty,
            "Pinned GCHandle is freed outside a finally block — handle leaks if anything in between throws:" + Environment.NewLine
            + string.Join(Environment.NewLine, offenders));
    }

    // -----------------------------------------------------------------------
    // Issue #38 — unused CUDA_DLLS dictionary should be removed
    // -----------------------------------------------------------------------

    [Test, Description("Issue #38: the unused CUDA_DLLS dictionary should be removed from cuda.cs.")]
    public void Cuda_NoUnusedCudaDllsDictionary()
    {
        var f = typeof(cuda).GetField("CUDA_DLLS", BindingFlags.NonPublic | BindingFlags.Static)
              ?? typeof(cuda).GetField("CUDA_DLLS", BindingFlags.Public | BindingFlags.Static);
        Assert.That(f, Is.Null,
            "cuda.CUDA_DLLS dictionary is still present and unread. Remove it (or wire it into the static-ctor switch).");
    }

    // -----------------------------------------------------------------------
    // Issue #39 — JittedModule.GetEntryPoint must not silently swallow CUresult
    // -----------------------------------------------------------------------

    // -----------------------------------------------------------------------
    // Issue #36 — soname resolution end-to-end
    //
    // These tests load the real CUDA shared libraries via the wrapper's
    // [DllImport]. They self-skip on hosts without the toolkit (e.g. CI
    // runners) by trapping DllNotFoundException at the first native call.
    // -----------------------------------------------------------------------

    private static T SkipIfCudaUnavailable<T>(Func<T> probe, string libName)
    {
        try { return probe(); }
        catch (DllNotFoundException ex)
        {
            Assert.Ignore($"{libName} is not available on this host ({ex.Message}) — skipping native soname test.");
            return default;
        }
        catch (TypeInitializationException ex) when (ex.InnerException is DllNotFoundException)
        {
            Assert.Ignore($"{libName} is not available on this host ({ex.InnerException.Message}) — skipping native soname test.");
            return default;
        }
    }

    [Test, Platform(Include = "Linux"),
     Description("Issue #36: libcudart.so.<MAJOR> must resolve through ldconfig — calls a no-device API to confirm.")]
    public void Linux_LibcudartSoname_Resolves()
    {
        // cudaGetErrorString is a pure runtime helper — no driver, no GPU
        // needed — but it does require libcudart.so.<MAJOR> to load.
        var msg = SkipIfCudaUnavailable(() => cuda.GetErrorString(cudaError_t.cudaSuccess), "libcudart.so.<MAJOR>");
        Assert.That(msg, Is.Not.Null.And.Not.Empty);
        TestContext.Out.WriteLine($"cudaGetErrorString(cudaSuccess) → \"{msg}\"");
    }

    [Test, Platform(Include = "Linux"),
     Description("Issue #36: libnvrtc.so.<MAJOR> must resolve through ldconfig — calls nvrtcVersion to confirm.")]
    public void Linux_LibnvrtcSoname_Resolves()
    {
        int major = -1, minor = -1;
        var res = SkipIfCudaUnavailable(() =>
        {
            int M = -1, m = -1;
            var r = nvrtc.Version(out M, out m);
            major = M; minor = m;
            return r;
        }, "libnvrtc.so.<MAJOR>");
        Assert.That(res, Is.EqualTo(nvrtcResult.NVRTC_SUCCESS),
            $"nvrtcVersion returned {res}");
        Assert.That(major, Is.GreaterThan(0));
        TestContext.Out.WriteLine($"nvrtcVersion → {major}.{minor}");
    }

    [Test, Platform(Include = "Linux"),
     Description("CUDA 13.2 explicit dispatch must select the new wrappers and still resolve the runtime/nvrtc libs.")]
    public void Cuda132_ExplicitDispatch_Works()
    {
        var versionField = typeof(cuda).GetField("_cudaversion", BindingFlags.Static | BindingFlags.NonPublic);
        var nvrtcInstanceField = typeof(nvrtc).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
        var savedVersion = versionField?.GetValue(null);
        var savedCudaInstance = cuda.instance;
        var savedNvrtcInstance = nvrtcInstanceField?.GetValue(null);
        try
        {
            cuda.SetCudaVersion("13.2");

            Assert.That(cuda.GetCudaVersion(), Is.EqualTo("132"));
            Assert.That(cuda.instance.GetType().Name, Does.Contain("132"),
                $"cuda.instance is {cuda.instance.GetType().Name}, expected a 132 wrapper");

            // Hit native code through the freshly-selected wrapper. Skip
            // gracefully if the toolkit isn't installed (CI / non-CUDA host).
            var err = SkipIfCudaUnavailable(() => cuda.GetErrorString(cudaError_t.cudaSuccess), "libcudart.so.13");
            Assert.That(err, Is.Not.Null.And.Not.Empty);

            int major = -1, minor = -1;
            var rc = SkipIfCudaUnavailable(() =>
            {
                int M = -1, m = -1;
                var r = nvrtc.Version(out M, out m);
                major = M; minor = m;
                return r;
            }, "libnvrtc.so.13");
            Assert.That(rc, Is.EqualTo(nvrtcResult.NVRTC_SUCCESS));
            TestContext.Out.WriteLine($"cuda 13.2 dispatch: cudaGetErrorString=\"{err}\", nvrtcVersion={major}.{minor}");
        }
        finally
        {
            versionField?.SetValue(null, savedVersion);
            cuda.instance = savedCudaInstance;
            nvrtcInstanceField?.SetValue(null, savedNvrtcInstance);
        }
    }

    [Test, Description("Issue #39: a public JittedModule API must propagate the CUresult from ModuleGetFunction (overload returning CUresult, Try* method, or out CUfunction param).")]
    public void GetEntryPoint_ExposesNativeError()
    {
        var t = typeof(JittedModule);
        var stringEntryPoints = t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == "GetEntryPoint" || m.Name == "TryGetEntryPoint")
            .Where(m =>
            {
                var ps = m.GetParameters();
                return ps.Length >= 1 && ps[0].ParameterType == typeof(string);
            })
            .ToList();

        bool exposesError = stringEntryPoints.Any(m =>
            m.ReturnType == typeof(CUresult)
            || m.GetParameters().Any(p => p.IsOut && p.ParameterType.GetElementType() == typeof(CUfunction)));

        Assert.That(exposesError, Is.True,
            "JittedModule.GetEntryPoint(string) discards the CUresult from ModuleGetFunction. "
            + "Add an overload that returns CUresult, or a TryGetEntryPoint with `out CUfunction`.");
    }
}
