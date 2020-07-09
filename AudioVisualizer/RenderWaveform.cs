using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AudioVisualizer
{
    class RenderWaveform : RenderBase
    {
        public RenderWaveform(Settings s, Panel c, string n) : base(s, c, n) { }

        public override void Render(Graphics g, float[] heights)
        {
            List<PointF> points = new List<PointF>();

            for (int i = 0; i < heights.Length; i++)
            {
                float height = Smooth(heights, i, Settings.Smoothing);

                points.Add(new PointF(i / (float)heights.Length * DrawingPanel.Width * Settings.XScale, (float)(DrawingPanel.Size.Height / 2 - height * Settings.YScale)));

                if (i / (float)heights.Length * DrawingPanel.Width * Settings.XScale > DrawingPanel.Width)
                {
                    break;
                }
            }

            Pen p = new Pen(Color.White, 2.0f);

            g.DrawLines(p, points.ToArray());
        }
    }
}
