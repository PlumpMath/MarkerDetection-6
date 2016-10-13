using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionDivide : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuaternionDivide class.
        /// </summary>
        public QuaternionDivide()
          : base("QuaternionDivide", "Divide",
              "Divides one quaternion by a second quaternion.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion1", "Q1", "The dividend quaternion.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion2", "Q2", "The divisor quaternion.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The quaternion that results from dividing Quaternion1 by Quaternion2. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            if (!DA.GetData<Quaternion>(0, ref a)) return;
            if (!DA.GetData<Quaternion>(1, ref b)) return;
            DA.SetData(0, Quaternion.Divide(a, b));
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
                return Resources.QuaternionDivide;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{4a6fdf05-e256-49ea-b871-247609e1cad8}"); }
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