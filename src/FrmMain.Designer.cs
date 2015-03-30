namespace LandisUserInterface
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.FileFolderIcons = new System.Windows.Forms.ImageList(this.components);
            this.dockContainer1 = new Crom.Controls.Docking.DockContainer();
            
           
             
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ScenarioOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RunSim = new System.Windows.Forms.ToolStripMenuItem();
            this.Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuOpenLocation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowFileOrFolderLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.updateoutputbackgroundworker = new LandisUserInterface.UpdateBackgroundWorker(UpdateBackgroundWorker.AddOrRemove.Add);
            this.updateInputBackGroundWorker = new LandisUserInterface.UpdateBackgroundWorker(UpdateBackgroundWorker.AddOrRemove.Add);
            this.updateBackgourndWorkerRemove = new LandisUserInterface.UpdateBackgroundWorker(UpdateBackgroundWorker.AddOrRemove.Remove);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
             
            this.ScenarioOptions.SuspendLayout();
            this.ContextMenuOpenLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dockContainer1);
            this.splitContainer1.Size = new System.Drawing.Size(284, 262);
            this.splitContainer1.SplitterDistance = 65;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.FileFolderIcons;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(61, 258);
            this.treeView1.TabIndex = 0;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeView1_NodeMouseHover);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.DragLeave += new System.EventHandler(this.treeView1_DragLeave);
            this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
            // 
            // FileFolderIcons
            // 
            this.FileFolderIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("FileFolderIcons.ImageStream")));
            this.FileFolderIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.FileFolderIcons.Images.SetKeyName(0, "File");
            this.FileFolderIcons.Images.SetKeyName(1, "Folder");
            this.FileFolderIcons.Images.SetKeyName(2, "RightArrow");
            // 
            // dockContainer1
            // 
            this.dockContainer1.AllowDrop = true;
            this.dockContainer1.BackColor = System.Drawing.Color.White;
            this.dockContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockContainer1.Location = new System.Drawing.Point(0, 0);
            this.dockContainer1.Name = "dockContainer1";
            this.dockContainer1.Size = new System.Drawing.Size(211, 258);
            this.dockContainer1.TabIndex = 1;
            this.dockContainer1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dockContainer1_DragDrop);
            this.dockContainer1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dockContainer1_DragEnter);
            
            
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            
            // 
            // RunSim
            // 
            this.RunSim.Name = "RunSim";
            this.RunSim.Size = new System.Drawing.Size(205, 22);
            this.RunSim.Text = "Run simulation";
            this.RunSim.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // Remove
            // 
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(205, 22);
            this.Remove.Text = "Remove from workspace";
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // ContextMenuOpenLocation
            // 
            this.ContextMenuOpenLocation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowFileOrFolderLocation});
            this.ContextMenuOpenLocation.Name = "contextMenuStrip1";
            this.ContextMenuOpenLocation.Size = new System.Drawing.Size(184, 26);
            // 
            // ShowFileOrFolderLocation
            // 
            this.ShowFileOrFolderLocation.Name = "ShowFileOrFolderLocation";
            this.ShowFileOrFolderLocation.Size = new System.Drawing.Size(183, 22);
            this.ShowFileOrFolderLocation.Text = "Show folder location";
            this.ShowFileOrFolderLocation.Click += new System.EventHandler(this.ShowFolderLocation_Click);
            // 
            // ScenarioOptions
            // 
            this.ScenarioOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunSim,
            this.Remove,
            this.ShowFileOrFolderLocation});
            this.ScenarioOptions.Name = "contextMenuStrip1";
            this.ScenarioOptions.Size = new System.Drawing.Size(206, 48);
            // 
            // updateoutputbackgroundworker
            // 
            this.updateoutputbackgroundworker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateoutputbackgroundworker_DoWork);
            // 
            // updateInputBackGroundWorker
            // 
            this.updateInputBackGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateInputBackGroundWorker_DoWork);
            // 
            // updateBackgourndWorkerRemove
            // 
            this.updateBackgourndWorkerRemove.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateBackgourndWorkerRemove_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
             
            this.ScenarioOptions.ResumeLayout(false);
            this.ContextMenuOpenLocation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private Crom.Controls.Docking.DockContainer dockContainer1;
         
         
         
        private UpdateBackgroundWorker updateoutputbackgroundworker;
        private UpdateBackgroundWorker updateInputBackGroundWorker;
        private UpdateBackgroundWorker updateBackgourndWorkerRemove;

        private System.Windows.Forms.Timer timer1;

         
        private System.Windows.Forms.ImageList FileFolderIcons;
        
        private System.Windows.Forms.ToolStripMenuItem RunSim;
        private System.Windows.Forms.ToolStripMenuItem Remove;
        private System.Windows.Forms.ToolStripMenuItem ShowFileOrFolderLocation;
        private System.Windows.Forms.ContextMenuStrip ContextMenuOpenLocation;
        private System.Windows.Forms.ContextMenuStrip ScenarioOptions;

    }
}

