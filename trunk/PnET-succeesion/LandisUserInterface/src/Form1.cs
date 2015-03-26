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
            HeaderScenarioFiles.SelectedImageKey =  HeaderScenarioFiles.ImageKey = "RightArrow";
            this.treeView1.Nodes.Add(HeaderScenarioFiles);
            HeaderScenarioFiles.ExpandAll();

            timer1.Start();
        }
        string LandisConsoleExe
        {
            get
            {
                return Properties.Settings.Default.LandisConsoleExe;
            }
            set
            {
                Properties.Settings.Default.LandisConsoleExe = value;
                Properties.Settings.Default.Save();
            }
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
                TreeNode node = new TreeNode();
                node.Text = node.Name = System.IO.Path.GetFileName(path);
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
                    ScenarioFileOptions.Show(this.treeView1, e.Location);
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
        void UpdateFolderNode(TreeNode parent)
        {
            string Folder = parent.ToolTipText;

            if (System.IO.Directory.Exists(Folder) == false)
            {
                return;
            }

            foreach (string file in System.IO.Directory.GetFiles(Folder))
            {
                if (parent.Nodes[System.IO.Path.GetFileName(file)] == null)
                {
                    TreeNode child = new TreeNode();
                    child.Name = child.Text = System.IO.Path.GetFileName(file);
                    child.ImageKey  = "File";
                    parent.Nodes.Add(child);

                }
            }
           

            foreach (string subfolder in System.IO.Directory.GetDirectories(Folder))
            {
                if (parent.Nodes[subfolder.Split(System.IO.Path.DirectorySeparatorChar).Last()] == null)
                {
                    TreeNode child = new TreeNode();
                    child.Name = child.Text = subfolder.Split(System.IO.Path.DirectorySeparatorChar).Last();
                    child.ToolTipText = subfolder;
                    child.ImageKey = "Folder";
                    parent.Nodes.Add(child);
                    
                }
                UpdateFolderNode(parent.Nodes[subfolder.Split(System.IO.Path.DirectorySeparatorChar).Last()]);
            }
             
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // List all files that should be in the interface
            foreach (TreeNode scenario_node in HeaderScenarioFiles.Nodes)
            {
                string path = scenario_node.ToolTipText;

                try
                {
                    System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(path));
                }
                catch
                {
                    double t = 0.0;
                }

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

                        if (System.IO.File.Exists(term) && SubNodeTexts(scenario_node.Nodes).Contains(term) == false)
                        {
                            scenario_node.Nodes.Add(term);
                        }
                    }
                }
                if (System.IO.Directory.Exists("output"))
                {
                    if (scenario_node.Nodes["output"] == null)
                    {
                        TreeNode child = new TreeNode();
                        child.Text = child.Name = "output";
                        child.ToolTipText = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(scenario_node.ToolTipText), "output");
                        child.ImageKey = "Folder";
                        scenario_node.Nodes.Add(child);
                    }

                    UpdateFolderNode(scenario_node.Nodes["output"]);
                
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
        public void RunSimulation(string path)
        {
            if (System.IO.File.Exists(path) == false) throw new System.Exception("File " + path + " does not exist");

            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(path));

            Directory.DeleteDirectory("output");

            if (System.IO.File.Exists(path))
            {
                System.Diagnostics.Process simulation = new System.Diagnostics.Process();

                simulation.StartInfo.FileName = @"C:\Program Files\LANDIS-II\v6\bin\Landis.Console-6.0.exe";

                if (System.IO.File.Exists(simulation.StartInfo.FileName) == false)
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Title = "Select your landis console executable";
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        LandisConsoleExe = dlg.FileName;
                        RunSimulation(path);
                    }
                    else return;
                }

                simulation.StartInfo.Arguments = "\"" + path + "\"";

                simulation.Start();

                 
            }

        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.ToolTipText;

            RunSimulation(path);
        }
    }
}
