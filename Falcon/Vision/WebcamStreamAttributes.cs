using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace Falcon.Vision
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
