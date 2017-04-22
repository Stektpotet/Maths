using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// Navigable - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:48:25 PM
/// 
/// </summary>
public class Grid : Navigable
{
    public int size = 32; 
    public Node this[int x, int y]
    {
        get { return m_nodes[x + y * size]; }
        set { m_nodes[x + y * size] = value; }
    }
    private Node this[Index2D i] { get { return this[i.x, i.y]; } }

    struct Index2D
    {
        public int x, y;
        public Index2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public void Start()
    {
        ConnectNodes();
    }

    public void OnValidate()
    {
        size = Mathf.Max(size, 5);
        m_nodes = new Node[size * size];
        CreateNodes();
    }


    protected void CreateNodes()
    {
        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                if(x == 0 || x == size - 1 || y == 0 || y == size - 1)
                {
                    this[x, y] = new Node(new Vector3(x, 0, y), false);
                }
                else
                {
                    this[x, y] = new Node(new Vector3(x, 0, y), UnityEngine.Random.value > 0.2f);
                }
            }
        }
    }
    /// <summary>
    /// Creates the Network/Graph
    /// </summary>
    protected void ConnectNodes()
    {
        for(int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                List<Node> neighbours = new List<Node>();
                if(x > 0)
                {
                    neighbours.Add(this[x - 1, y]);
                    if(y > 0)
                    { neighbours.Add(this[x - 1, y - 1]); }
                    if(y < size - 1)
                    { neighbours.Add(this[x - 1, y + 1]); }
                }
                if(x < size - 1)
                {
                    neighbours.Add(this[x + 1, y]);
                    if(y > 0)
                    { neighbours.Add(this[x + 1, y - 1]); }
                    if(y < size - 1)
                    { neighbours.Add(this[x + 1, y + 1]); }
                }
                if(y > 0)
                { neighbours.Add(this[x, y - 1]); }
                if(y < size - 1)
                { neighbours.Add(this[x, y + 1]); }
                this[x, y].AddNeighbours(neighbours.ToArray());
            }
        }
    }

    protected override void SetupNetwork()
    {
        CreateNodes();
        ConnectNodes();
    }

    public override Node WorldPositionToNode(Vector3 position)
    { return this[WorldPositionToIndex2D(position)]; }

    Index2D WorldPositionToIndex2D(Vector3 position)
    {
        return new Index2D(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
    }
}

