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
            List<string> usernames = new();
            try
            {
                SelectQuery query = new("Win32_UserAccount", $"domain=\"{CurrentDomain}\"");
                ManagementObjectSearcher searcher = new(query);
                try
                {
                    foreach (ManagementObject managementObject in searcher.Get())
                    {
                        try
                        {
                            string username = (string)(managementObject["Name"]);
                            if (username is not null && username is not "" && !StringHelper.MatchesArrayCaseless(username, usernames.ToArray()))
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
                                if (username is not null && username is not "" && !StringHelper.MatchesArrayCaseless(username, usernames.ToArray()))
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
            ProcessHelper.AwaitSuccess(ProcessHelper.Start(new TerminalCommand(Program.System32Folder + "\\net.exe", command), WindowMode.Hidden, true, Program.System32Folder), new TimeSpan(300000000), TimeoutAction.KillAndThrow, throwOnNonZeroExitCode);
        }
    }
}