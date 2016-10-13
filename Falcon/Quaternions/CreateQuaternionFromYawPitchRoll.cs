using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class CreateQuaternionFromYawPitchRoll : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateFromYawPitchRoll class.
        /// </summary>
        public CreateQuaternionFromYawPitchRoll()
          : base("CreateQuaternionFromYawPitchRoll", "CreateQuaternionFromYawPitchRoll",
              "Creates a new quaternion from the given yaw, pitch, and roll.",
              "Falcon", "Quaternion")
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
            pManager.AddGenericParameter("Quaternion", "Q", "The newly created quaternion.", GH_ParamAccess.item);
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
            var Qua = Quaternion.CreateFromYawPitchRoll((float) x, (float) y, (float) z);
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
                return Resources.CreateQuaternionFromYawPitchRoll;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{5c588a11-b6c0-492c-a842-9c23d0a480e4}"); }
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
    }
}