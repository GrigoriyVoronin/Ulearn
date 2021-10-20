using System;
using System.Diagnostics;
using System.Threading;
using CustomThreadPoolTask.ThreadPools;

namespace CustomThreadPoolTask
{
    public class DotNetThreadPoolWrapperTests : ThreadPoolTests<DotNetThreadPoolWrapper>
    {
    }

    public class MyThreadPoolTests : ThreadPoolTests<MyThreadPool>
    {
    }

    public abstract class ThreadPoolTests<T> where T : IThreadPool, new()
    {
        private T threadPool;

        public void RunTests()
        {
            LongCalculations();
            ShortCalculations();
            ExtremelyShortCalculations();
            InnerShortCalculations();
            InnerExtremelyShortCalculations();
        }

        private void LongCalculations()
        {
            Console.Write("LongCalculations test: ");
            threadPool = new T();
            var timer = Stopwatch.StartNew();
            long enqueueMs;

            const int actionsCount = 1 * 1000;

            using (var cev = new CountdownEvent(actionsCount))
            {
                Action sumAction = () =>
                {
                    cev.Signal();
                    Thread.SpinWait(1000 * 1000);
                };
                for (var i = 0; i < actionsCount; i++)
                    threadPool.EnqueueAction(sumAction);
                enqueueMs = timer.ElapsedMilliseconds;
                cev.Wait();
            }

            timer.Stop();
            Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]",
                timer.ElapsedMilliseconds, enqueueMs, -1, threadPool.GetTasksProcessedCount());
        }

        private void ShortCalculations()
        {
            Console.Write("ShortCalculations test: ");
            threadPool = new T();
            var timer = Stopwatch.StartNew();
            long enqueueMs;
            //
            const int actionsCount = 1 * 1000*1000;

            using (var cev = new CountdownEvent(actionsCount))
            {
                Action sumAction = () =>
                {
                    cev.Signal();
                    Thread.SpinWait(1000);
                };
                for (var i = 0; i < actionsCount; i++)
                    threadPool.EnqueueAction(sumAction);
                enqueueMs = timer.ElapsedMilliseconds;
                cev.Wait();
            }

            timer.Stop();
            Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]",
                timer.ElapsedMilliseconds, enqueueMs, -1, threadPool.GetTasksProcessedCount());
        }

        private void ExtremelyShortCalculations()
        {
            Console.Write("ExtremelyShortCalculations test: ");
            threadPool = new T();
            var timer = Stopwatch.StartNew();
            long enqueueMs;

            const int actionsCount = 1 * 1000 * 1000;

            using (var cev = new CountdownEvent(actionsCount))
            {
                Action sumAction = () => { cev.Signal(); };
                for (var i = 0; i < actionsCount; i++) threadPool.EnqueueAction(sumAction);
                enqueueMs = timer.ElapsedMilliseconds;
                cev.Wait();
            }

            timer.Stop();
            Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]",
                timer.ElapsedMilliseconds, enqueueMs, -1, threadPool.GetTasksProcessedCount());
        }

        private void InnerShortCalculations()
        {
            Console.Write("InnerCalculations test: ");
            threadPool = new T();
            var timer = Stopwatch.StartNew();
            long enqueueMs;
            //1000
            const int actionsCount = 1 * 1000;
            const int subactionsCount = 1 * 1000;

            using (var outerEvent = new CountdownEvent(actionsCount))
            using (var innerEvent = new CountdownEvent(actionsCount * subactionsCount))
            {
                Action innerAction = () =>
                {
                    innerEvent.Signal();
                    Thread.SpinWait(1000);
                };
                Action outerAction = () =>
                {
                    for (var i = 0; i < subactionsCount; i++)
                        threadPool.EnqueueAction(innerAction);
                    outerEvent.Signal();
                };

                for (var i = 0; i < actionsCount; i++)
                    threadPool.EnqueueAction(outerAction);

                outerEvent.Wait();
                enqueueMs = timer.ElapsedMilliseconds;
                innerEvent.Wait();
            }

            timer.Stop();
            Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]",
                timer.ElapsedMilliseconds, enqueueMs, -1, threadPool.GetTasksProcessedCount());
        }

        private void InnerExtremelyShortCalculations()
        {
            Console.Write("InnerExtremelyShortCalculations test: ");
            threadPool = new T();
            var timer = Stopwatch.StartNew();
            long enqueueMs;

            const int actionsCount = 1 * 1000;
            const int subactionsCount = 1 * 1000;

            using (var outerEvent = new CountdownEvent(actionsCount))
            using (var innerEvent = new CountdownEvent(actionsCount * subactionsCount))
            {
                Action innerAction = () => { innerEvent.Signal(); };
                Action outerAction = () =>
                {
                    for (var i = 0; i < subactionsCount; i++) threadPool.EnqueueAction(innerAction);
                    outerEvent.Signal();
                };

                for (var i = 0; i < actionsCount; i++) threadPool.EnqueueAction(outerAction);

                outerEvent.Wait();
                enqueueMs = timer.ElapsedMilliseconds;
                innerEvent.Wait();
            }

            timer.Stop();
            Console.WriteLine(" total {0} ms, enqueue {1} ms, wasted {2} ms [tasks processed ~{3}]",
                timer.ElapsedMilliseconds, enqueueMs, -1, threadPool.GetTasksProcessedCount());
        }
    }
}