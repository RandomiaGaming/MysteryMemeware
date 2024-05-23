namespace MysteryHelper
{
    public static class PathHelper
    {
        public static readonly string CurrentExePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        public static readonly string CurrentExeDirectory = System.IO.Path.GetDirectoryName(CurrentExePath);
        public static readonly string System32Folder = StringHelper.ReplaceCaseless(System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "System32", "Sysnative");
        public static readonly string System64Folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.SystemX86);
        public static readonly string SystemFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Windows);
        public static readonly string RootDrive = new System.IO.DirectoryInfo(System32Folder).Root.FullName;
    }
}