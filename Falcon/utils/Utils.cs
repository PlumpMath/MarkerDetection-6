using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Falcon.GlyphClasses;
using Falcon.Properties;

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
    }
}
