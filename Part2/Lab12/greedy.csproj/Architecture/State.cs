using System.Collections.Generic;
using System.Drawing;

namespace Greedy.Architecture
{
    public class State
    {
        public string MazeName { get; }
        public int[,] CellCost { get; }
        public List<Point> Chests { get; }
        public int InitialEnergy { get; }

        /// <summary>
        /// Количество сундуков, которые нужно собрать.
        /// Для последней задачи (нежадного алгоритма) всегда равен -1 — там нужно собрать максимально возможное количество сундуков.
        /// </summary>
        public int Goal { get; private set; }

        public int MapWidth => CellCost.GetLength(0);
        public int MapHeight => CellCost.GetLength(1);

        public Point Position;
        public int Energy;
        public int Scores;

        public State(
            string mazeName,
            int initialEnergy,
            Point initialPosition,
            int[,] cellCost,
            int goal,
            IEnumerable<Point> chests)
        {
            MazeName = mazeName;
            InitialEnergy = Energy = initialEnergy;
            Position = initialPosition;
            Goal = goal;
            CellCost = cellCost;
            Chests = new List<Point>(chests);
        }

        public State(State state) : this(state.MazeName, state.InitialEnergy, state.Position, state.CellCost, state.Goal,
            state.Chests)
        {
            Energy = state.Energy;
            Scores = state.Scores;
        }

        public bool IsWallAt(Point at)
        {
            return IsWallAt(at.X, at.Y);
        }

        public bool IsWallAt(int x, int y)
        {
            return CellCost[x, y] == 0;
        }

        public bool InsideMap(Point p) => InsideMap(p.X, p.Y);

        public bool InsideMap(int x, int y)
        {
            return x >= 0 && x < MapWidth && y >= 0 && y < MapHeight;
        }

        public void RemoveGoal()
        {
            Goal = -1;
        }
    }
}