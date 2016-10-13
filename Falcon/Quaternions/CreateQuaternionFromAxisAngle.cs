using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Quaternions
{
    public class CreateQuaternionFromAxisAngle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateQuaternionFromAxisAngle class.
        /// </summary>
        public CreateQuaternionFromAxisAngle()
          : base("CreateQuaternionFromAxisAngle", "Quaternion_Axis_Angle",
              "Creates a quaternion from a vector and an angle to rotate about the vector.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("AxisVector", "V", "The vector to rotate around.", GH_ParamAccess.item, Vector3d.ZAxis);
            pManager.AddNumberParameter("RotationAngle", "R", "The angle, in RADIANS, to rotate around the vector.",
                GH_ParamAccess.item,0.0);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
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
            Vector3d v = Vector3d.Unset;
            double a = 0.0;
            DA.GetData(0, ref v);
            DA.GetData(1, ref a);
            v.Unitize();
            Vector3 sv = new Vector3((float)v.X, (float)v.Y, (float)v.Z);
            float sa = (float) a;
            var Qua = System.Numerics.Quaternion.CreateFromAxisAngle(sv, sa);
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
                return Resources.CreateQuaternionFromAxisAngle;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{c4097e7d-2313-4750-b29f-6f6f0d890415}"); }
        }
    }
}