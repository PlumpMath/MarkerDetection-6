using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Falcon.Properties;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace Falcon.Vision
{
    public class LoadImage : GH_Component
    {
        public Bitmap WindowsBitmap;
        private GH_Document mydoc;

        public LoadImage()
          : base("LoadImage", "LoadImage",
              "load a file path to show the image",
              "Falcon", "Vision")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePahh", "P", "the file path of the image to load", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutputImage", "O", "Output image data", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string destination1 = "";
            if (!DA.GetData<string>(0, ref destination1))return;
            if (!File.Exists(destination1))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Image File does not exist");
                this.WindowsBitmap = (Bitmap)null;
            }
            else
            {
                try
                {
                    Bitmap bitmap1 = new Bitmap(destination1);
                    Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height, PixelFormat.Format32bppArgb);
                    Graphics graphics = Graphics.FromImage((Image)bitmap2);
                    graphics.DrawImage((Image)bitmap1, new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), new Rectangle(0, 0, bitmap1.Width, bitmap1.Height), GraphicsUnit.Pixel);
                    graphics.Dispose();
                    this.WindowsBitmap = (Bitmap) bitmap1.Clone();
                    
                }
                catch (InvalidCastException ex)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message.ToString());
                }
                if (this.WindowsBitmap == null)return;
                DA.SetData(0, (object)this.WindowsBitmap);
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.Loadimage1;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{e6518c95-c61b-4d61-be0f-6dc3cb2e1922}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        public override void CreateAttributes()
        {
            m_attributes = (IGH_Attributes)new LoadImageAttributes(this);
        }

        public override bool AppendMenuItems(ToolStripDropDown iMenu)
        {
            return true;
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