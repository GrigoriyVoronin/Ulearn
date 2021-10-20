using System;
using System.IO;
using System.Linq;

namespace DataParallelismTask.LogParsers
{
    public class SequentialLogParser : ILogParser
    {
        private readonly FileInfo file;
        private readonly Func<string, string?> tryGetIdFromLine;

        public SequentialLogParser(FileInfo file, Func<string, string?> tryGetIdFromLine)
        {
            this.file = file;
            this.tryGetIdFromLine = tryGetIdFromLine;
        }

        public string[] GetRequestedIdsFromLogFile()
        {
            var lines = File.ReadLines(file.FullName);
            return lines
                .Select(tryGetIdFromLine)
                .Where(id => id != null)
                .ToArray();
        }
    }
}