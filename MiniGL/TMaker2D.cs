using System;
using System.Collections.Generic;

namespace MiniGL
{
    public class TMaker2D : ICloneable
    {
        private Matrix3 mat;
        private Rect vp;

        public TMaker2D()
        {
            LoadIdentity();
        }
        public TMaker2D(Matrix3 init)
        {
            Load(init);
        }

        public object Clone()
        {
            var nMat = (Matrix3)mat.Clone();
            return new TMaker2D(nMat);
        }
        public void LoadIdentity()
        {
            mat = Matrix3.GetIdentity();
        }

        public void Load(Matrix3 mat)
        {
            this.mat = mat;
        }
        public Matrix3 GetMatrix()
        {
            return mat;
        }
        public TMaker2D GetInverse()
        {
            Matrix3 i = (Matrix3)mat.Clone();
            i.Inverse();
            return new TMaker2D(i);
        }

        public void Rotate(double angle)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            var rot = new Matrix3(c, -s, 0, 
                                  s, c, 0, 
                                  0, 0, 1);
            mat = rot * mat;
        }

        public void Translate(double x, double y)
        {
            var trans = new Matrix3(1, 0, x,
                                    0, 1, y, 
                                    0, 0, 1);
            mat = trans * mat;
        }

        public void Scale(double u)
        {
            Scale(u, u);
        }
        public void Scale(double x, double y)
        {
            var s = new Matrix3(x, 0, 0, 
                                0, y, 0, 
                                0, 0, 1);
            mat = s * mat;
        }
        public void ApplyCustomTransform(Matrix3 t)
        {
            mat = t * mat;
        }
        public void ApplyCustomTransform(TMaker2D t)
        {
            mat = t.GetMatrix() * mat;
        }

        public void SetViewport(double l, double t, double r, double b)
        {
            vp = new Rect(l, t, r, b);
        }
        public void SetViewport(Rect viewport)
        {
            vp = viewport;
        }

        public Vec3 Transform(Vec3 v)
        {
           return mat * v;
        }

        public I2Dimensional[] TransformToWin(GObject2D obj, Rect vp)
        {
            SetViewport(vp);
            return TransformToWin(obj, true); 
        }
        public I2Dimensional[] TransformToWin(GObject2D obj)
        {
            return TransformToWin(obj, true);
        }

        public I2Dimensional[] TransformToWin(GObject2D obj, bool doClip)
        {
            int num = obj.Length;
            
            var tmp = new I2Dimensional[num];
            //note that obj.Length is max 3

            int i = 0;
            for (; i < num; i++)
                tmp[i] = Vec2.FromHomo(mat * obj[i]);


            if (!doClip)
                return tmp;

            return clip(tmp);
        }

        private I2Dimensional[] clip(I2Dimensional[] unclipped)
        {
            var odd = new List<I2Dimensional>();
            var even = new List<I2Dimensional>(unclipped);

            I2Dimensional last = even[unclipped.Length - 1];
            for (int i = 0; i < even.Count; i++) // Left
            {
                var e = even[i];
                if (e.X >= vp.L)
                {
                    if (last.X < vp.L)
                        odd.Add(Utility.IntersectionLineLine(new Vec2(vp.L, vp.T), new Vec2(vp.L, vp.B), (Vec2)last, (Vec2)e));
                    odd.Add(e);
                }
                else if (last.X > vp.L)
                    odd.Add(Utility.IntersectionLineLine(new Vec2(vp.L, vp.T), new Vec2(vp.L, vp.B), (Vec2)last, (Vec2)e));
                last = e;
            }
            if (odd.Count == 0)
                return new I2Dimensional[0];
            last = odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Top
            {
                var e = odd[i];
                if (e.Y >= vp.T)
                {
                    if (last.Y < vp.T)
                        even.Add(Utility.IntersectionLineLine(new Vec2(vp.L, vp.T), new Vec2(vp.R, vp.T), (Vec2)last, (Vec2)e));
                    even.Add(e);
                }
                else if (last.Y > vp.T)
                    even.Add(Utility.IntersectionLineLine(new Vec2(vp.L, vp.T), new Vec2(vp.R, vp.T), (Vec2)last, (Vec2)e));
                last = e;
            }
            if (even.Count == 0)
                return new I2Dimensional[0];
            last = even[even.Count - 1];
            odd.Clear();

            for (int i = 0; i < even.Count; i++) // Right
            {
                var e = even[i];
                if (e.X <= vp.R)
                {
                    if (last.X > vp.R)
                        odd.Add(Utility.IntersectionLineLine(new Vec2(vp.R, vp.T), new Vec2(vp.R, vp.B), (Vec2)last, (Vec2)e));
                    odd.Add(e);
                }
                else if (last.X < vp.R)
                    odd.Add(Utility.IntersectionLineLine(new Vec2(vp.R, vp.T), new Vec2(vp.R, vp.B), (Vec2)last, (Vec2)e));
                last = e;
            }
            if (odd.Count == 0)
                return new I2Dimensional[0];
            last = odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Bot
            {
                var e = odd[i];
                if (e.Y <= vp.B)
                {
                    if (last.Y > vp.B)
                        even.Add(Utility.IntersectionLineLine(new Vec2(vp.L, vp.B), new Vec2(vp.R, vp.B), (Vec2)last, (Vec2)e));
                    even.Add(e);
                }
                else if (last.Y < vp.B)
                    even.Add(Utility.IntersectionLineLine(new Vec2(vp.L, vp.B), new Vec2(vp.R, vp.B), (Vec2)last, (Vec2)e));
                last = e;
            }
            return even.ToArray();
        }

        public static bool pointInTriangle(Vec2 p, Vec2 p1, Vec2 p2, Vec2 p3)
        {
            
            double a = ((p2.Y - p3.Y) * (p.X - p3.X) + (p3.X - p2.X) * (p.Y - p3.Y)) /
        ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));
            double b = ((p3.Y - p1.Y) * (p.X - p3.X) + (p1.X - p3.X) * (p.Y - p3.Y)) /
                   ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            return a > 0 && b > 0 && 1 - a - b > 0;
        }
    }
}
