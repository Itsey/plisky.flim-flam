//using Plisky.Plumbing.Legacy;
namespace Plisky.FlimFlam {
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using Plisky.Diagnostics.FlimFlam;

    internal class ODSDataGathererThread {

        private ODSDataGathererThread() {
        }

        internal static volatile bool continueRunning = true;

        internal static void InterceptODS() {
            //Bilge.Log("InterceptODS Thread started");

            continueRunning = true;

            unsafe { // Reads ODS Data from Swapfile
                ODSWin32Interface.SECURITY_ATTRIBUTES securityAttr;
                IntPtr hBufferReadyEvent, hDataReadyEvent, hSwapFileBuffer;
                int* viewOfSwapFile = null;
                int* ptrToPID;
                byte* pcharToDebugData;
                uint waitreturn;

                hBufferReadyEvent = 0;
                hDataReadyEvent = 0;
                hSwapFileBuffer = 0;

                // Allocate some memory for the security descriptor and lock it from the GC
                byte[] tmem = new byte[ODSWin32Interface.SECURITY_DESCRIPTOR_MIN_LENGTH];
                fixed (byte* securityDescr = tmem) {
                    try {

                        #region get security descriptor / dacl

                        if (!ODSWin32Interface.InitializeSecurityDescriptor((IntPtr)securityDescr, ODSWin32Interface.SECURITY_DESCRIPTOR_REVISION)) {
                            // Could not initialise security descriptor
                            int lastError = Marshal.GetLastWin32Error();
                            throw new Win32Exception("Win 32 Error " + lastError.ToString() + "While calling InitialiseSecurityDescriptor");
                        }

                        if (!ODSWin32Interface.SetSecurityDescriptorDacl((IntPtr)securityDescr, true, (IntPtr)null, false)) {
                            int lastError = Marshal.GetLastWin32Error();
                            throw new Win32Exception("Win 32 Error " + lastError.ToString() + " While trying to create null DACL");
                        }

                        // Now populate the parts of the security attributes structure that we are going to use.
                        securityAttr.nLength = (uint)sizeof(ODSWin32Interface.SECURITY_ATTRIBUTES);
                        securityAttr.bIneritHandle = true;
                        securityAttr.lpSecurityDescriptor = (IntPtr)securityDescr;

                        #endregion get security descriptor / dacl

                        #region set up access to swapfile

                        // Now we need to create the named events that notify us of changes to the
                        // T.ug string entries in the swap file.
                        var psecurityAttr = &securityAttr;

                        int errorFromApi = 0;
                        hBufferReadyEvent = ODSWin32Interface.CreateEvent((IntPtr)psecurityAttr, false, false, "DBWIN_BUFFER_READY");
                        errorFromApi = Marshal.GetLastWin32Error();
                        if (hBufferReadyEvent == 0) {
                            throw new Win32Exception("Unable to create the DBWIN_BUFFER_READY event, error:" + errorFromApi.ToString());
                        }

                        hDataReadyEvent = ODSWin32Interface.CreateEvent((IntPtr)psecurityAttr, false, false, "DBWIN_DATA_READY");
                        errorFromApi = Marshal.GetLastWin32Error();
                        if (hDataReadyEvent == 0) {
                            throw new Win32Exception("Unable to create DBWIN_DATA_READY event, error:" + errorFromApi.ToString());
                        }

                        IntPtr tempVal = -1;

                        hSwapFileBuffer = ODSWin32Interface.CreateFileMapping(tempVal, (IntPtr)psecurityAttr, ODSWin32Interface.PAGE_READWRITE, 0, 4096, "DBWIN_BUFFER");
                        errorFromApi = Marshal.GetLastWin32Error();
                        if (hSwapFileBuffer == 0) {
                            throw new Win32Exception("Failed miserably to get a mapping of the swap file, Error:" + errorFromApi.ToString());
                        }

                        // Now that we have the events and swap file buffer ready time to create a view of
                        // the windows swap file .....
                        viewOfSwapFile = (int*)ODSWin32Interface.MapViewOfFile(hSwapFileBuffer, ODSWin32Interface.FILE_MAP_READ, 0, 0, 512);

                        ptrToPID = viewOfSwapFile;         // Address of the entry starting with the pid
                        pcharToDebugData = (byte*)viewOfSwapFile; // Text 2 bytes further on

                        // Now we have created our buffer and registered all of the events. Now the code
                        // must loop waiting for the events to be signalled.

                        #endregion set up access to swapfile

                        ODSWin32Interface.SetEvent(hBufferReadyEvent);

                        while (continueRunning) {
                            try {
#if false
                                waitreturn = ODSWin32Interface.WaitForSingleObject(hDataReadyEvent, 50000);
#else
                                waitreturn = ODSWin32Interface.WaitForSingleObject(hDataReadyEvent, 5000);
#endif
                                if (waitreturn == ODSWin32Interface.WAIT_TIMEOUT) {
                                    // Have had issues where this does not get set and the ODS call hangs on apps, causing a deadlock
                                    // this only seems to occur when Mex is running therefore have put this set in.
                                    ODSWin32Interface.SetEvent(hBufferReadyEvent);
                                    continue;
                                }

                                if (waitreturn == ODSWin32Interface.WAIT_ABANDONED) {
                                    break;
                                }
                                //Bilge.TimeStart("ODSCapture", "ODS");
                                // Successfull wait notification

                                int pid = *ptrToPID;
                                pcharToDebugData = (byte*)ptrToPID;

                                pcharToDebugData += 4; // The first 4 bytes are the PID, the rest is the ODS string
                                // The text is 4 bytes in from the start therefore 2 incs should do it
                                byte* findLen = pcharToDebugData;
                                int strlenctr = 0;

                                // loop along the swap file one char at a time
                                while (*findLen != 0) {
                                    findLen++;
                                    strlenctr++;
                                    if (strlenctr > 4096) {
                                        throw new InvalidOperationException("No terminating zero found in 4k of swapfile");
                                    }
                                }

                                string s = new((sbyte*)pcharToDebugData, 0, strlenctr, System.Text.Encoding.Default);
                                //Bilge.TimeStop("ODSCapture", "ODS");

                                if (s.Length > 0) {
                                    //Bilge.TimeStart("AddIncomming", "ODS");
                                    MexCore.TheCore.MessageManager.AddIncomingMessage(InternalSource.ODSCapture, s, pid);
                                    //Bilge.TimeStop("AddIncomming", "ODS");
                                }

                                ODSWin32Interface.SetEvent(hBufferReadyEvent);   // Signal that were ready to go again
                            } catch (Win32Exception) {
                                // Make sure that errors dont kill the loop - not sure if this is a good idea or not.
                                //Bilge.Dump(ex, "MEX::ODSGatherer::InterceptODS Error, ignoring error");
                                MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.ODSStatusMessage, UserMessageType.WarningMessage, "The ODS Listener experienced an error and has lost a message.");
                            }
                        } // While Thread isalive loop
                    } catch (Win32Exception) {
                        //Bilge.Dump(eex, "Mex::ODSGatherer - Unable to map the swap file, failed to initialise the mapping and aborting");
                        //Bilge.Log("Disabling ODS listener, no ODS messages will be captured");
                        MexCore.TheCore.ViewManager.AddUserNotificationMessageByIndex(UserMessages.ODSStatusMessage, UserMessageType.ErrorMessage, "The ODS listener was unable to initialise the access to the swap file.  Possibly 64Bit machine?");
                    } finally {
                        // Clean Up resources Section
                        if (hBufferReadyEvent != 0) {
                            ODSWin32Interface.SetEvent(hBufferReadyEvent);
                            ODSWin32Interface.CloseHandle(hBufferReadyEvent);
                        }

                        if (viewOfSwapFile != null) { ODSWin32Interface.UnmapViewOfFile(viewOfSwapFile); }
                        if (hDataReadyEvent != 0) { ODSWin32Interface.CloseHandle(hDataReadyEvent); }
                        if (hSwapFileBuffer != 0) { ODSWin32Interface.CloseHandle(hSwapFileBuffer); }
                        // dont need to free memory just let the unfix hit
                    } // End finally section
                }
            }

            //Bilge.Log("Mex::ODSDataGatherer::ThreadWhileLoop -> Loop terminated");
        }
    }
}