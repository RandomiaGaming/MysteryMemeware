using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace MysteryMemeware
{
    public static class VolumeHelper
    {
        public static float GetVolume()
        {
            Marshal.ThrowExceptionForHR(Vol().GetMasterVolumeLevelScalar(out float v));
            return v;
        }
        public static void SetVolume(float volumeLevel)
        {
            if (volumeLevel < 0 || volumeLevel > 1)
            {
                throw new Exception("volumeLevel must be between 0 and 1.");
            }
            Marshal.ThrowExceptionForHR(Vol().SetMasterVolumeLevelScalar(volumeLevel, Guid.Empty));
        }
        public static bool GetMute()
        {
            Marshal.ThrowExceptionForHR(Vol().GetMute(out bool mute));
            return mute;
        }
        public static void SetMute(bool muteState)
        {
            Marshal.ThrowExceptionForHR(Vol().SetMute(muteState, Guid.Empty));
        }
        public static bool ToggleMute()
        {
            bool currentMute = !GetMute();
            SetMute(currentMute);
            return currentMute;
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
        #region Pinvoke Calls
        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAudioEndpointVolume
        {
            int f();
            int g();
            int h();
            int i();
            int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
            int j();
            int GetMasterVolumeLevelScalar(out float pfLevel);
            int k();
            int l();
            int m();
            int n();
            int SetMute([MarshalAs(UnmanagedType.Bool)] bool bMute, Guid pguidEventContext);
            int GetMute(out bool pbMute);
        }
        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDevice
        {
            int Activate(ref Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
        }
        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDeviceEnumerator
        {
            int f();
            int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
        }
        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        private class MMDeviceEnumeratorComObject
        {

        }
        private static IAudioEndpointVolume Vol()
        {
            var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
            Marshal.ThrowExceptionForHR(enumerator.GetDefaultAudioEndpoint(0, 1, out IMMDevice dev));
            var epvid = typeof(IAudioEndpointVolume).GUID;
            Marshal.ThrowExceptionForHR(dev.Activate(ref epvid, 23, 0, out IAudioEndpointVolume epv));
            return epv;
        }
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        #endregion
    }
}