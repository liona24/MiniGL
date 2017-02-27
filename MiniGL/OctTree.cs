using System.Collections;
using System.Collections.Generic;

using GraphicsUtility;

namespace MiniGL
{

    public interface IHasBoundaries3
    {
        Cuboid Boundaries { get; }
    }

    public class OctTree<T> : IHasBoundaries3, IEnumerable<T>
        where T : IHasBoundaries3
    {
        OctTree<T>[] children;
        int maxNumItems;
        List<T> items;
        Cuboid boundaries;
        int numItemsInChildren;

        public Cuboid Boundaries { get { return boundaries; } }
        public int Count { get { return numItemsInChildren + items.Count; } }
        public bool HasChildren { get { return children != null; } }

        public OctTree(Cuboid boundaries, int maxItemsPerNode)
        {
            maxNumItems = maxItemsPerNode;
            numItemsInChildren = 0;
            this.boundaries = boundaries;
            items = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Collect().GetEnumerator();
        }

        public bool Insert(T item)
        {
            if (!boundaries.Contains(item.Boundaries))
                return false;

            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (children[i].Insert(item))
                    {
                        numItemsInChildren++;
                        return true;
                    }
                }
            }
            else if (items.Count == maxNumItems)
            {
                split();
                return Insert(item);
            }

            items.Add(item);
            return true;
        }

        public bool Remove(T item)
        {
            if (!boundaries.Contains(item.Boundaries))
                return false;

            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (children[i].Remove(item))
                    {
                        if ((--numItemsInChildren + items.Count) / 2 <= maxNumItems)
                        {
                            collectFromChildren(items);
                            children = null;
                        }
                        return true;
                    }
                }
            }
            return items.Remove(item);
        }

        public List<T> CollectIntersection(Cuboid volume)
        {
            var ret = new List<T>();
            CollectIntersection(volume, ret);
            return ret;
        }
        public void CollectIntersection(Cuboid volume, List<T> collection)
        {
            if (!volume.IntersectsWith(boundaries))
                return;

            if (children != null)
            {
                for (int i = 0; i < 8; i++)
                    children[i].CollectIntersection(volume, collection);
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Boundaries.IntersectsWith(volume))
                    collection.Add(items[i]);
            }
        }

        public List<T> Collect()
        {
            var ret = new List<T>();
            Collect(ret);
            return ret;
        }
        public void Collect(List<T> collection)
        {
            for (int i = 0; i < items.Count; i++)
                collection.Add(items[i]);
            if (children != null)
                collectFromChildren(collection);
        }

        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
        private void collectFromChildren(List<T> coll)
        {
            for (int i = 0; i < 8; i++)
                children[i].Collect(coll);
        }

        private void split()
        {
            double w2 = boundaries.Width / 2;
            double h2 = boundaries.Height / 2;
            double d2 = boundaries.Depth / 2;

            children = new OctTree<T>[8];
            children[0] = new OctTree<T>(new Cuboid(boundaries.X, boundaries.Y, boundaries.Z, w2, h2, d2), maxNumItems);
            children[1] = new OctTree<T>(new Cuboid(boundaries.X + w2, boundaries.Y, boundaries.Z, w2, h2, d2), maxNumItems);
            children[2] = new OctTree<T>(new Cuboid(boundaries.X + w2, boundaries.Y + h2, boundaries.Z, w2, h2, d2), maxNumItems);
            children[3] = new OctTree<T>(new Cuboid(boundaries.X, boundaries.Y + h2, boundaries.Z, w2, h2, d2), maxNumItems);
            children[4] = new OctTree<T>(new Cuboid(boundaries.X + w2, boundaries.Y + h2, boundaries.Z + d2, w2, h2, d2), maxNumItems);
            children[5] = new OctTree<T>(new Cuboid(boundaries.X + w2, boundaries.Y, boundaries.Z + d2, w2, h2, d2), maxNumItems);
            children[6] = new OctTree<T>(new Cuboid(boundaries.X, boundaries.Y + h2, boundaries.Z + d2, w2, h2, d2), maxNumItems);
            children[7] = new OctTree<T>(new Cuboid(boundaries.X, boundaries.Y, boundaries.Z + d2, w2, h2, d2), maxNumItems);

            dump();
        }

        private void dump()
        {
            var nItems = new List<T>();
            for (int i = 0; i < items.Count; i++)
            {
                bool isDumped = false;
                for (int j = 0; j < 8; j++)
                {
                    if (children[j].Insert(items[i]))
                    {
                        numItemsInChildren++;
                        isDumped = true;
                        break;
                    }
                }
                if (!isDumped)
                    nItems.Add(items[i]);
            }
            items = nItems;
        }
    }
}

