using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Crom.Controls.Docking;

namespace LandisUserInterface
{
    public partial class FrmMain : Form
    {
        private TreeNode HeaderScenarioFiles;

        List<TreeNode> NodesForRemoval;
        List<TreeNode[]> NodesForAddition;

        List<TimerBackgroundWorker> bgw;

        Dictionary<string, List<DockableFormInfo>> Docks = new Dictionary<string, List<DockableFormInfo>>();


        public FrmMain()
        {
             
            InitializeComponent();

            NodesForRemoval = new List<TreeNode>();
            NodesForAddition = new List<TreeNode[]>();

            bgw = new List<TimerBackgroundWorker>();


            bgw.Add(new TimerBackgroundWorker(CheckForNewOutputNodesToAdd, AddNewNodes));
            bgw.Add(new TimerBackgroundWorker(CheckForNewInputNodesToAdd, null));
            bgw.Add(new TimerBackgroundWorker(CheckForNodesToRemove, RemoveNodes));

         
            this.WindowState = FormWindowState.Maximized;

            this.treeView1.AllowDrop = true;
            this.treeView1.Font = new Font("Times New Roman", 14);
            this.treeView1.ShowNodeToolTips = true;

           

            HeaderScenarioFiles = new TreeNode("Scenario Files");
            HeaderScenarioFiles.SelectedImageKey =  HeaderScenarioFiles.ImageKey = "RightArrow";
            this.treeView1.Nodes.Add(HeaderScenarioFiles);
            HeaderScenarioFiles.ExpandAll();

         

           
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
            string LastScenarioFileNames = Properties.Settings.Default.LastScenarioFileNames;

            while (Properties.Settings.Default.LastScenarioFileNames.Contains(FileName + ";"))
            { 
                Properties.Settings.Default.LastScenarioFileNames = Properties.Settings.Default.LastScenarioFileNames.Replace(FileName + ";", "");
                Properties.Settings.Default.Save();
            }
            
            
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
        System.Windows.Forms.ContextMenuStrip GetContextMenuStrip(System.Windows.Forms.ToolStripItem[] ToolStripItems)
        {
                System.Windows.Forms.ContextMenuStrip c = new System.Windows.Forms.ContextMenuStrip();
                c.Items.AddRange(ToolStripItems);
                //c.Name = "";
                c.Size = new System.Drawing.Size(166, 48);
                return c;
             
        }
        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.Location);

                if (treeView1.SelectedNode == HeaderScenarioFiles)
                {
                    GetContextMenuStrip(new ToolStripItem[] { GetToolStripMenuItem(new EventHandler(this.AddScnFl_Click), "Add Scenario"), GetToolStripMenuItem(new EventHandler(this.ClearScenarioFiles_Click), "Clear Scenarios") }).Show(this.treeView1, e.Location);
                }
                else if (IsScenarioFile(treeView1.SelectedNode.ToolTipText))
                {
                    GetContextMenuStrip(new ToolStripItem[] { GetToolStripMenuItem(new EventHandler(this.Remove_Click), "Remove Scenario"), GetToolStripMenuItem(new EventHandler(this.ShowFileLocation_Click), "Show File Location"), GetToolStripMenuItem(new EventHandler(this.RunSimulation_Click), "Run Simulation") }).Show(this.treeView1, e.Location);
                }
                else if (System.IO.File.Exists(treeView1.SelectedNode.ToolTipText))
                {
                    GetContextMenuStrip(new ToolStripItem[] { GetToolStripMenuItem(new EventHandler(this.ShowFileLocation_Click), "Show File Location") }).Show(this.treeView1, e.Location);
                }
                else if (System.IO.Directory.Exists(treeView1.SelectedNode.ToolTipText))
                {
                    GetContextMenuStrip(new ToolStripItem[] { GetToolStripMenuItem(new EventHandler(this.ShowFolderLocation_Click), "Show Folder Location") }).Show(this.treeView1, e.Location);

                    
                }
            }
        }
         
         
        
        System.Windows.Forms.ToolStripMenuItem GetToolStripMenuItem(EventHandler eventhandler, string Text)
        {
            System.Windows.Forms.ToolStripMenuItem t = new System.Windows.Forms.ToolStripMenuItem();
            //t.Name = "";
            t.Size = new System.Drawing.Size(205, 22);
            t.Text = Text;
            t.Click += new System.EventHandler(eventhandler);
            return t;
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
       
        private void ClearScenarioFiles_Click(object sender, EventArgs e)
        {
            // TODO: remove associated windows??
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
                        NodesForRemoval.Add(child);// }, UpdateBackgroundWorker.AddOrRemove.Remove);

                    }
                    RemoveOldNodes(child);
                }

            }

        }
        void AddNewNodes(TreeNode parent)
        {
            string path = parent.ToolTipText;

            foreach (string file_path in System.IO.Directory.GetFiles(path))
            {
                if (parent.Nodes[System.IO.Path.GetFileName(file_path)] == null)
                {
                    TreeNode child = new TreeNode();
                    child.Name = child.Text = System.IO.Path.GetFileName(file_path);
                    child.ToolTipText = file_path;
                    child.ImageKey = child.SelectedImageKey = "File";
                    NodesForAddition.Add(new TreeNode[] { parent, child });
                
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
                    NodesForAddition.Add(new TreeNode[] { parent, child });
                }
                else AddNewNodes(parent.Nodes[subfolder.Split(System.IO.Path.DirectorySeparatorChar).Last()]);
            }
                
            
            
        }
        private void CheckForNewInputNodesToAdd(object sender, DoWorkEventArgs e)
        {
            foreach (TreeNode scenario_node in HeaderScenarioFiles.Nodes)
            {
                string path_scenario_file = scenario_node.ToolTipText;

                if (System.IO.File.Exists(path_scenario_file) == false) continue;

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

                            if (term.Trim().Contains(System.IO.Directory.GetCurrentDirectory()) == false)
                            {
                                node.ToolTipText = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), term.Trim());
                            }
                            if (scenario_node.Nodes[node.Name] == null)
                            {
                                NodesForAddition.Add(new TreeNode[] { scenario_node, node });
//                                update_backround_worker.Schedule(, UpdateBackgroundWorker.AddOrRemove.Add);
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void RemoveNodes(object sender, RunWorkerCompletedEventArgs e)
        {
            while (NodesForRemoval.Count > 0)
            {
                treeView1.Nodes.Remove(NodesForRemoval[0]);
                NodesForRemoval.RemoveAt(0);
            }
             
        }
        private void AddNewNodes(object sender, RunWorkerCompletedEventArgs  e)
        {
            while (this.NodesForAddition.Count > 0)
            {
                NodesForAddition[0][0].Nodes.Add(NodesForAddition[0][1]);
                NodesForAddition.RemoveAt(0);
            }
        }
        private void CheckForNewOutputNodesToAdd(object sender, DoWorkEventArgs e)
        {
            // List all files that should be in the interface
            foreach (TreeNode scenario_node in HeaderScenarioFiles.Nodes)
            {
                if (scenario_node == null) return;

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

                        NodesForAddition.Add(new TreeNode[] { scenario_node, child });

                        return;
                    }
                    else
                    {
                        AddNewNodes(scenario_node.Nodes["output"]);
                    }
                }
                

            }
        }


        private void CheckForNodesToRemove(object sender, DoWorkEventArgs e)
        {
            foreach (TreeNode scenario_node in HeaderScenarioFiles.Nodes)
            {
                if (scenario_node == null) continue;

                if (System.IO.File.Exists(scenario_node.ToolTipText) == false)
                {
                    NodesForRemoval.Add(scenario_node);
                }
                

                if (scenario_node.Nodes["output"]!= null)
                {
                    RemoveOldNodes(scenario_node.Nodes["output"]);
                }
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
        private void RunSimulation_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.ToolTipText;

            RunSimulation(path);
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            RemoveLastScenarioFileNames(treeView1.SelectedNode.ToolTipText);

            this.treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

        private void ShowFileLocation_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.ToolTipText;

           
            if (System.IO.File.Exists(path))
            {
                System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(path));
            }
        }

        private void ShowFolderLocation_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.ToolTipText;

            if (System.IO.Directory.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
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

                    if (Docks.ContainsKey(file) == false)
                    { 
                        Docks.Add(file, new List<DockableFormInfo>());
                    }

                    Docks[file].Add(dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));
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

                LogFile.Write("Adding map to dockContainer1");

                if (Docks.ContainsKey(path) == false)
                { 
                    Docks.Add(path, new List<DockableFormInfo>());
                }

                Docks[path].Add(dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));
 
                LogFile.Write("LoadImageFile");

                map.LoadImageFile(path);
                
            }
            if (System.IO.Path.GetExtension(path) == ".txt" || System.IO.Path.GetExtension(path) == ".csv")
            {
                

               FrmSelectProgram fsp = new FrmSelectProgram(Cursor.Position);

               fsp.ShowInTaskbar = false;

               fsp.ShowDialog();

               if (fsp.Selection == FrmSelectProgram.Options.NotePad)
               {
                   FrmTXTDisplay txt = new FrmTXTDisplay(path);

                   txt.Location = this.dockContainer1.PointToClient(Cursor.Position);

                   if (Docks.ContainsKey(path) == false)
                   { 
                       Docks.Add(path, new List<DockableFormInfo>());
                   }

                   Docks[path].Add(dockContainer1.Add(txt, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));

                   
               }
               if (fsp.Selection == FrmSelectProgram.Options.Excel)
               {
                   FrmGrid grid = new FrmGrid(path);

                   grid.Location = this.dockContainer1.PointToClient(Cursor.Position);

                   if (Docks.ContainsKey(path) == false)
                   { 
                       Docks.Add(path, new List<DockableFormInfo>());
                   }

                   Docks[path].Add(dockContainer1.Add(grid, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));

                   
               }
               if (fsp.Selection == FrmSelectProgram.Options.Zgraph)
               {
                   FrmGraph graph = new FrmGraph();

                   graph.DragDrop += new System.Windows.Forms.DragEventHandler(FrmGraph_DragDrop);

                   graph.LoadFile(path);

                   graph.Location = this.dockContainer1.PointToClient(Cursor.Position);

                   if (Docks.ContainsKey(path) == false)
                   { 
                       Docks.Add(path, new List<DockableFormInfo>());
                   }

                   Docks[path].Add(dockContainer1.Add(graph, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));
                   
                    
               }
               
            }
        }
        void FrmGraph_DragDrop(object sender, DragEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string path = treeView1.SelectedNode.ToolTipText;
                ((FrmGraph)sender).LoadFile(path);
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
