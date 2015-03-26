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

        public FrmMap()
        {
            InitializeComponent();
        }
        public void Progress(string s1, int p, string s2)
        {
            toolStripProgressBar1.Value = p;
        }
        public void Error(string s1, string s2)
        {
            toolStripStatusLabel1.Text = s1;

        }
        public static IColorScheme GetColorScheme(int Min, int Max)
        {
            int BinWidth = Max - Min;

            if (Max - Min == 3)
            {
                return new ColorScheme(new System.Drawing.Color[] { System.Drawing.Color.White, System.Drawing.Color.Black, System.Drawing.Color.Red, System.Drawing.Color.Green });
            }
            else if (BinWidth == 1)
            {
                return new ColorScheme(ColorScheme.DefaultColorValues);
            }
            else
            {
                return new ColorSchemeClassified(new byte[] { 0, 255 }, new byte[] { 0, 255 }, new byte[] { 255, 255 }, 10);
            }
        }
        public static int GetBinWidth(int MapMin, int MapMax, byte colorcount)
        {
            int Range = MapMax - MapMin;
            if (Range > ColorScheme.DefaultColorValues.Count()) return (int)(Range / colorcount);
            else return 1;
        }
        public MapWinGIS.GridColorScheme GetGridScheme(int MapMin, int MapMax, int BinWidth, IColorScheme color_scheme)
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
        public void LoadImageFile(string FileName)
        {
            MapWinGIS.Grid grid = new MapWinGIS.Grid();

             
            statusStrip1.Refresh();

            grid.Open(FileName, MapWinGIS.GridDataType.LongDataType, true, MapWinGIS.GridFileType.UseExtension, this);

            
            MapMin = Math.Min(0, Math.Min(MapMin, int.Parse(grid.Minimum.ToString())));
            MapMax = Math.Max(MapMax, int.Parse(grid.Maximum.ToString()));

            IColorScheme Colorscheme = GetColorScheme(MapMin, MapMax);

            int BinWidth = GetBinWidth(MapMin, MapMax, (byte)Colorscheme.ColorCount);

            GridColorscheme = GetGridScheme(MapMin - 1, MapMax - 1, BinWidth, Colorscheme);

            toolStripStatusLabel1.Text = "Creating image " + System.IO.Path.GetFileName(FileName);
            statusStrip1.Refresh();

            MapWinGIS.Image map_image = new MapWinGIS.UtilsClass().GridToImage(grid, GridColorscheme, this);

            toolStripStatusLabel1.Text = "Ready";
 
            map_image.CustomColorScheme = GridColorscheme;

            grid.Close();

            int LayerHandle = axMap1.AddLayer(map_image, true);

            //Image node_image = null;
            //AddNode(new OutputFileMap(FileName).Year, RankedLegendNodes, this.treeViewLayers, LayerName, FileName, LayerName, node_image);

            //Set_LayerName(LayerHandle, LayerName);

            axMap1.SetImageLayerColorScheme(LayerHandle, GridColorscheme);

            axMap1.ZoomToMaxExtents();

            //ApplyColorSchemeToLegend(GridColorscheme);

            string NodeText = System.IO.Path.GetFileNameWithoutExtension(FileName);
            string TipToolText = FileName;

            //SelectLayer(new TreeViewEventArgs(this.treeViewLayers.Nodes[treeViewLayers.Nodes.Count - 1], TreeViewAction.ByMouse));
            //SelectLayerNode(new TreeViewEventArgs(treeViewLayers.Nodes[treeViewLayers.Nodes.Count - 1], TreeViewAction.ByMouse));
            
            this.axMap1.Invalidate();
            this.axMap1.Update();
            this.axMap1.Refresh();

            this.toolStripProgressBar1.Value = 0;
        }

        private void FrmMap_Load(object sender, EventArgs e)
        {

        }
        public void PlayAnimation(object sender, EventArgs e)
        {
            foreach (TreeNode tree_node in this.treeViewLayers.Nodes)
            {
                //SelectLayer(new TreeViewEventArgs(tree_node, TreeViewAction.ByMouse));
                //SelectLayerNode(new TreeViewEventArgs(tree_node, TreeViewAction.ByMouse));

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
         
    }
}
