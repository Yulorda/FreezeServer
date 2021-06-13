using System;

public struct Node : IEquatable<Node>, IComparable<Node>
{
    public Position Position { get; }
    public float G { get; }
    private float h;

    public float H
    {
        get
        {
            return h;
        }
        set
        {
            h = value;
            F = G + h;
        }
    }

    public Position Parent { get; }
    public float F { get; private set; }
    public bool HasBeenVisited => F > 0;

    public Node(Position position, float g, float h, Position parent)
    {
        Position = position;
        G = g;
        this.h = h;
        Parent = parent;
        F = g + h;
    }

    public int CompareTo(Node other)
    {
        if (this.F > other.F)
        {
            return 1;
        }

        if (this.F < this.F)
        {
            return -1;
        }

        return 0;
    }

    public bool Equals(Node other)
    {
        return this.Position == other.Position;
    }
}