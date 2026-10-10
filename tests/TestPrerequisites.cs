using System;
using NUnit.Framework;

namespace Hybridizer.Runtime.CUDAImports.Tests;

internal static class TestPrerequisites
{
    internal static void Missing(string message)
    {
        if (Environment.GetEnvironmentVariable("HYBRIDIZER_TEST_REQUIRED") == "1")
            Assert.Fail(message);
        Assert.Ignore(message + " Select the required suite with tools/run-tests.py to enforce this prerequisite.");
    }
}
