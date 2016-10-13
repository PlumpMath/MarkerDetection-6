using System;
using System.Drawing;
using Falcon.Properties;
using Firefly_Bridge;
using Grasshopper.Kernel;

namespace Falcon.Vision
{
    public class BitmapToFly : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BitmapToF class.
        /// </summary>
        public BitmapToFly()
          : base("BitmapToFly", "BitmapToFly",
              "Convert System Bitmap to Firefly image",
              "Falcon", "Vision")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "the system bitmap to convert", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FireflyImage", "F", "the firefly image converted from bitmap",
                GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            Bitmap bmap = null;
            if(!DA.GetData<Bitmap>(0, ref bmap)) return;
            if(bmap==null) return;
            Firefly_Bitmap fireflyImage = new Firefly_Bitmap();
            fireflyImage.SetSize(bmap.Width,bmap.Height);
            var newbmap =  bmap;
            for (int index1 =0; index1 < fireflyImage.Ry; index1++)
            {
                for (int index2 = 0; index2 < fireflyImage.Rx; index2++)
                {
                    Color c = newbmap.GetPixel(index2, fireflyImage.Ry - index1 - 1);
                    fireflyImage.pixels[index2, index1] = new Firefly_Pixel() {A = c.A, B = c.B, G = c.G, R = c.R};
                }
            }
            
            DA.SetData(0, fireflyImage);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.BitmaptoFly;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{01719761-a5be-45b7-9652-d3e41146eae5}"); }
        }
    }
}