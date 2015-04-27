using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
 
using System.Runtime.InteropServices;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using unvell.ReoGrid;

namespace LandisUserInterface
{
    public partial class FrmGrid : Form
    {
      
        public FrmGrid(string FileName)
        {
            this.Text = FileName;

            InitializeComponent();

            LoadFile(FileName);
        }

        public void LoadFile(string FileName)
        {
            try
            {
                string[] FileContent = System.IO.File.ReadAllLines(FileName);

                reoGridControl1.InsertRows(0, FileContent.Count());

                for (int row = 0; row < FileContent.Count(); row++)
                {
                    string[] terms = FileContent[row].Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    while (reoGridControl1.ColCount < terms.Count()) reoGridControl1.InsertCols(reoGridControl1.ColCount - 1, 1);

                    for (int col = 0; col < terms.Count(); col++)
                    {
                        reoGridControl1.SetCellData(row, col, terms[col]);
                    }
                }
            }
            catch (System.Exception e)
            {
                reoGridControl1.InsertRows(0, 1);
                reoGridControl1.SetCellData(0, 0, e.Message);
                return;
            }
        }
    }
}
