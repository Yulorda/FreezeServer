public struct Position
{
    public int X { get; }

    public int Y { get; }

    public Position(int x = 0, int y = 0)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Position a, Position b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Position a, Position b)
    {
        return !a.Equals(b);
    }

    public override bool Equals(object other)
    {
        if (other is Position otherPoint)
        {
            return X == otherPoint.X && Y == otherPoint.Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return $"[{X}.{Y}]";
    }
}