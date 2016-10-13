using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Quaternion = System.Numerics.Quaternion;

namespace Falcon.Matrixs
{
    public class MatrixDecompose : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Matrix4x4Decompose class.
        /// </summary>
        public MatrixDecompose()
          : base("MatrixDecompose", "Decompose",
              "Attempts to extract the scale, translation, and rotation components from the given scale, rotation, or translation matrix. ",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The matrix to decompose.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Scale", "S","When this method returns, contains the scaling component of the transformation matrix if the operation succeeded.",GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotation", "Q", "When this method returns, contains the rotation component of the transformation matrix if the operation succeeded.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Translation", "V", "When the method returns, contains the translation component of the transformation matrix if the operation succeeded.", GH_ParamAccess.item);
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
            sMatrix = Matrix4x4.Transpose(sMatrix);
            Vector3 S, T;
            Quaternion Q;
            bool success = Matrix4x4.Decompose(sMatrix, out S, out Q, out T);
            if (success)
            {
                Vector3d rS = new Vector3d(S.X,S.Y,S.Z);
                Vector3d rT = new Vector3d(T.X,T.Y,T.Z);
                DA.SetData(0, rS);
                DA.SetData(1, Q);
                DA.SetData(2, rT);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Decompose of the Matrix is not successful.");
            }

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
                return Resources.MatrixDecompose;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b4908182-e875-41ce-a905-e8a3bf65dbdd}"); }
        }

    }
}