using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    class NodeSorter : System.Collections.IComparer
    {
      
        public int Compare(object x, object y)
        {
            if (x.GetType() != typeof(TreeNode) || y.GetType() != typeof(TreeNode)) return 0;
  
            TreeNode tx = (TreeNode)x;
            TreeNode ty = (TreeNode)y;

            if (tx.Year < ty.Year)
            {
                return -1;
            }

            if (tx.Year > ty.Year)
            {
                return 1;
            }

            return 0;
        }
        /*
        static int get_Year(string FileName)
        {
            int year = -1;
            if (int.TryParse(System.Text.RegularExpressions.Regex.Match(FileName, @"\d+").Value, out year))
            {
                return year;
            }
            return year;
        }
         */
    }
}
