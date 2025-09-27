using System;
using System.Diagnostics;
using System.IO;

namespace Plisky.FlimFlam {
    /// <summary>
    /// Responsible for locating and launching the ff-ods helper process used for ODS data gathering.
    /// It will look for the executable in the current directory first, then two directories up under
    /// ff-ods/bin/Debug. Maintains a single Process reference; will only launch if not already running.
    /// </summary>
    internal static class OdsProcessGatherer {

        private static readonly object sync = new object();
        private static Process odsProcess;

        /// <summary>
        /// Ensures that the ff-ods process is running. Attempts to locate the executable in the current
        /// directory, or alternatively two levels up in ff-ods/bin/Debug. Returns false if the executable
        /// cannot be located or fails to start; true if already running or started successfully.
        /// </summary>
        internal static bool EnsureProcessRunning() {
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

        /// <summary>
        /// Attempts to terminate the ff-ods process if it is currently running.
        /// Returns true if a running process was found (and a kill was attempted), false if no process reference existed.
        /// </summary>
        internal static bool TryKillProcess() {
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
        private static string LocateExecutable() {
            string currentDir = Directory.GetCurrentDirectory();
            string primary = Path.Combine(currentDir, "ff-ods.exe");
            if (File.Exists(primary)) { return primary; }

            try {
                var parent = Directory.GetParent(currentDir);
                var grandParent = parent?.Parent;
                if (grandParent != null) {
                    string secondary = Path.Combine(grandParent.FullName, "ff-ods", "bin", "Debug", "ff-ods.exe");
                    if (File.Exists(secondary)) { return secondary; }
                }
            } catch (UnauthorizedAccessException) {
            } catch (DirectoryNotFoundException) {
            }

            return null;
        }
    }
}
