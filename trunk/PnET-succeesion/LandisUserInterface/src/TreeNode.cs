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
        Timer timer;
        BackgroundWorker bgw;

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
        void Run_BackgroundWorker(object sender, EventArgs e)
        {
            if (bgw.IsBusy == false)
            {
                bgw.RunWorkerAsync();
            }
            
        }

        public TreeNode(string Text)
            : base(Text)
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Start();
            timer.Tick += Run_BackgroundWorker;

            bgw = new BackgroundWorker();
            bgw.DoWork += DoWork;
            bgw.RunWorkerCompleted += RunWorkerCompleted;

            NodesToRemove = new List<TreeNode>();
        }


    }
}
