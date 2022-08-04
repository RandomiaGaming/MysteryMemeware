//#Approve File 08/03/2022 11:35am.
using System;
using Microsoft.Win32;
namespace MysteryMemeware
{
    public static class Debug
    {
        public const string debugRegistryPath = "SOFTWARE\\MysteryDebugInfo";
        public static void LogException(Exception ex)
        {
            try
            {
                string exMessage = "Unknown exception thrown.";
                if (ex is not null)
                {
                    exMessage = $"{ex.GetType().FullName} thrown at {ex.TargetSite.Name} with message {ex.Message}. {ex.StackTrace}";
                }
                RegistryKey currentUser = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                try
                {
                    RegistryKey mysteryMemewareDebug = currentUser.CreateSubKey(debugRegistryPath, true);
                    try
                    {
                        int entryID = 0;
                        try
                        {
                            for (int possibleEntryID = 0; possibleEntryID < 10000; possibleEntryID++)
                            {
                                if (mysteryMemewareDebug.GetValue($"Entry {possibleEntryID}") is null)
                                {
                                    entryID = possibleEntryID;
                                    break;
                                }
                            }
                        }
                        catch
                        {

                        }
                        mysteryMemewareDebug.SetValue($"Entry {entryID}", exMessage, RegistryValueKind.String);
                    }
                    catch
                    {

                    }
                    mysteryMemewareDebug.Close();
                    mysteryMemewareDebug.Dispose();
                }
                catch
                {

                }
                currentUser.Close();
                currentUser.Dispose();
            }
            catch
            {

            }
        }
    }
}