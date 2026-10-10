using System;
using System.IO;
using NUnit.Framework;

namespace Hybridizer.Runtime.CUDAImports.Tests;

[NonParallelizable]
public class CudaVersionTests
{
    [Test]
    public void SetCudaVersionBase()
    {
        using var scope = new CudaTestScope();
        cuda.SetCudaVersion("13.0");
        Assert.That(cuda.GetCudaVersion(), Is.EqualTo("130"));
    }

    [Test]
    public void SetCudaVersionOverridesEnv()
    {
        using var scope = new CudaTestScope();
        Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", "12.4");
        cuda.SetCudaVersion("13.0");
        Assert.That(cuda.GetCudaVersion(), Is.EqualTo("130"));
    }

    [Test]
    public void CudaVersionEnv()
    {
        using var scope = new CudaTestScope();
        Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", "12.4");
        Assert.That(cuda.GetCudaVersion(), Is.EqualTo("124"));
    }

    [Test]
    public void CudaVersionSetting()
    {
        using var scope = new CudaTestScope();
        File.WriteAllText(CudaTestScope.SettingsPath, @"{ ""CudaVersion"": ""12.4"" }");
        Assert.That(cuda.GetCudaVersion(), Is.EqualTo("124"));
    }

    [Test]
    public void CudaVersionDefaultsToLatest()
    {
        using var scope = new CudaTestScope();
        Assert.That(cuda.GetCudaVersion(), Is.EqualTo("131"));
    }

    [TestCase(false), TestCase(true)]
    public void ConfigurationTests_AreIndependentOfOrder(bool reverse)
    {
        using var scope = new CudaTestScope();
        Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", "13.0");
        Action[] cases = { SetCudaVersionBase, SetCudaVersionOverridesEnv,
            CudaVersionEnv, CudaVersionSetting, CudaVersionDefaultsToLatest };
        if (reverse) Array.Reverse(cases);
        foreach (var test in cases)
        {
            TestContext.Out.WriteLine(test.Method.Name);
            test();
            Assert.That(cuda.GetCudaVersion(), Is.EqualTo("130"), test.Method.Name);
            Assert.That(File.Exists(CudaTestScope.SettingsPath), Is.False, test.Method.Name);
        }
    }

    [TestCase(false), TestCase(true)]
    public void Sandbox_RestoresStateAfterException(bool existingSettings)
    {
        using var outer = new CudaTestScope();
        var originalBytes = System.Text.Encoding.UTF8.GetBytes("{ \"CudaVersion\": \"12.6\" }\r\n");
        if (existingSettings) File.WriteAllBytes(CudaTestScope.SettingsPath, originalBytes);
        Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", "12.4");
        cuda.SetCudaVersion("13.0");
        var originalCuda = cuda.instance;
        var originalNvrtc = CudaTestScope.NvrtcInstance.GetValue(null);
        Assert.Throws<InvalidOperationException>(() =>
        {
            using var inner = new CudaTestScope();
            File.WriteAllText(CudaTestScope.SettingsPath, "temporary invalid JSON");
            Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", "11.4");
            cuda.SetCudaVersion("13.2");
            throw new InvalidOperationException("Simulated assertion failure");
        });
        Assert.That(Environment.GetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION"), Is.EqualTo("12.4"));
        Assert.That(cuda.GetCudaVersion(), Is.EqualTo("130"));
        Assert.That(cuda.instance, Is.SameAs(originalCuda));
        Assert.That(CudaTestScope.NvrtcInstance.GetValue(null), Is.SameAs(originalNvrtc));
        Assert.That(File.Exists(CudaTestScope.SettingsPath), Is.EqualTo(existingSettings));
        if (existingSettings) Assert.That(File.ReadAllBytes(CudaTestScope.SettingsPath), Is.EqualTo(originalBytes));
    }
}
