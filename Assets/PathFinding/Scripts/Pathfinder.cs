using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
/// <summary>
/// Pathfinder - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:42:05 PM
/// 
/// </summary>
public class Pathfinder : MonoBehaviour, IPointerDownCollision
{
    public Network grid;
    public Transform startPoint, endPoint;

    public Color openColor = Color.green;
    public Color closedColor = Color.red;
    public Color pathColor =  Color.blue;
    
    int clickCount = 0;
    bool isPathSet { get { return clickCount % 2 == 0 && clickCount > 0; } }
    Vector3 startPos;

    private void OnValidate()
    {
    }

    private void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;
        while (current != start)
        {
            path.Add(current);
            current = current.parent;
            current.color = pathColor;
        }
        path.Add(current);
        start.color = Color.magenta;
        end.color = Color.magenta;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.DrawLine(path[i].position, path[i + 1].position, Color.red);
        }
    }

    bool FindPathList(Node start, Node end)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(start);
        Node current = openSet[0];
        //start.color = openColor;
        while (openSet.Count > 0)
        {
            float smallestF = Mathf.Infinity;
            foreach(Node n in openSet)
            {
                if(n.fCost < smallestF)
                { current = n; smallestF = n.fCost; }
            }

            if (current == end)
            {
                sw.Stop();
                Debug.Log("Path found in: " + sw.ElapsedMilliseconds + " ms");
                RetracePath(start, end);
                return true; //success! 
            }

            closedSet.Add(current);
            openSet.Remove(current);
            //current.color = open.Evaluate(((float)closedSet.Count * 2 + 1) / grid.nodeCount);
            foreach (Node n in current.neighbours.Keys)
            {
                if (closedSet.Contains(n) || !n.traversable)
                { continue; } // skip traversed and nontraversable nodes

                float gCost = current.fCost + current.neighbours[n];
                if (gCost < n.gCost || !openSet.Contains(n))
                {
                    n.hCost = n.DistanceTo(end);
                    n.gCost = gCost;
                    n.parent = current;
                    if (!openSet.Contains(n))
                    {
                        openSet.Add(n);
                        //n.color = openColor;
                    }
                }

            }
        }
        Debug.Log("No path found!");
        return false;
    }

    //A*
    bool FindPath(Node start, Node end)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        Heap<Node> openHeap = new Heap<Node>(grid.nodeCount);
        HashSet<Node> closedSet = new HashSet<Node>();

        Node current;
        openHeap.Add(start);
        //start.color = openColor;
        while (openHeap.Count > 0)
        {
            current = openHeap.Pop();
            if (current == end)
            {
                sw.Stop();
                Debug.Log("Path found in: " + sw.ElapsedMilliseconds + " ms");
                RetracePath(start, end);
                return true; //success! 
            }

            closedSet.Add(current);
            //current.color = open.Evaluate(((float)closedSet.Count * 2 + 1) / grid.nodeCount);
            foreach (Node n in current.neighbours.Keys)
            {
                if (closedSet.Contains(n) || !n.traversable)
                { continue; } // skip traversed and nontraversable nodes

                float gCost = current.gCost + current.neighbours[n];
                if (gCost < n.gCost || !openHeap.Contains(n))
                {
                    n.hCost = n.DistanceTo(end);
                    n.gCost = gCost;
                    n.parent = current;
                    if (!openHeap.Contains(n))
                    {
                        openHeap.Add(n);
                        //n.color = openColor;
                    }
                    else
                    { openHeap.Update(n); }
                }

            }
        }
        //Debug.Log("No path found!");
        return false;
    }
    
    IEnumerator FindPathRoutine(Node start, Node end)
    {
        Heap<Node> openHeap = new Heap<Node>(grid.nodeCount);
        HashSet<Node> closedSet = new HashSet<Node>();

        Node current;
        openHeap.Add(start);
        start.color = openColor;
        int i = 0;
        while (openHeap.Count > 0)
        {
            current = openHeap.Pop();
            if (current == end)
            {
                RetracePath(start, end);
                break;
            }

            closedSet.Add(current);
            //current.color = open.Evaluate(current.hCost/maxDist);
            current.color = closedColor;

            foreach (Node n in current.neighbours.Keys)
            {
                if (closedSet.Contains(n) || !n.traversable)
                { continue; } // skip traversed and nontraversable nodes
                
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
                    else
                    { openHeap.Update(n); }
                }
            }
            if(i%(((int)Mathf.Sqrt(grid.nodeCount))/10)==0)
            { yield return null; }
            i++;
            //Debug.Log(current.fCost);
            //current.color = Color.cyan;
        }
    }

    public void OnPointerDown(Vector3 clickHit)
    {
        clickCount++;
        if(isPathSet)
        {
            StartCoroutine(FindPathRoutine(grid.WorldPositionToNode(startPos), grid.WorldPositionToNode(clickHit)));
        }
        else
        {
            startPos = clickHit;
        }
    }

    //public void OnKeyDown(KeyCode key)
    //{
    //    switch(key)
    //    {
    //        case KeyCode.Mouse4:break;
    //    }
    //}

    private void FixedUpdate()
    {
        //FindPath(grid.WorldPositionToNode(startPoint.position), grid.WorldPositionToNode(endPoint.position));
        //StartCoroutine(FindPathRoutine(grid.WorldPositionToNode(startPoint.position), grid.WorldPositionToNode(endPoint.position)));
    }
}