using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class Walker
    {
        private static readonly Dictionary<MoveDirection, Size> DirectionToOffset = new Dictionary<MoveDirection, Size>
        {
            {MoveDirection.Up, new Size(0, -1)},
            {MoveDirection.Down, new Size(0, 1)},
            {MoveDirection.Left, new Size(-1, 0)},
            {MoveDirection.Right, new Size(1, 0)}
        };

        private static readonly Dictionary<Size, MoveDirection> OffsetToDirection = new Dictionary<Size, MoveDirection>
        {
            {new Size(0, -1), MoveDirection.Up},
            {new Size(0, 1), MoveDirection.Down},
            {new Size(-1, 0), MoveDirection.Left},
            {new Size(1, 0), MoveDirection.Right}
        };

        public static readonly IReadOnlyList<Size> PossibleDirections = OffsetToDirection.Keys.ToList();


        public Point Position { get; }
        public Point? PointOfCollision { get; }

        public Walker(Point position)
        {
            Position = position;
            PointOfCollision = null;
        }

        private Walker(Point position, Point pointOfCollision)
        {
            Position = position;
            PointOfCollision = pointOfCollision;
        }

        public Walker WalkInDirection(Map map, MoveDirection direction)
        {
            var newPoint = Position + DirectionToOffset[direction];
            if (!map.InBounds(newPoint))
                return new Walker(Position, Position);

            return map.Dungeon[newPoint.X, newPoint.Y] == MapCell.Wall ? new Walker(newPoint, newPoint) : new Walker(newPoint);
        }

        public static MoveDirection ConvertOffsetToDirection(Size offset) => OffsetToDirection[offset];
    }
}