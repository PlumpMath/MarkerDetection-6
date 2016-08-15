using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Falcon.Properties;

namespace Falcon
{
    sealed class LoadImageAttributes : GH_ComponentAttributes
    {
        private static Size minimumsize = new Size(100, 100);
        private static Size maximumsize = new Size(800, 800);
        private static Size defaultsize = new Size(240, 180);
        private static Padding pad = new Padding(5);
        private RectangleF innerrec = new RectangleF();
        private RectangleF menubar = new RectangleF();
        private Cursor m_cursor = Cursors.Arrow;
        private const int MenuBarHeight = 22;
        private GH_ResizeBorder rb;

        private Size MinimumSize
        {
            get
            {
                return LoadImageAttributes.minimumsize;
            }
        }

        private Padding SizingBorders
        {
            get
            {
                return LoadImageAttributes.pad;
            }
        }

        private Size MaximumSize
        {
            get
            {
                return LoadImageAttributes.maximumsize;
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

        public RectangleF InnerRectangle
        {
            get
            {
                return this.innerrec;
            }
        }

        internal LoadImageAttributes(LoadImage component)
        : base((IGH_Component) component)
            {
            this.Bounds = new RectangleF(this.Pivot, (SizeF)LoadImageAttributes.defaultsize);
            }

        protected override void Layout()
        {
            this.Bounds = new RectangleF(this.Pivot.X, this.Pivot.Y, this.Bounds.Width, this.Bounds.Height);
            this.innerrec = this.Bounds;
            this.innerrec.Inflate(-6f, -6f);
            GH_Component ghComponent = this.DocObject as GH_Component;
            int num1 = 1;
            int num2 = 1;
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
            foreach (GH_Border border in GH_Border.CreateBorders(this.Bounds, this.SizingBorders))
            {
                if (border.Contains((PointF)canvasPoint))
                {
                    this.m_cursor = border.Topology == GH_BorderTopology.Right || border.Topology == GH_BorderTopology.Left ? Cursors.SizeWE : (border.Topology == GH_BorderTopology.Top || border.Topology == GH_BorderTopology.Bottom ? Cursors.SizeNS : (border.Topology == GH_BorderTopology.TopLeft || border.Topology == GH_BorderTopology.BottomRight ? Cursors.SizeNWSE : (border.Topology == GH_BorderTopology.TopRight || border.Topology == GH_BorderTopology.BottomLeft ? Cursors.SizeNESW : Cursors.Arrow)));
                    Cursor.Current = this.m_cursor;
                }
            }
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (this.DocObject != null && channel == GH_CanvasChannel.Wires)
            {
                foreach (IGH_Param ghParam in (this.DocObject as GH_Component).Params.Input)
                {
                    foreach (IGH_Param source in (IEnumerable<IGH_Param>)ghParam.Sources)
                    {
                        if (source.Attributes.HasOutputGrip)
                            canvas.Painter.DrawConnection(ghParam.Attributes.Pivot, new PointF(source.Attributes.OutputGrip.X, source.Attributes.OutputGrip.Y), GH_WireDirection.left, GH_WireDirection.right, ghParam.Attributes.Selected, source.Attributes.Selected, GH_WireType.item);
                        else
                            canvas.Painter.DrawConnection(ghParam.Attributes.Pivot, new PointF(source.Attributes.Pivot.X, source.Attributes.Pivot.Y), GH_WireDirection.left, GH_WireDirection.right, ghParam.Attributes.Selected, source.Attributes.Selected, GH_WireType.item);
                    }
                }
            }
            if (channel != GH_CanvasChannel.Objects)
                return;
            GH_Palette palette = GH_Palette.Normal;
            if (this.Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Warning)
                palette = GH_Palette.Warning;
            else if (this.Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Error)
                palette = GH_Palette.Error;
            GH_Capsule capsule = GH_Capsule.CreateCapsule(this.Bounds, palette, 6, 30);
            if (this.DocObject != null)
            {
                foreach (IGH_Param ghParam in (this.DocObject as GH_Component).Params)
                {
                    if (ghParam.Attributes.HasOutputGrip)
                        capsule.AddOutputGrip(ghParam.Attributes.Pivot.Y);
                    if (ghParam.Attributes.HasInputGrip)
                        capsule.AddInputGrip(ghParam.Attributes.Pivot.Y);
                }
            }
            capsule.Render(graphics, this.Selected, this.Owner.Locked, false);
            if (this.DocObject != null)
            {
                LoadImage loadImage = this.DocObject as LoadImage;
                if (loadImage != null && loadImage.WindowsBitmap != null)
                {
                    TextureBrush textureBrush = new TextureBrush(Resources.checkerpattern);
                    textureBrush.WrapMode = WrapMode.Tile;
                    graphics.FillRectangle((Brush)textureBrush, this.InnerRectangle);
                    textureBrush.Dispose();
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.DrawImage((Image)loadImage.WindowsBitmap, this.InnerRectangle);
                }
                else
                {
                    TextureBrush textureBrush = new TextureBrush(Resources.checkerpattern);
                    textureBrush.WrapMode = WrapMode.Tile;
                    graphics.FillRectangle((Brush)textureBrush, this.InnerRectangle);
                    textureBrush.Dispose();
                    GH_GraphicsUtil.RenderCenteredIcon(graphics, this.InnerRectangle,Resources.NoImage);
                }
            }
            GH_GraphicsUtil.ShadowRectangle(graphics, GH_Convert.ToRectangle(this.InnerRectangle), 15, 50);
            graphics.DrawRectangle(Pens.Black, GH_Convert.ToRectangle(this.InnerRectangle));
        }

        public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return base.RespondToMouseDoubleClick(sender, e);
            if (Bounds.Contains(e.CanvasLocation))
            {
                this.Bounds = new RectangleF(this.Pivot, (SizeF)LoadImageAttributes.defaultsize);
                this.ExpireLayout();
                this.DocObject.OnDisplayExpired(true);
            }

            return base.RespondToMouseDoubleClick(sender, e);
        }

    }


}
