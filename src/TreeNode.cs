using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    class TreeNode : System.Windows.Forms.TreeNode
    {
         
        List<TreeNode> NodesToRemove;


        void DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (TreeNode tree_node in Nodes)
            {
                if (System.IO.File.Exists(tree_node.ToolTipText) == false && System.IO.Directory.Exists(tree_node.ToolTipText) == false)
                {
                    NodesToRemove.Add(tree_node);
                }
            }
        }
        void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach(TreeNode treenode in NodesToRemove)
            {
                treenode.Remove();
            }
        }
       
        public TreeNode(string Text)
            : base(Text)
        {
            TimerBackgroundWorker.BackGroundWorker.DoWork += DoWork;
            TimerBackgroundWorker.BackGroundWorker.RunWorkerCompleted += RunWorkerCompleted;

            NodesToRemove = new List<TreeNode>();
        }


    }
}
