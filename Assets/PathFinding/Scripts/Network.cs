using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// Navigable - Created by Halvor, DESKTOP-A2KUJ84 @ 4/17/2017 10:48:25 PM
/// 
/// </summary>
public abstract class Network : MonoBehaviour, IPointerDownCollision
{
    protected Node[] m_nodes;

    public int nodeCount { get { return m_nodes.Length; } }

    public void OnPointerDown(Vector3 clickHit)
    { WorldPositionToNode(clickHit).ToggleTraversability(); }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Node n in m_nodes)
            { n.color = n.traversable ? Color.white : Color.black; }
        }
    }

    protected abstract void SetupNetwork();

    //inefficient, override if possible
    public virtual Node WorldPositionToNode(Vector3 position)
    {
        Node closest = m_nodes[0];
        float distance = Mathf.Infinity;
        foreach(Node n in m_nodes)
        {
            float nodeDistance = Vector3.Distance(n.position, position);
            if( nodeDistance < distance )
            {
                closest = n;
                distance = nodeDistance;
            }
        }
        return closest;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < nodeCount; i++)
        {
            Node n = m_nodes[i];
            Gizmos.color = n.color;
            Gizmos.DrawCube(n.position, Vector3.one);
        }
    }

}

