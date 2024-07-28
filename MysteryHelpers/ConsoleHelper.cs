namespace MysteryHelper
{
    public static class ConsoleHelper
    {
        public static void HelloWorld()
        {
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.WriteLine("Hello World!");
        }
        public static void WriteLine()
        {
            System.Console.WriteLine();
        }
        public static void PressAnyKeyToExit()
        {
            System.Console.ForegroundColor = System.ConsoleColor.White;
            System.Console.WriteLine("Press any key to exit...");
            System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
            while (true)
            {
                System.Console.ReadKey(true);
                if (stopwatch.ElapsedTicks > 10000000)
                {
                    break;
                }
            }
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
