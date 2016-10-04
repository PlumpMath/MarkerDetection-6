using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using AForge.Math;
using Falcon.GlyphClasses;
using Falcon.Properties;
using Grasshopper.Kernel.Types;
using Rhino.Collections;
using Rhino.Geometry;

namespace Falcon.utils
{
    public static class Utils
    {
        public static GlyphDatabase loadDatabase()
        {
            String JSONString = Resources.NyID;
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var plaindatas = ser.Deserialize<List<MarkerPlainData>>(JSONString);
            int size = Int32.Parse(plaindatas[0].blocknumber);
            GlyphDatabase glyphDatabase = new GlyphDatabase(size);
            foreach (var plaindata in plaindatas)
            {
                Marker newMarker = new Marker(plaindata);
                Glyph newGlyph = new Glyph(newMarker.ID.ToString(), newMarker.DataBytes);
                glyphDatabase.Add(newGlyph);
            }
            return glyphDatabase;
        }

        public static Matrix ConvertAforgeMatrix(Matrix4x4 a)
        {
            Transform t = new Transform
            {
                M00 = a.V00,
                M01 = a.V01,
                M02 = a.V02,
                M03 = a.V03,
                M10 = a.V10,
                M11 = a.V11,
                M12 = a.V12,
                M13 = a.V13,
                M20 = a.V20,
                M21 = a.V21,
                M22 = a.V22,
                M23 = a.V23,
                M30 = a.V30,
                M31 = a.V31,
                M32 = a.V32,
                M33 = a.V33
            };
            return new Matrix(t);
        }
    }
}
