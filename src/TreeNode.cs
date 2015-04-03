﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public class TreeNode : System.Windows.Forms.TreeNode, ICloneable
    {
         
        public delegate TreeNode[] GetSubNodes(TreeNode me);

        GetSubNodes get_sub_nodes;

        public TreeNode Clone(int LayerHandle)
        {
            TreeNode Clone = new TreeNode(this.FullPath, this.Text, this.ImageKey, this.get_sub_nodes);

            Clone.LayerHandle = LayerHandle;

            return Clone;
        }
        public string FileName { get; private set; }

        public string FullPath { get; private set; }
        
        public int LayerHandle { get; private set; }

        
        public int Year { get; private set; }

        public void RunSimulation()
        {
            System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetDirectoryName(FullPath));

            Directory.DeleteDirectory("output");

            System.Diagnostics.Process simulation = new System.Diagnostics.Process();

            simulation.StartInfo.FileName = @"C:\Program Files\LANDIS-II\v6\bin\Landis.Console-6.0.exe";

            if (System.IO.File.Exists(simulation.StartInfo.FileName) == false)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Select your landis console executable";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FrmMain.LandisConsoleExe = dlg.FileName;
                    RunSimulation();
                }
                else return;
            }

            simulation.StartInfo.Arguments = "\"" + FullPath + "\"";

            simulation.Start();

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

        public TreeNode(string FullPath, string Text, string ImageKey, GetSubNodes get_sub_nodes)
            
        {
            this.FullPath = FullPath;
            this.FileName = System.IO.Path.GetFileName(FullPath);
            this.Year = get_Year(FileName);
            this.get_sub_nodes = get_sub_nodes;
            this.Tag = FullPath;
            this.Name = FullPath;
            this.ToolTipText = FullPath;
            this.Text = Text;
            this.ImageKey = this.SelectedImageKey = ImageKey;

            if (get_sub_nodes != null)
            {
                foreach (TreeNode node in get_sub_nodes(this))
                { 
                    Nodes.Add(node);
                }
            }
           
          
        }


    }
}
