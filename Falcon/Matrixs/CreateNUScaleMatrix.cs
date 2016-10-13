using System;
using System.Collections.Generic;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class CreateNUScaleMatrix : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateNUScaleMatrix class.
        /// </summary>
        public CreateNUScaleMatrix()
          : base("CreateNUScaleMatrix", "CreateNUScaleMatrix",
              "Creates a non-uniform scaling matrix that is offset by a given center point.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Center", "C", "Center point of scaling", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddNumberParameter("XScale", "X", "The value to scale by on the X axis.", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("YScale", "Y", "The value to scale by on the Y axis.", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("ZScale", "Z", "The value to scale by on the Z axis.", GH_ParamAccess.item, 1);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The scaling matrix.", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d p = Point3d.Origin;
            if (!DA.GetData(0, ref p)) return;
            Vector3 sp = Utils.RhinoVectoSystemVec((Vector3d)p);
            double x = 1, y = 1, z = 1;
            if (!DA.GetData(1, ref x)) return;
            if (!DA.GetData(2, ref y)) return;
            if (!DA.GetData(3, ref z)) return;
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(Matrix4x4.Transpose(Matrix4x4.CreateScale((float)x, (float)y, (float)z, sp))));
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
                return Resources.CreateNUScaleMatrix;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{92985d18-49d9-4f82-9264-8eae091180b2}"); }
        }
    }
}