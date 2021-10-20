#nullable enable
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;

namespace DataParallelismTask.LogParsers
{
    public class ThreadLogParser : ILogParser
    {
        private readonly FileInfo file;
        private readonly Func<string, string?> tryGetIdFromLine;

        public ThreadLogParser(FileInfo file, Func<string, string?> tryGetIdFromLine)
        {
            this.file = file;
            this.tryGetIdFromLine = tryGetIdFromLine;
        }

        public string[] GetRequestedIdsFromLogFile()
        {
            const int tCount = 10;
            var lines = File.ReadLines(file.FullName);
            var result = new ConcurrentQueue<string>();
            var threadsBuffers = new ConcurrentQueue<string>[tCount];
            var isReady = false;
            var threads = new Thread[tCount];
            for (var i = 0; i < tCount; i++)
            {
                var j = i;
                threadsBuffers[i]=new ConcurrentQueue<string>();
                threads[i] = new Thread(() =>
                {
                    while (!isReady || threadsBuffers[j].Count > 0)
                        if (threadsBuffers[j].TryDequeue(out var str))
                            result.Enqueue(tryGetIdFromLine(str));
                }) {IsBackground = true};
                threads[i].Start();
            }

            var index = 0;
            foreach (var line in lines)
            {
                threadsBuffers[index++].Enqueue(line);
                if (index == tCount)
                    index = 0;
            }

            isReady = true;
            foreach (var thread in threads)
                thread.Join();
            return result.Where(x => x != null).ToArray();
        }
    }
}