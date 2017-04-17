using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    Node[,] nodes;

    void InitializeNodes(Node[,] nodes)
    {
        this.nodes = nodes;
    }
    void TraverseFromTo(Position a, Position b)
    {

    }



    void Traverse(Position a, Position b)
    {

    }

    void CalculateCosts(Position a, Position b)
    {
    }
}
struct Position
{
    public int x, y;
}

public class Node
{
    public Vector2 position;

    public override int GetHashCode()
    { return hash; }

    public Node()
    { hash = ; }

    int hash;
    public HashSet<Node> connections;
    bool traversable = true;
    public float hCost, gCost, fCost;
    public float costMultiplier;
}