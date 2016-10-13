using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Quaternions
{
    public class CreateQuaternionFromMatrix : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateQuaternionFromMatrix class.
        /// </summary>
        public CreateQuaternionFromMatrix()
          : base("CreateQuaternionFromMatrix", "Quaternion_Matrix",
              "Creates a quaternion from the specified rotation matrix.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The rotation matrix.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The newly created quaternion.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Matrix rMatrix = new Matrix(4,4);
            DA.GetData(0, ref rMatrix);


            System.Numerics.Matrix4x4 sMatrix4X4 = Utils.ConvertFromRhinoMatrixToSystemMatrix(rMatrix);
            sMatrix4X4 = Matrix4x4.Transpose(sMatrix4X4);
            var Qua = System.Numerics.Quaternion.CreateFromRotationMatrix(sMatrix4X4);
            DA.SetData(0, Qua);

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
                return Resources.CreateQuaternionFromMatrix;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b21daa4e-b64f-4c95-b528-a5b628f282ec}"); }
        }
    }
}