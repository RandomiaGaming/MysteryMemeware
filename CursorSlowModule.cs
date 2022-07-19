namespace MysteryMemeware
{
    public static class CursorSlowModule
    {
        private static int OriginalCursorSpeed = 10;
        public static unsafe void SlowCursor()
        {
            int currentSpeed;
            SystemParametersInfo(SPI_GETMOUSESPEED, 0, new System.IntPtr(&currentSpeed), 0);
            OriginalCursorSpeed = currentSpeed;
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, new System.IntPtr(1), 0);
        }
        public static void UnslowCursor()
        {
            SystemParametersInfo(SPI_SETMOUSESPEED, 0, new System.IntPtr(OriginalCursorSpeed), 0);
        }
        private const uint SPI_GETMOUSESPEED = 0x0070;
        private const uint SPI_SETMOUSESPEED = 0x0071;
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, System.IntPtr pvParam, uint fWinIni);
    }
}
