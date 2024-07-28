namespace MysteryExperience
{
    public static class MusicModule
    {
        public const string SongResourceName = "MysteryMemeware.MysteryAssets.Song.wav";
        public static void Run()
        {
            try
            {
                System.Reflection.Assembly assembly = typeof(Program).Assembly;
                System.IO.Stream songStream = assembly.GetManifestResourceStream(SongResourceName);
                bool initComplete = false;
                System.Threading.Thread soundPlayerThread = new System.Threading.Thread(() =>
                {
                    System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();
                    soundPlayer.Stream = songStream;
                    soundPlayer.LoadTimeout = int.MaxValue;
                    soundPlayer.Load();
                    initComplete = true;
                    while (true)
                    {
                        try
                        {
                            soundPlayer.PlaySync();
                        }
                        catch
                        {

                        }
                    }
                });
                soundPlayerThread.Start();
                while (!initComplete)
                {

                }
            }
            catch
            {

            }
        }
    }
}