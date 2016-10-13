using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class MatrixTransform : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MatrixTransform class.
        /// </summary>
        public MatrixTransform()
          : base("MatrixTransform", "MatrixTransform",
              "Transforms the specified matrix by applying the specified Quaternion rotation.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The matrix to transform.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion", "Q", "The Quaternion to apply", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The transformed matrix.", GH_ParamAccess.item);

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
            System.Numerics.Quaternion sQua = new System.Numerics.Quaternion();
            DA.GetData(1, ref sQua);
            sMatrix = Matrix4x4.Transpose(sMatrix);

            var newSMatrix = Matrix4x4.Transform(sMatrix, sQua);
            newSMatrix = Matrix4x4.Transpose(newSMatrix);
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(newSMatrix));

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
                return Resources.MatrixTransform;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1f6882a3-f3dd-4986-a3a5-85fe61dcefa1}"); }
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