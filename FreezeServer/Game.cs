using System;
using System.Collections.Generic;

namespace FreezeServer
{
    public class Game
    {
        public int boardRank;
        public Dictionary<int, UnitMoveState> unitMoveStates = new Dictionary<int, UnitMoveState>();
        private Dictionary<int, UnitMoveState> moveTasks = new Dictionary<int, UnitMoveState>();
        private Grid<bool> grid;
        private PathFinder pathFinder;

        public Game()
        {
            var random = new Random();
            boardRank = random.Next(7, 12);
            var unitLength = random.Next(3, 5);
            for (int i = 0; i < unitLength; i++)
            {
                unitMoveStates.Add(i, new UnitMoveState() { id = i, state = UnitState.Stay, position = new Position(i, i) });
            }

            grid = new Grid<bool>(boardRank, boardRank);
            pathFinder = new PathFinder(new NodeFactory(), grid);

            foreach (var unit in unitMoveStates.Values)
            {
                grid[unit.position] = true;
            }
        }

        public void Do(NetworkServer networkServer)
        {
            List<int> remove = new List<int>();

            foreach (var task in moveTasks.Values)
            {
                if (unitMoveStates.TryGetValue(task.id, out var element))
                {
                    var path = pathFinder.FindPath(element.position, task.position);
                    if (path.Length > 0)
                    {
                        grid[unitMoveStates[task.id].position] = false;
                        element.position = path[0];
                        grid[unitMoveStates[task.id].position] = true;
                    }

                    if (path.Length == 0 || path[0] == task.position)
                    {
                        remove.Add(element.id);
                        element.state = UnitState.Stay;
                    }
                    else
                    {
                        element.state = UnitState.Run;
                    }
                }
            }

            foreach (var point in moveTasks.Keys)
            {
                networkServer.Broadcast(unitMoveStates[point]);
            }

            foreach (var point in remove)
            {
                moveTasks.Remove(point);
            }
        }

        public void SetDestenition(MoveTask moveTask)
        {
            foreach (var task in moveTask.unitMoveStates)
            {
                if (!moveTasks.TryAdd(task.id, task))
                {
                    moveTasks[task.id] = task;
                }
            }
        }
    }
}