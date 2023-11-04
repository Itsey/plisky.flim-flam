//using Plisky.Plumbing.Legacy;
using System;

namespace Plisky.FlimFlam { 

    internal interface IDiagnosticSupport {

        bool GetDiagnosticByIndex(out string name, out string val);

        string GetDiagnosticGroup();

        int GetDiagnosticStats();
    }

    /// <summary>
    /// Summary description for MexDiagnosticManager.
    /// </summary>
    internal class MexDiagnosticManager {
        private string m_machineName;

        internal MexDiagnosticManager() {
            //
            // TODO: Add constructor logic here
            //
        }

        internal string ThisMachineName {
            get {
                if (m_machineName == null) {
                    m_machineName = Environment.MachineName;
                }
                return m_machineName;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void DumpInternalMessages() {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void ShutDown() {
            //Bilge.Log("Mex::DiagnosticManager::Shutdown >> Mex Diagnostics Shutting down");
        }

        // end ThisMachineName
    }
}