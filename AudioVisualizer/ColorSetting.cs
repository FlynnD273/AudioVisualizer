using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AudioVisualizer
{
    class ColorSetting
    {
        public Color StoredColor { get; set; }
        public string Name { get; set; }

        public ColorSetting (Color c, string n)
        {
            StoredColor = c;
            Name = n;
        }
    }
}
