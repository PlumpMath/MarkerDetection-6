using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;

namespace Falcon.Matrixs
{
    public class MatrixIdentity : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MatrixIdentity class.
        /// </summary>
        public MatrixIdentity()
          : base("MatrixIdentity", "MatrixIdentity",
              "The multiplicative identity matrix.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The multiplicative identity matrix. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(Matrix4x4.Identity));
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
                return Resources.MatrixIdentity;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{fbffd376-765b-492e-b51f-7bd0caf6732b}"); }
        }
    }
}