using System;

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
        public static GObject[] MakeCube(bool fill, TMaker tmaker, GManager manager)
        {
            var res = new GObject[12];
            if (fill)
            {
                //front surface 
                res[0] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1));
                res[1] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1));
                //back surface
                res[2] = manager.AddVertices(new Vec4(-0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                res[3] = manager.AddVertices(new Vec4(-0.5, 0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));

                //left surface
                res[4] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1), new Vec4(-0.5, -0.5, 0.5, 1));
                res[5] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, -0.5, 0.5, 1));
                //right surface
                res[6] = manager.AddVertices(new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                res[7] = manager.AddVertices(new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                
                //top surface
                res[8] = manager.AddVertices(new Vec4(-0.5, -0.5, -0.5, 1), new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                res[9] = manager.AddVertices(new Vec4(-0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                //bottom surface
                res[10] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, 0.5, 1));
                res[11] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, 0.5, 1));
            }
            else 
            {
                res[0] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1));
                res[1] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1));
                res[2] = manager.AddVertices(new Vec4(-0.5, 0.5, -0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1));

                res[3] = manager.AddVertices(new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, 0.5, -0.5, 1));
                res[4] = manager.AddVertices(new Vec4(0.5, -0.5, -0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1));
                res[5] = manager.AddVertices(new Vec4(0.5, -0.5, -0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));

                res[6] = manager.AddVertices(new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                res[7] = manager.AddVertices(new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1));
                res[8] = manager.AddVertices(new Vec4(-0.5, -0.5, 0.5, 1), new Vec4(-0.5, -0.5, -0.5, 1));

                res[9] = manager.AddVertices(new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, -0.5, 0.5, 1));
                res[10] = manager.AddVertices(new Vec4(0.5, 0.5, 0.5, 1), new Vec4(-0.5, 0.5, 0.5, 1));
                res[11] = manager.AddVertices(new Vec4(0.5, 0.5, 0.5, 1), new Vec4(0.5, 0.5, -0.5, 1));
            }

            return res;
        }
        ///<summary>
        /// Constructs a pyramid with square base of height=1 and side length=1, center of base surface laying at (0,0,0)
        ///</summary>
        public static GObject[] MakePyramid(bool fill, TMaker tmaker, GManager manager)
        {
            GObject[] res = null;
            var tip = new Vec4(0,0,-1,1);
            if (fill)
            {
                res = new GObject[6];

                //base
                res[0] = manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1));
                res[1] = manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1));

                res[2] = manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(0.5, -0.5, 0, 1), tip);
                res[3] = manager.AddVertices(new Vec4(0.5, -0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1), tip);
                res[4] = manager.AddVertices(new Vec4(0.5, 0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), tip);
                res[5] = manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), new Vec4(-0.5, -0.5, 0, 1), tip);
            }
            else 
            {
                res = new GObject[8];
                
                res[0] = manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), new Vec4(0.5, -0.5, 0, 1), tip);
                res[1] = manager.AddVertices(new Vec4(0.5, -0.5, 0, 1), new Vec4(0.5, 0.5, 0, 1), tip);
                res[2] = manager.AddVertices(new Vec4(0.5, 0.5, 0, 1), new Vec4(-0.5, 0.5, 0, 1), tip);
                res[3] = manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), new Vec4(-0.5, -0.5, 0, 1), tip);
                
                res[4] = manager.AddVertices(new Vec4(-0.5, -0.5, 0, 1), tip);
                res[5] = manager.AddVertices(new Vec4(0.5, -0.5, 0, 1), tip);
                res[6] = manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), tip);
                res[7] = manager.AddVertices(new Vec4(-0.5, 0.5, 0, 1), tip);
            }
            return res;
        }

        ///<summary>
        ///Constructs a cone of height=1 and radius=1. The center of the base lays at (0,0,0) and the tip at (0,0,-1)
        ///<param name="numberOfSplits">The number of times the sides are split into two. Value must be >= 2, wheres 2 would equal a pyramid</param>
        ///</summary>
        public static GObject[] MakeCone(bool fill, int numberOfSplits, TMaker tmaker, GManager manager)
        {
            if (numberOfSplits < 2)
                throw new ArgumentException("NumberOfSplits must be at least 2 or more", "numberOfSplits");

            int numSegments = 1 << numberOfSplits;
            var res = new GObject[2 * numSegments];
            
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
                    res[i * 4] = manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), center);
                    res[i * 4 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), center);
                    res[i * 4 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), center);
                    res[i * 4 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), center);
                    
                    res[i * 8] = manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tip);
                    res[i * 8 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tip);
                    res[i * 8 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tip);
                    res[i * 8 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tip);
                }
                x = nx;
                y = ny;
                nx = 0;
                ny = 1;
                res[i * 4] = manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), center);
                res[i * 4 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), center);
                res[i * 4 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), center);
                res[i * 4 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), center);
                
                res[i * 8] = manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1), tip);
                res[i * 8 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1), tip);
                res[i * 8 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1), tip);
                res[i * 8 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1), tip);

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
                    res[i * 4] = manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1));
                    res[i * 4 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1));
                    res[i * 4 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1));
                    res[i * 4 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1));
                    
                    res[i * 8] = manager.AddVertices(new Vec4(x, y, 0, 1), tip);
                    res[i * 8 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1),tip);
                    res[i * 8 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), tip);
                    res[i * 8 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), tip);
                }

                x = nx;
                y = ny;
                nx = 0;
                ny = 1;
                res[i * 4] = manager.AddVertices(new Vec4(x, y, 0, 1), new Vec4(nx, ny, 0, 1));
                res[i * 4 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1), new Vec4(-nx, ny, 0, 1));
                res[i * 4 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), new Vec4(-nx, -ny, 0, 1));
                res[i * 4 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), new Vec4(nx, -ny, 0, 1));
                
                res[i * 8] = manager.AddVertices(new Vec4(x, y, 0, 1), tip);
                res[i * 8 + 1] = manager.AddVertices(new Vec4(-x, y, 0, 1),tip);
                res[i * 8 + 2] = manager.AddVertices(new Vec4(-x, -y, 0, 1), tip);
                res[i * 8 + 3] = manager.AddVertices(new Vec4(x, -y, 0, 1), tip);
            }
            return res;
        }

        public static GObject[] MakeSphere(int numberOfSplits, bool fill, TMaker tmaker, GManager manager)
        {
            //constructs a icosahedron and splits the triangles into smaller ones numberOfSplits times
            
            
        }
    }
}
