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
    class RenderBasicCircle : RenderBase
    {
        public RenderBasicCircle(Settings s, string n) : base(s, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);
            GetLastIndex(heights);

            DrawOutline(g, heights, Color.BlueViolet);
            DrawCircle(g, heights);
        }
        
        private void DrawOutline(Graphics g, float[] heights, Color inner)
        {
            List<PointF> points = GetCircularPoints(heights, 2.0f, g, 150f);
            
            PathGradientBrush b = new PathGradientBrush(points.ToArray());
            b.CenterColor = inner;
            b.SurroundColors = new Color[] { Color.FromArgb(0, Color.Black) };

            g.FillPolygon(b, points.ToArray());
        }

        private void DrawCircle(Graphics g, float[] heights)
        {
            List<PointF> points = GetCircularPoints(heights, 1.0f, g);
            g.FillPolygon(Brushes.Black, points.ToArray());
        }
    }
}
