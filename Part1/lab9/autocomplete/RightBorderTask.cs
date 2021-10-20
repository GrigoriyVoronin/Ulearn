using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete
{
    public class RightBorderTask
    {
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, long left, long right)
        {
            long middle = 0;
            while (left + 1 < right)
            {
                middle = (left + right) / 2;
                if (phrases[(int)middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    left = middle;
                else if (string.Compare(prefix, phrases[(int)middle], StringComparison.OrdinalIgnoreCase) < 0)
                    right = middle;
                else
                    left = middle;
            }
            return (int)right;
        }
    }
}