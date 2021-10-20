using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];
            for (var i = 1; i <= first.Count; i++)
            for (var j = 1; j <= second.Count; j++)
                if (first[i - 1] == second[j - 1])
                    opt[i, j] = opt[i - 1, j - 1] + 1;
                else
                    opt[i, j] = Math.Max(opt[i - 1, j - 1], Math.Max(opt[i - 1, j], opt[i, j - 1]));

            return opt;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var words = new List<string>();
            var lastNumb = opt[first.Count, second.Count];
            if (lastNumb == 0)
                return new List<string>();

            for (var i = first.Count; i >= 1; i--)
            for (var j = second.Count; j >= 1; j--)
                if (opt[i, j] == lastNumb && first[i - 1] == second[j - 1])
                {
                    lastNumb--;
                    words.Add(first[i - 1]);
                    break;
                }

            words.Reverse();
            return words;
        }
    }
}