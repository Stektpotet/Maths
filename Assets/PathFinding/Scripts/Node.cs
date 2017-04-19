using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// Node - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:44:38 PM
/// 
/// </summary>
public class Node : IHeapable<Node>
{
    public Vector3 position;
    public bool traversable;

    private int m_heapIndex;
    public Dictionary<Node, float> neighbours;
    public Node parent;

    public float hCost = Mathf.Infinity, gCost = Mathf.Infinity;
    public float fCost { get { return hCost + gCost; } }

    public int heapIndex
    {
        get { return m_heapIndex; }
        set { m_heapIndex = value; }
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if(compare == 0)
        { compare = hCost.CompareTo(other.hCost); }
        return -compare;
    }

    public float DistanceTo(Node other)
    { return Vector3.Distance(position, other.position); }
    
}