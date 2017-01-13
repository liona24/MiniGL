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
        ///<param name="numberOfSplits">The number of times the sides are split into two. Value must be >= 2, wheres 2 would equal a pyramid</param>
        ///</summary>
        public static void MakeCone(bool fill, int numberOfSplits, GManager manager)
        {
            if (numberOfSplits < 2)
                throw new ArgumentException("NumberOfSplits must be at least 2 or more", "numberOfSplits");

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

        public static GObject[] MakeSphere(int numberOfSplits, bool fill, GManager manager)
        {
            //constructs a icosahedron and splits the triangles into smaller ones numberOfSplits times
            
            
        }
    }
}
