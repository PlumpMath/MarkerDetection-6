using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using jp.nyatla.nyartoolkit.cs.core;
using jp.nyatla.nyartoolkit.cs.cs4;
using jp.nyatla.nyartoolkit.cs.detector;
using NyARToolkitCSUtils.Capture;
using System.Drawing.Imaging;
using jp.nyatla.nyartoolkit.cs;
using NyARToolkitCSUtils.Direct3d;
using jp.nyatla.nyar4psg;
using IKVM.Runtime;
using processing.core;
using java.awt.image;

namespace TestImage
{

    /***
    public partial class Form1 : Form, CaptureListener
    {
        private const String AR_CODE_FILE = "../../data/patt.hiro";
        private const String AR_CAMERA_FILE = "../../data/camera_para.dat";
        public const String TESTIMAGE = "../../data/320x240ABGR.png";

        private DsRgbRaster m_raster;
        private NyARSingleDetectMarker m_ar;
        private CaptureDevice m_cap;

       
        

        private PApplet me;
        public Form1()
        {



            InitializeComponent();
            //pictureBox1.Image = loadBitmap;
           // pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            NyARParam ap = NyARParam.loadFromARParamFile(File.OpenRead(AR_CAMERA_FILE), 320, 240);
            NyARCode code = NyARCode.loadFromARPattFile(File.OpenRead(AR_CODE_FILE), 16, 16);
            
            NyARDoubleMatrix44 result_mat = new NyARDoubleMatrix44();

            CaptureDeviceList cl = new CaptureDeviceList();
            CaptureDevice cap = cl[0];
            cap.SetCaptureListener(this);
            cap.PrepareCapture(320, 240, 30);
            this.m_cap = cap;
            this.m_raster = new DsRgbRaster(cap.video_width, cap.video_height);
            this.m_ar = NyARSingleDetectMarker.createInstance(ap, code, 80.0);
            
            this.m_ar.setContinueMode(false);

        }


        public void OnBuffer(CaptureDevice i_sender, double i_sample_time, IntPtr i_buffer, int i_buffer_len)
        {
            int w = i_sender.video_width;
            int h = i_sender.video_height;
            int s = w * (i_sender.video_bit_count / 8);


            Bitmap b = new Bitmap(w, h, s, PixelFormat.Format32bppRgb, i_buffer);


            // If the image is upsidedown
            b.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox1.Image = b;

            //ARの計算
            this.m_raster.setBuffer(i_buffer, i_buffer_len, i_sender.video_vertical_flip);
            if (this.m_ar.detectMarkerLite(this.m_raster, 100))
            {
                NyARDoubleMatrix44 result_mat = new NyARDoubleMatrix44();
                this.m_ar.getTransmationMatrix(result_mat);
                this.Invoke(
                    (MethodInvoker)delegate ()
                    {
                        label1.Text = this.m_ar.getConfidence().ToString();
                    }
                );
            }
            else
            {
                this.Invoke(
                    (MethodInvoker)delegate () {
                        label1.Text = "nothing detected";

                    }
                );
            }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.m_cap.StartCapture();
        }
    }


    **/

    public partial class Form1 : Form
    {
        private const String AR_CODE_FILE = "../../data/patt.hiro";
        private const String AR_CAMERA_FILE = "../../data/camera_para.dat";
        public const String TESTIMAGE = "../../data/1.png";

        private CaptureDevice m_cap;

        MultiMarker nya;


        private PApplet me;

        public Form1()
        {
            InitializeComponent();
            
            me = new PApplet();
            Bitmap lbBitmap = new Bitmap(TESTIMAGE);
            java.awt.Image a = new BufferedImage(lbBitmap);
            PImage myimage = new PImage(a);
            nya = new MultiMarker(me, 640, 480, AR_CAMERA_FILE, NyAR4PsgConfig.CONFIG_PSG);
            nya.addNyIdMarker(0, 80);
            //nya.addARMarker(AR_CODE_FILE, 80);
            nya.detect(myimage);
            label1.Text = myimage.pixels.Length.ToString();
            if (nya.isExist(0))
            {
                label1.Text = "SUCCESSFUL!!!!!!!!!!!";
            }
            else
            {
                label1.Text = "BAD!!!!";
            }
            pictureBox1.Image = new Bitmap(TESTIMAGE);


            
           

        }
    }
}
