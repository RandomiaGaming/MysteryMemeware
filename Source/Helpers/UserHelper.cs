using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Management;
namespace MysteryMemeware
{
    public static class UserHelper
    {
        public static readonly string CurrentUsername = Environment.UserName;
        public static readonly string CurrentDomain = Environment.UserDomainName;
        private static readonly Random RNG = new Random((int)DateTime.Now.Ticks);
        public const string PasswordCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const char SAMSeporatorChar = '\\';
        public static readonly string SAMSeporatorString = "\\";
        public const char UPNSeporatorChar = '@';
        public static readonly string UPNSeporatorString = "@";
        public static string GeneratePassword(int length = 14)
        {
            if (length < 0)
            {
                throw new Exception("Password length must be greater than or equal to 0.");
            }
            char[] passwordChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                passwordChars[i] = PasswordCharset[RNG.Next(0, PasswordCharset.Length)];
            }
            return new string(passwordChars);
        }
        public static string[] GetLocalUsernames()
        {
            List<string> usernames = new List<string>();
            try
            {
                SelectQuery query = new SelectQuery("Win32_UserAccount", $"domain=\"{CurrentDomain}\"");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                try
                {
                    foreach (ManagementObject managementObject in searcher.Get())
                    {
                        try
                        {
                            string username = (string)(managementObject["Name"]);
                            if (!(username is null) && !(username is "") && !StringHelper.MatchesArrayCaseless(username, usernames.ToArray()))
                            {
                                usernames.Add(username);
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            managementObject.Dispose();
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
                try
                {
                    searcher.Dispose();
                }
                catch
                {

                }
            }
            catch
            {

            }
            try
            {
                var computerEntry = new DirectoryEntry($"WinNT://{CurrentDomain},computer");
                try
                {
                    foreach (DirectoryEntry childEntry in computerEntry.Children)
                    {
                        try
                        {
                            if (childEntry.SchemaClassName == "User")
                            {
                                string username = childEntry.Name;
                                if (!(username is null) && !(username is "") && !StringHelper.MatchesArrayCaseless(username, usernames.ToArray()))
                                {
                                    usernames.Add(username);
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            childEntry.Dispose();
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
                try
                {
                    computerEntry.Dispose();
                }
                catch
                {

                }
            }
            catch
            {

            }
            return usernames.ToArray();
        }
        public static void AddUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user /add \"{username}\"", true);
        }
        public static void AddUser(string username, string password)
        {
            if (password is null || password is "")
            {
                AddUser(username);
                return;
            }
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user /add \"{username}\" \"{password}\"", true);
        }
        public static void RemoveUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user /delete \"{username}\"", true);
        }
        public static void ChangeUserPassword(string username, string password)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            if (password is null || password is "")
            {
                password = "";
            }
            RunNetCommand($"user \"{username}\" \"{password}\"", true);
        }
        public static void AddAdmin(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"localgroup Administrators /add \"{username}\"", true);
        }
        public static void RemoveAdmin(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"localgroup Administrators /delete \"{username}\"", true);
        }
        public static void ActivateUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user \"{username}\" /active:yes", true);
        }
        public static void DeactivateUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user \"{username}\" /active:no", true);
        }
        public static void EnableUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user \"{username}\" /enable:yes", true);
        }
        public static bool UserExists(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            try
            {
                RunNetCommand($"user \"{username}\"", true);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void RunNetCommand(string command, bool throwOnNonZeroExitCode = true)
        {
            if (UACHelper.CurrentProcessIsAdmin is false)
            {
                throw new Exception("Net commands require administrator.");
            }
            ProcessHelper.AwaitSuccess(ProcessHelper.Start(new TerminalCommand(PathHelper.System32Folder + "\\net.exe", command), WindowMode.Hidden, true, PathHelper.System32Folder), new TimeSpan(300000000), TimeoutAction.KillAndThrow, throwOnNonZeroExitCode);
        }

    }
}
/*
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Management;
namespace MysteryMemeware.Helpers
{
    public static class UserHelper
    {
        public static readonly string CurrentUsername = Environment.UserName;
        public static readonly string CurrentDomain = Environment.UserDomainName;
        private static readonly Random RNG = new Random((int)DateTime.Now.Ticks);
        public const string PasswordCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const char SAMSeporatorChar = '\\';
        public static readonly string SAMSeporatorString = "\\";
        public const char UPNSeporatorChar = '@';
        public static readonly string UPNSeporatorString = "@";
        public static string GeneratePassword(int length = 14)
        {
            if (length < 0)
            {
                throw new Exception("Password length must be greater than or equal to 0.");
            }
            char[] passwordChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                passwordChars[i] = PasswordCharset[RNG.Next(0, PasswordCharset.Length)];
            }
            return new string(passwordChars);
        }
        public static string[] GetLocalUsernames()
        {
            List<string> usernames = new List<string>();
            try
            {
                SelectQuery query = new SelectQuery("Win32_UserAccount", $"domain=\"{CurrentDomain}\"");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                try
                {
                    foreach (ManagementObject managementObject in searcher.Get())
                    {
                        try
                        {
                            string username = (string)(managementObject["Name"]);
                            if (!(username is null) && !(username is "") && !StringHelper.MatchesArrayCaseless(username, usernames.ToArray()))
                            {
                                usernames.Add(username);
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            managementObject.Dispose();
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
                try
                {
                    searcher.Dispose();
                }
                catch
                {

                }
            }
            catch
            {

            }
            try
            {
                var computerEntry = new DirectoryEntry($"WinNT://{CurrentDomain},computer");
                try
                {
                    foreach (DirectoryEntry childEntry in computerEntry.Children)
                    {
                        try
                        {
                            if (childEntry.SchemaClassName == "User")
                            {
                                string username = childEntry.Name;
                                if (!(username is null) && !(username is "") && !StringHelper.MatchesArrayCaseless(username, usernames.ToArray()))
                                {
                                    usernames.Add(username);
                                }
                            }
                        }
                        catch
                        {

                        }
                        try
                        {
                            childEntry.Dispose();
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
                try
                {
                    computerEntry.Dispose();
                }
                catch
                {

                }
            }
            catch
            {

            }
            return usernames.ToArray();
        }
        public static void AddUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user /add \"{username}\"", true);
        }
        public static void AddUser(string username, string password)
        {
            if (password is null || password is "")
            {
                AddUser(username);
                return;
            }
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user /add \"{username}\" \"{password}\"", true);
        }
        public static void RemoveUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user /delete \"{username}\"", true);
        }
        public static void ChangeUserPassword(string username, string password)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            if (password is null || password is "")
            {
                password = "";
            }
            RunNetCommand($"user \"{username}\" \"{password}\"", true);
        }
        public static void AddAdmin(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"localgroup Administrators /add \"{username}\"", true);
        }
        public static void RemoveAdmin(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"localgroup Administrators /delete \"{username}\"", true);
        }
        public static void ActivateUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user \"{username}\" /active:yes", true);
        }
        public static void DeactivateUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user \"{username}\" /active:no", true);
        }
        public static void EnableUser(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            RunNetCommand($"user \"{username}\" /enable:yes", true);
        }
        public static bool UserExists(string username)
        {
            if (username is null || username is "")
            {
                throw new Exception("username cannot be null or empty.");
            }
            try
            {
                RunNetCommand($"user \"{username}\"", true);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void RunNetCommand(string command, bool throwOnNonZeroExitCode = true)
        {
            if (UACHelper.CurrentProcessIsAdmin is false)
            {
                throw new Exception("Net commands require administrator.");
            }
            ProcessHelper.AwaitSuccess(ProcessHelper.Start(new TerminalCommand(PathHelper.System32Folder + "\\net.exe", command), WindowMode.Hidden, true, PathHelper.System32Folder), new TimeSpan(300000000), TimeoutAction.KillAndThrow, throwOnNonZeroExitCode);
        }
    }
    public sealed class UsernameDomainPair
    {
        public readonly string Name = "";
        public readonly string Domain = null;
        public readonly bool OnDefaultDomain = true;
        public readonly string UPN = "";
        public readonly string SAM = "";
        public UsernameDomainPair(string userPath)
        {
            if (userPath is null)
            {
                Name = "";
                Domain = null;
                OnDefaultDomain = true;
                UPN = "";
                SAM = "";
                return;
            }
            bool upn = userPath.Contains(UserHelper.UPNSeporatorString);
            bool sam = userPath.Contains(UserHelper.SAMSeporatorString);
            if (upn && sam)
            {
                throw new Exception("Invalid username");
            }
            else if (upn && !sam)
            {
                string[] split = userPath.Split(UserHelper.UPNSeporatorChar);
                if (split.Length != 2)
                {
                    throw new Exception("Invalid username");
                }
                Name = split[0];
                Domain = split[1];
                OnDefaultDomain = false;
                UPN = $"{Name}{UserHelper.UPNSeporatorString}{Domain}";
                SAM = $"{Domain}{UserHelper.SAMSeporatorString}{Name}";
                return;
            }
            else if (sam && !upn)
            {
                string[] split = userPath.Split(UserHelper.SAMSeporatorChar);
                if (split.Length != 2)
                {
                    throw new Exception("Invalid username");
                }
                Name = split[1];
                Domain = split[0];
                OnDefaultDomain = false;
                UPN = $"{Name}{UserHelper.UPNSeporatorString}{Domain}";
                SAM = $"{Domain}{UserHelper.SAMSeporatorString}{Name}";
                return;
            }
            else
            {
                Name = userPath;
                Domain = null;
                OnDefaultDomain = true;
                UPN = userPath;
                SAM = userPath;
                return;
            }
        }
        public UsernameDomainPair(string name, string domain)
        {
            if (name is null)
            {
                Name = "";
            }
            else
            {
                Name = name;
            }
            if (domain is "")
            {
                Domain = null;
            }
            else
            {
                Domain = domain;
            }
            OnDefaultDomain = Domain is null;
            if (OnDefaultDomain)
            {
                UPN = Name;
                SAM = Name;
            }
            else
            {
                UPN = $"{Name}{UserHelper.UPNSeporatorString}{Domain}";
                SAM = $"{Domain}{UserHelper.SAMSeporatorString}{Name}";
            }
            return;
        }
    }
}
*/