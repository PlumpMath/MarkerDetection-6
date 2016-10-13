using System;
using System.Collections.Generic;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class CreateScaleMatrix : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateScaleMatrix class.
        /// </summary>
        public CreateScaleMatrix()
          : base("CreateScaleMatrix", "CreateScaleMatrix",
              "Creates a uniform scaling matrix that scales equally on each axis with a center point.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Center", "C", "Center point of scaling", GH_ParamAccess.item,Point3d.Origin);
            pManager.AddNumberParameter("Factor", "F", "The uniform scaling factor.", GH_ParamAccess.item, 1);
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
            double f = 1;
            if (!DA.GetData(1, ref f)) return;
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(Matrix4x4.Transpose(Matrix4x4.CreateScale((float)f,sp))));
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
                return Resources.CreateScaleMatrix;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{ea5dfd58-e42f-40f9-a797-6d0f13357e99}"); }
        }
    }
}