using System;
using Microsoft.Win32;
namespace MysteryMemeware.Helpers
{
    public static class RegistryHelper
    {
        #region Public Static Variables
        public static bool AllowAltSeparator = true;
        #endregion
        #region Public Constants
        public const char SeparatorChar = '\\';
        public const string SeparatorString = "\\";

        public const char AltSeparatorChar = '/';
        public const string AltSeparatorString = "/";

        public const string ComputerName = "COMPUTER";
        public const string Computer32Name = "COMPUTER_32";
        public const string Computer64Name = "COMPUTER_64";

        public static readonly string[] ComputerNames = new string[] { "COMPUTER", "REGISTRY", "REG" };
        public static readonly string[] Computer32Names = new string[] { "COMPUTER_32", "REGISTRY_32", "REG_32", "COMPUTER32", "REGISTRY32", "REG32" };
        public static readonly string[] Computer64Names = new string[] { "COMPUTER_64", "REGISTRY_64", "REG_64", "COMPUTER64", "REGISTRY64", "REG64" };

        public const string HKEYClassesRootName = "HKEY_CLASSES_ROOT";
        public const string HKEYCurrentUserName = "HKEY_CURRENT_USER";
        public const string HKEYLocalMachineName = "HKEY_LOCAL_MACHINE";
        public const string HKEYUsersName = "HKEY_USERS";
        public const string HKEYCurrentConfigName = "HKEY_CURRENT_CONFIG";

        public static readonly string[] HKEYClassesRootNames = new string[] { "HKEY_CLASSES_ROOT", "CLASSES_ROOT", "HKEYCLASSESROOT", "CLASSESROOT", "HKCR" };
        public static readonly string[] HKEYCurrentUserNames = new string[] { "HKEY_CURRENT_USER", "CLASSES_ROOT", "HKEYCURRENTUSER", "CURRENTUSER", "HKCU" };
        public static readonly string[] HKEYLocalMachineNames = new string[] { "HKEY_LOCAL_MACHINE", "LOCAL_MACHINE", "HKEYLOCALMACHINE", "LOCALMACHINE", "HKLM" };
        public static readonly string[] HKEYUsersNames = new string[] { "HKEY_USERS", "USERS", "HKEYUSERS", "HKU" };
        public static readonly string[] HKEYCurrentConfigNames = new string[] { "HKEY_CURRENT_CONFIG", "CURRENT_CONFIG", "HKEYCURRENTCONFIG", "CURRENTCONFIG", "HKCC" };
        #endregion
        #region Registry Values
        public static bool ValueExists(string registryPath)
        {
            return ValueExists(new RegistryValuePath(registryPath));
        }
        public static bool ValueExists(RegistryKeyPath registryKeyPath, string valueName)
        {
            return ValueExists(new RegistryValuePath(registryKeyPath, valueName));
        }
        public static bool ValueExists(RegistryValuePath registryValuePath)
        {
            try
            {
                GetValue(registryValuePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteValue(string registryPath)
        {
            DeleteValue(new RegistryValuePath(registryPath));
        }
        public static void DeleteValue(RegistryKeyPath registryKeyPath, string valueName)
        {
            DeleteValue(new RegistryValuePath(registryKeyPath, valueName));
        }
        public static void DeleteValue(RegistryValuePath registryValuePath)
        {
            RegistryKey registryKey = OpenKeyWrite(registryValuePath);
            try
            {
                registryKey.DeleteValue(registryValuePath.ValueName);
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
        }

        public static object GetValue(string registryPath)
        {
            return GetValue(new RegistryValuePath(registryPath));
        }
        public static object GetValue(RegistryKeyPath registryKeyPath, string valueName)
        {
            return GetValue(new RegistryValuePath(registryKeyPath, valueName));
        }
        public static object GetValue(RegistryValuePath registryValuePath)
        {
            RegistryKey registryKey = OpenKeyRead(registryValuePath);
            object output;
            try
            {
                output = registryKey.GetValue(registryValuePath.ValueName);
                if (output is null)
                {
                    throw new Exception("Registry value does not exist.");
                }
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
            return output;
        }

        public static void CreateAndSetValue(string registryPath, object value)
        {
            CreateAndSetValue(registryPath, value, GetRegistryType(value));
        }
        public static void CreateAndSetValue(string registryPath, object value, RegistryType valueType)
        {
            CreateAndSetValue(new RegistryValuePath(registryPath), value, valueType);
        }
        public static void CreateAndSetValue(RegistryKeyPath registryKeyPath, string valueName, object value)
        {
            CreateAndSetValue(registryKeyPath, valueName, value, GetRegistryType(value));
        }
        public static void CreateAndSetValue(RegistryKeyPath registryKeyPath, string valueName, object value, RegistryType valueType)
        {
            CreateAndSetValue(new RegistryValuePath(registryKeyPath, valueName), value, valueType);
        }
        public static void CreateAndSetValue(RegistryValuePath registryValuePath, object value)
        {
            CreateAndSetValue(registryValuePath, value, GetRegistryType(value));
        }
        public static void CreateAndSetValue(RegistryValuePath registryValuePath, object value, RegistryType valueType)
        {
            if (value is null)
            {
                throw new Exception("value cannot be null.");
            }
            RegistryKey registryKey = OpenOrCreateKeyWrite(registryValuePath);
            try
            {
                RegistryValueKind registryValueKind;
                if (valueType is RegistryType.BINARY)
                {
                    registryValueKind = RegistryValueKind.Binary;
                }
                else if (valueType is RegistryType.DWORD)
                {
                    registryValueKind = RegistryValueKind.DWord;
                }
                else if (valueType is RegistryType.EXPAND_SZ)
                {
                    registryValueKind = RegistryValueKind.ExpandString;
                }
                else if (valueType is RegistryType.MULTI_SZ)
                {
                    registryValueKind = RegistryValueKind.MultiString;
                }
                else if (valueType is RegistryType.QWORD)
                {
                    registryValueKind = RegistryValueKind.QWord;
                }
                else if (valueType is RegistryType.SZ)
                {
                    registryValueKind = RegistryValueKind.String;
                }
                else if (valueType is RegistryType.NONE)
                {
                    registryValueKind = RegistryValueKind.None;
                }
                else
                {
                    registryValueKind = RegistryValueKind.Unknown;
                }
                registryKey.SetValue(registryValuePath.ValueName, value, registryValueKind);
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
        }

        public static void SetValue(string registryPath, object value)
        {
            SetValue(registryPath, value, GetRegistryType(value));
        }
        public static void SetValue(string registryPath, object value, RegistryType valueType)
        {
            SetValue(new RegistryValuePath(registryPath), value, valueType);
        }
        public static void SetValue(RegistryKeyPath registryKeyPath, string valueName, object value)
        {
            SetValue(registryKeyPath, valueName, value, GetRegistryType(value));
        }
        public static void SetValue(RegistryKeyPath registryKeyPath, string valueName, object value, RegistryType valueType)
        {
            SetValue(new RegistryValuePath(registryKeyPath, valueName), value, valueType);
        }
        public static void SetValue(RegistryValuePath registryValuePath, object value)
        {
            SetValue(registryValuePath, value, GetRegistryType(value));
        }
        public static void SetValue(RegistryValuePath registryValuePath, object value, RegistryType valueType)
        {
            if (value is null)
            {
                throw new Exception("value cannot be null.");
            }
            RegistryKey registryKey = OpenKeyWrite(registryValuePath);
            try
            {
                RegistryValueKind registryValueKind;
                if (valueType is RegistryType.BINARY)
                {
                    registryValueKind = RegistryValueKind.Binary;
                }
                else if (valueType is RegistryType.DWORD)
                {
                    registryValueKind = RegistryValueKind.DWord;
                }
                else if (valueType is RegistryType.EXPAND_SZ)
                {
                    registryValueKind = RegistryValueKind.ExpandString;
                }
                else if (valueType is RegistryType.MULTI_SZ)
                {
                    registryValueKind = RegistryValueKind.MultiString;
                }
                else if (valueType is RegistryType.QWORD)
                {
                    registryValueKind = RegistryValueKind.QWord;
                }
                else if (valueType is RegistryType.SZ)
                {
                    registryValueKind = RegistryValueKind.String;
                }
                else if (valueType is RegistryType.NONE)
                {
                    registryValueKind = RegistryValueKind.None;
                }
                else
                {
                    registryValueKind = RegistryValueKind.Unknown;
                }
                registryKey.SetValue(registryValuePath.ValueName, value, registryValueKind);
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
        }
        #endregion
        #region Registry Keys
        public static RegistryKey ReplaceKeyWrite(RegistryValuePath registryValuePath)
        {
            return ReplaceKeyWrite(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey ReplaceKeyWrite(string registryPath)
        {
            return ReplaceKeyWrite(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey ReplaceKeyWrite(RegistryKeyPath registryKeyPath)
        {
            return ReplaceKey(registryKeyPath, true);
        }

        public static RegistryKey ReplaceKeyRead(RegistryValuePath registryValuePath)
        {
            return ReplaceKeyRead(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey ReplaceKeyRead(string registryPath)
        {
            return ReplaceKeyRead(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey ReplaceKeyRead(RegistryKeyPath registryKeyPath)
        {
            return ReplaceKey(registryKeyPath, false);
        }

        public static RegistryKey ReplaceKey(RegistryValuePath registryValuePath, bool writeable)
        {
            return ReplaceKey(new RegistryKeyPath(registryValuePath), writeable);
        }
        public static RegistryKey ReplaceKey(string registryPath, bool writeable)
        {
            return ReplaceKey(new RegistryKeyPath(registryPath), writeable);
        }
        public static RegistryKey ReplaceKey(RegistryKeyPath registryKeyPath, bool writable)
        {
            DeleteKey(registryKeyPath);
            return CreateKey(registryKeyPath, writable);
        }

        public static void DeleteKey(RegistryValuePath registryValuePath)
        {
            DeleteKey(new RegistryKeyPath(registryValuePath));
        }
        public static void DeleteKey(string registryPath)
        {
            DeleteKey(new RegistryKeyPath(registryPath));
        }
        public static void DeleteKey(RegistryKeyPath registryKeyPath)
        {
            RegistryKey baseKey = OpenBase(registryKeyPath);
            try
            {
                baseKey.DeleteSubKeyTree(registryKeyPath.SubKeyPath, false);
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
        }

        public static RegistryKey OpenOrCreateKeyWrite(RegistryValuePath registryValuePath)
        {
            return OpenOrCreateKeyWrite(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey OpenOrCreateKeyWrite(string registryPath)
        {
            return OpenOrCreateKeyWrite(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey OpenOrCreateKeyWrite(RegistryKeyPath registryKeyPath)
        {
            return OpenOrCreateKey(registryKeyPath, true);
        }

        public static RegistryKey OpenOrCreateKeyRead(RegistryValuePath registryValuePath)
        {
            return OpenOrCreateKeyRead(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey OpenOrCreateKeyRead(string registryPath)
        {
            return OpenOrCreateKeyRead(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey OpenOrCreateKeyRead(RegistryKeyPath registryKeyPath)
        {
            return OpenOrCreateKey(registryKeyPath, false);
        }

        public static RegistryKey OpenOrCreateKey(RegistryValuePath registryValuePath, bool writeable)
        {
            return OpenOrCreateKey(new RegistryKeyPath(registryValuePath), writeable);
        }
        public static RegistryKey OpenOrCreateKey(string registryPath, bool writeable)
        {
            return OpenOrCreateKey(new RegistryKeyPath(registryPath), writeable);
        }
        public static RegistryKey OpenOrCreateKey(RegistryKeyPath registryKeyPath, bool writable)
        {
            if (registryKeyPath is null)
            {
                throw new Exception("registryKeyPath cannot be null.");
            }
            RegistryKey output;
            RegistryKey baseKey = OpenBase(registryKeyPath);
            try
            {
                output = baseKey.CreateSubKey(registryKeyPath.SubKeyPath, writable);
                if (output is null)
                {
                    throw new Exception("Registry key could not be opened or created.");
                }
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }

        public static RegistryKey CreateKeyWrite(RegistryValuePath registryValuePath)
        {
            return CreateKeyWrite(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey CreateKeyWrite(string registryPath)
        {
            return CreateKeyWrite(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey CreateKeyWrite(RegistryKeyPath registryKeyPath)
        {
            return CreateKey(registryKeyPath, true);
        }

        public static RegistryKey CreateKeyRead(RegistryValuePath registryValuePath)
        {
            return CreateKeyRead(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey CreateKeyRead(string registryPath)
        {
            return CreateKeyRead(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey CreateKeyRead(RegistryKeyPath registryKeyPath)
        {
            return CreateKey(registryKeyPath, false);
        }

        public static RegistryKey CreateKey(RegistryValuePath registryValuePath, bool writeable)
        {
            return CreateKey(new RegistryKeyPath(registryValuePath), writeable);
        }
        public static RegistryKey CreateKey(string registryPath, bool writeable)
        {
            return CreateKey(new RegistryKeyPath(registryPath), writeable);
        }
        public static RegistryKey CreateKey(RegistryKeyPath registryKeyPath, bool writable)
        {
            if (registryKeyPath is null)
            {
                throw new Exception("registryKeyPath cannot be null.");
            }
            if (KeyExists(registryKeyPath))
            {
                throw new Exception("Registry key already exists at registryKeyPath.");
            }
            RegistryKey output;
            RegistryKey baseKey = OpenBase(registryKeyPath);
            try
            {
                output = baseKey.CreateSubKey(registryKeyPath.SubKeyPath, writable);
                if (output is null)
                {
                    throw new Exception("Registry key could not be created.");
                }
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }

        public static bool KeyExists(RegistryValuePath registryValuePath)
        {
            return KeyExists(new RegistryKeyPath(registryValuePath));
        }
        public static bool KeyExists(string registryPath)
        {
            return KeyExists(new RegistryKeyPath(registryPath));
        }
        public static bool KeyExists(RegistryKeyPath registryKeyPath)
        {
            try
            {
                SafelyReleaseKey(OpenKey(registryKeyPath, false));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static RegistryKey OpenKeyWrite(RegistryValuePath registryValuePath)
        {
            return OpenKeyWrite(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey OpenKeyWrite(string registryPath)
        {
            return OpenKeyWrite(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey OpenKeyWrite(RegistryKeyPath registryKeyPath)
        {
            return OpenKey(registryKeyPath, true);
        }

        public static RegistryKey OpenKeyRead(RegistryValuePath registryValuePath)
        {
            return OpenKeyRead(new RegistryKeyPath(registryValuePath));
        }
        public static RegistryKey OpenKeyRead(string registryPath)
        {
            return OpenKeyRead(new RegistryKeyPath(registryPath));
        }
        public static RegistryKey OpenKeyRead(RegistryKeyPath registryKeyPath)
        {
            return OpenKey(registryKeyPath, false);
        }

        public static RegistryKey OpenKey(RegistryValuePath registryValuePath, bool writeable)
        {
            return OpenKey(new RegistryKeyPath(registryValuePath), writeable);
        }
        public static RegistryKey OpenKey(string registryPath, bool writeable)
        {
            return OpenKey(new RegistryKeyPath(registryPath), writeable);
        }
        public static RegistryKey OpenKey(RegistryKeyPath registryKeyPath, bool writable)
        {
            if (registryKeyPath is null)
            {
                throw new Exception("registryKeyPath cannot be null.");
            }
            RegistryKey baseKey = OpenBase(registryKeyPath);
            RegistryKey output;
            try
            {
                output = baseKey.OpenSubKey(registryKeyPath.SubKeyPath, writable);
                if (output is null)
                {
                    throw new Exception("Registry key does not exist.");
                }
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }
        #endregion
        #region Registry Bases
        public static RegistryKey OpenBase(RegistryValuePath registryValuePath)
        {
            if (registryValuePath is null)
            {
                throw new Exception("registryValuePath cannot be null.");
            }
            return OpenBase(registryValuePath.RegistryRoot, registryValuePath.RegistryBase);
        }
        public static RegistryKey OpenBase(RegistryKeyPath registryKeyPath)
        {
            if (registryKeyPath is null)
            {
                throw new Exception("registryKeyPath cannot be null.");
            }
            return OpenBase(registryKeyPath.RegistryRoot, registryKeyPath.RegistryBase);
        }
        public static RegistryKey OpenBase(RegistryBase registryBase)
        {
            return OpenBase(RegistryRoot.COMPUTER, registryBase);
        }
        public static RegistryKey OpenBase(RegistryRoot registryRoot, RegistryBase registryBase)
        {
            RegistryView registryView;

            if (registryRoot is RegistryRoot.COMPUTER_32)
            {
                registryView = RegistryView.Registry32;
            }
            else if (registryRoot is RegistryRoot.COMPUTER_64)
            {
                registryView = RegistryView.Registry64;
            }
            else
            {
                registryView = RegistryView.Default;
            }

            if (registryBase is RegistryBase.HKEY_CLASSES_ROOT)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, registryView);
            }
            else if (registryBase is RegistryBase.HKEY_CURRENT_CONFIG)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, registryView);
            }
            else if (registryBase is RegistryBase.HKEY_CURRENT_USER)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, registryView);
            }
            else if (registryBase is RegistryBase.HKEY_USERS)
            {
                return RegistryKey.OpenBaseKey(RegistryHive.Users, registryView);
            }
            else
            {
                return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            }
        }
        #endregion
        #region Registry Helpers
        public static void SafelyReleaseKey(RegistryKey registryKey)
        {
            try
            {
                registryKey.Flush();
            }
            catch
            {

            }
            try
            {
                registryKey.Close();
            }
            catch
            {

            }
            try
            {
                registryKey.Dispose();
            }
            catch
            {

            }
        }
        #endregion
        #region Registry Types
        public static bool IsRegistryType(object data, RegistryType expectedType)
        {
            try
            {
                return GetRegistryType(data) == expectedType;
            }
            catch
            {
                return false;
            }
        }

        public static RegistryType GetRegistryType(object value)
        {
            if (value is null)
            {
                throw new Exception("value cannot be null.");
            }

            Type dataType = value.GetType();

            if (dataType == typeof(byte[]))
            {
                return RegistryType.BINARY;
            }
            else if (dataType == typeof(int) || dataType == typeof(uint))
            {
                return RegistryType.DWORD;
            }
            else if (dataType == typeof(string))
            {
                string dataString = (string)value;
                if (dataString.Contains("%"))
                {
                    return RegistryType.EXPAND_SZ;
                }
                else
                {
                    return RegistryType.SZ;
                }
            }
            else if (dataType == typeof(string[]))
            {
                return RegistryType.MULTI_SZ;
            }
            else if (dataType == typeof(long) || dataType == typeof(ulong))
            {
                return RegistryType.QWORD;
            }
            else
            {
                return RegistryType.CHOOSE_AUTOMATICALLY;
            }
        }
        #endregion
    }
    public sealed class RegistryValuePath
    {
        #region Public Variables
        public readonly string RegistryPath = "Computer\\HKey_Local_Machine\\Software\\TestProgram\\InstallLocation";
        public readonly string[] Identifiers = new string[0];
        public readonly RegistryRoot RegistryRoot = RegistryRoot.COMPUTER;
        public readonly RegistryBase RegistryBase = RegistryBase.HKEY_LOCAL_MACHINE;
        public readonly string[] SubKeyNames = new string[2] { "Software", "TestProgram" };
        public readonly string SubKeyPath = "Software\\TestProgram";
        public readonly string ValueName = "InstallLocation";
        #endregion
        #region Public Constructors
        public RegistryValuePath(RegistryKeyPath registryKeyPath, string valueName)
        {
            if (registryKeyPath is null)
            {
                throw new Exception("registryKeyPath cannot be null.");
            }

            if (valueName is null)
            {
                throw new Exception("valueName cannot be null.");
            }

            if (valueName is "")
            {
                throw new Exception("valueName cannot be empty.");
            }

            if (RegistryHelper.AllowAltSeparator)
            {
                valueName = valueName.Replace(RegistryHelper.AltSeparatorChar, RegistryHelper.SeparatorChar);
            }

            if (StringHelper.StringContainsChar(valueName, RegistryHelper.SeparatorChar))
            {
                throw new Exception("valueName cannot contain seporator characters.");
            }

            RegistryPath = registryKeyPath.RegistryPath + RegistryHelper.SeparatorString + valueName;

            Identifiers = new string[registryKeyPath.Identifiers.Length + 1];
            Array.Copy(registryKeyPath.Identifiers, 0, Identifiers, 0, registryKeyPath.Identifiers.Length);
            Identifiers[Identifiers.Length - 1] = valueName;

            RegistryRoot = registryKeyPath.RegistryRoot;

            RegistryBase = registryKeyPath.RegistryBase;

            SubKeyNames = registryKeyPath.SubKeyNames;

            SubKeyPath = registryKeyPath.SubKeyPath;
        }
        public RegistryValuePath(string registryPath)
        {
            if (registryPath is null)
            {
                throw new Exception("registryPath cannot be null.");
            }
            if (RegistryHelper.AllowAltSeparator)
            {
                registryPath = registryPath.Replace(RegistryHelper.AltSeparatorString, RegistryHelper.SeparatorString);
            }
            if (!(registryPath.Length is 0) && registryPath[0] is RegistryHelper.SeparatorChar)
            {
                registryPath = registryPath.Substring(1, registryPath.Length - 1);
            }
            if (!(registryPath.Length is 0) && registryPath[registryPath.Length - 1] is RegistryHelper.SeparatorChar)
            {
                registryPath = registryPath.Substring(0, registryPath.Length - 1);
            }
            RegistryPath = registryPath;



            Identifiers = registryPath.Split(RegistryHelper.SeparatorChar);
            foreach (string identifier in Identifiers)
            {
                if (identifier is "")
                {
                    throw new Exception("registryPath cannot contain empty identifiers.");
                }
            }



            if (Identifiers.Length is 0)
            {
                RegistryRoot = RegistryRoot.COMPUTER;
                Identifiers = new string[1] { RegistryHelper.ComputerName };
            }
            else
            {
                string registryRootToUpper = Identifiers[0].ToUpper();
                if (StringHelper.MatchesArray(registryRootToUpper, RegistryHelper.ComputerNames))
                {
                    RegistryRoot = RegistryRoot.COMPUTER;
                }
                else if (StringHelper.MatchesArray(registryRootToUpper, RegistryHelper.Computer32Names))
                {
                    RegistryRoot = RegistryRoot.COMPUTER_32;
                }
                else if (StringHelper.MatchesArray(registryRootToUpper, RegistryHelper.Computer64Names))
                {
                    RegistryRoot = RegistryRoot.COMPUTER_64;
                }
                else
                {
                    RegistryRoot = RegistryRoot.COMPUTER;
                    string[] newIdentifiers = new string[Identifiers.Length + 1];
                    Array.Copy(Identifiers, 0, newIdentifiers, 1, Identifiers.Length);
                    newIdentifiers[0] = RegistryHelper.ComputerName;
                    Identifiers = newIdentifiers;
                }
            }



            if (Identifiers.Length < 2)
            {
                throw new Exception("registryPath must specify a base name.");
            }
            string registryBaseToUpper = Identifiers[1].ToUpper();
            if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYClassesRootNames))
            {
                RegistryBase = RegistryBase.HKEY_CLASSES_ROOT;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYCurrentUserNames))
            {
                RegistryBase = RegistryBase.HKEY_CURRENT_USER;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYLocalMachineNames))
            {
                RegistryBase = RegistryBase.HKEY_LOCAL_MACHINE;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYUsersNames))
            {
                RegistryBase = RegistryBase.HKEY_USERS;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYCurrentConfigNames))
            {
                RegistryBase = RegistryBase.HKEY_CURRENT_CONFIG;
            }
            else
            {
                throw new Exception("registryPath specifies a base key name which does not exist.");
            }



            if (Identifiers.Length < 3)
            {
                throw new Exception("registryPath must specify a value name.");
            }
            else if (Identifiers.Length is 3)
            {
                throw new Exception("registryPath must specify at least one sub key name.");
            }
            SubKeyNames = new string[Identifiers.Length - 3];
            Array.Copy(Identifiers, 2, SubKeyNames, 0, SubKeyNames.Length);



            SubKeyPath = "";
            for (int i = 0; i < SubKeyNames.Length; i++)
            {
                if (i is 0)
                {
                    SubKeyPath += SubKeyNames[i];
                }
                else
                {
                    SubKeyPath += RegistryHelper.SeparatorString + SubKeyNames[i];
                }
            }



            ValueName = Identifiers[Identifiers.Length - 1];
        }
        #endregion
    }
    public sealed class RegistryKeyPath
    {
        #region Public Variables
        public readonly string RegistryPath = "Computer\\HKey_Local_Machine\\Software\\TestProgram";
        public readonly string[] Identifiers = new string[0];
        public readonly RegistryRoot RegistryRoot = RegistryRoot.COMPUTER;
        public readonly RegistryBase RegistryBase = RegistryBase.HKEY_LOCAL_MACHINE;
        public readonly string[] SubKeyNames = new string[2] { "Software", "TestProgram" };
        public readonly string SubKeyPath = "Software\\TestProgram";
        #endregion
        #region Public Constructors
        public RegistryKeyPath(RegistryValuePath registryValuePath)
        {
            if (registryValuePath is null)
            {
                throw new Exception("registryValuePath cannot be null.");
            }

            RegistryPath = registryValuePath.RegistryPath.Substring(0, registryValuePath.RegistryPath.Length - (registryValuePath.ValueName.Length + 1));

            Identifiers = new string[registryValuePath.Identifiers.Length - 1];
            Array.Copy(registryValuePath.Identifiers, 0, Identifiers, 0, Identifiers.Length);

            RegistryRoot = registryValuePath.RegistryRoot;

            RegistryBase = registryValuePath.RegistryBase;

            SubKeyNames = registryValuePath.SubKeyNames;

            SubKeyPath = registryValuePath.SubKeyPath;
        }
        public RegistryKeyPath(string registryPath)
        {
            if (registryPath is null)
            {
                throw new Exception("registryPath cannot be null.");
            }
            if (RegistryHelper.AllowAltSeparator)
            {
                registryPath = registryPath.Replace(RegistryHelper.AltSeparatorString, RegistryHelper.SeparatorString);
            }
            if (!(registryPath.Length is 0) && registryPath[0] is RegistryHelper.SeparatorChar)
            {
                registryPath = registryPath.Substring(1, registryPath.Length - 1);
            }
            if (!(registryPath.Length is 0) && registryPath[registryPath.Length - 1] is RegistryHelper.SeparatorChar)
            {
                registryPath = registryPath.Substring(0, registryPath.Length - 1);
            }
            RegistryPath = registryPath;



            Identifiers = registryPath.Split(RegistryHelper.SeparatorChar);
            foreach (string identifier in Identifiers)
            {
                if (identifier is "")
                {
                    throw new Exception("registryPath cannot contain empty identifiers.");
                }
            }



            if (Identifiers.Length is 0)
            {
                RegistryRoot = RegistryRoot.COMPUTER;
                Identifiers = new string[1] { RegistryHelper.ComputerName };
            }
            else
            {
                string registryRootToUpper = Identifiers[0].ToUpper();
                if (StringHelper.MatchesArray(registryRootToUpper, RegistryHelper.ComputerNames))
                {
                    RegistryRoot = RegistryRoot.COMPUTER;
                }
                else if (StringHelper.MatchesArray(registryRootToUpper, RegistryHelper.Computer32Names))
                {
                    RegistryRoot = RegistryRoot.COMPUTER_32;
                }
                else if (StringHelper.MatchesArray(registryRootToUpper, RegistryHelper.Computer64Names))
                {
                    RegistryRoot = RegistryRoot.COMPUTER_64;
                }
                else
                {
                    RegistryRoot = RegistryRoot.COMPUTER;
                    string[] newIdentifiers = new string[Identifiers.Length + 1];
                    Array.Copy(Identifiers, 0, newIdentifiers, 1, Identifiers.Length);
                    newIdentifiers[0] = RegistryHelper.ComputerName;
                    Identifiers = newIdentifiers;
                }
            }



            if (Identifiers.Length < 2)
            {
                throw new Exception("registryPath must specify a base name.");
            }
            string registryBaseToUpper = Identifiers[1].ToUpper();
            if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYClassesRootNames))
            {
                RegistryBase = RegistryBase.HKEY_CLASSES_ROOT;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYCurrentUserNames))
            {
                RegistryBase = RegistryBase.HKEY_CURRENT_USER;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYLocalMachineNames))
            {
                RegistryBase = RegistryBase.HKEY_LOCAL_MACHINE;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYUsersNames))
            {
                RegistryBase = RegistryBase.HKEY_USERS;
            }
            else if (StringHelper.MatchesArray(registryBaseToUpper, RegistryHelper.HKEYCurrentConfigNames))
            {
                RegistryBase = RegistryBase.HKEY_CURRENT_CONFIG;
            }
            else
            {
                throw new Exception("registryPath specifies a base key name which does not exist.");
            }



            if (Identifiers.Length < 3)
            {
                throw new Exception("registryPath must specify at least one sub key name.");
            }
            SubKeyNames = new string[Identifiers.Length - 2];
            Array.Copy(Identifiers, 2, SubKeyNames, 0, SubKeyNames.Length);



            SubKeyPath = "";
            for (int i = 0; i < SubKeyNames.Length; i++)
            {
                if (i is 0)
                {
                    SubKeyPath += SubKeyNames[i];
                }
                else
                {
                    SubKeyPath += RegistryHelper.SeparatorString + SubKeyNames[i];
                }
            }
        }
        #endregion
    }
    public enum RegistryType : byte
    {
        BINARY = 0,
        DWORD = 1,
        QWORD = 2,
        SZ = 3,
        MULTI_SZ = 4,
        EXPAND_SZ = 5,
        CHOOSE_AUTOMATICALLY = 6,
        NONE = 7
    }
    public enum RegistryBase : byte
    {
        HKEY_CLASSES_ROOT = 0,
        HKEY_CURRENT_USER = 1,
        HKEY_LOCAL_MACHINE = 2,
        HKEY_USERS = 3,
        HKEY_CURRENT_CONFIG = 4,
    }
    public enum RegistryRoot : byte
    {
        COMPUTER = 0,
        COMPUTER_32 = 1,
        COMPUTER_64 = 2
    }
}