namespace MysteryHelper
{
    public static class VolumeHelper
    {
        public static float GetVolume()
        {
            System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(Vol().GetMasterVolumeLevelScalar(out float v));
            return v;
        }
        public static void SetVolume(float volumeLevel)
        {
            if (volumeLevel < 0 || volumeLevel > 1)
            {
                throw new System.Exception("volumeLevel must be between 0 and 1.");
            }
            System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(Vol().SetMasterVolumeLevelScalar(volumeLevel, System.Guid.Empty));
        }
        public static bool GetMute()
        {
            System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(Vol().GetMute(out bool mute));
            return mute;
        }
        public static void SetMute(bool muteState)
        {
            System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(Vol().SetMute(muteState, System.Guid.Empty));
        }
        public static bool ToggleMute()
        {
            bool currentMute = !GetMute();
            SetMute(currentMute);
            return currentMute;
        }
        public static void ToggleMuteWithPopup()
        {
            System.IntPtr processHandle = ProcessHelper.CurrentProcess.Handle;
            SendMessageW(processHandle, 793, processHandle, (System.IntPtr)524288);
        }
        public static void VolumeDownWithPopup()
        {
            System.IntPtr processHandle = ProcessHelper.CurrentProcess.Handle;
            SendMessageW(processHandle, 793, processHandle, (System.IntPtr)589824);
        }
        public static void VolumeUpWithPopup()
        {
            System.IntPtr processHandle = ProcessHelper.CurrentProcess.Handle;
            SendMessageW(processHandle, 793, processHandle, (System.IntPtr)655360);
        }
        public static void ShowVolumePopup()
        {
            bool muteState = GetMute();
            float volumeLevel = GetVolume();
            VolumeUpWithPopup();
            SetVolume(volumeLevel);
            SetMute(muteState);
        }
        #region ComInterfaces
        [System.Runtime.InteropServices.Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
        private interface IAudioEndpointVolume
        {
            int f();
            int g();
            int h();
            int i();
            int SetMasterVolumeLevelScalar(float fLevel, System.Guid pguidEventContext);
            int j();
            int GetMasterVolumeLevelScalar(out float pfLevel);
            int k();
            int l();
            int m();
            int n();
            int SetMute([System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)] bool bMute, System.Guid pguidEventContext);
            int GetMute(out bool pbMute);
        }
        [System.Runtime.InteropServices.Guid("D666063F-1587-4E43-81F1-B948E807363F"), System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDevice
        {
            int Activate(ref System.Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
        }
        [System.Runtime.InteropServices.Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDeviceEnumerator
        {
            int f();
            int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
        }
        [System.Runtime.InteropServices.ComImport, System.Runtime.InteropServices.Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        private class MMDeviceEnumeratorComObject
        {
        }
        private static IAudioEndpointVolume Vol()
        {
            var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
            System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(enumerator.GetDefaultAudioEndpoint(0, 1, out IMMDevice dev));
            var epvid = typeof(IAudioEndpointVolume).GUID;
            System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(dev.Activate(ref epvid, 23, 0, out IAudioEndpointVolume epv));
            return epv;
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern System.IntPtr SendMessageW(System.IntPtr hWnd, int Msg, System.IntPtr wParam, System.IntPtr lParam);
        #endregion
    }
}