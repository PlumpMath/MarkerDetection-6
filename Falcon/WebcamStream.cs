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



namespace Falcon
{
    public abstract class WebcamStream : GH_Component
    {
        
        private GH_Document mydoc;
        public WebcamSource theWebcamSource;
        public abstract Bitmap WindowsBitmap { get; set; }
        public bool speed1_state;
        public bool speed2_state;
        public bool speed3_state;
        public bool speed4_state;
        public bool Res0_state;
        public bool Res1_state;
        public bool Res2_state;
        public bool Res3_state;
        public bool Res4_state;
        public bool Res5_state;
        public bool Res6_state;
        private int m_refresh;
        public bool Paused;
        /*
        private void Menu_ItemClicked1(object sender, EventArgs e)
        {
            this.speed1_state = true;
            this.speed2_state = false;
            this.speed3_state = false;
            this.speed4_state = false;
            this.m_refresh = 10000;
        }

        private void Menu_ItemClicked2(object sender, EventArgs e)
        {
            this.speed1_state = false;
            this.speed2_state = true;
            this.speed3_state = false;
            this.speed4_state = false;
            this.m_refresh = 900;
        }

        private void Menu_ItemClicked3(object sender, EventArgs e)
        {
            this.speed1_state = false;
            this.speed2_state = false;
            this.speed3_state = true;
            this.speed4_state = false;
            this.m_refresh = 70;
        }

        private void Menu_ItemClicked4(object sender, EventArgs e)
        {
            this.speed1_state = false;
            this.speed2_state = false;
            this.speed3_state = false;
            this.speed4_state = true;
            this.m_refresh = 5;
        }
        public bool preview
        {
            get
            {
                if (this.m_attributes == null)
                    return false;
                return ((WebcamStreamAttributes)m_attributes).DrawVideo;
            }
            set
            {
                if (this.m_attributes == null)
                    return;
                ((WebcamStreamAttributes)m_attributes).DrawVideo = value;
            }
        }
        public WebcamStream()
          : base("WebcamStream", "WebcamStream",
              "start webcam video stream",
              "Falcon", "Vision")
        {
            
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
            GH_ObjectWrapper destination = new GH_ObjectWrapper();
            if (!DA.GetData<GH_ObjectWrapper>(0, ref destination))
                return;
            try
            {
                this.WindowsBitmap = (Bitmap)destination.Value;
            }
            catch (InvalidCastException ex)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, ex.Message.ToString());
            }
            if (this.WindowsBitmap == null)
                return;
            DA.SetData(0, (object)this.WindowsBitmap);
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



        public override bool AppendMenuItems(ToolStripDropDown iMenu)
        {
            MenuHeaderItem menuHeaderItem1 = new MenuHeaderItem
            {
                Text = "Capture Devices:",
                Font = GH_FontServer.StandardItalic
            };
            iMenu.Items.Add((ToolStripItem)menuHeaderItem1);
            VideoInputManager.EnumDevices();
            for (int index = 0; index < VideoInputManager.Sources.Count; ++index)
            {
                bool @checked = false;
                if (this.theWebcamSource == VideoInputManager.Sources[index])
                    @checked = true;
                GH_DocumentObject.Menu_AppendItem((ToolStrip)iMenu, VideoInputManager.Sources[index].Name, new EventHandler(this.Menu_ItemCaptureDevice), true, @checked).Tag = (object)VideoInputManager.Sources[index];
            }
            GH_DocumentObject.Menu_AppendSeparator((ToolStrip)iMenu);
            MenuHeaderItem menuHeaderItem2 = new MenuHeaderItem();
            menuHeaderItem2.Text = "Video Frame Update:";
            menuHeaderItem2.Font = GH_FontServer.StandardItalic;
            iMenu.Items.Add((ToolStripItem)menuHeaderItem2);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)iMenu, "Excruciating  (10s)", new EventHandler(this.Menu_ItemClicked1), true, this.speed1_state);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)iMenu, "Slow (1s)", new EventHandler(this.Menu_ItemClicked2), true, this.speed2_state);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)iMenu, "Jagged (100ms)", new EventHandler(this.Menu_ItemClicked3), true, this.speed3_state);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)iMenu, "Smooth (25ms)", new EventHandler(this.Menu_ItemClicked4), true, this.speed4_state);
            GH_DocumentObject.Menu_AppendSeparator((ToolStrip)iMenu);
            MenuHeaderItem menuHeaderItem3 = new MenuHeaderItem();
            menuHeaderItem3.Text = "Video Resolution:";
            menuHeaderItem3.Font = GH_FontServer.StandardItalic;
            iMenu.Items.Add((ToolStripItem)menuHeaderItem3);
            for (int index = 0; index < VideoInputManager.Resolutions.Count; ++index)
            {
                bool @checked = false;
                if (this.WindowsBitmap != null && this.WindowsBitmap.DesiredResolution == VideoInputManager.Resolutions[index])
                    @checked = true;
                GH_DocumentObject.Menu_AppendItem((ToolStrip)iMenu, VideoInputManager.Resolutions[index].Name, new EventHandler(this.Menu_ItemResolution), true, @checked).Tag = (object)VideoInputManager.Resolutions[index];
            }
            GH_DocumentObject.Menu_AppendSeparator((ToolStrip)iMenu);
            GH_DocumentObject.Menu_AppendItem((ToolStrip)iMenu, "Show Video Preview", new EventHandler(this.Menu_ItemClicked12), true, this.preview);
            return true;
        }

    }


    public class MenuHeaderItem : ToolStripMenuItem
    {
        public override bool CanSelect => false;
    }

    public class VideoInputManager
    {
        public static List<VideoInputSource> Sources = new List<VideoInputSource>();
        public static List<PresetResolution> Resolutions = new List<PresetResolution>();

        static VideoInputManager()
        {
            VideoInputManager.Resolutions.Add(new PresetResolution("640px x 480px", 640, 480));
            VideoInputManager.Resolutions.Add(new PresetResolution("480px x 360px", 480, 360));
            VideoInputManager.Resolutions.Add(new PresetResolution("320px x 240px", 320, 240));
            VideoInputManager.Resolutions.Add(new PresetResolution("240px x 180px", 240, 180));
            VideoInputManager.Resolutions.Add(new PresetResolution("160px x 120px", 160, 120));
            VideoInputManager.Resolutions.Add(new PresetResolution("80px x 60px", 80, 60));
            VideoInputManager.Resolutions.Add(new PresetResolution("40px x 30px", 40, 30));
        }

        public static void EnumDevices()
        {
            VideoIN.EnumCaptureDevices();
            for (int index1 = 0; index1 < VideoInputManager.Sources.Count; ++index1)
            {
                bool flag = true;
                for (int index2 = 0; index2 < ((List<VideoCaptureDevice>)VideoIN.CaptureDevices).Count; ++index2)
                {
                    if (VideoInputManager.Sources[index1].Name == (string)((List<VideoCaptureDevice>)VideoIN.CaptureDevices)[index2].Name)
                    {
                        flag = false;
                        VideoInputManager.Sources[index1].DeviceID = (int)((List<VideoCaptureDevice>)VideoIN.CaptureDevices)[index2].CamId;
                        break;
                    }
                }
                VideoInputManager.Sources[index1].IsDisconnected = flag;
            }
            for (int index1 = 0; index1 < ((List<VideoCaptureDevice>)VideoIN.CaptureDevices).Count; ++index1)
            {
                bool flag = true;
                for (int index2 = 0; index2 < VideoInputManager.Sources.Count; ++index2)
                {
                    if (VideoInputManager.Sources[index2].Name == (string)((List<VideoCaptureDevice>)VideoIN.CaptureDevices)[index1].Name)
                    {
                        flag = false;
                        VideoInputManager.Sources[index2].DeviceID = (int)((List<VideoCaptureDevice>)VideoIN.CaptureDevices)[index1].CamId;
                        break;
                    }
                }
                if (flag)
                    VideoInputManager.Sources.Add(new VideoInputSource(((List<VideoCaptureDevice>)VideoIN.CaptureDevices)[index1])
                    {
                        DesiredResolution = VideoInputManager.Resolutions[4]
                    });
            }
        }

        public static VideoInputSource GetDevice(VideoInputSource Device)
        {
            if (Device == null)
                return (VideoInputSource)null;
            ++Device.References;
            return Device;
        }

        public static VideoInputSource GetFirstAvailableDevice()
        {
            VideoInputManager.EnumDevices();
            if (VideoInputManager.Sources.Count > 0)
                return VideoInputManager.GetDevice(VideoInputManager.Sources[0]);
            return (VideoInputSource)null;
        }

        public static void ReleaseDevice(VideoInputSource Device)
        {
            if (Device == null)
                return;
            --Device.References;
            if (Device.References > 0)
                return;
            Device.References = 0;
            Device.Stop();
        }
        */
    }

}

    