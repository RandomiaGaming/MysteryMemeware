namespace MysteryHelper
{
    public struct Point
    {
        public int X;
        public int Y;
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public static class Win32InputHelper
    {
        #region Keyboard Constants
        #endregion
        #region Mouse Constants
        //[in] dwFlags DWORD
        //The dx and dy parameters contain normalized absolute coordinates. If not set, those parameters contain relative data: the change in position since the last reported position. This flag can be set, or not set, regardless of what kind of mouse or mouse-like device, if any, is connected to the system. For further information about relative mouse motion, see the following Remarks section.
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        //The left button is down.
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //The left button is up.
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        //The middle button is down.
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //The middle button is up.
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //Movement occurred.
        public const int MOUSEEVENTF_MOVE = 0x0001;
        //The right button is down.
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //The right button is up.
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //The wheel has been moved, if the mouse has a wheel. The amount of movement is specified in dwData.
        public const int MOUSEEVENTF_WHEEL = 0x0800;
        //An X button was pressed.
        public const int MOUSEEVENTF_XDOWN = 0x0080;
        //An X button was released.
        public const int MOUSEEVENTF_XUP = 0x0100;
        //The wheel button is tilted.
        public const int MOUSEEVENTF_HWHEEL = 0x01000;
        //[in] dwData DWORD
        //Set if the first X button was pressed or released.
        public const int XBUTTON1 = 0x0001;
        //Set if the second X button was pressed or released.
        public const int XBUTTON2 = 0x0002;
        #endregion
        public static void PressKey(int virtualKeyCode)
        {
        }
        public static void KeyDown(int virtualKeyCode)
        {
        }
        public static void KeyUp(int virtualKeyCode)
        {
        }
        public static void SetMousePosition(int mousePositionX, int mousePositionY)
        {
            SetCursorPos(mousePositionX, mousePositionY);
        }
        public static Point GetMousePosition()
        {
            GetCursorPos(out Point mousePosition);
            return mousePosition;
        }
        public static void Click()
        {
            GetCursorPos(out Point mousePosition);
            mouse_event(MOUSEEVENTF_LEFTDOWN, mousePosition.X, mousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, mousePosition.X, mousePosition.Y, 0, 0);
        }
        public static void ClickDown()
        {
            GetCursorPos(out Point mousePosition);
            mouse_event(MOUSEEVENTF_LEFTDOWN, mousePosition.X, mousePosition.Y, 0, 0);
        }
        public static void ClickUp()
        {
            GetCursorPos(out Point mousePosition);
            mouse_event(MOUSEEVENTF_LEFTUP, mousePosition.X, mousePosition.Y, 0, 0);
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Point point);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
    }
}
