namespace MysteryMemeware
{
    public static class DisplayCoverModule
    {
        public static void CoverDisplays()
        {
            int screenCount = System.Windows.Forms.Screen.AllScreens.Length;
            for (int i = 0; i < screenCount; i++)
            {
                int localI = i;
                System.Threading.Thread thread = new System.Threading.Thread(() =>
                {
                    System.Drawing.Image coverImage = System.Drawing.Image.FromStream(typeof(Program).Assembly.GetManifestResourceStream("MysteryMemeware.CoverImage.png"));
                    System.Windows.Forms.Form displayCoverForm = new DisplayCoverForm(localI, coverImage);
                    displayCoverForm.ShowDialog();
                });
                thread.Start();
            }
        }
        private sealed class DisplayCoverForm : System.Windows.Forms.Form
        {
            public DisplayCoverForm(int screenID, System.Drawing.Image coverImage)
            {
                this.BackColor = System.Drawing.Color.White;

                this.ShowInTaskbar = false;

                this.TopMost = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;

                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens[screenID];

                this.Location = screen.Bounds.Location;
                this.Width = screen.Bounds.Width;
                this.Height = screen.Bounds.Height;

                CustomPictureBox pictureBox = new CustomPictureBox();

                double targetAspectRatio = coverImage.Width / (double)coverImage.Height;

                double viewPortWidth = screen.Bounds.Width;
                double viewPortHeight = screen.Bounds.Height;

                double renderWidth = viewPortHeight * targetAspectRatio;
                double renderHeight = viewPortWidth / targetAspectRatio;

                if (renderWidth > viewPortWidth)
                {
                    renderWidth = viewPortWidth;
                }
                if (renderHeight > viewPortHeight)
                {
                    renderHeight = viewPortHeight;
                }

                double renderX = (viewPortWidth - renderWidth) / 2;
                double renderY = (viewPortHeight - renderHeight) / 2;

                pictureBox.Location = new System.Drawing.Point((int)renderX, (int)renderY);
                pictureBox.Width = (int)renderWidth;
                pictureBox.Height = (int)renderHeight;

                pictureBox.Image = coverImage;
                pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

                this.Controls.Add(pictureBox);

                this.FormClosing += OnFormClosing;

                this.Load += OnFormLoad;
            }

            private void OnFormLoad(object sender, System.EventArgs e)
            {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Tick += OnTimerTick;
                timer.Interval = 100;
                timer.Start();
            }
            private void OnTimerTick(object sender, System.EventArgs e)
            {
                System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(this.Location, new System.Drawing.Size(1, 1));
                this.TopMost = true;
                ///AllowSetForegroundWindow(this.Handle);
                //SetForegroundWindow(this.Handle);
            }
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            static extern bool SetForegroundWindow(System.IntPtr hWnd);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            private static extern bool AllowSetForegroundWindow(System.IntPtr hWnd);
            private void OnFormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
            {
                e.Cancel = true;
            }
            private sealed class CustomPictureBox : System.Windows.Forms.PictureBox
            {
                protected override void OnPaint(System.Windows.Forms.PaintEventArgs paintEventArgs)
                {
                    paintEventArgs.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    base.OnPaint(paintEventArgs);
                }
            }
        }
    }
}