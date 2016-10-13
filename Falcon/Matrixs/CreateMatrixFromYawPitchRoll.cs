using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;

namespace Falcon.Matrixs
{
    public class CreateMatrixFromYawPitchRoll : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateMatrix4x4FromYawPitchRoll class.
        /// </summary>
        public CreateMatrixFromYawPitchRoll()
          : base("CreateMatrixFromYawPitchRoll", "CreateMatrixFromYawPitchRoll",
              "Creates a rotation matrix from the specified yaw, pitch, and roll.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Yaw", "Y", "The yaw angle, in radians, around the Y axis.", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Pitch", "P", "The pitch angle, in radians, around the X axis.", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Roll", "R", "The roll angle, in radians, around the Z axis.", GH_ParamAccess.item, 0.0);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The rotation matrix. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double x = 0, y = 0, z = 0;
            DA.GetData(0, ref x);
            DA.GetData(1, ref y);
            DA.GetData(2, ref z);
            var sMatrix = Matrix4x4.CreateFromYawPitchRoll((float)x, (float)y, (float)z);
            sMatrix = Matrix4x4.Transpose(sMatrix);
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
                return Resources.CreateMatrixFromYawPitchRoll;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{bcb93757-ca25-4487-a476-954b228b607b}"); }
        }
    }
}