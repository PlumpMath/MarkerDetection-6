using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionInverse : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuaternionInverse class.
        /// </summary>
        public QuaternionInverse()
          : base("QuaternionInverse", "Inverse",
              "Returns the inverse of a quaternion.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The quaternion to invert.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The inverted quaternion. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            if (!DA.GetData<Quaternion>(0, ref a)) return;

            DA.SetData(0, Quaternion.Inverse(a));
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
                return Resources.QuaternionInverse;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{41354da1-098e-4f9c-8365-9a5168889f0c}"); }
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