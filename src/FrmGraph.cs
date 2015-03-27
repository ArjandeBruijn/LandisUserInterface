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
    public partial class FrmGraph : Form
    {
        public FrmGraph(string FileName)
        {
            
            this.Text = FileName;

            InitializeComponent();

            LoadFile(FileName);
        }

        public void LoadFile(string FileName)
        {
            string[] FileContent = System.IO.File.ReadAllLines(FileName);

            string[] headers = FileContent[0].Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, ZedGraph.LineItem> CurveCollection = new Dictionary<string,ZedGraph.LineItem>();

            foreach (string hdr in headers)
            {
                CurveCollection.Add(hdr,new ZedGraph.LineItem("test", null, System.Drawing.Color.Black, ZedGraph.SymbolType.None));
            }
             
            for (int row = 1; row < FileContent.Count(); row++)
            {
                string[] terms = FileContent[row].Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                double X;

                if (double.TryParse(terms[0], out X) == false) continue;


                for (int col = 0; col < terms.Count(); col++)
                {
                    double Y;

                    if (double.TryParse(terms[col], out Y) == true)
                    {
                        CurveCollection[headers[col]].AddPoint(X, Y);
                    }
                    

                   
                }
            }
            foreach (KeyValuePair<string, ZedGraph.LineItem> curve in CurveCollection)
            {
                if (tabControl1.TabPages[curve.Key] == null)
                {
                    tabControl1.TabPages.Add(new TabPageWithGraph(curve.Key));
                }
                ((TabPageWithGraph)tabControl1.TabPages[curve.Key]).AddCurve(curve.Value);
            
            }
        }
    }
}
