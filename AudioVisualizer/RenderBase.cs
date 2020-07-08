using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AudioVisualizer
{
    abstract class RenderBase
    {
        private Settings _settings;
        public Settings Settings { get => _settings; set => _settings = value; }
        private PictureBox _drawingPictureBox;
        public PictureBox DrawingPictureBox { get => _drawingPictureBox; set => _drawingPictureBox = value; }
        public string Name { get; private set; }

        public RenderBase (Settings s, PictureBox c, string n)
        {
            Settings = s;
            DrawingPictureBox = c;
            Name = n;
        }

        public abstract void Render(Graphics g, float[] samples);

        protected float Smooth(float[] heights, int index, int step)
        {
            int sampleCount = Math.Min(heights.Length - index, step);
            if (sampleCount < 3)
            {
                return heights[index];
            }

            double[] k = Window.BartlettHann(sampleCount);
            float result = 0;
            float total = 0;
            for (int i = 0; i < sampleCount; i++)
            {
                result += (float)(heights[i + index] * k[i]);
                total += (float)k[i];
            }

            result /= step / 2;

            return result;
        }
    }
}
