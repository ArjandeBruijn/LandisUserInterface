using System.Windows.Forms;
using System.Collections.Generic;


namespace LandisUserInterface
{
    class UpdateBackgroundWorker : System.ComponentModel.BackgroundWorker
    {
        public delegate void RemoveDockWindow(string FileName);

        RemoveDockWindow remove_dockwindow;

        Dictionary<TreeNode[], AddOrRemove> nodes = new Dictionary<TreeNode[], AddOrRemove>();
        
        public enum AddOrRemove
        { 
            Add,
            Remove
        }

        public UpdateBackgroundWorker(RemoveDockWindow remove_dockwindow)
        {
            this.remove_dockwindow = remove_dockwindow;

            RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            foreach (KeyValuePair<TreeNode[],AddOrRemove>  node in nodes)
            {
                if (node.Value == AddOrRemove.Add) node.Key[0].Nodes.Add(node.Key[1]);
                else
                {
                    node.Key[0].Nodes.Remove(node.Key[1]);
                    remove_dockwindow(node.Key[1].ToolTipText);
                }
            }
            nodes.Clear();
        }
         
        public void Schedule(TreeNode[] NodeToAdd, AddOrRemove add_or_remove)
        {
            this.nodes.Add(NodeToAdd, add_or_remove);
        }
        

        
    }
}
