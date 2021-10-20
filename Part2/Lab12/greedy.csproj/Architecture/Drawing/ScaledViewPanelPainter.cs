using System;
using System.Drawing;
using System.Windows.Forms;

namespace Greedy.Architecture.Drawing
{
    public class ScaledViewPanelPainter : IScenePainter
    {
        public SizeF Size => Controller == null
            ? new SizeF(1, 1)
            : new SizeF(state.MapWidth * CellSize, state.MapHeight * CellSize + 10);

        public ScaledViewPanel ScaledViewPanel { get; }

        public int CellSize => Sprites.Wall.Height;
        private State state => Controller.State;
        private string lastStateName;
        private Bitmap cachedMapBmp;

        public StateController Controller { get; set; }

        public ScaledViewPanelPainter()
        {
            ScaledViewPanel = new ScaledViewPanel(this) {Dock = DockStyle.Fill};
        }

        public void Paint(Graphics g, float zoomScale)
        {
            if (Controller == null)
                return;
            var clip = g.VisibleClipBounds;
            var top = (int) Math.Max(clip.Top / CellSize - 1, 0);
            var bottom = (int) Math.Min(clip.Bottom / CellSize + 1, state.MapHeight);
            var left = (int) Math.Max(clip.Left / CellSize - 1, 0);
            var right = (int) Math.Min(clip.Right / CellSize + 1, state.MapWidth);
            if (lastStateName == null || lastStateName != state.MazeName)
            {
                CacheMap(top, bottom, left, right);
                lastStateName = state.MazeName;
            }

            g.DrawImage(cachedMapBmp, 0, 0);
            DrawDigitsAndChests(g, zoomScale, top, bottom, left, right);
            DrawMovements(g);
            DrawPlayer(g);
        }

        private void DrawDigitsAndChests(Graphics g, float zoomScale, int top, int bottom, int left, int right)
        {
            var font = new Font("Segoe UI Light", 25);
            for (var y = top; y < bottom; y++)
            for (var x = left; x < right; x++)
            {
                var logicalLocation = new Point(x, y);
                var cellCoords = logicalLocation.MultiplyTransform(CellSize);
                if (!state.IsWallAt(x, y))
                {
                    var drawDigits = zoomScale > 0.5;
                    if (state.Chests.Contains(logicalLocation))
                    {
                        var chestLocation = drawDigits
                            ? new Point(cellCoords.X + 30, cellCoords.Y + 30)
                            : new Point(cellCoords.X + 15, cellCoords.Y + 15);
                        var chestRect = new Rectangle(chestLocation, new Size(CellSize - 25, CellSize - 30));
                        g.DrawImage(Controller.VisitedChests.Contains(logicalLocation) ? Sprites.ChestOpened : Sprites.ChestClosed,
                            chestRect);
                    }

                    if (drawDigits)
                        g.DrawString(state.CellCost[x, y].ToString(), font, Brushes.Black, cellCoords.X, cellCoords.Y);
                }
            }
        }

        private void DrawMovements(Graphics g)
        {
            var elSize = new Size(CellSize, CellSize);
            foreach (var logicalTo in Controller.MovementsToFrom.Keys)
            {
                var logicalsFrom = Controller.MovementsToFrom[logicalTo];
                var physicalTo = logicalTo.MultiplyTransform(CellSize);
                var currentCenter = new Rectangle(physicalTo, elSize).GetCenter();

                foreach (var logicalFrom in logicalsFrom)
                {
                    var physicalFrom = logicalFrom.MultiplyTransform(CellSize);
                    var prevCenter = new Rectangle(physicalFrom, elSize).GetCenter();
                    g.DrawLine(new Pen(Color.DarkBlue, 2f), prevCenter, currentCenter);
                }
            }
        }

        private void DrawPlayer(Graphics g)
        {
            if (state.InsideMap(state.Position))
            {
                var playerLocation = state.Position.MultiplyTransform(CellSize);
                if (Controller.GameIsLost)
                {
                    playerLocation.Y += 20;
                }

                g.DrawImage(PlayerImageProvider.ProvidePlayerImage(Controller), playerLocation);
            }
        }

        private void CacheMap(int top, int bottom, int left, int right)
        {
            cachedMapBmp = new Bitmap(state.MapWidth * CellSize, state.MapHeight * CellSize);
            using (var g = Graphics.FromImage(cachedMapBmp))
            {
                for (var y = top; y < bottom; y++)
                for (var x = left; x < right; x++)
                {
                    var logicalLocation = new Point(x, y);
                    var cellCoords = logicalLocation.MultiplyTransform(CellSize);
                    if (state.IsWallAt(x, y))
                    {
                        g.DrawImage(Sprites.Wall, cellCoords);
                    }
                    else if (!state.IsWallAt(x, y))
                    {
                        var gradientBrushForCell = GetGradientBrushForCell(state.CellCost[x, y], GetMinMax(state.CellCost));
                        g.FillRectangle(gradientBrushForCell, cellCoords.X, cellCoords.Y, CellSize, CellSize);
                    }
                }
            }
        }

        private Brush GetGradientBrushForCell(int density, Point minMax)
        {
            var levelsCount = minMax.Y - minMax.X;
            var delta = 50 / levelsCount;
            return new SolidBrush(Color.FromArgb(density * delta, Color.Red));
        }

        private Point GetMinMax(int[,] map)
        {
            var result = new Point(0, 0);
            for (var y = 0; y < map.GetLength(1); y++)
            for (var x = 0; x < map.GetLength(0); x++)
                if (map[x, y] > result.Y)
                    result.Y = map[x, y];
                else if (map[x, y] < result.X)
                    result.X = map[x, y];
            return result;
        }
    }
}