using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Greedy.Architecture
{
    public class StateController
    {
        private bool isGreedyPathFinder;

        public Dictionary<Point, List<Point>> MovementsToFrom { get; } = new Dictionary<Point, List<Point>>();
        public Queue<Point> Path { get; }
        public State State { get; }
        public HashSet<Point> Chests { get; }
        public HashSet<Point> VisitedChests { get; } = new HashSet<Point>();
        public int Cost;

        public StateController(State state, IEnumerable<Point> path, bool isGreedyPathFinder = true) : this(state)
        {
            this.isGreedyPathFinder = isGreedyPathFinder;
            Path = new Queue<Point>(path);
        }

        private StateController(State state)
        {
            State = new State(state);
            Chests = new HashSet<Point>(State.Chests);
            CollectChestIfStandingOnIt();
            SetGameOverMessageIfOutOfEnergy();
        }

        public string GameLostMessage { get; private set; }

        public bool GameIsLost => GameLostMessage != null;
        public bool GameIsWon => State.Scores >= State.Goal && !GameIsLost;

        public void MoveThroughPathFast()
        {
            while (TryMoveOneStep()) ;
        }

        public bool TryMoveOneStep()
        {
            if (GameIsLost)
                return false;
            if (!Path.Any())
            {
                if (!GameIsWon)
                    GameLostMessage = isGreedyPathFinder
                        ? "No more steps. Not able to reach scores goal"
                        : "No more steps. Not able to collect maximum possible chests";
                return false;
            }
            var nextPosition = Path.Dequeue();
            if (!State.InsideMap(nextPosition))
            {
                GameLostMessage = $"Can't move to {nextPosition}";
                return false;
            }
            var movementDistance = DistanceBetween(State.Position, nextPosition);
            if (movementDistance == 0)
            {
                GameLostMessage = $"Player can't move from its position to same position: {State.Position}";
                return false;
            }
            if (movementDistance > 1)
            {
                GameLostMessage =
                    $"Player can only step in range of one cell. Can't jump from {State.Position} to {nextPosition}: they are not neighbours!";
                return false;
            }
            MemorizeMovementFromTo(State.Position, nextPosition);
            State.Position = nextPosition;
            CollectChestIfStandingOnIt();
            if (State.IsWallAt(nextPosition))
            {
                GameLostMessage = "Crashed against wall";
                return false;
            }
            State.Energy -= State.CellCost[State.Position.X, State.Position.Y];
            return !SetGameOverMessageIfOutOfEnergy();
        }

        private void CollectChestIfStandingOnIt()
        {
            if (Chests.Contains(State.Position))
            {
                State.Scores++;
                VisitedChests.Add(State.Position);
                Chests.Remove(State.Position);
            }
        }

        private int DistanceBetween(Point statePosition, Point nextPosition)
        {
            return Math.Abs(nextPosition.X - statePosition.X) + Math.Abs(nextPosition.Y - statePosition.Y);
        }

        private bool SetGameOverMessageIfOutOfEnergy()
        {
            if (State.Energy < 0)
            {
                GameLostMessage = "Out of energy";
                return true;
            }
            return false;
        }

        private void MemorizeMovementFromTo(Point from, Point to)
        {
            if (!MovementsToFrom.ContainsKey(to))
                MovementsToFrom[to] = new List<Point> {from};
            else
                MovementsToFrom[to].Add(from);
        }
    }
}