using System;
using System.Collections.Generic;
namespace Autocomplete
{
    public class LeftBorderTask
    {
        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, long left, long right)
        {
            if (left + 1 >= right)
                return (int)left;
            long middle = (left + right) / 2;
            if (phrases[(int) middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return GetLeftBorderIndex(phrases, prefix, left, middle);
            if (string.Compare( prefix, phrases[(int)middle], StringComparison.OrdinalIgnoreCase) < 0)
                return GetLeftBorderIndex(phrases, prefix, left, middle);
            return GetLeftBorderIndex(phrases, prefix, middle, right);
        }
    }
}