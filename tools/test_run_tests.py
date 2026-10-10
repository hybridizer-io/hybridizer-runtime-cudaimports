"""Safety checks for required test reports and bounded child processes."""
import importlib.util
import os
from pathlib import Path
import sys
import tempfile
import unittest

spec = importlib.util.spec_from_file_location("runner", Path(__file__).with_name("run-tests.py"))
runner = importlib.util.module_from_spec(spec)
spec.loader.exec_module(runner)

class HarnessTests(unittest.TestCase):
    def test_missing_empty_or_skipped_report_is_not_success(self):
        with tempfile.TemporaryDirectory() as folder:
            report = Path(folder) / "results.trx"
            for content in (None, "<TestRun/>",
                            '<TestRun><UnitTestResult testName="required" outcome="NotExecuted"/></TestRun>',
                            '<TestRun><UnitTestResult testName="ok" outcome="Passed"/>'
                            '<UnitTestResult testName="required" outcome="NotExecuted"/></TestRun>'):
                with self.subTest(content=content):
                    if content is not None:
                        report.write_text(content)
                    with self.assertRaises(RuntimeError):
                        runner.read_results(report, "gpu")

    def test_failed_test_is_not_success_even_with_a_passing_test(self):
        with tempfile.TemporaryDirectory() as folder:
            report = Path(folder) / "results.trx"
            report.write_text('<TestRun><UnitTestResult testName="ok" outcome="Passed"/>'
                              '<UnitTestResult testName="broken" outcome="Failed"/></TestRun>')
            with self.assertRaises(RuntimeError):
                runner.read_results(report, "fast")

    def test_namespaced_passing_report_is_accepted(self):
        with tempfile.TemporaryDirectory() as folder:
            report = Path(folder) / "results.trx"
            report.write_text('<TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">'
                              '<Results><UnitTestResult testName="required" outcome="Passed"/></Results></TestRun>')
            self.assertEqual(runner.read_results(report, "gpu")[0]["name"], "required")

    def test_timeout_terminates_a_real_child(self):
        with tempfile.TemporaryDirectory() as folder:
            log = Path(folder) / "timeout.log"
            with self.assertRaisesRegex(RuntimeError, "terminated its process tree"):
                runner.run([sys.executable, "-c", "import time; time.sleep(30)"],
                           folder, os.environ.copy(), log, 0.2)

if __name__ == "__main__":
    unittest.main()
