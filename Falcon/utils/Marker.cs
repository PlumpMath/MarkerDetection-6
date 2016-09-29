using System;
using System.Drawing;

namespace Falcon.utils
{
    public struct MarkerPlainData
    {
        public String ID;
        public String blocknumber;
        public String data;
        public String category;

        public MarkerPlainData(String categorytext, String id, String datatext, String size)
        {
            category = categorytext;
            ID = id;
            data = datatext;
            blocknumber = size;
        }
    }
    public class Marker
    {
        public Bitmap image;
        public int blocknumber;
        public int[,] data;
        public byte[,] DataBytes;
        public int ID;
        public String text;
        public String datatext;
        public String category;
        public MarkerPlainData PlainData;

        public Marker(Bitmap bmap, int size, int id)
        {
            blocknumber = size;
            ID = id;
            data = new int[blocknumber, blocknumber];
            DataBytes = new byte[blocknumber, blocknumber];
            image = new Bitmap(bmap, new Size(blocknumber * 20, blocknumber * 20));
            detectcolor();
            getDataString();
            category = "NyID";
            PlainData = new MarkerPlainData(category, ID.ToString(), datatext, blocknumber.ToString());
        }

        public Marker(MarkerPlainData reader)
        {
            category = reader.category;
            ID = Int32.Parse(reader.ID);
            blocknumber = Int32.Parse(reader.blocknumber);
            data = new int[blocknumber, blocknumber];
            DataBytes = new byte[blocknumber, blocknumber];
            ParseDataString(reader.data);
        }

        public override string ToString()
        {
            return string.Format("Category: {0} ID: {1}  Data: {2} Size: {3}", category, ID, datatext, blocknumber);
        }

        public void detectcolor()
        {
            for (int j = 0; j < blocknumber; j++)
            {
                for (int i = 0; i < blocknumber; i++)
                {
                    var a = image.GetPixel(10 + i * 20, 10 + j * 20);
                    data[i, j] = (a.R > 150 ? 1 : 0);
                    DataBytes[i, j] = (byte)data[i, j];
                }
            }
        }

        void getDataString()
        {
            for (int j = 0; j < blocknumber; j++)
            {
                for (int i = 0; i < blocknumber; i++)
                {
                    text += data[i, j].ToString() + "  ";
                    datatext += data[i, j].ToString();
                }
            }
        }

        void ParseDataString(String datastring)
        {
            for (var i = 0; i < blocknumber; i++)
            {
                for (var j = 0; j < blocknumber; j++)
                {
                    int index = i * blocknumber + j;
                    int b = (int)Char.GetNumericValue(datastring[index]);
                    data[i, j] = b;
                    DataBytes[i, j] = (byte)b;

                }
            }
        }



      


    }
}