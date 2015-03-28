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

        public delegate void UpdateCurvelabels(List<string[]> LabelsFromTo);

        UpdateCurvelabels update_curvelabels;

        public TabPageWithGraph(string Name, UpdateCurvelabels update_curvelabels)
            : base(Name)
        {
            this.update_curvelabels = update_curvelabels;

            InitializeComponent();
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
            return max_value + 0.01F * Range;
        }
        double Min_Axis(double Range, double min_value)
        {
            return min_value - 0.01F * Range;
        }
        public void UpdateLegend(List<string[]> LabelsFromTo)
        {
            foreach (CurveItem curve in Graph1.GraphPane.CurveList)
            {
                for (int label = 0; label < LabelsFromTo.Count; label++)
                {
                    if (curve.Label.Text == LabelsFromTo[label][0])
                    {
                        Graph1.GraphPane.CurveList[label].Label.Text = LabelsFromTo[label][1];
                    }
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

            Graph1.AxisChange();

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
            this.Graph1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Graph1_MouseDoubleClick);
            // 
            // TabPageWithGraph
            // 
            this.Controls.Add(this.Graph1);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResumeLayout(false);

        }

        private void Graph1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Drawing.RectangleF rect =  this.Graph1.GraphPane.Legend.Rect;

            if (rect.Contains(e.X, e.Y))
            {
                List<string[]> LabelsFromTo = new List<string[]>();

                foreach (CurveItem curve in Graph1.GraphPane.CurveList)
                {
                    LabelsFromTo.Add(new string[] { curve.Label.Text, String.Empty });
                }

                FrmRelableGraph frg = new FrmRelableGraph(Cursor.Position, Graph1.GraphPane.CurveList, () => this.Graph1.Refresh());

                frg.Location = this.Graph1.Location;

                frg.ShowDialog();

                for (int curve =0; curve < Graph1.GraphPane.CurveList.Count; curve++)
                {
                    LabelsFromTo[curve][1] = Graph1.GraphPane.CurveList[curve].Label.Text;
                }


                if (frg.ImplementForAllGraphs)
                {
                    update_curvelabels(new List<string[]>(LabelsFromTo.Where(o => o[0] != o[1])));
                }

              

            }

             
        }

         

       
      
    }
     
}
