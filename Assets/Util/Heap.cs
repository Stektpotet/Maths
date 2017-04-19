using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
/// <summary>
/// Heap - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:52:25 PM
/// 
/// </summary>
public class Heap<T> : IEnumerable<T> where T : IHeapable<T>
{
    List<T> items;
    public int Count { get { return items.Count; } }

    public void Add(T item)
    {
        items.Add(item);
        T parent = items[ParentIndex(item.heapIndex)];
        while(item.CompareTo(parent) > 0 && item.heapIndex < 2)
        {
            Swap(item, parent);
            parent = items[ParentIndex(item.heapIndex)];
        }
    }
    
    public T Pop()
    {
        T poppedTop = items[0];
        items[0] = items[items.Count - 1];
        items[0].heapIndex = 0;

        items.RemoveAt(items.Count - 1);
        //SORT DOWN
        return poppedTop;
    }

    public bool Contains(T item)
    { return items.Contains(item); }

    public void Clear()
    { items.Clear(); }

    int ParentIndex(int n) { return (n - 1) / 2; }// n-1/2
    int LeftChild(int n) { return 2 * n + 1; }    // 2n+1
    int RightChild(int n) { return 2 * (n + 1); } // 2n+2

    void Swap(T a, T b)
    {
        items[a.heapIndex] = b;
        items[b.heapIndex] = a;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    { return GetEnumerator(); }
}

public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable { }

public interface IHeapable<T> : IComparable<T>
{ int heapIndex { get; set; } }