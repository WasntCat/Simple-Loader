using Simple_Loader.Core.Protection.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Loader.Core.Protection
{
    public static class Intger
    {
        #region Imports
        [DllImport("Kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, [MarshalAs(UnmanagedType.Bool)] ref bool isDebuggerPresent);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("ntdll.dll", SetLastError = true)]
        static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, IntPtr processInformation, uint processInformationLength, IntPtr returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        public enum ProcessInfo : uint
        {
            ProcessBasicInformation = 0x00,
            ProcessDebugPort = 0x07,
            ProcessExceptionPort = 0x08,
            ProcessAccessToken = 0x09,
            ProcessWow64Information = 0x1A,
            ProcessImageFileName = 0x1B,
            ProcessDebugObjectHandle = 0x1E,
            ProcessDebugFlags = 0x1F,
            ProcessExecuteFlags = 0x22,
            ProcessInstrumentationCallback = 0x28,
            MaxProcessInfoClass = 0x64
        }
        #endregion
        #region Force Raise Bsod
        public enum Privilege : int
        {
            SeCreateTokenPrivilege = 1,
            SeAssignPrimaryTokenPrivilege = 2,
            SeLockMemoryPrivilege = 3,
            SeIncreaseQuotaPrivilege = 4,
            SeUnsolicitedInputPrivilege = 5,
            SeMachineAccountPrivilege = 6,
            SeTcbPrivilege = 7,
            SeSecurityPrivilege = 8,
            SeTakeOwnershipPrivilege = 9,
            SeLoadDriverPrivilege = 10,
            SeSystemProfilePrivilege = 11,
            SeSystemtimePrivilege = 12,
            SeProfileSingleProcessPrivilege = 13,
            SeIncreaseBasePriorityPrivilege = 14,
            SeCreatePagefilePrivilege = 15,
            SeCreatePermanentPrivilege = 16,
            SeBackupPrivilege = 17,
            SeRestorePrivilege = 18,
            SeShutdownPrivilege = 19,
            SeDebugPrivilege = 20,
            SeAuditPrivilege = 21,
            SeSystemEnvironmentPrivilege = 22,
            SeChangeNotifyPrivilege = 23,
            SeRemoteShutdownPrivilege = 24,
            SeUndockPrivilege = 25,
            SeSyncAgentPrivilege = 26,
            SeEnableDelegationPrivilege = 27,
            SeManageVolumePrivilege = 28,
            SeImpersonatePrivilege = 29,
            SeCreateGlobalPrivilege = 30,
            SeTrustedCredManAccessPrivilege = 31,
            SeRelabelPrivilege = 32,
            SeIncreaseWorkingSetPrivilege = 33,
            SeTimeZonePrivilege = 34,
            SeCreateSymbolicLinkPrivilege = 35
        }
        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr RtlAdjustPrivilege(Privilege privilege, bool bEnablePrivilege,
        bool IsThreadPrivilege, out bool PreviousValue);
        [DllImport("ntdll.dll")]
        public static extern uint NtRaiseHardError(
        NTStatus ErrorStatus,
        uint NumberOfParameters,
        uint UnicodeStringParameterMask,
        IntPtr Parameters,
        uint ValidResponseOption,
        out uint Response
        );
        public enum NTStatus : uint 
        {
            STATUS_SUCCESS = 0x00000000,
            STATUS_WAIT_0 = 0x00000000,
            STATUS_WAIT_1 = 0x00000001,
            STATUS_WAIT_2 = 0x00000002,
            STATUS_WAIT_3 = 0x00000003,
            STATUS_WAIT_63 = 0x0000003F,
            STATUS_ABANDONED = 0x00000080,
            STATUS_ABANDONED_WAIT_0 = 0x00000080,
            STATUS_ABANDONED_WAIT_63 = 0x000000BF,
            STATUS_USER_APC = 0x000000C0,
            STATUS_KERNEL_APC = 0x00000100,
            STATUS_ALERTED = 0x00000101,
            STATUS_TIMEOUT = 0x00000102,
            STATUS_PENDING = 0x00000103,
            STATUS_REPARSE = 0x00000104,
            STATUS_CRASH_DUMP = 0x00000116,
            DBG_EXCEPTION_HANDLED = 0x00010001,
            DBG_CONTINUE = 0x00010002,
            STATUS_PRIVILEGED_INSTRUCTION = 0xC0000096,
            STATUS_MEMORY_NOT_ALLOCATED = 0xC00000A0,
            STATUS_BIOS_FAILED_TO_CONNECT_INTERRUPT = 0xC000016E,
            STATUS_ASSERTION_FAILURE = 0xC0000420
        }
        #endregion

        // Add your own func if a bad procces is found on the users pc // Unless you want Them to bsod 
        public static void StartProtections()
        {
            if (PreventSandboxie())
            {
                RtlAdjustPrivilege(Privilege.SeShutdownPrivilege, true, false, out bool previousValue);
                NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
            }
            if (PreventDebugging())
            {
                RtlAdjustPrivilege(Privilege.SeShutdownPrivilege, true, false, out bool previousValue);
                NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
            }
            if (IntegCheck())
            {
                RtlAdjustPrivilege(Privilege.SeShutdownPrivilege, true, false, out bool previousValue);
                NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
            }
            if (BlacklistModule.ProccesBlacklist())
            {
                RtlAdjustPrivilege(Privilege.SeShutdownPrivilege, true, false, out bool previousValue);
                NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
            }
            if (VMModule.PreventVM())
            {
                RtlAdjustPrivilege(Privilege.SeShutdownPrivilege, true, false, out bool previousValue);
                NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
            }
            try { DumpModule.PreventDumping(); } catch { } // Just adding a try catch because this may break the program because im stupid
        }

        #region Extra protections
        private static bool IntegCheck()
        {
            byte[] byteRead = new byte[1];
            byte[] mov = new byte[1] { 0x8B };
            bool bIntegrityCompromised = false;
            IntPtr CheckRemoteDebuggerPresentAddr = GetProcAddress(GetModuleHandle("Kernel32.dll"), "CheckRemoteDebuggerPresent");
            Helper.ReadProcessMemory(Process.GetCurrentProcess().Handle, CheckRemoteDebuggerPresentAddr, byteRead, 2, out _);
            if (!ByteArrayCompare(byteRead, mov))
            {
                bIntegrityCompromised = true;
            }
            return bIntegrityCompromised;
        }
        private static bool PreventDebugging()
        {
            bool DebuggerPresent = false;
            CheckRemoteDebuggerPresent(OpenProcess(ProcessAccessFlags.All, false, Process.GetCurrentProcess().Id), ref DebuggerPresent);
            if (DebuggerPresent == false)
            {
                IntPtr hProc = OpenProcess(ProcessAccessFlags.All, false, Process.GetCurrentProcess().Id);
                IntPtr dwReturnLength = Marshal.AllocHGlobal(sizeof(long));
                IntPtr dwDebugPort = IntPtr.Zero;

                if (NtQueryInformationProcess(hProc, (int)ProcessInfo.ProcessDebugPort, dwReturnLength, (uint)Marshal.SizeOf(dwDebugPort), dwReturnLength) >= 0)
                {
                    CloseHandle(hProc);
                    if (dwDebugPort == (IntPtr)(-1))
                    {
                        Marshal.FreeHGlobal(dwReturnLength);
                        DebuggerPresent = true;
                    }
                }
                if (!Process.GetCurrentProcess().Parent().ProcessName.Contains("explorer"))
                    DebuggerPresent = true;
            }
            return DebuggerPresent;
        }
        private static bool PreventSandboxie()
        {
            if (GetModuleHandle("SbieDll.dll").ToInt32() != 0)
                return true;
            else
                return false;
        }
        public static IntPtr OpenProcess(ProcessAccessFlags flags1, Process proc, ProcessAccessFlags flags)
        {
            return OpenProcess(flags, false, proc.Id);
        }
        #endregion
    }
}
