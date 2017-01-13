using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MiniGL
{
    abstract class VertexStorage<T>
        where T : GObject
    {
        private const int EMPTY_HASH = -1;

        private struct VInfo
        {
            public readonly Vec4[] Vertices;
            public readonly int ObjHash;

            public VInfo(Vec4 v1, Vec4 v2, Vec4 v3, int objHash)
            {
                Vertices = new Vec4[3] { v1, v2, v3 };
                ObjHash = objHash;
            }
            public VInfo(Vec4 v1, Vec4 v2, int objHash)
            {
                Vertices = new Vec4[2] { v1, v2 };
                ObjHash = objHash;
            }

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
            }
        }

        private VInfo[] vertexStorage;
        private int fillLevelVertexStorage;
        
        private int activeHash;
        private readonly Dictionary<int, T> objectStorage;

        protected VertexStorage(int initialStorageSize)
        {
            vertexStorage = new VInfo[initialStorageSize];
            fillLevelVertexStorage = 0;
            activeHash = 0;
            objectStorage = new Dictionary<int, T>();
            objectStorage.Add(EMPTY_HASH, null);
        }
        
        protected void next()
        {
            activeHash++;
        }
        protected T getObjectByHash(int hash)
        {
            return objectStorage[hash];
        }
        protected void setObject(T obj)
        {
            objectStorage.Add(activeHash, obj);
        }
        protected void addTriangle(Vec4 v1, Vec4 v2, Vec4 v3)
        {
            add(new VInfo(v1, v2, v3, activeHash));
        }
        protected void addLine(Vec4 v1, Vec4 v2)
        {
            add(new VInfo(v1, v2, activeHash));
        }
        protected void remove(int hash)
        {
            objectStorage.Remove(hash);
            for (int i = 0; i < fillLevelVertexStorage; i++)
            {
                if (vertexStorage[i].ObjHash == hash)
                    vertexStorage[i--] = vertexStorage[fillLevelVertexStorage--];
            }
        }
        protected void drawStorage(ZBuffer zBuffer, Rasterizer raster, ViewTransformator viewT)
        {
            int cacheHash = EMPTY_HASH;
            T cacheObject = null;
            for (int i = 0; i < fillLevelVertexStorage; i++)
            {
                var stored = vertexStorage[i];
                if (stored.ObjHash != cacheHash)
                {
                    cacheHash = stored.ObjHash;
                    cacheObject = objectStorage[cacheHash];
                }
                var transformed = new Vec4[stored.Vertices.Length];
                cacheObject.TMaker.Transform(stored.Vertices, transformed);
                raster.Rasterize(cacheHash, zBuffer, viewT.TransformToWindow(transformed));
            }
        }
        public void Clear()
        {
            activeHash = 0;
            fillLevelVertexStorage = 0;
            objectStorage.Clear();
        }

        private void add(VInfo vi)
        {
            if (fillLevelVertexStorage >= vertexStorage.Length)
            {
                int nSize = Math.Min(int.MaxValue, vertexStorage.Length * 2);
                Array.Resize(ref vertexStorage, nSize);
            }
            vertexStorage[fillLevelVertexStorage++] = vi;
        }

    } 

    public class GManager : VertexStorage<GObject>
    {
        public GManager(int initialStorageSize)
            : base (initialStorageSize)
        { }
        public GManager()
            : this (64)
        { }

        public void NewGObject(TMaker tmaker)
        {
            next();
            setObject(new GObject(tmaker));
        }
        public void AddVertices(Vec4 v1, Vec4 v2, Vec4 v3)
        {
            addTriangle(v1, v2, v3);
        }
        public void AddVertices(Vec4 v1, Vec4 v2)
        {
            addLine(v1, v2);
        }
        public void RemoveGObject(int hash)
        {
            remove(hash);
        }
        public GObject GetGObject(int hash)
        {
            return getObjectByHash(hash);
        }
        public void DrawBuffer(ZBuffer zBuffer, Rasterizer raster, ViewTransformator viewT)
        {
            drawStorage(zBuffer, raster, viewT);
        }
    }

    public class GObject
    {
        protected TMaker tmaker;

        public GObject(TMaker tmaker)
        {
            this.tmaker = tmaker;
        }
    }

}
