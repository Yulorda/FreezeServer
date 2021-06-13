using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PathFinder
{
    private IGridFactory<Node> factory;
    private Grid<bool> permission;
    private int limit = 1000;

    public PathFinder(IGridFactory<Node> factory, Grid<bool> permission)
    {
        this.permission = permission;
        this.factory = factory;
    }

    public Position[] FindPath(Position start, Position end)
    {
        var index = 0;
        PathFinderGraph graph = new PathFinderGraph(new Grid<Node>(permission.Height, permission.Width, factory));

        var startNode = new Node(position: start, g: 0, h: CalculateH(start, end), parent: start);
        graph.Set(startNode);
        graph.AddToOpen(startNode);

        while (graph.HasOpenNodes)
        {
            var q = graph.GetNextOpenPoint();

            if (q.Position == end)
            {
                return OrderClosedNodesAsArray(graph, startNode, q);
            }

            if (index > limit)
            {
                return OrderClosedNodesAsArray(graph, startNode, graph.GetClosest());
            }

            foreach (var successor in graph.GetNeighbors(q))
            {
                if (permission[successor.Position])
                {
                    continue;
                }

                var updatedSuccessor = new Node(
                    position: successor.Position,
                    g: q.G + 1,
                    h: CalculateH(successor.Position, end),
                    parent: q.Position);

                if (graph.grid[successor.Position].Position == end)
                {
                    return OrderClosedNodesAsArray(graph, startNode, updatedSuccessor);
                }

                if (IsBetterPath(updatedSuccessor, successor))
                {
                    graph.AddToOpen(updatedSuccessor);
                    graph.Set(updatedSuccessor);
                }
            }

            index++;
        }

        return OrderClosedNodesAsArray(graph, startNode, graph.GetClosest());
    }

    private bool IsBetterPath(Node newNode, Node oldNode)
    {
        return !oldNode.HasBeenVisited || (oldNode.HasBeenVisited && newNode.F < oldNode.F);
    }

    private Position[] OrderClosedNodesAsArray(PathFinderGraph graph, Node start, Node endNode)
    {
        var path = new Stack<Position>();

        var currentNode = endNode;

        while (currentNode.Position != start.Position)
        {
            path.Push(currentNode.Position);
            currentNode = graph.GetParent(currentNode);
        }

        return path.ToArray();
    }

    private float CalculateH(Position source, Position destination)
    {
        var heuristicEstimate = 2;
        var h = heuristicEstimate * (Math.Pow((source.X - destination.X), 2) + Math.Pow((source.Y - destination.Y), 2));
        return (float)h;
    }
}