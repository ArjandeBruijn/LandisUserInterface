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
    public partial class FrmMap : Form, MapWinGIS.ICallback 
    {
        
        TreeNodeFile ActiveLayer;

        MapWinGIS.GridColorScheme GridColorscheme;

        private int MapMin = int.MaxValue;

        private int MapMax = int.MinValue;

        IColorScheme color_scheme;

        BackgroundWorker backgroundworker;

        

        private List<TreeNodeFile> ImageFilesToLoad = new List<TreeNodeFile>();

        int BinWidth;

        public FrmMap(DragEventHandler DragDrop)
        {
            
            InitializeComponent();

            axMap1.SendMouseDown = true;

            backgroundworker = new BackgroundWorker();
            backgroundworker.RunWorkerCompleted += LoadGridFiles;

            backgroundworker.DoWork += new DoWorkEventHandler(SleepWorker);
            backgroundworker.RunWorkerAsync();

            this.DragDrop += DragDrop;
            TreeViewLegend.ImageList = new ImageList();
            this.treeViewLayers.TreeViewNodeSorter = new NodeSorter();

            axMap1.ScalebarVisible = false;

           
        }
        int Get_PX_max(MapWinGIS.Grid myGrid)
        {
            int col = 0;

            object o = myGrid.get_Value(col, 0);

            while ((int)Math.Round(double.Parse(myGrid.get_Value(col, 0).ToString()), 0) >= 0)
            {
                col++;
            }
            return col;
        }
        private MapWinGIS.Grid ActiveGrid
        {
            get
            {
                string FileName = ActiveLayer.FullPath;

                MapWinGIS.Grid grid = new MapWinGIS.Grid();

                grid.Open(FileName, MapWinGIS.GridDataType.LongDataType, true, MapWinGIS.GridFileType.UseExtension, this);

                return grid;
            }
        }
       
        private void axMap1_MouseDownEvent(object sender, AxMapWinGIS._DMapEvents_MouseDownEvent e)
        {
            if (axMap1.CursorMode != MapWinGIS.tkCursorMode.cmSelection) return;

            double PX = 0;
            double PY = 0;

            axMap1.PixelToProj(e.x, e.y, ref PX, ref PY);
            
            MapWinGIS.Grid myGrid = ActiveGrid;

            int PX_max = Get_PX_max(myGrid);

            int c, r;
            myGrid.ProjToCell(PX, -PY, out c, out r);
            double LandisValue = (double)myGrid.get_Value(c, r);
            string landis_label = (LandisValue < 0) ? "n/a" : LandisValue.ToString();

            string ManualLabel = String.Empty;
            foreach (TreeNodeLegendEntry node in TreeViewLegend.Nodes)
            {
                if (LandisValue > node.Min && LandisValue <= node.Max)
                {
                    ManualLabel = node.Text;
                }
            }

            myGrid.ProjToCell(PX, PY, out c, out r);
            double ProjectedValue = (double)myGrid.get_Value(c, r);
            string projected = (ProjectedValue < 0) ? "n/a" : ProjectedValue.ToString();

            toolStripStatusLabel2.Text = landis_label.ToString();

            if (ManualLabel != String.Empty && ManualLabel != landis_label)
            {
                toolStripStatusLabel2.Text += "|" + ManualLabel;
            }
            

        }
        
        void SleepWorker(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
        }

        public void Progress(string s1, int p, string s2)
        {
            toolStripProgressBar1.Value = p;
        }

        public void Error(string s1, string s2)
        {
            toolStripStatusLabel1.Text = s1;

        }

        

        public void LoadImageFile(TreeNodeFile node)
        {
            if (System.IO.Path.GetExtension(node.FullPath) == ".img" || System.IO.Path.GetExtension(node.FullPath) == ".gis")
            {
                ImageFilesToLoad.Add(node);
            }



        }
        int ValueRange
        {
            get
            {
                return MapMax - MapMin;
            }
        }
        private IColorScheme GetColorScheme()
        {
            BinWidth =1;

            if (ValueRange > new ColorScheme().ColorCount) BinWidth = (int)(1+ (ValueRange / new ColorScheme().ColorCount));

            if (ValueRange == 3)
            {
                return new ColorScheme(new System.Drawing.Color[] { System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Red, System.Drawing.Color.Green });
            }
            else if (BinWidth == 1)
            {
                System.Drawing.Color[] colors = ColorScheme.DefaultColorValues;

                if (MapMin == 0) colors[0] = System.Drawing.Color.White;

                return new ColorScheme(colors);
            }
            else
            {
                return new ColorSchemeClassified(new byte[] { 0, 255 }, new byte[] { 0, 255 }, new byte[] { 255, 255 }, 10);
            }
        }

         

        private MapWinGIS.GridColorScheme GetGridScheme(int MapMin, int MapMax, int BinWidth, IColorScheme color_scheme)
        {
            MapWinGIS.GridColorScheme gridScheme = new MapWinGIS.GridColorScheme();

            float i = MapMin;
            while (i <= MapMax)
            {
                MapWinGIS.GridColorBreak colorbreak = new MapWinGIS.GridColorBreak();

                colorbreak.LowValue = i;
                colorbreak.HighValue = i + BinWidth;

                System.Drawing.Color mycolor = color_scheme.NextColor;

                colorbreak.HighColor = colorbreak.LowColor = mycolor.ToUInt();

                gridScheme.InsertBreak(colorbreak);

                i += BinWidth;
            }
            gridScheme.ApplyColoringType(MapWinGIS.ColoringType.Gradient);

            return gridScheme;
        }

        Bitmap NodeRect(Font MyFont, System.Drawing.Color myColor, string RefString = "----")
        {
            SizeF stringSize = new SizeF();
            stringSize = this.CreateGraphics().MeasureString(RefString, MyFont);

            Bitmap flag = new Bitmap((int)stringSize.Width, (int)stringSize.Height);

            float margin = 0.2F;

            Rectangle rect = new Rectangle((int)(margin * flag.Size.Width), (int)(margin * flag.Size.Height), (int)((1F - margin) * flag.Size.Width), (int)((1F - margin) * flag.Size.Height));


            System.Drawing.Graphics.FromImage(flag).FillRectangle(new System.Drawing.SolidBrush(myColor), rect);

            System.Drawing.Graphics.FromImage(flag).DrawRectangle(new Pen(System.Drawing.Color.Black, 2), rect);

            return flag;
        }
        
        private void SetLayerSelection(TreeNodeFile node)
        {
            ActiveLayer = node;

            treeViewLayers.SelectedNode = node;

            foreach (System.Windows.Forms.TreeNode _node in treeViewLayers.Nodes)
            {
                if (_node.Name == node.Name) _node.Checked = true;
                else _node.Checked = false;
            }

            axMap1.set_LayerVisible(ActiveLayer.Layerhandle, true);


            for (int layer = 0; layer < axMap1.NumLayers; layer++)
            {
                int handle = axMap1.get_LayerHandle(layer);

                if (handle != ActiveLayer.Layerhandle)
                {
                    axMap1.set_LayerVisible(handle, false);
                }

            }
            
        
        }
        
        private void PlayAnimation()
        {
            SetLayerSelection((TreeNodeFile)this.treeViewLayers.Nodes[0]);

            animation_timer.Start();

            /*
            foreach (TreeNodeFile tree_node in this.treeViewLayers.Nodes)
            {
                 

                LogFile.WriteLine("SetLayerSelection");
                SetLayerSelection((TreeNodeFile)tree_node);

                LogFile.WriteLine("Refresh");
                this.Refresh();

                LogFile.WriteLine("Sleep");
                System.Threading.Thread.Sleep(2000);
            }
             */
        }
        private void SelectNextLayer(object sender, EventArgs e)
        {
            if (treeViewLayers.SelectedNode.Index == treeViewLayers.Nodes.Count - 1) animation_timer.Stop();

            else SetLayerSelection((TreeNodeFile)this.treeViewLayers.Nodes[treeViewLayers.SelectedNode.Index+1]);
        }
        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            
            string tag = (string)e.Button.Tag;
            switch (tag)
            {
                case "ZoomIn":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmZoomIn;
                    break;
                case "ZoomOut":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmZoomOut;
                    break;
                case "Pan":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmPan;
                    break;
                case "FullExtents":
                    axMap1.ZoomToMaxExtents();
                    break;
                case "Animation":

                    PlayAnimation();
                     
                    break;
                case "Info":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmSelection;
                    break;


            }
        }

        private void FrmMap_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void treeViewLayers_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                SetLayerSelection((TreeNodeFile)e.Node);
            }
        }

        private static bool PathIsNetworkPath(string path)
        {
            return new System.IO.DriveInfo(path).DriveType == System.IO.DriveType.Network;
        }

        private void LoadGridFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            while (ImageFilesToLoad.Count() > 0)
            {
                TreeNodeFile node = ImageFilesToLoad[0];
                ImageFilesToLoad.RemoveAt(0);
                if (PathIsNetworkPath(node.FullPath))
                {
                    string NewFileName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetFileName(node.FullPath));
                    
                    while (System.IO.File.Exists(NewFileName))
                    {
                        try
                        {
                            System.IO.File.Delete(NewFileName);
                        }
                        catch
                        {
                            int c = 0;
                            while (System.IO.File.Exists(NewFileName))
                            {
                                NewFileName = System.IO.Path.GetFileNameWithoutExtension(NewFileName) + (c++).ToString() + System.IO.Path.GetExtension(NewFileName);
                                c++;
                            }
                        }
                    }
                    System.IO.File.Copy(node.FullPath, NewFileName);
                    node.FullPath = NewFileName;
                }

                if (treeViewLayers.Nodes.ContainsKey(node.FullPath) == true)
                {
                    continue;
                }

                
                MapWinGIS.Grid grid = new MapWinGIS.Grid();


                statusStrip1.Refresh();


                grid.Open(node.FullPath, MapWinGIS.GridDataType.LongDataType, true, MapWinGIS.GridFileType.UseExtension, this);

                

                MapMin = Math.Min(0, Math.Min(MapMin, int.Parse(grid.Minimum.ToString())));
                MapMax = Math.Max(MapMax, int.Parse(grid.Maximum.ToString()));

                color_scheme = GetColorScheme();

                GridColorscheme = GetGridScheme(MapMin - 1, MapMax - 1, BinWidth, color_scheme);

                toolStripStatusLabel1.Text = "Creating image " + System.IO.Path.GetFileName(node.FullPath);
                statusStrip1.Refresh();

                MapWinGIS.Image map_image = new MapWinGIS.UtilsClass().GridToImage(grid, GridColorscheme, this);

                toolStripStatusLabel1.Text = "Ready";

                map_image.CustomColorScheme = GridColorscheme;

                 
                grid.Close();

                
                node.Layerhandle= axMap1.AddLayer(map_image, true);

                try
                {
                    treeViewLayers.Nodes.Add(node.Clone());
                }
                catch (System.Exception em)
                {
                    double t = 0.0;
                }
                 
                axMap1.set_LayerName(node.Layerhandle , node.Name);

                axMap1.SetImageLayerColorScheme(node.Layerhandle, GridColorscheme);

                axMap1.ZoomToMaxExtents();

                TreeViewLegend.Nodes.Clear();

                for (int i = 0; i < GridColorscheme.NumBreaks; i++)
                {
                    MapWinGIS.GridColorBreak colorbreak = GridColorscheme.get_Break(i);

                    string Label = null;
                    if (colorbreak.HighValue - colorbreak.LowValue <= 1)
                    {
                        Label = colorbreak.HighValue.ToString();
                    }
                    else
                    {
                        Label = colorbreak.LowValue.ToString() + "-" + colorbreak.HighValue.ToString();
                    }

                    TreeNodeLegendEntry legend_node = new TreeNodeLegendEntry(colorbreak.LowValue, colorbreak.HighValue);

                    if (TreeViewLegend.ImageList.Images.ContainsKey(legend_node.ImageKey) == false)
                    {
                        TreeViewLegend.ImageList.Images.Add(legend_node.ImageKey, NodeRect(this.TreeViewLegend.Font, Color.UIntToColor(colorbreak.HighColor)));
                    }
                    

                    this.TreeViewLegend.Nodes.Add(legend_node);

                }
                SetLayerSelection(node);

                

                this.axMap1.Invalidate();
                this.axMap1.Update();
                this.axMap1.Refresh();
                this.toolStripProgressBar1.Value = 0;
            }
            backgroundworker.RunWorkerAsync();
        }

        private void TreeViewLegend_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        string[] LegendLabels
        {
            get
            {
                List<string> Labels = new List<string>();
                foreach (System.Windows.Forms.TreeNode node in TreeViewLegend.Nodes)
                {
                    Labels.Add(node.Text);
                }
                return Labels.ToArray();
            }
        }

        void UpdateLabels(string[] NewLabels)
        {
            for (int label = 0; label < this.TreeViewLegend.Nodes.Count; label++)
            {
                TreeViewLegend.Nodes[label].Text = NewLabels[label];
            }
        }

        private void TreeViewLegend_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            FrmRelable f = new FrmRelable(Cursor.Position, LegendLabels, true);

            f.ShowDialog();

            UpdateLabels(f.NewLabels);
        }

        void RemoveLayer(object sender, EventArgs e)
        {
            axMap1.RemoveLayer(((TreeNodeFile)this.treeViewLayers.SelectedNode).Layerhandle);

            this.treeViewLayers.Nodes.Remove(this.treeViewLayers.SelectedNode);
        }

        private void treeViewLayers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                new ContextMenuStrip(new ToolStripItem[] { new ToolStripMenuItem(RemoveLayer, "Remove layer") }).Show(this.treeViewLayers, e.Location);
            }
        }

        private void treeViewLayers_MouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (e.Node != null)
            {
               treeViewLayers.SelectedNode = e.Node;
            }
            
        }

        

       
        
      
      
          
    }
}
