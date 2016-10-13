using System;
using System.Collections.Generic;
using System.Numerics;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Matrixs
{
    public class CreateTranslationMatrix : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreatTranslationMatrix class.
        /// </summary>
        public CreateTranslationMatrix()
          : base("CreateTranslationMatrix", "CreateTranslationMatrix",
              "Creates a translation matrix from the specified 3-dimensional vector.",
              "Falcon", "Matrix4x4")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddVectorParameter("Vector", "V", "The translation vector.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMatrixParameter("Matrix", "M", "The translation matrix.", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Vector3d rv = Vector3d.Zero;
            if (!DA.GetData(0, ref rv)) return;
            Vector3 sv = Utils.RhinoVectoSystemVec(rv);
            Matrix4x4 sm = Matrix4x4.Transpose(Matrix4x4.CreateTranslation(sv));
            DA.SetData(0, Utils.ConvertFromSystemMatrixToRhinoMatrix(sm));
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
                return Resources.CreateTranslationMatrix;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{cb44b4f1-ae5d-4ba2-a2fd-16d0f36909a1}"); }
        }
    }
}