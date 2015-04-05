using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public class TabPageWithGraph : System.Windows.Forms.TabPage
    {
        private ZedGraphControl Graph1;
        private System.ComponentModel.IContainer components;

        public ColorScheme Colorscheme = new ColorScheme();

        public delegate void UpdateCurvelabels(string[] Labels);

        UpdateCurvelabels update_curvelabels;

        public TabPageWithGraph(string Name, UpdateCurvelabels update_curvelabels)
            : base(Name)
        {
            this.update_curvelabels = update_curvelabels;

            

            InitializeComponent();

            
            
            this.Graph1.GraphPane.XAxis.Scale.Min = double.MaxValue;
            this.Graph1.GraphPane.XAxis.Scale.Max = double.MinValue;
            this.Graph1.GraphPane.YAxis.Scale.Min = double.MaxValue;
            this.Graph1.GraphPane.YAxis.Scale.Max = double.MinValue;

            this.Name = Name;

        }

        public string[] Get_Labels()
        {
            List<string> Labels = new List<string>();
            foreach (LineItem l in Graph1.GraphPane.CurveList)
            {
                Labels.Add(l.Label.Text);
            }
            return Labels.ToArray();
        }
        public int NrOfPoints()
        {
            int nrofpoints = -1;
             
            foreach (ZedGraph.LineItem l in Get_CurveList())
            {
                if (nrofpoints < 0)
                {
                    nrofpoints = l.Points.Count;
                }
            }
            return nrofpoints;
        }
         
       
        public CurveList Get_CurveList()
        {
            return Graph1.GraphPane.CurveList;
        }
        public LineItem GetCurve(string Label)
        {
            foreach (LineItem l in Graph1.GraphPane.CurveList)
            {
                if (l.Label.Text == Label) return l;
            }
            return null;
        }
        
        double Max_Axis(double Range, double max_value)
        {
            return max_value + 0.025F * Range;
        }
        double Min_Axis(double Range, double min_value)
        {
            return min_value - 0.025F * Range;
        }
        public void UpdateLegend(string[] Labels)
        {
            foreach (CurveItem curve in Graph1.GraphPane.CurveList)
            {
                for (int label = 0; label < Labels.Length; label++)
                {
                    curve.Label.Text = Labels[label];
                    this.Graph1.Refresh();
                }
            }

            
        }
        public void AddCurve(ZedGraph.LineItem curve)
        {
            Graph1.TabIndex = 6;

            curve.Color = Colorscheme.NextColor;

            GraphPane myPane = Graph1.GraphPane;

            myPane.Title.Text = String.Empty;
            myPane.YAxis.Title.Text = String.Empty;
            myPane.XAxis.Title.Text = "Year";

            myPane.CurveList.Add(curve);
            curve.Line.Width = 2;
            curve.Label.IsVisible = true;

            

            double Xmin = double.MaxValue;
            double Ymin = double.MaxValue;
            double Xmax = double.MinValue;
            double Ymax = double.MinValue;

            for (int p = 0; p < curve.Points.Count; p++)
            {
                if (curve.Points[p].X < Xmin) Xmin = curve.Points[p].X;
                else if (curve.Points[p].X > Xmax) Xmax = curve.Points[p].X;

                if (curve.Points[p].Y < Ymin) Ymin = curve.Points[p].Y;
                else if (curve.Points[p].Y > Ymax) Ymax = curve.Points[p].Y;
            }


            double range = (Xmax - Xmin);
            if (curve.Points.Count > 0) myPane.XAxis.Scale.Max = Math.Max(myPane.XAxis.Scale.Max, Max_Axis(range, Xmax));
            if (curve.Points.Count > 0) myPane.XAxis.Scale.Min = Math.Min(myPane.XAxis.Scale.Min, Min_Axis(range, Xmin));
            myPane.XAxis.Scale.MajorStep = range / 10;
            myPane.XAxis.Scale.MinorStep = myPane.XAxis.Scale.MajorStep;

            double range2 = (Ymax - Ymin);
            if (curve.Points.Count > 0) myPane.YAxis.Scale.Max = Math.Max(myPane.YAxis.Scale.Max, Max_Axis(range2, Ymax));
            if (curve.Points.Count > 0) myPane.YAxis.Scale.Min = Math.Min(myPane.YAxis.Scale.Min, Min_Axis(range2, Ymin));
            myPane.YAxis.Scale.MajorStep = range2 / 10;
            myPane.YAxis.Scale.MinorStep = myPane.YAxis.Scale.MajorStep;

            Graph1.AxisChange();
            Graph1.Refresh();
        }

      

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Graph1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // Graph1
            // 
            this.Graph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Graph1.Location = new System.Drawing.Point(0, 0);
            this.Graph1.Name = "Graph1";
            this.Graph1.ScrollGrace = 0D;
            this.Graph1.ScrollMaxX = 0D;
            this.Graph1.ScrollMaxY = 0D;
            this.Graph1.ScrollMaxY2 = 0D;
            this.Graph1.ScrollMinX = 0D;
            this.Graph1.ScrollMinY = 0D;
            this.Graph1.ScrollMinY2 = 0D;
            this.Graph1.Size = new System.Drawing.Size(200, 100);
            this.Graph1.TabIndex = 0;
            this.Graph1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Graph1_MouseClick);
            this.Graph1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Graph1_MouseDoubleClick);
            // 
            // TabPageWithGraph
            // 
            this.Controls.Add(this.Graph1);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResumeLayout(false);

            Graph1.IsShowContextMenu = false;
        }
        void UpdateAxisMinMax(string[] AxisMinMax)
        {
            foreach (string line in AxisMinMax)
            {
                if (line.Split('\t')[0].Contains("X_min"))
                {
                    double result;
                    if(double.TryParse( line.Split('\t')[1].Trim(), out result)==true)
                    {
                        Graph1.GraphPane.XAxis.Scale.Min = result;
                    }
                }
                if (line.Split('\t')[0].Contains("X_max"))
                {
                    double result;
                    if (double.TryParse(line.Split('\t')[1].Trim(), out result) == true)
                    {
                        Graph1.GraphPane.XAxis.Scale.Max = result;
                    }
                }
                if (line.Split('\t')[0].Contains("Y_min"))
                {
                    double result;
                    if (double.TryParse(line.Split('\t')[1].Trim(), out result) == true)
                    {
                        Graph1.GraphPane.YAxis.Scale.Min = result;
                    }
                }
                if (line.Split('\t')[0].Contains("Y_max"))
                {
                    double result;
                    if (double.TryParse(line.Split('\t')[1].Trim(), out result) == true)
                    {
                        Graph1.GraphPane.YAxis.Scale.Max = result;
                    }
                }
            }
        }
        void UpdateLabels(string[] NewLabels)
        {
            for (int label = 0; label < this.Graph1.GraphPane.CurveList.Count(); label++)
            {
                Graph1.GraphPane.CurveList[label].Label.Text = NewLabels[label];
            }
            Graph1.Refresh();
        }
        private void Graph1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Drawing.RectangleF rect =  this.Graph1.GraphPane.Legend.Rect;

            if (rect.Contains(e.X, e.Y))
            {
                 

                FrmRelable frg = new FrmRelable(Cursor.Position, Graph1.GraphPane.CurveList.Select(o=> o.Label.Text).ToArray(), false);

                frg.Location = this.Graph1.Location;

                frg.ShowDialog();

                UpdateLabels(frg.NewLabels);

                if (frg.ImplementForAllGraphs)
                {
                    update_curvelabels(frg.NewLabels);
                }
            }
            /*
            if (this.Graph1.GraphPane.Rect.Contains(e.X, e.Y))
            {
                List<string> OldTerms = new List<string>();

                OldTerms.Add("X_min\t" + Graph1.GraphPane.XAxis.Scale.Min);
                OldTerms.Add("X_max\t" + Graph1.GraphPane.XAxis.Scale.Max);
                OldTerms.Add("Y_min\t" + Graph1.GraphPane.YAxis.Scale.Min);
                OldTerms.Add("Y_max\t" + Graph1.GraphPane.YAxis.Scale.Max);

                FrmRelable frg = new FrmRelable(Cursor.Position, OldTerms.ToArray(), false);

                frg.Location = this.Graph1.Location;

                frg.ShowDialog();

                UpdateAxisMinMax(frg.NewLabels);

                
            }
            */
             
        }
        
        void OnExportAsCsvFileClick(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "Comma delimited (.csv)|*.csv|Tab delimited (.txt)|*.txt";
            if (s.ShowDialog() == DialogResult.OK)
            {
                char delimiter = ' ';

                if( System.IO.Path.GetFileNameWithoutExtension(s.FileName)==".txt")delimiter = '\t';
                else if (System.IO.Path.GetFileNameWithoutExtension(s.FileName) == ".csv") delimiter = ',';

                List<string> Content = new List<string>();

                string hdr = "Time" + delimiter;

                Graph1.GraphPane.CurveList.ForEach(o => hdr += o.Label.Text + delimiter);

                Content.Add(hdr);

                List<double> X = new List<double>();

                foreach (CurveItem curve in Graph1.GraphPane.CurveList)
                {
                    for (int i = 0; i < curve.Points.Count; i++)
                    {
                        X.Add(curve.Points[i].X);
                    }
                }

                X = new List<double>(X.OrderBy(o=>o));

                foreach (double x in X)
                {
                    string line = x.ToString() + '\t';

                    foreach (CurveItem curve in Graph1.GraphPane.CurveList)
                    {
                        bool FlagFound = false;
                        for (int i = 0; i < curve.Points.Count; i++)
                        {
                            if (curve.Points[i].X == x)
                            {
                                line += curve.Points[i].Y + delimiter;
                                FlagFound=true;
                                break;
                            }
                        }
                        if (FlagFound == false)
                        {
                            line += delimiter;
                        }
                    }

                    Content.Add(line);
                }
                System.IO.File.WriteAllLines(s.FileName, Content.ToArray());
            }

            
             
        }
        private void Graph1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                new ContextMenuStrip(new ToolStripItem[]{new ToolStripMenuItem(OnExportAsCsvFileClick, "Export as text file")} ).Show(this.Graph1, e.Location);
            }
        }

    }
     
}
