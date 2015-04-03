using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public class TreeNode : System.Windows.Forms.TreeNode
    {
         
        public delegate TreeNode[] GetSubNodes(TreeNode me);

         

        public TreeNode(string FullPath, string Text, string ImageKey, GetSubNodes get_sub_nodes)
            
        {
            if (FrmMain.BackgroundWorkerCancellationPending) return;
            this.Tag =this.Name = this.ToolTipText = FullPath;
            this.Text = Text;
            this.ImageKey = this.SelectedImageKey = ImageKey;

            if (get_sub_nodes != null)
            {
                foreach (TreeNode node in get_sub_nodes(this))
                {
                    Nodes.Add(node);
                }
            }
           
          
        }


    }
}
