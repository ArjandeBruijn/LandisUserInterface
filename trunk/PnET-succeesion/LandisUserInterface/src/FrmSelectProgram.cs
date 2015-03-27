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
        private int desiredStartLocationX;
        private int desiredStartLocationY;

        public FrmSelectProgram(int x, int y)
        {
            // here store the value for x & y into instance variables
            this.desiredStartLocationX = x;
            this.desiredStartLocationY = y;

            InitializeComponent();

            
        }
        private void FrmSelectProgram_Load(object sender, EventArgs e)
        {
            this.SetDesktopLocation(desiredStartLocationX, desiredStartLocationY);
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

        
    }
}
