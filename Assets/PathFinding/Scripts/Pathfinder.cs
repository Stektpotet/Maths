using UnityEngine;
using System.Collections.Generic;
using System.Collections;
/// <summary>
/// Pathfinder - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:42:05 PM
/// 
/// </summary>
public class Pathfinder : MonoBehaviour, IPointerDownCollision
{
    public Navigable grid;

    public Color openColor = Color.green;
    public Color closedColor = Color.red;
    public Color pathColor =  Color.blue;
    
    int clickCount = 0;
    bool isPathSet { get { return clickCount % 2 == 0 && clickCount > 0; } }
    Vector3 startPos;

    //A*
    //bool FindPath(Node start, Node end) 
    //{

    //    Heap<Node> openHeap = new Heap<Node>(grid.size * grid.size);
    //    HashSet<Node> closedSet = new HashSet<Node>();
        
    //    Node current;
    //    openHeap.Add(start);
    //    start.color = openColor;
    //    while(openHeap.Count > 0)
    //    {
    //        current = openHeap.Pop();
    //        if (current == end)
    //        {
    //            RetracePath(start,end);
    //            return true; //success! 
    //        }

    //        closedSet.Add(current);
    //        current.color = closedColor;
    //        foreach(Node n in current.neighbours.Keys)
    //        {
    //            if (closedSet.Contains(n) || !n.traversable) 
    //            { continue; } // skip traversed an nontraversable nodes
    //            if (!openHeap.Contains(n))
    //            {
    //                openHeap.Add(n);
    //                n.color = openColor;
    //            }
    //            float gCost = current.fCost + current.neighbours[n];
    //            if(gCost < n.gCost)
    //            {
    //                n.hCost = n.DistanceTo(end);
    //                n.gCost = gCost;
    //                n.parent = current;
    //            }
    //        }
            
    //    }
    //    return false;
    //}
    private void RetracePath(Node start, Node end)
    {
        Stack<Node> path = new Stack<Node>();
        Node current = end;
        while (current != start)
        {
            path.Push(current);
            current = current.parent;
            current.color = pathColor;
        }
        path.Push(current);
        current.color = Color.magenta;
    }

    IEnumerator FindPathRoutine(Node start, Node end)
    {
        Heap<Node> openHeap = new Heap<Node>(grid.size*grid.size);
        HashSet<Node> closedSet = new HashSet<Node>();

        Node current = start;
        openHeap.Add(start);
        start.color = openColor;
        while (openHeap.Count > 0)
        {
            if (current == end)
            {
                RetracePath(start, end);
                break;
            }

            closedSet.Add(current);
            current.color = closedColor;

            foreach (Node n in current.neighbours.Keys)
            {
                if (closedSet.Contains(n) || !n.traversable)
                { continue; } // skip traversed an nontraversable nodes

                
                float gCost = current.gCost + current.neighbours[n];
                if (gCost < n.gCost || !openHeap.Contains(n))
                {
                    n.hCost = n.DistanceTo(end);
                    n.gCost = gCost;
                    n.parent = current;

                    if (!openHeap.Contains(n))
                    {
                        openHeap.Add(n);
                        n.color = openColor;
                    }
                    //else
                    //{ openHeap.Update(n); }
                }
                yield return null;
            }
            current = openHeap.Pop();
            //Debug.Log(current.fCost);
            //current.color = Color.cyan;
        }
    }

    IEnumerator FindPathRoutineList(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        Node current = start;
        openSet.Add(start);
        start.color = openColor;
        while (openSet.Count > 0)
        {
            for(int i=1;i<openSet.Count;i++)
            {
                if (openSet[i].fCost < current.fCost || openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                {
                    current = openSet[i];
                }
            }
            openSet.Remove(current);
            closedSet.Add(current);
            current.color = closedColor;

            if (current == end)
            {
                RetracePath(start, end);
                break;
            }

            foreach(Node n in current.neighbours.Keys)
            {
                if(!n.traversable || closedSet.Contains(n))
                {
                    continue;
                }
                float newGCost = current.gCost + current.neighbours[n];
                if(newGCost < n.gCost || !openSet.Contains(n))
                {
                    n.gCost = newGCost;
                    n.hCost = n.DistanceTo(end);
                    n.parent = current;
                    if(!openSet.Contains(n))
                    { openSet.Add(n); }
                }
            }
           
            //current.color = Color.cyan;
        }
        yield return null;
    }

    public void OnPointerDown(Vector3 clickHit)
    {
        clickCount++;
        if(isPathSet)
        {
            StartCoroutine(FindPathRoutine(grid.WorldPositionToNode(startPos), grid.WorldPositionToNode(clickHit)));
            //FindPath(grid.WorldPositionToNode(startPos), grid.WorldPositionToNode(clickHit));
        }
        else
        {
            startPos = clickHit;
        }
    }
}