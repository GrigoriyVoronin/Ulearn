using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            var roads = new HashSet<Point> {start};
            var hashChests = chests.ToHashSet();
            var directions = new[] {0, 1, 2, 3};
            queue.Enqueue(new SinglyLinkedList<Point>(start));
            while (queue.Count != 0)
            {
                var currentPath = queue.Dequeue();
                var walker = new Walker(currentPath.Value);
                foreach (var newPosition in
                    directions
                        .Select(x => walker.WalkInDirection(map, (MoveDirection) x))
                        .Where(x => x.PointOfCollision == null && !roads.Contains(x.Position))
                        .Select(x => x.Position))
                {
                    var path = new SinglyLinkedList<Point>(newPosition, currentPath);
                    queue.Enqueue(path);
                    roads.Add(newPosition);
                    if (hashChests.Contains(newPosition))
                        yield return path;
                }
            }
        }
    }
}