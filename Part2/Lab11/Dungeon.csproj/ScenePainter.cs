using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Dungeon
{
    public class ScenePainter
    {
        public SizeF Size => new SizeF(currentMap.Dungeon.GetLength(0), currentMap.Dungeon.GetLength(1));
        public Size LevelSize => new Size(currentMap.Dungeon.GetLength(0), currentMap.Dungeon.GetLength(1));

        private readonly Dictionary<Map, Point[]> paths;
        private Map currentMap;
        private int mainIteration;
        private Bitmap mapImage;

        private Point? lastMouseClick;
        private IEnumerable<List<Point>> pathsToChests;

        public ScenePainter(IReadOnlyList<Map> maps)
        {
            paths = maps.ToDictionary(x => x, x => TransformPath(x, DungeonTask.FindShortestPath(x)).ToArray());

            currentMap = maps[0];
            mainIteration = 0;
            CreateMap();
        }

        public void ChangeLevel(Map newMap)
        {
            currentMap = newMap;
            CreateMap();
            mainIteration = 0;
            lastMouseClick = null;
            pathsToChests = null;
        }

        public void Update()
        {
            mainIteration = Math.Min(mainIteration + 1, paths[currentMap].Length - 1);
        }

        public void OnMouseDown(Point location)
        {
            lastMouseClick = new Point(location.X, location.Y);
            pathsToChests = null;
            if (currentMap.InBounds(location) && currentMap.Dungeon[lastMouseClick.Value.X, lastMouseClick.Value.Y] == MapCell.Empty)
            {
                pathsToChests = BfsTask.FindPaths(currentMap, lastMouseClick.Value, currentMap.Chests)
                    .Select(x => x.ToList()).ToList();
                foreach (var pathsToChest in pathsToChests)
                    pathsToChest.Reverse();
            }
        }

        public void OnMouseUp()
        {
            pathsToChests = null;
        }

        public void Paint(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawLevel(g);
            DrawMainPath(g, mainIteration);
            if (pathsToChests != null && lastMouseClick.HasValue)
                DrawAdditionalPaths(g, lastMouseClick.Value);
        }

        private void DrawLevel(Graphics graphics)
        {
            graphics.DrawImage(mapImage, new Rectangle(0, 0, LevelSize.Width, LevelSize.Height));
            foreach (var chest in currentMap.Chests)
                graphics.DrawImage(Properties.Resources.Chest, new Rectangle(chest.X, chest.Y, 1, 1));
            graphics.DrawImage(Properties.Resources.Castle, new Rectangle(currentMap.Exit.X, currentMap.Exit.Y, 1, 1));
        }

        private void DrawPath(Graphics graphics, Color color, IEnumerable<Point> path)
        {
            var points = path.Select(x => new PointF(x.X + 0.5f, x.Y + 0.5f)).ToArray();
            var pen = new Pen(color, 0.15f)
            {
                DashStyle = DashStyle.Dash,
            };
            for (var i = 0; i < points.Length - 1; i++)
                graphics.DrawLine(pen, points[i], points[i + 1]);
        }

        private void DrawMainPath(Graphics graphics, int interation)
        {
            var path = paths[currentMap].Take(interation + 1).ToArray();
            DrawPath(graphics, Color.Green, path);
            var position = path[path.Length - 1];
            graphics.DrawImage(Properties.Resources.Peasant, new Rectangle(position.X, position.Y, 1, 1));
        }

        private void DrawAdditionalPaths(Graphics graphics, Point lastClick)
        {
            graphics.FillRectangle(Brushes.Red, new Rectangle(lastClick.X, lastClick.Y, 1, 1));
            foreach (var pathToChest in pathsToChests)
                DrawPath(graphics, Color.Red, pathToChest);
        }

        private IEnumerable<Point> TransformPath(Map map, MoveDirection[] path)
        {
            var walker = new Walker(map.InitialPosition);
            yield return map.InitialPosition;
            foreach (var direction in path)
            {
                walker = walker.WalkInDirection(map, direction);
                yield return walker.Position;
                if (walker.PointOfCollision.HasValue)
                    break;
            }
        }

        private void CreateMap()
        {
            var cellWidth = Properties.Resources.Grass.Width;
            var cellHeight = Properties.Resources.Grass.Height;
            mapImage = new Bitmap(LevelSize.Width * cellWidth, LevelSize.Height * cellHeight);
            using (var graphics = Graphics.FromImage(mapImage))
            {
                for (var x = 0; x < currentMap.Dungeon.GetLength(0); x++)
                {
                    for (var y = 0; y < currentMap.Dungeon.GetLength(1); y++)
                    {
                        var image = currentMap.Dungeon[x, y] == MapCell.Wall ? Properties.Resources.Grass : Properties.Resources.Path;
                        graphics.DrawImage(image, new Rectangle(x * cellWidth, y * cellHeight, cellWidth, cellHeight));
                    }
                }
            }
        }
    }
}
