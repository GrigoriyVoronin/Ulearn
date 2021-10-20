#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DataParallelismTask.LogParsers;

namespace DataParallelismTask
{
    public class Program
    {
        private static readonly Regex IdLineRegex = new Regex(@"\d{4}-\d{2}-\d{2}.+GetMessages\?formId=(?<id>\S+)");

        public static void Main(string[] args)
        {
            var file = new FileInfo("Files/bigLog.log");

            var sequential = RunParser(new SequentialLogParser(file, TryGetIdFromLine));
            var threads = RunParser(new ThreadLogParser(file, TryGetIdFromLine));
            var parallel = RunParser(new ParallelLogParser(file, TryGetIdFromLine));
            var plinq = RunParser(new PLinqLogParser(file, TryGetIdFromLine));

            CheckAreSame(sequential, threads, nameof(sequential), nameof(threads));
            CheckAreSame(sequential, parallel, nameof(sequential), nameof(parallel));
            CheckAreSame(sequential, plinq, nameof(sequential), nameof(plinq));
        }

        private static void CheckAreSame(string[] first, string[] second, string firstName, string secondName)
        {
            if (first.Length != second.Length)
            {
                Console.WriteLine(
                    $"Results are different: {firstName} and {secondName}: {first.Length} and {second.Length}");
                return;
            }

            var isSame = first
                .OrderBy(l => l)
                .Zip(second.OrderBy(l => l), (a, b) => a == b)
                .All(eq => eq);

            if (!isSame)
                Console.WriteLine($"Results are different: {firstName} and {secondName}");
        }

        private static string[] RunParser(ILogParser logParser)
        {
            var sw = Stopwatch.StartNew();
            var result = logParser.GetRequestedIdsFromLogFile();
            Console.WriteLine($"{logParser.GetType().Name}:\t\t{sw.Elapsed.TotalMilliseconds}");
            return result;
        }

        private static string? TryGetIdFromLine(string line)
        {
            var match = IdLineRegex.Match(line);
            return match.Success ? match.Groups["id"].Value : null;
        }
    }
}