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
        MapWinGIS.GridColorScheme GridColorscheme;

        private int MapMin = int.MaxValue;
        private int MapMax = int.MinValue;
        IColorScheme Colorscheme;
        BackgroundWorker backgroundworker;
        private List<TreeNode> ImageFilesToLoad = new List<TreeNode>();

        static int get_Year(System.Windows.Forms.TreeNode node)
        {
            int year = -1;
            if (int.TryParse(System.Text.RegularExpressions.Regex.Match(((string[])node.Tag)[0].ToString(), @"\d+").Value, out year))
            {
                return year;
            }
            return year;
        }

        public FrmMap(DragEventHandler DragDrop)
        {
            
            InitializeComponent();


            backgroundworker = new BackgroundWorker();
            backgroundworker.RunWorkerCompleted += LoadGridFiles;

            backgroundworker.DoWork += new DoWorkEventHandler(SleepWorker);
            backgroundworker.RunWorkerAsync();

            this.DragDrop += DragDrop;
            TreeViewLegend.ImageList = new ImageList();
            this.treeViewLayers.TreeViewNodeSorter = new NodeSorter(get_Year);
             
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
        

        public void LoadImageFile(TreeNode node)
        {

            ImageFilesToLoad.Add(node);

        }
        private static IColorScheme GetColorScheme(int Min, int Max)
        {
            int BinWidth = Max - Min;

            if (Max - Min == 3)
            {
                return new ColorScheme(new System.Drawing.Color[] { System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Red, System.Drawing.Color.Green });
            }
            else if (BinWidth == 1)
            {
                return new ColorScheme();
            }
            else
            {
                return new ColorSchemeClassified(new byte[] { 0, 255 }, new byte[] { 0, 255 }, new byte[] { 255, 255 }, 10);
            }
        }
        private int GetBinWidth(int MapMin, int MapMax, byte colorcount)
        {
            int Range = MapMax - MapMin;
            if (Range > Colorscheme.ColorCount) return (int)(Range / colorcount);
            else return 1;
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
        
        private void SetLayerSelection(string FileName)
        {
            int LayerHandle = int.Parse(((string[])treeViewLayers.Nodes[FileName].Tag)[1]);
            
            axMap1.set_LayerVisible(LayerHandle, true);

            for (int layer = 0; layer < axMap1.NumLayers; layer++)
            {
                int handle = axMap1.get_LayerHandle(layer);

                if (handle != LayerHandle)
                {
                    axMap1.set_LayerVisible(handle, false);
                }

            }
            foreach (System.Windows.Forms.TreeNode _node in treeViewLayers.Nodes)
            {
                if (_node.Name == FileName) _node.Checked = true;
                else _node.Checked = false;
            }
        
        }
       
        private void PlayAnimation(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.TreeNode tree_node in this.treeViewLayers.Nodes)
            {
                SetLayerSelection(tree_node.ToolTipText);
                

                axMap1.Invalidate();
                axMap1.Refresh();

                treeViewLayers.Invalidate();
                treeViewLayers.Refresh();

                System.Threading.Thread.Sleep(2000);
            }
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
                    PlayAnimation(this, new EventArgs());
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
                SetLayerSelection(e.Node.Name);
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
                string FileName = ImageFilesToLoad[0].Tag.ToString();
                ImageFilesToLoad.RemoveAt(0);
                if (PathIsNetworkPath(FileName))
                {
                    string NewFileName = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetFileName(FileName));
                    
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
                    System.IO.File.Copy(FileName, NewFileName);
                    FileName = NewFileName;
                }
                
                if (treeViewLayers.Nodes.ContainsKey(FileName) == true)
                {
                    continue;
                }

                
                MapWinGIS.Grid grid = new MapWinGIS.Grid();


                statusStrip1.Refresh();

                 
                grid.Open(FileName, MapWinGIS.GridDataType.LongDataType, true, MapWinGIS.GridFileType.UseExtension, this);


                MapMin = Math.Min(0, Math.Min(MapMin, int.Parse(grid.Minimum.ToString())));
                MapMax = Math.Max(MapMax, int.Parse(grid.Maximum.ToString()));

                Colorscheme = GetColorScheme(MapMin, MapMax);

                int BinWidth = GetBinWidth(MapMin, MapMax, (byte)Colorscheme.ColorCount);

                GridColorscheme = GetGridScheme(MapMin - 1, MapMax - 1, BinWidth, Colorscheme);

                toolStripStatusLabel1.Text = "Creating image " + System.IO.Path.GetFileName(FileName);
                statusStrip1.Refresh();

                MapWinGIS.Image map_image = new MapWinGIS.UtilsClass().GridToImage(grid, GridColorscheme, this);

                toolStripStatusLabel1.Text = "Ready";

                map_image.CustomColorScheme = GridColorscheme;

                 
                grid.Close();

                int LayerHandle = axMap1.AddLayer(map_image, true);

                System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode(System.IO.Path.GetFileName(FileName));
                node.Name = FileName;
                node.ToolTipText = FileName;
                node.Tag = new string[] { new OutputFileMap(System.IO.Path.GetFileName(FileName)).Year.ToString(), LayerHandle.ToString() };

                treeViewLayers.Nodes.Add(node);

                /*
                for (int index = 0; index < treeViewLayers.Nodes.Count; index++)
                {
                    if (int.Parse(((string[])treeViewLayers.Nodes[index].Tag)[0].ToString()) > int.Parse(((string[])node.Tag)[0].ToString()))
                    {
                        treeViewLayers.Nodes.Insert(index, node);
                        break;
                    }

                }
                if (treeViewLayers.Nodes.ContainsKey(node.Name) == false)
                {
                    
                }
                 */
                axMap1.set_LayerName(LayerHandle, node.Name);

                axMap1.SetImageLayerColorScheme(LayerHandle, GridColorscheme);

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

                    System.Windows.Forms.TreeNode legend_node = new System.Windows.Forms.TreeNode();

                    legend_node.Text = colorbreak.LowValue.ToString() + "-" + colorbreak.HighValue.ToString();

                    legend_node.ToolTipText = "";

                    string ImageKey = colorbreak.LowValue.ToString() + "-" + colorbreak.HighValue.ToString();
                    if (TreeViewLegend.ImageList.Images.ContainsKey(ImageKey) == false)
                    {
                        TreeViewLegend.ImageList.Images.Add(ImageKey, NodeRect(this.TreeViewLegend.Font, Color.UIntToColor(colorbreak.HighColor)));
                    }
                    legend_node.Tag = legend_node.SelectedImageKey = legend_node.ImageKey = ImageKey;

                    this.TreeViewLegend.Nodes.Add(legend_node);

                }
                SetLayerSelection(FileName);

                

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
                foreach (TreeNode node in TreeViewLegend.Nodes)
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
    }
}
