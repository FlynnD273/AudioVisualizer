using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace AudioVisualizer
{
    class RenderReflectedCircle : RenderBase
    {
        public RenderReflectedCircle(Settings s, Panel c, string n) : base(s, c, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);
            GetLastIndex(heights);

            float minX = DrawingPanel.Width;
            float maxX = 0;

            List<PointF> points = new List<PointF>();
            int i;
            for (i = 0; i < lastIndex; i++)
            {
                double angle = (double)i / heights.Length * Math.PI * Settings.XScale;

                float height = Smooth(heights, i, Settings.Smoothing);

                double x = Math.Cos(angle - Math.PI / 2) * (height * Settings.YScale + 100) + DrawingPanel.Width / 2;
                double y = Math.Sin(angle - Math.PI / 2) * (height * Settings.YScale + 100) + DrawingPanel.Height / 2;
                points.Add(new PointF((float)x, (float)y));

                minX = (float)Math.Min(minX, x);
                maxX = (float)Math.Max(maxX, x);
            }
            i--;
            for (  ; i > 0; i--)
            {
                double angle = (double)-i / heights.Length * Math.PI * Settings.XScale;

                float height = Smooth(heights, i, Settings.Smoothing);

                double x = Math.Cos(angle - Math.PI / 2) * (height * Settings.YScale + 100) + DrawingPanel.Width / 2;
                double y = Math.Sin(angle - Math.PI / 2) * (height * Settings.YScale + 100) + DrawingPanel.Height / 2;
                points.Add(new PointF((float)x, (float)y));

                minX = (float)Math.Min(minX, x);
                maxX = (float)Math.Max(maxX, x);
            }

            LinearGradientBrush b = new LinearGradientBrush(new PointF(minX, 0), new PointF(maxX, 0), Color.BlueViolet, Color.OrangeRed);
            g.FillPolygon(b, points.ToArray());
        }
    }
}
