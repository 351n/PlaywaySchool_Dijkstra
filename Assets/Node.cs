using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string name;
    public Dictionary<Node, int> neighborNodes = new Dictionary<Node, int>();
    public Upgrade upgrade;

    public Node(string name) {
        this.name = name;
        this.upgrade = Upgrade.None;
    }

    public Node(string name, Upgrade upgrade) {
        this.name = name;
        this.upgrade = upgrade;
    }

    internal void AddNeighbor(Node other, int distance) {
        neighborNodes.Add(other, distance);

        if(!other.neighborNodes.ContainsKey(this))
            other.AddNeighbor(this, distance);
    }
}

public enum Upgrade
{
    None,
    Armor,
    Dmg
}
