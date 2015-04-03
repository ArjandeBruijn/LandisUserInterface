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

        TreeNode UpdatedScenarioNode = null;
        System.Windows.Forms.TreeNode UpdateScenarioNode = null;
        static int c = 0;

        Dictionary<string, List<DockableFormInfo>> Docks = new Dictionary<string, List<DockableFormInfo>>();
         
        static BackgroundWorker backgroundworker;
         
        public FrmMain()
        {
            InitializeComponent();

            //Properties.Settings.Default.LastScenarioFileNames = "";
            //Properties.Settings.Default.Save();

            this.WindowState = FormWindowState.Maximized;

            this.treeView1.AllowDrop = true;
            this.treeView1.Font = new Font("Times New Roman", 14);
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.TreeViewNodeSorter = new NodeSorter();

            backgroundworker = new BackgroundWorker();
            
            HeaderScenarioFiles = new TreeNode("Scenario Files","Scenario Files", "RightArrow", null);
           
            this.treeView1.Nodes.Add(HeaderScenarioFiles);
            HeaderScenarioFiles.ExpandAll();

            
            
            backgroundworker.DoWork += backgroundworker_DoWork;
            backgroundworker.RunWorkerCompleted += backgroundworker_RunWorkerCompleted;
            backgroundworker.WorkerSupportsCancellation = true;
            backgroundworker.RunWorkerAsync();
        }
       
        private void FrmMain_Load(object sender, EventArgs e)
        {
            foreach (string FileName in Global.LastScenarioFileNames)
            {
                AddScenario(FileName);
            }
            if (this.treeView1.Nodes["Scenario Files"].Nodes.Count > 0)
            {
                UpdateScenarioNode = this.treeView1.Nodes["Scenario Files"].Nodes[0];
                toolStripStatusLabel1.Text = "Loading " + UpdateScenarioNode.Name;
            }
        }
        private void AddScenario(string FileName)
        {
            // Get a placeholder without subnodes
            if (this.treeView1.Nodes["Scenario Files"].Nodes.ContainsKey(FileName) == false)
            {
                TreeNode node = new TreeNode(FileName, System.IO.Path.GetFileName(FileName), "File", null);

                this.treeView1.Nodes["Scenario Files"].Nodes.Add(node);
                foreach (TreeNode tn in treeView1.Nodes) tn.Expand();
                UpdateScenarioNode = node;
                toolStripStatusLabel1.Text = "Loading " + UpdateScenarioNode.Name;
            }

            // Add a node and a filename
            Global.AddScenario(FileName);

        }
        // Removes a treenode and associated name in the registry
        private void RemoveScenario(TreeNode node)
        {
            backgroundworker.CancelAsync();

            while (backgroundworker.IsBusy == false) ;

            Global.RemoveScenario(node.FullPath);

            this.treeView1.Nodes.Remove(node);

            
        }
        
        void backgroundworker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundworker.CancellationPending)
            {
                return;
            }
            // Update a predetermined scenario node
            if (UpdateScenarioNode != null)
            {
                UpdatedScenarioNode = new TreeNode(UpdateScenarioNode.Name, System.IO.Path.GetFileName(UpdateScenarioNode.Name), "File", GetScenarioSubNodes);
            }
            System.Threading.Thread.Sleep(500);
        }
        void backgroundworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundworker.CancellationPending)
            {
                return;
            }

            // Replace scenario with updated version
            if (UpdatedScenarioNode != null)
            {
                NodesCompare(this.treeView1.Nodes["Scenario Files"].Nodes[UpdateScenarioNode.Name], UpdatedScenarioNode);

                UpdatedScenarioNode = null;
            }

            // Set scenario node for updating
            if (treeView1.Nodes["Scenario Files"].Nodes.Count > c)
            {
                UpdateScenarioNode = treeView1.Nodes["Scenario Files"].Nodes[c++];
                toolStripStatusLabel1.Text = "Updating " + UpdateScenarioNode.Name;
            }
            else c = 0;

            backgroundworker.RunWorkerAsync();
        }
        void NodesCompare(System.Windows.Forms.TreeNode old_node, System.Windows.Forms.TreeNode new_node)
        {
            foreach (System.Windows.Forms.TreeNode new_sub_node in new_node.Nodes)
            {
                if (old_node.Nodes.ContainsKey(new_sub_node.Name))
                {
                    NodesCompare(old_node.Nodes[new_sub_node.Name], new_sub_node);
                }
                else
                {
                    old_node.Nodes.Add(new_sub_node);
                }
            }
        }

        TreeNode[] GetFolderContent(TreeNode parent)
        {
            /// Populate a treenode that is associated with a folder name with its subnodes
            List<TreeNode> TreeNodes = new List<TreeNode>();

            string path = parent.Tag.ToString();

            foreach (string folder in System.IO.Directory.GetDirectories(path))
            {
                string lastsubdir = folder.Split(System.IO.Path.DirectorySeparatorChar).Last();
                TreeNodes.Add(new TreeNode(folder, lastsubdir, "Folder", GetFolderContent));
            }

            foreach (string file in System.IO.Directory.GetFiles(path))
            {
                TreeNode node = new TreeNode(file, System.IO.Path.GetFileName(file),"File", null);

                TreeNodes.Add(node);
            }

            return TreeNodes.ToArray();
        }
        TreeNode[] GetScenarioSubNodes(TreeNode parent)
        {
            /// Populate a treenode that is associated with a scenario file name with its subnodes
            List<TreeNode> TreeNodes = new List<TreeNode>();

            string path = parent.Tag.ToString();

            string Directory = System.IO.Path.GetDirectoryName(path);

            string OutputDirectory =System.IO.Path.Combine(Directory, "output");

            if(System.IO.Directory.Exists(OutputDirectory))
            {
                string folder = OutputDirectory.Split(System.IO.Path.DirectorySeparatorChar).Last();
                TreeNodes.Add(new TreeNode(OutputDirectory, folder, "Folder", GetFolderContent));
            }

            string LogFile = System.IO.Path.Combine(Directory, "Landis-log.txt");

            if (System.IO.File.Exists(LogFile))
            {
                TreeNodes.Add(new TreeNode(LogFile, System.IO.Path.GetFileName(LogFile), "File", null));
            }

            foreach (string File in GetFileNamesInFile(path).Distinct())
            {
                TreeNodes.Add(new TreeNode(File, System.IO.Path.GetFileName(File), "File", null));
            }


            return TreeNodes.ToArray();
        }
       
        string[] GetFileNamesInFile(string path)
        {
            List<string> FileNamesInFile = new List<string>();

            List<string> Content = new List<string>(System.IO.File.ReadAllLines(path));

            for (int line = Content.Count()-1; line > 0; line--)
            {
                if(Content[line].Contains(">>"))
                {
                    Content[line] = Content[line].Remove(Content[line].IndexOf(">>"));
                }
                if (Content[line].Trim().Length == 0)
                {
                    Content.RemoveAt(line);
                    continue;
                }
            }
            
            for (int l =0; l< Content.Count(); l++)
            {
                string[] line = Content[l].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(string term in line)
                {
                    if (term.Contains('.')==false)continue;

                    //if (term.IndexOf('.') != term.Length - 4) continue;

                    if (term.Contains(".img") || term.Contains(".gis")) continue;

                    try
                    {
                        string FileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), term);
                        if (System.IO.File.Exists(FileName) == true)
                        {
                            FileNamesInFile.Add(FileName);
                            FileNamesInFile.AddRange(GetFileNamesInFile(FileName));
                        }
                    }
                    catch
                    {
                        continue;
                    }
                    
                }
            }

            return FileNamesInFile.ToArray();
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
        System.Windows.Forms.ContextMenuStrip GetContextMenuStrip(System.Windows.Forms.ToolStripItem[] ToolStripItems)
        {
                System.Windows.Forms.ContextMenuStrip c = new System.Windows.Forms.ContextMenuStrip();
                c.Items.AddRange(ToolStripItems);
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
                    GetContextMenuStrip(new ToolStripItem[] { GetToolStripMenuItem(new EventHandler(this.AddScnFl_Click), "Add Scenario"), GetToolStripMenuItem(new EventHandler(this.ClearScenarios_Click), "Clear Scenarios") }).Show(this.treeView1, e.Location);
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
                AddScenario(of.FileName);
            }
        }
       
       
        
        
        private void RunSimulation_Click(object sender, EventArgs e)
        {
           
            ((TreeNode)treeView1.SelectedNode).RunSimulation();
        }
        private void ClearScenarios_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                RemoveScenario(node);
            }
        }
        private void Remove_Click(object sender, EventArgs e)
        {
            

            RemoveScenario((TreeNode)treeView1.SelectedNode);

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

         
        void AddMapsInFolder(TreeNode folder_node, ref FrmMap map)
        {
            foreach (TreeNode sub_node in folder_node.Nodes)
            {
                string file = sub_node.Tag.ToString();

                if (System.IO.Path.GetExtension(file) == ".img" || System.IO.Path.GetExtension(file) == ".gis")
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
                    map.LoadImageFile(sub_node);
                }
                AddMapsInFolder(sub_node, ref map);
            }
          
        }
        private void dockContainer1_DragDrop(object sender, DragEventArgs e)
        {
            
            if (treeView1.SelectedNode == null) return;

            string path = treeView1.SelectedNode.ToolTipText;

            if (System.IO.Directory.Exists(path))
            {
                FrmMap map = null;
                AddMapsInFolder((TreeNode) treeView1.SelectedNode, ref map);
            }
            if (System.IO.File.Exists(path) == false) return;
            
            if (System.IO.Path.GetExtension(path) == ".img" || System.IO.Path.GetExtension(path) == ".gis")
            {
                FrmMap map = new FrmMap(DragDropOnMap);
                
                map.Location = this.dockContainer1.PointToClient(Cursor.Position);

                 

                if (Docks.ContainsKey(path) == false)
                { 
                    Docks.Add(path, new List<DockableFormInfo>());
                }

                Docks[path].Add(dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));

                map.LoadImageFile((TreeNode)treeView1.SelectedNode);
                
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
                ((FrmMap)sender).LoadImageFile((TreeNode)treeView1.SelectedNode);
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
