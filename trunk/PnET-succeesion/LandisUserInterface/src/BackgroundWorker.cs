using System.Windows.Forms;


namespace LandisUserInterface
{
    class BackgroundWorker : System.ComponentModel.BackgroundWorker
    {
        TreeNode[] node;
        AddOrRemove add_or_remove;

        public enum AddOrRemove
        { 
            Add,
            Remove
        }

        public BackgroundWorker()
        {
            RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (node != null)
            {
                node[0].Nodes.Add(node[1]);
                node[0].Nodes.Remove(node[1]);
            }
            
        }
        public bool HasScheduledWork
        {
            get
            {
                return node != null;
            }
        }

        public void Schedule(TreeNode[] NodeToAdd, AddOrRemove add_or_remove)
        {
            this.node = NodeToAdd;
            this.add_or_remove = add_or_remove;
        }
        

        public void Reset()
        {
            node = null;
        }
    }
}
