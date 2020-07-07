using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVisualizer
{
    class Settings
    {
        private float _xScale = 1.0f;
        public float XScale { get => _xScale; set => _xScale = value; }
        private float _yScale = 0.75f;
        public float YScale { get => _yScale; set => _yScale = value; }
        private int _sampleCount = 8192;
        public int SampleCount 
        { 
            get => _sampleCount; 

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

        public Settings (float x, float y, int samples)
        {
            XScale = x;
            YScale = y;
            SampleCount = samples;
        }
    }
}
