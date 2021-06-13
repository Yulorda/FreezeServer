using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

internal class PathFinderGraph
{
    public readonly Grid<Node> grid;
    private readonly List<Node> open = new List<Node>();
    private readonly List<Node> close = new List<Node>();

    public bool HasOpenNodes
    {
        get
        {
            return open.Count > 0;
        }
    }

    public PathFinderGraph(Grid<Node> grid)
    {
        this.grid = grid;
        open.Clear();
    }

    public IEnumerable<Node> GetNeighbors(Node node)
    {
        return grid
            .GetNeighbors(node)
            .Where(neighbour => !close.Contains(neighbour));
    }

    public Node GetParent(Node node)
    {
        return grid[node.Parent];
    }

    public Node GetNextOpenPoint()
    {
        open.Sort();
        var result = open[0];
        open.Remove(result);
        AddToClose(result);
        return result;
    }

    public void AddToClose(Node node)
    {
        close.Add(node);
    }

    public void AddToOpen(Node node)
    {
        open.Add(node);
    }

    public void Set(Node node)
    {
        grid[node.Position] = node;
    }

    public Node GetClosest()
    {
        return close.Aggregate((a, b) => a.F < b.F ? a : b);
    }
}