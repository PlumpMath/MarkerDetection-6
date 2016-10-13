using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class MatrixScale : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MatrixScale class.
        /// </summary>
        public MatrixScale()
          : base("MatrixScale", "MatrixScale",
              "Returns the matrix that results from scaling all the elements of a specified matrix by a scalar factor.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The matrix to scale.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Scale", "S", "The scaling value to use.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The scaled matrix. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Matrix rMatrix1 = new Matrix(4, 4);
            double c = 0;
            if (!DA.GetData(0, ref rMatrix1)) return;
            if (!DA.GetData(1, ref c)) return;
            Matrix4x4 sMatrix1 = Utils.ConvertFromRhinoMatrixToSystemMatrix(rMatrix1);
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(Matrix4x4.Multiply(sMatrix1, (float)c)));
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
                return Resources.MatrixScale;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{a63e809d-907a-4353-88fa-5a5080002203}"); }
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