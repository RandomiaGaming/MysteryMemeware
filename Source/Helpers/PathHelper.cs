using System;
using System.IO;
namespace MysteryMemeware
{
    public static class PathHelper
    {
        #region Public Constants
        public static readonly string CurrentExePath = typeof(Program).Assembly.Location;
        public static readonly string CurrentExeDirectory = Path.GetDirectoryName(CurrentExePath);
        public static readonly string System32Folder = StringHelper.ReplaceCaseless(Environment.GetFolderPath(Environment.SpecialFolder.System), "System32", "Sysnative");
        public static readonly string System64Folder = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        public static readonly string SystemFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        #endregion
    }
}