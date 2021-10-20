using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class DijkstraPathFinder
    {
        private readonly Point[] directions = {new Point(0, 1), new Point(0, -1), new Point(-1, 0), new Point(1, 0)};

        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start,
            IEnumerable<Point> targets)
        {
            var chests = targets.ToHashSet();
            var visited = new Dictionary<Point, int>();
            var noVisited = new Dictionary<Point, int> {{start, 0}};
            var paths = new Dictionary<Point, Point?> {{start, null}};
            var nextPoints = new HashSet<Point> {start};
            while (chests.Any())
            {
                if (!nextPoints.Any())
                    yield break;

                var currentP = TakePointAndFindNeighbors(visited, noVisited, nextPoints, state, paths);
                visited.Add(currentP, noVisited[currentP]);
                noVisited.Remove(currentP);
                if (!chests.Contains(currentP))
                    continue;

                chests.Remove(currentP);
                yield return TakePath(paths, currentP, visited[currentP]);
            }
        }

        private Point TakePointAndFindNeighbors(Dictionary<Point, int> visited, Dictionary<Point, int> noVisited,
            HashSet<Point> nextPoints, State state, Dictionary<Point, Point?> paths)
        {
            var currentP = nextPoints
                .First(x => noVisited[x] == noVisited.Values.Min());
            nextPoints.Remove(currentP);
            foreach (var next in directions
                .Select(direction =>
                    new Point(direction.X + currentP.X, direction.Y + currentP.Y))
                .Where(next =>
                    state.InsideMap(next)
                    && !visited.ContainsKey(next)
                    && !nextPoints.Contains(next)
                    && !state.IsWallAt(next)
                    && (!noVisited.ContainsKey(next) ||
                        noVisited[next] < noVisited[currentP] + state.CellCost[next.X, next.Y])))
            {
                noVisited[next] = noVisited[currentP] + state.CellCost[next.X, next.Y];
                nextPoints.Add(next);
                paths[next] = currentP;
            }

            return currentP;
        }

        private static PathWithCost TakePath(IReadOnlyDictionary<Point, Point?> paths, Point endP, int cost)
        {
            var path = new PathWithCost(cost);
            Point? point = endP;
            while (point != null)
            {
                path.Path.Add((Point) point);
                point = paths[(Point) point];
            }

            path.Path.Reverse();
            return path;
        }
    }
}