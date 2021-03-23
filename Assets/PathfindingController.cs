using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathfindingController : MonoBehaviour
{
    Node startNode;
    List<Node> nodes = new List<Node>();

    List<Node> visitedNodes = new List<Node>();
    List<Node> unvisitedNodes = new List<Node>();

    Dictionary<Node, int> shortestDistanceFromStart = new Dictionary<Node, int>();
    Dictionary<Node, Node> previousNode = new Dictionary<Node, Node>();

    private List<Node> path = new List<Node>();

    public void Start() {
        Node a = new Node("A", Upgrade.Armor);
        Node b = new Node("B");
        Node c = new Node("C");
        Node d = new Node("D", Upgrade.Dmg);
        Node e = new Node("E");

        a.AddNeighbor(b, 6);
        a.AddNeighbor(d, 1);

        b.AddNeighbor(c, 5);
        b.AddNeighbor(d, 2);
        b.AddNeighbor(e, 2);

        c.AddNeighbor(e, 5);

        d.AddNeighbor(e, 1);

        nodes.AddRange(new Node[] { a, b, c, d, e });
        unvisitedNodes.AddRange(nodes);

        startNode = GetNodeWithUpgrade(Upgrade.Armor);

        foreach(Node n in nodes) {
            shortestDistanceFromStart.Add(n, int.MaxValue);
        }

        shortestDistanceFromStart[startNode] = 0;
        previousNode.Add(a, a);

        CalculateDistances();

        Debug.Log($"Path to C:");
        string path = "start -> ";
        foreach(Node n in GetPathTo(c)) {
            path += $"{n.name} -> ";
        }
        path += "finish";
        Debug.Log(path);
    }

    public void CalculateDistances() {
        foreach(Node n in startNode.neighborNodes.Keys) {
            if(startNode.neighborNodes[n] < shortestDistanceFromStart[n]) {
                shortestDistanceFromStart[n] = n.neighborNodes[startNode];
                previousNode[n] = startNode;
            }
        }

        visitedNodes.Add(startNode);
        unvisitedNodes.Remove(startNode);

        int i = 0;
        while(unvisitedNodes.Count > 0 && i < 10) {
            i++;
            Node current = GetUnvisitedWithSmallestDistance();

            foreach(Node n in current.neighborNodes.Keys) {
                if(!visitedNodes.Contains(n)) {
                    if(shortestDistanceFromStart[current] + current.neighborNodes[n] < shortestDistanceFromStart[n]) {
                        shortestDistanceFromStart[n] = shortestDistanceFromStart[current] + current.neighborNodes[n];
                        previousNode[n] = current;
                    }
                }
            }

            visitedNodes.Add(current);
            unvisitedNodes.Remove(current);
        }

        Debug.Log("End of calculation");

        foreach(Node n in nodes) {
            Debug.Log($"{n.name} {shortestDistanceFromStart[n]} {previousNode[n].name}");
        }
    }

    public Node GetUnvisitedWithSmallestDistance() {
        Node result = unvisitedNodes.First();

        foreach(Node n in unvisitedNodes) {
            if(shortestDistanceFromStart[n] < shortestDistanceFromStart[result]) {
                result = n;
            }
        }

        return result;
    }

    public Node GetNodeWithUpgrade(Upgrade upgrade) {
        foreach(Node n in nodes) {
            if(n.upgrade == upgrade) {
                Debug.Log($"Node with {upgrade}: {n.name}");
                return n;
            }
        }
        Debug.Log($"Node with {upgrade} not found");
        return null;
    }

    public List<Node> GetPathTo(Node node) {
        path.Clear();
        path.Add(node);

        while(!path.Contains(startNode)) {
            path.Add(previousNode[path.Last()]);
        }

        path.Reverse();

        return path;
    }
}
