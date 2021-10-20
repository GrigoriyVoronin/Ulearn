using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;

namespace Rivals
{
    public class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var queueM = new Queue<OwnedLocation>();
            InitPlayers(queueM, map);
            while (queueM.Count > 0)
            {
                var currentPoint = queueM.Dequeue();
                yield return currentPoint;

                for (var i = 0; i < 4; i++)
                    if (TryGoInDirection(map, currentPoint, (MoveDirection) i, out var newPoint))
                        queueM.Enqueue(newPoint);
            }
        }

        private static readonly Dictionary<MoveDirection, Size> OffsetToDirection = new Dictionary<MoveDirection, Size>
        {
            {MoveDirection.Up, new Size(0, -1)},
            {MoveDirection.Down, new Size(0, 1)},
            {MoveDirection.Left, new Size(-1, 0)},
            {MoveDirection.Right, new Size(1, 0)}
        };

        private static bool TryGoInDirection(Map map, OwnedLocation position, MoveDirection direction,
            out OwnedLocation potentialPoint)
        {
            potentialPoint = new OwnedLocation(
                position.Owner,
                position.Location + OffsetToDirection[direction],
                position.Distance + 1);
            if (!map.InBounds(potentialPoint.Location) ||
                map.Maze[potentialPoint.Location.X, potentialPoint.Location.Y] != MapCell.Empty)
                return false;

            map.Maze[potentialPoint.Location.X, potentialPoint.Location.Y] = MapCell.Wall;
            return true;
        }

        private static void InitPlayers(Queue<OwnedLocation> queueM, Map map)
        {
            for (var i = 0; i < map.Players.Length; i++)
            {
                queueM.Enqueue(new OwnedLocation(i, map.Players[i], 0));
                map.Maze[map.Players[i].X, map.Players[i].Y] = MapCell.Wall;
            }
        }

        private enum MoveDirection
        {
            Right,
            Up,
            Down,
            Left
        }
    }
}