using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MiniGL
{
    //TODO add xml serialization
    
    ///<summary>
    /// Handles vertex and GObject storage
    ///</summary>
    public class GManager 
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
            }
        }

        private VInfo[] vertexStorage;
        private int fillLevelVertexStorage;
        
        private int activeHash;
        private readonly Dictionary<int, GObject> objectStorage;

        public GManager(int initialStorageSize)
        {
            vertexStorage = new VInfo[initialStorageSize];
            fillLevelVertexStorage = 0;
            activeHash = EMPTY_HASH;
            objectStorage = new Dictionary<int, GObject>();
            objectStorage.Add(activeHash, new GObject(new TMaker()));
        }
        ///<summary>
        ///Returns the GObject corresponding to the given hash
        ///</summary>
        public GObject GetObjectByHash(int hash)
        {
            return objectStorage[hash];
        }
        ///<summary>
        ///Adds a new GObject to the internal storage. All vertices will be linked to that GObject until another one becomes active
        ///</summary>
        public void AddGObject(GObject obj)
        {
            activeHash++;
            objectStorage.Add(activeHash, obj);
        }
        ///<summary>
        ///Adds a triangle to the vertex buffer and links it to the currently active GObject
        ///</summary>
        public void AddVertices(Vec4 v1, Vec4 v2, Vec4 v3)
        {
            add(new VInfo(v1, v2, v3, activeHash));
        }
        ///<summary>
        ///Adds a line to the vertex buffer and links it to the currently active GObject
        ///</summary>
        public void AddVertices(Vec4 v1, Vec4 v2)
        {
            add(new VInfo(v1, v2, activeHash));
        }
        ///<summary>
        ///Removes the GObject corresponding to the given hash from the internal storage and all linked vertices
        ///</summary>
        public void RemoveObjectByHash(int hash)
        {
            objectStorage.Remove(hash);
            for (int i = 0; i < fillLevelVertexStorage; i++)
            {
                if (vertexStorage[i].ObjHash == hash)
                    vertexStorage[i--] = vertexStorage[fillLevelVertexStorage--];
            }
        }
        ///<summary>
        ///Draws every vertex in the storage to the given zBuffer using the given rasterizer and ViewTransformator
        ///</summary>
        public void DrawStorage(ZBuffer zBuffer, Rasterizer raster, ViewTransformator viewT)
        {
            int cacheHash = EMPTY_HASH;
            GObject cacheObject = null;
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
        ///<summary>
        ///Clears the vertex storage and all removes all GObjects
        ///</summary>
        public void Clear()
        {
            activeHash = 0;
            fillLevelVertexStorage = 0;
            objectStorage.Clear();
            objectStorage.Add(EMPTY_HASH, new GObject(new TMaker()));
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

    public class GObject
    {
        protected TMaker tmaker;

        public GObject(TMaker tmaker)
        {
            this.tmaker = tmaker;
        }
    }
}
