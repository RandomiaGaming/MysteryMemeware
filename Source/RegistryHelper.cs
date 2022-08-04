//#Approve File 08/03/2022 11:35am.
using System;
using Microsoft.Win32;
namespace MysteryMemeware
{
    public static class RegistryHelper
    {
        public const char PathSeparatorChar = '\\';
        public static readonly string PathSeparatorString = new string(new char[1] { PathSeparatorChar });
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
        public static object GetRegistryValue(string registryPath)
        {
            return GetRegistryValue(ParseRegistryValuePath(registryPath));
        }
        public static object GetRegistryValue(RegistryValuePath registryPath)
        {
            object output = null;
            RegistryKey baseKey = OpenBaseKey(registryPath.BaseKeyName);
            try
            {
                RegistryKey subKey = baseKey.OpenSubKey(registryPath.SubKeyPath, false);
                if (subKey is null)
                {
                    throw new Exception($"Subkey with name \"{registryPath.KeyPath}\" could not be found or is inaccessable.");
                }
                try
                {
                    output = subKey.GetValue(registryPath.ValueName);
                }
                finally
                {
                    try
                    {
                        subKey.Dispose();
                    }
                    catch
                    {

                    }
                }
            }
            finally
            {
                try
                {
                    baseKey.Dispose();
                }
                catch
                {

                }
            }
            return output;
        }
        public static void SetRegistryValue(string registryPath, object value)
        {
            SetRegistryValue(ParseRegistryValuePath(registryPath), value);
        }
        public static void SetRegistryValue(string registryPath, object value, RegistryValueKind registryValueKind)
        {
            SetRegistryValue(ParseRegistryValuePath(registryPath), value, registryValueKind);
        }
        public static void SetRegistryValue(RegistryValuePath registryPath, object value)
        {
            SetRegistryValue(registryPath, value, GetRegistryValueKind(value));
        }
        public static void SetRegistryValue(RegistryValuePath registryPath, object value, RegistryValueKind registryValueKind)
        {
            RegistryKey baseKey = OpenBaseKey(registryPath.BaseKeyName);
            try
            {
                RegistryKey subKey = baseKey.CreateSubKey(registryPath.SubKeyPath, true);
                if (subKey is null)
                {
                    throw new Exception($"Subkey with name \"{registryPath.KeyPath}\" could not be created and is inaccessable.");
                }
                try
                {
                    subKey.SetValue(registryPath.ValueName, value, registryValueKind);
                }
                finally
                {
                    try
                    {
                        subKey.Dispose();
                    }
                    catch
                    {

                    }
                }
            }
            finally
            {
                try
                {
                    baseKey.Dispose();
                }
                catch
                {

                }
            }
        }
        public static RegistryValueKind GetRegistryValueKind(object data)
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
            return RegistryValueKind.Unknown;
        }
        public static RegistryKey OpenBaseKey(string baseKeyName)
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

            throw new Exception($"No base key exists with name {baseKeyName}.");
        }
        public static RegistryKeyPath ParseRegistryKeyPath(string registryPath)
        {
            registryPath = FormatRegistryPath(registryPath);
            string baseKeyName = StringHelper.SelectBeforeCaseless(registryPath, PathSeparatorString);
            if (registryPath is null || registryPath is "" || baseKeyName is null || baseKeyName is "")
            {
                throw new Exception("Provided registryPath was invalid.");
            }
            string subKeyPath = registryPath.Substring(baseKeyName.Length + 1, registryPath.Length - baseKeyName.Length - 1);
            if (subKeyPath is "")
            {
                throw new Exception("Provided registryPath was invalid.");
            }
            return new RegistryKeyPath(baseKeyName, subKeyPath);
        }
        public struct RegistryKeyPath
        {
            public readonly string BaseKeyName;
            public readonly string SubKeyPath;
            public RegistryKeyPath(string baseKeyName, string subKeyPath)
            {
                BaseKeyName = baseKeyName;
                SubKeyPath = subKeyPath;
            }
        }
        public static RegistryValuePath ParseRegistryValuePath(string registryPath)
        {
            registryPath = FormatRegistryPath(registryPath);
            string baseKeyName = StringHelper.SelectBeforeCaseless(registryPath, PathSeparatorString);
            string valueName = StringHelper.SelectAfterCaseless(registryPath, PathSeparatorString);
            if (registryPath is null || registryPath is "" || baseKeyName is null || baseKeyName is "" || valueName is null || valueName is "")
            {
                throw new Exception("Provided registryPath was invalid.");
            }
            string subKeyPath = registryPath.Substring(baseKeyName.Length + 1, registryPath.Length - baseKeyName.Length - valueName.Length - 2);
            if (subKeyPath is "")
            {
                throw new Exception("Provided registryPath was invalid.");
            }
            return new RegistryValuePath(baseKeyName, subKeyPath, valueName);
        }
        public struct RegistryValuePath
        {
            public readonly string BaseKeyName;
            public readonly string SubKeyPath;
            public readonly string ValueName;
            public string KeyPath => BaseKeyName + PathSeparatorString + SubKeyPath;
            public RegistryValuePath(string baseKeyName, string subKeyPath, string valueName)
            {
                BaseKeyName = baseKeyName;
                SubKeyPath = subKeyPath;
                ValueName = valueName;
            }
        }
        public static string FormatRegistryPath(string registryPath)
        {
            if (registryPath is not null && registryPath is not "")
            {
                if (!StringHelper.EndsWithCaseless(registryPath, PathSeparatorString + PathSeparatorString) && StringHelper.EndsWithCaseless(registryPath, PathSeparatorString))
                {
                    registryPath = registryPath.Substring(0, registryPath.Length - 1);
                }
                if (!StringHelper.StartsWithCaseless(registryPath, PathSeparatorString + PathSeparatorString) && StringHelper.StartsWithCaseless(registryPath, PathSeparatorString))
                {
                    registryPath = registryPath.Substring(1, registryPath.Length - 1);
                }
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