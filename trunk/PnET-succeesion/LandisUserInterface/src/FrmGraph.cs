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
        static int label_counter = 0;
         
        public FrmGraph()
        {
            InitializeComponent();
        
        }
         
        public void LoadFile(string FileName)
        {
            try
            {
                string label = (label_counter++).ToString();

                string[] FileContent = System.IO.File.ReadAllLines(FileName);

                string[] headers = FileContent[0].Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, ZedGraph.LineItem> CurveCollection = new Dictionary<string, ZedGraph.LineItem>();

                foreach (string hdr in headers)
                {
                    CurveCollection.Add(hdr, new ZedGraph.LineItem(label, null, System.Drawing.Color.Black, ZedGraph.SymbolType.None));
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
                        tabControl1.TabPages.Add(new TabPageWithGraph(curve.Key, UpdateCurveLabels));
                    }
                    ((TabPageWithGraph)tabControl1.TabPages[curve.Key]).AddCurve(curve.Value);

                }
            }
            catch
            {
                return;
            }
        }
        void UpdateCurveLabels(List<string[]> LabelsFromTo)
        {
            for (int i = 0; i < tabControl1.TabPages.Count;i++ )
            {
                ((TabPageWithGraph)tabControl1.TabPages[i]).UpdateLegend(LabelsFromTo);
            }
        }
          
        private void FrmGraph_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }
        
        

        
    }
}
