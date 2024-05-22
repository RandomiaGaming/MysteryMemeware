namespace MysteryMemeware
{
    public static class VMCheckModule
    {
        public static void VMCheck()
        {
            try
            {
                System.Management.ManagementObjectSearcher searcher1 = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                foreach (System.Management.ManagementBaseObject obj in searcher1.Get())
                {
                    string manufacturer = obj["Manufacturer"]?.ToString().ToLower();
                    string model = obj["Model"]?.ToString().ToLower();

                    if ((manufacturer.Contains("microsoft") && model.Contains("virtual")) || model.Contains("virtualbox") || model.Contains("vmware") || model.Contains("qemu") || model.Contains("bochs") || model.Contains("parallels") || model.Contains("xen"))
                    {
                        return;
                    }
                }
                searcher1.Dispose();

                System.Management.ManagementObjectSearcher searcher2 = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                foreach (System.Management.ManagementBaseObject obj in searcher2.Get())
                {
                    string manufacturer = obj["Manufacturer"]?.ToString().ToLower();

                    if (manufacturer.Contains("microsoft") || manufacturer.Contains("vmware") || manufacturer.Contains("virtualbox"))
                    {
                        return;
                    }
                }
                searcher2.Dispose();

                System.Management.ManagementObjectSearcher searcher3 = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Bios");
                foreach (System.Management.ManagementBaseObject obj in searcher3.Get())
                {
                    string manufacturer = obj["Manufacturer"]?.ToString().ToLower();
                    string version = obj["Version"]?.ToString();

                    if (manufacturer.Contains("innotek") && version.Contains("virtualbox"))
                    {
                        return;
                    }
                }
                searcher3.Dispose();
            }
            catch
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}