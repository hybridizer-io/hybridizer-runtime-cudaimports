# Runtime test harness

Requires Python 3.9+ and the .NET 8 SDK. From this repository:

```sh
python tools/run-tests.py --suite fast --repeat 2 --results ../results-fast
python tools/run-tests.py --suite native --repeat 2 --results ../results-native
HYBRIDIZER_CUDA_VERSION=13.0 python tools/run-tests.py --suite native-cuda --repeat 2 --reverse --results ../results-native-cuda
HYBRIDIZER_CUDA_VERSION=13.0 python tools/run-tests.py --suite gpu --repeat 2 --results ../results-gpu
```

Use a **new output directory outside this runtime checkout** for each invocation.
The runner copies source without bin/obj, builds Release/x64, and starts a new
testhost for each group. It leaves source and caller environment untouched.
Each repetition rebuilds a fresh copy. TRX, stdout/stderr, build logs and
summary.json remain under the chosen output directory, including on failure.
Processes have a five-minute build limit and a three-minute test limit;
--timeout adjusts the latter (1..3600 seconds). A timeout kills the process tree.
Exit code 0 requires a nonempty result file and passing required tests, and
identical test names/outcomes across repetitions.

| Suite | Requires | Contents |
|---|---|---|
| fast (default) | .NET only; no GPU/toolkit/compiler | Managed contract, marshalling and configuration tests; small native-memory allocations are freed after each test |
| native | Linux, gcc (or --cc PATH) | 20 HybRunner IL tests with a compiled stub; compile failures and the 15-second compiler timeout fail the suite |
| native-cuda | Linux CUDA runtime + NVRTC libraries | Three native dispatch/soname probes, each in its own process; --reverse changes their execution order |
| gpu | CUDA driver, compatible toolkit, GPU | 11 device allocation/copy/cleanup tests; CUDA category retained for compatibility, GPU category added |
| stress | Explicit memory budget | AllocateLarge only; no automatic execution in normal dotnet test or CI |

CUDA configuration tests use a serial sandbox that restores the caller's
HYBRIDIZER_CUDA_VERSION, cuda.settings.json (original file moved back, not rewritten),
cuda selected version/instance and nvrtc instance. They explicitly exercise both
scenario orders and failure cleanup, with and without a preexisting settings file.
This resets **managed selection only**, not loaded native libraries.
Use the suite runner for native/GPU tests; don't combine all tests in one testhost
and assume a DLL load can be undone by changing a field.

The Windows fast suite reports two explicit platform skips:
DriverApi_DoesNotHardcodeWindowsDll_OnLinux and LinuxDlls_AreNotHardcodedAbsolutePaths.
All other skips fail the required runner. Windows stub integration and the
Linux-specific soname checks are unsupported and return a clear error when
requested on Windows. They are not counted as validated Windows coverage.
Optional direct NUnit runs may skip missing CUDA prerequisites; the runner sets
HYBRIDIZER_TEST_REQUIRED=1 so selected jobs cannot pass via those skips.
The existing GUID/attribute and reflection checks remain in the fast suite.
Commented ICudaTests/ResidentArray code is not part of its executed-test count.

## Stress budget

```sh
# Small opt-in validation of the allocation/cleanup path: >= 32 MiB total payload.
python tools/run-tests.py --suite stress --stress-doubles 2097152 --results ../results-stress-small
# Original size: >= 16,000,000,000 bytes (14.9 GiB), PLUS CLR/runner/system headroom.
# Only on a dedicated host with sufficient available RAM:
python tools/run-tests.py --suite stress --stress-doubles 1000000000 --timeout 600 --results ../results-stress-large
```

The budget is 8*N managed bytes plus at least 8*N native bytes, excluding runtime
overhead; count is bounded to 1..1,000,000,000. The upper bound retains the original
case; a small run does **not** qualify the 8 GB array path. The test is NUnit
Explicit and also requires the runner's opt-in environment variables.
Do not interpret test durations (including copies/restores/builds) as benchmarks.

## CI and other repositories

The runtime workflow runs fast on Windows/Linux and the native stub on Linux.
GPU/native-CUDA jobs need provisioned hardware/toolkits and are run explicitly;
a hosted CPU runner is not a GPU validation. The parent repository invokes the
same runner at its pinned submodule commit; this repository has no dependency on
the parent scripts.

GeneralTestsCode satellites and hybridizer-basic-samples (including tiny-llama)
are separate integration suites, not inputs to this quick runtime command.
Their manifests, bounded datasets and mandatory satellite checks are tracked in
hybridizer-software-suite #280–#283. In particular, tiny-llama's current satellite
skip must not count as GPU coverage. Product defect reproducers remain visible;
do not change assertions or product behavior to make this harness green.
