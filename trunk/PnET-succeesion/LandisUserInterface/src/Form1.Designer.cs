﻿namespace LandisUserInterface
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dockContainer1 = new Crom.Controls.Docking.DockContainer();
            this.AddRemoveScenarioFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddScnFl = new System.Windows.Forms.ToolStripMenuItem();
            this.RmvScnFl = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.AddRemoveScenarioFile.SuspendLayout();
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
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(61, 258);
            this.treeView1.TabIndex = 0;
            this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
            // 
            // dockContainer1
            // 
            this.dockContainer1.BackColor = System.Drawing.Color.White;
            this.dockContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockContainer1.Location = new System.Drawing.Point(0, 0);
            this.dockContainer1.Name = "dockContainer1";
            this.dockContainer1.Size = new System.Drawing.Size(211, 258);
            this.dockContainer1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.AddRemoveScenarioFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddScnFl,
            this.RmvScnFl});
            this.AddRemoveScenarioFile.Name = "contextMenuStrip1";
            this.AddRemoveScenarioFile.Size = new System.Drawing.Size(187, 48);
            // 
            // AddScnFl
            // 
            this.AddScnFl.Name = "AddScnFl";
            this.AddScnFl.Size = new System.Drawing.Size(186, 22);
            this.AddScnFl.Text = "Add Scenario File";
            this.AddScnFl.Click += new System.EventHandler(this.AddScnFl_Click);
            // 
            // RmvScnFl
            // 
            this.RmvScnFl.Name = "RmvScnFl";
            this.RmvScnFl.Size = new System.Drawing.Size(186, 22);
            this.RmvScnFl.Text = "Remove Scenario File";
            this.RmvScnFl.Click += new System.EventHandler(this.RmvScnFl_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            this.AddRemoveScenarioFile.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private Crom.Controls.Docking.DockContainer dockContainer1;
        private System.Windows.Forms.ContextMenuStrip AddRemoveScenarioFile;
        private System.Windows.Forms.ToolStripMenuItem AddScnFl;
        private System.Windows.Forms.ToolStripMenuItem RmvScnFl;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;

    }
}

