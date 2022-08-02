using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace MysteryMemeware
{
    public static class VolumeHelper
    {
        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IAudioEndpointVolume
        {
            // f(), g(), ... are unused COM method slots. Define these if you care
            int f(); int g(); int h(); int i();
            int SetMasterVolumeLevelScalar(float fLevel, System.Guid pguidEventContext);
            int j();
            int GetMasterVolumeLevelScalar(out float pfLevel);
            int k(); int l(); int m(); int n();
            int SetMute([MarshalAs(UnmanagedType.Bool)] bool bMute, System.Guid pguidEventContext);
            int GetMute(out bool pbMute);
        }
        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDevice
        {
            int Activate(ref System.Guid id, int clsCtx, int activationParams, out IAudioEndpointVolume aev);
        }
        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IMMDeviceEnumerator
        {
            int f(); // Unused
            int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice endpoint);
        }
        [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")] class MMDeviceEnumeratorComObject { }
        public class Audio
        {
            static IAudioEndpointVolume Vol()
            {
                var enumerator = new MMDeviceEnumeratorComObject() as IMMDeviceEnumerator;
                IMMDevice dev = null;
                Marshal.ThrowExceptionForHR(enumerator.GetDefaultAudioEndpoint(/*eRender*/ 0, /*eMultimedia*/ 1, out dev));
                IAudioEndpointVolume epv = null;
                var epvid = typeof(IAudioEndpointVolume).GUID;
                Marshal.ThrowExceptionForHR(dev.Activate(ref epvid, /*CLSCTX_ALL*/ 23, 0, out epv));
                return epv;
            }
            public static float Volume
            {
                get { float v = -1; Marshal.ThrowExceptionForHR(Vol().GetMasterVolumeLevelScalar(out v)); return v; }
                set { Marshal.ThrowExceptionForHR(Vol().SetMasterVolumeLevelScalar(value, System.Guid.Empty)); }
            }
            public static bool Mute
            {
                get { bool mute; Marshal.ThrowExceptionForHR(Vol().GetMute(out mute)); return mute; }
                set { Marshal.ThrowExceptionForHR(Vol().SetMute(value, System.Guid.Empty)); }
            }
        }
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        public static void ToggleMute()
        {
            IntPtr processHandle = Process.GetCurrentProcess().Handle;
            SendMessageW(processHandle, WM_APPCOMMAND, processHandle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
        public static void VolumeDown()
        {
            IntPtr processHandle = Process.GetCurrentProcess().Handle;
            SendMessageW(processHandle, WM_APPCOMMAND, processHandle, (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }
        public static void VolumeUp()
        {
            IntPtr processHandle = Process.GetCurrentProcess().Handle;
            SendMessageW(processHandle, WM_APPCOMMAND, processHandle, (IntPtr)APPCOMMAND_VOLUME_UP);
        }
    }
}