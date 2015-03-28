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
        List<string> TextToAppend = new List<string>();

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
            TextToAppend.AddRange(text);
            
        }
        
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Write cedits to file
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

            if (this.backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
        

        private void FrmTXTDisplay_TextChanged(object sender, EventArgs e)
        {
            TextChangedFlag = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int line = 0; line < TextToAppend.Count;line++ )
            {
                richTextBox1.AppendText(TextToAppend[line] + "\n");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();

                this.toolStripProgressBar1.Value = (int) (100 * ((float)line / (float)TextToAppend.Count));
            }
            this.toolStripProgressBar1.Value = 0;
        }

         
    }
}
