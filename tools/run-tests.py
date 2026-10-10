#!/usr/bin/env python3
"""Run a bounded runtime test suite from a clean source copy (Python 3.9+, .NET 8 SDK)."""
import argparse
import collections
import json
import os
from pathlib import Path
import platform
import shutil
import signal
import subprocess
import sys
import time
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
PROJECT = Path("tests/Hybridizer.Runtime.CUDAImports.Tests.csproj")
FAST = "TestCategory!=CUDA&TestCategory!=GPU&TestCategory!=Native&TestCategory!=NativeCuda&TestCategory!=Stress"
NATIVE_CUDA = ["Linux_LibcudartSoname_Resolves", "Linux_LibnvrtcSoname_Resolves", "Cuda132_ExplicitDispatch_Works"]
WINDOWS_SKIPS = {"DriverApi_DoesNotHardcodeWindowsDll_OnLinux", "LinuxDlls_AreNotHardcodedAbsolutePaths"}

def run(command, cwd, env, log, timeout):
    print("Running: " + subprocess.list2cmdline(command), flush=True)
    with log.open("w", encoding="utf-8") as output:
        process = subprocess.Popen(command, cwd=cwd, env=env, stdout=output,
                                   stderr=subprocess.STDOUT, start_new_session=os.name != "nt")
        try:
            return process.wait(timeout=timeout)
        except subprocess.TimeoutExpired:
            if os.name == "nt":
                subprocess.run(["taskkill", "/PID", str(process.pid), "/T", "/F"],
                               stdout=output, stderr=subprocess.STDOUT, timeout=15)
            else:
                os.killpg(process.pid, signal.SIGKILL)
            process.wait(timeout=15)
            raise RuntimeError(f"Process exceeded {timeout}s; terminated its process tree. See {log}")

def read_results(path, suite):
    if not path.is_file():
        raise RuntimeError(f"Missing required TRX: {path}")
    document = ET.parse(path)
    tests = []
    for result in document.findall(".//{*}UnitTestResult"):
        message = result.find(".//{*}Message")
        tests.append({"name": result.get("testName"), "outcome": result.get("outcome"),
                      "message": message.text if message is not None else ""})
    if not tests:
        raise RuntimeError(f"No tests executed: {path}")
    invalid = [test for test in tests if test["outcome"] != "Passed" and not (
        suite == "fast" and platform.system() == "Windows"
        and test["outcome"] == "NotExecuted" and test["name"] in WINDOWS_SKIPS)]
    if not any(test["outcome"] == "Passed" for test in tests):
        raise RuntimeError(f"No passing tests in required suite: {path}")
    if invalid:
        raise RuntimeError("Required tests failed or skipped: " + json.dumps(invalid, ensure_ascii=False))
    return tests

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--suite", choices=["fast", "native", "native-cuda", "gpu", "stress"], default="fast")
    parser.add_argument("--results", type=Path, required=True, help="New output directory outside the runtime checkout")
    parser.add_argument("--repeat", type=int, default=1, choices=range(1, 11))
    parser.add_argument("--reverse", action="store_true", help="Reverse native CUDA subprocess order; configuration tests exercise both orders in every fast run")
    parser.add_argument("--timeout", type=int, default=180, help="Per-testhost timeout in seconds (1..3600)")
    parser.add_argument("--stress-doubles", type=int, help="Explicit stress size: 1..1,000,000,000; at least 16*N bytes RAM")
    parser.add_argument("--cc", help="Native stub compiler executable (default gcc)")
    args = parser.parse_args()
    if not 1 <= args.timeout <= 3600:
        parser.error("--timeout must be in 1..3600")
    if args.suite == "stress" and (args.stress_doubles is None or not 1 <= args.stress_doubles <= 1_000_000_000):
        parser.error("stress requires --stress-doubles in 1..1,000,000,000 (minimum RAM = 16*N bytes)")
    results = args.results.resolve()
    if results == ROOT or ROOT in results.parents:
        parser.error("--results must be outside the runtime checkout (avoids recursive copying)")
    results.mkdir(parents=True, exist_ok=False)
    summary = {"suite": args.suite, "platform": platform.platform(), "runs": [], "status": "failed"}
    exit_code = 1
    try:
        dotnet = shutil.which("dotnet")
        if not dotnet:
            raise RuntimeError(".NET SDK missing: dotnet is not on PATH")
        if args.suite in ("native", "native-cuda") and platform.system() != "Linux":
            raise RuntimeError("This native suite requires Linux. The Windows ABI stub/soname probes are not ported.")
        for number in range(1, args.repeat + 1):
            folder = results / f"run-{number}"
            work = folder / "work"
            shutil.copytree(ROOT, work, ignore=shutil.ignore_patterns(
                ".git", "bin", "obj", "TestResults", "__pycache__", ".vs"))
            temp = folder / "temp"
            temp.mkdir()
            env = os.environ.copy()
            env.update({"HYBRIDIZER_TEST_REQUIRED": "1", "DOTNET_CLI_TELEMETRY_OPTOUT": "1",
                        "TMP": str(temp), "TEMP": str(temp), "TMPDIR": str(temp)})
            if args.cc:
                env["HYBRIDIZER_TEST_CC"] = args.cc
            if args.suite == "stress":
                env.update({"HYBRIDIZER_RUN_STRESS": "1", "HYBRIDIZER_STRESS_DOUBLES": str(args.stress_doubles)})
            build = run([dotnet, "build", str(PROJECT), "-c", "Release", "-p:Platform=x64", "-v", "quiet"],
                        work, env, folder / "build.log", 300)
            if build:
                raise RuntimeError(f"Build failed ({build}); see {folder / 'build.log'}")
            if args.suite == "native-cuda":
                groups = [(name, "FullyQualifiedName~." + name) for name in NATIVE_CUDA]
                if args.reverse:
                    groups.reverse()
            else:
                filters = {"fast": FAST, "native": "TestCategory=Native", "gpu": "TestCategory=GPU",
                           "stress": "FullyQualifiedName~MarshallerTests.AllocateLarge"}
                groups = [(args.suite, filters[args.suite])]
            all_tests = []
            for name, expression in groups:
                output = folder / name
                output.mkdir()
                command = [dotnet, "test", str(PROJECT), "-c", "Release", "-p:Platform=x64",
                           "--no-build", "--no-restore", "--filter", expression,
                           "--results-directory", str(output), "--logger", "trx;LogFileName=results.trx",
                           "-v", "normal", "--", "NUnit.NumberOfTestWorkers=0"]
                started = time.monotonic()
                rc = run(command, work, env, output / "test.log", args.timeout)
                tests = read_results(output / "results.trx", args.suite)
                if rc:
                    raise RuntimeError(f"Testhost failed ({rc}); see {output / 'test.log'}")
                all_tests.extend(tests)
                summary["runs"].append({"repeat": number, "group": name, "seconds": time.monotonic() - started,
                                        "counts": dict(collections.Counter(t["outcome"] for t in tests)), "tests": tests})
            leftovers = list(temp.glob("hybrunner_il_tests_*")) + list(work.glob("tests/bin/**/cuda.settings.json*"))
            if leftovers:
                raise RuntimeError("Test cleanup left state behind: " + str(leftovers))
            signature = sorted((t["name"], t["outcome"]) for t in all_tests)
            if number == 1:
                baseline = signature
            elif signature != baseline:
                raise RuntimeError("Repeated execution changed test identities or outcomes")
        summary["status"] = "passed"
        exit_code = 0
    except Exception as exc:
        summary["error"] = str(exc)
        print(str(exc), file=sys.stderr)
    finally:
        (results / "summary.json").write_text(json.dumps(summary, indent=2, ensure_ascii=False), encoding="utf-8")
    print(f"{args.suite}: {summary['status']}; results: {results}", flush=True)
    return exit_code

if __name__ == "__main__":
    sys.exit(main())
