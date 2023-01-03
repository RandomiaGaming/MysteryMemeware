using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
namespace MysteryMemeware
{
    public static class Win32HandleHelper
    {
        public static IntPtr GetChildWindowByTitle(IntPtr windowHandle, string targetTitle)
        {
            IntPtr[] handles = GetChildWindowHandles(windowHandle);
            foreach (IntPtr handle in handles)
            {
                if(GetWindowTitle(handle) == targetTitle)
                {
                    return handle;
                }
            }
            throw new Exception("Child window with specified title does not exist.");
        }
        public static IntPtr GetChildWindowByClass(IntPtr windowHandle, string targetClass)
        {
            IntPtr[] handles = GetChildWindowHandles(windowHandle);
            foreach (IntPtr handle in handles)
            {
                if (GetWindowClass(handle) == targetClass)
                {
                    return handle;
                }
            }
            throw new Exception("Child window with specified class does not exist.");
        }

        public static string GetWindowTitle(IntPtr windowHandle)
        {
            var outputLength = GetWindowTextLength(windowHandle) + 1;
            var outputStringBuilder = new StringBuilder(outputLength);
            GetWindowText(windowHandle, outputStringBuilder, outputLength);
            return outputStringBuilder.ToString();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetWindowClass(IntPtr windowHandle)
        {
            var outputLength = 256;
            var outputStringBuilder = new StringBuilder(outputLength);
            GetClassName(windowHandle, outputStringBuilder, outputLength);
            return outputStringBuilder.ToString();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);
        public static IntPtr[] GetChildWindowHandles(IntPtr windowHandle)
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(windowHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles.ToArray();
        }
        private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }
    }
}
