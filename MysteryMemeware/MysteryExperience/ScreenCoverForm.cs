using System;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
namespace MysteryExperience
{
    public sealed class ScreenCoverForm : Form
    {
        #region Private Variables
        private readonly Screen _screen = null;
        private ScreenCoverPictureBox _screenCoverPictureBox = null;
        private readonly Image _screenCoverImage = null;
        private bool _recursionLocked = false;
        #endregion
        #region Public Constructors
        public ScreenCoverForm(Screen screen, Image screenCoverImage)
        {
            if (screen is null)
            {
                throw new Exception("screen cannot be null.");
            }
            _screen = screen;
            if (screenCoverImage is null)
            {
                throw new Exception("screenCoverImage cannot be null.");
            }
            _screenCoverImage = (Image)screenCoverImage.Clone();
            _screenCoverPictureBox = new ScreenCoverPictureBox();
            Controls.Add(_screenCoverPictureBox);
            FormClosing += OnFormClosing;
            RefreshFrom();
        }
        #endregion
        #region Private Methods
        public void RefreshFrom()
        {
            DisableMouse();
            if (_recursionLocked)
            {
                return;
            }
            _recursionLocked = true;
            try
            {
                if (Location != _screen.Bounds.Location)
                {
                    Location = _screen.Bounds.Location;
                }
            }
            catch
            {

            }
            try
            {
                if (Width != _screen.Bounds.Width)
                {
                    Width = _screen.Bounds.Width;
                }
            }
            catch
            {

            }
            try
            {
                if (Height != _screen.Bounds.Height)
                {
                    Height = _screen.Bounds.Height;
                }
            }
            catch
            {

            }
            BackColor = Color.White;
            ShowInTaskbar = false;
            TopMost = true;
            AllowDrop = false;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            MinimizeBox = false;
            MaximizeBox = false;
            MinimumSize = new Size(_screen.Bounds.Width, _screen.Bounds.Height);
            Padding = new Padding(0, 0, 0, 0);
            WindowState = FormWindowState.Maximized;
            _screenCoverPictureBox.Image = _screenCoverImage;
            _screenCoverPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _screenCoverPictureBox.Location = _screenCoverPictureBox.Location;
            _screenCoverPictureBox.Width = Width;
            _screenCoverPictureBox.Height = Height;
            _recursionLocked = false;
        }
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            RefreshFrom();
        }
        protected override void OnResizeBegin(EventArgs e)
        {
            RefreshFrom();
        }
        protected override void OnResizeEnd(EventArgs e)
        {
            RefreshFrom();
        }
        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            RefreshFrom();
        }
        #endregion
        private sealed class MessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 0x201 || m.Msg == 0x202 || m.Msg == 0x203 || m.Msg == 0x204 || m.Msg == 0x205 || m.Msg == 0x206)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private void DisableMouse()
        {
            //Cursor.Clip = new Rectangle(0, 0, 1, 1);
            //Cursor.Hide();
            //Application.AddMessageFilter(new MessageFilter());
        }
        private sealed class ScreenCoverPictureBox : PictureBox
        {
            protected override void OnPaint(PaintEventArgs paintEventArgs)
            {
                paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                base.OnPaint(paintEventArgs);
            }
        }
    }
}
