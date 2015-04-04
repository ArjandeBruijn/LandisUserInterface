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
            TreeNode tx = (TreeNode)x;
            TreeNode ty = (TreeNode)y;

            if (tx.RankNumber < ty.RankNumber)
            {
                return -1;
            }
            if (tx.RankNumber > ty.RankNumber)
            {
                return 1;
            }

             
            return 0;
        }
        
    }
}
