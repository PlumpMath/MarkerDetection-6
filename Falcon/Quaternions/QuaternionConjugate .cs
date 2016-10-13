using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionConjugate : GH_Component
    {
        public QuaternionConjugate()
          : base("QuaternionConjugate", "Conjugate",
              "	Returns the conjugate of a specified quaternion.",
              "Falcon", "Quaternion")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The quaternion.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "A new quaternion that is the conjugate of the input. ", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            if (!DA.GetData<Quaternion>(0, ref a)) return;

            DA.SetData(0, Quaternion.Conjugate(a));
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.QuaternionConjugate;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{9cb93729-3038-41d5-97e7-5705480399ee}"); }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
    }
}