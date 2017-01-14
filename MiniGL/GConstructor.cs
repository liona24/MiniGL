using System;
using System.Collections.Generic;

namespace MiniGL
{
    ///<summary>
    ///Use to construct simple 3 dimensional objects like cubes, spheres, pyramids, ...
    ///</summary>
    public static class GConstructor
    {
        ///<summary>
        /// Constructs a cube with side length=1 and center laying at (0,0,0)
        ///</summary>
        public static void MakeCube(bool fill, GManager manager)
        {
            if (fill)
            {
                //front surface 
                manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1));
                manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1));
                //back surface
                manager.AddVertices(new Vec4(-0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                manager.AddVertices(new Vec4(-0.5, 0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));

                //left surface
                manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1), new Vec4(-0.5, -0.5, 0.5, 1));
                manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, -0.5, 0.5, 1));
                //right surface
                manager.AddVertices(new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                manager.AddVertices(new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                
                //top surface
                 manager.AddVertices(new Vec4(-0.5, -0.5, -0.5, 1), new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                 manager.AddVertices(new Vec4(-0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                //bottom surface
                 manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, 0.5, 1));
                 manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, 0.5, 1));
            }
            else 
            {
                manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1));
                manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1));
                manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1));

                manager.AddVertices(new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1));
                manager.AddVertices(new Vec4(0.5, -0.5, -0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1));
                manager.AddVertices(new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));

                manager.AddVertices(new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                manager.AddVertices(new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1));
                manager.AddVertices(new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1));

                manager.AddVertices(new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                manager.AddVertices(new Vec4(0.5, 0.5, 0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1));
                manager.AddVertices(new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, -0.5, 1));
            }
        }
        ///<summary>
        /// Constructs a pyramid with square base of height=1 and side length=1, center of base surface laying at (0,0,0)
        ///</summary>
        public static void MakePyramid(bool fill, GManager manager)
        {
            var tip = new Vec4(0,0,-1,1);
            if (fill)
            {
                //base
                manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1));
                manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1));

                manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(0.5, -0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(0.5, -0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(0.5, 0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), new Vec4(-0.5, -0.5, 0, 1), tip);
            }
            else 
            {
                manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(0.5, -0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(0.5, -0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(0.5, 0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), new Vec4(-0.5, -0.5, 0, 1), tip);
                
                manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(0.5, -0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), tip);
                manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), tip);
            }
        }

        ///<summary>
        ///Constructs a cone of height=1 and radius=1. The center of the base lays at (0,0,0) and the tip at (0,0,-1)
        ///<param name="numberOfSplits">The number of times the sides are split into two. Value must be >= 0, wheres 0 would equal a pyramid</param>
        ///</summary>
        public static void MakeCone(int numberOfSplits, bool fill,  GManager manager)
        {
            if (numberOfSplits < 0)
                throw new ArgumentException("NumberOfSplits must be at least 2 or more", "numberOfSplits");
            numberOfSplits += 2;
            int numSegments = 1 << numberOfSplits;
            
            var tip = new Vec4(0,0,-1,1);
            var center = new Vec4(0,0,0,1);
            
            var circle = new Vec2[numSegments / 4];
            circle[0] = new Vec2(1, 0);
            double step = Math.PI * 2 / numSegments; //calculating position over quarter of circle
            double angle = step;
            int numIter = numSegments / 4;
            double x, y, nx, ny;
            for (int i = 1; i < numIter; i++)
            {
                x = Math.Sin(angle);
                y = Math.Cos(angle);
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
                    manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), center);
                    manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), center);
                    manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), center);
                    manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), center);
                    
                    manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tip);
                    manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tip);
                    manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tip);
                    manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tip);
                }
                x = nx;
                y = ny;
                nx = 0;
                ny = 1;
                manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), center);
                manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), center);
                manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), center);
                manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), center);
                
                manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tip);
                manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tip);
                manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tip);
                manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tip);

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
                    manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1));
                    manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1));
                    manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1));
                    manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1));
                    
                    manager.AddVertices(new Vec4(x, y, 0, 1), tip);
                    manager.AddVertices(new Vec4(-x, y, 0, 1),tip);
                    manager.AddVertices(new Vec4(-x, -y, 0, 1), tip);
                    manager.AddVertices(new Vec4(x, -y, 0, 1), tip);
                }

                x = nx;
                y = ny;
                nx = 0;
                ny = 1;
                manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1));
                manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1));
                manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1));
                manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1));
                
                manager.AddVertices(new Vec4(x, y, 0, 1), tip);
                manager.AddVertices(new Vec4(-x, y, 0, 1),tip);
                manager.AddVertices(new Vec4(-x, -y, 0, 1), tip);
                manager.AddVertices(new Vec4(x, -y, 0, 1), tip);
            }
        }

        public static void MakeSphere(int numberOfSplits, bool fill, GManager manager)
        {
            //constructs a icosahedron and splits the triangles into smaller ones numberOfSplits times
            
            var vecs = new List<Vec4>();

            var t = Math.Sqrt((5 + Math.Sqrt(5)) / 10);
            var s = Math.Sqrt((5 - Math.Sqrt(5)) / 10);
            vecs.Add(new Vec4(-s, t, 0, 1));
            vecs.Add(new Vec4(s, t, 0, 1));
            vecs.Add(new Vec4(-s, -t, 0, 1));
            vecs.Add(new Vec4(s, -t, 0, 1));

            vecs.Add(new Vec4(0, -s, t, 1));
            vecs.Add(new Vec4(0, s, t, 1));
            vecs.Add(new Vec4(0, -s, -t, 1));
            vecs.Add(new Vec4(0, s, -t, 1));
            
            vecs.Add(new Vec4(t, 0, -s, 1));
            vecs.Add(new Vec4(t, 0, s, 1));
            vecs.Add(new Vec4(-t, 0, -s, 1));
            vecs.Add(new Vec4(-t, 0, s, 1));
           
            var indexCache = new Dictionary<long, int>();
            var tris = new List<Vec3I>();
            tris.Add(new Vec3I(0, 11, 5));
            tris.Add(new Vec3I(0, 5, 1));
            tris.Add(new Vec3I(0, 1, 7));
            tris.Add(new Vec3I(0, 7, 10));
            tris.Add(new Vec3I(0, 10, 11));

            tris.Add(new Vec3I(1, 5, 9));
            tris.Add(new Vec3I(5, 11, 4));
            tris.Add(new Vec3I(11, 10, 2));
            tris.Add(new Vec3I(10, 7, 6));
            tris.Add(new Vec3I(7, 1, 8));

            tris.Add(new Vec3I(3, 9, 4));
            tris.Add(new Vec3I(3, 4, 2));
            tris.Add(new Vec3I(3, 2, 6));
            tris.Add(new Vec3I(3, 6, 8));
            tris.Add(new Vec3I(3, 8, 9));
            
            tris.Add(new Vec3I(4, 9, 5));
            tris.Add(new Vec3I(2, 4, 11));
            tris.Add(new Vec3I(6, 2, 10));
            tris.Add(new Vec3I(8, 6, 7));
            tris.Add(new Vec3I(9, 8, 1));

            for (int i = 0; i < numberOfSplits; i++)
            {
                var tris2 = new List<Vec3I>();
                foreach (var tri in tris)
                {
                    int i1 = tri.X;
                    int i2 = tri.Y;
                    long hash = hashTwoIndices(i1, i2);
                    int index;
                    if (!indexCache.TryGetValue(hash, out index))
                    {
                        var mid = vecs[i1] + vecs[i2];
                        mid.MakeUnit();
                        index = vecs.Count;
                        indexCache.Add(hash, index);
                        vecs.Add(mid);
                    }
                    int a = index;
                    i1 = tri.Z;
                    hash = hashTwoIndices(i1, i2);
                    if (!indexCache.TryGetValue(hash, out index))
                    {
                        var mid = vecs[i1] + vecs[i2];
                        mid.MakeUnit();
                        index = vecs.Count;
                        indexCache.Add(hash, index);
                        vecs.Add(mid);
                    }
                    int b = index;
                    i2 = tri.X;
                    hash = hashTwoIndices(i1, i2);
                    if (!indexCache.TryGetValue(hash, out index))
                    {
                        var mid = vecs[i1] + vecs[i2];
                        mid.MakeUnit();
                        index = vecs.Count;
                        indexCache.Add(hash, index);
                        vecs.Add(mid);
                    }
                    int c = index;

                    tris2.Add(new Vec3I(tri.X, a, c));
                    tris2.Add(new Vec3I(tri.Y, b, a));
                    tris2.Add(new Vec3I(tri.Z, c, b));
                    tris2.Add(new Vec3I(a, b, c));
                }
                tris = tris2;
            }

            if (fill)
            {
                foreach (var tri in tris)
                    manager.AddVertices(vecs[tri.X], vecs[tri.Y], vecs[tri.Z]);
            }
            else
            {
                var lineCache = new Dictionary<long, Vec2I>();
                foreach (var tri in tris)
                {
                    long hash = hashTwoIndices(tri.X, tri.Y);
                    if (!lineCache.ContainsKey(hash))
                        lineCache.Add(hash, new Vec2I(tri.X, tri.Y));
                    hash = hashTwoIndices(tri.Z, tri.Y);
                    if (!lineCache.ContainsKey(hash))
                        lineCache.Add(hash, new Vec2I(tri.Z, tri.Y));
                    hash = hashTwoIndices(tri.Z, tri.X);
                    if (!lineCache.ContainsKey(hash))
                        lineCache.Add(hash, new Vec2I(tri.Z, tri.X));
                }
                foreach (var kvp in lineCache)
                    manager.AddVertices(vecs[kvp.Value.X], vecs[kvp.Value.Y]);
            }
        }

        private static long hashTwoIndices(int i1, int i2)
        {
            long hash; 
            if (i1 < i2)
                hash = (long)i1 << 32 + i2;
            else
                hash = (long)i2 << 32 + i1;
            return hash;
        }
    }
}
