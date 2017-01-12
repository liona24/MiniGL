using System;

namespace MiniGL
{
    public static class GConstructor2D
    {
        public static GObject2D[] GetRectangle(Rect rect, double w, TMaker2D tmaker)
        {
            return GetRectangle(rect.L, rect.T, rect.R, rect.B, w, tmaker);
        }
        public static GObject2D[] GetRectangle(Rect rect, TMaker2D tmaker)
        {
            return GetRectangle(rect.L, rect.T, rect.R, rect.B, 1.0, tmaker);
        }
        public static GObject2D[] GetRectangle(double l, double t, double r, double b, double w, TMaker2D tmaker)
        {
            return new GObject2D[] { new GObject2D (new Vec3(l, t, w), new Vec3(r, t, w), new Vec3(l, b, w), tmaker),
                                        new GObject2D (new Vec3(r, t, w), new Vec3(r, b, w), new Vec3(l, b, w), tmaker) };
        }
    }
 
    ///<summary>
    ///Use to construct simple 3 dimensional objects like cubes, spheres, pyramids, ...
    ///</summary>
    public static class GConstructor
    {
#region Cubic objects 
        public static GObject[] MakeCube(double centerX, double centerY, double centerZ, double w, double sideLength, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            return MakeCuboid(centerX - sideLength, centerY - sideLength, centerZ - sideLength, centerX + sideLength, centerY + sideLength, centerZ + sideLength, w, fill, tmaker, factory);
        } 
        public static GObject[] MakeCube(I4Dimensional origin, double sideLength, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            return MakeCuboid(origin.X, origin.Y, origin.Z, origin.X + sideLength, origin.Y + sideLength, origin.Z + sideLength,  fill, tmaker, factory);
        }

        public static GObject[] MakeCuboid(I4Dimensional origin, double w, double h, double d, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            return MakeCuboid(origin.X, origin.Y, origin.Z, origin.X + w, origin.Y + h, origin.Z + d, fill,  tmaker, factory);
        }
        ///<summary>
        /// Constructs cuboid out of triangles, sides parallel to axis, spanned by the two corners, w component set to 1
        ///</summary>
        public static GObject[] MakeCuboid(I3Dimensional corner1, I3Dimensional corner2, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            double l = Math.Min(corner1.X, corner2.X);
            double r = Math.Max(corner1.X, corner2.X);
            double t = Math.Min(corner1.Y, corner2.Y);
            double b = Math.Max(corner1.Y, corner2.Y);
            double n = Math.Min(corner1.Z, corner2.Z);
            double f = Math.Max(corner1.Z, corner2.Z);

            return MakeCuboid(l, t, n, r, b, f, 1.0, fill,  tmaker, factory);
        }
        ///<summary>
        /// Constructs cuboid out of triangles, sides parallel to axis, spanned by the two corners
        ///</summary>
        public static GObject[] MakeCuboid(I4Dimensional corner1, I4Dimensional corner2, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            double l = Math.Min(corner1.X, corner2.X);
            double r = Math.Max(corner1.X, corner2.X);
            double t = Math.Min(corner1.Y, corner2.Y);
            double b = Math.Max(corner1.Y, corner2.Y);
            double n = Math.Min(corner1.Z, corner2.Z);
            double f = Math.Max(corner1.Z, corner2.Z);

            return MakeCuboid(l, t, n, r, b, f, corner1.W, fill,  tmaker, factory);
        }
        ///<summary>
        /// Constructs a cuboid out of triangles, sides parallel to axis, defined by the given parameters, w component set to 1
        /// <param name="l">Left</param>
        /// <param name="t">Top</param>
        ///<param name="n">Near</param>
        ///<param name="r">Right</param>
        ///<param name="b">Bottom</param>
        ///<param name="f">Far</param>
        ///<param name="fill">If true creates a surface model, else creates a mesh</param>
        ///</summary>
        public static GObject[] MakeCuboid(double l, double t, double n, double r, double b, double f, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            return MakeCuboid(l, t, n, r, b, f, 1.0, fill, tmaker, factory);
        }
        ///<summary>
        /// Constructs a cuboid out of triangles, sides parallel to axis, defined by the given parameters
        /// <param name="l">Left</param>
        /// <param name="t">Top</param>
        ///<param name="n">Near</param>
        ///<param name="r">Right</param>
        ///<param name="b">Bottom</param>
        ///<param name="f">Far</param>
        ///<param name="w">W component of the homogenous coordinates</param>
        ///<param name="fill">If true creates a surface model, else creates a mesh</param>
        ///</summary>
        public static GObject[] MakeCuboid(double l, double t, double n, double r, double b, double f, double w, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            var res = new GObject[12];
            if (fill)
            {
                //front surface 
                res[0] = factory.Create(new Vec4(l, b, n, w), new Vec4(r, b, n, w), new Vec4(r, t, n, w), tmaker);
                res[1] = factory.Create(new Vec4(l, b, n, w), new Vec4(r, t, n, w), new Vec4(r, t, n, w), tmaker);
                //back surface
                res[2] = factory.Create(new Vec4(l, b, f, w), new Vec4(r, b, f, w), new Vec4(r, t, f, w), tmaker);
                res[3] = factory.Create(new Vec4(l, b, f, w), new Vec4(r, t, f, w), new Vec4(r, t, f, w), tmaker);

                //left surface
                res[4] = factory.Create(new Vec4(l, b, n, w), new Vec4(l, t, n, w), new Vec4(l, t, f, w), tmaker);
                res[5] = factory.Create(new Vec4(l, b, n, w), new Vec4(l, b, n, w), new Vec4(l, t, f, w), tmaker);
                //right surface
                res[6] = factory.Create(new Vec4(r, b, n, w), new Vec4(r, t, n, w), new Vec4(r, t, f, w), tmaker);
                res[7] = factory.Create(new Vec4(r, b, n, w), new Vec4(r, b, n, w), new Vec4(r, t, f, w), tmaker);
                
                //top surface
                res[8] = factory.Create(new Vec4(l, t, n, w), new Vec4(l, t, f, w), new Vec4(r, t, f, w), tmaker);
                res[9] = factory.Create(new Vec4(l, t, n, w), new Vec4(r, t, f, w), new Vec4(r, t, f, w), tmaker);
                //bottom surface
                res[10] = factory.Create(new Vec4(l, b, n, w), new Vec4(l, b, f, w), new Vec4(r, b, f, w), tmaker);
                res[11] = factory.Create(new Vec4(l, b, n, w), new Vec4(r, b, f, w), new Vec4(r, b, f, w), tmaker);
            }
            else 
            {
                res[0] = factory.Create(new Vec4(l, b, n, w), new Vec4(r, b, n, w), tmaker);
                res[1] = factory.Create(new Vec4(l, b, n, w), new Vec4(l, t, n, w), tmaker);
                res[2] = factory.Create(new Vec4(l, b, n, w), new Vec4(l, b, f, w), tmaker);

                res[3] = factory.Create(new Vec4(r, t, n, w), new Vec4(r, b, n, w), tmaker);
                res[4] = factory.Create(new Vec4(r, t, n, w), new Vec4(l, t, n, w), tmaker);
                res[5] = factory.Create(new Vec4(r, t, n, w), new Vec4(r, t, f, w), tmaker);

                res[6] = factory.Create(new Vec4(l, t, f, w), new Vec4(r, t, f, w), tmaker);
                res[7] = factory.Create(new Vec4(l, t, f, w), new Vec4(l, b, f, w), tmaker);
                res[8] = factory.Create(new Vec4(l, t, f, w), new Vec4(l, t, n, w), tmaker);

                res[9] = factory.Create(new Vec4(r, b, f, w), new Vec4(r, t, f, w), tmaker);
                res[10] = factory.Create(new Vec4(r, b, f, w), new Vec4(l, b, f, w), tmaker);
                res[11] = factory.Create(new Vec4(r, b, f, w), new Vec4(r, b, n, w), tmaker);
            }

            return res;
        }

#endregion
#region Pyramids
        public static GObject[] MakePyramid(double l, double t, double r, double b, double z, double w, Vec4 tip, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {
            GObject[] res = null;
            if (fill)
            {
                res = new GObject[6];

                //base
                res[0] = factory.Create(new Vec4(l, t, z, w), new Vec4(l, b, z, w), new Vec4(r, b, z, w), tmaker);
                res[1] = factory.Create(new Vec4(l, t, z, w), new Vec4(t, r, z, w), new Vec4(r, b, z, w), tmaker);

                res[2] = factory.Create(new Vec4(l, t, z, w), new Vec4(r, t, z, w), tip, tmaker);
                res[3] = factory.Create(new Vec4(r, t, z, w), new Vec4(r, b, z, w), tip, tmaker);
                res[4] = factory.Create(new Vec4(r, b, z, w), new Vec4(l, b, z, w), tip, tmaker);
                res[5] = factory.Create(new Vec4(l, b, z, w), new Vec4(l, t, z, w), tip, tmaker);
            }
            else 
            {
                res = new GObject[8];
                
                res[0] = factory.Create(new Vec4(l, t, z, w), new Vec4(r, t, z, w), tip, tmaker);
                res[1] = factory.Create(new Vec4(r, t, z, w), new Vec4(r, b, z, w), tip, tmaker);
                res[2] = factory.Create(new Vec4(r, b, z, w), new Vec4(l, b, z, w), tip, tmaker);
                res[3] = factory.Create(new Vec4(l, b, z, w), new Vec4(l, t, z, w), tip, tmaker);
                
                res[4] = factory.Create(new Vec4(l, t, z, w), tip, tmaker);
                res[5] = factory.Create(new Vec4(r, t, z, w), tip, tmaker);
                res[6] = factory.Create(new Vec4(l, b, z, w), tip, tmaker);
                res[7] = factory.Create(new Vec4(l, b, z, w), tip, tmaker);
            }
            return res;
        }

        public static GObject[] MakeCone(double radius, double height, bool fill, int numberOfSplits, TMaker tmaker, GObject.GObjectFactory factory)
        {
            if (numberOfSplits < 2)
                throw new ArgumentException("NumberOfSplits must be at least 2 or more", "numberOfSplits");

            int numSegments = 1 << numberOfSplits;
            GObject[] res = new GObject[2 * numSegments];
            
            var circle = new Vec2[numSegments / 4];
            circle[0] = new Vec2(radius, 0);
            double step = Math.PI * 2 / numSegments; //calculating position over quarter of circle
            double angle = step;
            int numIter = numSegments / 4;
            double x, y, nx, ny;
            for (int i = 1; i < numIter; i++)
            {
                x = Math.Sin(angle) * radius;
                y = Math.Cos(angle) * radius;
                angle += step;
                circle[i] = new Vec2(x, y);
            }
            if (fill)
            {
                int i = 0;
                for (; i < numIter - 1; i++)
                {
                    x = circle[i].X;
                    y = circle[i].Y;
                    nx = circle[i + 1].X;
                    ny = circle[i + 1].Y;
                    res[i * 4] = factory.Create(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), center, tmaker);
                    res[i * 4 + 1] = factory.Create(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), center, tmaker);
                    res[i * 4 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), center, tmaker);
                    res[i * 4 + 3] = factory.Create(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), center, tmaker);
                    
                    res[i * 8] = factory.Create(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tip, tmaker);
                    res[i * 8 + 1] = factory.Create(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tip, tmaker);
                    res[i * 8 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tip, tmaker);
                    res[i * 8 + 3] = factory.Create(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tip, tmaker);
                }
                x = nx;
                y = ny;
                nx = 0;
                ny = radius;
                res[i * 4] = factory.Create(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), center, tmaker);
                res[i * 4 + 1] = factory.Create(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), center, tmaker);
                res[i * 4 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), center, tmaker);
                res[i * 4 + 3] = factory.Create(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), center, tmaker);
                
                res[i * 8] = factory.Create(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tip, tmaker);
                res[i * 8 + 1] = factory.Create(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tip, tmaker);
                res[i * 8 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tip, tmaker);
                res[i * 8 + 3] = factory.Create(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tip, tmaker);

            }
            else 
            {
                int i = 0;
                for (; i < numIter - 1; i++)
                {
                    x = circle[i].X;
                    y = circle[i].Y;
                    nx = circle[i + 1].X;
                    ny = circle[i + 1].Y;
                    res[i * 4] = factory.Create(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tmaker);
                    res[i * 4 + 1] = factory.Create(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tmaker);
                    res[i * 4 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tmaker);
                    res[i * 4 + 3] = factory.Create(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tmaker);
                    
                    res[i * 8] = factory.Create(new Vec4(x, y, 0, 1), tip, tmaker);
                    res[i * 8 + 1] = factory.Create(new Vec4(-x, y, 0, 1),tip, tmaker);
                    res[i * 8 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), tip, tmaker);
                    res[i * 8 + 3] = factory.Create(new Vec4(x, -y, 0, 1), tip, tmaker);
                }

                x = nx;
                y = ny;
                nx = 0;
                ny = radius;
                res[i * 4] = factory.Create(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tmaker);
                res[i * 4 + 1] = factory.Create(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tmaker);
                res[i * 4 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tmaker);
                res[i * 4 + 3] = factory.Create(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tmaker);
                
                res[i * 8] = factory.Create(new Vec4(x, y, 0, 1), tip, tmaker);
                res[i * 8 + 1] = factory.Create(new Vec4(-x, y, 0, 1),tip, tmaker);
                res[i * 8 + 2] = factory.Create(new Vec4(-x, -y, 0, 1), tip, tmaker);
                res[i * 8 + 3] = factory.Create(new Vec4(x, -y, 0, 1), tip, tmaker);
            }
            return res;
        }
#endregion

#region Sphere
        public static GObject[] MakeSphere(double radius, int numberOfSplits, bool fill, TMaker tmaker, GObject.GObjectFactory factory)
        {

        }

#endregion
    }
}
