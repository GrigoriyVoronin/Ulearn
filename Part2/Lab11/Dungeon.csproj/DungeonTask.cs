using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var roadsFromStart = BfsTask
                .FindPaths(map, map.InitialPosition, map.Chests);
            var roadsFromExit = BfsTask
                .FindPaths(map, map.Exit, map.Chests);
            var lengths = new List<int>();
            var fullpaths = roadsFromExit
                .Join(roadsFromStart,
                    x => x.Value,
                    y => y.Value,
                    (x, y) =>
                    {
                        lengths.Add(x.Length + y.Length - 1);
                        return y.Reverse().Concat(x.Skip(1));
                    });

            fullpaths.All(x => true);

            if (fullpaths.Any())
                return TransformToDirections(fullpaths.Skip(lengths.IndexOf(lengths.Min())).First());

            fullpaths = BfsTask.FindPaths(map, map.InitialPosition, new[] {map.Exit});
            return fullpaths.Any() ? TransformToDirections(fullpaths.First().Reverse()) : new MoveDirection[0];
        }

        private static MoveDirection[] TransformToDirections(IEnumerable<Point> path)
        {
            var old = path.First();
            var size = new Size();
            return path.Skip(1).Select(
                    x =>
                    {
                        size.Width = x.X - old.X;
                        size.Height = x.Y - old.Y;
                        var w = Walker.ConvertOffsetToDirection(size);
                        old = x;
                        return w;
                    })
                .ToArray();
        }
    }
}