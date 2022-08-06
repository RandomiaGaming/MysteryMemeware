//#approved 08/05/2022 2:17pm
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace MysteryMemeware
{
    public sealed class CustomPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            base.OnPaint(paintEventArgs);
        }
    }
}