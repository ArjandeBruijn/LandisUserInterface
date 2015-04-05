using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public partial class FrmRelable : Form
    {
        public string[] NewLabels { get; private set; }
        string[] OldLabels;
 
        System.Drawing.Point DesiredStartLocation;

        public bool ImplementForAllGraphs
        {
            get
            {
                return this.checkBox1.Checked;
            }
        }
        
        public FrmRelable(System.Drawing.Point p, string[] OldTerms, bool HideReplaceInAll)
        {
            this.DesiredStartLocation = p;

            InitializeComponent();

            this.ShowInTaskbar = false;

            if (HideReplaceInAll) this.checkBox1.Visible = false;

            this.OldLabels = OldTerms;
            this.NewLabels = OldTerms;

            foreach (string term in OldLabels)
            {
                this.richTextBox1.Text += term + '\n';
            }
        }
 
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string[] content = this.richTextBox1.Text.Split(new char[] {'\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int term = 0; term < content.Length; term++)
            {
                if (term >= OldLabels.Length)
                {
                    this.richTextBox1.Text = this.richTextBox1.Text.Replace(content[term], "");
                    return;
                }
                NewLabels[term] = content[term];
                

            }
        }

        private void FrmRelableGraph_Load(object sender, EventArgs e)
        {
             this.SetDesktopLocation(DesiredStartLocation.X, DesiredStartLocation.Y);
        }

        

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==  Keys.Return)
            {
                this.Close();
            }
        }

         
        

       
     
       

       
    }
}