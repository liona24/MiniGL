using System;
using GraphicsUtility;

namespace MiniGL
{
    public class TMaker : ICloneable
    {
        protected Matrix4 mat; //transformation matrix

        public TMaker()
        {
            LoadIdentity();
        }
        public TMaker(Matrix4 init)
        {
            mat = init;
        }

        public Matrix4 GetMatrix()
        {
            return mat;
        }

        public void LoadIdentity()
        {
            mat = Matrix4.GetIdentity();
        }
        public virtual object Clone()
        {
            var nMat = (Matrix4)mat.Clone();
            return new TMaker(nMat);
        }
        public void Load(Matrix4 mat)
        {
            this.mat = mat;
        }

        public void Rotate(double angle, double x, double y, double z)
        {
            double c = Math.Cos(angle);
            double s = Math.Sin(angle);

            var rot = new Matrix4(new double[] { x * x * (1 - c) + c, x * y * (1 - c) - z * s, x * z * (1 - c) + y * s, 0,
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

            var rotX = new Matrix4(new double[] { (1 - c) + c, 0, 0, 0,
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

            var rotY = new Matrix4(new double[] { c, 0, s, 0,
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

            var rotZ = new Matrix4(new double[] { c, -s, 0, 0,
                                                        s, c, 0, 0,
                                                        0, 0, (1 - c) + c, 0,
                                                        0, 0, 0, 1});
            mat = rotZ * mat;
        }

        public void Translate(double x, double y, double z)
        {
            var trans = new Matrix4( new double[] { 1, 0, 0, x,
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
            var sca = new Matrix4( new double[] { x, 0, 0, 0,
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
        public void Transform(Vec4[] src, Vec4[] dst)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i] = mat * src[i];
        }
    }
}
