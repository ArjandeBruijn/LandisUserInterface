using System.Windows.Forms;


namespace LandisUserInterface
{
    class BackgroundWorker : System.ComponentModel.BackgroundWorker
    {
        TreeNode[] nodetoadd;
        TreeNode[] nodetoremove;

        public BackgroundWorker()
        {
            RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }
        

        public TreeNode[] NodeToAdd
        {
            get
            {
                return nodetoadd;
            }
        }
        public TreeNode[] NodeToRemove
        {
            get
            {
                return nodetoremove;
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (NodeToAdd != null)
            {
                NodeToAdd[0].Nodes.Add(NodeToAdd[1]);
            }
            if (NodeToRemove != null)
            {
                NodeToRemove[0].Nodes.Remove(NodeToRemove[1]);
            }
        }
        public bool HasScheduledWork
        {
            get
            {
                return nodetoadd != null || nodetoremove != null;
            }
        }

        public void ScheduleNodeAddition(TreeNode[] NodeToAdd)
        {
            this.nodetoadd = NodeToAdd;
        }
        public void ScheduleNodeRemoval(TreeNode[] NodeToRemove)
        {
            this.nodetoremove = NodeToRemove;
        }

        public void Reset()
        {
            nodetoadd = nodetoremove = null;
        }
    }
}
