using System;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class DeconstructQuaternion : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DeconstructQuaternion class.
        /// </summary>
        public DeconstructQuaternion()
          : base("DeconstructQuaternion", "XYZW_Quaternion",
              "Deconstruct Quaternion",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The Quaternion to deconstruct", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("X", "X", "X", GH_ParamAccess.item);
            pManager.AddNumberParameter("Y", "Y", "Y", GH_ParamAccess.item);
            pManager.AddNumberParameter("Z", "Z", "Z", GH_ParamAccess.item);
            pManager.AddNumberParameter("W", "W", "W", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            System.Numerics.Quaternion sQua = new System.Numerics.Quaternion();
            DA.GetData(0, ref sQua);
            var x = (double)sQua.X;
            var y = (double)sQua.Y;
            var z = (double)sQua.Z;
            var w = (double)sQua.W;

            DA.SetData(0, x);
            DA.SetData(1, y);
            DA.SetData(2, z);
            DA.SetData(3, w);
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
                return Resources.DeconstructQuaternion;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{19f87f80-ea69-4333-b07e-c820bded69d9}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
    }
}