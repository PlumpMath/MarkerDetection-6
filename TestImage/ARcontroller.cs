using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestImage
{
    class ARController
    {
        int id;
        int w;
        int h;
        private string orientation;
        private int defaultMarkerWidth;
        private float[] transform_mat;
        private object cameraParam;

        public ARController(int width, int height, object camera)
        {
            w = width;
            h = height;
            defaultMarkerWidth = 1;
            transform_mat = new float[16];
            cameraParam = camera;
            this._initialize();
        }

        void dispose()
        {
            
        }

        void process(Bitmap image)
        {
            this.detectMarker(image);
        }

        void detectMarker(Bitmap image)
        {
            
        }
    }
}
