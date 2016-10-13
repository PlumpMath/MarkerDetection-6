using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class MatrixLerp : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MatrixLerp class.
        /// </summary>
        public MatrixLerp()
          : base("MatrixLerp", "MatrixLerp",
              "Performs a linear interpolation from one matrix to a second matrix based on a value that specifies the weighting of the second matrix.",
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
            pManager.AddNumberParameter("Amount", "A", "The relative weight of matrix2 in the interpolation.",
                GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The interpolated matrix.", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Matrix rMatrix1 = new Matrix(4, 4);
            Matrix rMatrix2 = new Matrix(4, 4);
            double c = 0;
            if (!DA.GetData(0, ref rMatrix1)) return;
            if (!DA.GetData(1, ref rMatrix2)) return;
            if (!DA.GetData(2, ref c)) return;
            Matrix4x4 sMatrix1 = Utils.ConvertFromRhinoMatrixToSystemMatrix(rMatrix1);
            Matrix4x4 sMatrix2 = Utils.ConvertFromRhinoMatrixToSystemMatrix(rMatrix2);
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(Matrix4x4.Lerp(sMatrix1,sMatrix2,(float)c)));
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
                return Resources.MatrixLerp;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{04307c94-b26d-42c3-b629-12bff55fdfd3}"); }
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