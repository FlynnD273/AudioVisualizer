using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace AudioVisualizer
{
    class RenderBasicFreq : RenderBase
    {
        public RenderBasicFreq(Settings s, Panel c, string n) : base(s, c, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);

            List<PointF> points = new List<PointF>();

            points.Add(new PointF(0, DrawingPanel.Height));

            for (int i = 0; i < heights.Length; i++)
            {
                float height = Smooth(heights, i, Settings.Smoothing);
                points.Add(new PointF(i / (float)heights.Length * DrawingPanel.Width * Settings.XScale, (float)(DrawingPanel.Height - 20.0f - height * Settings.YScale)));

                if (i / (float)heights.Length * DrawingPanel.Width * Settings.XScale > DrawingPanel.Width)
                {
                    break;
                }
            }

            points.Add(new PointF(DrawingPanel.Width, DrawingPanel.Height));

            LinearGradientBrush b = new LinearGradientBrush(new Point(0, 0), new Point(DrawingPanel.Width, 0), Color.BlueViolet, Color.OrangeRed);

            g.FillPolygon(b, points.ToArray());

            b = new LinearGradientBrush(new Point(0, 0), new Point(0, DrawingPanel.Height), Color.ForestGreen, Color.FromArgb(0, Color.ForestGreen));

            g.FillPolygon(b, points.ToArray());
        }
    }
}
