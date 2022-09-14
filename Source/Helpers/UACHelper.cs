using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MysteryMemeware.Helpers
{
    public static class UACHelper
    {
        public static bool CurrentProcessIsAdmin = GetCurrentProcessIsAdmin();
        private static bool GetCurrentProcessIsAdmin()
        {
            bool output = false;
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                try
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    output = principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                finally
                {
                    try
                    {
                        identity.Dispose();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
                output = false;
            }
            return output;
        }
        public static bool CurrentUserIsAdmin = GetCurrentUserIsAdmin();
        private static bool GetCurrentUserIsAdmin()
        {
            bool output = false;
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                try
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    output = !(principal.Claims.FirstOrDefault(c => c.Value == "S-1-5-32-544") is null);
                }
                finally
                {
                    try
                    {
                        identity.Dispose();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
                output = false;
            }
            return output;
        }
        public static bool ElevateWithPopup()
        {
            try
            {
                ProcessHelper.Start(Program.CurrentExePath, WindowMode.Default, true, Environment.CurrentDirectory);
                return true;
            }
            catch (UserDeclinedUACException)
            {
                return false;
            }
        }
    }
}
