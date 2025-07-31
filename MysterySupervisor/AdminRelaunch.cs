using MysteryHelper;
namespace MysteryInstaller
{
    public static class AdminRelaunchModule
    {
        public static void Run()
        {
            try
            {
                if (!UACHelper.CurrentProcessIsAdmin)
                {
                    ProcessHelper.Start(typeof(AdminRelaunchModule).Assembly.Location, WindowMode.Default, true);
                    ProcessHelper.CurrentProcess.Kill();
                }
            }
            catch
            {

            }
        }
    }
}