using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Rivals
{
    public class ScenePainter
    {
        public SizeF Size => new SizeF(currentMap.Maze.GetLength(0), currentMap.Maze.GetLength(1));
        public Size LevelSize => new Size(currentMap.Maze.GetLength(0), currentMap.Maze.GetLength(1));

        private Map currentMap;
        private int currentIteration;
        private Bitmap mapImage;
        private Dictionary<Map, List<OwnedLocation>> mapStates;
        private static readonly Color[] colourValues = new []
        {
            "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF", "#000000",
            "#800000", "#008000", "#000080", "#808000", "#800080", "#008080", "#808080"
        }.Select(ColorTranslator.FromHtml).ToArray();

        public ScenePainter(Map[] maps)
        {
            currentMap = maps[0];
            PlayLevels(maps);
            currentIteration = 0;
            CreateMap();
        }

        private void PlayLevels(Map[] maps)
        {
            mapStates = new Dictionary<Map, List<OwnedLocation>>();
            foreach (var map in maps)
                mapStates[map] = RivalsTask.AssignOwners(map).ToList();
        }

        public void ChangeLevel(Map newMap)
        {
            currentMap = newMap;
            CreateMap();
            currentIteration = 0;
        }

        public void Update()
        {
            currentIteration = Math.Min(currentIteration + 1, mapStates[currentMap].Count);
        }

        public void Paint(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(mapImage, new Rectangle(0, 0, LevelSize.Width, LevelSize.Height));
            DrawPath(g);
        }

        private void DrawPath(Graphics graphics)
        {
            var mapState = mapStates[currentMap];
            foreach (var cell in mapState.Take(currentIteration))
            {
                var rect = new Rectangle(cell.Location.X, cell.Location.Y, 1, 1);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, colourValues[cell.Owner % colourValues.Length])), rect);
                graphics.DrawString(cell.Distance.ToString(), new Font(SystemFonts.DefaultFont.FontFamily, 0.3f), Brushes.Beige, rect,
                    new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
            }
        }

        private void CreateMap()
        {
            var dungeonWidth = currentMap.Maze.GetLength(0);
            var dungeonHeight = currentMap.Maze.GetLength(1);
            var cellWidth = Properties.Resources.Grass.Width;
            var cellHeight = Properties.Resources.Grass.Height;
            mapImage = new Bitmap(LevelSize.Width *cellWidth, LevelSize.Height * cellHeight);
            using (var graphics = Graphics.FromImage(mapImage))
            {
                for (var x = 0; x < dungeonWidth; x++)
                {
                    for (var y = 0; y < dungeonHeight; y++)
                    {
                        var image = currentMap.Maze[x, y] == MapCell.Empty ? Properties.Resources.Path : Properties.Resources.Grass;
                        graphics.DrawImage(image, new Rectangle(x*cellWidth, y*cellHeight, cellWidth, cellHeight));
                    }
                }
            }
        }
    }
}
