using System;
using System.IO;
using System.Reflection;

namespace Hybridizer.Runtime.CUDAImports.Tests;

/// <summary>
/// Serial, managed-only configuration sandbox. Native-library loading is not
/// reversible: native suites must run in separate testhost processes.
/// </summary>
internal sealed class CudaTestScope : IDisposable
{
    internal static readonly string SettingsPath = Path.Combine(AppContext.BaseDirectory, "cuda.settings.json");
    private readonly string environment = Environment.GetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION");
    private readonly string backup = SettingsPath + "." + Guid.NewGuid().ToString("N") + ".backup";
    private readonly FieldInfo versionField = typeof(cuda).GetField("_cudaversion", BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new MissingFieldException(typeof(cuda).FullName, "_cudaversion");
    internal static readonly FieldInfo NvrtcInstance = typeof(nvrtc).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic)
        ?? throw new MissingFieldException(typeof(nvrtc).FullName, "instance");
    private object version;
    private ICuda cudaInstance;
    private object nvrtcInstance;
    private bool savedState;
    private bool ownsSettings;
    private bool disposed;

    internal CudaTestScope()
    {
        try
        {
            // Move instead of rewriting the caller's file, preserving bytes and metadata.
            if (File.Exists(SettingsPath)) File.Move(SettingsPath, backup);
            ownsSettings = true;
            Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", null);
            version = versionField.GetValue(null);
            cudaInstance = cuda.instance;
            nvrtcInstance = NvrtcInstance.GetValue(null);
            savedState = true;
            cuda.SetCudaVersion("13.1");
            versionField.SetValue(null, string.Empty);
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;
        try
        {
            if (savedState)
            {
                versionField.SetValue(null, version);
                cuda.instance = cudaInstance;
                NvrtcInstance.SetValue(null, nvrtcInstance);
            }
        }
        finally
        {
            try
            {
                if (ownsSettings && File.Exists(SettingsPath)) File.Delete(SettingsPath);
                if (File.Exists(backup)) File.Move(backup, SettingsPath);
            }
            finally
            {
                Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", environment);
            }
        }
    }
}
