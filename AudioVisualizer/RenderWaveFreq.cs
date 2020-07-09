using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace AudioVisualizer
{
    class RenderWaveFreq : RenderBase
    {
        public RenderWaveFreq(Settings s, Panel c, string n) : base(s, c, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);
            GetLastIndex(heights);
            List<PointF> points = new List<PointF>();

            points.Add(new PointF(0, DrawingPanel.Height / 2));

            for (int i = 0; i < lastIndex; i++)
            {
                float height = Smooth(heights, i, Settings.Smoothing);
                float x = i / (float)heights.Length * DrawingPanel.Width * Settings.XScale;
                float y = (float)(DrawingPanel.Height / 2 - height * Settings.YScale * ((i % 2) * 2 - 1));

                points.Add(new PointF(x, y));
            }

            points.Add(new PointF(DrawingPanel.Width, DrawingPanel.Height / 2));

            Pen p = new Pen(Color.White, 2.0f);

            g.DrawLines(p, points.ToArray());
        }
    }
}
