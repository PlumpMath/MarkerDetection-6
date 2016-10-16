using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Falcon.Vision
{
    public partial class VideoCaptureForm : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoDevice;
        private string videoDeviceMoniter = string.Empty;
        private Dictionary<string, VideoCapabilities> videoCapabilitiesDictionary = new Dictionary<string, VideoCapabilities>();
        private Size captureSize = new Size(0, 0);
        // stop watch used for measuring FPS
        private Stopwatch stopWatch = null;
        public Bitmap currentFrame;

        public VideoCaptureForm()
        {
            InitializeComponent();
            try
            {
                this.videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (this.videoDevices.Count == 0)
                    throw new ApplicationException();
                foreach (FilterInfo videoDevice in (CollectionBase)this.videoDevices)
                    this.comboBoxVideoSource.Items.Add((object)videoDevice.Name);
            }
            catch (ApplicationException ex)
            {
                this.comboBoxVideoSource.Items.Add((object)"No capture devices detected");
                this.comboBoxVideoSource.Enabled = false;
                this.buttonStart.Enabled = false;
            }

            int num = 0;
            for (int index = 0; index < this.videoDevices.Count; ++index)
            {
                if (this.videoDeviceMoniter == this.videoDevices[index].MonikerString)
                {
                    num = index;
                    break;
                }
            }
            this.comboBoxVideoSource.SelectedIndex = num;
            
        }

        private void VideoCaptureForm_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.videoDeviceMoniter = this.videoDevice.Source;
            if (this.videoCapabilitiesDictionary.Count != 0)
            {
                VideoCapabilities videoCapabilities = this.videoCapabilitiesDictionary[(string)this.comboBoxVideoResolution.SelectedItem];
                this.videoDevice.DesiredFrameSize = videoCapabilities.FrameSize;
                this.videoDevice.DesiredFrameRate = videoCapabilities.FrameRate;
                this.captureSize = videoCapabilities.FrameSize;
            }
            OpenVideoSource(this.videoDevice);
        }

        private void videoSourcePlayer_NewFrame(object sender, ref Bitmap image)
        {
            currentFrame = (Bitmap)image.Clone();
            
        }

        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // reset glyph processor
            //imageProcessor.Reset();
            
            // stop current video source
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();

            // start new video source
            videoSourcePlayer1.VideoSource = new AsyncVideoSource(source);
            videoSourcePlayer1.Start();

            // reset stop watch
            stopWatch = null;

            // start timer
            timer1.Start();

            this.Cursor = Cursors.Default;
        }





        private void VideoCaptureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSourcePlayer1.VideoSource != null)
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
            }
            this.Dispose(true);
            this.Close();
        }

        private void comboBoxVideoSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.videoDevices.Count == 0)
                return;
            this.videoDevice = new VideoCaptureDevice(this.videoDevices[this.comboBoxVideoSource.SelectedIndex].MonikerString);
            this.EnumeratedSupportedFrameSizes(this.videoDevice);
        }

        private void EnumeratedSupportedFrameSizes(VideoCaptureDevice videoDevice)
        {
            this.Cursor = Cursors.WaitCursor;
            this.comboBoxVideoResolution.Items.Clear();

            this.videoCapabilitiesDictionary.Clear();
            try
            {
                VideoCapabilities[] videoCapabilities1 = videoDevice.VideoCapabilities;
                int num1 = 0;
                foreach (VideoCapabilities videoCapabilities2 in videoCapabilities1)
                {
                    string key = string.Format("{0} x {1}", (object)videoCapabilities2.FrameSize.Width, (object)videoCapabilities2.FrameSize.Height);
                    if (!this.comboBoxVideoResolution.Items.Contains((object)key))
                    {
                        if (this.captureSize == videoCapabilities2.FrameSize)
                            num1 = this.comboBoxVideoResolution.Items.Count;
                        this.comboBoxVideoResolution.Items.Add((object)key);
                    }
                    if (!this.videoCapabilitiesDictionary.ContainsKey(key))
                        this.videoCapabilitiesDictionary.Add(key, videoCapabilities2);
                    else if (videoCapabilities2.FrameRate > videoCapabilitiesDictionary[key].FrameRate)
                        this.videoCapabilitiesDictionary[key] = videoCapabilities2;
                }
                if (videoCapabilities1.Length == 0)
                    this.comboBoxVideoResolution.Items.Add((object)"Not supported");
                this.comboBoxVideoResolution.SelectedIndex = num1;
                
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            IVideoSource videoSource = videoSourcePlayer1.VideoSource;

            if (videoSource != null)
            {
                // get number of frames since the last timer tick
                int framesReceived = videoSource.FramesReceived;

                if (stopWatch == null)
                {
                    stopWatch = new Stopwatch();
                    stopWatch.Start();
                }
                else
                {
                    stopWatch.Stop();

                    float fps = 1000.0f * framesReceived / stopWatch.ElapsedMilliseconds;
                    fpsLabel.Text = fps.ToString("F2") + " fps";

                    stopWatch.Reset();
                    stopWatch.Start();
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (videoSourcePlayer1.VideoSource != null)
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
            }
            this.Dispose(true);
            this.Close();

            
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {

            if (videoSourcePlayer1.VideoSource != null)
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
            }
        }

    }
}
