using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionConcatenate : GH_Component
    {

        public QuaternionConcatenate()
          : base("QuaternionConcatenate", "Concatenate",
              "Concatenates two quaternions.",
              "Falcon", "Quaternion")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion1", "Q1", "The first quaternion rotation in the series.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion2", "Q2", "The second quaternion rotation in the series.", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "A new quaternion representing the concatenation of the Quaternion1 rotation followed by the Quaternion2 rotation.", GH_ParamAccess.item);

        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            if (!DA.GetData<Quaternion>(0, ref a)) return;
            if (!DA.GetData<Quaternion>(1, ref b)) return;
            DA.SetData(0, Quaternion.Concatenate(a, b));
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
                return Resources.QuaternionConcatenate;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{92d4a9f7-ff6d-48b2-be48-1c5af2d2f365}"); }
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