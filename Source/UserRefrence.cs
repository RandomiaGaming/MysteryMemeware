//#approve 08/05/2022 12:51pm
using System;
using MysteryMemeware.Helpers;
namespace MysteryMemeware
{
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