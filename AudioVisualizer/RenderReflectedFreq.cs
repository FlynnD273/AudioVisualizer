using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace AudioVisualizer
{
    class RenderReflectedFreq : RenderBase
    {
        public RenderReflectedFreq(Settings s, PictureBox c, string n) : base(s, c, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);

            List<PointF> points = new List<PointF>();

            points.Add(new PointF(0, DrawingPictureBox.Height / 2));
            int i;

            for (i = 0; i < heights.Length; i++)
            {
                points.Add(new PointF(i / (float)heights.Length * DrawingPictureBox.Width * Settings.XScale, (float)(DrawingPictureBox.Height / 2 - heights[i] * Settings.YScale)));

                if (i / (float)heights.Length * DrawingPictureBox.Width * Settings.XScale > DrawingPictureBox.Width)
                {
                    break;
                }
            }

            points.Add(new PointF(DrawingPictureBox.Width, DrawingPictureBox.Height / 2));

            LinearGradientBrush b = new LinearGradientBrush(new Point(0, 0), new Point(DrawingPictureBox.Width, 0), Color.BlueViolet, Color.OrangeRed);

            g.FillPolygon(b, points.ToArray());

            b = new LinearGradientBrush(new Point(0, 0), new Point(0, DrawingPictureBox.Height / 2), Color.White, Color.FromArgb(0, Color.White));

            g.FillPolygon(b, points.ToArray());

            points = new List<PointF>();
            points.Add(new PointF(DrawingPictureBox.Width, DrawingPictureBox.Height / 2));

            for (i--; i > 0; i--)
            {
                points.Add(new PointF(i / (float)heights.Length * DrawingPictureBox.Width * Settings.XScale, (float)(DrawingPictureBox.Height / 2 + heights[i] * Settings.YScale)));
            }

            points.Add(new PointF(0, DrawingPictureBox.Height / 2));

            b = new LinearGradientBrush(new Point(0, 0), new Point(DrawingPictureBox.Width, 0), Color.BlueViolet, Color.OrangeRed);

            g.FillPolygon(b, points.ToArray());

            b = new LinearGradientBrush(new PointF(0, DrawingPictureBox.Height / 2 - 1), new PointF(0, DrawingPictureBox.Height), Color.FromArgb(0, Color.Black), Color.Black);

            g.FillPolygon(b, points.ToArray());
        }
    }
}
