using System;
using System.Collections.Generic;

//MiniGL rasterizer

namespace MiniGL
{
    public class Rasterizer
    {
        const float EMPTY = 1f;

        int width, height;
        int offsetX, offsetY;

        readonly float[][] idcMap; //used to render polygons

        public int OffsetX { get { return offsetX; } set { offsetX = value; } }
        public int OffsetY { get { return offsetY; } set { offsetY = value; } }

        public Rasterizer(int width, int height, int offsetX, int offsetY)
        {
            this.width = width;
            this.height = height;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            idcMap = new float[width][];
            for (int i = 0; i < width; i++)
            {
                idcMap[i] = new float[height];
                for (int j = 0; j < height; j++)
                    idcMap[i][j] = EMPTY;
            }
        }

        public Vec2I[] Rasterize(params I2Dimensional[] poly)
        {
            var res = new List<Vec2I>();
            Rasterize(res, poly);
            return res.ToArray();
        }
        public void Rasterize(List<Vec2I> result, params I2Dimensional[] poly)
        {
            if (poly.Length == 1)
                result.Add(new Vec2I((int)poly[0].GetX(), (int)poly[1].GetY()));
            else if (poly.Length == 2)
                rasterLine(poly[0], poly[1], result);
            else if (poly.Length > 2)
                rasterPoly(poly, result);
        }
        public void Rasterize(GObject2D item, Screen<GObject2D> screen, params I2Dimensional[] poly)
        {
            if (poly.Length == 1)
                screen[(int)poly[0].GetX(), (int)poly[0].GetY()] = item;
            else if (poly.Length == 2)
                rasterLine(poly[0], poly[1], item, screen);
            else if (poly.Length > 2)
                rasterPoly(poly, item, screen);
        }
        public void Rasterize(GObject item, ZBuffer<GObject> zBuffer, params I3Dimensional[] poly)
        {
            if (poly.Length == 1)
                zBuffer.Insert((int)poly[0].GetX(), (int)poly[0].GetY(), (int)poly[0].GetZ(), item);
            else if (poly.Length == 2)
                rasterLine(poly[0], poly[1], item, zBuffer);
            else if (poly.Length > 2)
                rasterPoly(poly, item, zBuffer);
        }

        private void rasterPoly(I2Dimensional[] poly, List<Vec2I> result)
        {
            var bounds = (RectI)Utility.GetBounds(poly);
            bounds = new RectI(bounds.L - offsetX, bounds.T - offsetY, bounds.R - offsetX, bounds.B - offsetY);

            for (int i = 0; i < poly.Length - 1; i++)
                indicateScanLine(poly[i], poly[i + 1]);
            indicateScanLine(poly[poly.Length - 1], poly[0]);

            bool inside;
            for (int i = bounds.L; i <= bounds.R; i++)
            {
                inside = false;
                for (int j = bounds.T; j <= bounds.B; j++)
                {
                    if (idcMap[i][j] != EMPTY)
                    {
                        double z = idcMap[i][j];
                        idcMap[i][j] = EMPTY;
                        inside = !inside;

                        if (inside)
                        {
                            for (int j2 = j + 1; j2 <= bounds.B; j2++)
                            {
                                if (idcMap[i][j2] != EMPTY)
                                {
                                    inside = false;
                                    break;
                                }
                            }
                            inside = !inside;
                        }
                    }
                    else if (inside)
                        result.Add(new Vec2I(i, j));
                }
            }
        }
        private void rasterPoly(I2Dimensional[] poly, GObject2D item, Screen<GObject2D> screen)
        {
            var bounds = (RectI)Utility.GetBounds(poly);
            bounds = new RectI(bounds.L - offsetX, bounds.T - offsetY, bounds.R - offsetX, bounds.B - offsetY);

            for (int i = 0; i < poly.Length - 1; i++)
                indicateScanLine(poly[i], poly[i + 1]);
            indicateScanLine(poly[poly.Length - 1], poly[0]);

            bool inside;
            for (int i = bounds.L; i <= bounds.R; i++)
            {
                inside = false;
                for (int j = bounds.T; j <= bounds.B; j++)
                {
                    if (idcMap[i][j] != EMPTY)
                    {
                        double z = idcMap[i][j];
                        idcMap[i][j] = EMPTY;
                        inside = !inside;

                        if (inside)
                        {
                            for (int j2 = j + 1; j2 <= bounds.B; j2++)
                            {
                                if (idcMap[i][j2] != EMPTY)
                                {
                                    inside = false;
                                    break;
                                }
                            }
                            inside = !inside;
                        }
                    }
                    if (inside)
                        screen[i, j] = item;
                }
            }
        }
        private void rasterPoly(I3Dimensional[] poly, GObject item, ZBuffer<GObject> zBuffer)
        {
            var bounds = (RectI)Utility.GetBounds(poly);
            bounds = new RectI(bounds.L - offsetX, bounds.T - offsetY, bounds.R - offsetX, bounds.B - offsetY);

            for (int i = 0; i < poly.Length - 1; i++)
                indicateScanLine(poly[i], poly[i + 1]);
            indicateScanLine(poly[poly.Length - 1], poly[0]);

            bool inside;
            float sz = 0;
            float z = 0;
            for (int i = bounds.L; i <= bounds.R; i++)
            {
                inside = false;
                for (int j = bounds.T; j <= bounds.B; j++)
                {
                    if (idcMap[i][j] != EMPTY)
                    {
                        z = idcMap[i][j];
                        idcMap[i][j] = EMPTY;
                        inside = !inside;

                        if (inside)
                        {
                            for (int j2 = j + 1; j2 <= bounds.B; j2++)
                            {
                                if (idcMap[i][j2] != EMPTY)
                                {
                                    sz = (idcMap[i][j2] - z) / (j2 - j);
                                    inside = false;
                                    break;
                                }
                            }
                            inside = !inside;
                        }   
                    }
                    if (inside)
                    {
                        z += sz;
                        zBuffer.Insert(i, j, z, item);
                    }

                }
            }
        }

        private void rasterLine(I2Dimensional from, I2Dimensional to, List<Vec2I> result)
        {
            int x1 = (int)from.GetX() - offsetX;
            int x2 = (int)to.GetX() - offsetX;
            int y1 = (int)from.GetY() - offsetY;
            int y2 = (int)to.GetY() - offsetY;

            int dx = Math.Abs(x2 - x1);
            int dy = -Math.Abs(y2 - y1);
            int sx = x2 > x1 ? 1 : -1;
            int sy = y2 > y1 ? 1 : -1;
            int e2;
            int err = dx + dy;

            do
            {
                result.Add(new Vec2I(x1, y1));
                if (y1 == y2 && x1 == x2)
                    break;
                e2 = err + err;
                if (e2 > dy)
                {
                    err += dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            } while (true);
        }
        private void rasterLine(I2Dimensional from, I2Dimensional to, GObject2D item, Screen<GObject2D> screen)
        {
            int x1 = (int)from.GetX() - offsetX;
            int x2 = (int)to.GetX() - offsetX;
            int y1 = (int)from.GetY() - offsetY;
            int y2 = (int)to.GetY() - offsetY;

            int dx = Math.Abs(x2 - x1);
            int dy = -Math.Abs(y2 - y1);
            int sx = x2 > x1 ? 1 : -1;
            int sy = y2 > y1 ? 1 : -1;
            int e2;
            int err = dx + dy;

            do
            {
                screen[x1, y1] = item;
                if (y1 == y2 && x1 == x2)
                    break;
                e2 = err + err;
                if (e2 > dy)
                {
                    err += dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            } while (true);
        }
        private void rasterLine(I3Dimensional from, I3Dimensional to, GObject item, ZBuffer<GObject> zBuffer)
        {
            int x1 = (int)from.GetX() - offsetX;
            int x2 = (int)to.GetX() - offsetX;
            int y1 = (int)from.GetY() - offsetY;
            int y2 = (int)to.GetY() - offsetY;
            float z = (float)from.GetZ();

            int dx = Math.Abs(x2 - x1);
            int dy = -Math.Abs(y2 - y1);
            float dz = (float)to.GetZ() - z;
            float dzx = 0;
            float dzy = 0;
            int znorm = 0;
            if (dy < 0)
            {
                znorm++;
                dzy = -dz / dy;
            }
            if (dx > 0)
            {
                znorm++;
                dzx = dz / dx;
            }
            dzx /= znorm;
            dzy /= znorm; 


            int sx = x2 > x1 ? 1 : -1;
            int sy = y2 > y1 ? 1 : -1;
            int e2;
            int err = dx + dy;

            do
            {
                zBuffer.Insert(x1, y1, z, item);
                if (y1 == y2 && x1 == x2)
                    break;
                e2 = err + err;
                if (e2 > dy)
                {
                    err += dy;
                    x1 += sx;
                    z += dzx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                    z += dzy;
                }
            } while (true);
        }



        //prepares the indicator map to raster a polygon
        private void indicateScanLine(I2Dimensional from, I2Dimensional to)
        {
            int x1 = (int)from.GetX() - offsetX;
            int x2 = (int)to.GetX() - offsetX;
            int y1 = (int)from.GetY() - offsetY;
            int y2 = (int)to.GetY() - offsetY;

            int dx = Math.Abs(x2 - x1);
            int dy = -Math.Abs(y2 - y1);
            int sx = x2 > x1 ? 1 : -1;
            int sy = y2 > y1 ? 1 : -1;
            int e2;
            int err = dx + dy;
            do
            {
                if (y1 == y2 && x1 == x2)
                    break;
                e2 = err + err;
                if (e2 > dy)
                {
                    err += dy;
                    x1 += sx;
                    idcMap[x1][y1] = 0f;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            } while (true);
        }
        private void indicateScanLine(I3Dimensional from, I3Dimensional to)
        {
            int x1 = (int)from.GetX() - offsetX;
            int x2 = (int)to.GetX() - offsetX;
            int y1 = (int)from.GetY() - offsetY;
            int y2 = (int)to.GetY() - offsetY;
            float z = (float)from.GetZ();

            int dx = Math.Abs(x2 - x1);
            if (dx == 0)
                return;
            int dy = -Math.Abs(y2 - y1);
            float dz = (float)to.GetZ() - z;
            float dzx = 0;
            float dzy = 0;
            if (dy < 0)
            {
                dz /= 2f;
                dzy = -dz / dy;
            }
            dzx = dz / dx;


            int sx = x2 > x1 ? 1 : -1;
            int sy = y2 > y1 ? 1 : -1;
            int e2;
            int err = dx + dy;
            do
            {
                if (y1 == y2 && x1 == x2)
                    break;

                e2 = err + err;
                if (e2 > dy)
                {
                    err += dy;
                    x1 += sx;
                    z += dzx;

                    idcMap[x1][y1] = z;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                    z += dzy;
                }
            } while (true);
        }
    }


    public class Screen<T>
    {
        protected int width, height;
        protected T[][] map;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public virtual T this[int x, int y]
        {
            get { return map[x][y]; }
            set { map[x][y] = value; }
        }

        public Screen(int width, int height)
        {
            this.width = width;
            this.height = height;

            map = new T[width][];
            for (int i = 0; i < width; i++)
                map[i] = new T[height];
        }
        public Screen(int width, int height, T background)
            : this(width, height)
        {
            Clear(background);
        }

        public void Clear(T background)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                    map[i][j] = background;
            }
        }

    }

    public class ZBuffer<T> : Screen<T>
    {
        float[][] zs;

        //ZBuffer does not allow direct writing, use Insert instead
        public override T this[int x, int y]
        {
            get
            {
                return base[x, y];
            }
            set
            {
                Insert(x, y, 0, value);
            }
        }

        public ZBuffer(int width, int height)
            : base(width, height)
        {
            zs = new float[width][];
            for (int i = 0; i < width; i++)
            {
                zs[i] = new float[height];
                for (int j = 0; j < height; j++)
                    zs[i][j] = float.NegativeInfinity;
            }
        }
      public ZBuffer(int width, int height, T background)
            : base(width, height, background)
        {
            zs = new float[width][];
            for (int i = 0; i < width; i++)
            {
                zs[i] = new float[height];
                for (int j = 0; j < height; j++)
                    zs[i][j] = float.NegativeInfinity;
            }
        }

        public new void Clear(T background)
        {
            Clear(background, float.NegativeInfinity);
        }
        public void Clear(T background, float z)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    zs[i][j] = z;
                    map[i][j] = background;
                }
            }

        }

        public bool Insert(int x, int y, float z, T obj)
        {
            if (zs[x][y] < z)
            {
                zs[x][y] = z;
                map[x][y] = obj;
                return true;
            }
            return false;
        }
    }

}
