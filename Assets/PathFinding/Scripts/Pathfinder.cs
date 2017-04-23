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

    public Gradient open;

    //public float maxDist { get { return Mathf.Sqrt(2 * grid.size * grid.size); } }
    
    int clickCount = 0;
    bool isPathSet { get { return clickCount % 2 == 0 && clickCount > 0; } }
    Vector3 startPos;

    //A*
    bool FindPath(Node start, Node end)
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        Heap<Node> openHeap = new Heap<Node>(grid.nodeCount);
        HashSet<Node> closedSet = new HashSet<Node>();

        Node current;
        openHeap.Add(start);
        start.color = openColor;
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
            current.color = closedColor;
            foreach (Node n in current.neighbours.Keys)
            {
                if (closedSet.Contains(n) || !n.traversable)
                { continue; } // skip traversed an nontraversable nodes

                float gCost = current.fCost + current.neighbours[n];
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
                }

            }
        }
        return false;
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
        current.color = Color.magenta;
        end.color = Color.magenta;
        for (int i = 0; i < path.Count-1; i++)
        {
            Debug.DrawLine(path[i].position, path[i + 1].position, Color.red,3);
        }
    }

    IEnumerator FindPathRoutine(Node start, Node end)
    {
        
        Heap<Node> openHeap = new Heap<Node>(grid.nodeCount);
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
            //current.color = open.Evaluate(current.hCost/maxDist);
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
            }
            yield return new WaitForSeconds(0);
            current = openHeap.Pop();
            //Debug.Log(current.fCost);
            //current.color = Color.cyan;
        }
    }

    public void OnPointerDown(Vector3 clickHit)
    {
        clickCount++;
        if(isPathSet)
        {
            //StartCoroutine(FindPathRoutine(grid.WorldPositionToNode(startPos), grid.WorldPositionToNode(clickHit)));
            FindPath(grid.WorldPositionToNode(startPos), grid.WorldPositionToNode(clickHit));
        }
        else
        {
            startPos = clickHit;
        }
    }
}