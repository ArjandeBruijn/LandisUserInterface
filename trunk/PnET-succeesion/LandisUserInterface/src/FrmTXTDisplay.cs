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
        string FileName;
        public FrmTXTDisplay()
        {
            InitializeComponent();
            this.Text = FileName;
            timer1.Interval = 500;
            timer1.Start();
        }
        public void SetFile(string FileName)
        {
            this.FileName = FileName;
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
            if (backgroundWorker1.IsBusy==false)
            {
                this.backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (FileName == null) return;
            try
            {
                string[] content = System.IO.File.ReadAllLines(FileName);

                foreach (string line in content)
                {
                    AppendText(line);
                }
                timer1.Stop();

            }
            catch
            { 
            
            }
        }
    }
}
