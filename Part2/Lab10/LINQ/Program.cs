using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LINQ
{
    class Program
    {
        public class Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X, Y;
        }

        public class Classroom
        {
            public List<string> Students = new List<string>();
        }

        public class Document
        {
            public int Id;
            public string Text;
        }

        private static void Main(string[] args)
        {
            //1
            foreach (var num in ParseNumbers(new[] { "-0", "+0000" }))
                Console.WriteLine(num);
            foreach (var num in ParseNumbers(new List<string> { "1", "", "-03", "0" }))
                Console.WriteLine(num);
            //2
            foreach (var point in ParsePoints(new[] { "1 -2", "-3 4", "0 2" }))
                Console.WriteLine(point.X + " " + point.Y);
            foreach (var point in ParsePoints(new List<string> { "+01 -0042" }))
                Console.WriteLine(point.X + " " + point.Y);
            //3
            Classroom[] classes =
            {
                new Classroom {Students = {"Pavel", "Ivan", "Petr"},},
                new Classroom {Students = {"Anna", "Ilya", "Vladimir"},},
                new Classroom {Students = {"Bulat", "Alex", "Galina"},}
            };
            var allStudents = GetAllStudents(classes);
            Array.Sort(allStudents);
            Console.WriteLine(string.Join(" ", allStudents));
            //5
            var vocabulary = GetSortedWords(
                "Hello, hello, hello, how low",
                "",
                "With the lights out, it's less dangerous",
                "Here we are now; entertain us",
                "I feel stupid and contagious",
                "Here we are now; entertain us",
                "A mulatto, an albino, a mosquito, my libido...",
                "Yeah, hey");
            foreach (var word in vocabulary)
                Console.WriteLine(word);
            //7
            Console.WriteLine(GetLongest(new[] { "azaz", "as", "sdsd" }));
            Console.WriteLine(GetLongest(new[] { "zzzz", "as", "sdsd" }));
            Console.WriteLine(GetLongest(new[] { "as", "12345", "as", "sds" }));
            //9
            Document[] documents =
            {
                new Document {Id = 1, Text = "Hello world!"},
                new Document {Id = 2, Text = "World, world, world... Just words..."},
                new Document {Id = 3, Text = "Words — power"},
                new Document {Id = 4, Text = ""}
            };
            var index = BuildInvertedIndex(documents);
            SearchQuery("world", index);
            SearchQuery("words", index);
            SearchQuery("power", index);
            SearchQuery("cthulhu", index);
            SearchQuery("", index);
        }

        //1 Чтение массива чисел
        public static int[] ParseNumbers(IEnumerable<string> lines)
        {
            return lines
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => int.Parse(l))
                .ToArray();
        }
        //2 Чтение списка чисел
        public static List<Point> ParsePoints(IEnumerable<string> lines)
        {
            return lines
                .Select(p => p.Split()
                    .Select(x => int.Parse(x))
                    .ToArray())
                .Select(x => new Point(x[0], x[1]))
                .ToList();
        }
        //3 Объеденение коллекций
        public static string[] GetAllStudents(Classroom[] classes)
        {
            return classes
                .SelectMany(x => x.Students)
                .ToArray();
        }
        //4 Декартово произведение
        public static IEnumerable<Point> GetNeighbours(Point p)
        {
            int[] d = { -1, 0, 1 };
            return d.SelectMany(x => d
            .Select(y => new Point(p.X + x, p.Y + y))
            .Where(x => !x.Equals(p)));
        }
        //5 Составление словаря
        public static string[] GetSortedWords(params string[] textLines)
        {
            return textLines
                .SelectMany(x => Regex.Split(x.ToLower(), @"\W+"))
                .Distinct()
                .OrderBy(x => x)
                .ToArray();
        }
        //6 Сортировка кортежей
        public static List<string> GetSortedWords(string text)
        {
            return Regex.Split(text.ToLower(), @"\W+")
                .OrderBy(x => (x.Length, x))
                .Distinct()
                .Where(x => x != "")
                .ToList();
        }
        //7 Поиск самого длинного слова
        public static string GetLongest(IEnumerable<string> words)
        {
            return words
                .Min(x => (-x.Length, x))
                .x;
        }
        //8 Создание частотного словаря
        public static (string, int)[] GetMostFrequentWords(string text, int count)
        {
            return Regex.Split(text.ToLower(), @"\W+")
                .Where(word => word != "")
                .GroupBy(x => x)
                .OrderBy(x => (-x.Count(), x.Key))
                .Take(count)
                .Select(x => (x.Key, x.Count()))
                .ToArray();
        }
        //9 Создание обратного индекса
        public static ILookup<string, int> BuildInvertedIndex(Document[] documents)
        {
            return documents
            .Select(x => (Regex.Split(x.Text.ToLower(), @"\W+")
                          .Where(word => word != "")
                          .Distinct(), x.Id))
            .SelectMany(x => x.Item1
                        .Select(y => (y, x.Id)))
            .ToLookup(x => x.y, x => x.Id);
        }
        private static void SearchQuery(string v, ILookup<string, int> index)
        {
            Console.WriteLine($"SearchQuery({v}) found documents: " + string.Join(", ", index[v]));
        }
    }
}
