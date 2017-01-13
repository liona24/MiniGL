using System.Collections.Generic;

namespace MiniGL
{
    public enum ProjectionType
    {
        Orthogonal,
        Perspective
    }
    public class ViewTransformer
    {
        private Matrix4 mat;

        public Vec2 DepthRange { get; set; }
        public Rect ViewPort { get; set; }
        public ProjectionType ProjectionType { get; set; }

        public ViewTransformer(Matrix4 init, Vec2 depthRange, Rect viewPort)
        {
            mat = init;
            DepthRange = depthRange;
            ViewPort = viewPort;
        }
        public ViewTransformer()
        {
            mat = Matrix4.GetIdentity();
        }

        public void Calculate()
        {
            if (ProjectionType == ProjectionType.Orthogonal)
            {
                double l = ViewPort.L;
                double r = ViewPort.R;
                double b = ViewPort.B;
                double t = ViewPort.T;
                double n = DepthRange.X;
                double f = DepthRange.Y;

                mat = new Matrix4(new double[] { 2 / (r - l), 0, 0, (-l - r) / (r - l),
                                                   0, 2 / (b - t), 0, (-t - b) / (b - t),
                                                   0, 0, -2 / (f - n), (-n - f) / (f - n),
                                                   0, 0, 0, 1});
            }
            else if (ProjectionType == ProjectionType.Perspective)
            {
                double f = DepthRange.Y;
                double n = DepthRange.X;
                double r = ViewPort.R;
                double l = ViewPort.L;
                double t = ViewPort.T;
                double b = ViewPort.B;
                mat = new Matrix4(new double[] {
                       2 * n / (r - l),                0, (r + l) / (r - l), 0,
                                     0,  2 * n / (b - t), (t + b) / (b - t), 0,
                                     0,                0, (f + n) / (n - f), 2 * f * n / (n - f),
                                     0,                0,                -1, 0 });

            }
        }

        public Vec3[] TransformToWindow(Vec4[] vertices)
        {
            var unclipped = new Vec3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                unclipped[i] = Vec3.FromHomo(mat * vertices[i]);
            return clip(unclipped);
        }
        
        private Vec3[] clip(Vec3[] unclipped)
        {
            var odd = new List<Vec3>();
            var even = new List<Vec3>(unclipped);

            if (unclipped.Length == 2)
                return clip2(unclipped[0], unclipped[1]);

            Vec3 last = even[unclipped.Length - 1];
            for (int i = 0; i < even.Count; i++) // Left
            {
                Vec3 e = even[i];
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
                return new Vec3[0];
            last = odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Top
            {
                Vec3 e = odd[i];
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
                return new Vec3[0];
            last = even[even.Count - 1];
            odd.Clear();

            for (int i = 0; i < even.Count; i++) // Right
            {
                Vec3 e = even[i];
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
                return new Vec3[0];
            last = odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Bot
            {
                Vec3 e = odd[i];
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
                return new Vec3[0];
            last = even[even.Count - 1];
            odd.Clear();
            for (int i = 0; i < even.Count; i++) // Front
            {
                Vec3 e = even[i];
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
                return new Vec3[0];
            last = odd[odd.Count - 1];
            even.Clear();
            for (int i = 0; i < odd.Count; i++) // Back
            {
                Vec3 e = odd[i];
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
        
        private Vec3[] clip2(Vec3 p1, Vec3 p2)
        {
            if (p1.X >= -1)
            {
                if (p2.X < -1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(-1, 1, 0), new Vec3(-1, 0, 1), p2, p1);
            }
            else if (p2.X >= -1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(-1, 1, 0), new Vec3(-1, 0, 1), p2, p1);
            else
                return new Vec3[0];

            if (p1.Y >= -1)
            {
                if (p2.Y < -1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(1, -1, 0), new Vec3(0, -1, 1), p2, p1);
            }
            else if (p2.Y >= -1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(-1, -1, 0), new Vec3(1, -1, 0), new Vec3(0, -1, 1), p2, p1);
            else
                return new Vec3[0];

            if (p1.X <= 1)
            {
                if (p2.X > 1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(1, -1, 0), new Vec3(1, 1, 0), new Vec3(1, 0, 1), p2, p1);
            }
            else if (p2.X <= 1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(1, -1, 0), new Vec3(1, 1, 0), new Vec3(1, 0, 1), p2, p1);
            else
                return new Vec3[0];

            if (p1.Y <= 1)
            {
                if (p2.Y > 1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 1), p2, p1);
            }
            else if (p2.Y <= 1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(-1, 1, 0), new Vec3(1, 1, 0), new Vec3(0, 1, 1), p2, p1);
            else
                return new Vec3[0];

            if (p1.Z >= -1)
            {
                if (p2.Z < -1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(0, 0, -1), new Vec3(1, 0, -1), new Vec3(0, 1, -1), p2, p1);
            }
            else if (p2.Z >= -1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(0, 0, -1), new Vec3(1, 0, -1), new Vec3(0, 1, -1), p2, p1);
            else
                return new Vec3[0];

            if (p1.Z <= 1)
            {
                if (p2.Z > 1)
                    p2 = Utility.IntersectionPlaneLine(new Vec3(0, 0, 1), new Vec3(1, 0, 1), new Vec3(0, 1, 1), p2, p1);
            }
            else if (p2.Z <= 1)
                p1 = Utility.IntersectionPlaneLine(new Vec3(0, 0, 1), new Vec3(1, 0, 1), new Vec3(0, 1, 1), p2, p1);
            else
                return new Vec3[0];

            return new [] { toWindow(p1), toWindow(p2) };
        }

        private Vec3 toWindow(Vec3 ndc)
        {
            return new Vec3((ndc.X + 1) * 0.5 * ViewPort.Width + ViewPort.L,
                (ndc.Y + 1) * 0.5 * ViewPort.Height + ViewPort.T,
                (ndc.Z + 1) * 0.5 * (DepthRange.Y - DepthRange.X) + DepthRange.X);
        }
    }
}
       
