using UnityEngine;
using System.Collections.Generic;
using System;
/// <summary>
/// GraphDebugger - Created by Halvor, DESKTOP-A2KUJ84 @ 4/22/2017 4:35:22 PM
/// </summary>
public class GraphDebugger : MonoBehaviour, IKeyDownEvent, IPointerDownCollision
{
    public Navigable network;
    Node current;
    public Color selectionColor;
    public void OnKeyDown(KeyCode key)
    {
        foreach(Node n in current.neighbours.Keys)
        {
            n.color = selectionColor;
        }
    }

    public void OnPointerDown(Vector3 clickHit)
    {
        current = network.WorldPositionToNode(clickHit);
    }
}
