using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AForge.Video;

namespace Falcon.Vision
{
    public class VideoSourcePlayer : Control
    {
        private Color borderColor = Color.Black;
        private Size frameSize = new Size(320, 240);
        private bool firstFrameNotProcessed = true;
        private object sync = new object();
        private IVideoSource videoSource;
        private Bitmap currentFrame;
        public Bitmap convertedFrame;
        private string lastMessage;
        private bool autosize;
        private bool needSizeUpdate;
        private volatile bool requestedToStop;
        private Control parent;
        private IContainer components;

        [DefaultValue(false)]
        public bool AutoSizeControl
        {
            get
            {
                return this.autosize;
            }
            set
            {
                this.autosize = value;
                this.UpdatePosition();
            }
        }

        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public IVideoSource VideoSource
        {
            get
            {
                return this.videoSource;
            }
            set
            {
                this.CheckForCrossThreadAccess();
                if (this.videoSource != null)
                {
                    this.videoSource.NewFrame -= new NewFrameEventHandler(this.videoSource_NewFrame);
                    this.videoSource.VideoSourceError -= new VideoSourceErrorEventHandler(this.videoSource_VideoSourceError);
                    this.videoSource.PlayingFinished -= new PlayingFinishedEventHandler(this.videoSource_PlayingFinished);
                }
                lock (this.sync)
                {
                    if (this.currentFrame != null)
                    {
                        this.currentFrame.Dispose();
                        this.currentFrame = (Bitmap)null;
                    }
                }
                this.videoSource = value;
                if (this.videoSource != null)
                {
                    this.videoSource.NewFrame += new NewFrameEventHandler(this.videoSource_NewFrame);
                    this.videoSource.VideoSourceError += new VideoSourceErrorEventHandler(this.videoSource_VideoSourceError);
                    this.videoSource.PlayingFinished += new PlayingFinishedEventHandler(this.videoSource_PlayingFinished);
                }
                else
                    this.frameSize = new Size(320, 240);
                this.lastMessage = (string)null;
                this.needSizeUpdate = true;
                this.firstFrameNotProcessed = true;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public bool IsRunning
        {
            get
            {
                this.CheckForCrossThreadAccess();
                if (this.videoSource == null)
                    return false;
                return this.videoSource.IsRunning;
            }
        }

        public event VideoSourcePlayer.NewFrameHandler NewFrame;

        public event PlayingFinishedEventHandler PlayingFinished;

        public VideoSourcePlayer()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        }

        private void CheckForCrossThreadAccess()
        {
            if (!this.IsHandleCreated)
            {
                this.CreateControl();
                if (!this.IsHandleCreated)
                    this.CreateHandle();
            }
            if (this.InvokeRequired)
                throw new InvalidOperationException("Cross thread access to the control is not allowed.");
        }

        public void Start()
        {
            this.CheckForCrossThreadAccess();
            this.requestedToStop = false;
            if (this.videoSource == null)
                return;
            this.firstFrameNotProcessed = true;
            this.videoSource.Start();
            this.Invalidate();
        }

        public void Stop()
        {
            this.CheckForCrossThreadAccess();
            this.requestedToStop = true;
            if (this.videoSource == null)
                return;
            this.videoSource.Stop();
            if (this.currentFrame != null)
            {
                this.currentFrame.Dispose();
                this.currentFrame = (Bitmap)null;
            }
            this.Invalidate();
        }

        public void SignalToStop()
        {
            this.CheckForCrossThreadAccess();
            this.requestedToStop = true;
            if (this.videoSource == null)
                return;
            this.videoSource.SignalToStop();
        }

        public void WaitForStop()
        {
            this.CheckForCrossThreadAccess();
            if (!this.requestedToStop)
                this.SignalToStop();
            if (this.videoSource == null)
                return;
            this.videoSource.WaitForStop();
            if (this.currentFrame != null)
            {
                this.currentFrame.Dispose();
                this.currentFrame = (Bitmap)null;
            }
            this.Invalidate();
        }

        public Bitmap GetCurrentVideoFrame()
        {
            lock (this.sync)
                return this.currentFrame == null ? (Bitmap)null : AForge.Imaging.Image.Clone(this.currentFrame);
        }

        private void VideoSourcePlayer_Paint(object sender, PaintEventArgs e)
        {
            if (this.needSizeUpdate || this.firstFrameNotProcessed)
            {
                this.UpdatePosition();
                this.needSizeUpdate = false;
            }
            lock (this.sync)
            {
                Graphics local_0 = e.Graphics;
                Rectangle local_1 = this.ClientRectangle;
                Pen local_2 = new Pen(this.borderColor, 1f);
                local_0.DrawRectangle(local_2, local_1.X, local_1.Y, local_1.Width - 1, local_1.Height - 1);
                if (this.videoSource != null)
                {
                    if (this.currentFrame != null && this.lastMessage == null)
                    {
                        local_0.DrawImage(this.convertedFrame != null ? (System.Drawing.Image)this.convertedFrame : (System.Drawing.Image)this.currentFrame, local_1.X + 1, local_1.Y + 1, local_1.Width - 2, local_1.Height - 2);
                        this.firstFrameNotProcessed = false;
                    }
                    else
                    {
                        SolidBrush local_3 = new SolidBrush(this.ForeColor);
                        local_0.DrawString(this.lastMessage == null ? "Connecting ..." : this.lastMessage, this.Font, (Brush)local_3, new PointF(5f, 5f));
                        local_3.Dispose();
                    }
                }
                local_2.Dispose();
            }
        }

        private void UpdatePosition()
        {
            if (!this.autosize || this.Dock == DockStyle.Fill || this.Parent == null)
                return;
            Rectangle clientRectangle = this.Parent.ClientRectangle;
            int width = this.frameSize.Width;
            int height = this.frameSize.Height;
            this.SuspendLayout();
            this.Location = new System.Drawing.Point((clientRectangle.Width - width - 2) / 2, (clientRectangle.Height - height - 2) / 2);
            this.Size = new Size(width + 2, height + 2);
            this.ResumeLayout();
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (this.requestedToStop)
                return;
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            if (this.NewFrame != null)
                this.NewFrame((object)this, ref image);
            lock (this.sync)
            {
                if (this.currentFrame != null)
                {
                    if (this.currentFrame.Size != eventArgs.Frame.Size)
                        this.needSizeUpdate = true;
                    this.currentFrame.Dispose();
                    this.currentFrame = (Bitmap)null;
                }
                if (this.convertedFrame != null)
                {
                    this.convertedFrame.Dispose();
                    this.convertedFrame = (Bitmap)null;
                }
                this.currentFrame = image;
                this.frameSize = this.currentFrame.Size;
                this.lastMessage = (string)null;
                if (this.currentFrame.PixelFormat != PixelFormat.Format16bppGrayScale && this.currentFrame.PixelFormat != PixelFormat.Format48bppRgb)
                {
                    if (this.currentFrame.PixelFormat != PixelFormat.Format64bppArgb)
                        goto label_15;
                }
                this.convertedFrame = AForge.Imaging.Image.Convert16bppTo8bpp(this.currentFrame);
            }
            label_15:
            this.Invalidate();
        }

        private void videoSource_VideoSourceError(object sender, VideoSourceErrorEventArgs eventArgs)
        {
            this.lastMessage = eventArgs.Description;
            this.Invalidate();
        }

        private void videoSource_PlayingFinished(object sender, ReasonToFinishPlaying reason)
        {
            switch (reason)
            {
                case ReasonToFinishPlaying.EndOfStreamReached:
                    this.lastMessage = "Video has finished";
                    break;
                case ReasonToFinishPlaying.StoppedByUser:
                    this.lastMessage = "Video was stopped";
                    break;
                case ReasonToFinishPlaying.DeviceLost:
                    this.lastMessage = "Video device was unplugged";
                    break;
                case ReasonToFinishPlaying.VideoSourceError:
                    this.lastMessage = "Video has finished because of error in video source";
                    break;
                default:
                    this.lastMessage = "Video has finished for unknown reason";
                    break;
            }
            this.Invalidate();
            if (this.PlayingFinished == null)
                return;
            this.PlayingFinished((object)this, reason);
        }

        private void VideoSourcePlayer_ParentChanged(object sender, EventArgs e)
        {
            if (this.parent != null)
                this.parent.SizeChanged -= new EventHandler(this.parent_SizeChanged);
            this.parent = this.Parent;
            if (this.parent == null)
                return;
            this.parent.SizeChanged += new EventHandler(this.parent_SizeChanged);
        }

        private void parent_SizeChanged(object sender, EventArgs e)
        {
            this.UpdatePosition();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // VideoSourcePlayer
            // 
            this.Size = new System.Drawing.Size(640, 480);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.VideoSourcePlayer_Paint);
            this.ParentChanged += new System.EventHandler(this.VideoSourcePlayer_ParentChanged);
            this.ResumeLayout(false);

        }

        public delegate void NewFrameHandler(object sender, ref Bitmap image);
    }
}