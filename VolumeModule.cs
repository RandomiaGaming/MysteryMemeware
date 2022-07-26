//Approved 06/02/2022
namespace MysteryMemeware
{
    public static class VolumeModule
    {
        public static void SetVolume()
        {
            System.Threading.Thread childThread = new System.Threading.Thread(() =>
            {
                System.IntPtr processHandle = System.Diagnostics.Process.GetCurrentProcess().Handle;
                while (true)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        SendMessage(processHandle, 793, processHandle, 655360);
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            });
            childThread.Start();
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr SendMessage(System.IntPtr WMAppCommandProcessHandle, uint WMAppCommand, System.IntPtr AppCommandProcessHandle, uint appCommand);
    }
}
