using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// Navigable - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:48:25 PM
/// 
/// </summary>
public class RandomGraph : Navigable
{
    public Bounds bounds = new Bounds(Vector3.zero, Vector3.one*10);
    public int m_nodeCount = 100;
    public int maxNeighbours;

    public void Start()
    {
        ConnectNodes();
    }

    public void OnValidate()
    {
        m_nodeCount = Mathf.Max(m_nodeCount, 5);
        m_nodes = new Node[m_nodeCount];
        maxNeighbours = Mathf.Min(maxNeighbours, m_nodeCount - 1);
        CreateNodes();
    }


    protected void CreateNodes()
    {
        for(int i = 0; i < m_nodeCount; i++)
        {
            Vector3 position = bounds.center + new Vector3(
                ( Random.value - 0.5f ) * bounds.size.x,
                ( Random.value - 0.5f ) * bounds.size.y,
                ( Random.value - 0.5f ) * bounds.size.z);
            m_nodes[i] = new Node(position, true);
        }
    }
    /// <summary>
    /// Creates the Network/Graph
    /// </summary>
    protected void ConnectNodes()
    {
        foreach(Node n in m_nodes)
        {
            List<Node> neighbours = new List<Node>(maxNeighbours);
            for(int i=0; i < Random.Range(0,maxNeighbours); i++)
            {
                int randomIndex;
                do
                { randomIndex = Random.Range(0, m_nodeCount); }
                while(m_nodes[randomIndex] != n || neighbours.Contains(m_nodes[randomIndex]));
                neighbours.Add(m_nodes[randomIndex]);
            }
            n.AddNeighbours(neighbours.ToArray());
        }
    }

    protected override void SetupNetwork()
    {
        CreateNodes();
        ConnectNodes();
    }
}

