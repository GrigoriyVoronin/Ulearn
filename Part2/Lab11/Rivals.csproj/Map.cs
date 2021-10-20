using System;
using System.Collections.Generic;
using System.Drawing;

namespace Rivals
{
    public class Map
    {
        public readonly MapCell[,] Maze;
        public readonly Point[] Players;

        private Map(MapCell[,] maze, Point[] players)
        {
            Maze = maze;
            Players = players;
        }

        public static Map FromText(string text)
        {
            var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            return FromLines(lines);
        }

        public static Map FromLines(string[] lines)
        {
            var dungeon = new MapCell[lines[0].Length, lines.Length];
            var players = new List<Point>();
            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[0].Length; x++)
                switch (lines[y][x])
                {
                    case '#':
                        dungeon[x, y] = MapCell.Wall;
                        break;
                    case 'P':
                        dungeon[x, y] = MapCell.Empty;
                        players.Add(new Point(x, y));
                        break;
                    default:
                        dungeon[x, y] = MapCell.Empty;
                        break;
                }

            return new Map(dungeon, players.ToArray());
        }

        public bool InBounds(Point point)
        {
            var bounds = new Rectangle(0, 0, Maze.GetLength(0), Maze.GetLength(1));
            return bounds.Contains(point);
        }
    }
}