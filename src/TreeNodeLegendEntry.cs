using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    public class TreeNodeLegendEntry : System.Windows.Forms.TreeNode
    {
        public double Min{get; private set;}
        public double Max {get; private set;}
        


        public TreeNodeLegendEntry(double Min, double Max)
        {
            this.Min = Min;
            this.Max = Max;

            this.Text = Min.ToString() + "-" + Max.ToString();

            this.ImageKey = Min.ToString() + "-" + Max.ToString();
        }
    }
}
