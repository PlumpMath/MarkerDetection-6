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
using Rhino.Geometry.Intersect;
using Vector3 = System.Numerics.Vector3;


namespace Falcon.utils
{
    public static class Utils
    {
        public static GlyphDatabase loadDatabase(int category)
        {
            String JSONString;
            if (category == 0)
                JSONString = Resources.AprilTag;
            else
                JSONString = Resources.NyID;
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

        public static System.Numerics.Matrix4x4 ConvertLeftToRight(System.Numerics.Matrix4x4 left)
        {
            return new System.Numerics.Matrix4x4(left.M11, left.M21, left.M31, left.M41,
                                            left.M12, left.M22, left.M32, left.M42,
                                            left.M13, left.M23, left.M33, left.M43,
                                            left.M14, left.M24, left.M34, left.M44);
        }

        public static Matrix ConvertFromSystemMatrixToRhinoMatrix(System.Numerics.Matrix4x4 m)
        {
            Transform t = new Transform
            {
                M00 = m.M11,
                M01 = m.M12,
                M02 = m.M13,
                M03 = m.M14,
                M10 = m.M21,
                M11 = m.M22,
                M12 = m.M23,
                M13 = m.M24,
                M20 = m.M31,
                M21 = m.M32,
                M22 = m.M33,
                M23 = m.M34,
                M30 = m.M41,
                M31 = m.M42,
                M32 = m.M43,
                M33 = m.M44
            };
            return new Matrix(t);
        }

        public static System.Numerics.Matrix4x4 ConvertFromRhinoMatrixToSystemMatrix(Matrix rMatrix)
        {
            System.Numerics.Matrix4x4 sm = new System.Numerics.Matrix4x4(
                (float)rMatrix[0, 0], (float)rMatrix[0, 1], (float)rMatrix[0, 2], (float)rMatrix[0, 3],
                (float)rMatrix[1, 0], (float)rMatrix[1, 1], (float)rMatrix[1, 2], (float)rMatrix[1, 3],
                (float)rMatrix[2, 0], (float)rMatrix[2, 1], (float)rMatrix[2, 2], (float)rMatrix[2, 3],
                (float)rMatrix[3, 0], (float)rMatrix[3, 1], (float)rMatrix[3, 2], (float)rMatrix[3, 3]);
            return sm;
        }

        public static Vector3d SystemVectoRhinoVec(Vector3 v)
        {
            return new Vector3d(v.X,v.Y,v.Z);
        }

        public static System.Numerics.Vector3 RhinoVectoSystemVec(Vector3d v)
        {
            return new System.Numerics.Vector3((float)v.X,(float)v.Y,(float)v.Z);
        }

        public static List<GeometryBase> CastToGeometry(Point3d StartPoint, Point3d EndPoint, List<Brep> Objects)
        {
            List<GeometryBase> Geoms = new List<GeometryBase>();
            Geoms.Add((GeometryBase)new Point(StartPoint));
            Geoms.Add((GeometryBase)new Point(EndPoint));
            if (Objects.Count > 0) Geoms.AddRange(Objects.Cast<GeometryBase>());
            return Geoms;
        }

        public static BoundingBox ComputeBoundingBox(List<GeometryBase> Objects)
        {
            BoundingBox UnionBox = BoundingBox.Unset;
            foreach (var Object in Objects)
            {
                BoundingBox b = Object.GetBoundingBox(false);
                UnionBox.Union(b);
            }
            return UnionBox;
        }

        public static bool HaveDirectPath(Point3d StartPoint, Point3d EndPoint, List<Brep> Obstacles)
        {
            Line directPath = new Line(StartPoint, EndPoint);
            Curve myCurve = new LineCurve(directPath);
            if (Obstacles.Count == 0) return true;
            foreach (var obstacle in Obstacles)
            {
                Point3d[] outPoints;
                Curve[] outCurve;
                var intersect = Intersection.CurveBrep(myCurve, obstacle, 0.001, out outCurve, out outPoints);
                if (intersect == true)
                {
                    if (outCurve.Length > 0 || outPoints.Length > 0) return false;
                }
            }
            return true;
        }

        public static List<Brep> Screenobstacles(Point3d StartPoint, Point3d EndPoint, List<Brep> Obstacles)
        {
            if (Obstacles.Count == 0) return new List<Brep>();
            BoundingBox myBoundingBox = ComputeBoundingBox(CastToGeometry(StartPoint, EndPoint, new List<Brep>()));
            Box mybox = new Box(myBoundingBox);
            List<Brep> screenedObstacles = new List<Brep>();
            foreach (var obstacle in Obstacles)
            {
                Point3d[] outPoints;
                Curve[] outCurve;
                Brep myBrep = Brep.CreateFromBox(mybox);
                if (myBrep == null)
                {
                    Point3d[] Corners = mybox.GetCorners();
                    Corners = Point3d.CullDuplicates(Corners, 0.0001);
                    if (Corners.Length == 4)
                    {
                        NurbsSurface mySurface = NurbsSurface.CreateFromCorners(Corners[0], Corners[1], Corners[2], Corners[3]);
                        myBrep = Brep.CreateFromSurface(mySurface);
                    }
                    if (Corners.Length == 2)
                    {
                        Corners[0].X += 0.01;
                        Corners[0].Y += 0.01;
                        Corners[0].Z += 0.01;
                        BoundingBox b = new BoundingBox(Corners[0], Corners[1]);
                        myBrep = b.ToBrep();
                    }
                }
                var intersect = Intersection.BrepBrep(myBrep, obstacle, 0.001, out outCurve, out outPoints);
                if (intersect == true)
                {
                    if (outCurve.Length > 0 || outPoints.Length > 0) screenedObstacles.Add(obstacle);
                }
            }
            return screenedObstacles;
        }

        public static Point3d GetCenterPt(Point3d[] Points)
        {
            int i = Points.Length;
            Point3d AllPt = Point3d.Unset;
            foreach (var pt in Points)
            {
                AllPt = AllPt + pt;
            }
            return AllPt / i;
        }
    }
}
