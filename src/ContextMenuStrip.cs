using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LandisUserInterface
{
    class ContextMenuStrip : System.Windows.Forms.ContextMenuStrip
    {
        public ContextMenuStrip(System.Windows.Forms.ToolStripItem[] ToolStripItems)
        {
            Items.AddRange(ToolStripItems);
            Size = new System.Drawing.Size(166, 48);
        }

    }
}
