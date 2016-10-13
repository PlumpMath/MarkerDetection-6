using AForge.Math;
using Falcon.utils;
using Rhino.Geometry;

namespace Falcon.Vision
{
    public class MarkerData
    {
        public string ID;
        public Matrix TransformationMatrix;
        public Plane plane;

        public MarkerData(string id, Matrix4x4 transMatrix4X4)
        {
            ID = id;
            TransformationMatrix = reOrientMatrix(Utils.ConvertAforgeMatrix(transMatrix4X4));
            plane = calculatePlane();
        }

        private Plane calculatePlane()
        {
            var theplane = Plane.WorldXY;
            var newTransform = new Transform
            {
                M00 = TransformationMatrix[0, 0],
                M01 = TransformationMatrix[0, 1],
                M02 = TransformationMatrix[0, 2],
                M03 = TransformationMatrix[0, 3],
                M10 = TransformationMatrix[1, 0],
                M11 = TransformationMatrix[1, 1],
                M12 = TransformationMatrix[1, 2],
                M13 = TransformationMatrix[1, 3],
                M20 = TransformationMatrix[2, 0],
                M21 = TransformationMatrix[2, 1],
                M22 = TransformationMatrix[2, 2],
                M23 = TransformationMatrix[2, 3],
                M30 = TransformationMatrix[3, 0],
                M31 = TransformationMatrix[3, 1],
                M32 = TransformationMatrix[3, 2],
                M33 = TransformationMatrix[3, 3]
            };
            theplane.Transform(newTransform);
            return theplane;
        }

        protected Matrix reOrientMatrix(Matrix originalMatrix)
        {
            var sm = Utils.ConvertFromRhinoMatrixToSystemMatrix(originalMatrix);
            System.Numerics.Matrix4x4 transMatrix4X4 = new System.Numerics.Matrix4x4(-1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1);
            System.Numerics.Matrix4x4 mirror = new System.Numerics.Matrix4x4(-1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1);
            sm = System.Numerics.Matrix4x4.Multiply(sm, transMatrix4X4);
            sm = System.Numerics.Matrix4x4.Multiply(mirror, sm);
            return Utils.ConvertFromSystemMatrixToRhinoMatrix(sm);
        }
    }
}