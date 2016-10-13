using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionScale : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuaternionScale class.
        /// </summary>
        public QuaternionScale()
          : base("QuaternionScale", "Scale",
              "Returns the quaternion that results from scaling all the components of a specified quaternion by a scalar factor.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The source quaternion.", GH_ParamAccess.item);
        
            pManager.AddNumberParameter("Factor", "F", "The scalar value.",GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The scaled quaternion.", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            double b = 0;
            if (!DA.GetData<Quaternion>(0, ref a)) return;
            if (!DA.GetData(1, ref b)) return;
            DA.SetData(0, Quaternion.Multiply(a,(float)b));
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
                return Resources.QuaternionScale;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1fabb5f5-950e-46a0-bb2e-72b17099b20d}"); }
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