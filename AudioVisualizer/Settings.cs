using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVisualizer
{
    class Settings
    {
        public float XScale { get; set; }
        public float YScale { get; set; }
        public int Smoothing { get; set; }
        private int _sampleCount;
        public int SampleCount 
        {
            get { return _sampleCount; } 

            set 
            {
                if (Math.Abs(Math.Log2(value) % 1) <= (double.Epsilon * 100))
                {
                    _sampleCount = value;
                }
                else
                {
                    _sampleCount = (int)Math.Pow(2, Math.Floor(Math.Log2(value)));
                }
            } 
        }

        public Settings (float x, float y, int samples, int smoothing)
        {
            XScale = x;
            YScale = y;
            SampleCount = samples;
            Smoothing = smoothing;
        }
    }
}
