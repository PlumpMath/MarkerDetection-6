using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class MatrixNegate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MatrixNegate class.
        /// </summary>
        public MatrixNegate()
          : base("MatrixNegate", "MatrixNegate",
              "Negates the specified matrix by multiplying all its values by -1.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The matrix to negate.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The negated matrix.", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Matrix rMatrix = new Matrix(4, 4);
            if (!DA.GetData(0, ref rMatrix)) return;
            Matrix4x4 sMatrix = Utils.ConvertFromRhinoMatrixToSystemMatrix(rMatrix);
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(Matrix4x4.Negate(sMatrix)));
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
                return Resources.MatrixNegate;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{4511697d-a2a4-4851-b3a4-18f641382676}"); }
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