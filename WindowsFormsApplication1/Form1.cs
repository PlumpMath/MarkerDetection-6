using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Vision.GlyphRecognition;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        const string CAMDATA = "cam.data";
        // const string IMG = "../../data/capture.png";
        const string IMG = "C:/Users/chen/Desktop/capture.png";


        GlyphImageProcessor imageProcessor = new GlyphImageProcessor();

        // object used for synchronization
        private object sync = new object();

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            // make two glyph
            //number 0
            byte[,] id0_data =
            {
                {0, 0, 0, 0, 0, 0, 0}, {0, 1, 1, 1, 1, 1, 0}, {0, 0, 1, 1, 1, 1, 0},
                {0, 1, 1, 1, 1, 1, 0}, {0, 0, 1, 1, 1, 1, 0}, {0, 1, 0, 1, 0, 1, 0}, {0, 0, 0, 0, 0, 0, 0}
            };
            Glyph id0 = new Glyph("id0",id0_data);

            // make a glyphdatabase
            GlyphDatabase glyphDatabase = new GlyphDatabase(7);
            glyphDatabase.Add(id0);

            //set this database to imageprocessor
            imageProcessor.GlyphDatabase = glyphDatabase;
            
            //load image
            Bitmap bmap = new Bitmap(IMG);
            pictureBox1.Image = bmap;
            pictureBox1.SizeMode= PictureBoxSizeMode.AutoSize;

            //change imageprocesser settings
            imageProcessor.CameraFocalLength = bmap.Width;
            imageProcessor.GlyphSize = 7;
            imageProcessor.VisualizationType = VisualizationType.Name;

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
                int a = 0;
                List<ExtractedGlyphData> glyphs = imageProcessor.ProcessImage(bmap);
                    foreach (ExtractedGlyphData glyph in glyphs)
                    {
                        
                        if ((glyph.RecognizedGlyph != null)   &&
                             (glyph.IsTransformationDetected))
                        {
                            a++;
                            label1.Text = glyph.TransformationMatrix.V00.ToString();
                        }
                    }
                label1.Text = a.ToString();



            }

        }
    }
}
