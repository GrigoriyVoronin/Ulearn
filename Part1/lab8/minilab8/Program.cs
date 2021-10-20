using System;
using System.Collections.Generic;
using System.Linq;

namespace minilab8
{
    public class CaseAlternatorTask
    {
        //Поиск паролей
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            return result;
        }

        static void AlternateCharCases(char[] words, int startIndex, List<string> result)
        {
            var word = (char[])words.Clone();
            if (startIndex == word.Length)
            {
                result.Add(string.Join("", word));
                Console.WriteLine(string.Join("", word));
            }
            else
            {
                AlternateCharCases(word, startIndex + 1, result);
                if (char.IsLetter(word[startIndex]) && char.IsLower(word[startIndex]))
                {
                    if (char.ToLower(word[startIndex]) == char.ToUpper(word[startIndex]))
                        return;
                    word[startIndex] = char.ToUpper(word[startIndex]);
                    AlternateCharCases(word, startIndex + 1, result);
                }
            }
        }
    }

    class Program
    {
        public static void WriteReversed(char[] items, int startIndex = 0)
        {
            if (startIndex == items.Length)
                return;
            WriteReversed(items, startIndex + 1);
            Console.Write(items[startIndex]);
        }

        static void MakeSubsets(char[] subset, int position = 0)
        {
            if (position == subset.Length)
            {
                Console.WriteLine(new string(subset));
                return;
            }
            subset[position] = 'a';
            MakeSubsets(subset, position + 1);
            subset[position] = 'b';
            MakeSubsets(subset, position + 1);
            subset[position] = 'c';
            MakeSubsets(subset, position + 1);
        }

        static void WriteAllWordsOfSize(int size)
        {
            MakeSubsets(new char[size]);
        }

        static void TestOnSize(int size)
        {
            var result = new List<int[]>();
            MakePermutations(new int[size], 0, result);
            foreach (var permutation in result)
                WritePermutation(permutation);
        }

        static void WritePermutation(int[] permutation)
        {
            var strings = new List<string>();
            foreach (var i in permutation)
                strings.Add(i.ToString());
            Console.WriteLine(string.Join(" ", strings.ToArray()));
        }

        static void MakePermutations(int[] permutation, int position, List<int[]> result)
        {
            if (position == permutation.Length)
            {
                result.Add((int[])permutation.Clone());
            }
            else
            {
                for (int i = 0; i < permutation.Length; i++)
                {
                    var index = Array.IndexOf(permutation, i, 0, position);
                    if (index == -1)
                    {
                        permutation[position] = i;
                        MakePermutations(permutation, position + 1, result);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            //n1
            //WriteReversed(new []{'a','b','c'});

            //n2
            //WriteAllWordsOfSize(1);
            //WriteAllWordsOfSize(2);
            //WriteAllWordsOfSize(0);
            //WriteAllWordsOfSize(4);

            //n3
            //TestOnSize(1);
            //TestOnSize(2);
            //TestOnSize(0);
            //TestOnSize(3);
            //TestOnSize(4);

            //n4
            CaseAlternatorTask.AlternateCharCases("straße");

        }
    }
}
