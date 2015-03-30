using System.Windows.Forms;
using System.Collections.Generic;


namespace LandisUserInterface
{
    class UpdateBackgroundWorker : System.ComponentModel.BackgroundWorker
    {
        List<TreeNode[]> nodes = new List<TreeNode[]>();
        AddOrRemove add_or_remove;

        public enum AddOrRemove
        { 
            Add,
            Remove
        }

        public UpdateBackgroundWorker(AddOrRemove add_or_remove)
        {
            this.add_or_remove = add_or_remove;
            RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            foreach (TreeNode[] node in nodes)
            {
                if (add_or_remove == AddOrRemove.Add) node[0].Nodes.Add(node[1]);
                else node[0].Nodes.Remove(node[1]);
            }
            nodes.Clear();
        }
         
        public void Schedule(TreeNode[] NodeToAdd)
        {
            this.nodes.Add(NodeToAdd);
        }
        

        
    }
}
