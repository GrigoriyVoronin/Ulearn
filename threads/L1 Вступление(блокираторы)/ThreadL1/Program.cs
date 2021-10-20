using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ThreadL1
{
    internal class Program
    {
        private static readonly Stopwatch Timer = new Stopwatch();
        private static int curThreadId;
        private static readonly List<double> Data = new List<double>();

        public static void Main(string[] args)
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr) (2);
            var tCount = 4;
            var myThreads = new Thread[tCount];
            for (int i = 0; i < tCount; i++)
                myThreads[i] = new Thread(MeasureTime);

            for (int i = 0; i < tCount; i++)
                myThreads[i].Start();

            while (true)
                Console.WriteLine(Math.Round(Data.Sum()/ Data.Count,2));
        }

        public static void MeasureTime()
        {
            while (true)
            {
                if (Interlocked.Exchange(ref curThreadId,  Thread.CurrentThread.ManagedThreadId) != curThreadId)
                lock (Data)
                {
                    Data.Add(Timer.Elapsed.TotalMilliseconds);
                    Timer.Restart();
                    curThreadId = Thread.CurrentThread.ManagedThreadId;
                }
            }
        }
    }
}