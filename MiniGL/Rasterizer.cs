using System;
using System.Collections.Generic;

namespace MiniGL
{
    public class Rasterizer
    {
        //Marks the zMap as empty
        const float EMPTY = 1f; //choice is just a positive number since the common view direction is along the negative z axis

        int width, height;
        int offsetX, offsetY;

        readonly float[][] zIndicMap; //used to render polygons

        public int OffsetX { get { return offsetX; } set { offsetX = value; } }
        public int OffsetY { get { return offsetY; } set { offsetY = value; } }

        public Rasterizer(int width, int height, int offsetX, int offsetY)
        {
            this.width = width;
            this.height = height;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            zIndicMap = new float[width][];
            for (int i = 0; i < width; i++)
            {
                zIndicMap[i] = new float[height];
                for (int j = 0; j < height; j++)
                    zIndicMap[i][j] = EMPTY;
            }
        }

        public void Rasterize(int code, ZBuffer zBuffer, params Vec3[] poly)
        {
            if (poly.Length == 1)
                zBuffer.TryInsert((int)poly[0].X, (int)poly[0].Y, (int)poly[0].Z, code);
            else if (poly.Length == 2)
                rasterLine(poly[0], poly[1], code, zBuffer);
            else if (poly.Length > 2)
                rasterPoly(poly, code, zBuffer);
        }

        private void rasterPoly(Vec3[] poly, int code, ZBuffer zBuffer)
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
                    if (zIndicMap[i][j] != EMPTY)
                    {
                        z = zIndicMap[i][j];
                        zIndicMap[i][j] = EMPTY;
                        inside = !inside;

                        if (inside)
                        {
                            for (int j2 = j + 1; j2 <= bounds.B; j2++)
                            {
                                if (zIndicMap[i][j2] != EMPTY)
                                {
                                    sz = (zIndicMap[i][j2] - z) / (j2 - j);
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
                        zBuffer.TryInsert(i, j, z, code);
                    }

                }
            }
        }

        private void rasterLine(Vec3 from, Vec3 to, int code, ZBuffer zBuffer)
        {
            int x1 = (int)from.X - offsetX;
            int x2 = (int)to.X - offsetX;
            int y1 = (int)from.Y - offsetY;
            int y2 = (int)to.Y - offsetY;
            float z = (float)from.Z;

            int dx = Math.Abs(x2 - x1);
            int dy = -Math.Abs(y2 - y1);
            float dz = (float)to.Z - z;
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
                zBuffer.TryInsert(x1, y1, z, code);
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

        private void indicateScanLine(Vec3 from, Vec3 to)
        {
            int x1 = (int)from.X - offsetX;
            int x2 = (int)to.X - offsetX;
            int y1 = (int)from.Y - offsetY;
            int y2 = (int)to.Y - offsetY;
            float z = (float)from.Z;

            int dx = Math.Abs(x2 - x1);
            if (dx == 0)
                return;
            int dy = -Math.Abs(y2 - y1);
            float dz = (float)to.Z - z;
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

                    zIndicMap[x1][y1] = z;
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
}
