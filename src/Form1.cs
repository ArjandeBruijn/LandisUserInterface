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
        private TreeNode HeaderScenarioFiles;

        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            this.treeView1.Font = new Font("Times New Roman", 14);
            this.treeView1.ShowNodeToolTips = true;

            HeaderScenarioFiles = new TreeNode("Scenario Files");
            this.treeView1.Nodes.Add(HeaderScenarioFiles);
            HeaderScenarioFiles.ExpandAll();

            timer1.Start();
        }
        string LastScenarioFileNames
        {
            get
            {
                return Properties.Settings.Default.LastScenarioFileNames;
            }
        }
        string[] LastScenarioPathList
        {
            get
            {
                return LastScenarioFileNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        void SetLastScenarioFileNames(List<string> Paths)
        {
            Properties.Settings.Default.LastScenarioFileNames = string.Join(";", Paths.Select(x => x).Distinct().ToArray());

            Properties.Settings.Default.Save();
        }
        private bool IsScenarioFile(string path)
        {
            // LandisData  Scenario
            if (path.Length > 0)
            {
                foreach (string line in System.IO.File.ReadAllLines(path))
                {
                    if (line.Contains("LandisData") && line.Contains("Scenario"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void AddLastScenarioFileName(string path)
        {
            List<string> lastscenariofilenames = new List<string>(LastScenarioPathList);

            lastscenariofilenames.Add(path);

            SetLastScenarioFileNames(lastscenariofilenames);
        }
        private void RemoveLastScenarioFileNames(string FileName)
        {
            List<string> lastscenariofilenames = new List<string>();
            foreach (string path in LastScenarioPathList)
            {
                if (System.IO.Path.GetFileName(path) != FileName)
                {
                    lastscenariofilenames.Add(path);
                }
            }
            if (Properties.Settings.Default.LastScenarioFileNames.Contains(FileName + ";"))
            { 
                Properties.Settings.Default.LastScenarioFileNames.Replace(FileName + ";", "");
            }
            Properties.Settings.Default.Save();
        }
        
                
        private void Form1_Load(object sender, EventArgs e)
        {
            if (LastScenarioFileNames != null && LastScenarioFileNames.Length > 0)
            {
                foreach (string FileName in LastScenarioPathList)
                {
                    AddScenarioFile(FileName);
                }
            }
        }

        
        void AddScenarioFile(string path)
        {
            if (IsScenarioFile(path))
            {
                TreeNode node = new TreeNode(System.IO.Path.GetFileName(path));

                node.ToolTipText = path;

                HeaderScenarioFiles.Nodes.Add(node);
                HeaderScenarioFiles.ExpandAll();
                AddLastScenarioFileName(path);
            }
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.Location);

                if (treeView1.SelectedNode == HeaderScenarioFiles)
                {
                    AddRemoveScenarioFile.Show(this.treeView1, e.Location);
                }
                else if (IsScenarioFile(treeView1.SelectedNode.ToolTipText))
                { 
                
                }

               
            }
        }

        private void AddScnFl_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Select Scenario File";

            if(of.ShowDialog() == DialogResult.OK)
            {
                AddScenarioFile(of.FileName);
            }
        }

        private void RmvScnFl_Click(object sender, EventArgs e)
        {
            string FileName ="";

            RemoveLastScenarioFileNames(FileName);
        }
       
        List<string> SubNodeTexts(TreeNodeCollection node_collection)
        {
            List<string> SubNodeTexts = new List<string>();
            foreach (TreeNode node in node_collection)
            {
                SubNodeTexts.Add(node.Text);
            }
            return SubNodeTexts;
        } 
         
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // List all files that should be in the interface
            foreach (TreeNode scenario_nodes in HeaderScenarioFiles.Nodes)
            {
                string path = scenario_nodes.ToolTipText;

                System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(path));

                foreach (string line in System.IO.File.ReadAllLines(path))
                {
                    string _line = line;
                    if (_line.Contains("<<"))
                    {
                        _line = _line.Remove(line.IndexOf("<<"));
                    }

                    string[] terms = _line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string term in terms)
                    {

                        if (System.IO.File.Exists(term) && SubNodeTexts(scenario_nodes.Nodes).Contains(term) == false)
                        {
                            scenario_nodes.Nodes.Add(term);
                        }
                    }

                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
    }
}
