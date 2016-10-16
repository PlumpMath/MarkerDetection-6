using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Falcon.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon.Vision
{
    public class ImageLoad : GH_Component
    {
        public Bitmap WindowsBitmap;
        public String externalpath;
        public String internalpath;
        private bool loadexternalpath;
        /// <summary>
        /// Initializes a new instance of the ImageLoad class.
        /// </summary>
        public ImageLoad()
          : base("LoadImage", "LoadImage",
              "Load local image by giving image path",
              "Falcon", "Vision")
        {
            
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "P", "The local path of the image you want to load.", GH_ParamAccess.item);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "load image and output system bitmap.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            loadexternalpath = DA.GetData<string>(0, ref externalpath);
            loadimage();
            DA.SetData(0, this.WindowsBitmap);
        }

        protected void loadimage()
        {
            String path;
            if (loadexternalpath) path = externalpath;
            else path = internalpath;
            if (!File.Exists(path))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Image File does not exist");
                this.WindowsBitmap = (Bitmap)null;
            }
            else
            {
                try
                {
                    this.WindowsBitmap = new Bitmap(path).Clone() as Bitmap;
                }
                catch (InvalidCastException ex)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message);
                }
            }
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
                return Resources.Loadimage1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{0ae34725-1ee6-48e9-ac55-2ad3a050909e}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            ToolStripMenuItem item1 = Menu_AppendItem(menu, "Set File Path", SetFilePath);
            item1.ToolTipText = @"set one file path";
            
        }

        private void SetFilePath(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = @"Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            openFileDialog1.Title = @"Select an image File";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
              internalpath = openFileDialog1.FileName;
              this.ExpireSolution(true);
            }
        }

    }
}