using System;
using System.Collections;
using System.Collections.Generic;

using GraphicsUtility;

namespace MiniGL
{
    public interface IHasBoundaries
    {
        Rect Boundaries { get; }
    }

    public class QuadTree<T> : IHasBoundaries, IEnumerable<T> where T : IHasBoundaries
    {
        Rect bounds;
        bool hasChildren;
        QuadTree<T> tl, tr, bl, br;
        int numItemsBeforeSplit;
        int numItems;
        int numItemsInChildren;
        T[] items;

        public Rect Boundaries { get { return bounds; } }
        public int Count { get { return numItems + numItemsInChildren; } }
        public bool HasChildren { get { return hasChildren; } }

        public QuadTree(int numItemsBeforeSplit, Rect bounds)
        {
            numItems = 0;
            hasChildren = false;
            numItemsInChildren = 0;
            this.bounds = bounds;
            this.numItemsBeforeSplit = numItemsBeforeSplit;
            items = new T[numItemsBeforeSplit];
        }
        public QuadTree(int numItemsBeforeSplit, Rect bounds, T[] items)
        : this(numItemsBeforeSplit, bounds)
        {
            foreach (T i in items)
                Insert(i);
        }
        public QuadTree(int numItemsBeforeSplit, Rect bounds, T item)
        : this(numItemsBeforeSplit, bounds)
        {
            Insert(item);
        }

        public bool Insert(T item)
        {
            if (bounds.Contains(item.Boundaries))
            {
                if (hasChildren)
                {
                    if (tl.Insert(item) || tl.Insert(item) || br.Insert(item) || bl.Insert(item))
                    {
                        numItemsInChildren++;
                        return true;
                    }

                    add(item);

                    return true;
                }
                if (numItems < numItemsBeforeSplit)
                {
                    add(item);
                    return true;
                }
                split();
                return Insert(item);
            }
            return false;
        }

        public bool Remove(T item)
        {
            if (!bounds.Contains(item.Boundaries))
                return false;

            if (numItemsInChildren > 0 && (tl.Remove(item) || tr.Remove(item) || bl.Remove(item) || br.Remove(item)))
            {
                if (--numItemsInChildren + numItems < numItemsBeforeSplit)
                    removeChildren();
                return true;
            }

            int i = 0;
            for (; i < numItems; i++)
            {
                if (items[i].Equals(item))
                    break;
            }
            if (i < numItems)
            {
                for (; i < numItems - 1; i++)
                    items[i] = items[i + 1];
                return true;
            }

            return false;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Collect().GetEnumerator();
        }
        public List<T> Collect()
        {
            var result = new List<T>();
            Collect(result);
            return result;
        }
        public void Collect(List<T> result)
        {
            if (numItemsInChildren > 0)
            {
                tl.Collect(result);
                tr.Collect(result);
                bl.Collect(result);
                br.Collect(result);
            }
            for (int i = 0; i < numItems; i++)
                result.Add(items[i]);
        }

        public List<T> CollectIntersection(Rect bounds)
        {
            var result = new List<T>();
            CollectIntersection(bounds, result);
            return result;
        }
        public void CollectIntersection(Rect bounds, List<T> result)
        {
            if (!this.bounds.IntersectsWith(bounds))
                return;

            if (numItemsInChildren > 0)
            {
                tl.CollectIntersection(bounds, result);
                tr.CollectIntersection(bounds, result);
                bl.CollectIntersection(bounds, result);
                br.CollectIntersection(bounds, result);
            }

            for (int i = 0; i < numItems; i++)
            {
                if (bounds.IntersectsWith(items[i].Boundaries))
                    result.Add(items[i]);
            }
        }

        private void add(T item)
        {

            if (numItems >= items.Length)
            {
                var tmp = new T[items.Length * 2];
                Array.Copy(items, tmp, items.Length);
                items = tmp;
            }

            items[numItems++] = item;
        }

        private void split()
        {
            hasChildren = true;
            double midX = (bounds.R - bounds.L) * 0.5 + bounds.L;
            double midY = (bounds.B - bounds.T) * 0.5 + bounds.T;
            tl = new QuadTree<T>(numItemsBeforeSplit, new Rect(bounds.L, bounds.T, midX, midY));
            tr = new QuadTree<T>(numItemsBeforeSplit, new Rect(midX, bounds.T, bounds.R, midY));
            bl = new QuadTree<T>(numItemsBeforeSplit, new Rect(bounds.L, midY, midX, bounds.B));
            br = new QuadTree<T>(numItemsBeforeSplit, new Rect(midX, midY, bounds.R, bounds.B));

            int n = 0;
            for (int i = 0; i < numItems; i++)
            {
                if (!(tl.Insert(items[i]) || tr.Insert(items[i]) || bl.Insert(items[i]) || br.Insert(items[i])))
                    items[n++] = items[i];
            }
            numItems = n;
        }

        private void removeChildren()
        {
            var toAdd = new List<T>(numItemsInChildren);
            numItemsInChildren = 0;
            hasChildren = false;
            if (tl.Count > 0)
                tl.Collect(toAdd);
            if (tr.Count > 0)
                tr.Collect(toAdd);
            if (bl.Count > 0)
                bl.Collect(toAdd);
            if (br.Count > 0)
                br.Collect(toAdd);
            for (int i = 0; i < toAdd.Count; i++)
                add(toAdd[i]);

            tl = null;
            tr = null;
            bl = null;
            br = null;
        }

        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

    }

}
