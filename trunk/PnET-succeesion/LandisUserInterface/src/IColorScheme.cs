using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    public interface IColorScheme
    {
        int ColorCount { get; }

        System.Drawing.Color NextColor { get; }

        

    }
}
