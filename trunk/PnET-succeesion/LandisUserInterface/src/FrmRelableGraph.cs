using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public partial class FrmRelableGraph : Form
    {
        ZedGraph.CurveList curve_list;

        public delegate void UpdateGraph();

        UpdateGraph update_graph;

        System.Drawing.Point DesiredStartLocation;
        
        public FrmRelableGraph(System.Drawing.Point p, ZedGraph.CurveList curve_list, UpdateGraph update_graph)
        {
            this.DesiredStartLocation = p;

           

            InitializeComponent();

             
            this.update_graph += update_graph;

            this.curve_list = curve_list;

            FillTextBox();
        }

        private void FillTextBox()
        {
            foreach (ZedGraph.CurveItem curve in curve_list)
            {
                this.richTextBox1.Text += curve.Label.Text + '\t';
            }
            
            
        }

        
       
 
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string[] content = this.richTextBox1.Text.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int term = 0; term < content.Length; term++)
            {
                if (term >= curve_list.Count)
                {
                    this.richTextBox1.Text = this.richTextBox1.Text.Replace(content[term], "");
                    return;
                }
                curve_list[term].Label.Text = content[term];
                update_graph();

            }
        }

        private void FrmRelableGraph_Load(object sender, EventArgs e)
        {
             this.SetDesktopLocation(DesiredStartLocation.X, DesiredStartLocation.Y);
        }

         
        

       
     
       

       
    }
}