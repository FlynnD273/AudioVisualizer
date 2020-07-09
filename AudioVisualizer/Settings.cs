using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace AudioVisualizer
{
    class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private float _xScale;
        public float XScale
        {
            get => _xScale;
            set
            {
                if (value != _xScale)
                {
                    _xScale = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private float _yScale;

        public float YScale
        {
            get { return _yScale; }
            set
            {
                _yScale = value;
                NotifyPropertyChanged();
            }
        }

        private int _smoothing;

        public int Smoothing
        {
            get { return _smoothing; }
            set
            {
                _smoothing = value;
                NotifyPropertyChanged();
            }
        }

        private int _samplePow;

        public int SamplePow
        {
            get { return _samplePow; }
            set
            {
                _samplePow = value;
                _sampleCount = (int)Math.Pow(2, _samplePow);
                NotifyPropertyChanged();
            }
        }

        private int _sampleCount;
        public int SampleCount 
        {
            get { return _sampleCount; } 
        }

        private List<ColorSetting> _colors;

        public List<ColorSetting> Colors
        {
            get { return _colors; }
            set
            {
                _colors = value;
                NotifyPropertyChanged();
            }
        }

        public Settings (float x, float y, int samplePow, int smoothing)
        {
            XScale = x;
            YScale = y;
            SamplePow = samplePow;
            Smoothing = smoothing;
            Colors = new List<ColorSetting>();
        }
    }
}
