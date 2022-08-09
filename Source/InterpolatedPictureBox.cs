//#approved 08/07/2022 1:03am
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace MysteryMemeware
{
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
}