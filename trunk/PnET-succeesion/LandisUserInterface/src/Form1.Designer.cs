namespace LandisUserInterface
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.FileFolderIcons = new System.Windows.Forms.ImageList(this.components);
            this.dockContainer1 = new Crom.Controls.Docking.DockContainer();
            this.ProjectOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddScnFl = new System.Windows.Forms.ToolStripMenuItem();
            this.RmvScnFl = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ScenarioOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RunSim = new System.Windows.Forms.ToolStripMenuItem();
            this.Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowFolderLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowFileLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.updateoutputbackgroundworker = new LandisUserInterface.UpdateBackgroundWorker();
            this.updateInputBackGroundWorker = new LandisUserInterface.UpdateBackgroundWorker();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ProjectOptions.SuspendLayout();
            this.ScenarioOptions.SuspendLayout();
            this.FolderOptions.SuspendLayout();
            this.FileOptions.SuspendLayout();
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
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
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
            // ProjectOptions
            // 
            this.ProjectOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddScnFl,
            this.RmvScnFl});
            this.ProjectOptions.Name = "contextMenuStrip1";
            this.ProjectOptions.Size = new System.Drawing.Size(166, 48);
            // 
            // AddScnFl
            // 
            this.AddScnFl.Name = "AddScnFl";
            this.AddScnFl.Size = new System.Drawing.Size(165, 22);
            this.AddScnFl.Text = "Add Scenario File";
            this.AddScnFl.Click += new System.EventHandler(this.AddScnFl_Click);
            // 
            // RmvScnFl
            // 
            this.RmvScnFl.Name = "RmvScnFl";
            this.RmvScnFl.Size = new System.Drawing.Size(165, 22);
            this.RmvScnFl.Text = "Clear";
            this.RmvScnFl.Click += new System.EventHandler(this.RmvScnFl_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ScenarioOptions
            // 
            this.ScenarioOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunSim,
            this.Remove});
            this.ScenarioOptions.Name = "contextMenuStrip1";
            this.ScenarioOptions.Size = new System.Drawing.Size(206, 48);
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
            // FolderOptions
            // 
            this.FolderOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowFolderLocation});
            this.FolderOptions.Name = "contextMenuStrip1";
            this.FolderOptions.Size = new System.Drawing.Size(184, 26);
            // 
            // ShowFolderLocation
            // 
            this.ShowFolderLocation.Name = "ShowFolderLocation";
            this.ShowFolderLocation.Size = new System.Drawing.Size(183, 22);
            this.ShowFolderLocation.Text = "Show folder location";
            this.ShowFolderLocation.Click += new System.EventHandler(this.ShowFolderLocation_Click);
            // 
            // FileOptions
            // 
            this.FileOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowFileLocation});
            this.FileOptions.Name = "contextMenuStrip1";
            this.FileOptions.Size = new System.Drawing.Size(169, 26);
            // 
            // ShowFileLocation
            // 
            this.ShowFileLocation.Name = "ShowFileLocation";
            this.ShowFileLocation.Size = new System.Drawing.Size(168, 22);
            this.ShowFileLocation.Text = "Show file location";
            this.ShowFileLocation.Click += new System.EventHandler(this.ShowFileLocation_Click);
            // 
            // updateoutputbackgroundworker
            // 
            this.updateoutputbackgroundworker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // updateInputBackGroundWorker
            // 
            this.updateInputBackGroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateInputBackGroundWorker_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ProjectOptions.ResumeLayout(false);
            this.ScenarioOptions.ResumeLayout(false);
            this.FolderOptions.ResumeLayout(false);
            this.FileOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private Crom.Controls.Docking.DockContainer dockContainer1;
        private System.Windows.Forms.ContextMenuStrip ProjectOptions;
        private System.Windows.Forms.ToolStripMenuItem AddScnFl;
        private System.Windows.Forms.ToolStripMenuItem RmvScnFl;
        private UpdateBackgroundWorker updateoutputbackgroundworker;
        private UpdateBackgroundWorker updateInputBackGroundWorker;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip ScenarioOptions;
        private System.Windows.Forms.ToolStripMenuItem RunSim;
        private System.Windows.Forms.ImageList FileFolderIcons;
        private System.Windows.Forms.ToolStripMenuItem Remove;
        private System.Windows.Forms.ContextMenuStrip FolderOptions;
        private System.Windows.Forms.ToolStripMenuItem ShowFolderLocation;
        private System.Windows.Forms.ContextMenuStrip FileOptions;
        private System.Windows.Forms.ToolStripMenuItem ShowFileLocation;
        

    }
}

