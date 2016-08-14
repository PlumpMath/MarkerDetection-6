using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Math;
using AForge;
using AForge.Vision.GlyphRecognition;

namespace MarkerDetection
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private GlyphRecognizer recognizer = new GlyphRecognizer(5);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice); 
            videoSource = new VideoCaptureDevice();
            foreach (FilterInfo videoDevice in videoDevices)
            {
                comboBox1.Items.Add(videoDevice.Name);
            }
            comboBox1.SelectedItem = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (videoSource.IsRunning)
            {
                videoSource.Stop();
                pictureBox1.Image = null;
                pictureBox1.Invalidate();

            }
            else
            {
                videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoSourceOnNewFrame;
              
                videoSource.Start();
            }
        }

        private void VideoSourceOnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;
            List<ExtractedGlyphData> glyphs = recognizer.FindGlyphs(image);
            foreach (ExtractedGlyphData glyphData in glyphs)
            {
                label1.Text = "marker detected";
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(videoSource.IsRunning) videoSource.Stop();
        }
    }
}
