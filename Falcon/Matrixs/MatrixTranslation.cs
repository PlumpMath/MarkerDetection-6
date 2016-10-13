using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class MatrixTranslation : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MatrixTranslation class.
        /// </summary>
        public MatrixTranslation()
          : base("MatrixTranslation", "MatrixTranslation",
              "Gets the translation component of this matrix.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The matrix.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddVectorParameter("Translation", "V", "The translation component of the current instance. ",GH_ParamAccess.item);
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
            Vector3 vector3 = sMatrix.Translation;
            Vector3d vector3D = new Vector3d(vector3.X,vector3.Y,vector3.Z);
            DA.SetData(0, vector3D);
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
                return Resources.MatrixTranslation;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{450fc74a-3627-4a2b-8518-cdfe249fa992}"); }
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