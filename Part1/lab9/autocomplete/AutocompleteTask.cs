using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return phrases[index];
            return null;
        }

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
            var arr = new List<string>();
            for (int i = 0; i < count; i++)
                if (i + index < phrases.Count && 
                    phrases[index + i].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    arr.Add(phrases[index + i]);
                else
                    break;
            return arr.ToArray();
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var end = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);
            var start = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            return end-start-1;
        }
    }

    [TestFixture]
    public class AutocompleteTests
    {
        [Test]
        public void TopByPrefix_IsEmpty_WhenNoPhrases()
        {
            var list = new List<string> { "aa", "ab", "bc", "bd", "be", "ca", "cb" };
            var actualTopWords = AutocompleteTask.GetTopByPrefix(list, "z", 2);
            CollectionAssert.IsEmpty(actualTopWords);
        }

        [Test]
        public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
        {
            var list = new List<string> { "aa", "ab", "bc", "bd", "be", "ca", "cb" };
            var actualCount = AutocompleteTask.GetCountByPrefix(list, "a");
            Assert.AreEqual(2, actualCount);
        }
    }
}
