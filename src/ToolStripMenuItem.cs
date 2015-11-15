using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    class ToolStripMenuItem : System.Windows.Forms.ToolStripMenuItem
    {
        public ToolStripMenuItem(EventHandler eventhandler, string Text)
        {
             
            Size = new System.Drawing.Size(205, 22);
            this.Text = Text;
            Click += new System.EventHandler(eventhandler);
         
        }
    }
}
