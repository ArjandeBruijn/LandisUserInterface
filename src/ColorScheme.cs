using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LandisUserInterface
{
    public class ColorScheme : IColorScheme
    {
        int n = -1;

        public int ColorCount
        {
            get
            {
                return ColorValues.Count();
            }
        }
        string[] ColourStrings = new string[] 
        { 
            "FF0000", "00FF00", "0000FF", "FFFF00", "FF00FF", "00FFFF", "000000", 
            "800000", "008000", "000080", "808000", "800080", "008080", "808080", 
            "C00000", "00C000", "0000C0", "C0C000", "C000C0", "00C0C0", "C0C0C0", 
            "400000", "004000", "000040", "404000", "400040", "004040", "404040", 
            "200000", "002000", "000020", "202000", "200020", "002020", "202020", 
            "600000", "006000", "000060", "606000", "600060", "006060", "606060", 
            "A00000", "00A000", "0000A0", "A0A000", "A000A0", "00A0A0", "A0A0A0", 
            "E00000", "00E000", "0000E0", "E0E000", "E000E0", "00E0E0", "E0E0E0", 
        };
        public System.Drawing.Color[] DefaultColorValues
        { 
            get
            {
                List<System.Drawing.Color> Colors = new List<System.Drawing.Color>();

                foreach (string s in ColourStrings)
                {
                    Colors.Add(System.Drawing.ColorTranslator.FromHtml("#" + s));
                }

                return Colors.ToArray();
            }
        }
        private System.Drawing.Color[] ColorValues;

        public ColorScheme(System.Drawing.Color[] ColorValues = null)
        {
            if (ColorValues == null)
            {
                this.ColorValues = DefaultColorValues;
            }
            else
            {
                this.ColorValues = ColorValues;
            }
            
        }
        public System.Drawing.Color NextColor
        {
            get
            {
                n++;

                if (n > ColorValues.Count() - 1) n = 0;
                return ColorValues[n];
            }
        }
    }
}
