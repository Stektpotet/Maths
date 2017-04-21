using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
/// <summary>
/// Heap - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:52:25 PM
/// 
/// </summary>
//public class Heap<T> : IEnumerable<T> where T : IHeapable<T>
//{
//    List<T> items = new List<T>();
//    public int Count { get { return items.Count; } }

//    public void Add(T item)
//    {
//        items.Add(item);
//        SortUp(item);
//    }

//    public T Pop()
//    {
//        T top = items[0];
//        items[0] = items[Count - 1];
//        items[0].heapIndex = 0;

//        items.RemoveAt(Count - 1);
//        SortDown(items[0]);
//        return top;
//    }
//    public void Update(T item)
//    {
//        SortUp(item);
//    }

//    void SortUp(T item)
//    {
//        T parent = items[ParentIndex(item.heapIndex)];
//        if (item.CompareTo(parent) > 0)
//        {
//            Swap(item, parent);
//            SortUp(item); //recursively sort
//        }
//    }


//    void SortDown(T item)
//    {
//        int left = LeftChild(item.heapIndex);
//        int right = RightChild(item.heapIndex);
//        int swapIndex = 0;

//        if (left < Count)
//        {
//            swapIndex = left;

//            if (right < Count)
//            {
//                if (items[left].CompareTo(items[right]) < 0)
//                {
//                    swapIndex = right;
//                }
//            }

//            if (item.CompareTo(items[swapIndex]) < 0)
//            {
//                Swap(item, items[swapIndex]);
//            }
//            else
//            {
//                return;
//            }

//        }
//        else
//        {
//            return;
//        }
//    }

//    int ParentIndex(int n) { return (n - 1) / 2; }// n-1/2
//    int LeftChild(int n) { return 2 * n + 1; }    // 2n+1
//    int RightChild(int n) { return 2 * (n + 1); } // 2n+2

//    void Swap(T a, T b)
//    {
//        int aIndex = a.heapIndex;
//        items[aIndex] = b;
//        items[b.heapIndex] = a;
//        //assign new HeapIndices
//        a.heapIndex = b.heapIndex;
//        b.heapIndex = aIndex;

//    }

//    public bool Contains(T item)
//    { return items.Contains(item); }

//    public void Clear()
//    { items.Clear(); }


//    public IEnumerator<T> GetEnumerator()
//    {
//        return items.GetEnumerator();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    { return GetEnumerator(); }
//}

public class Heap<T> where T : IHeapable<T>
{

    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T Pop()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void Update(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.heapIndex * 2 + 1;
            int childIndexRight = item.heapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.heapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        int itemAIndex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = itemAIndex;
    }
}

public interface IHeapable<T> : IComparable<T>
{ int heapIndex { get; set; } }