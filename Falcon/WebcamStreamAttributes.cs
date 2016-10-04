using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Falcon;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace Falcon
{
    public class WebcamStreamAttributes : GH_ComponentAttributes
    {
        internal WebcamStreamAttributes(WebcamStream component)
      : base((IGH_Component) component)
         {
        }
        
        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            (this.DocObject as WebcamStream).ShowWebcamForm();
            return base.RespondToMouseDoubleClick(sender, e);
        }
    }
}
