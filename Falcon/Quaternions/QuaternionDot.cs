using System;
using System.Numerics;
using Falcon.Properties;
using Grasshopper.Kernel;

namespace Falcon.Quaternions
{
    public class QuaternionDot : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the QuaternionDot class.
        /// </summary>
        public QuaternionDot()
          : base("QuaternionDot", "Dot",
              "Calculates the dot product of two quaternions.",
              "Falcon", "Quaternion")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion1", "Q1", "The first quaternion.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quaternion2", "Q2", "The second quaternion.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Quaternion", "Q", "The dot product. ", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();
            if (!DA.GetData<Quaternion>(0, ref a)) return;
            if (!DA.GetData<Quaternion>(1, ref b)) return;
            DA.SetData(0, Quaternion.Dot(a, b));
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
                return Resources.QuaternionDot;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7b8c9ff6-f814-46fc-a4ef-ab8e047ba7b4}"); }
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