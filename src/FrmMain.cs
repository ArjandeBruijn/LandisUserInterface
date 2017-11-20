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
        Dictionary<string, List<DockableFormInfo>> Docks = new Dictionary<string, List<DockableFormInfo>>();

        TreeNodeFile UpdatedScenarioNode = null;

        List<TreeNodeFile> ScenariosToRemove = new List<TreeNodeFile>();

        static int c = 0;

        private void FrmMain_Load(object sender, EventArgs e)
        {
           
            foreach (string FileName in Global.ScenarioFileNames)
            {
                AddScenario(FileName);
            }
            //if (this.treeView1.Nodes["Scenario Files"].Nodes.Count > 0)
            //{
            //    UpdateScenarioNode = this.treeView1.Nodes["Scenario Files"].Nodes[0];
            //    toolStripStatusLabel1.Text = "Loading " + UpdateScenarioNode.Name;
            //}
        }

        public FrmMain()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            this.treeView1.AllowDrop = true;
            this.treeView1.Font = new Font("Times New Roman", 14);
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.TreeViewNodeSorter = new NodeSorter();


            TreeNodeFile HeaderScenarioFiles = new TreeNodeFile("Scenario Files", "Scenario Files", 0, "RightArrow", null, () => backgroundWorker1.CancellationPending);
            this.treeView1.Nodes.Add(HeaderScenarioFiles);
            HeaderScenarioFiles.ExpandAll();

            backgroundWorker2.RunWorkerAsync();
            timer1.Start();
        }

        static int get_Year(string FileName)
        {
            int year = -1;
            if (int.TryParse(System.Text.RegularExpressions.Regex.Match(FileName, @"\d+").Value, out year))
            {
                return year;
            }
            return year;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripStatusLabel1.Text = e.UserState.ToString();
        }

        private void AddScenario(string FileName)
        {
            if (IsScenarioFile(FileName) == false) return;

            if (this.treeView1.Nodes["Scenario Files"].Nodes.ContainsKey(FileName)) return;

            // Add a node and a filename
            Global.AddScenario(FileName);

            // Get a placeholder without subnodes
            UpdatedScenarioNode = new TreeNodeFile(FileName, System.IO.Path.GetFileName(FileName), 0, "File", null, () => backgroundWorker1.CancellationPending);


            this.treeView1.Nodes["Scenario Files"].Nodes.Add(UpdatedScenarioNode);
            foreach (TreeNodeFile tn in treeView1.Nodes) tn.Expand();
          
        }

        private void RemoveScenario(TreeNodeFile node)
        {
            Global.RemoveScenario(node.FullPath);
            this.treeView1.Nodes["Scenario Files"].Nodes.Remove(node);

            if (UpdatedScenarioNode != null)
            {
                if (node.FullPath == UpdatedScenarioNode.FullPath)
                {
                    UpdatedScenarioNode = null;
                    this.toolStripStatusLabel1.Text = String.Empty;
                }
            }
        }
      
        void NodesCompare(System.Windows.Forms.TreeNode old_node, System.Windows.Forms.TreeNode new_node)
        {
            if (old_node.ForeColor != new_node.ForeColor) old_node.ForeColor = new_node.ForeColor;

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

            foreach (System.Windows.Forms.TreeNode old_sub_node in old_node.Nodes)
            {
                if (new_node.Nodes.ContainsKey(old_sub_node.Name)==false)
                {
                    old_sub_node.Remove();
                    //old_sub_node.Nodes.RemoveByKey(old_sub_node.Name);
                }


            }
        }

        TreeNodeFile[] GetFolderContent(TreeNodeFile parent)
        {
            /// Populate a treenode that is associated with a folder name with its subnodes
            List<TreeNodeFile> TreeNodes = new List<TreeNodeFile>();

            if (this.backgroundWorker1.CancellationPending)return TreeNodes.ToArray();

            try
            {
                string path = parent.Tag.ToString();

                foreach (string folder in System.IO.Directory.GetDirectories(path))
                {
                    string lastsubdir = folder.Split(System.IO.Path.DirectorySeparatorChar).Last();
                    TreeNodes.Add(new TreeNodeFile(folder, lastsubdir, 0, "Folder", GetFolderContent, () => backgroundWorker1.CancellationPending));
                }

                foreach (string file in System.IO.Directory.GetFiles(path))
                {
                    TreeNodeFile node = new TreeNodeFile(file, System.IO.Path.GetFileName(file), get_Year(System.IO.Path.GetFileName(file)), "File", null, () => backgroundWorker1.CancellationPending);

                    TreeNodes.Add(node);
                }
            }
            catch(System.Exception ex)
            {
                if (ex is System.IO.DirectoryNotFoundException || ex is System.IO.FileNotFoundException)
                {
                    return TreeNodes.ToArray();
                }
                else throw ex;
            }

            return TreeNodes.ToArray();
        }

        TreeNodeFile[] GetScenarioSubNodes(TreeNodeFile parent)
        {
            /// Populate a treenode that is associated with a scenario file name with its subnodes
            List<TreeNodeFile> TreeNodes = new List<TreeNodeFile>();

            if (System.IO.File.Exists(parent.FullPath) == false)
            {
                parent.ForeColor = System.Drawing.Color.Red;
                parent.ToolTipText += "No access to this path";
                return TreeNodes.ToArray();
            }
            else
            {
                parent.ToolTipText = parent.ToolTipText.Replace("No access to this path","");
                parent.ForeColor = System.Drawing.Color.Black;
            }

            if (this.backgroundWorker1.CancellationPending) return TreeNodes.ToArray();
            
            string Directory = System.IO.Path.GetDirectoryName(parent.FullPath);

            string OutputDirectory =System.IO.Path.Combine(Directory, "output");

            if(System.IO.Directory.Exists(OutputDirectory))
            {
                string folder = OutputDirectory.Split(System.IO.Path.DirectorySeparatorChar).Last();
                TreeNodes.Add(new TreeNodeFile(OutputDirectory, folder, 0, "Folder", GetFolderContent, () => backgroundWorker1.CancellationPending));
            }

            string LogFile = System.IO.Path.Combine(Directory, "Landis-log.txt");

            if (System.IO.File.Exists(LogFile))
            {
                TreeNodes.Add(new TreeNodeFile(LogFile, System.IO.Path.GetFileName(LogFile), 0, "File", null, () => backgroundWorker1.CancellationPending));
            }

           
            TreeNodes.AddRange(GetSubNodesFromTextFile(parent));
            


            return TreeNodes.ToArray();
        }
       
        TreeNodeFile[] GetSubNodesFromTextFile(TreeNodeFile parent)
        {
           
            List<TreeNodeFile> FileNamesInFile = new List<TreeNodeFile>();
 
            List<string> Content = new List<string>(System.IO.File.ReadAllLines(parent.FullPath));

            for (int l =0; l< Content.Count(); l++)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    return FileNamesInFile.ToArray();
                }

                if (Content[l].Contains(">>"))
                {
                    Content[l] = Content[l].Remove(Content[l].IndexOf(">>"));
                }
                if (Content[l].Trim().Length == 0)
                {
                    continue;
                }

                string[] line = Content[l].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(string term in line)
                {
                    if (term.Contains('.')==false)continue;

                    //if (term.IndexOf('.') != term.Length - 4) continue;

                    //if (term.Contains(".img") || term.Contains(".gis")) continue;

                    double d;
                    if (double.TryParse(term, out d) == true) continue;

                    try
                    {
                        string FileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(parent.FullPath), term);
                        if (System.IO.File.Exists(FileName) == true && FileNamesInFile.Where(o => FileName == o.FullPath).Count() ==0)
                        {
                            TreeNodeFile node = new TreeNodeFile(FileName, System.IO.Path.GetFileName(FileName), 0, "File", null, () => backgroundWorker1.CancellationPending);
                            FileNamesInFile.Add(node);
                            FileNamesInFile.AddRange(GetSubNodesFromTextFile(node));
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

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.Location);

                if (treeView1.SelectedNode == treeView1.Nodes["Scenario Files"])
                {
                    new ContextMenuStrip(new ToolStripItem[] 
                    {
                        new ToolStripMenuItem(
                            new EventHandler(AddScnFl_Click), "Add Scenario"),
                        new ToolStripMenuItem(
                            new EventHandler(ClearScenarioFiles_Click), "Clear Scenarios"),
                        new ToolStripMenuItem(
                            new EventHandler(ChangeLandisExecutableLocation_Click), "Change Landis Executable Location")
                    })
                    .Show(this.treeView1, e.Location);


                }
                else if (IsScenarioFile(((TreeNodeFile)treeView1.SelectedNode).FullPath)) 
                {
                    new ContextMenuStrip(new ToolStripItem[] { new ToolStripMenuItem(new EventHandler(this.Remove_Click), "Remove Scenario"), new ToolStripMenuItem(new EventHandler(this.ShowFileLocation_Click), "Show File Location"), new ToolStripMenuItem(new EventHandler(this.RunSimulation_Click), "Run Simulation") }).Show(this.treeView1, e.Location);
                }
                else if (System.IO.File.Exists(((TreeNodeFile)treeView1.SelectedNode).FullPath))
                {
                    new ContextMenuStrip(new ToolStripItem[] { new ToolStripMenuItem(new EventHandler(this.ShowFileLocation_Click), "Show File Location") }).Show(this.treeView1, e.Location);
                }
                else if (System.IO.Directory.Exists(((TreeNodeFile)treeView1.SelectedNode).FullPath))
                {
                    new ContextMenuStrip(new ToolStripItem[] { new ToolStripMenuItem(new EventHandler(this.ShowFolderLocation_Click), "Show Folder Location") }).Show(this.treeView1, e.Location);                    
                }
            }
        }
        
        
        private void AddScnFl_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();

            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Select Scenario File";
            of.Multiselect = true;

            if(of.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in of.FileNames)
                {
                    AddScenario(file);
                }
            }

             
        }
       
        private void ClearScenarioFiles_Click(object sender, EventArgs e)
        {
            Global.ClearScenarios();

            this.treeView1.Nodes["Scenario Files"].Nodes.Clear();

            this.toolStripStatusLabel1.Text = String.Empty;
        }
        private void ChangeLandisExecutableLocation_Click(object sender, EventArgs e)
        {
            GetLandisExecutableLocation(true);
        }
        public void RunSimulation(string path)
        {
            if (System.IO.File.Exists(path) == false) throw new System.Exception("File " + path + " does not exist");

            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(path));

            Directory.DeleteDirectory("output");

            if (System.IO.File.Exists(path))
            {
                var sb = new StringBuilder();

                System.Diagnostics.Process simulation = new System.Diagnostics.Process();

                simulation.StartInfo.UseShellExecute = false;

                // redirect the output
                simulation.StartInfo.RedirectStandardOutput = true;

                simulation.StartInfo.RedirectStandardError = true;

                // hookup the eventhandlers to capture the data that is received
                simulation.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
                simulation.ErrorDataReceived += (sender, args) => sb.AppendLine(args.Data);
                 
                GetLandisExecutableLocation(false);

                simulation.StartInfo.FileName = Global.LandisConsoleExe;

                simulation.StartInfo.Arguments = "\"" + path + "\"";

                simulation.Start();

                simulation.BeginOutputReadLine();

                simulation.BeginErrorReadLine();

                // until we are done
                simulation.WaitForExit();

                MessageBox.Show(sb.ToString());
            }

        }

        private static void GetLandisExecutableLocation(bool change)
        {
            string fileName = change? "": Global.LandisConsoleExe;

            while (System.IO.File.Exists(fileName) == false)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Select your landis console executable";

                DialogResult result = dlg.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Global.LandisConsoleExe = fileName = dlg.FileName;
                }
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }
        }

        private void RunSimulation_Click(object sender, EventArgs e)
        {
            string path = treeView1.SelectedNode.ToolTipText;

            RunSimulation(path);
        }
        
        private void Remove_Click(object sender, EventArgs e)
        {
            ScenariosToRemove.Add((TreeNodeFile)treeView1.SelectedNode);
            backgroundWorker1.CancelAsync();
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
         
        void AddMapsInFolder(TreeNodeFile node, ref FrmMap map)
        {
            // node is a folder node. Get the files with img or gis extensions
            foreach (string path in System.IO.Directory.GetFiles(node.FullPath).Where(o => System.IO.Path.GetExtension(o) == ".img" || System.IO.Path.GetExtension(o) == ".gis"))
            {
                // If the first time, create the map
                if (map == null)
                {
                    map = new FrmMap(DragDropOnMap);

                    map.Location = this.dockContainer1.PointToClient(Cursor.Position);

                    if (Docks.ContainsKey(path) == false)
                    {
                        Docks.Add(path, new List<DockableFormInfo>());
                    }

                    Docks[path].Add(dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));
                }//---------------------------

                TreeNodeFile sub_node = new TreeNodeFile(path, System.IO.Path.GetFileName(path), get_Year(System.IO.Path.GetFileName(path)), "File", null, () => backgroundWorker1.CancellationPending);
                map.LoadImageFile(sub_node);
                node.Nodes.Add(sub_node);
            }
            // Get the subfolders
            foreach (string subfolder in System.IO.Directory.GetDirectories(node.FullPath))
            {
                TreeNodeFile sub_node = new TreeNodeFile(subfolder, System.IO.Path.GetFileName(subfolder), 0, "Folder", null, () => backgroundWorker1.CancellationPending);

                AddMapsInFolder(sub_node, ref map);
            }
        }

        private void dockContainer1_DragDrop(object sender, DragEventArgs e)
        {
            
            if (treeView1.SelectedNode == null) return;

            TreeNodeFile source_node = (TreeNodeFile)treeView1.SelectedNode;

            // If it is a directory path
            if (System.IO.Directory.Exists(source_node.FullPath))
            {
                // Get its content and try to display it
                FrmMap map = null;
                AddMapsInFolder(source_node, ref map);
            }
            if (System.IO.File.Exists(source_node.FullPath) == false) return;

            if (System.IO.Path.GetExtension(source_node.FullPath) == ".img" || System.IO.Path.GetExtension(source_node.FullPath) == ".gis")
            {
                FrmMap map = new FrmMap(DragDropOnMap);
                
                map.Location = this.dockContainer1.PointToClient(Cursor.Position);

                if (Docks.ContainsKey(source_node.FullPath) == false)
                {
                    Docks.Add(source_node.FullPath, new List<DockableFormInfo>());
                }

                Docks[source_node.FullPath].Add(dockContainer1.Add(map, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));

                map.LoadImageFile(source_node);
                
            }
            if (System.IO.Path.GetExtension(source_node.FullPath) == ".txt" || System.IO.Path.GetExtension(source_node.FullPath) == ".csv")
            {
                

               FrmSelectProgram fsp = new FrmSelectProgram(Cursor.Position);

               fsp.ShowInTaskbar = false;

               fsp.ShowDialog();

               if (fsp.Selection == FrmSelectProgram.Options.NotePad)
               {
                   FrmTXTDisplay txt = new FrmTXTDisplay(source_node.FullPath);

                   txt.Location = this.dockContainer1.PointToClient(Cursor.Position);

                   if (Docks.ContainsKey(source_node.FullPath) == false)
                   {
                       Docks.Add(source_node.FullPath, new List<DockableFormInfo>());
                   }

                   Docks[source_node.FullPath].Add(dockContainer1.Add(txt, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));

                   
               }
               if (fsp.Selection == FrmSelectProgram.Options.Excel)
               {
                   FrmGrid grid = new FrmGrid(source_node.FullPath);

                   grid.Location = this.dockContainer1.PointToClient(Cursor.Position);

                   if (Docks.ContainsKey(source_node.FullPath) == false)
                   {
                       Docks.Add(source_node.FullPath, new List<DockableFormInfo>());
                   }

                   Docks[source_node.FullPath].Add(dockContainer1.Add(grid, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));

                   
               }
               if (fsp.Selection == FrmSelectProgram.Options.Zgraph)
               {
                   FrmGraph graph = new FrmGraph(source_node.FullPath);

                   graph.DragDrop += new System.Windows.Forms.DragEventHandler(FrmGraph_DragDrop);

                   graph.LoadFile(source_node.FullPath);

                   graph.Location = this.dockContainer1.PointToClient(Cursor.Position);

                   if (Docks.ContainsKey(source_node.FullPath) == false)
                   {
                       Docks.Add(source_node.FullPath, new List<DockableFormInfo>());
                   }

                   Docks[source_node.FullPath].Add(dockContainer1.Add(graph, Crom.Controls.Docking.zAllowedDock.All, Guid.NewGuid()));
                   
                    
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
                ((FrmMap)sender).LoadImageFile((TreeNodeFile)treeView1.SelectedNode);
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Cancel)
            {
                this.backgroundWorker1.ReportProgress(0, "Worker cancelled");
                return;
            }

            // Pick the node to update
            if (UpdatedScenarioNode == null)
            {
                if (c > treeView1.Nodes["Scenario Files"].Nodes.Count - 1) c = 0;

                if (treeView1.Nodes["Scenario Files"].Nodes.Count > 0)
                {
                    TreeNodeFile node = (TreeNodeFile)treeView1.Nodes["Scenario Files"].Nodes[c];

                    this.backgroundWorker1.ReportProgress(0, "Loading " + node.Name);

                    UpdatedScenarioNode = new TreeNodeFile(node.Name, System.IO.Path.GetFileName(node.Name), 0, "File", GetScenarioSubNodes, () => backgroundWorker1.CancellationPending);
                }

                c++;
            }

            System.Threading.Thread.Sleep(500);
        }
       
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ScenariosToRemove.Count() > 0)
            {
                foreach (TreeNodeFile node in ScenariosToRemove)
                {
                    RemoveScenario(node);
                }
                ScenariosToRemove.Clear();
            }

            if (e.Cancelled) return;

            if (UpdatedScenarioNode != null && this.treeView1.Nodes["Scenario Files"].Nodes.ContainsKey(UpdatedScenarioNode.Name))
            {
                NodesCompare(this.treeView1.Nodes["Scenario Files"].Nodes[UpdatedScenarioNode.Name], UpdatedScenarioNode);

            }
            UpdatedScenarioNode = null;

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.toolStripStatusLabel1.Text = e.UserState.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                for (int d = 0; d < Docks.Count; d++)
                {
                    List<DockableFormInfo> values = Docks.Values.ToArray()[d];

                    for (int fi = values.Count - 1; fi >= 0; fi--)
                    {
                        if (dockContainer1.Contains(values[fi].DockableForm) == false)
                        {
                            if (values[fi].DockableForm.IsDisposed == false)
                            {
                                if (values[fi].DockableForm.GetType() == typeof(FrmTXTDisplay))
                                {
                                    ((FrmTXTDisplay)values[fi].DockableForm).ClosePending = true;
                                }

                                values.RemoveAt(fi);
                            }

                        }
                    }
                }
            }
            catch(ObjectDisposedException)
            {
             
            }
            backgroundWorker2.RunWorkerAsync();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            double t = 0.0;
        }
    }
}
