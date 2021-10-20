using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTrees
{
    public static class ShuffleExtension
    {
        private static readonly Random Rnd = new Random(42);

        public static List<T> Shuffle<T>(this IEnumerable<T> _list)
        {
            var list = _list.ToList();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Rnd.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}