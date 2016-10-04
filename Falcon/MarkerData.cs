using AForge.Math;
using Falcon.utils;
using Rhino.Geometry;

namespace Falcon
{
    public class MarkerData
    {
        public string ID;
        public Matrix TransformationMatrix;

        public MarkerData(string id, Matrix4x4 transMatrix4X4)
        {
            ID = id;
            TransformationMatrix = Utils.ConvertAforgeMatrix(transMatrix4X4);
        }
    }
}