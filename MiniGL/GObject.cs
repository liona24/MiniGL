﻿using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MiniGL
{
    /// <summary>
    /// A graphical object used for painters and tmakers, either line or triangle
    /// </summary>
    public class GObject2D : IXmlSerializable, IHasBoundaries
    {
        private Vec3[] points;

        protected Rect boundaries;
        protected TMaker2D tmaker;

        public int Length { get { return points.Length; } }
        public Vec3 this[int i] { get { return points[i]; } set { points[i] = value; } }
        public TMaker2D TMaker { get { return tmaker; } set { tmaker = value; } }
        public Rect Boundaries { get { return boundaries; } }

        protected GObject2D(Vec3 p1, Vec3 p2, Vec3 p3, TMaker2D tmaker)
        {
            this.tmaker = tmaker;
            points = new Vec3[] { p1, p2, p3 };
            UpdateBoundaries();
        }
        protected GObject2D(Vec3 p1, Vec3 p2, TMaker2D tmaker)
        {
            this.tmaker = tmaker;
            points = new Vec3[] { p1, p2 };
            UpdateBoundaries();
        }
        
        public GObject2D()
        { }

        public void UpdateBoundaries()
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X < minX)
                    minX = points[i].X;
                else if (points[i].X > maxX)
                    maxX = points[i].X;
                if (points[i].Y < minY)
                    minY = points[i].Y;
                else if (points[i].Y > maxY)
                    maxY = points[i].Y;
            }
            boundaries = new Rect(minX, minY, maxX, maxY);
        }

        public I2Dimensional[] TransformToWindow()
        {
            return tmaker.TransformToWin(this, true);
        }

        public void ReadXml(XmlReader read)
        {
            read.ReadStartElement();
            read.ReadStartElement("Vertices");
            var s = new XmlSerializer(typeof(Vec3));
            var points = new List<Vec3>();
            while (read.Name != "Vertices")
            {
                points.Add((Vec3)s.Deserialize(read.ReadSubtree()));
                read.ReadEndElement();
            }
            this.points = points.ToArray();
            read.ReadEndElement();

            readAdditionalXml(read);

            read.ReadEndElement();
        }
        public void WriteXml(XmlWriter write)
        {
            write.WriteStartElement("Vertices");

            var s = new XmlSerializer(typeof(Vec3));
            for (int i = 0; i < points.Length; i++)
                s.Serialize(write, points[i]);

            write.WriteEndElement(); //Vertices

            writeAdditionalXml(write);
        }
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Method for inheritated classes to write addtional information to the xml file
        /// </summary>
        /// <param name="write"></param>
        protected virtual void writeAdditionalXml(XmlWriter write)
        {

        }

        /// <summary>
        /// Method for inheritated classes to read additional information from the xml file
        /// </summary>
        protected virtual void readAdditionalXml(XmlReader read)
        {

        }

        public class GObject2DFactory
        {
            public virtual GObject2D Create(Vec3 p1, Vec3 p2, Vec3 p3, TMaker2D tmaker)
            {
                return new GObject2D(p1, p2, p3, tmaker);
            }
            public virtual GObject2D Create(Vec3 p1, Vec3 p2, TMaker2D tmaker)
            {
                return new GObject2D(p1, p2, tmaker);
            }
            public virtual GObject2D Create(I3Dimensional p1, I3Dimensional p2, I3Dimensional p3, TMaker2D tmaker)
            {
                return new GObject2D(new Vec3(p1.X, p1.Y, p1.Z), new Vec3(p2.X, p2.Y, p2.Z),
                                    new Vec3(p3.X, p3.Y, p3.Z), tmaker);
            }
            public virtual GObject2D Create(I3Dimensional p1, I3Dimensional p2, TMaker2D tmaker)
            {
                return new GObject2D(new Vec3(p1.X, p1.Y, p1.Z), new Vec3(p2.X, p2.Y, p2.Z), tmaker);
            }
        }
    }

    /// <summary>
    /// A graphical object used for painters and tmakers, either line or triangle
    /// </summary>
    public class GObject : IXmlSerializable, IHasBoundaries3
    {
        private Vec4[] points;
        private Cuboid boundaries;

        protected TMaker tmaker;

        public Cuboid Boundaries {  get { return boundaries; } }

        public int Length { get { return points.Length; } }
        public Vec4 this[int i] { get { return points[i]; } set { points[i] = value; } }
        public TMaker TMaker { get { return tmaker; } set { tmaker = value; } }

        protected GObject(Vec4 p1, Vec4 p2, Vec4 p3, TMaker tmaker)
        {
            this.tmaker = tmaker;
            points = new Vec4[] { p1, p2, p3 };
            UpdateBoundaries();
        }
        protected GObject(Vec4 p1, Vec4 p2, TMaker tmaker)
        {
            this.tmaker = tmaker;
            points = new Vec4[] { p1, p2 };
            UpdateBoundaries();
        }

        //only used for xml serialization
        public GObject()
        { }

        public void UpdateBoundaries()
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double minZ = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double maxZ = double.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].Normalize();
                if (points[i].X < minX)
                    minX = points[i].X;
                else if (points[i].X / points[i].W > maxX)
                    maxX = points[i].X;
                if (points[i].Y < minY)
                    minY = points[i].Y;
                else if (points[i].Y > maxY)
                    maxY = points[i].Y;
                if (points[i].Z < minZ)
                    minZ = points[i].Z;
                else if (points[i].Z > maxZ)
                    maxZ = points[i].Z;
            }
            boundaries = new Cuboid(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public I3Dimensional[] TransformToWindow()
        {
            return tmaker.TransformToWin(this, true);
        }

        public void ReadXml(XmlReader read)
        {
            read.ReadStartElement();
            read.ReadStartElement("Vertices");
            var s = new XmlSerializer(typeof(Vec4));
            var points = new List<Vec4>();
            while (read.Name != "Vertices")
            {
                points.Add((Vec4)s.Deserialize(read.ReadSubtree()));
                read.ReadEndElement();
            }
            this.points = points.ToArray();
            read.ReadEndElement();

            readAdditionalXml(read);

            read.ReadEndElement();
        }
        public void WriteXml(XmlWriter write)
        {
            write.WriteStartElement("Vertices");

            var s = new XmlSerializer(typeof(Vec4));
            for (int i = 0; i < points.Length; i++)
                s.Serialize(write, points[i]);

            write.WriteEndElement(); //Vertices

            writeAdditionalXml(write);
        }
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Method for inheritated classes to write addtional information to the xml file
        /// </summary>
        /// <param name="write"></param>
        protected virtual void writeAdditionalXml(XmlWriter write)
        {

        }

        /// <summary>
        /// Method for inheritated classes to read additional information from the xml file
        /// </summary>
        protected virtual void readAdditionalXml(XmlReader read)
        {

        }

        public class GObjectFactory
        {
            public virtual GObject Create(Vec4 p1, Vec4 p2, TMaker tmaker)
            {
                return new GObject(p1, p2, tmaker);
            }
            public virtual GObject Create(Vec4 p1, Vec4 p2, Vec4 p3, TMaker tmaker)
            {
                return new GObject(p1, p2, p3, tmaker);
            }
            public virtual GObject Create(I4Dimensional p1, I4Dimensional p2, TMaker tmaker)
            {
                return new GObject(new Vec4(p1.X, p1.Y, p1.Z, p1.W),
                        new Vec4(p2.X, p2.Y, p2.Z, p2.W), tmaker);
            }
            public virtual GObject Create(I4Dimensional p1, I4Dimensional p2, I4Dimensional p3, TMaker tmaker)
            {
                return new GObject(new Vec4(p1.X, p1.Y, p1.Z, p1.W),
                        new Vec4(p2.X, p2.Y, p2.Z, p2.W),
                        new Vec4(p3.X, p3.Y, p3.Z, p3.W), tmaker);
            }
        }
    }

}
