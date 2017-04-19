using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Pathfinder - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:42:05 PM
/// 
/// </summary>
public class Pathfinder : MonoBehaviour
{
    Heap<Node> openHeap;
    HashSet<Node> closedSet = new HashSet<Node>();
    Stack<Node> path = new Stack<Node>();

    void Clear()
    {
        openHeap.Clear();
        closedSet.Clear();
        path.Clear();
    }

    //A*
    bool FindPath(Node start, Node end) 
    {
        Clear();
        Node current = start;
        openHeap.Add(start);
        while(openHeap.Count > 0)
        {
            if(current == end)
            {
                RetracePath(start,end);
                return true; //success! 
            }

            closedSet.Add(current);

            foreach(Node n in current.neighbours.Keys)
            {
                if (closedSet.Contains(n) || !n.traversable) 
                { continue; } // skip traversed an nontraversable nodes
                if (!openHeap.Contains(n))
                { openHeap.Add(n); }
                float gCost = current.fCost + current.neighbours[n];
                if(gCost < n.gCost)
                {
                    n.hCost = n.DistanceTo(end);
                    n.gCost = gCost;
                    n.parent = current;
                }
            }
            current = openHeap.Pop();
        }
        return false;
    }
    internal void RetracePath(Node start, Node end)
    {
        Node current = end;
        while (current != start)
        {
            path.Push(current);
            current = current.parent;
        }
        path.Push(current);
    }
}