using System;
using Falcon.Properties;
using Grasshopper.Kernel;
using Quaternion = System.Numerics.Quaternion;

namespace Falcon.Quaternions
{
    public class CreateQuaternionFromXYZW : GH_Component
    {

        public CreateQuaternionFromXYZW()
          : base("CreateQuaternionFromXYZW", "Quaternion_XYZW",
              "Creat a quaternion by it's x,y,z,w value",
              "Falcon", "Quaternion")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("X", "X", "X", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Y", "Y", "Y", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Z", "Z", "Z", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("W", "W", "W", GH_ParamAccess.item, 1.0);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;

        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The newly created quaternion.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double x = 0, y = 0, z = 0, w = 1;
            DA.GetData(0, ref x);
            DA.GetData(1, ref y);
            DA.GetData(2, ref z);
            DA.GetData(3, ref w);

            var Qua = new Quaternion((float)x, (float)y, (float)z, (float)w);
            
            DA.SetData(0, Qua);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.CreateQuaternionFromXYZW;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{07df8038-0992-4124-bc3e-3081807d89a4}"); }
        }
    }
}
