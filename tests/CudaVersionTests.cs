using System;
using System.IO;
using NUnit.Framework;

namespace Hybridizer.Runtime.CUDAImports.Tests {

    public class CudaVersionTests
    {
        [Test]
        public void SetCudaVersionBase() 
        {
            cuda.SetCudaVersion("13.0");
            Assert.That(cuda.GetCudaVersion(), Is.EqualTo("130"));
        } 
        
        [Test]
        public void SetCudaVersionOverridesEnv() 
        {
            Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", "12.4");
            cuda.SetCudaVersion("13.0");
            Assert.That(cuda.GetCudaVersion(), Is.EqualTo("130"));
        } 

        
        [Test]
        public void CudaVersionEnv() 
        {
            Environment.SetEnvironmentVariable("HYBRIDIZER_CUDA_VERSION", "12.4");
            Assert.That(cuda.GetCudaVersion(), Is.EqualTo("124"));
        } 

        
        [Test]
        public void CudaVersionSetting() 
        {
            var settingPath = Path.Combine(AppContext.BaseDirectory, "cuda.settings.json");
            File.WriteAllText(settingPath, @"{ ""CudaVersion"": ""12.4"" }");
            Assert.That(cuda.GetCudaVersion(), Is.EqualTo("124"));
            File.Delete(settingPath);
        } 

        
        [Test]
        public void CudaVersionDefaultsToLatest() 
        {
            Assert.That(cuda.GetCudaVersion(), Is.EqualTo("131"));
        } 
    }
}