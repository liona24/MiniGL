using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MiniGL
{
    public static class Utility
    {
        public static bool PointInTri(Vec2 p, Vec2 t1,
                                        Vec2 t2, Vec2 t3)
        {
            double a = ((t2.Y - t3.Y) * (p.X - t3.X) + (t3.X - t2.X) * (p.Y - t3.Y)) /
                   ((t2.Y - t3.Y) * (t1.X - t3.X) + (t3.X - t2.X) * (t1.Y - t3.Y));
            double b = ((t3.Y - t1.Y) * (p.X - t3.X) + (t1.X - t3.X) * (p.Y - t3.Y)) /
                   ((t2.Y - t3.Y) * (t1.X - t3.X) + (t3.X - t2.X) * (t1.Y - t3.Y));

            return a > 0 && b > 0 && 1 - a - b > 0;
        }

        public static bool PointInPoly(Vec2 point, I2Dimensional[] poly)
        {
            int res = rayIntersection(point, poly[poly.Length - 1].GetX(), 
                                            poly[poly.Length - 1].GetY(), 
                                            poly[0].GetX(), 
                                            poly[0].GetY());
            for (int i = 0; i < poly.Length - 1; i++)
            {
                res *=  rayIntersection(point, poly[i].GetX(), 
                                            poly[i].GetY(), 
                                            poly[i + 1].GetX(), 
                                            poly[i + 1].GetY());
            }

            return res > 0;
        }

        private static int rayIntersection(Vec2 point, double x1, double y1, double x2, double y2)
        {
            if (point.Y == y1 && y1 == y2)
            {
                if (point.X <= x2 && x1 <= point.X || point.X <= x1 && x2 <= point.X)
                    return 1;
                return -1;
            }
            if (y1 > y2)
            {
                double tmp = y1;
                y1 = y2;
                y2 = tmp;
                tmp = x1;
                x1 = x2;
                x2 = tmp;
            }
            if (point.Y <= y1 || point.Y >= y2)
                return 1;
            double delta = (x1 - point.X) * (y2 - point.Y) - (y1 - point.Y) * (x2 - point.X);
            if (delta >= 0)
                return -1;
            return 1;
        }


        public static Vec2 IntersectionLineLine(Vec2 from1, Vec2 to1, Vec2 from2, Vec2 to2)
        {
            var dir1 = to1 - from1;
            var m = new Matrix2(-dir1.X, to2.X - from2.X, -dir1.Y, to2.Y - from2.Y);
            m.Inverse();
            var res = m * (from1 - from2);
            return new Vec2(from1.X + dir1.X * res.X, from1.Y + dir1.Y * res.X);
        }
        //first 3 parameters describe a plane, last two the line 
        /// <summary>
        /// Returns point of intersection between a plane and a line:
        /// planeOrigin + a * p + b * q = from + (to - from) * c
        /// </summary>
        /// <param name="planeOrigin">Plane support vector</param>
        /// <param name="p">plane direction 1</param>
        /// <param name="q">plane direction 2</param>
        /// <param name="from">Line support vector</param>
        /// <param name="to">Line through</param>
        /// <returns>Returns the point of intersection, empty point if no intersection occurs</returns>
        public static Vec3 IntersectionPlaneLine(Vec3 planeOrigin, Vec3 p, Vec3 q, Vec3 from, Vec3 to)
        {
            var dir = to - from;
            var m = new Matrix3(planeOrigin.X - p.X, planeOrigin.X - q.X, dir.X, 
                                planeOrigin.Y - p.Y, planeOrigin.Y - q.Y, dir.Y,
                                planeOrigin.Z - p.Z, planeOrigin.Z - q.Z, dir.Z);
            m.Inverse();
            var res = m * (planeOrigin - from);
            return new Vec3(from.X + dir.X * res.Z, from.Y + dir.Y * res.Z, from.Z + dir.Z * res.Z);
        }

        public static Vec2[][] Polygon2Triangles(Vec2[] poly)
        {
            var tris = new Vec2[poly.Length - 2][];
            int k = 0;

            var list = new LinkedList<Vec2>();
            for (int i = 0; i < poly.Length; i++)
                list.AddLast(poly[i]);

            var node = list.First;
            while (node.Next != null &&
                    node.Next.Next != null)
            {
                var p1 = node.Value;
                var p2 = node.Next.Value;
                var p3 = node.Next.Next.Value;
                bool doRemove = true;
                var forward = node.Next.Next;
                while (forward.Next != null)
                {
                    forward = forward.Next;
                    if (PointInTri(forward.Value, p1, p2, p3))
                    {
                        doRemove = false;
                        break;
                    }
                }
                if (doRemove)
                {
                    var backward = node;
                    while (backward.Previous != null)
                    {
                        backward = backward.Previous;
                        if (PointInTri(backward.Value, p1, p2, p3))
                        {
                            doRemove = false;
                            break;
                        }
                    }

                    if (doRemove)
                    {
                        list.Remove(node.Next);
                        tris[k++] = new Vec2[] { p1, p2, p3 };
                    }
                }
                node = node.Next;
            }
            return tris;
        }
        public static Vec2I[][] Polygon2Triangles(Vec2I[] poly)
        {
            var tris = new Vec2I[poly.Length - 2][];
            int k = 0;

            var list = new LinkedList<Vec2I>();
            for (int i = 0; i < poly.Length; i++)
                list.AddLast(poly[i]);

            var node = list.First;
            while (node.Next != null &&
                    node.Next.Next != null)
            {
                var p1 = node.Value;
                var p2 = node.Next.Value;
                var p3 = node.Next.Next.Value;
                bool doRemove = true;
                var forward = node.Next.Next;
                while (forward.Next != null)
                {
                    forward = forward.Next;
                    if (PointInTri(forward.Value, p1, p2, p3))
                    {
                        doRemove = false;
                        break;
                    }
                }
                if (doRemove)
                {
                    var backward = node;
                    while (backward.Previous != null)
                    {
                        backward = backward.Previous;
                        if (PointInTri(backward.Value, p1, p2, p3))
                        {
                            doRemove = false;
                            break;
                        }
                    }

                    if (doRemove)
                    {
                        list.Remove(node.Next);
                        tris[k++] = new Vec2I[] { p1, p2, p3 };
                    }
                }
                node = node.Next;
            }
            return tris;
        }

        public static Rect GetBounds(I2Dimensional[] coll)
        {
            double l = double.MaxValue;
            double r = double.MinValue;
            double t = double.MaxValue;
            double b = double.MinValue;
            for (int i = 0; i < coll.Length; i++)
            {
                if (coll[i].GetX() < l)
                    l = coll[i].GetX();
                if (coll[i].GetX() > r)
                    r = coll[i].GetX();
                if (coll[i].GetY() < t)
                    t = coll[i].GetY();
                if (coll[i].GetY() > b)
                    b = coll[i].GetY();
            }
            return new Rect(l, t, r, b);
        }
        public static RectI GetBounds(Vec2I[] coll)
        {
            int l = int.MaxValue;
            int r = int.MinValue;
            int t = int.MaxValue;
            int b = int.MinValue;
            for (int i = 0; i < coll.Length; i++)
            {
                if (coll[i].X < l)
                    l = coll[i].X;
                else if (coll[i].X > r)
                    r = coll[i].X;
                if (coll[i].Y < t)
                    t = coll[i].Y;
                else if (coll[i].Y > b)
                    b = coll[i].Y;
            }
            return new RectI(l, t, r, b);
        }

        public static void MarkContour(int[][] map, Vec2I start, int[][] markMap, int label)
        {
            int width = map.Length + 2;
            int height = map[0].Length + 2;
            start = new Vec2I(start.X + 1, start.Y + 1);
            var extMap = new int[width][];
            for (int k = 1; k < width - 1; k++)
            {
                extMap[k] = new int[height];
                for (int n = 1; n < height - 1; n++)
                    extMap[k][n] = map[k - 1][n - 1];
            }
            
            int r = extMap[start.X][start.Y];
            int border = -1 - r;
            extMap[0] = new int[height];
            extMap[width - 1] = new int[height];
            for (int n = 0; n < height; n++)
            {
                extMap[0][n] = border;
                extMap[width - 1][n] = border;
            }
            for (int k = 0; k < width; k++)
            {
                extMap[k][0] = border;
                extMap[k][height - 1] = border;
            }
            
            int i = start.X;
            for (; i < width; i++)
            {
                if (extMap[i][start.Y] != r)
                    break;
            }
            markMap[i - 2][start.Y - 1] = label;
            int fi = i - 1;
            int fj = start.Y;
            int j = start.Y;
            Vec2I dir = new Vec2I(0, 1);
            //special case of having a contour like this:
            // ##
            // ### <- start here
            // ##
            // requires the stop criteria to be ignored once, therefor special loop that covers this case
            for (int k = 0; k < 4; k++)
            {
                i += dir.X;
                j += dir.Y;

                if (extMap[i][j] == r)
                {
                    markMap[i - 1][j - 1] = label;
                    dir = new Vec2I(dir.Y, -dir.X);
                }
                else
                    dir = new Vec2I(-dir.Y, dir.X);
            }
            do
            {
                i += dir.X;
                j += dir.Y;

                if (extMap[i][j] == r)
                {
                    markMap[i - 1][j - 1] = label;
                    dir = new Vec2I(dir.Y, -dir.X);
                }
                else
                    dir = new Vec2I(-dir.Y, dir.X);

            } while (i != fi || j != fj);
        }
        public static void MarkCluster(int[][] map, Vec2I start, int[][] markMap, int label, bool fillHoles = true)
        {
            int width = map.Length + 2;
            int height = map[0].Length + 2;
            start = new Vec2I(start.X + 1, start.Y + 1);
            var extMap = new int[width][];
            for (int k = 1; k < width - 1; k++)
            {
                extMap[k] = new int[height];
                for (int n = 1; n < height - 1; n++)
                    extMap[k][n] = map[k - 1][n - 1];
            }
            
            int r = extMap[start.X][start.Y];
            int border = -1 - r;
            extMap[0] = new int[height];
            extMap[width - 1] = new int[height];
            for (int n = 0; n < height; n++)
            {
                extMap[0][n] = border;
                extMap[width - 1][n] = border;
            }
            for (int k = 0; k < width; k++)
            {
                extMap[k][0] = border;
                extMap[k][height - 1] = border;
            }
            int i = start.X;
            for (; i < width; i++)
            {
                if (extMap[i][start.Y] != r)
                    break;
            }
            markMap[i - 2][start.Y - 1] = label;
            int fi = i;
            int fj = start.Y;
            int j = start.Y;
            Vec2I dir = new Vec2I(0, 1);
            int minX = width;
            int minY = height;
            int maxX = 0;
            int maxY = 0;
            for (int k = 0; k < 4; k++)
            {
                i += dir.X;
                j += dir.Y;

                if (extMap[i][j] == r)
                {
                    if (i < minX)
                        minX = i;
                    else if (i > maxX)
                        maxX = i;
                    if (j < minY)
                        minY = j;
                    else if (j > maxY)
                        maxY = j;
                    markMap[i - 1][j - 1] = label;
                    dir = new Vec2I(dir.Y, -dir.X);
                }
                else
                    dir = new Vec2I(-dir.Y, dir.X);
            }
            do
            {
                i += dir.X;
                j += dir.Y;

                if (extMap[i][j] == r)
                {
                    if (i < minX)
                        minX = i;
                    else if (i > maxX)
                        maxX = i;
                    if (j < minY)
                        minY = j;
                    else if (j > maxY)
                        maxY = j;
                    markMap[i - 1][j - 1] = label;
                    dir = new Vec2I(dir.Y, -dir.X);
                }
                else
                    dir = new Vec2I(-dir.Y, dir.X);
            } while (i != fi || j != fj);

            maxX = Math.Min(maxX, width - 3);
            maxY = Math.Min(maxY, height - 3);

            i = minX;

            for (; i <= maxX; i++)
            {
                j = minY;
                for (; j <= maxY; j++)
                {
                    if (markMap[i][j] == label)
                    {
                        for (; j < maxY; j++)
                        {
                            if (markMap[i][j + 1] != label)
                                break;
                        }
                        int count = -1;
                        int s = ++j;
                        for (; j < maxY; j++)
                        {
                            count--;
                            if (markMap[i][j + 1] == label)
                            {
                                count *= -1;
                                break;
                            }
                        }
                        for (int j2 = s; j2 < s + count; j2++)
                        {
                            if (fillHoles || map[i][j2] == r)
                                markMap[i][j2] = label;
                        }
                    }
                }
            }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct DoubleLongUnion
    {
        [FieldOffset(0)]
        public double Double;
        [FieldOffset(0)]
        public long Long;
    }

    [Serializable]
    public struct Vec2I
    {
        public int X, Y;

        public Vec2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        public int Length2()
        {
            return X * X + Y * Y;
        }

        public double GetX() { return X; }
        public double GetY() { return Y; }

        public override string ToString()
        {
            return "X=" + X.ToString() + " Y=" + Y.ToString(); 
        }

        public static Vec2I operator + (Vec2I l, Vec2I r)
        {
            return new Vec2I(l.X + r.X, l.Y + r.Y);
        }
        public static Vec2I operator - (Vec2I l, Vec2I r)
        {
            return new Vec2I(l.X - r.X, l.Y - r.Y);
        }
        public static Vec2I operator * (Vec2I l, int scalar)
        {
            return new Vec2I(l.X * scalar, l.Y * scalar);
        }
        public static int operator * (Vec2I l, Vec2I r)
        {
            return l.X * r.X + l.Y * l.Y;
        }
        public static implicit operator Vec2(Vec2I p)
        {
            return new Vec2(p.X, p.Y);
        }
    }
    [Serializable]
    public struct Vec3I
    {
        public int X, Y, Z;

        public Vec3I(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        public int Length2()
        {
            return X * X + Y * Y + Z * Z;
        }

        public override string ToString()
        {
            return "X=" + X.ToString("N5") + " Y=" + Y.ToString("N5") + " Z=" + Z.ToString("N5");
        }

        public static Vec3I Cross(Vec3I l, Vec3I r)
        {
            return new Vec3I(l.Y * r.Z - l.Z * r.Y, l.Z * r.X - l.X * r.Z, l.X * r.Y - l.Y * r.X);
        }

        public static Vec3I operator+ (Vec3I l, Vec3I r)
        {
            return new Vec3I(l.X + r.X, l.Y + r.Y, l.Z + l.Y);
        }
        public static Vec3I operator- (Vec3I l, Vec3I r)
        {
            return new Vec3I(l.X - r.X, l.Y - r.Y, l.Z - r.Y);
        }
        public static Vec3I operator* (Vec3I l, int scalar)
        {
            return new Vec3I(l.X * scalar, l.Y * scalar, l.Z * scalar);
        }
        public static int operator* (Vec3I l, Vec3I r)
        {
            return l.X * r.X + l.Y * r.Y + l.Z * r.Z;
        }
        public static implicit operator Vec3(Vec3I p)
        {
            return new Vec3(p.X, p.Y, p.Z);
        }
    }
    [Serializable]
    public struct RectI
    {
        public int L, T, R, B;
        public RectI(int l, int t, int r, int b)
        {
            L = l;
            T = t;
            R = r;
            B = b;
        }

        public int Area { get { return (R - L) * (B - T); } }
        public int Width { get { return R - L; } }
        public int Height { get { return B - T; } }
        public Vec2I Origin { get { return new Vec2I(L, T); } }

        public Vec2I Position { get { return new Vec2I(L, T); } }

        public bool IntersectsWith(RectI other)
        {
            return !(R < other.L || other.R < L || B < other.T || other.B < T);
        }
        public bool Contains(RectI small)
        {
            return (R >= small.R && L <= small.L && T <= small.T && B >= small.B);
        }
        public bool Contains(Vec2I p)
        {
            return (L <= p.X && R >= p.X && T <= p.Y && B >= p.Y);
        }
        public bool Continues(RectI other)
        {
            return (R == other.R && L == other.L && ( B == other.T || T == other.B )) ||
                (B == other.B && T == other.T && ( L == other.R || R == other.L ));
        }

        public override string ToString()
        {
            return string.Format("Rect L={0} T={1} R={2} B={3}", L, T, R, B);
        }

        public static RectI FromXYWH(int x, int y, int w, int h)
        {
            return new RectI(x, y, x+w, y+h);
        }
        public static implicit operator Rect(RectI r)
        {
            return new Rect(r.L, r.T, r.R, r.B);
        }
    }

    [Serializable]
    public struct Vec2 : I2Dimensional
    {
        private double _x, _y;

        public double X { get { return _x; }  set { _x = value; } }
        public double Y { get { return _y; }  set { _y = value; } }

        public double GetX()
        { return _x; }
        public double GetY()
        { return _y; }

        public Vec2(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double Length()
        {
            return Math.Sqrt(_x * _x + _y * _y);
        }
        public double Length2()
        {
            return _x * _x + _y * _y;
        }
        public void Normalize()
        {
            double v = _x * _x + _y * _y;
            if (v != 0)
            {
                v = Math.Sqrt(v);
                _x = _x / v;
                _y = _y / v;
            }
        }

        public override string ToString()
        {
            return "_x=" + _x.ToString("N5") + " _y=" + _y.ToString("N5");
        }

        public static Vec2 operator+ (Vec2 l, Vec2 r)
        {
            return new Vec2(l._x + r._x, l._y + r._y);
        }
        public static Vec2 operator- (Vec2 l, Vec2 r)
        {
            return new Vec2(l._x - r._x, l._y - r._y);
        }
        public static Vec2 operator* (Vec2 l, double scalar)
        {
            return new Vec2(l._x * scalar, l._y * scalar);
        }
        public static Vec2 operator* (double scalar, Vec2 l)
        {
            return new Vec2(l._x * scalar, l._y * scalar);
        }
        public static double operator* (Vec2 l, Vec2 r)
        {
            return l._x * r._x + l._y * r._y;
        }
        public static explicit operator Vec2I(Vec2 p)
        {
            return new Vec2I((int)p._x, (int)p._y);
        }

        public static Vec2 FromHomo(Vec3 homo)
        {
            return new Vec2(homo.X / homo.Z, homo.Y / homo.Z);
        }

    }
    [Serializable]
    public struct Vec3 : I3Dimensional
    {
        private double _x,_y,_z;

        public double X { get { return _x; }  set { _x = value; } }
        public double Y { get { return _y; }  set { _y = value; } }
        public double Z { get { return _z; }  set { _z = value; } }

        public Vec3(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        public Vec3(Vec2 p, double z)
        {
            _x = p.X;
            _y = p.Y;
            _z = z;
        }
        public double GetX()
        { return _x; }
        public double GetY()
        { return _y; }
        public double GetZ()
        { return _z; }
        public double Length()
        {
            return Math.Sqrt(_x * _x + _y * _y + _z * _z);
        }
        public double Length2()
        {
            return _x * _x + _y * _y + _z * _z;
        }
        public void Normalize()
        {
            double v = _x * _x + _y * _y + _z * _z;
            if (v != 0)
            {
                v = Math.Sqrt(v);
                _x = _x / v;
                _y = _y / v;
                _z = _z / v;
            }
        }

        public override string ToString()
        {
            return "_x=" + _x.ToString("N5") + " _y=" + _y.ToString("N5") + " _z=" + _z.ToString("N5");
        }

        public static Vec3 Cross(Vec3 l, Vec3 r)
        {
            return new Vec3(l._y * r._z - l._z * r._y, l._z * r._x - l._x * r._z, l._x * r._y - l._y * r._x);
        }

        public Vec3 WeightThis(Vec3 w)
        {
            return new Vec3(_x * w._x, _y * w._y, _z * w._z);
        }

        public static Vec3 FromHomo(Vec4 h)
        {
            return new Vec3(h.X / h.W, h.Y / h.W, h.Z / h.W);
        }

        public static Vec3 operator+ (Vec3 l, Vec3 r)
        {
            return new Vec3(l._x + r._x, l._y + r._y, l._z + r._z);
        }
        public static Vec3 operator- (Vec3 l, Vec3 r)
        {
            return new Vec3(l._x - r._x, l._y - r._y, l._z - r._z);
        }
        public static Vec3 operator* (Vec3 v, double scalar)
        {
            return new Vec3(v._x * scalar, v._y * scalar, v._z * scalar);
        }
        public static Vec3 operator *(double scalar, Vec3 v)
        {
            return new Vec3(v._x * scalar, v._y * scalar, v._z * scalar);
        }
        public static double operator* (Vec3 l, Vec3 r)
        {
            return l._x * r._x + l._y * r._y + l._z * r._z;
        }
        public static explicit operator Vec3I(Vec3 p)
        {
            return new Vec3I((int)p._x, (int)p._y, (int)p._z);
        }
    }
    [Serializable]
    public struct Vec4 : I4Dimensional
    {
        private double _x, _y, _z, _w;


        public double X { get { return _x; }  set { _x = value; } }
        public double Y { get { return _y; }  set { _y = value; } }
        public double Z { get { return _z; }  set { _z = value; } }
        public double W { get { return _w; }  set { _w = value; } }

        public Vec4(double x, double y, double z, double w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }
        public Vec4(Vec3 v, double w)
        {
            _x = v.X;
            _y = v.Y;
            _z = v.Z;
            _w = w;
        }
        public double GetX()
        { return _x; }
        public double GetY()
        { return _y; }
        public double GetZ()
        { return _z; }
        public double GetW()
        { return _w; }

        public double Length()
        {
            return Math.Sqrt(Length2());
        }
        public double Length2()
        {
            if (Math.Abs(_w) < 0.0001)
                return _x * _x + _y * _y + _z * _z;
            double n = 1 / _w / _w;
            return _x * _x * n + _y * _y * n + _z * _z * n;
        }

        public void MakeUnit()
        {
            Normalize();
            double l = Math.Sqrt(_x * _x + _y * _y + _z * _z);
            if (l > 0)
            {
                l = 1 / l;
                _x *= l;
                _y *= l;
                _z *= l;
            }
        }
        public void Normalize()
        {
            if (_w == 0 || _w == 1)
                return;
            _x /= _w;
            _y /= _w;
            _z /= _w;
            _w = 1;
        }

        public Vec3 ToCartesian()
        {
            if (_w == 0)
                return new Vec3(_x, _y, _z);
            return new Vec3(_x / _w, _y / _w, _z / _w);
        }

        public override string ToString()
        {
            return "_x=" + _x.ToString("N3") + " _y=" + _y.ToString("N3") + " _z=" + _z.ToString("N3") + " _w=" + _w.ToString("N3");
        }

        public override int GetHashCode()
        {
            Normalize();
            long lHash = new DoubleLongUnion { Double = _x }.Long ^ 
                        new DoubleLongUnion { Double = _y }.Long ^
                        new DoubleLongUnion { Double = _z }.Long;
            return (int)(lHash ^ (lHash >> 32));
        }

        public static Vec4 operator +(Vec4 l, Vec4 r)
        {
            return new Vec4(l._x + r._x, l._y + r._y, l._z + r._z, l._w + r._w);
        }
        public static Vec4 operator -(Vec4 l, Vec4 r)
        {
            return new Vec4(l._x - r._x, l._y - r._y, l._z - r._z, l._w - r._w);
        }
        public static Vec4 operator *(Vec4 v, double scalar)
        {
            return new Vec4(v._x, v._y, v._z, v._w / scalar);
        }
        public static Vec4 operator *(double scalar, Vec4 v)
        {
            return new Vec4(v._x, v._y, v._z, v._w / scalar);
        }
    }
    [Serializable]
    public struct Rect
    {
        public double L, T, R, B;
        public Rect(double l, double t, double r, double b)
        {
            L = l;
            T = t;
            R = r;
            B = b;
        }

        public double Area { get { return (R - L) * (B - T); } }
        public double Width { get { return R - L; } }
        public double Height { get { return B - T; } }
        public Vec2 Origin { get { return new Vec2(L, T); } }

        public Vec2 Position { get { return new Vec2(L, T); } }

        public bool IntersectsWith(Rect other)
        {
            return !(R < other.L || other.R < L || B < other.T || other.B < T);
        }
        public bool Contains(Rect small)
        {
            return (R >= small.R && L <= small.L && T <= small.T && B >= small.B);
        }
        public bool Contains(Vec2 p)
        {
            return (L <= p.X && R >= p.X && T <= p.Y && B >= p.Y);
        }
        public bool Contains(double x, double y)
        {
            return (L <= x && R >= x && T <= y && B >= y);
        }

        public static Rect FromXYWH(double x, double y, double w, double h)
        {
            return new Rect(x, y, x+w, y+h);
        }
        public static explicit operator RectI(Rect r)
        {
            return new RectI((int)r.L, (int)r.T, (int)r.R, (int)r.B);
        }
    }
    [Serializable]
    public struct Cuboid
    {
        public double L; //Left
        public double T; //Top
        public double N; //Near
        public double R; //Right
        public double B; //Bot
        public double F; //Far

        public double Width { get { return R - L; } }
        public double Height { get { return B - T; } }
        public double Depth { get { return F - N; } }

        public double X { get { return L; } }
        public double Y { get { return T; } }
        public double Z { get { return N; } }
        
        public Cuboid(double l, double t, double n, double r, double b, double f)
        {
            L = l;
            T = t;
            N = n;
            R = r;
            B = b;
            F = f;
        }
        public Cuboid(Vec3 pos, Vec3 size)
        {
            L = pos.X;
            T = pos.Y;
            N = pos.Z;
            R = pos.X + size.X;
            B = pos.Y + size.Y;
            F = pos.Z + size.Z;
        }

        public bool Contains(Cuboid small)
        {
            return (L <= small.L && R >= small.R &&
                T <= small.T && B >= small.B &&
                N <= small.N && F >= small.F);
        }
        public bool Contains(Vec3 point)
        {
            return (L <= point.X && R >= point.X &&
                T <= point.Y && B >= point.Y &&
                N <= point.Z && F >= point.Z);
        }

        public bool IntersectsWith(Cuboid other)
        {
            return !(R < other.L || other.R < L || B < other.T || other.B < T ||
                N < other.F || other.N < F);
        }

    }

    public interface IMatrix : ICloneable
    {
        int Width { get; }
        int Height { get; }
        double this[int i, int j] { get; set; }
        double this[int i] { get; set; }
        double Det();
        double Trace();
        bool Inverse();
        void Transpose();

        void Apply(Func<int, int, double> f);
        void Apply(Func<int, double> f);
        void Apply(Func<double, double> f);
    }
    public class Matrix2 : IMatrix
    {
        // v0 v1
        // v2 v3
        double[] v;

        public double this[int i] { get { return v[i]; } set { v[i] = value; } }
        public double this[int i, int j] { get { return v[j * 2 + i]; } set { v[j * 2 + i] = value; } }

        public int Width { get { return 2; } }
        public int Height { get { return 2; } }

        public Matrix2(double[] values)
        {
            v = values;
        }
        public Matrix2(double v11, double v12, double v21, double v22)
        {
            v = new double[4];
            v[0] = v11;
            v[1] = v12;
            v[2] = v21;
            v[3] = v22;
        }

        public void Transpose()
        {
            double v21 = v[2];
            v[2] = v[1];
            v[1] = v21;
        }
        public override string ToString()
        {
            return "[" + v[0].ToString("N3") + " " + v[1].ToString("N3") + "]\n[" + v[2].ToString("N3") + " " + v[3].ToString("N3") + "]";
        }

        public double Det()
        {
            return v[0] * v[3] - v[1] * v[2];
        }
        public double Trace()
        {
            return v[0] + v[3];
        }
        public bool Inverse()
        {
            double det = Det();
            if (det == 0)
            {
                v = new double[] { 1, 0, 0, 1 };
                return false;
            }
            det = 1 / det;
            v = new double[] { v[3] * det, -v[1] * det, -v[2] * det, v[0] * det };
            return true;
        }

        public void Add(Matrix2 r)
        {
            v[0] += r[0];
            v[1] += r[1];
            v[2] += r[2];
            v[3] += r[3];
        }
        public void Sub(Matrix2 r)
        {
            v[0] -= r[0];
            v[1] -= r[1];
            v[2] -= r[2];
            v[3] -= r[3];
        }

        public void Apply(Func<int, double> f)
        {
            for (int i = 0; i < 4; i++)
                v[i] = f(i);
        }
        public void Apply(Func<int, int, double> f)
        {
            v[0] = f(0, 0);
            v[1] = f(1, 0);
            v[2] = f(0, 1);
            v[3] = f(1, 1);
        }
        public void Apply(Func<double, double> f)
        {
            for (int i = 0; i < 4; i++)
                v[i] = f(v[i]);
        }

        public object Clone()
        {
            return new Matrix2(v[0], v[1], v[2], v[3]);
        }

        public static Matrix2 GetIdentity()
        {
            return new Matrix2(1, 0, 0, 1);
        }

        public static Matrix2 operator +(Matrix2 l, Matrix2 r)
        {
            return new Matrix2(l[0] + r[0], l[1] + r[1], l[2] + r[2], l[3] + r[3]);
        }
        public static Matrix2 operator -(Matrix2 l, Matrix2 r)
        {
            return new Matrix2(l[0] + r[0], l[1] + r[1], l[2] + r[2], l[3] + r[3]);
        }
        public static Matrix2 operator *(Matrix2 l, Matrix2 r)
        {
            return new Matrix2(l[0] * r[0] + l[1] * r[2], 
                                l[0] * r[1] + l[1] * r[3],  
                                l[2] * r[0] + l[3] * r[2],
                                l[2] * r[1] + l[3] * r[3]);
        }
        public static Matrix2 operator *(Matrix2 l, double s)
        {
            return new Matrix2(l[0] * s, l[1] * s, l[2] * s, l[3] * s);
        }
        public static Vec2 operator *(Matrix2 l, I2Dimensional r)
        {
            return new Vec2(l[0] * r.X + l[1] * r.Y, l[2] * r.X + l[3] * r.Y);
        }
    }
    public class Matrix3 : IMatrix
    {
        double[] v;

        public double this[int i] { get { return v[i]; } set { v[i] = value; } }
        public double this[int i, int j] { get { return v[i * 3 + j]; } set { v[i * 3 + j] = value; } }

        public int Width { get { return 3; } }
        public int Height { get { return 3; } }

        public Matrix3(double[] values)
        {
            v = values;
        }
        public Matrix3(double v11, double v12, double v13, double v21, double v22, double v23, double v31, double v32, double v33)
        {
            v = new double[9];
            v[0] = v11;
            v[1] = v12;
            v[2] = v13;
            v[3] = v21;
            v[4] = v22;
            v[5] = v23;
            v[6] = v31;
            v[7] = v32;
            v[8] = v33;
        }

        public double Det()
        {
            return v[0] * v[4] * v[8] + v[3] * v[7] * v[2] + v[6] * v[1] * v[5] - v[0] * v[7] * v[5] - v[6] * v[4] * v[2] - v[3] * v[1] * v[8];
        }
        public double Trace()
        {
            return v[0] + v[4] + v[8];
        }
        public void Transpose()
        {
            double[] v = new double[9];
            int k = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    v[k++] = v[j * 3 + i];
                }
            }
            this.v = v;
        }
        public bool Inverse()
        {
            double det = Det();
            if (det == 0)
            {
                v = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1 };
                return false;
            }
            det = 1 / det;
            v = new double[] {
                v[4]*v[8] - v[5]*v[7],  v[2]*v[7] - v[1]*v[8],  v[1]*v[5] - v[2]*v[4],
                v[5]*v[6] - v[3]*v[8],  v[0]*v[8] - v[2]*v[6],  v[2]*v[3] - v[0]*v[5],
                v[3]*v[7] - v[4]*v[6],  v[1]*v[6] - v[0]*v[7],  v[0]*v[4] - v[1]*v[3] };
            for (int i = 0; i < 9; i++)
                v[i] *= det;

            return true;
        }
        
        public void Add(Matrix3 r)
        {
            for (int i = 0; i < 9; i++)
                v[i] += r[i];
        }
        public void Sub(Matrix3 r)
        {
            for (int i = 0; i < 9; i++)
                v[i] -= r[i];
        }

        public void Apply(Func<int, double> f)
        {
            for (int i = 0; i < 9; i++)
                v[i] = f(i);
        }
        public void Apply(Func<int, int, double> f)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    v[j * 3 + i] = f(i, j);
            }
        }
        public void Apply(Func<double, double> f)
        {
            for (int i = 0; i < 9; i++)
                v[i] = f(v[i]);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 3; i++)
            {
                s += "[ ";
                for (int j = 0; j < 3; j++)
                {
                    s += v[i * 3 + j].ToString("N4") + " ";
                }
                s += "]\n";
            }
            return s;
        }

        public object Clone()
        {
            double[] nV = new double[9];
            for (int i = 0; i < 9; i++)
                nV[i] = v[i];
            return new Matrix3(nV);
        }

        public static Matrix3 operator +(Matrix3 l, Matrix3 r)
        {
            var v = new double[9];
            for (int i = 0; i < 9; i++)
                v[i] = l[i] + r[i];
            return new Matrix3(v);
        }
        public static Matrix3 operator -(Matrix3 l, Matrix3 r)
        {
            var v = new double[9];
            for (int i = 0; i < 9; i++)
                v[i] = l[i] - r[i];
            return new Matrix3(v);
        }
        public static Matrix3 operator *(Matrix3 l, double scalar)
        {
            var v = new double[9];
            for (int i = 0; i < 9; i++)
                v[i] = l[i] * scalar;
            return new Matrix3(v);
        }
        public static Matrix3 operator *(Matrix3 l, Matrix3 r)
        {
            var v = new double[9];
            int k = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    v[k++] = l[i, 0] * r[0, j] +
                            l[i, 1] * r[1, j] +
                            l[i, 2] * r[2, j];
                }
            }
            return new Matrix3(v);
        }
        public static Vec3 operator *(Matrix3 l, I3Dimensional r)
        {
            return new Vec3(l[0] * r.X + l[1] * r.Y + l[2] * r.Z, 
                l[3] * r.X + l[4] * r.Y + l[5] * r.Z, 
                l[6] * r.X + l[7] * r.Y + l[8] * r.Z);
        }

        public static Matrix3 GetIdentity()
        {
            return new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);
        }

    }
    public class Matrix4 : IMatrix
    {
        public double this[int i, int j]
        {
            get { return v[4 * i + j]; }
            set { v[4 * i + j] = value; }
        }
        public double this[int i]
        {
            get { return v[i]; }
            set { v[i] = value; }
        }

        public int Width { get { return 4; } }
        public int Height { get { return 4; } }

        //row major order
        private double[] v;

        public Matrix4(double[][] data)
        {
            v = new double[16];
            int k = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    v[k++] = data[i][j];
                }
            }
        }
        public Matrix4(double[] data)
        {
            if (data.Length != 16)
                throw new ArgumentOutOfRangeException("data.Length != 16 in TMatrix4 !");
            v = data;
        }

        public void SetRow(int i, double[] row)
        {
            for (int j = 0; j < 4; j++)
            {
                v[i * 4 + j] = row[j];
            }
        }
        public void SetCollumn(int i, double[] col)
        {
            for (int j = 0; j < 4; j++)
            {
                v[j * 4 + i] = col[j];
            }
        }
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < 4; i++)
            {
                s += "[ ";
                for (int j = 0; j < 4; j++)
                {
                    s += v[i * 4 + j].ToString("N4") + " ";
                }
                s += "]\n";
            }
            return s;
        }

        public void Transpose()
        {
            double[] v = new double[16];
            int k = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    v[k++] = this.v[j * 4 + i];
                }
            }
            this.v = v;
        }
        public bool Inverse()
        {
            double[] inv = new double[16];
            double det;
            int i;

            inv[0] = v[5]  * v[10] * v[15] -
                     v[5]  * v[11] * v[14] -
                     v[9]  * v[6]  * v[15] +
                     v[6]  * v[13]  * v[11] +
                     v[7] * v[9]  * v[14] -
                     v[13] * v[7]  * v[10];

            inv[1] = -v[1]  * v[10] * v[15] +
                      v[1]  * v[11] * v[14] +
                      v[2]  * v[9]  * v[15] -
                      v[2]  * v[13]  * v[11] -
                      v[3] * v[9]  * v[14] +
                      v[3] * v[13]  * v[10];

            inv[2] = v[1]  * v[6] * v[15] -
                     v[1]  * v[14] * v[7] -
                     v[2]  * v[5] * v[15] +
                     v[2]  * v[13] * v[7] +
                     v[3] * v[5] * v[14] -
                     v[3] * v[13] * v[6];

            inv[3] = -v[1]  * v[5] * v[11] +
                       v[1]  * v[10] * v[7] +
                       v[2]  * v[5] * v[11] -
                       v[2]  * v[9] * v[7] -
                       v[3] * v[5] * v[10] +
                       v[3] * v[9] * v[6];

            det = v[0] * inv[0] + v[4] * inv[1] + v[8] * inv[2] + v[12] * inv[3];

            if (det == 0)
            {
                v = new double[] { 1, 0, 0, 0,
                                    0, 1, 0, 0,
                                    0, 0, 1, 0,
                                    0, 0, 0, 1};
                return false;
            }


            inv[4] = -v[4]  * v[10] * v[15] +
                      v[4]  * v[14] * v[11] +
                      v[6]  * v[8] * v[15] -
                      v[6]  * v[12] * v[11] -
                      v[7] * v[8] * v[14] +
                      v[7] * v[12] * v[10];

            inv[5] = v[0]  * v[10] * v[15] -
                     v[0]  * v[11] * v[14] -
                     v[8]  * v[2] * v[15] +
                     v[8]  * v[12] * v[11] +
                     v[3] * v[8] * v[14] -
                     v[12] * v[3] * v[10];

            inv[6] = -v[0]  * v[6] * v[15] +
                      v[0]  * v[14] * v[7] +
                      v[2]  * v[4] * v[15] -
                      v[2]  * v[12] * v[7] -
                      v[3] * v[4] * v[14] +
                      v[3] * v[12] * v[6];

            inv[7] = v[0]  * v[6] * v[11] -
                      v[0]  * v[10] * v[7] -
                      v[2]  * v[4] * v[11] +
                      v[2]  * v[8] * v[7] +
                      v[3] * v[4] * v[10] -
                      v[3] * v[8] * v[6];

            inv[8] = v[4]  * v[9] * v[15] -
                     v[4]  * v[13] * v[11] -
                     v[5]  * v[8] * v[15] +
                     v[5]  * v[12] * v[11] +
                     v[7] * v[8] * v[13] -
                     v[7] * v[12] * v[9];

            inv[9] = -v[0]  * v[9] * v[15] +
                      v[0]  * v[13] * v[11] +
                      v[1]  * v[8] * v[15] -
                      v[1]  * v[12] * v[11] -
                      v[3] * v[8] * v[13] +
                      v[3] * v[12] * v[9];
            //check 10
            inv[10] = v[0]  * v[5] * v[15] -
                      v[0]  * v[7] * v[13] -
                      v[4]  * v[1] * v[15] +
                      v[4]  * v[12] * v[7] +
                      v[3] * v[4] * v[13] -
                      v[3] * v[12] * v[5];

            inv[11] = -v[0]  * v[5] * v[11] +
                       v[0]  * v[9] * v[7] +
                       v[4]  * v[1] * v[11] -
                       v[1]  * v[8] * v[7] -
                       v[3] * v[4] * v[9] +
                       v[3] * v[8] * v[5];

            inv[12] = -v[4] * v[9] * v[14] +
                      v[4] * v[13] * v[10] +
                      v[5] * v[8] * v[14] -
                      v[5] * v[12] * v[10] -
                      v[6] * v[8] * v[13] +
                      v[6] * v[12] * v[9];

            inv[13] = v[0] * v[9] * v[14] -
                     v[0] * v[13] * v[10] -
                     v[1] * v[8] * v[14] +
                     v[1] * v[12] * v[10] +
                     v[2] * v[8] * v[13] -
                     v[2] * v[12] * v[9];

            inv[14] = -v[0] * v[5] * v[14] +
                       v[0] * v[13] * v[6] +
                       v[4] * v[1] * v[14] -
                       v[1] * v[12] * v[6] -
                       v[2] * v[4] * v[13] +
                       v[2] * v[12] * v[5];

            inv[15] = v[0] * v[5] * v[10] -
                      v[0] * v[6] * v[9] -
                      v[4] * v[1] * v[10] +
                      v[1] * v[8] * v[6] +
                      v[2] * v[4] * v[9] -
                      v[2] * v[8] * v[5];

            det = 1.0 / det;

            for (i = 0; i < 16; i++)
                inv[i] = inv[i] * det;

            v = inv;

            return true;
        }
        //TODO check if that is optimized
        public double Det()
        {
            double i0 = v[5] * v[10] * v[15] -
                     v[5] * v[11] * v[14] -
                     v[9] * v[6] * v[15] +
                     v[6] * v[13] * v[11] +
                     v[7] * v[9] * v[14] -
                     v[13] * v[7] * v[10];

            double i1 = -v[1] * v[10] * v[15] +
                      v[1] * v[11] * v[14] +
                      v[2] * v[9] * v[15] -
                      v[2] * v[13] * v[11] -
                      v[3] * v[9] * v[14] +
                      v[3] * v[13] * v[10];

            double i2 = v[1] * v[6] * v[15] -
                     v[1] * v[14] * v[7] -
                     v[2] * v[5] * v[15] +
                     v[2] * v[13] * v[7] +
                     v[3] * v[5] * v[14] -
                     v[3] * v[13] * v[6];

            double i3 = -v[1] * v[5] * v[11] +
                       v[1] * v[10] * v[7] +
                       v[2] * v[5] * v[11] -
                       v[2] * v[9] * v[7] -
                       v[3] * v[5] * v[10] +
                       v[3] * v[9] * v[6];

            return v[0] * i0 + v[4] * i1 + v[8] * i2 + v[12] * i3;
        }
        public double Trace()
        {
            double t = 0;
            for (int i = 0; i < 4; i++)
                t += v[i * 4 + i];
            return t;
        }

        public void Add(Matrix4 r)
        {
            for (int i = 0; i < 16; i++)
                v[i] += r[i];
        }
        public void Sub(Matrix4 r)
        {
            for (int i = 0; i < 16; i++)
                v[i] += r[i];
        }

        public void Apply(Func<int, double> f)
        {
            for (int i = 0; i < 16; i++)
                v[i] = f(i);
        }
        public void Apply(Func<int, int, double> f)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    v[j * 4 + i] = f(i, j);
            }
        }
        public void Apply(Func<double, double> f)
        {
            for (int i = 0; i < 16; i++)
                v[i] = f(v[i]);
        }

        public object Clone()
        {
            double[] nVals = new double[16];
            for (int i = 0; i < 16; i++)
                nVals[i] = v[i];
            return new Matrix4(nVals);
        }

        public static Vec4 operator* (Matrix4 l, I4Dimensional r)
        {
            return new Vec4(l[0] * r.X + l[1] * r.Y + l[2] * r.Z + l[3] * r.W,
                            l[4] * r.X + l[5] * r.Y + l[6] * r.Z + l[7] * r.W,
                            l[8] * r.X + l[9] * r.Y + l[10] * r.Z + l[11] * r.W,
                            l[12] * r.X + l[13] * r.Y + l[14] * r.Z + l[15] * r.W);
        }
        public static Matrix4 operator* (Matrix4 l, Matrix4 r)
        {
            double[] v = new double[16];
            int k = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    v[k++] = l[i, 0] * r[0, j] +
                            l[i, 1] * r[1, j] +
                            l[i, 2] * r[2, j] +
                            l[i, 3] * r[3, j];
                }
            }
            return new Matrix4(v);
        }
        public static Matrix4 operator* (Matrix4 l, double scalar)
        {
            double[] v = new double[16];
            for (int i = 0; i < 16; i++)
                v[i] = l[i] * scalar;
            return new Matrix4(v);
        }
        public static Matrix4 operator+ (Matrix4 l, Matrix4 r)
        {
            double[] v = new double[16];
            for (int i = 0; i < 16; i++)
                v[i] = l[i] + r[i];
            return new Matrix4(v);
        }
        public static Matrix4 operator- (Matrix4 l, Matrix4 r)
        {
            double[] nV = new double[16];
            for (int i = 0; i < 16; i++)
                nV[i] = l[i] - r[i];
            return new Matrix4(nV);
        }

        public static Matrix4 GetIdentity()
        {
            double[] v = new double[16] { 1, 0, 0, 0,
                                        0, 1, 0, 0,
                                        0, 0, 1, 0,
                                        0, 0, 0, 1, };
            return new Matrix4(v);
        }
    }
}
