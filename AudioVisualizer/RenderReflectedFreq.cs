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
        public RenderReflectedFreq(Settings s, string n) : base(s, n) { }

        public override void Render(Graphics g, float[] samples)
        {
            float[] heights = FFT.SampleToFreq(samples, Settings.SampleCount);

            List<PointF> points = new List<PointF>();
            List<PointF> pointsReflected = new List<PointF>();

            points.Add(new PointF(0,  g.VisibleClipBounds.Height / 2));
            pointsReflected.Add(new PointF(0,  g.VisibleClipBounds.Height / 2));

            for (int i = 0; i < heights.Length; i++)
            {
                float height = Smooth(heights, i, Settings.Smoothing);

                points.Add(new PointF(i / (float)heights.Length *  g.VisibleClipBounds.Width * Settings.XScale, (float)( g.VisibleClipBounds.Height / 2 - height * Settings.YScale)));
                pointsReflected.Add(new PointF(i / (float)heights.Length *  g.VisibleClipBounds.Width * Settings.XScale, (float)( g.VisibleClipBounds.Height / 2 + height * Settings.YScale)));


                if (i / (float)heights.Length *  g.VisibleClipBounds.Width * Settings.XScale >  g.VisibleClipBounds.Width)
                {
                    break;
                }
            }

            points.Add(new PointF( g.VisibleClipBounds.Width,  g.VisibleClipBounds.Height / 2));
            pointsReflected.Add(new PointF( g.VisibleClipBounds.Width,  g.VisibleClipBounds.Height / 2));

            //Draw top

            LinearGradientBrush b = new LinearGradientBrush(new PointF(0, 0), new PointF(g.VisibleClipBounds.Width, 0), Color.BlueViolet, Color.OrangeRed);

            g.FillPolygon(b, points.ToArray());

            b = new LinearGradientBrush(new PointF(0, 0), new PointF(0, g.VisibleClipBounds.Height / 2), Color.White, Color.FromArgb(0, Color.White));

            g.FillPolygon(b, points.ToArray());

            //Draw bottom

            b = new LinearGradientBrush(new PointF(0, 0), new PointF(g.VisibleClipBounds.Width, 0), Color.BlueViolet, Color.OrangeRed);

            g.FillPolygon(b, pointsReflected.ToArray());

            b = new LinearGradientBrush(new PointF(0, g.VisibleClipBounds.Height / 2 - 1), new PointF(0,  g.VisibleClipBounds.Height), Color.FromArgb(0, Color.Black), Color.Black);

            g.FillPolygon(b, pointsReflected.ToArray());
        }
    }
}
