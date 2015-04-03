using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public class TreeNode : System.Windows.Forms.TreeNode, ICloneable
    {
         
        public delegate TreeNode[] GetSubNodes(TreeNode me);

        GetSubNodes get_sub_nodes;

        public TreeNode Clone(int LayerHandle)
        {
            TreeNode Clone = new TreeNode(this.FullPath, this.Text, this.ImageKey, this.get_sub_nodes);

            Clone.LayerHandle = LayerHandle;

            return Clone;
        }
        public string FileName { get; private set; }

        public int LayerHandle { get; private set; }

        public int Year { get; private set; }

        static int get_Year(string FileName)
        {
            int year = -1;
            if (int.TryParse(System.Text.RegularExpressions.Regex.Match(FileName, @"\d+").Value, out year))
            {
                return year;
            }
            return year;
        }

        public TreeNode(string FullPath, string Text, string ImageKey, GetSubNodes get_sub_nodes)
            
        {
            this.FileName = System.IO.Path.GetFileName(FullPath);
            this.Year = get_Year(FileName);
            this.get_sub_nodes = get_sub_nodes;
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
