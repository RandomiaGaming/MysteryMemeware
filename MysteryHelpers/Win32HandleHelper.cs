namespace MysteryHelper
{
    public static class Win32HandleHelper
    {
        public static System.IntPtr GetChildWindowByTitle(System.IntPtr windowHandle, string targetTitle)
        {
            System.IntPtr[] handles = GetChildWindowHandles(windowHandle);
            foreach (System.IntPtr handle in handles)
            {
                if (GetWindowTitle(handle) == targetTitle)
                {
                    return handle;
                }
            }
            throw new System.Exception("Child window with specified title does not exist.");
        }
        public static System.IntPtr GetChildWindowByClass(System.IntPtr windowHandle, string targetClass)
        {
            System.IntPtr[] handles = GetChildWindowHandles(windowHandle);
            foreach (System.IntPtr handle in handles)
            {
                if (GetWindowClass(handle) == targetClass)
                {
                    return handle;
                }
            }
            throw new System.Exception("Child window with specified class does not exist.");
        }
        public static string GetWindowTitle(System.IntPtr windowHandle)
        {
            var outputLength = GetWindowTextLength(windowHandle) + 1;
            var outputStringBuilder = new System.Text.StringBuilder(outputLength);
            GetWindowText(windowHandle, outputStringBuilder, outputLength);
            return outputStringBuilder.ToString();
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(System.IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int GetWindowTextLength(System.IntPtr hWnd);
        public static string GetWindowClass(System.IntPtr windowHandle)
        {
            var outputLength = 256;
            var outputStringBuilder = new System.Text.StringBuilder(outputLength);
            GetClassName(windowHandle, outputStringBuilder, outputLength);
            return outputStringBuilder.ToString();
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(System.IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);
        private delegate bool EnumWindowProc(System.IntPtr hwnd, System.IntPtr lParam);
        [System.Runtime.InteropServices.DllImport("user32")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(System.IntPtr window, EnumWindowProc callback, System.IntPtr lParam);
        public static System.IntPtr[] GetChildWindowHandles(System.IntPtr windowHandle)
        {
            System.Collections.Generic.List<System.IntPtr> childHandles = new System.Collections.Generic.List<System.IntPtr>();
            System.Runtime.InteropServices.GCHandle gcChildhandlesList = System.Runtime.InteropServices.GCHandle.Alloc(childHandles);
            System.IntPtr pointerChildHandlesList = System.Runtime.InteropServices.GCHandle.ToIntPtr(gcChildhandlesList);
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
        private static bool EnumWindow(System.IntPtr hWnd, System.IntPtr lParam)
        {
            System.Runtime.InteropServices.GCHandle gcChildhandlesList = System.Runtime.InteropServices.GCHandle.FromIntPtr(lParam);
            if (gcChildhandlesList.Target == null)
            {
                return false;
            }
            System.Collections.Generic.List<System.IntPtr> childHandles = gcChildhandlesList.Target as System.Collections.Generic.List<System.IntPtr>;
            childHandles.Add(hWnd);
            return true;
        }
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        private static extern bool QueryFullProcessImageName(System.IntPtr hProcess, uint dwFlags, System.Text.StringBuilder lpExeName, ref uint lpdwSize);
        public static string GetMainModuleFileName(System.IntPtr processHandle, int maxPathLength = 1024)
        {
            var fileNameBuilder = new System.Text.StringBuilder(maxPathLength);
            uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
            if (QueryFullProcessImageName(processHandle, 0, fileNameBuilder, ref bufferLength))
            {
                return fileNameBuilder.ToString();
            }
            else
            {
                throw new System.Exception($"Unable to get main module file path for process with handle \"{processHandle}\".");
            }
        }
    }
}
