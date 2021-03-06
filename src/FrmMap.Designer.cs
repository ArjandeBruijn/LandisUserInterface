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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeViewLayers = new System.Windows.Forms.TreeView();
            this.TreeViewLegend = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.axMap1 = new AxMapWinGIS.AxMap();
            this.animation_timer = new System.Windows.Forms.Timer(this.components);
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
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(656, 28);
            this.toolBar1.TabIndex = 33;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // tbZoomIn
            // 
            this.tbZoomIn.ImageKey = "icon_zoom_in.png";
            this.tbZoomIn.Name = "tbZoomIn";
            this.tbZoomIn.Tag = "ZoomIn";
            this.tbZoomIn.ToolTipText = "Zoom In";
            // 
            // tbZoomOut
            // 
            this.tbZoomOut.ImageKey = "icon_zoom_out.png";
            this.tbZoomOut.Name = "tbZoomOut";
            this.tbZoomOut.Tag = "ZoomOut";
            this.tbZoomOut.ToolTipText = "Zoom Out";
            // 
            // tbFullExtents
            // 
            this.tbFullExtents.ImageKey = "icon_zoom_max_extents.png";
            this.tbFullExtents.Name = "tbFullExtents";
            this.tbFullExtents.Tag = "FullExtents";
            this.tbFullExtents.ToolTipText = "Zoom to full extents";
            // 
            // tbPan
            // 
            this.tbPan.ImageKey = "icon_pan.png";
            this.tbPan.Name = "tbPan";
            this.tbPan.Tag = "Pan";
            this.tbPan.ToolTipText = "Pan";
            // 
            // tbInfo
            // 
            this.tbInfo.ImageKey = "icon_identify.png";
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.Tag = "Info";
            this.tbInfo.ToolTipText = "Identify pixel value";
            // 
            // tbAnimation
            // 
            this.tbAnimation.ImageKey = "StopWatch.png";
            this.tbAnimation.Name = "tbAnimation";
            this.tbAnimation.Tag = "Animation";
            this.tbAnimation.ToolTipText = "Animate";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icon_identify.png");
            this.imageList1.Images.SetKeyName(1, "icon_pan.png");
            this.imageList1.Images.SetKeyName(2, "icon_zoom_in.png");
            this.imageList1.Images.SetKeyName(3, "icon_zoom_max_extents.png");
            this.imageList1.Images.SetKeyName(4, "icon_zoom_out.png");
            this.imageList1.Images.SetKeyName(5, "StopWatch.png");
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
            this.treeViewLayers.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLayers_AfterCheck);
            this.treeViewLayers.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeViewLayers_MouseHover);
            this.treeViewLayers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewLayers_MouseDown);
            // 
            // TreeViewLegend
            // 
            this.TreeViewLegend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewLegend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TreeViewLegend.Location = new System.Drawing.Point(490, 28);
            this.TreeViewLegend.Name = "TreeViewLegend";
            this.TreeViewLegend.ShowNodeToolTips = true;
            this.TreeViewLegend.Size = new System.Drawing.Size(166, 380);
            this.TreeViewLegend.TabIndex = 39;
            this.TreeViewLegend.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewLegend_AfterSelect);
            this.TreeViewLegend.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TreeViewLegend_MouseDoubleClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
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
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
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
            this.axMap1.MouseDownEvent += new AxMapWinGIS._DMapEvents_MouseDownEventHandler(this.axMap1_MouseDownEvent);
            // 
            // animation_timer
            // 
            this.animation_timer.Interval = 2000;
            this.animation_timer.Tick += new System.EventHandler(this.SelectNextLayer);
            // 
            // FrmMap
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(656, 433);
            this.Controls.Add(this.axMap1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.TreeViewLegend);
            this.Controls.Add(this.treeViewLayers);
            this.Controls.Add(this.toolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMap";
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmMap_DragEnter);
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
        private System.Windows.Forms.TreeView treeViewLayers;
        private System.Windows.Forms.TreeView TreeViewLegend;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private AxMapWinGIS.AxMap axMap1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer animation_timer;
    }
}