using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    public static class Color
    {
        public static uint ToUInt(this System.Drawing.Color c)
        {

            return (uint)((((c.A << 24) |c.B << 16) | (c.G << 8) | c.R) & 0xffffffffL);
        }
        
        public  static System.Drawing.Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte b = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte r = (byte)(color >> 0);
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

    }
}
