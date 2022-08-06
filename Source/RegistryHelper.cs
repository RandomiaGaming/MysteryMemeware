//#approve 08/05/2022 12:38pm
using System;
using Microsoft.Win32;
namespace MysteryMemeware
{
    public static class RegistryHelper
    {
        public const char PathSeparatorChar = '\\';
        public static readonly string PathSeparatorString = PathSeparatorChar.ToString();
        public static readonly string[] RootKeyNames = new string[1] { "Computer" };
        public static readonly string[] ClassesRootKeyNames = new string[4] { "HKEY_CLASSES_ROOT", "CLASSES_ROOT", "HKEYCLASSESROOT", "CLASSESROOT" };
        public static readonly string[] CurrentUserKeyNames = new string[4] { "HKEY_CURRENT_USER", "CLASSES_ROOT", "HKEYCURRENTUSER", "CURRENTUSER" };
        public static readonly string[] LocalMachineKeyNames = new string[4] { "HKEY_LOCAL_MACHINE", "LOCAL_MACHINE", "HKEYLOCALMACHINE", "LOCALMACHINE" };
        public static readonly string[] UsersKeyNames = new string[3] { "HKEY_USERS", "USERS", "HKEYUSERS" };
        public static readonly string[] CurrentConfigKeyNames = new string[4] { "HKEY_CURRENT_CONFIG", "CURRENT_CONFIG", "HKEYCURRENTCONFIG", "CURRENTCONFIG" };
        public static readonly string[] Root32KeyNames = new string[2] { "Computer_32", "Computer32" };
        public static readonly string[] ClassesRoot32KeyNames = new string[8] { "HKEY_CLASSES_ROOT_32", "CLASSES_ROOT_32", "HKEYCLASSESROOT_32", "CLASSESROOT_32", "HKEY_CLASSES_ROOT32", "CLASSES_ROOT32", "HKEYCLASSESROOT32", "CLASSESROOT32" };
        public static readonly string[] CurrentUser32KeyNames = new string[8] { "HKEY_CURRENT_USER_32", "CLASSES_ROOT_32", "HKEYCURRENTUSER_32", "CURRENTUSER_32", "HKEY_CURRENT_USER32", "CLASSES_ROOT32", "HKEYCURRENTUSER32", "CURRENTUSER32" };
        public static readonly string[] LocalMachine32KeyNames = new string[8] { "HKEY_LOCAL_MACHINE_32", "LOCAL_MACHINE_32", "HKEYLOCALMACHINE_32", "LOCALMACHINE_32", "HKEY_LOCAL_MACHINE32", "LOCAL_MACHINE32", "HKEYLOCALMACHINE32", "LOCALMACHINE32" };
        public static readonly string[] Users32KeyNames = new string[6] { "HKEY_USERS_32", "USERS_32", "HKEYUSERS_32", "HKEY_USERS32", "USERS32", "HKEYUSERS32" };
        public static readonly string[] CurrentConfig32KeyNames = new string[8] { "HKEY_CURRENT_CONFIG_32", "CURRENT_CONFIG_32", "HKEYCURRENTCONFIG_32", "CURRENTCONFIG_32", "HKEY_CURRENT_CONFIG32", "CURRENT_CONFIG32", "HKEYCURRENTCONFIG32", "CURRENTCONFIG32" };
        public static bool RegistryValueExists(RegistryValueRefrence registryValue)
        {
            try
            {
                GetRegistryValue(registryValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static object GetRegistryValue(RegistryValueRefrence registryValue)
        {
            object output = null;
            RegistryKey registryKey = OpenRegistryKey(registryValue.ParentKeyRefrence, false);
            try
            {
                object value = registryKey.GetValue(registryValue.ValueName);
                if (value is null)
                {
                    throw new Exception("registryValue does not exist or could not be accessed.");
                }
                output = value;
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
            return output;
        }
        public static void SetRegistryValue(RegistryValueRefrence registryValue, object value, RegistryValueKind registryValueKind = RegistryValueKind.Unknown)
        {
            RegistryKey registryKey = OpenRegistryKey(registryValue.ParentKeyRefrence, true);
            try
            {
                if (registryValueKind is RegistryValueKind.Unknown)
                {
                    registryKey.SetValue(registryValue.ValueName, value, GetObjectRegistryKind(value));
                }
                else
                {
                    registryKey.SetValue(registryValue.ValueName, value, registryValueKind);
                }
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
        }
        public static void CreateRegistryValue(RegistryValueRefrence registryValue, object value, RegistryValueKind registryValueKind = RegistryValueKind.Unknown)
        {
            RegistryKey registryKey = CreateRegistryKey(registryValue.ParentKeyRefrence, true);
            try
            {
                if (registryValueKind is RegistryValueKind.Unknown)
                {
                    registryKey.SetValue(registryValue.ValueName, value, GetObjectRegistryKind(value));
                }
                else
                {
                    registryKey.SetValue(registryValue.ValueName, value, registryValueKind);
                }
            }
            finally
            {
                SafelyReleaseKey(registryKey);
            }
        }
        public static bool RegistryKeyExists(RegistryKeyRefrence registryKey)
        {
            try
            {
                SafelyReleaseKey(OpenRegistryKey(registryKey, false));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static RegistryKey OpenRegistryKey(RegistryKeyRefrence registryKey, bool writable = false)
        {
            if (registryKey is null)
            {
                throw new Exception("registryKey cannot be null.");
            }
            RegistryKey output = null;
            RegistryKey baseKey = OpenBaseKey(registryKey.BaseKeyName);
            try
            {
                RegistryKey subKey = baseKey.OpenSubKey(registryKey.SubKeyPath, writable);
                if (subKey is null)
                {
                    throw new Exception("subKey does not exist or could not be accessed.");
                }
                output = subKey;
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }
        public static RegistryKey CreateRegistryKey(RegistryKeyRefrence registryKey, bool writable = true)
        {
            if (registryKey is null)
            {
                throw new Exception("registryKey cannot be null.");
            }
            RegistryKey output = null;
            RegistryKey baseKey = OpenBaseKey(registryKey.BaseKeyName);
            try
            {
                RegistryKey subKey = baseKey.CreateSubKey(registryKey.SubKeyPath, writable);
                if (subKey is null)
                {
                    throw new Exception("subKey could not be created or could not be accessed.");
                }
                output = subKey;
            }
            finally
            {
                SafelyReleaseKey(baseKey);
            }
            return output;
        }
        public static bool BaseKeyExists(string baseKeyName)
        {
            try
            {
                SafelyReleaseKey(OpenBaseKey(baseKeyName));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static RegistryKey OpenBaseKey(string baseKeyName)
        {
            try
            {
                if (StringHelper.MatchesArrayCaseless(baseKeyName, ClassesRootKeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, CurrentUserKeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, LocalMachineKeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, UsersKeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Registry64);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, CurrentConfigKeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Registry64);
                }

                if (StringHelper.MatchesArrayCaseless(baseKeyName, ClassesRoot32KeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, CurrentUser32KeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, LocalMachine32KeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, Users32KeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.Users, RegistryView.Registry32);
                }
                if (StringHelper.MatchesArrayCaseless(baseKeyName, CurrentConfig32KeyNames))
                {
                    return RegistryKey.OpenBaseKey(RegistryHive.CurrentConfig, RegistryView.Registry32);
                }
            }
            catch
            {
                throw new Exception("Base key with given name could not be accessed.");
            }
            throw new Exception("Base key with given name does not exist.");
        }
        public static void SafelyReleaseKey(RegistryKey registryKey)
        {
            if (registryKey is null)
            {
                return;
            }
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
        public static RegistryValueKind GetObjectRegistryKind(object data)
        {
            if (data is null)
            {
                return RegistryValueKind.None;
            }
            Type dataType = data.GetType();
            if (dataType == typeof(string))
            {
                return RegistryValueKind.String;
            }
            if (dataType == typeof(long))
            {
                return RegistryValueKind.QWord;
            }
            if (dataType == typeof(int))
            {
                return RegistryValueKind.DWord;
            }
            if (dataType == typeof(byte[]))
            {
                return RegistryValueKind.Binary;
            }
            if (dataType == typeof(string[]))
            {
                return RegistryValueKind.MultiString;
            }
            return RegistryValueKind.None;
        }
        public static string FormatRegistryPath(string registryPath)
        {
            if (registryPath is null || registryPath is "")
            {
                return "";
            }
            if (!StringHelper.EndsWithCaseless(registryPath, PathSeparatorString + PathSeparatorString) && StringHelper.EndsWithCaseless(registryPath, PathSeparatorString))
            {
                registryPath = registryPath.Substring(0, registryPath.Length - 1);
            }
            if (!StringHelper.StartsWithCaseless(registryPath, PathSeparatorString + PathSeparatorString) && StringHelper.StartsWithCaseless(registryPath, PathSeparatorString))
            {
                registryPath = registryPath.Substring(1, registryPath.Length - 1);
            }
            string firstKeyName = StringHelper.SelectBeforeCaseless(registryPath, PathSeparatorString);
            if (StringHelper.MatchesArrayCaseless(firstKeyName, RootKeyNames))
            {
                registryPath = registryPath.Substring(firstKeyName.Length + 1, registryPath.Length - firstKeyName.Length - 1);
            }
            else if (StringHelper.MatchesArrayCaseless(firstKeyName, Root32KeyNames))
            {
                registryPath = registryPath.Substring(firstKeyName.Length + 1, registryPath.Length - firstKeyName.Length - 1);
                string secondKeyName = StringHelper.SelectBeforeCaseless(registryPath, PathSeparatorString);
                if (!StringHelper.EndsWithCaseless(secondKeyName, "32"))
                {
                    registryPath = secondKeyName + "_32" + registryPath.Substring(secondKeyName.Length, registryPath.Length - secondKeyName.Length);
                }
            }
            return registryPath;
        }
    }
}