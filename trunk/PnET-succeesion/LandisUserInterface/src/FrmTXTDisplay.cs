using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LandisUserInterface
{
    public partial class FrmTXTDisplay : Form
    {
        List<string> TextToAppend = new List<string>();

        string FileName;
         
        string LastTextBoxTextOnFile = null;
        private DateTime FileCreationTime;

        

        public FrmTXTDisplay(string FileName)
        {
            InitializeComponent();
            this.FileName = this.Text = FileName;
            timer1.Interval = 500;
            timer1.Start();

            
            try
            {
                string[] Content = System.IO.File.ReadAllLines(FileName);
                TextToAppend.AddRange(Content);

                FileCreationTime = System.IO.File.GetLastWriteTime(FileName);
            }
            catch(System.Exception e)
            {
                TextToAppend.AddRange(new string[]{e.Message}); 
            }

            
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (this.backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int count = 0;
            float InitialLength = (float)TextToAppend.Count();
            while (TextToAppend.Count() > 0)
            {
                richTextBox1.AppendText(TextToAppend[0] + "\n");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();

                this.toolStripProgressBar1.Value = (int)(100 * ((float)count / InitialLength));
                count++;
                TextToAppend.RemoveAt(0);

                // Distinguish used changes
                LastTextBoxTextOnFile = richTextBox1.Text;
            }

            try
            {
                System.IO.File.OpenRead(FileName);
            }
            catch
            {
                return;
            }
            if (System.IO.File.Exists(FileName) == false)
            {
                return;
            }

            this.toolStripProgressBar1.Value = 0;


            // If text changed in the text editor
            if (richTextBox1.Text != LastTextBoxTextOnFile)
            {
                try
                {
                    System.IO.File.Delete(FileName);

                    StreamWriter sw = new StreamWriter(FileName);

                    foreach (string s in richTextBox1.Text.Split('\n'))
                    {
                        sw.WriteLine(s);
                    }
                    sw.Close();

                    LastTextBoxTextOnFile = richTextBox1.Text;

                    FileCreationTime = System.IO.File.GetLastWriteTime(FileName);
                }
                catch (System.Exception error)
                {
                    toolStripStatusLabel1.Text = "Could not write content to " + FileName + " " + error.Message;
                }
            }

            // If files was changed in external editor
            if (FileCreationTime != System.IO.File.GetLastWriteTime(FileName))
            {
                 
               richTextBox1.Text ="";
               TextToAppend.AddRange(System.IO.File.ReadAllLines(FileName));
            }
           
        }
        

     
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           

 


        }

         
    }
}
