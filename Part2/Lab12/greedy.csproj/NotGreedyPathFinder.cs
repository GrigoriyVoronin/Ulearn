using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class NotGreedyPathFinder : IPathFinder
    {
        private readonly DijkstraPathFinder pathFinder = new DijkstraPathFinder();

        private readonly List<(List<int>, int)> permutations = new List<(List<int>, int)>();

        private int maxEnergy;

        public List<Point> FindPathToCompleteGoal(State state)
        {
            maxEnergy = state.Energy;
            var chests = state.Chests.ToList();
            chests.Add(state.Position);
            chests.Reverse();
            var (costs, roads) = CalculateRoadsAndCosts(state, chests);

            MakePermutations(costs, chests);
            var (optimalPath, chestsCount) =
                permutations.FirstOrDefault(x => x.Item2 == permutations.Max(y => y.Item2));
            var path = new List<Point>();
            for (var i = 1; i < chestsCount; i++)
                path.AddRange(roads[optimalPath[i - 1]][optimalPath[i]].Skip(1));
            return path;
        }

        private (Dictionary<int, Dictionary<int, int>>, Dictionary<int, Dictionary<int, List<Point>>>)
            CalculateRoadsAndCosts(State state, List<Point> chests)
        {
            var costs = new Dictionary<int, Dictionary<int, int>>();
            var roads = new Dictionary<int, Dictionary<int, List<Point>>>();
            for (var i = 0; i < chests.Count; i++)
            {
                var start = chests[i];
                var paths = pathFinder.GetPathsByDijkstra(state, start, chests).ToArray();
                costs[i] = new Dictionary<int, int>();
                roads[i] = new Dictionary<int, List<Point>>();
                for (var j = 0; j < paths.Length; j++)
                {
                    var currentPath = paths.First(x => x.End == chests[j]);
                    costs[i][j] = currentPath.Cost;
                    roads[i][j] = currentPath.Path;
                }
            }

            return (costs, roads);
        }

        private void MakePermutations(Dictionary<int, Dictionary<int, int>> costs, List<Point> chests)
        {
            var permutation = new int[chests.Count];
            permutation[0] = 0;
            MakePermutations(permutation, 1, costs, 0);
        }

        private void MakePermutations(int[] permutation, int position,
            Dictionary<int, Dictionary<int, int>> costs, int curCost)
        {
            if (curCost > maxEnergy)
            {
                permutations.Add((permutation.ToList(), position - 1));
                return;
            }

            if (permutation.Length == position)
            {
                permutations.Add((permutation.ToList(), position));
                return;
            }

            for (var i = 1; i < permutation.Length; i++)
            {
                if (Array.IndexOf(permutation, i, 0, position) != -1)
                    continue;

                permutation[position] = i;
                MakePermutations(permutation, position + 1, costs,
                    curCost + costs[permutation[position - 1]][permutation[position]]);
            }
        }
    }
}