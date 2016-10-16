using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using Falcon.GlyphClasses;
using Falcon.Properties;
using Falcon.utils;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Render;
using Plane = Rhino.Geometry.Plane;

namespace Falcon.Vision
{
    public class FalconEye : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the FalconEye class.
        /// </summary>
        Bitmap bmap = null;

        object sync = new object();
        private GlyphImageProcessor imageProcessor;
        private List<MarkerData> markers;
        private List<MarkerData> markersbuffer;
        private List<string> markerIDs;
        private List<Plane> planes;
        private List<Matrix> transformations;
        private double size = 0;

        public FalconEye()
            : base("FalconEye", "FalconEye",
                "FalconEye, the marker detector",
                "Falcon", "Vision")
        {
            markers = new List<MarkerData>();
            //set this database to imageprocessor
            imageProcessor = new GlyphImageProcessor();
            imageProcessor.GlyphDatabase = Utils.loadDatabase(0);
            imageProcessor.VisualizationType = VisualizationType.Name;
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Image", "I", "image to process", GH_ParamAccess.item);
            pManager.AddNumberParameter("MarkerSize", "S", "The real size of the marker(mm).", GH_ParamAccess.item, 150);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("MarkerID", "ID", "", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "P",
                "The plane of markers if you take your camera as standard xy plane with z pointing up.",
                GH_ParamAccess.list);
            pManager.AddMatrixParameter("Transformation", "T", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!DA.GetData<Bitmap>(0, ref bmap)) return;
            if (!DA.GetData(1, ref size)) return;
            if (bmap == null) return;


            markersbuffer = new List<MarkerData>();
            markerIDs = new List<string>();
            transformations = new List<Matrix>();
            planes = new List<Plane>();


            //change imageprocesser settings
            try
            {
                imageProcessor.CameraFocalLength = bmap.Width;
            }
            catch (Exception)
            {
                return;
            }

            imageProcessor.GlyphSize = (float) size;
            detectMarkers();


            DA.SetDataList(0, markerIDs);
            DA.SetDataList(1, planes);
            DA.SetDataList(2, transformations);
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
                return Resources.FalconEye;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b8c7f4e4-d7eb-4ed7-b28e-2e68946d86f0}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }



        public void detectMarkers()
        {
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
                List<ExtractedGlyphData> glyphs = imageProcessor.ProcessImage(bmap);
                foreach (ExtractedGlyphData glyph in glyphs)
                {

                    if ((glyph.RecognizedGlyph != null) &&
                        (glyph.IsTransformationDetected))
                    {
                        MarkerData m = new MarkerData(glyph.RecognizedGlyph.Name, glyph.TransformationMatrix);
                        markersbuffer.Add(m);
                        markerIDs.Add(m.ID);
                        planes.Add(m.plane);
                        transformations.Add(m.TransformationMatrix);
                    }
                }
            }
            if (markersbuffer.Count > 0)
            {
                markers = markersbuffer;
            }
        }

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            MenuHeaderItem menuHeaderItem1 = new MenuHeaderItem();
            menuHeaderItem1.Text = @"Marker Database:";
            menuHeaderItem1.Font = GH_FontServer.StandardBold;
            menuHeaderItem1.ToolTipText = "The marker category that you are using. Defalut category is Apriltags. \n0 = AprilTags\n1 = NyID\n In case you need more markers, please contact the author.\nmail@chenjingcheng.com";
            menu.Items.Add((ToolStripItem)menuHeaderItem1);
            base.AppendAdditionalComponentMenuItems(menu);
            ToolStripMenuItem item1 = Menu_AppendItem(menu, "  0.Apriltags", Apriltags);
            ToolStripMenuItem item2 = Menu_AppendItem(menu, "  1.NyID", NyID);
        }

        private void Apriltags(object sender, EventArgs e)
        {
            imageProcessor = new GlyphImageProcessor();
            imageProcessor.GlyphDatabase = Utils.loadDatabase(0);
            ExpireSolution(true);
        }
        private void NyID(object sender, EventArgs e)
        {
            imageProcessor = new GlyphImageProcessor();
            imageProcessor.GlyphDatabase = Utils.loadDatabase(1);
            ExpireSolution(true);
        }
    }

    public class MenuHeaderItem : ToolStripMenuItem
    {
        public override bool CanSelect
        {
            get
            {
                return false;
            }
        }
    }
}