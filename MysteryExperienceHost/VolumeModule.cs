using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MysteryHelper;

namespace MysteryExperience
{
    public static class VolumeModule
    {
        public static void Run()
        {
            try
            {
                System.Threading.Thread volumeLockThread = new System.Threading.Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            VolumeHelper.SetVolume(1.0f);
                            VolumeHelper.SetMute(false);
                        }
                        catch
                        {

                        }
                    }
                });
                volumeLockThread.Start();
            }
            catch
            {

            }
        }
    }
}
