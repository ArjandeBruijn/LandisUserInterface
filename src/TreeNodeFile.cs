using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public class TreeNodeFile : System.Windows.Forms.TreeNode
    {
         
        public delegate TreeNodeFile[] GetSubNodes(TreeNodeFile me);

        new public string FullPath;
        GetSubNodes get_sub_nodes;
        Func<bool> IsCancelled;
        public int RankNumber { get; private set; }
        public int Layerhandle;
        new public TreeNodeFile Clone()
        {
            TreeNodeFile node = new TreeNodeFile(FullPath, Text, RankNumber, ImageKey, get_sub_nodes, IsCancelled);
            node.Layerhandle = Layerhandle;
            return node;
        }

        public TreeNodeFile(string FullPath, string Text, int RankNumber, string ImageKey, GetSubNodes get_sub_nodes, Func<bool> IsCancelled)
            
        {
            this.IsCancelled = IsCancelled;
            this.get_sub_nodes = get_sub_nodes;
            this.RankNumber = RankNumber;
            this.FullPath = FullPath;
            this.Tag =this.Name = this.ToolTipText = FullPath;
            this.Text = Text;
            this.ImageKey = this.SelectedImageKey = ImageKey;

            if(IsCancelled())return;

            if (get_sub_nodes != null)
            {
                foreach (TreeNodeFile node in get_sub_nodes(this))
                {
                    Nodes.Add(node);
                }
            }
           
          
        }


    }
}
