using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// Node - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:44:38 PM
/// 
/// </summary>
public class Node : IHeapable<Node>
{
    public Color color;

    public Vector3 position;
    public bool traversable;

    private int m_heapIndex = 0;
    public Dictionary<Node, float> neighbours = new Dictionary<Node, float>();
    public Node parent = null;

    public float hCost, gCost;
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
    
    public Node(Vector3 position, bool traversable)
    {
        this.position = position;
        this.traversable = traversable;
        color = (traversable) ? Color.white : Color.black;
    }
    
    public void AddNeighbours(Node[] neighbours)
    {
        foreach(Node n in neighbours)
        { this.neighbours.Add(n, Vector3.Distance(n.position, position)); }
    }

    public void ToggleTraversability()
    {
        traversable = !traversable;
        color = (traversable) ? Color.white : Color.black;
    }

}