using System;
using System.Collections.Generic;
using Falcon.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Firefly_Bridge;

namespace Falcon
{
    public class FtoBitmap : GH_Component
    {

        public FtoBitmap()
          : base("FtoBitmap", "FtoBitmap",
              "convert Firefly image into standard system bitmap",
              "Falcon", "Vision")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Firefly Image", "F", "the firefly image to be converted", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutputImage", "O", "the standard output image data", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Firefly_Bitmap fireflyImage = new Firefly_Bitmap();
            DA.GetData<Firefly_Bitmap>(0, ref fireflyImage);
            DA.SetData(0, (object)fireflyImage.Wbitmap);
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.ftoimg;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{f1d9cb8b-51bc-48e0-8848-5ded7b90f8bc}"); }
        }
    }
}