namespace LandisUserInterface
{
    partial class FrmGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGrid));
            this.reoGridControl1 = new unvell.ReoGrid.ReoGridControl();
            this.SuspendLayout();
            // 
            // reoGridControl1
            // 
            this.reoGridControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.reoGridControl1.CellContextMenuStrip = null;
            this.reoGridControl1.ColCount = 100;
            this.reoGridControl1.ColHeadContextMenuStrip = null;
            this.reoGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reoGridControl1.Location = new System.Drawing.Point(0, 0);
            this.reoGridControl1.Name = "reoGridControl1";
            this.reoGridControl1.RowCount = 200;
            this.reoGridControl1.RowHeadContextMenuStrip = null;
            this.reoGridControl1.Script = null;
            this.reoGridControl1.Size = new System.Drawing.Size(538, 392);
            this.reoGridControl1.TabIndex = 0;
            this.reoGridControl1.Text = "reoGridControl1";
            // 
            // FrmGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 392);
            this.Controls.Add(this.reoGridControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGrid";
            this.ResumeLayout(false);

        }

        #endregion

        private unvell.ReoGrid.ReoGridControl reoGridControl1;
    }
}