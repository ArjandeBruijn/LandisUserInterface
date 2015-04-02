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

        TreeNode ScenarioNode = null;
        string LoadingScenarioFile = null;
        static int c = 0;

        Dictionary<string, List<DockableFormInfo>> Docks = new Dictionary<string, List<DockableFormInfo>>();
        BackgroundWorker backgroundworker;
        Timer timer;
        public FrmMain()
        {
            InitializeComponent();

            
            this.WindowState = FormWindowState.Maximized;

            this.treeView1.AllowDrop = true;
            this.treeView1.Font = new Font("Times New Roman", 14);
            this.treeView1.ShowNodeToolTips = true;
             
            HeaderScenarioFiles = new TreeNode("Scenario Files","Scenario Files", "RightArrow", null);
           
            this.treeView1.Nodes.Add(HeaderScenarioFiles);
            HeaderScenarioFiles.ExpandAll();

            backgroundworker = new BackgroundWorker();
            timer = new Timer();
            timer.Tick += RunWorker;
            timer.Interval = 500;
            timer.Start();

            backgroundworker.DoWork += LoadFiles;
            backgroundworker.RunWorkerCompleted += AddScenarioNodes;
            TreeNode.sendmessage = SendMessage;

             
        }
        void RunWorker(object sender, EventArgs e)
        {
            if (backgroundworker.IsBusy == false)
            {
                this.backgroundworker.RunWorkerAsync();
            }
        }
        
        void AddScenarioNodes(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ScenarioNode != null)
            {
                if (this.treeView1.Nodes["Scenario Files"].Nodes.ContainsKey(LoadingScenarioFile))
                {
                    int index = this.treeView1.Nodes["Scenario Files"].Nodes.IndexOfKey(LoadingScenarioFile);
                    this.treeView1.Nodes["Scenario Files"].Nodes.RemoveAt(index);
                    this.treeView1.Nodes["Scenario Files"].Nodes.Insert(index, ScenarioNode);
                }
                else this.treeView1.Nodes["Scenario Files"].Nodes.Add(ScenarioNode);
                
                foreach (TreeNode tn in treeView1.Nodes)tn.Expand();
                 
                ScenarioNode = null;
            }
            LoadingScenarioFile = null;

            foreach (string ScenarioFile in LastScenarioPathList)
            {
                if (this.treeView1.Nodes["Scenario Files"].Nodes.ContainsKey(ScenarioFile) == false)
                {
                    LoadingScenarioFile = ScenarioFile;
                    toolStripStatusLabel1.Text = "Loading " + LoadingScenarioFile;
                    toolStripStatusLabel1.ToolTipText = LoadingScenarioFile;
                    return;
                }
            }
            if (LoadingScenarioFile == null)
            {
                LoadingScenarioFile = treeView1.Nodes["Scenario Files"].Nodes[c++].Tag.ToString();
                toolStripStatusLabel1.Text = "Updating " + LoadingScenarioFile;
                toolStripStatusLabel1.ToolTipText = LoadingScenarioFile;
                if (c == treeView1.Nodes["Scenario Files"].Nodes.Count) c = 0;
            }
        }
        void LoadFiles(object sender, DoWorkEventArgs e)
        {
            if (LoadingScenarioFile != null)
            {
                ScenarioNode = new TreeNode(LoadingScenarioFile, System.IO.Path.GetFileName(LoadingScenarioFile), "File", GetScenarioSubNodes);
            
            }
           
        }

        TreeNode[] GetFolderContent(TreeNode parent)
        {
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
        void SendMessage(string msg)
        {
            toolStripStatusLabel1.Text = msg;
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
                AddLastScenarioFileName(of.FileName);
            }
        }
       
        private void ClearScenarioFiles_Click(object sender, EventArgs e)
        {
            // TODO: remove associated windows??
            this.HeaderScenarioFiles.Nodes.Clear();
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

                 

                if (Docks.ContainsKey(path) == false)
                { 
                    Docks.Add(path, new List<DockableFormInfo>());
                }

                Docks[path].Add(dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));
 
                 

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
