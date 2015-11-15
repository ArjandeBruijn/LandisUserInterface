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
        

        private DateTime FileCreationTime;

        bool ClearRichtTextBox = true;

        bool TextChanged = false;

        public bool ClosePending = false;

        public FrmTXTDisplay(string FileName)
        {
            InitializeComponent();
            this.FileName = this.Text = FileName;
            
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

            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);

            if (IsEditedInExternalEditor)
            {
                try
                {
                    TextToAppend.AddRange(System.IO.File.ReadAllLines(FileName));
                    ClearRichtTextBox = true;
                }
                catch
                { 
                
                }
            }
        }
       
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ClearRichtTextBox) this.richTextBox1.Text = "";
            ClearRichtTextBox = false;

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

                
            }
            FileCreationTime = System.IO.File.GetLastWriteTime(FileName);

            this.toolStripProgressBar1.Value = 0;


            // If text changed in the text editor
            if (TextChanged)
            {
                TextChanged = false;
                try
                {
                    System.IO.File.Delete(FileName);

                    StreamWriter sw = new StreamWriter(FileName);

                    foreach (string s in richTextBox1.Text.Split('\n'))
                    {
                        sw.WriteLine(s);
                    }
                    sw.Close();

                    FileCreationTime = System.IO.File.GetLastWriteTime(FileName);
                }
                catch (System.Exception error)
                {
                    toolStripStatusLabel1.Text = "Could not write content to " + FileName + " " + error.Message;
                }
            }


            if (ClosePending) this.Close();
            else backgroundWorker1.RunWorkerAsync();
        }

        bool IsEditedInExternalEditor
        {
            get
            {
                return FileCreationTime.Equals(System.IO.File.GetLastWriteTime(FileName)) == false;
            }
        }

        
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextChanged = true;
        }
     
        

         
    }
}
