//#Approve File 2022/08/04/PM/3/49
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Management;
namespace MysteryMemeware
{
    public static class UserHelper
    {
        public const char SAMSeporatorChar = '\\';
        public static readonly string SAMSeporatorString = SAMSeporatorChar.ToString();
        public const char UPNSeporatorChar = '@';
        public static readonly string UPNSeporatorString = UPNSeporatorChar.ToString();
        public static UserRefrence GetCurrentUser()
        {
            try
            {
                return new UserRefrence(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            }
            catch
            {

            }
            try
            {
                return new UserRefrence(Environment.UserName, Environment.UserDomainName);
            }
            catch
            {

            }
            throw new Exception("Could not get current username.");
        }
        public static UserRefrence[] GetLocalUsers()
        {
            List<UserRefrence> output = new();
            List<string> usernames = new();
            string localDomain = GetCurrentUser().Domain;
            try
            {
                SelectQuery query = new("Win32_UserAccount", $"domain=\"{localDomain}\"");
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
                                output.Add(new UserRefrence(username, localDomain));
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
                var computerEntry = new DirectoryEntry($"WinNT://{localDomain},computer");
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
                                    output.Add(new UserRefrence(username, localDomain));
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
            return output.ToArray();
        }
        public static void CreateUser(UserRefrence user)
        {
            if (user.OnDefaultDomain)
            {
                RunNetCommand($"user /add \"{user.Name}\"", true);
            }
            else
            {
                RunNetCommand($"user /add \"{user.Name}\" /domain \"{user.Domain}\"", true);
            }
        }
        public static void CreateUser(UserRefrence user, string password)
        {
            if (user.OnDefaultDomain)
            {
                RunNetCommand($"user /add \"{user.Name}\" \"{password}\"", true);
            }
            else
            {
                RunNetCommand($"user /add \"{user.Name}\" \"{password}\" /domain \"{user.Domain}\"", true);
            }
        }
        public static void ChangeUserPassword(UserRefrence user, string password)
        {
            if (user.OnDefaultDomain)
            {
                RunNetCommand($"user \"{user.Name}\" \"{password}\"", true);
            }
            else
            {
                RunNetCommand($"user \"{user.Name}\" \"{password}\" /domain \"{user.Domain}\"", true);
            }
        }
        public static void SetAdminStatus(UserRefrence user, bool isAdmin)
        {
            if (isAdmin)
            {
                if (user.OnDefaultDomain)
                {
                    RunNetCommand($"localgroup Administrators /add \"{user.Name}\"", false);
                }
                else
                {
                    RunNetCommand($"localgroup Administrators /add \"{user.Name}\" /domain \"{user.Domain}\"", false);
                }
            }
            else
            {
                if (user.OnDefaultDomain)
                {
                    RunNetCommand($"localgroup Administrators /remove \"{user.Name}\"", false);
                }
                else
                {
                    RunNetCommand($"localgroup Administrators /remove \"{user.Name}\" /domain \"{user.Domain}\"", false);
                }
            }
        }
        public static void SetUserActiveState(UserRefrence user, bool activeState)
        {
            if (activeState)
            {
                if (user.OnDefaultDomain)
                {
                    RunNetCommand($"user \"{user.Name}\" /active:yes", true);
                }
                else
                {
                    RunNetCommand($"user \"{user.Name}\" /active:yes /domain \"{user.Domain}\"", true);
                }
            }
            else
            {
                if (user.OnDefaultDomain)
                {
                    RunNetCommand($"user \"{user.Name}\" /active:no", true);
                }
                else
                {
                    RunNetCommand($"user \"{user.Name}\" /active:no /domain \"{user.Domain}\"", true);
                }
            }
        }
        public static bool UserExists(UserRefrence user)
        {
            if (user.OnDefaultDomain)
            {
               return RunNetCommand($"user \"{user.Name}\"", false);
            }
            else
            {
               return RunNetCommand($"user \"{user.Name}\" /domain \"{user.Domain}\"", false);
            }
        }
        public static bool RunNetCommand(string arguments, bool throwOnNonZeroExitCode = true)
        {
            if (ProcessHelper.IsAdmin() is false)
            {
                throw new Exception("Net commands require administrator.");
            }
            string system32 = PathHelper.GetSystem32Path();
            return ProcessHelper.AwaitSuccess(ProcessHelper.Start(new TerminalCommand($"{system32}\\net.exe", arguments), WindowMode.Hidden, true, system32), 300000000, TimeoutAction.KillAndThrow, throwOnNonZeroExitCode);
        }
    }
    public sealed class UserRefrence
    {
        public readonly string Name = "";
        public readonly string Domain = null;
        public readonly bool OnDefaultDomain = true;
        public readonly string UPN = "";
        public readonly string SAM = "";
        public UserRefrence(string accountPath)
        {
            if (accountPath is null)
            {
                Name = "";
                Domain = null;
                OnDefaultDomain = true;
                UPN = "";
                SAM = "";
                return;
            }
            bool upn = accountPath.Contains(UserHelper.UPNSeporatorString);
            bool sam = accountPath.Contains(UserHelper.SAMSeporatorString);
            if (upn && sam)
            {
                throw new Exception("Invalid username");
            }
            else if (upn && !sam)
            {
                string[] split = accountPath.Split(UserHelper.UPNSeporatorChar);
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
                string[] split = accountPath.Split(UserHelper.SAMSeporatorChar);
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
                Name = accountPath;
                Domain = null;
                OnDefaultDomain = true;
                UPN = accountPath;
                SAM = accountPath;
                return;
            }
        }
        public UserRefrence(string name, string domain)
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