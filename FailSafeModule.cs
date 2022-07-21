//Approved 07/13/2022
namespace MysteryMemeware
{
    public static class FailSafeModule
    {
        private static long pressStartTime = 0;
        private static System.Diagnostics.Stopwatch stopwatch = null;
        public static void RunKillLoop()
        {
            System.Threading.Thread childThread = new System.Threading.Thread(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.AutoReset = true;
                timer.Interval = 100;
                timer.Elapsed += OnTimerTick;
                timer.Start();

                stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
            });
            childThread.Start();
        }
        private static void OnTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool escapeDown = GetKeyState(27) < -64;

            if (escapeDown && pressStartTime == 0)
            {
                pressStartTime = stopwatch.ElapsedTicks;
            }
            else if (!escapeDown)
            {
                pressStartTime = 0;
            }

            if (stopwatch.ElapsedTicks - pressStartTime >= 10000000 * 3 && pressStartTime != 0)
            {
                Abort();
            }
        }
        private static void Abort()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        [System.Runtime.InteropServices.DllImport("USER32.dll")]
        private static extern short GetKeyState(int virtualKeyID);
    }
}
