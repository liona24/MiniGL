using System;
using System.Collections.Generic;

namespace MiniGL
{
    public struct HashableLeaf : IHasBoundaries
    {
        public readonly int HashCode;
        public readonly Rect Boundaries;
        public readonly Vec3[] Points;

        public HashableLeaf(int hashCode, Vec3[] points)
        {
            HashCode = hashCode;
            Points = points;
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            foreach (var v in points)
            {
                if (v.X < minX)
                    minX = v.X;
                if (v.Y < minY)
                    minY = v.Y;
                if (v.Y > maxY)
                    maxY = v.Y;
                if (v.X > maxX)
                    maxX = v.X;
            }
            Boundaries = new Rect(minX, minY, maxX, maxY);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }

    public abstract class Painter
    {
        private int offsetX, offsetY;
        private int width, height;
        
        public int OffsetX { get { return offsetX; } set { offsetX = value; } }
        public int OffsetY { get { return offsetY; } set { offsetY = value; } }

        private Painter(int offsetX, int offsetY, int width, int height)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.width = width;
            this.height = height;
        }

        public void Paint(int code, params Vec3[] poly)
        {
            if (poly.Length == 1)
                rasterPoint(poly[0], code);
            else if (poly.Length == 2)
                rasterLine(poly[0], poly[1], code);
            else if (poly.Length > 2)
                rasterPoly(poly, code);
        }

        private abstract void rasterPoly(Vec3[] poly, int code);
        private abstract void rasterLine(Vec3 p1, Vec3 p2, int code);
        private abstract void rasterPoint(Vec3 p1, int code);
        
        public static Painter GetPainter(ZBuffer target)
        {
            return GetPainter(target, 0, 0);
        }
        public static Painter GetPainter(ZBuffer target, int offsetX, int offsetY)
        {
            return new ArrayPainter(offsetX, offsetY, target);
        }
        public static Painter GetPainter(QuadTree<HashableLeaf> target)
        {
            return GetPainter(target, 0, 0);
        }
        public static Painter GetPainter(QuadTree<HashableLeaf> target, int offsetX, int offsetY)
        {
            return new TreePainter(target, offsetX, offsetY);
        }

        private class ArrayPainter : Painter
        {
            private const float EMPTY = float.MaxValue;

            private readonly float[][] zIndicMap;
            private ZBuffer zBuffer;

            public ArrayPainter(int offsetX, int offsetY, ZBuffer target)
                : base(offsetX, offsetY, target.Width, target.Height)
            {
                zBuffer = target;
                zIndicMap = new float[zBuffer.Width][];
                for (int i = 0; i < zBuffer.Width; i++)
                {
                    zIndicMap[i] = new float[zBuffer.Height];
                    for (int j = 0; j < zBuffer.Height; j++)
                        zIndicMap[i][j] = EMPTY;
                }
            }

            private override void rasterPoly(Vec3[] poly, int code)
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

            private override void rasterLine(Vec3 from, Vec3 to, int code)
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

        private class TreePainter : Painter
        {
            QuadTree<HashableLeaf> tree;

            public TreePainter(int offsetX, int offsetY, QuadTree<HashableLeaf> target)
                : base(offsetX, offsetY, (int)target.Boundaries.Width, (int)target.Boundaries.Height)
            {
                tree = target;
            }
        }
    }
}
