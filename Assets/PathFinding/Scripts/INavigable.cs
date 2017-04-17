using UnityEngine;
using System.Collections.Generic;

public abstract class INavigable<T> : ICollection<T> where T : Node
{
    //ICollection<T> nodes { get; }
    //T Traverse();
}

public class Path<T> : INavigable<T> where T : Node
{
    List<T> pathNodes;
    public ICollection<T> nodes
    { get { return pathNodes; } }
}

public class NavNet : INavigable<Node>
{
    public HashSet<Node> n;
    public ICollection<Node> nodes
    { get { return n; } }

    Path<Node> CalculatePath(Node start, Node end)
    {
        Path<Node> path = new Path<Node>();
        if(n.Contains(start) && n.Contains(end))
        {
            Node nextNode = start;
            foreach(Node node in start.connections)
            {
                node.hCost = Mathf.Min(node.hCost, ( node.position - start.position ).magnitude * node.costMultiplier);
                node.gCost = Mathf.Min(node.gCost, ( end.position  - end.position   ).magnitude * node.costMultiplier);
                if(node.fCost < nextNode.fCost) { nextNode = node; }
            }
            nextNode
            CalculatePath(nextNode, end);
        }
        else return new Path<Node>();
    }
}