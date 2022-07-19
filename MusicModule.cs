//Approved 07/13/2022
namespace MysteryMemeware
{
    public static class MusicModule
    {
        public static void PlayMusic()
        {
            System.Threading.Thread childThread = new System.Threading.Thread(() =>
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(typeof(Program).Assembly.GetManifestResourceStream("MysteryMemeware.Music.wav"));
                while (true)
                {
                    player.PlaySync();
                }
            });
            childThread.Start();
        }
    }
}