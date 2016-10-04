using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;
using Grasshopper.Documentation;
using Rhino;
using Rhino.Geometry;
using System.Windows;
using AForge.Video.DirectShow;
using Falcon.utils;


namespace Falcon
{
    public class WebcamStream : GH_Component
    {
        public Bitmap image;
        public VideoCaptureForm iVideoCaptureForm;

        public WebcamStream()
          : base("WebcamStream", "WebcamStream",
              "start webcam video stream",
              "Falcon", "Vision")
        {
            iVideoCaptureForm = new VideoCaptureForm();
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutputImage", "O", "Output image data", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (this.iVideoCaptureForm != null)
            {
                if (this.iVideoCaptureForm.currentFrame != null)
                {
                    image?.Dispose();
                    image = this.iVideoCaptureForm.currentFrame;
                }
            }
            DA.SetData(0, image);
            ExpireSolution(true);
        }

        
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

     
        public override Guid ComponentGuid
        {
            get { return new Guid("{1d50f973-9502-4305-b1de-327de1509003}"); }
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
            m_attributes = (IGH_Attributes)new WebcamStreamAttributes(this);
        }

        public void ShowWebcamForm()
        {
            iVideoCaptureForm = new VideoCaptureForm();
            iVideoCaptureForm.Show();
        }

    }

}

    