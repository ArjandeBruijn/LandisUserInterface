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
        }
        string LastScenarioFileNames
        {
            get
            {
                return Properties.Settings.Default.LastScenarioFileNames;
            }
        }
        string[] LastScenarioFileNamesList
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
            foreach (string line in System.IO.File.ReadAllLines(path))
            {
                if (line.Contains("LandisData") && line.Contains("Scenario"))
                {
                    return true;
                }
            }
            return false;
        }
        private void AddLastScenarioFileName(string path)
        {
            List<string> lastscenariofilenames = new List<string>(LastScenarioFileNamesList);

            lastscenariofilenames.Add(path);

            SetLastScenarioFileNames(lastscenariofilenames);
        }
        private void RemoveLastScenarioFileNames(string FileName)
        {
            List<string> lastscenariofilenames = new List<string>();
            foreach (string path in LastScenarioFileNamesList)
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
                foreach (string FileName in LastScenarioFileNamesList)
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
                contextMenuStrip1.Show(this.treeView1, e.Location);
               
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
    }
}
