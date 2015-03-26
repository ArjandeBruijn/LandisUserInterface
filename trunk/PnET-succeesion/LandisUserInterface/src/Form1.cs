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
    public partial class Form1 : Form
    {
        public Form1()
        {
           
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;


            this.treeView1.Font = new Font("Times New Roman", 20);
            this.treeView1.Nodes.Add("bfsdjkfnsdodfjslfmsdlm");
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
