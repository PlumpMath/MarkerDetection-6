using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class MatrixMultiply : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MatrixMultiply class.
        /// </summary>
        public MatrixMultiply()
          : base("MatrixMultiply", "MatrixMultiply",
              "REMEMBER,ORDER MATTERS! Returns the matrix that results from multiplying two matrices together.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix1", "M1", "The first matrix.", GH_ParamAccess.item);
            pManager.AddMatrixParameter("Matrix2", "M2", "The second matrix.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The product matrix. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Matrix rMatrix1 = new Matrix(4, 4);
            Matrix rMatrix2 = new Matrix(4, 4);
            if (!DA.GetData(0, ref rMatrix1)) return;
            if (!DA.GetData(1, ref rMatrix2)) return;
            Matrix4x4 sMatrix1 = Utils.ConvertFromRhinoMatrixToSystemMatrix(rMatrix1);
            Matrix4x4 sMatrix2 = Utils.ConvertFromRhinoMatrixToSystemMatrix(rMatrix2);
            Matrix4x4 sMatrix = Matrix4x4.Multiply(sMatrix1, sMatrix2);
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(sMatrix));
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
                return Resources.MatrixMultiply;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{631c6246-c619-41e6-b29c-38eccdf0fb3f}"); }
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