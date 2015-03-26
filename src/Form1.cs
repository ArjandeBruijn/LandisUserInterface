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

            this.treeView1.Font = new Font("Times New Roman", 14);

            this.treeView1.Nodes.Add("Scenario Files");
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dockContainer1_Load(object sender, EventArgs e)
        {

        }
         
        void AddScenarioFile(object sender, MouseEventArgs e)
        { 
        
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(this.treeView1, e.Location);
               
            }
        }

        private void AddScnFl_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Select Scenario File";

            if(of.ShowDialog() == DialogResult.OK)
            {
                treeView1.Nodes[0].Nodes.Add(of.FileName);
                treeView1.Nodes[0].ExpandAll();
            }
        }

        private void RmvScnFl_Click(object sender, EventArgs e)
        {

        }
    }
}
