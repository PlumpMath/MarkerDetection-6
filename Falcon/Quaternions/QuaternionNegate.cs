using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionNegate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuaternionNegate class.
        /// </summary>
        public QuaternionNegate()
          : base("QuaternionNegate", "Negate",
              "Reverses the sign of each component of the quaternion.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The quaternion to negate.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The negated quaternion.", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            if (!DA.GetData<Quaternion>(0, ref a)) return;

            DA.SetData(0, Quaternion.Negate(a));
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
                return Resources.QuaternionNegate;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{27fc17f8-899e-4126-95bd-5a4c5c1fbe91}"); }
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