using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using ColorMine.ColorSpaces;

namespace AudioVisualizer
{
    class RenderOutlineCircle : RenderBase
    {
        public RenderOutlineCircle(Settings s, string n) : base(s, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);
            GetLastIndex(heights);

            DrawCircle(g, heights);
        }

        private void DrawCircle(Graphics g, float[] heights)
        {
            List<PointF> points = GetCircularPoints(heights, 1.0f, g);
            points.Add(points[0]);

            Pen p = new Pen(Color.White, 2.0f);

            g.DrawLines(p, points.ToArray());
        }
    }
}
