using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataParallelismTask.LogParsers
{
    public class ParallelLogParser : ILogParser
    {
        private readonly FileInfo file;
        private readonly Func<string, string?> tryGetIdFromLine;

        public ParallelLogParser(FileInfo file, Func<string, string?> tryGetIdFromLine)
        {
            this.file = file;
            this.tryGetIdFromLine = tryGetIdFromLine;
        }

        public string[] GetRequestedIdsFromLogFile()
        {
            var lines = File.ReadLines(file.FullName);

            //Если порядок не важен
            var result = new ConcurrentQueue<string>();
            Parallel.ForEach(lines,
                str =>
                    result.Enqueue(tryGetIdFromLine(str)));
            return result.Where(x => x != null).ToArray();

            //Если порядок важен
            //var result = new ConcurrentDictionary<long,string>();
            //Parallel.ForEach(lines, ((str, state, index) =>
            //    result[index] = tryGetIdFromLine(str)));
            //return result
            //    .OrderBy(x => x.Key)
            //    .Where(x => x.Value != null)
            //    .Select(x => x.Value)
            //    .ToArray();
        }
    }
}