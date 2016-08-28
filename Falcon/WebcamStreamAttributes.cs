using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Falcon.Properties;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace Falcon
{
    class WebcamStreamAttributes : GH_ComponentAttributes
    {
        private static Size minimumsize = new Size(100, 100);
        private static Size maximumsize = new Size(800, 800);
        private static Size defaultsize = new Size(240, 180);
        private static Padding pad = new Padding(5);
        public bool DrawVideo = true;
        private RectangleF innerrec = new RectangleF();
        private RectangleF menubar = new RectangleF();
        private RectangleF pauserec = new RectangleF();
        private Cursor m_cursor = Cursors.Arrow;
        private const int MenuBarHeight = 22;
        private const int PauseButtonHeight = 24;
        private GH_ResizeBorder rb;

        protected Size MinimumSize
        {
            get
            {
                return WebcamStreamAttributes.minimumsize;
            }
        }

        protected Padding SizingBorders
        {
            get
            {
                return WebcamStreamAttributes.pad;
            }
        }

        protected Size MaximumSize
        {
            get
            {
                return WebcamStreamAttributes.maximumsize;
            }
        }

        public RectangleF MenuRectangle
        {
            get
            {
                RectangleF rectangleF = this.menubar;
                rectangleF.Y = this.Bounds.Bottom - 22f;
                rectangleF.X = this.Bounds.X;
                rectangleF.Height = 22f;
                rectangleF.Width = this.Bounds.Width;
                return rectangleF;
            }
        }

        public RectangleF PauseButton
        {
            get
            {
                return this.pauserec;
            }
        }

        public RectangleF InnerRectangle
        {
            get
            {
                return this.innerrec;
            }
        }

        internal WebcamStreamAttributes(WebcamStream component)
      : base((IGH_Component) component)
         {
            this.Bounds = new RectangleF(this.Pivot, (SizeF)WebcamStreamAttributes.defaultsize);
        }

        protected override void Layout()
        {
            this.Bounds = new RectangleF(this.Pivot.X, this.Pivot.Y, this.Bounds.Width, this.Bounds.Height);
            this.innerrec = this.Bounds;
            this.innerrec.Inflate(-6f, -6f);
            GH_Component ghComponent = this.DocObject as GH_Component;
            int num1 = 1;
            int num2 = 1;
            this.pauserec.Width = 24f;
            this.pauserec.Height = 24f;
            this.pauserec.X = (float)((double)this.innerrec.X + (double)this.innerrec.Width - 24.0 - 6.0);
            this.pauserec.Y = (float)((double)this.innerrec.Y + (double)this.innerrec.Height - 24.0 - 6.0);
            foreach (IGH_Param ghParam in ghComponent.Params)
            {
                if (ghParam.Attributes.HasOutputGrip)
                {
                    float num3 = this.Bounds.Height / (float)(ghComponent.Params.Output.Count + 1);
                    ghParam.Attributes.Pivot = new PointF(this.Bounds.Right, this.Bounds.Top + num3 * (float)num1);
                    ghParam.Attributes.Bounds = new RectangleF(this.Bounds.Right - 2f, (float)((double)this.Bounds.Top + (double)num3 * (double)num1 - 2.0), 4f, 4f);
                    ++num1;
                }
                else if (ghParam.Attributes.HasInputGrip)
                {
                    float num3 = this.Bounds.Height / (float)(ghComponent.Params.Input.Count + 1);
                    ghParam.Attributes.Pivot = new PointF(this.Bounds.Left, this.Bounds.Top + num3 * (float)num2);
                    ghParam.Attributes.Bounds = new RectangleF(this.Bounds.Left - 2f, (float)((double)this.Bounds.Top + (double)num3 * (double)num2 - 2.0), 4f, 4f);
                    ++num2;
                }
            }
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            List<GH_Border> borders1 = GH_Border.CreateBorders(this.Bounds, this.SizingBorders);
            this.rb = (GH_ResizeBorder)null;
            GH_Border borders2 = (GH_Border)null;
            if (this.PauseButton.Contains(e.CanvasLocation) && this.DrawVideo)
            {
                WebcamStream webCamVideoStream = this.DocObject as WebcamStream;
                //webCamVideoStream.Paused = !webCamVideoStream.Paused;
                webCamVideoStream.ExpireSolution(true);
            }
            foreach (GH_Border ghBorder in borders1)
            {
                if (ghBorder.Contains(e.CanvasLocation))
                    borders2 = ghBorder;
            }
            if (borders2 != null)
            {
                this.rb = new GH_ResizeBorder(borders2);
                this.rb.Setup((IGH_Attributes)this, e.CanvasLocation, (SizeF)this.MinimumSize, (SizeF)this.MaximumSize);
                return GH_ObjectResponse.Capture;
            }
            this.rb = (GH_ResizeBorder)null;
            return base.RespondToMouseDown(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (this.rb != null)
            {
                Cursor.Current = this.m_cursor;
                RectangleF new_shape;
                PointF new_pivot;
                this.rb.Solve(e.CanvasLocation, out new_shape, out new_pivot);
                this.Pivot = new_pivot;
                this.Bounds = new_shape;
                this.ExpireLayout();
                this.DocObject.OnDisplayExpired(true);
                return GH_ObjectResponse.Handled;
            }
            this.SetupCursor(new Point((int)e.CanvasX, (int)e.CanvasY));
            return GH_ObjectResponse.Handled;
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (this.rb == null)
                return base.RespondToMouseUp(sender, e);
            this.rb = (GH_ResizeBorder)null;
            return GH_ObjectResponse.Release;
        }

        public void SetupCursor(Point canvasPoint)
        {
            this.m_cursor = !this.PauseButton.Contains((PointF)canvasPoint) || !this.DrawVideo ? Cursors.Arrow : Cursors.Hand;
            foreach (GH_Border border in GH_Border.CreateBorders(this.Bounds, this.SizingBorders))
            {
                if (border.Contains((PointF)canvasPoint))
                    this.m_cursor = border.Topology == GH_BorderTopology.Right || border.Topology == GH_BorderTopology.Left ? Cursors.SizeWE : (border.Topology == GH_BorderTopology.Top || border.Topology == GH_BorderTopology.Bottom ? Cursors.SizeNS : (border.Topology == GH_BorderTopology.TopLeft || border.Topology == GH_BorderTopology.BottomRight ? Cursors.SizeNWSE : (border.Topology == GH_BorderTopology.TopRight || border.Topology == GH_BorderTopology.BottomLeft ? Cursors.SizeNESW : Cursors.Arrow)));
            }
            Cursor.Current = this.m_cursor;
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel != GH_CanvasChannel.Objects)
                return;
            GH_Palette palette = GH_Palette.Normal;
            if (this.Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning)
                palette = GH_Palette.Warning;
            else if (this.Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error)
                palette = GH_Palette.Error;
            GH_Capsule capsule = GH_Capsule.CreateCapsule(this.Bounds, palette, 6, 30);
            foreach (IGH_Param ghParam in (this.DocObject as GH_Component).Params)
            {
                if (ghParam.Attributes.HasOutputGrip)
                    capsule.AddOutputGrip(ghParam.Attributes.Pivot.Y);
                if (ghParam.Attributes.HasInputGrip)
                    capsule.AddInputGrip(ghParam.Attributes.Pivot.Y);
            }
            capsule.Render(graphics, this.Selected, this.Owner.Locked, false);
            WebcamStream webCamVideoStream = this.DocObject as WebcamStream;
            if (this.DrawVideo && webCamVideoStream.WindowsBitmap != null)
            {
                SolidBrush solidBrush = new SolidBrush(Color.Black);
                graphics.FillRectangle((Brush)solidBrush, GH_Convert.ToRectangle(this.InnerRectangle));
                solidBrush.Dispose();
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.DrawImage((Image)webCamVideoStream.WindowsBitmap, this.InnerRectangle);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Near;
                format.Trimming = StringTrimming.EllipsisCharacter;
                RectangleF layoutRectangle = new RectangleF();
                layoutRectangle.Width = 60f;
                layoutRectangle.Height = 16f;
                layoutRectangle.X = this.InnerRectangle.Left + 3f;
                layoutRectangle.Y = this.InnerRectangle.Top + 2f;
                /*
                string s = webCamVideoStream.FPS.ToString() + "fps";
                if (webCamVideoStream.IsCornerWhite)
                    graphics.DrawString(s, GH_FontServer.Small, Brushes.Black, layoutRectangle, format);
                else
                    graphics.DrawString(s, GH_FontServer.Small, Brushes.LightGray, layoutRectangle, format);
                format.Dispose();
                if (webCamVideoStream.Paused)
                    GH_GraphicsUtil.RenderCenteredIcon(graphics, this.PauseButton, (Image)Resources.Icon_Play2);
                else
                    GH_GraphicsUtil.RenderCenteredIcon(graphics, this.PauseButton, (Image)Resources.Icon_Pause2);
                    */
            }
            else
            {
                TextureBrush textureBrush = new TextureBrush((Image)Resources.checkerpattern);
                textureBrush.WrapMode = WrapMode.Tile;
                graphics.FillRectangle((Brush)textureBrush, this.InnerRectangle);
                textureBrush.Dispose();
                GH_GraphicsUtil.RenderCenteredIcon(graphics, this.InnerRectangle, (Image)Resources.NoImage);
            }
            GH_GraphicsUtil.ShadowRectangle(graphics, GH_Convert.ToRectangle(this.InnerRectangle), 15, 40);
            graphics.DrawRectangle(Pens.Black, GH_Convert.ToRectangle(this.InnerRectangle));
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return base.RespondToMouseDoubleClick(sender, e);
            if (Bounds.Contains(e.CanvasLocation))
            {
                WebcamStream thisWebcamStream = this.DocObject as WebcamStream;
                if (thisWebcamStream == null)
                    this.Bounds = new RectangleF(this.Pivot, (SizeF)WebcamStreamAttributes.defaultsize);
                else
                    this.Bounds = new RectangleF(this.Pivot, (SizeF)thisWebcamStream.WindowsBitmap.Size);
                this.ExpireLayout();
                this.DocObject.OnDisplayExpired(true);
            }

            return base.RespondToMouseDoubleClick(sender, e);
        }
    }
}
