using System.Collections.Generic;

public class Grid<T>
{
    public int Height { get; }
    public int Width { get; }

    private T[] grid;

    public Grid(int height, int width)
    {
        Height = height;
        Width = width;

        grid = new T[height * width];

        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < width; column++)
            {
                grid[GetIndex(row, column)] = default(T);
            }
        }
    }

    public Grid(int height, int width, IGridFactory<T> gridFactory)
    {
        Height = height;
        Width = width;

        grid = new T[height * width];

        for (var row = 0; row < height; row++)
        {
            for (var column = 0; column < width; column++)
            {
                grid[GetIndex(row, column)] = gridFactory.GetT(row, column);
            }
        }
    }

    public IEnumerable<T> GetNeighbors(Node node)
    {
        foreach (var neighbourOffset in Offsets)
        {
            var successorRow = node.Position.X + neighbourOffset.row;

            if (successorRow < 0 || successorRow >= Height)
            {
                continue;
            }

            var successorColumn = node.Position.Y + neighbourOffset.column;

            if (successorColumn < 0 || successorColumn >= Width)
            {
                continue;
            }

            yield return this[successorRow, successorColumn];
        }
    }

    public T this[Position position]
    {
        get
        {
            return grid[GetIndex(position.X, position.Y)];
        }
        set
        {
            grid[GetIndex(position.X, position.Y)] = value;
        }
    }

    public T this[int row, int column]
    {
        get
        {
            return grid[GetIndex(row, column)];
        }
        set
        {
            grid[GetIndex(row, column)] = value;
        }
    }

    private int GetIndex(int row, int column)
    {
        return Width * row + column;
    }

    public static IEnumerable<(sbyte row, sbyte column)> Offsets
    {
        get
        {
            yield return (0, -1);
            yield return (1, 0);
            yield return (0, 1);
            yield return (-1, 0);
        }
    }
}

public interface IGridFactory<T>
{
    public T GetT(int row, int column);
}

public class NodeFactory : IGridFactory<Node>
{
    public Node GetT(int row, int column)
    {
        return new Node(new Position(row, column), 0, 0, new Position());
    }
}