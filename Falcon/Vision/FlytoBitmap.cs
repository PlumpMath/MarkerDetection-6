using System;
using Falcon.Properties;
using Firefly_Bridge;
using Grasshopper.Kernel;

namespace Falcon.Vision
{
    public class FlytoBitmap : GH_Component
    {

        public FlytoBitmap()
          : base("FlytoBitmap", "FlytoBitmap",
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
            pManager.AddGenericParameter("Bitmap", "B", "the standard system bitmap.", GH_ParamAccess.item);
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
                return Resources.FlytoBitmap;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{f1d9cb8b-51bc-48e0-8848-5ded7b90f8bc}"); }
        }
    }
}