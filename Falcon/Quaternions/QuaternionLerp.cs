using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionLerp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuaternionLerp class.
        /// </summary>
        public QuaternionLerp()
          : base("QuaternionLerp", "Lerp",
              "Performs a linear interpolation between two quaternions based on a value that specifies the weighting of the second quaternion.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion1", "Q1", "The first quaternion.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion2", "Q2", "The second quaternion.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Amount", "A", "The relative weight of quaternion2 in the interpolation.",
                GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The interpolated quaternion. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            double c = 0;
            if (!DA.GetData<Quaternion>(0, ref a)) return;
            if (!DA.GetData<Quaternion>(1, ref b)) return;
            if (!DA.GetData(2, ref c)) return;
            DA.SetData(0, Quaternion.Lerp(a, b,(float)c));
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
                return Resources.QuaternionLerp;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{893b9bff-3e67-4868-8c43-44e9d0366de6}"); }
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