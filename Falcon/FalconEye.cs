using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using Falcon.GlyphClasses;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Falcon
{
    public class FalconEye : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FalconEye class.
        /// </summary>
        public FalconEye()
          : base("FalconEye", "FalconEye",
              "FalconEye, the marker detector",
              "Falcon", "Vision")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "image to process", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Number", "N", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Bitmap bmap = null;
            int number = 0;
            if (!DA.GetData<Bitmap>(0, ref bmap)) return;

            
            GlyphImageProcessor imageProcessor = new GlyphImageProcessor();
            object sync = new object();

            
            //set this database to imageprocessor
            imageProcessor.GlyphDatabase = Utils.loadDatabase();

            //change imageprocesser settings
            imageProcessor.CameraFocalLength = bmap.Width;
            imageProcessor.GlyphSize = 7;
            imageProcessor.VisualizationType = VisualizationType.Name;

            //detect markers
            if (bmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                // convert image to RGB if it is grayscale
                GrayscaleToRGB filter = new GrayscaleToRGB();

                Bitmap temp = filter.Apply(bmap);
                bmap.Dispose();
                bmap = temp;

            }

            lock (sync)
            {
                int a = 0;
                List<ExtractedGlyphData> glyphs = imageProcessor.ProcessImage(bmap);
                foreach (ExtractedGlyphData glyph in glyphs)
                {

                    if ((glyph.RecognizedGlyph != null) &&
                         (glyph.IsTransformationDetected))
                    {
                        a++;
                    }
                }
                number = a;

            }


            DA.SetData(0, number);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b8c7f4e4-d7eb-4ed7-b28e-2e68946d86f0}"); }
        }
    }
}