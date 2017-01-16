using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MiniGL
{
    //TODO add xml serialization
    
    internal struct Line 
    {
        public readonly int Index1, Index2;
        public readonly int ObjHash;

        public Line(int i1, int i2, int objHash)
        {
            Index1 = i1;
            Index2 = i2;
            ObjHash = objHash;
        }
    }
    internal struct Triangle 
    {
        public readonly int Index1, Index2, Index3;
        public readonly int ObjHash;

        public Triangle(int i1, int i2, int i3, int objHash)
        {
            Index1 = i1;
            Index2 = i2;
            Index3 = i3;
            ObjHash = objHash;
        }
    }

    ///<summary>
    /// Handles vertex and GObject storage
    ///</summary>
    public class GManager 
    {
        private const int EMPTY_HASH = -1;
           /* 
            //TODO export this function somewhere where it is usefull
            public VInfo[] Split()
            {
                VInfo[] res = null;

                if (Vertices.Length == 2)
                {
                    var mid = Vertices[0] + Vertices[1];
                    res = new VInfo[2];
                    res[0] = new VInfo(Vertices[0], mid, ObjHash);
                    res[1] = new VInfo(mid, Vertices[1], ObjHash);
                }
                else
                {
                    res = new VInfo[4];
                    var mid01 = Vertices[0] + Vertices[1];
                    var mid02 = Vertices[0] + Vertices[2];
                    var mid12 = Vertices[1] + Vertices[2];
                    res[0] = new VInfo(Vertices[0], mid01, mid02, ObjHash);
                    res[1] = new VInfo(Vertices[1], mid01, mid12, ObjHash);
                    res[2] = new VInfo(Vertices[2], mid02, mid12, ObjHash);
                    res[3] = new VInfo(mid01, mid02, mid12, ObjHash);
                }
                return res;
            }*/

        private readonly Dictionary<int, Vec4> vertexStorage;
        private readonly List<Triangle> triangles;
        private readonly List<Line> lines;
        private int activeHash;

        private readonly Dictionary<int, GObject> objectStorage;

        public GManager(int initialStorageSize)
        {
            vertexStorage = new Dictionary<int, Vec4>();
            activeHash = EMPTY_HASH;
            objectStorage = new Dictionary<int, GObject>();
            objectStorage.Add(activeHash, new GObject(new TMaker()));
            triangles = new List<Triangle>();
            lines = new List<Line>();
        }
        ///<summary>
        ///Returns the GObject corresponding to the given hash, null if it does not exist
        ///</summary>
        public GObject GetObjectByHash(int hash)
        {
            GObject ret;
            if (objectStorage.TryGetValue(hash, out ret))
                return ret;

            return null;
        }
        ///<summary>
        ///Adds a new GObject to the internal storage. All vertices will be linked to that GObject until another one becomes active
        ///</summary>
        public void AddGObject(GObject obj)
        {
            activeHash = obj.GetHashCode();
            objectStorage.Add(activeHash, obj);
        }
        ///<summary>
        ///Activates the given GObject. All triangles and lines will be linked to that GObject until another on becomes active
        ///</summary>
        public void ActiveGObject(GObject obj)
        {
            int hash = obj.GetHashCode();
            if (!objectStorage.ContainsKey(hash))
                throw new ArgumentException("obj", "GObject does not exist in the storage!");
            activeHash = hash;
        }
        ///<summary>
        ///Activates the given GObject. All triangles and lines will be linked to that GObject until another on becomes active
        ///</summary>
        public void ActiveGObject(int hash)
        {
            if (!objectStorage.ContainsKey(hash))
                throw new ArgumentException("hash", "Hashcode does not correspond with a GObject in the storage");

            activeHash = hash;
        }
        ///<summary>
        ///Adds a triangle to the vertex buffer and links it to the currently active GObject
        ///</summary>
        public void AddVertices(Vec4 v1, Vec4 v2, Vec4 v3)
        {
            int hash1 = v1.GetHashCode();
            int hash2 = v2.GetHashCode();
            int hash3 = v3.GetHashCode();
            if (!vertexStorage.ContainsKey(hash1))
                vertexStorage.Add(hash1, v1);
            if (!vertexStorage.ContainsKey(hash2))
                vertexStorage.Add(hash2, v2);
            if (!vertexStorage.ContainsKey(hash3))
                vertexStorage.Add(hash3, v3);
            triangles.Add(new Triangle(hash1, hash2, hash3, activeHash));
        }
        ///<summary>
        ///Adds a line to the vertex buffer and links it to the currently active GObject
        ///</summary>
        public void AddVertices(Vec4 v1, Vec4 v2)
        {
            int hash1 = v1.GetHashCode();
            int hash2 = v2.GetHashCode();
            if (!vertexStorage.ContainsKey(hash1))
                vertexStorage.Add(hash1, v1);
            if (!vertexStorage.ContainsKey(hash2))
                vertexStorage.Add(hash2, v2);
            lines.Add(new Line(hash1, hash2, activeHash));
        }
        ///<summary>
        ///Removes the GObject corresponding to the given hash from the internal storage and all linked lines and triangles
        ///</summary>
        public void RemoveObjectByHash(int hash)
        {
            objectStorage.Remove(hash);
            triangles.RemoveAll(s => s.ObjHash == hash);
            lines.RemoveAll(s => s.ObjHash == hash);
        }
        ///<summary>
        ///Draws every vertex in the storage using the given painter and ViewTransformator
        ///</summary>
        public void DrawStorage(Painter painter, ViewTransformator viewT)
        {
            int cacheHash = EMPTY_HASH;
            GObject cacheObject = objectStorage[EMPTY_HASH];
            foreach (var tri in triangles) 
            {
                if (tri.ObjHash != cacheHash)
                {
                    cacheHash = tri.ObjHash;
                    cacheObject = objectStorage[cacheHash];
                }
                var v1 = vertexStorage[tri.Index1];
                var v2 = vertexStorage[tri.Index2];
                var v3 = vertexStorage[tri.Index3];
                var tf = viewT.TransformToWindow(cacheObject.TMaker.Transform(v1),
                                                cacheObject.TMaker.Transform(v2),
                                                cacheObject.TMaker.Transform(v3));
                painter.Paint(cacheHash, tf);
            }
            cacheHash = EMPTY_HASH;
            cacheObject = objectStorage[EMPTY_HASH];
            foreach (var line in lines) 
            {
                if (line.ObjHash != cacheHash)
                {
                    cacheHash = tri.ObjHash;
                    cacheObject = objectStorage[cacheHash];
                }
                var v1 = vertexStorage[line.Index1];
                var v2 = vertexStorage[line.Index2];
                var tf = viewT.TransformToWindow(cacheObject.TMaker.Transform(v1),
                                                cacheObject.TMaker.Transform(v2));
                painter.Paint(cacheHash, tf);
            }
        }
        ///<summary>
        ///Clears the vertex storage and removes all GObjects
        ///</summary>
        public void Clear()
        {
            activeHash = EMPTY_HASH;
            objectStorage.Clear();
            objectStorage.Add(EMPTY_HASH, new GObject(new TMaker()));
            vertexStorage.Clear();
            triangles.Clear();
            lines.Clear();
        }
    } 

    public class GObject
    {
        protected TMaker tmaker;
        public TMaker TMaker { get { return tmaker; } }

        public GObject(TMaker tmaker)
        {
            this.tmaker = tmaker;
        }
    }
}
