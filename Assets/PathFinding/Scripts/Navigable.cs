using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// Navigable - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:48:25 PM
/// 
/// </summary>
public class Navigable : MonoBehaviour, IPointerDownCollision
{
    public int size = 32;
    Node[,] nodes;
    private Node this[Index2D i] { get { return nodes[i.x, i.y]; } }

    struct Index2D
    {
        public int x, y;
        public Index2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public void OnPointerDown(Vector3 clickHit)
    { WorldPositionToNode(clickHit).ToggleTraversability(); }

    public void Start()
    {
        OnValidate();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Node n in nodes)
            { n.color = n.traversable ? Color.white : Color.black; }
        }
    }

    public void OnValidate()
    {
        size = Mathf.Max(size, 5);
        nodes = new Node[size,size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x==0 || x == size-1 || y == 0 || y == size-1)
                {
                    nodes[x, y] = new Node(new Vector3(x, 0, y), false);
                }
                else
                {
                    nodes[x, y] = new Node(new Vector3(x, 0, y), UnityEngine.Random.value > 0.2f);
                }
            }
        }
        //add neighbours
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                List<Node> neighbours = new List<Node>();
                if (x > 0)
                {
                    neighbours.Add(nodes[x - 1, y]);
                    if (y > 0)
                    { neighbours.Add(nodes[x - 1, y - 1]); }
                    if(y < size-1)
                    { neighbours.Add(nodes[x - 1, y + 1]); }
                }
                if (x < size-1)
                {
                    neighbours.Add(nodes[x + 1, y]);
                    if (y > 0)
                    { neighbours.Add(nodes[x + 1, y - 1]); }
                    if (y < size-1)
                    { neighbours.Add(nodes[x + 1, y + 1]); }
                }
                if (y > 0)
                { neighbours.Add(nodes[x, y - 1]); }
                if (y < size-1)
                { neighbours.Add(nodes[x, y + 1]); }
                nodes[x, y].AddNeighbours(neighbours.ToArray());
            }
        }
    }
    

    public Node WorldPositionToNode(Vector3 position)
    { return this[WorldPositionToIndex2D(position)]; }

    Index2D WorldPositionToIndex2D(Vector3 position)
    {
        return new Index2D(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
    }

    private void OnDrawGizmos()
    {
        foreach(Node n in nodes)
        {
            Gizmos.color = n.color;
            Gizmos.DrawCube(n.position, Vector3.one * 0.7f);
        }
    }
}

