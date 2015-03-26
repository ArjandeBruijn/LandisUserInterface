﻿namespace LandisUserInterface
{
    partial class FrmMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMap));
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.tbZoomIn = new System.Windows.Forms.ToolBarButton();
            this.tbZoomOut = new System.Windows.Forms.ToolBarButton();
            this.tbFullExtents = new System.Windows.Forms.ToolBarButton();
            this.tbPan = new System.Windows.Forms.ToolBarButton();
            this.tbInfo = new System.Windows.Forms.ToolBarButton();
            this.tbAnimation = new System.Windows.Forms.ToolBarButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.treeViewLayers = new System.Windows.Forms.TreeView();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.axMap1 = new AxMapWinGIS.AxMap();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbZoomIn,
            this.tbZoomOut,
            this.tbFullExtents,
            this.tbPan,
            this.tbInfo,
            this.tbAnimation});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList2;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(656, 28);
            this.toolBar1.TabIndex = 33;
            // 
            // tbZoomIn
            // 
            this.tbZoomIn.ImageIndex = 4;
            this.tbZoomIn.Name = "tbZoomIn";
            this.tbZoomIn.Tag = "ZoomIn";
            this.tbZoomIn.ToolTipText = "Zoom In";
            // 
            // tbZoomOut
            // 
            this.tbZoomOut.ImageIndex = 5;
            this.tbZoomOut.Name = "tbZoomOut";
            this.tbZoomOut.Tag = "ZoomOut";
            this.tbZoomOut.ToolTipText = "Zoom Out";
            // 
            // tbFullExtents
            // 
            this.tbFullExtents.ImageIndex = 3;
            this.tbFullExtents.Name = "tbFullExtents";
            this.tbFullExtents.Tag = "FullExtents";
            this.tbFullExtents.ToolTipText = "Zoom to full extents";
            // 
            // tbPan
            // 
            this.tbPan.ImageIndex = 6;
            this.tbPan.Name = "tbPan";
            this.tbPan.Tag = "Pan";
            this.tbPan.ToolTipText = "Pan";
            // 
            // tbInfo
            // 
            this.tbInfo.ImageIndex = 17;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.Tag = "Info";
            // 
            // tbAnimation
            // 
            this.tbAnimation.ImageKey = "stopwatch2.ico";
            this.tbAnimation.Name = "tbAnimation";
            this.tbAnimation.Tag = "Animation";
            this.tbAnimation.ToolTipText = "Animate map folder";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            this.imageList2.Images.SetKeyName(2, "");
            this.imageList2.Images.SetKeyName(3, "");
            this.imageList2.Images.SetKeyName(4, "");
            this.imageList2.Images.SetKeyName(5, "");
            this.imageList2.Images.SetKeyName(6, "");
            this.imageList2.Images.SetKeyName(7, "");
            this.imageList2.Images.SetKeyName(8, "");
            this.imageList2.Images.SetKeyName(9, "");
            this.imageList2.Images.SetKeyName(10, "");
            this.imageList2.Images.SetKeyName(11, "bw-running.ico");
            this.imageList2.Images.SetKeyName(12, "text-plain.ico");
            this.imageList2.Images.SetKeyName(13, "browse.ico");
            this.imageList2.Images.SetKeyName(14, "stopwatch2.ico");
            this.imageList2.Images.SetKeyName(15, "Rerun.png");
            this.imageList2.Images.SetKeyName(16, "refresh.ico");
            this.imageList2.Images.SetKeyName(17, "info.ico");
            // 
            // treeViewLayers
            // 
            this.treeViewLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewLayers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewLayers.CheckBoxes = true;
            this.treeViewLayers.Location = new System.Drawing.Point(0, 28);
            this.treeViewLayers.Name = "treeViewLayers";
            this.treeViewLayers.ShowNodeToolTips = true;
            this.treeViewLayers.Size = new System.Drawing.Size(166, 380);
            this.treeViewLayers.TabIndex = 38;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(490, 28);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(166, 380);
            this.treeView1.TabIndex = 39;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 411);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(656, 22);
            this.statusStrip1.TabIndex = 40;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // axMap1
            // 
            this.axMap1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axMap1.Enabled = true;
            this.axMap1.Location = new System.Drawing.Point(172, 28);
            this.axMap1.Name = "axMap1";
            this.axMap1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap1.OcxState")));
            this.axMap1.Size = new System.Drawing.Size(312, 380);
            this.axMap1.TabIndex = 41;
            // 
            // FrmMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(656, 433);
            this.Controls.Add(this.axMap1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.treeViewLayers);
            this.Controls.Add(this.toolBar1);
            this.Name = "FrmMap";
            this.Text = "FrmMap";
            this.Load += new System.EventHandler(this.FrmMap_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.ToolBarButton tbZoomIn;
        private System.Windows.Forms.ToolBarButton tbZoomOut;
        private System.Windows.Forms.ToolBarButton tbFullExtents;
        private System.Windows.Forms.ToolBarButton tbPan;
        private System.Windows.Forms.ToolBarButton tbInfo;
        private System.Windows.Forms.ToolBarButton tbAnimation;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.TreeView treeViewLayers;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private AxMapWinGIS.AxMap axMap1;
    }
}