using System;
using System.Collections.Generic;

namespace MiniGL
{
    public class TMaker : ICloneable
    {
        private Matrix4 mat; //transformation matrix
        private Matrix4 persp;

        private Rect vp; // viewport
        private Vec2 dr; // depthrange

        public TMaker()
        {
            LoadIdentity();
        }
        public TMaker(Matrix4 init)
        {
            Load(init);
        }

        public Matrix4 GetMatrix()
        {
            return mat;
        }

        public void LoadIdentity()
        {
            mat = Matrix4.GetIdentity();
            persp = Matrix4.GetIdentity();
        }
        public object Clone()
        {
            var nMat = (Matrix4)mat.Clone();
            return new TMaker(nMat);
        }
        public void Load(Matrix4 mat)
        {
            this.mat = mat;
            persp = Matrix4.GetIdentity();
        }

        public void Rotate(double angle, double x, double y, double z)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            Matrix4 rot = new Matrix4(new double[] { x * x * (1 - c) + c, x * y * (1 - c) - z * s, x * z * (1 - c) + y * s, 0,
                                                        x * y * (1 - c) + z * s, y * y * (1 - c) + c, y * z * (1 - c) - x * s, 0,
                                                        x * z * (1 - c) - y * s, y * z * (1 - c) + x * s, z * z * (1 - c) + c, 0,
                                                        0, 0, 0, 1});
            mat = rot * mat;
        }

        public void RotateX(double angle)
        {
            //x = 1, y = 0, z = 0
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            Matrix4 rotX = new Matrix4(new double[] { (1 - c) + c, 0, 0, 0,
                                                        0, c, -s, 0,
                                                        0, s, c, 0,
                                                        0, 0, 0, 1});
            mat = rotX * mat;
        }

        public void RotateY(double angle)
        {
            //x = 0, y = 1, z = 0
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            Matrix4 rotY = new Matrix4(new double[] { c, 0, s, 0,
                                                        0, (1 - c) + c, 0, 0,
                                                        -s, 0, c, 0,
                                                        0, 0, 0, 1});
            mat = rotY * mat;
        }

        public void RotateZ(double angle)
        {
            //x = 0, y = 0, z = 1
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            Matrix4 rotZ = new Matrix4(new double[] { c, -s, 0, 0,
                                                        s, c, 0, 0,
                                                        0, 0, (1 - c) + c, 0,
                                                        0, 0, 0, 1});
            mat = rotZ * mat;
        }

        public void Translate(double x, double y, double z)
        {
            Matrix4 trans = new Matrix4( new double[] { 1, 0, 0, x,
                                                        0, 1, 0, y,
                                                        0, 0, 1, z,
                                                        0, 0, 0, 1 });
            mat = trans * mat;
        }
        public void Scale(double u)
        {
            Scale(u, u, u);
        }
        public void Scale(double x, double y, double z)
        {
            Matrix4 sca = new Matrix4( new double[] { x, 0, 0, 0,
                                                        0, y, 0, 0,
                                                        0, 0, z, 0,
                                                        0, 0, 0, 1 });
            mat = sca * mat;
        }

        public void ApplyCustomTransform(Matrix4 t)
        {
            mat = t * mat;
        }
        public void ApplyCustomTranform(TMaker t)
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

        public void SetDepthRange(double n, double f)
        {
            dr = new Vec2(n, f);
        }

        public void PerspProjection()
        {
            double f = dr.Y;
            double n = dr.X;
            double r = vp.R;
            double l = vp.L;
            double t = vp.T;
            double b = vp.B;
            var p = new Matrix4(new double[] {
                   2 * n / (r - l),                0, (r + l) / (r - l), 0,
                                 0,  2 * n / (b - t), (t + b) / (b - t), 0,
                                 0,                0, (f + n) / (n - f), 2 * f * n / (n - f),
                                 0,                0,                -1, 0 });
            persp = p;
        }

        public void OrthoProjection()
        {
            double l = vp.L;
            double r = vp.R;
            double b = vp.B;
            double t = vp.T;
            double n = dr.X;
            double f = dr.Y;

            var p = new Matrix4(new double[] { 2 / (r - l), 0, 0, (-l - r) / (r - l),
                                               0, 2 / (b - t), 0, (-t - b) / (b - t),
                                               0, 0, -2 / (f - n), (-n - f) / (f - n),
                                               0, 0, 0, 1});
            persp = p;
        }
        public TMaker GetInverse()
        {
            var inv = (Matrix4)mat.Clone();
            inv.Inverse();
            return new TMaker(inv);
        }

        public Vec4 Transform(Vec4 v)
        {
            return mat * v;
        }

        public I3Dimensional[] TransformToWin(GObject obj, Rect vp)
        {
            SetViewport(vp);
            return TransformToWin(obj, true);
        }
        public I3Dimensional[] TransformToWin(GObject obj, bool doClip)
        {
            int num = obj.Length;
            var tmp = new I3Dimensional[num];

            if (!doClip)
            {
                for (int i = 0; i < num; i++)
                    tmp[i] = toWindow(Vec3.FromHomo(persp * mat * obj[i]));

                return tmp;
            }

            for (int i = 0; i < num; i++)
                tmp[i] = Vec3.FromHomo(persp * mat * obj[i]);

            return clip(tmp);
        }
        private I3Dimensional[] clip(I3Dimensional[] unclipped)
        {
            

            var odd = new List<I3Dimensional>();
            var even = new List<I3Dimensional>(unclipped);

            if (unclipped.Length == 2)
                return clip2((Vec3)unclipped[0], (Vec3)unclipped[1]);


            Vec3 last = (Vec3)even[unclipped.Length - 1];
            for (int i = 0; i < even.Count; i++) // Left
            {
                Vec3 e = (Vec3)even[i];
                if (e.X >= -1)
                {
                    if (last.X < -1)
                        odd.Add(Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(-1, 1, 0), new Vec3(-1, 0, 1), last, e));
                    odd.Add(e);
                }
                else if (last.X > -1)
                    odd.Add(Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(-1, 1, 0), new Vec3(-1, 0, 1), last, e));
                last = e;
            }
            if (odd.Count == 0)
                return new I3Dimensional[0];
            last = (Vec3)odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Top
            {
                Vec3 e = (Vec3)odd[i];
                if (e.Y >= -1)
                {
                    if (last.Y < -1)
                        even.Add(Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(1, -1, 0), new Vec3(0, -1, 1), last, e));
                    even.Add(e);
                }
                else if (last.Y > -1)
                    even.Add(Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(1, -1, 0), new Vec3(0, -1, 1), last, e));
                last = e;
            }
            if (even.Count == 0)
                return new I3Dimensional[0];
            last = (Vec3)even[even.Count - 1];
            odd.Clear();

            for (int i = 0; i < even.Count; i++) // Right
            {
                Vec3 e = (Vec3)even[i];
                if (e.X <= 1)
                {
                    if (last.X > 1)
                        odd.Add(Utility.IntersectionPlaneLine(new Vec3(1, -1, 0), new Vec3(1, 1, 0), new Vec3(1, 0, 1), last, e));
                    odd.Add(e);
                }
                else if (last.X < 1)
                    odd.Add(Utility.IntersectionPlaneLine(new Vec3(1, -1, 0), new Vec3(1, 1, 0), new Vec3(1, 0, 1), last, e));
                last = e;
            }
            if (odd.Count == 0)
                return new I3Dimensional[0];
            last = (Vec3)odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Bot
            {
                Vec3 e = (Vec3)odd[i];
                if (e.Y <= 1)
                {
                    if (last.Y > 1)
                        even.Add(Utility.IntersectionPlaneLine(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 1), last, e));
                    even.Add(e);
                }
                else if (last.Y < 1)
                    even.Add(Utility.IntersectionPlaneLine(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 1), last, e));
                last = e;
            }

            if (even.Count == 0)
                return new I3Dimensional[0];
            last = (Vec3)even[even.Count - 1];
            odd.Clear();
            for (int i = 0; i < even.Count; i++) // Front
            {
                Vec3 e = (Vec3)even[i];
                if (e.Z >= -1)
                {
                    if (last.Z < -1)
                        odd.Add(Utility.IntersectionPlaneLine(new Vec3(0, 0, -1), new Vec3(1, 0, -1), new Vec3(0, 1, -1), last, e));
                    odd.Add(e);
                }
                else if (last.Z > -1)
                    odd.Add(Utility.IntersectionPlaneLine(new Vec3(0, 0, -1), new Vec3(1, 0, -1), new Vec3(0, 1, -1), last, e));
                last = e;
            }
            if (odd.Count == 0)
                return new I3Dimensional[0];
            last = (Vec3)odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Back
            {
                Vec3 e = (Vec3)odd[i];
                if (e.Z <= 1)
                {
                    if (last.Z > 1)
                        even.Add(toWindow(Utility.IntersectionPlaneLine(new Vec3(0, 0, 1), new Vec3(1, 0, 1), new Vec3(0, 1, 1), last, e)));
                    even.Add(toWindow(e));
                }
                else if (last.Z < 1)
                    even.Add(toWindow(Utility.IntersectionPlaneLine(new Vec3(0, 0, 1), new Vec3(1, 0, 1), new Vec3(0, 1, 1), last, e)));
                last = e;
            }
            return even.ToArray();
        }
        
        private I3Dimensional[] clip2(Vec3 p1, Vec3 p2)
        {
            

            if (p1.X >= -1)
            {
                if (p2.X < -1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(-1, 1, 0), new Vec3(-1, 0, 1), p2, p1);
            }
            else if (p2.X >= -1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(-1, 1, 0), new Vec3(-1, 0, 1), p2, p1);
            else
                return new I3Dimensional[0];

            if (p1.Y >= -1)
            {
                if (p2.Y < -1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(1, -1, 0), new Vec3(0, -1, 1), p2, p1);
            }
            else if (p2.Y >= -1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(1, -1, 0), new Vec3(0, -1, 1), p2, p1);
            else
                return new I3Dimensional[0];

            if (p1.X <= 1)
            {
                if (p2.X > 1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(1, -1, 0), new Vec3(1, 1, 0), new Vec3(1, 0, 1), p2, p1);
            }
            else if (p2.X <= 1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(1, -1, 0), new Vec3(1, 1, 0), new Vec3(1, 0, 1), p2, p1);
            else
                return new I3Dimensional[0];

            if (p1.Y <= 1)
            {
                if (p2.Y > 1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 1), p2, p1);
            }
            else if (p2.Y <= 1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 1), p2, p1);
            else
                return new I3Dimensional[0];

            if (p1.Z >= -1)
            {
                if (p2.Z < -1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(0, 0, -1), new Vec3(1, 0, -1), new Vec3(0, 1, -1), p2, p1);
            }
            else if (p2.Z >= -1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(0, 0, -1), new Vec3(1, 0, -1), new Vec3(0, 1, -1), p2, p1);
            else
                return new I3Dimensional[0];

            if (p1.Z <= 1)
            {
                if (p2.Z > 1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(0, 0, 1), new Vec3(1, 0, 1), new Vec3(0, 1, 1), p2, p1);
            }
            else if (p2.Z <= 1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(0, 0, 1), new Vec3(1, 0, 1), new Vec3(0, 1, 1), p2, p1);
            else
                return new I3Dimensional[0];

            return new I3Dimensional[] { toWindow(p1), toWindow(p2) };
        }

        private I3Dimensional toWindow(Vec3 ndc)
        {
            return new Vec3((ndc.X + 1) * 0.5 * vp.Width + vp.L,
                (ndc.Y + 1) * 0.5 * vp.Height + vp.T,
                (ndc.Z + 1) * 0.5 * (dr.Y - dr.X) + dr.X);
        }
    }

}
