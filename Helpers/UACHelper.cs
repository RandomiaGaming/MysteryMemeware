namespace MysteryMemeware
{
    public static class UACHelper
    {
        public static bool CurrentProcessIsAdmin = GetCurrentProcessIsAdmin();
        private static bool GetCurrentProcessIsAdmin()
        {
            bool output = false;
            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                try
                {
                    System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                    output = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
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
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                try
                {
                    System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                    foreach (System.Security.Claims.Claim claim in principal.Claims)
                    {
                        if (claim.Value is "S-1-5-32-544")
                        {
                            output = true;
                            break;
                        }
                    }
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
    }
}