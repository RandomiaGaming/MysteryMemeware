namespace MysteryUninstaller
{
    public static class Program
    {
        public static void Main()
        {
            System.Console.WriteLine("Mystery uninstaller is still under development and does not function at this time.");
            System.Console.WriteLine();
            System.Console.WriteLine("Process has exited.");
            System.Console.WriteLine("Press any key to close this window...");
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Restart();
            while (stopwatch.ElapsedTicks < 10000000)
            {
                System.Console.ReadKey(true);
            }
            System.Environment.Exit(0);
            System.Threading.Thread.Sleep(-1);
        }
    }
}