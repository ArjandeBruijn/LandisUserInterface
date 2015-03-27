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

            this.treeView1.AllowDrop = true;
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
            if (path.Length > 0 && System.IO.File.Exists(path))
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
                HeaderScenarioFiles.Expand();
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
                    ProjectOptions.Show(this.treeView1, e.Location);
                }
                else if (IsScenarioFile(treeView1.SelectedNode.ToolTipText))
                {
                    ScenarioOptions.Show(this.treeView1, e.Location);
                }
                else if (System.IO.File.Exists(treeView1.SelectedNode.ToolTipText))
                {
                    FileOptions.Show(this.treeView1, e.Location);
                }
                else if (System.IO.Directory.Exists(treeView1.SelectedNode.ToolTipText))
                {
                    FolderOptions.Show(this.treeView1, e.Location);
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
            this.HeaderScenarioFiles.Nodes.Clear();
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


        void RemoveOldNodes(TreeNode parent)
        {
            if (System.IO.Directory.Exists(parent.ToolTipText))
            {
                foreach (TreeNode child in parent.Nodes)
                {
                    if (System.IO.File.Exists(child.ToolTipText) == false && System.IO.Directory.Exists(child.ToolTipText) == false)
                    {
                        this.updateBackgourndWorkerRemove.Schedule(new TreeNode[] { parent, child });
                    }
                    RemoveOldNodes(child);
                }

            }

        }
        void AddNewNodes(TreeNode parent)
        {
            string path = parent.ToolTipText;

            if (System.IO.Directory.Exists(path))
            {
                foreach (string file_path in System.IO.Directory.GetFiles(path))
                {
                    if (parent.Nodes[System.IO.Path.GetFileName(file_path)] == null)
                    {
                        TreeNode child = new TreeNode();
                        child.Name = child.Text = System.IO.Path.GetFileName(file_path);
                        child.ToolTipText = file_path;
                        child.ImageKey = child.SelectedImageKey = "File";
                        this.updateoutputbackgroundworker.Schedule(new TreeNode[] { parent, child } );
                    }
                }
                foreach (string subfolder in System.IO.Directory.GetDirectories(path))
                {
                    if (parent.Nodes[subfolder.Split(System.IO.Path.DirectorySeparatorChar).Last()] == null)
                    {
                        TreeNode child = new TreeNode();
                        child.Name = child.Text = subfolder.Split(System.IO.Path.DirectorySeparatorChar).Last();
                        child.ToolTipText = subfolder;
                        child.ImageKey = child.SelectedImageKey = "Folder";
                        updateoutputbackgroundworker.Schedule(new TreeNode[] { parent, child } );
                    }
                    else AddNewNodes(parent.Nodes[subfolder.Split(System.IO.Path.DirectorySeparatorChar).Last()]);
                }
                
            }
            
        }
        private void updateInputBackGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (TreeNode scenario_node in HeaderScenarioFiles.Nodes)
            {
                string path_scenario_file = scenario_node.ToolTipText;

                foreach (string line in System.IO.File.ReadAllLines(path_scenario_file))
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
                            TreeNode node = new TreeNode();
                            node.ToolTipText = node.Text = node.Name = term;

                            if (term.Contains(System.IO.Directory.GetCurrentDirectory()) == false)
                            {
                                node.ToolTipText = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), term);
                            }
                            if (scenario_node.Nodes[node.Name] == null)
                            {
                                updateInputBackGroundWorker.Schedule(new TreeNode[] { scenario_node, node } );
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void updateoutputbackgroundworker_DoWork(object sender, DoWorkEventArgs e)
        {
            // List all files that should be in the interface
            foreach (TreeNode scenario_node in HeaderScenarioFiles.Nodes)
            {
                string path_scenario_file = scenario_node.ToolTipText;

                System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(path_scenario_file));

                
                if (System.IO.Directory.Exists("output"))
                {
                    if (scenario_node.Nodes["output"] == null)
                    {
                        TreeNode child = new TreeNode();
                        child.Text = child.Name = "output";
                        child.ToolTipText = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(scenario_node.ToolTipText), "output");
                        child.ImageKey = child.SelectedImageKey = "Folder";

                        updateoutputbackgroundworker.Schedule(new TreeNode[] { scenario_node, child } );
                        return; 
                    }
                    AddNewNodes(scenario_node.Nodes["output"]);
                }
                

            }
        }
        
        private void updateBackgourndWorkerRemove_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (TreeNode scenario_node in HeaderScenarioFiles.Nodes)
            {
                if (scenario_node.Nodes["output"]!= null)
                {
                    RemoveOldNodes(scenario_node.Nodes["output"]);
                }
                else if(scenario_node.Nodes["output"] != null)
                {
                    updateBackgourndWorkerRemove.Schedule(new TreeNode[] { scenario_node, scenario_node.Nodes["output"] });
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (updateoutputbackgroundworker.IsBusy == false)
            {
                updateoutputbackgroundworker.RunWorkerAsync();
            }

            if (updateInputBackGroundWorker.IsBusy == false)
            {
                updateInputBackGroundWorker.RunWorkerAsync();
            }

            if (updateBackgourndWorkerRemove.IsBusy == false)
            {
                updateBackgourndWorkerRemove.RunWorkerAsync();
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

        private void Remove_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

        private void ShowFolderLocation_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.ToolTipText;

            if (System.IO.Directory.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        private void ShowFileLocation_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.ToolTipText;

            if (System.IO.File.Exists(path))
            {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(path));
            }
             
        }
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used. 
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }

            // Copy the dragged node when the right mouse button is used. 
            else if (e.Button == MouseButtons.Right)
            {
                DoDragDrop(e.Item, DragDropEffects.Copy);
            }
        }

        // Set the target drop effect to the effect  
        // specified in the ItemDrag event handler. 
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = e.AllowedEffect;
        }
        private void treeView1_DragLeave(object sender, EventArgs e)
        {
           
        }
        void AddMapsInFolder(string path, ref FrmMap map)
        {
            foreach (string file in System.IO.Directory.GetFiles(path).Where(o => System.IO.Path.GetExtension(o) == ".img" || System.IO.Path.GetExtension(o) == ".gis"))
            {
                if (map == null)
                {
                    map = new FrmMap(DragDropOnMap);

                    map.Location = this.dockContainer1.PointToClient(Cursor.Position);

                    dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid());

                }
                map.LoadImageFile(file);
            }
            foreach (string subfolder in System.IO.Directory.GetDirectories(path))
            {
                AddMapsInFolder(subfolder, ref map);
            }
        }
        private void dockContainer1_DragDrop(object sender, DragEventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            string path = treeView1.SelectedNode.ToolTipText;

            if (System.IO.Directory.Exists(path))
            {
                FrmMap map = null;
                AddMapsInFolder(path, ref map);
            }
            if (System.IO.File.Exists(path) == false) return;

            if (System.IO.Path.GetExtension(path) == ".img" || System.IO.Path.GetExtension(path) == ".gis")
            {
                FrmMap map = new FrmMap(DragDropOnMap);
                
                map.Location = this.dockContainer1.PointToClient(Cursor.Position);
                
                dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid());

                map.LoadImageFile(path);
                
            }
            if (System.IO.Path.GetExtension(path) == ".txt" || System.IO.Path.GetExtension(path) == ".csv")
            {
                FrmTXTDisplay txt = new FrmTXTDisplay();

                txt.Location = this.dockContainer1.PointToClient(Cursor.Position);

                dockContainer1.Add(txt, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid());


                txt.AppendText(System.IO.File.ReadAllLines(path));
            }
        }
        void DragDropOnMap(object sender, DragEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string path = treeView1.SelectedNode.ToolTipText;
                ((FrmMap)sender).LoadImageFile(path);
            }
        }
        private void dockContainer1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (e.Node != null)
            {
                treeView1.SelectedNode = e.Node;
            }
        }

        

        

         
        
    }
}
