using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace AudioVisualizer
{
    class RenderBasicCircle : RenderBase
    {
        public RenderBasicCircle(Settings s, PictureBox c, string n) : base(s, c, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);

            List<PointF> points = new List<PointF>();
            
            for (int i = 0; i < heights.Length; i++)
            {
                double angle = (double)i / heights.Length * Math.PI * 2 * Settings.XScale;
                double x = Math.Cos(angle - Math.PI / 2) * (heights[i] * Settings.YScale + 100) + DrawingPictureBox.Width / 2;
                double y = Math.Sin(angle - Math.PI / 2) * (heights[i] * Settings.YScale + 100) + DrawingPictureBox.Height / 2;
                points.Add(new PointF((float)x, (float)y));

                if (angle > Math.PI * 2)
                {
                    break;
                }
            }

            //LinearGradientBrush b = new LinearGradientBrush(new Point(0, 0), new Point(DrawingPictureBox.Width, 0), Color.BlueViolet, Color.OrangeRed);
            Brush b = Brushes.White;
            g.FillPolygon(b, points.ToArray());
        }
    }
}
