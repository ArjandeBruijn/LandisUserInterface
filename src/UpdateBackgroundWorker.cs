using System.Windows.Forms;


namespace LandisUserInterface
{
    class UpdateBackgroundWorker : System.ComponentModel.BackgroundWorker
    {
        TreeNode[] node =null;
        AddOrRemove add_or_remove;

        public enum AddOrRemove
        { 
            Add,
            Remove
        }

        public UpdateBackgroundWorker()
        {
            RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (node != null)
            {
                if (add_or_remove == AddOrRemove.Add) node[0].Nodes.Add(node[1]);
                else node[0].Nodes.Remove(node[1]);
            }
            Reset();
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
        

        void Reset()
        {
            node = null;
        }
    }
}
