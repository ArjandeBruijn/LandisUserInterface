using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;

namespace LandisUserInterface
{
    public class TabPageWithGraph : System.Windows.Forms.TabPage
    {
        private ZedGraphControl Graph1;
        private System.ComponentModel.IContainer components;

        public ColorScheme Colorscheme = new ColorScheme(ColorScheme.DefaultColorValues);
         

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
        public string[] Get_FileContent()
        {
            List<string> FileContent = new List<string>();

            string Hdr = "Time\t";

            foreach (string hdr in Get_Labels())
            {
                Hdr += hdr + "\t";
            }

            FileContent.Add(Hdr);

            for (int p = 0; p < NrOfPoints(); p++)
            {
                string line = String.Empty;
                foreach (string hdr in Get_Labels())
                {
                    if (p <= GetCurve(hdr).Points.Count)
                    {
                        if (line.Length == 0)
                        {
                            line += GetCurve(hdr).Points[p].X + "\t";
                        }
                        line += GetCurve(hdr).Points[p].Y + "\t";
                    }
                    else line += "\t";
                }
                FileContent.Add(line);
            }

            return FileContent.ToArray();
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
        public void AddCurve(ZedGraph.LineItem curve)
        {
            Graph1.TabIndex = 6;

            GraphPane myPane = Graph1.GraphPane;

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
        
 
        public TabPageWithGraph(string Name)
            : base(Name)
        {

            InitializeComponent();
            this.Name = Name;

        }

        private void InitializeComponent()
        {
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.components = new System.ComponentModel.Container();
            this.Graph1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // Graph1
            // 
            this.Graph1.Location = new System.Drawing.Point(0, 0);
            this.Graph1.Name = "Graph1";
            this.Graph1.ScrollGrace = 0D;
            this.Graph1.ScrollMaxX = 0D;
            this.Graph1.ScrollMaxY = 0D;
            this.Graph1.ScrollMaxY2 = 0D;
            this.Graph1.ScrollMinX = 0D;
            this.Graph1.ScrollMinY = 0D;
            this.Graph1.ScrollMinY2 = 0D;
            this.Graph1.Size = new System.Drawing.Size(150, 150);
            this.Graph1.TabIndex = 0;

            
            Graph1.GraphPane.Title.Text = null;
            Graph1.GraphPane.XAxis.Title.Text = null;
            Graph1.GraphPane.YAxis.Title.Text = "";

           
            Graph1.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            Controls.Add(Graph1);


            this.ResumeLayout(false);

        }
    }
     
}
