using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var chests = state.Chests.ToHashSet();
            var goal = 0;
            var myEnergy = 0;
            var pathFinder = new DijkstraPathFinder();
            var path = new List<Point>();
            var start = state.Position;
            while (myEnergy < state.Energy && goal < state.Goal)
            {
                var roadsByDijkstra = pathFinder.GetPathsByDijkstra(state, start, chests);

                var shortestWay = roadsByDijkstra.FirstOrDefault();
                if (shortestWay==null)
                    return new List<Point>();

                myEnergy += shortestWay.Cost;
                goal++;
                path.AddRange(shortestWay.Path.Skip(1));
                start = shortestWay.End;
                chests.Remove(start);
            }

            return goal < state.Goal || myEnergy > state.Energy ? new List<Point>() : path;
        }
    }
}