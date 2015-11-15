using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public partial class FrmSelectProgram : Form
    {
        Options selection;

        private System.Drawing.Point DesiredStartLocation;

        public Options Selection
        {
            get
            {
                return selection;
            }
        }

        public enum Options
        { 
            NoSelection,
            Excel,
            Zgraph,
            NotePad
        }
         
        public FrmSelectProgram(System.Drawing.Point p)
        {
            // here store the value for x & y into instance variables
            DesiredStartLocation = p;

            InitializeComponent();

            this.KeyPreview = true;
        }
        private void FrmSelectProgram_Load(object sender, EventArgs e)
        {
            this.SetDesktopLocation(DesiredStartLocation.X, DesiredStartLocation.Y);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            selection = Options.Excel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selection = Options.NotePad;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            selection = Options.Zgraph;
            this.Close();
        }

        private void FrmSelectProgram_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                Close();
            }
        }

        
    }
}
