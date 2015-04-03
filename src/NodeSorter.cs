using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    class NodeSorter : System.Collections.IComparer
    {
        public delegate int GetRankNumber(System.Windows.Forms.TreeNode node);

        GetRankNumber get_rank_number;

        public NodeSorter(GetRankNumber get_rank_number)
        {
            this.get_rank_number = get_rank_number;
        }

        public int Compare(object x, object y)
        {
            System.Windows.Forms.TreeNode tx = (System.Windows.Forms.TreeNode)x;
            System.Windows.Forms.TreeNode ty = (System.Windows.Forms.TreeNode)y;

            if (get_rank_number(tx) < get_rank_number(ty))
            {
                return -1;
            }

            if (get_rank_number(tx) > get_rank_number(ty))
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
