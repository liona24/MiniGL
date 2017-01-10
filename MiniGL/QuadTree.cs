using System;
using System.Collections.Generic;

namespace MiniGL
{
    public interface IHasBoundaries
    {
        Rect Boundaries { get; }
    }

    public class QuadTree<T> : IHasBoundaries where T : IHasBoundaries
    {
        Rect bounds;
        bool hasChildren;
        QuadTree<T> tl, tr, bl, br;
        int numItemsBeforeSplit;
        int numItems;
        int numItemsInChildren;
        T[] items;

        public Rect Boundaries { get { return bounds; } }
        public int ItemCount { get { return numItems + numItemsInChildren; } }
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

        public List<T> GetItems()
        {
            var result = new List<T>();
            GetItems(result);
            return result;
        }
        public void GetItems(List<T> result)
        {
            if (numItemsInChildren > 0)
            {
                tl.GetItems(result);
                tr.GetItems(result);
                bl.GetItems(result);
                br.GetItems(result);
            }
            for (int i = 0; i < numItems; i++)
                result.Add(items[i]);
        }

        public List<T> GetIntersecting(Rect bounds)
        {
            var result = new List<T>();
            GetIntersecting(bounds, result);
            return result;
        }
        public void GetIntersecting(Rect bounds, List<T> result)
        {
            if (!this.bounds.IntersectsWith(bounds))
                return;

            if (numItemsInChildren > 0)
            {
                tl.GetIntersecting(bounds, result);
                tr.GetIntersecting(bounds, result);
                bl.GetIntersecting(bounds, result);
                br.GetIntersecting(bounds, result);
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
            if (tl.ItemCount > 0)
                tl.GetItems(toAdd);
            if (tr.ItemCount > 0)
                tr.GetItems(toAdd);
            if (bl.ItemCount > 0)
                bl.GetItems(toAdd);
            if (br.ItemCount > 0)
                br.GetItems(toAdd);
            for (int i = 0; i < toAdd.Count; i++)
                add(toAdd[i]);

            tl = null;
            tr = null;
            bl = null;
            br = null;
        }

    }

}