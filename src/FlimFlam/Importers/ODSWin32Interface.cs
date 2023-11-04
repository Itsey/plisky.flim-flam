using System;
using System.Runtime.InteropServices;

namespace Plisky.FlimFlam { 

    /// <summary>
    /// Win32 Imports section for API calls that are used by the intercept ODS routine.  These
    /// are around the use of memory mapped files and notification events, as well as closehandle.
    /// </summary>
    internal class ODSWin32Interface {

        #region w32intf Constants

        internal const int FILE_MAP_COPY = SECTION_QUERY;
        internal const int FILE_MAP_READ = SECTION_MAP_READ;
        internal const int FILE_MAP_WRITE = SECTION_MAP_WRITE;
        internal const int PAGE_READWRITE = 4;
        internal const int SECTION_EXTEND_SIZE = 0x10;
        internal const int SECTION_MAP_EXECUTE = 8;
        internal const int SECTION_MAP_READ = 4;
        internal const int SECTION_MAP_WRITE = 2;
        internal const int SECTION_QUERY = 1;
        internal const int SECURITY_DESCRIPTOR_MIN_LENGTH = 20;
        internal const int SECURITY_DESCRIPTOR_REVISION = 1;
        internal const int STATUS_ABANDONED_WAIT_0 = 0x00000080;
        internal const int STATUS_TIMEOUT = 0x00000102;
        internal const int STATUS_USER_APC = 0x000000C0;
        internal const int STATUS_WAIT_0 = 0x00000000;
        internal const int WAIT_ABANDONED = ((STATUS_ABANDONED_WAIT_0) + 0);
        internal const int WAIT_ABANDONED_0 = ((STATUS_ABANDONED_WAIT_0) + 0);
        internal const uint WAIT_FAILED = 0xFFFFFFFF;
        internal const int WAIT_OBJECT_0 = ((STATUS_WAIT_0) + 0);
        internal const int WAIT_TIMEOUT = STATUS_TIMEOUT;

        #endregion w32intf Constants

        internal ODSWin32Interface() {
        }

        [Flags]
        internal enum SECURITY_INFORMATION : uint {
            OWNER_SECURITY_INFORMATION = 0x00000001,
            GROUP_SECURITY_INFORMATION = 0x00000002,
            DACL_SECURITY_INFORMATION = 0x00000004,
            SACL_SECURITY_INFORMATION = 0x00000008,
            UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000,
            UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,
            PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,
            PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000
        }

        [Flags]
        internal enum SYNCHOBJECT_RIGHTS : uint {
            DELETE = 0x00010000, // Required to delete the object.
            READ_CONTROL = 0x00020000, // Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
            SYNCHRONIZE = 0x00100000, // The right to use the object for synchronization. This enables a thread to wait until the object is in the signaled state.
            WRITE_DAC = 0x00040000, // Required to modify the DACL in the security descriptor for the object.
            WRITE_OWNER = 0x00080000,
            EVENT_ALL_ACCESS = 0x1F0003, // All possible access rights for an event object. Use this right only if your application requires access beyond that granted by the standard access rights and EVENT_MODIFY_STATE. Using this access right increases the possibility that your application must be run by an Administrator.
            EVENT_MODIFY_STATE = 0x0002, // Modify state access, which is required for the SetEvent, ResetEvent and PulseEvent functions.
            MUTEX_ALL_ACCESS = 0x1F0001, // All possible access rights for a mutex object. Use this right only if your application requires access beyond that granted by the standard access rights and MUTEX_MODIFY_STATE. Using this access right increases the possibility that your application must be run by an Administrator.
            MUTEX_MODIFY_STATE = 0x0001, // Modify state access, which is required for the ReleaseMutex function.
            SEMAPHORE_ALL_ACCESS = 0x1F0003, // All possible access rights for a semaphore object. Use this right only if your application requires access beyond that granted by the standard access rights and SEMAPHORE_MODIFY_STATE. Using this access right increases the possibility that your application must be run by an Administrator.
            SEMAPHORE_MODIFY_STATE = 0x0002, // Modify state access, which is required for the ReleaseSemaphore function.
            TIMER_ALL_ACCESS = 0x1F0003, // All possible access rights for a waitable timer object. Use this right only if your application requires access beyond that granted by the standard access rights and TIMER_MODIFY_STATE. Using this access right increases the possibility that your application must be run by an Administrator.
            TIMER_MODIFY_STATE = 0x0002, // Modify state access, which is required for the SetWaitableTimer and CancelWaitableTimer functions.
            TIMER_QUERY_STATE = 0x0001
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SECURITY_ATTRIBUTES {
            [MarshalAs(UnmanagedType.U4)] internal uint nLength;
            [MarshalAs(UnmanagedType.LPStruct)] internal IntPtr lpSecurityDescriptor;
            [MarshalAs(UnmanagedType.Bool)] internal bool bIneritHandle;
        }

        #region original definitions

        /* Original Definitions:

           function InitializeSecurityDescriptor(pSecurityDescriptor: PSecurityDescriptor;
                      dwRevision: DWORD): BOOL; stdcall;

           function SetSecurityDescriptorDacl(pSecurityDescriptor: PSecurityDescriptor;
                      bDaclPresent: BOOL; pDacl: PACL; bDaclDefaulted: BOOL): BOOL; stdcall;

           function CreateFileMapping(hFile: THandle; lpFileMappingAttributes: PSecurityAttributes;
                      flProtect, dwMaximumSizeHigh, dwMaximumSizeLow: DWORD; lpName: PChar): THandle; stdcall;

           function CreateEvent(lpEventAttributes: PSecurityAttributes;
                      bManualReset, bInitialState: BOOL; lpName: PChar): THandle; stdcall;

           function MapViewOfFile(hFileMappingObject: THandle; dwDesiredAccess: DWORD;
                      dwFileOffsetHigh, dwFileOffsetLow, dwNumberOfBytesToMap: DWORD): Pointer; stdcall;

           function SetEvent(hEvent: THandle): BOOL; stdcall;

           function WaitForSingleObject(hHandle: THandle; dwMilliseconds: DWORD): DWORD; stdcall;

           function UnmapViewOfFile(lpBaseAddress: Pointer): BOOL; stdcall;

           function CloseHandle(hObject: THandle): BOOL; stdcall;
        */

        #endregion original definitions

        #region Win32 DLL Import Statements

        [DllImport("Kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("Kernel32.dll", EntryPoint = "CreateEvent", SetLastError = true)]
        internal static extern IntPtr CreateEvent(IntPtr lpEventAttributes,
            [MarshalAs(UnmanagedType.Bool)] bool bManualReset,
            [MarshalAs(UnmanagedType.Bool)] bool bInitialState, string lpName);

        [DllImport("Kernel32.dll", EntryPoint = "CreateFileMapping", SetLastError = true)]
        internal static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpFileMappingAttributes, uint flProtect,
          uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

        [DllImport("AdvApi32.dll", EntryPoint = "InitializeSecurityDescriptor", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool InitializeSecurityDescriptor(IntPtr pSecurityDescriptor, uint dwRevision);

        [DllImport("Kernel32.dll", EntryPoint = "MapViewOfFile", SetLastError = true)]
        internal static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh,
          uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", EntryPoint = "OpenMutex", SetLastError = true)]
        internal static extern IntPtr OpenMutex(SYNCHOBJECT_RIGHTS dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);

        [DllImport("Kernel32.dll", EntryPoint = "SetEvent", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetEvent(IntPtr hEvent);

        [DllImport("AdvApi32.dll", EntryPoint = "SetKernelObjectSecurity", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetKernelObjectSecurity(IntPtr Handle, SECURITY_INFORMATION SecurityInformation, IntPtr SecurityDescriptor);

        // PSECURITY_DESCRIPTOR last param
        [DllImport("AdvApi32.dll", EntryPoint = "SetSecurityDescriptorDacl", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetSecurityDescriptorDacl(IntPtr pSecurityDescriptor,
            [MarshalAs(UnmanagedType.Bool)] bool bDaclPresent,
            IntPtr pDacl,
            [MarshalAs(UnmanagedType.Bool)] bool bDaclDefaulted);

        [DllImport("Kernel32.dll", EntryPoint = "UnmapViewOfFile", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern unsafe bool UnmapViewOfFile(void* lpBaseAddress);

        [DllImport("Kernel32.dll", EntryPoint = "WaitForSingleObject", SetLastError = true)]
        internal static extern uint WaitForSingleObject(IntPtr hEvent, uint dwMilliseconds);

        #endregion Win32 DLL Import Statements
    }
}