using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    class TreeNode : System.Windows.Forms.TreeNode
    {
        public delegate void SendMessage(string msg);

        public static SendMessage sendmessage;

        List<TreeNode> NodesToRemove;


        void DoWork(object sender, DoWorkEventArgs e)
        {
            sendmessage("Updating " + this.Tag.ToString());

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
            
        {
            this.Tag = this.Text = Text;

            TimerBackgroundWorker.BackGroundWorker.DoWork += DoWork;
            TimerBackgroundWorker.BackGroundWorker.RunWorkerCompleted += RunWorkerCompleted;

            NodesToRemove = new List<TreeNode>();
        }


    }
}
