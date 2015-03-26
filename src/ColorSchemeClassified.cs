using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LandisUserInterface
{
    public class ColorSchemeClassified : IColorScheme
    {
        int n = -1;
        List<System.Drawing.Color> ColourValues = new List<System.Drawing.Color>();

        public int ColorCount
        {
            get
            {
                return ColourValues.Count();
            }
        }

        public System.Drawing.Color NextColor
        {
            get
            {
                n++;

                if (n > ColourValues.Count() - 1) n = ColourValues.Count() - 1;
                return ColourValues[n];
                
            }
        }
        byte InterPolate(float Fraction, byte Min, byte Max)
        {
            return (byte)(Min + (1.0-Fraction) * (Max - Min));
        }
        public ColorSchemeClassified(byte[] Red, byte[] Blue, byte[] Green, int NrOfCategories)
        {
            for (int c = 0; c <= NrOfCategories; c++)
            {
                byte r = InterPolate(c/(float)NrOfCategories,  Red[0], Red[1]);
                byte b = InterPolate(c / (float)NrOfCategories, Blue[0], Blue[1]);
                byte g = InterPolate(c / (float)NrOfCategories, Green[0], Green[1]); 
                
                ColourValues.Add(System.Drawing.Color.FromArgb(r, g, b));
           }
        }
    }
}
