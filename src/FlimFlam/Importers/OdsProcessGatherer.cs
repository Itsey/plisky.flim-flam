using System;
using System.Diagnostics;
using System.IO;

namespace Plisky.FlimFlam {
    /// <summary>
    /// Manages thee external process to capture output debug strings.  Due to stability issues with FF the unsafe code has been moved into an external
    /// process which is then managed by this class. That external process uses the TCP gatherer to send the messages back to the main application.
    /// </summary>
    public class OdsProcessGatherer {

        private readonly object sync = new object();
        private Process odsProcess;
        private const string ODSIMPORTNAME = "ff-ods.exe";

        public virtual bool StartOdsGathererProcess() {
            lock (sync) {
                if (odsProcess != null) {
                    try {
                        if (!odsProcess.HasExited) {
                            return true; // Already running
                        }
                    } catch (InvalidOperationException) {
                        // Process handle no longer valid; will attempt restart.
                    }
                    odsProcess.Dispose();
                    odsProcess = null;
                }

                string exePath = LocateExecutable();
                if (exePath == null) {
                    return false; // Could not find the executable in either location.
                }

                try {
                    var psi = new ProcessStartInfo {
                        FileName = exePath,
                        WorkingDirectory = Path.GetDirectoryName(exePath) ?? Environment.CurrentDirectory,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    odsProcess = Process.Start(psi);
                    if (odsProcess == null) { return false; }
                    return true;
                } catch (Exception) {
                    // Swallow and return false; caller can decide how to log/notify.
                    odsProcess = null;
                    return false;
                }
            }
        }


        public virtual bool StopOdsGathererProcess() {
            lock (sync) {
                if (odsProcess == null) { return false; }
                try {
                    if (!odsProcess.HasExited) {

                        odsProcess.Kill(entireProcessTree: true);

                    }
                } finally {
                    try { odsProcess.Dispose(); } catch { }
                    odsProcess = null;
                }
                return true;
            }
        }

        /// <summary>
        /// Returns the full path to ff-ods.exe if it can be found using the search rules; otherwise null.
        /// </summary>
        private static string? LocateExecutable() {
            string currentDir = Directory.GetCurrentDirectory();
            string matchedPath = null;

            foreach(string pathToCheck in MexCore.TheCore.Options.PathsToCheckForImporters) { 
                string testPath = Path.Combine(currentDir, pathToCheck, ODSIMPORTNAME);

                if (File.Exists(testPath)) {
                    matchedPath = testPath;
                    break;
                }
            };

            return matchedPath;
        }
    }
}
