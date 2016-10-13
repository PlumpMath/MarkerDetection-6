using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;

namespace Falcon.Matrixs
{
    public class CreateMatrixFromQuaternion : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateMatrixFromQuaternion class.
        /// </summary>
        public CreateMatrixFromQuaternion()
          : base("CreateMatrixFromQuaternion", "Matrix_Quaternion",
              "Creates a rotation matrix from the specified Quaternion rotation value.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The source Quaternion.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The rotation matrix.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var sQua = new System.Numerics.Quaternion();
            DA.GetData(0, ref sQua);

            // create left-hand matrix
            System.Numerics.Matrix4x4 sMatrix4X4 = Matrix4x4.CreateFromQuaternion(sQua);
            // create right-hand matrix
            sMatrix4X4 = Matrix4x4.Transpose(sMatrix4X4);
          
            // transfer to rhino matrix
            Rhino.Geometry.Matrix rMatrix = Utils.ConvertFromSystemMatrixToRhinoMatrix(sMatrix4X4);

            DA.SetData(0, rMatrix);
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
                return Resources.CreateMatrixFromQuaternion;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b8cd39cc-eb46-4d23-9da5-2112220e60ef}"); }
        }
    }
}