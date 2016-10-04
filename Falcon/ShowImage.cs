using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Falcon.Properties;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Falcon
{
    public class ShowImage : GH_Component
    {
        public Bitmap WindowsBitmap;
        private GH_Document mydoc;
        // object used for synchronization
        private object sync = new object();

        public ShowImage()
          : base("ShowImage", "ShowImage",
              "Show image",
              "Falcon", "Vision")
        {
            
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "Image to show", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutputImage", "O", "Output image data", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_ObjectWrapper destination = new GH_ObjectWrapper();
            if (!DA.GetData<GH_ObjectWrapper>(0, ref destination))
                return;
            try
            {
                this.WindowsBitmap = destination.Value as Bitmap;
            }
            catch (InvalidCastException ex)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message.ToString());
            }
            if (this.WindowsBitmap == null)
                return;
            DA.SetData(0, (object) this.WindowsBitmap);

        }

        // disable right click menu
        public override bool AppendMenuItems(ToolStripDropDown iMenu)
        {
            return true;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resources.showimage;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{e89dcfc7-eaf4-4fc7-9685-8c258bae300e}"); }
        }

        // modify the order of components in GH,otherwise ordered alphabatically
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        public override void CreateAttributes()
        {
            m_attributes = (IGH_Attributes)new ShowImageAttributes(this);
        }

        public void UnregisterEvents()
        {
            Instances.DocumentServer.DocumentRemoved -= new GH_DocumentServer.DocumentRemovedEventHandler(this.OnDocRemoved);
            if (this.mydoc != null)
                this.mydoc.ObjectsDeleted -= new GH_Document.ObjectsDeletedEventHandler(this.ObjectsDeleted);
            this.mydoc = (GH_Document)null;
        }

        public void OnDocRemoved(GH_DocumentServer ds, GH_Document doc)
        {
            GH_Document ghDocument = this.OnPingDocument();
            if (ghDocument != null && !object.ReferenceEquals((object)ghDocument, (object)doc))
                return;
            this.UnregisterEvents();
        }

        public void ObjectsDeleted(object sender, GH_DocObjectEventArgs e)
        {
            if (!e.Objects.Contains((IGH_DocumentObject)this))
                return;
            this.UnregisterEvents();
        }
    }


}
