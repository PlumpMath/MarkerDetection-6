using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Falcon.Properties;
using Firefly_Bridge;
using Grasshopper.Kernel;

namespace Falcon.Vision
{
    public class BitmapToFly : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BitmapToF class.
        /// </summary>
        public BitmapToFly()
          : base("BitmapToFly", "BitmapToFly",
              "Convert System Bitmap to Firefly image",
              "Falcon", "Vision")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bitmap", "B", "the system bitmap to convert", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FireflyImage", "F", "the firefly image converted from bitmap",
                GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            Bitmap bmap = null;
            if(!DA.GetData<Bitmap>(0, ref bmap)) return;
            if(bmap==null) return;



            //Parallel.For(0, fireflyImage.Ry, index1 => { Parallel.For(0, fireflyImage.Rx, index2 => { Color c = newbmap.GetPixel(index2, fireflyImage.Ry - index1 - 1); fireflyImage.pixels[index2, index1] = new Firefly_Pixel() { A = c.A, B = c.B, G = c.G, R = c.R }; }); });

            //Firefly_Bitmap fireflyImage = new Firefly_Bitmap();
            //fireflyImage.SetSize(bmap.Width, bmap.Height);
            //var newbmap = bmap;
            //for (int index1 = 0; index1 < fireflyImage.Ry; index1++)
            //{
            //    for (int index2 = 0; index2 < fireflyImage.Rx; index2++)
            //    {
            //        Color c = newbmap.GetPixel(index2, fireflyImage.Ry - index1 - 1);
            //        fireflyImage.pixels[index2, index1] = new Firefly_Pixel() { A = c.A, B = c.B, G = c.G, R = c.R };
            //    }
            //}

            Firefly_Bitmap fireflyImage = TurnBitmapToFireflyBitmap(bmap);
            DA.SetData(0, fireflyImage);
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
                return Resources.BitmaptoFly;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{01719761-a5be-45b7-9652-d3e41146eae5}"); }
        }

        protected Firefly_Bitmap TurnBitmapToFireflyBitmap(Bitmap b)
        {
            Firefly_Bitmap newFirefly = new Firefly_Bitmap();
            try
            {
                Bitmap bitmap2 = new Bitmap(b.Width, b.Height, PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage((Image)bitmap2);
                graphics.DrawImage((Image)b, new Rectangle(0, 0, b.Width, b.Height), new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
                graphics.Dispose();
                newFirefly = new Firefly_Bitmap();
                newFirefly.SetSize(bitmap2.Width, bitmap2.Height);
                BitmapData bitmapdata = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadOnly, bitmap2.PixelFormat);
                IntPtr scan0 = bitmapdata.Scan0;
                int length = bitmapdata.Stride * bitmap2.Height;
                byte[] destination = new byte[length];
                Marshal.Copy(scan0, destination, 0, length);
                int index1 = 0;
                for (int index2 = bitmap2.Height - 1; index2 >= 0; --index2)
                {
                    for (int index3 = 0; index3 < bitmap2.Width; ++index3)
                    {
                        double num1 = (double)destination[index1];
                        double num2 = (double)destination[index1 + 1];
                        double num3 = (double)destination[index1 + 2];
                        double num4 = (double)destination[index1 + 3];
                        newFirefly.pixels[index3, index2].B = (byte)num1;
                        newFirefly.pixels[index3, index2].G = (byte)num2;
                        newFirefly.pixels[index3, index2].R = (byte)num3;
                        newFirefly.pixels[index3, index2].A = (byte)num4;
                        newFirefly.pixels[index3, index2].V = (byte)(((int)newFirefly.pixels[index3, index2].R + (int)newFirefly.pixels[index3, index2].G + (int)newFirefly.pixels[index3, index2].B) / 3);
                        index1 += 4;
                    }
                }
                newFirefly.ComputeGradient();
                bitmap2.UnlockBits(bitmapdata);
            }
            catch (InvalidCastException ex)
            {
                ((GH_ActiveObject)this).AddRuntimeMessage((GH_RuntimeMessageLevel)10, ex.Message.ToString());
            }

            return newFirefly;
        }
    }
}