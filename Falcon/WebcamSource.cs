using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Math;
using AForge;

namespace Falcon
{
    public class WebcamSource
    {
        public FilterInfoCollection videoDevices;
        public VideoCaptureDevice videoSource;
        public int webcamIndex;
        public Bitmap image;

        public void InitializeWebcam()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            videoSource = new VideoCaptureDevice();
        }

        public void StartWebcamStream()
        {
            videoSource = new VideoCaptureDevice(videoDevices[webcamIndex].MonikerString);
            videoSource.NewFrame += VideoSourceOnNewFrame;
            videoSource.Start();
        }

        public void PauseWebcamStream()
        {
            if (!videoSource.IsRunning) return;
            videoSource.Stop();
            this.image = (Bitmap)null;
        }

        public void StopWebcamStream()
        {
            if (videoSource.IsRunning) videoSource.Stop();
        }

        private void VideoSourceOnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            this.image = (Bitmap)eventArgs.Frame.Clone();
        }
    }

    
}
