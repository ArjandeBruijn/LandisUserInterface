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
    public partial class FrmTXTDisplay : Form
    {
        bool TextChangedFlag = false;
        string FileName;
        public FrmTXTDisplay(string FileName)
        {
            InitializeComponent();
            this.FileName = this.Text = FileName;
            timer1.Interval = 500;
            timer1.Start();
        }
         
        public void AppendText(string[] text)
        {
            foreach (string line in text)
            {
                AppendText(line);
            }
        }
        
        public void AppendText(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => AppendText(text)));
            }
            else
            {
                richTextBox1.AppendText(text + "\n");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TextChangedFlag == false) return;

            string temp_file = System.IO.Path.ChangeExtension(FileName, "~temp.txt");

            try
            {
                System.IO.File.Move(FileName, temp_file);

                System.IO.File.WriteAllLines(FileName, new string[] { richTextBox1.Text });

                System.IO.File.Delete(temp_file);

                TextChangedFlag = false;
            }
            catch
            {
                System.IO.File.Move(temp_file, FileName);
            }
        }
        

        private void FrmTXTDisplay_TextChanged(object sender, EventArgs e)
        {
            TextChangedFlag = true;
        }

         
    }
}
