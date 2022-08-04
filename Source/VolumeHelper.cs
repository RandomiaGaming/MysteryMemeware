//#Approve File 08/03/2022 11:35am.
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace MysteryMemeware
{
    public static class VolumeHelper
    {
        public static float GetVolume()
        {
            float v;
            Marshal.ThrowExceptionForHR(Vol().GetMasterVolumeLevelScalar(out v));
            return v;
        }
        public static void SetVolume(float volumeLevel)
        {
            if(volumeLevel < 0 || volumeLevel > 1)
            {
                throw new Exception("volumeLevel must be between 0 and 1.");
            }
            Marshal.ThrowExceptionForHR(Vol().SetMasterVolumeLevelScalar(volumeLevel, Guid.Empty));
        }
        public static bool GetMute()
        {
            bool mute;
            Marshal.ThrowExceptionForHR(Vol().GetMute(out mute)); return mute;
        }
        public static void SetMute(bool muteState)
        {
            Marshal.ThrowExceptionForHR(Vol().SetMute(muteState, Guid.Empty));
        }
        public static void ToggleMuteWithPopup()
        {
            IntPtr processHandle = Process.GetCurrentProcess().Handle;
            SendMessageW(processHandle, 793, processHandle, (IntPtr)524288);
        }
        public static void VolumeDownWithPopup()
        {
            IntPtr processHandle = Process.GetCurrentProcess().Handle;
            SendMessageW(processHandle, 793, processHandle, (IntPtr)589824);
        }
        public static void VolumeUpWithPopup()
        {
            IntPtr processHandle = Process.GetCurrentProcess().Handle;
            SendMessageW(processHandle, 793, processHandle, (IntPtr)655360);
        }
        public static void ShowVolumePopup()
        {
            bool muteState = GetMute();
            float volumeLevel = GetVolume();
            VolumeUpWithPopup();
            SetVolume(volumeLevel);
            SetMute(muteState);
        }
        //Internals
        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAudioEndpointVolume
        {
            public int f();
            public int g();
            public int h();
            public int i();
            public int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
            public int j();
            public int GetMasterVolumeLevelScalar(out float pfLevel);
            public int k();
            public int l();
            public int m();
            public int n();
            public int SetMute([MarshalAs(UnmanagedType.Bool)] bool bMute, Guid pguidEventContext);
            public int GetMute(out bool pbMute);
        }
        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDevice
        {
            public int Activate(ref Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
        }
        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDeviceEnumerator
        {
            public int f();
            public int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
        }
        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        private class MMDeviceEnumeratorComObject
        {

        }
        private static IAudioEndpointVolume Vol()
        {
            var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
            IMMDevice dev = null;
            Marshal.ThrowExceptionForHR(enumerator.GetDefaultAudioEndpoint(0, 1, out dev));
            IAudioEndpointVolume epv = null;
            var epvid = typeof(IAudioEndpointVolume).GUID;
            Marshal.ThrowExceptionForHR(dev.Activate(ref epvid, 23, 0, out epv));
            return epv;
        }
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}