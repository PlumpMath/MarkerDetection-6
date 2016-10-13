using System;
using Falcon.Properties;
using Grasshopper.Kernel;
using Quaternion = System.Numerics.Quaternion;

namespace Falcon.Quaternions
{
    public class QuaternionAdd : GH_Component
    {

        public QuaternionAdd()
          : base("QuaternionAdd", "Add",
              "Adds each element in one quaternion with its corresponding element in a second quaternion.",
              "Falcon", "Quaternion")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion1", "Q1", "The first quaternion.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion2", "Q2", "The second quaternion.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The quaternion that contains the summed values of value1 and value2.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            if (!DA.GetData<Quaternion>(0, ref a)) return;
            if (!DA.GetData<Quaternion>(1, ref b)) return;
            DA.SetData(0, Quaternion.Add(a, b));
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.QuaternionAdd;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("{03e5ab2f-e09b-4568-9749-8e9e5bb96bd0}"); }
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