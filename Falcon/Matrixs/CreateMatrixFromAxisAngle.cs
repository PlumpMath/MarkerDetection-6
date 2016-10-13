using System;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class CreateMatrixFromAxisAngle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateMatrixFromAxisAngle class.
        /// </summary>
        public CreateMatrixFromAxisAngle()
          : base("CreateMatrixFromAxisAngle", "CreateMatrixFromAxisAngle",
              "Creates a matrix that rotates around an arbitrary vector.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("AxisVector", "V", "The vector to rotate around.", GH_ParamAccess.item, Vector3d.ZAxis);
            pManager.AddNumberParameter("RotationAngle", "R", "The angle, in RADIANS, to rotate around the vector.",
                GH_ParamAccess.item, 0.0);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
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
            Vector3d v = Vector3d.Unset;
            double a = 0.0;
            DA.GetData(0, ref v);
            DA.GetData(1, ref a);

            v.Unitize();
            Vector3 sv = new Vector3((float)v.X, (float)v.Y, (float)v.Z);
            
            float sa = (float)a;
            Matrix4x4 sMatrix4X4 = Matrix4x4.CreateFromAxisAngle(sv,sa);
            sMatrix4X4 = Matrix4x4.Transpose(sMatrix4X4);
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(sMatrix4X4));
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
                return Resources.CreateMatrixFromAxisAngle;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{c628e539-4ab8-496f-ae2d-ae5a2ac5e8a0}"); }
        }
    }
}