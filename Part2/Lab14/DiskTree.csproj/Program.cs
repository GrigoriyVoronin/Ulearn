using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var count = int.Parse(Console.ReadLine());
            var input = new string[count];
            for (var i = 0; i < count; i++)
                input[i] = Console.ReadLine();
            foreach (var dir in Solve(input))
                Console.WriteLine(dir);
        }

        public static IEnumerable<string> Solve(string[] input)
        {
            var main = new Node("main", -1);
            foreach (var directories in input
                .Select(path => path.Split('\\')))
                AddPathToTree(directories, main);

            return CreateDirectorysEnumerable(main, new List<string>());
        }

        private static List<string> CreateDirectorysEnumerable(Node main, List<string> directorys)
        {
            foreach (var directory in main.SubDirectories)
            {
                directorys.Add(new string(' ', directory.Level) + directory.Name);
                directorys = CreateDirectorysEnumerable(directory, directorys);
            }

            return directorys;
        }

        private static void AddPathToTree(string[] directories, Node main)
        {
            var parent = main;
            var currentNode = main;
            foreach (var directory in directories)
            {
                currentNode = currentNode.SubDirectories.FirstOrDefault(x => x.Name == directory);
                if (currentNode == null)
                {
                    var newNode = new Node(directory, parent.Level + 1);
                    parent.SubDirectories.Add(newNode);
                    currentNode = newNode;
                }

                parent = currentNode;
            }
        }

        private sealed class Node
        {
            public Node(string name, int level)
            {
                Name = name;
                Level = level;
            }

            public string Name { get; }

            public int Level { get; }

            public SortedSet<Node> SubDirectories { get; } = new SortedSet<Node>(new StrCompareOrd());

            public override string ToString() => Name;
        }

        private sealed class StrCompareOrd : IComparer<Node>
        {
            public int Compare(Node x, Node y) => string.CompareOrdinal(x.Name, y.Name);
        }
    }
}