using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CustomThreadPoolTask.Collections;

namespace CustomThreadPoolTask.ThreadPools
{
    public class MyThreadPool : IThreadPool
    {
        private const int WorkersCount = 20;

        private readonly ConcurrentQueue<Action> mainQueue;

        private readonly ThreadLocal<WorkStealingQueue<Action>> tasks;

        private readonly object taskWaiter = new object();

        private readonly HashSet<MyWorker> workers;

        private long processedTask;

        public MyThreadPool()
        {
            tasks = new ThreadLocal<WorkStealingQueue<Action>>();
            workers = new HashSet<MyWorker>();
            mainQueue = new ConcurrentQueue<Action>();
            for (var i = 0; i < WorkersCount; i++)
                workers.Add(new MyWorker(this));
            foreach (var worker in workers)
                worker.WorkingThread.Start();
        }

        public void EnqueueAction(Action action)
        {
            if (tasks.Value == null)
                mainQueue.Enqueue(() =>
                {
                    action.Invoke();
                    Interlocked.Increment(ref processedTask);
                });
            else
                tasks.Value.LocalPush(() =>
                {
                    action.Invoke();
                    Interlocked.Increment(ref processedTask);
                });
            lock (taskWaiter)
            {
                Monitor.Pulse(taskWaiter);
            }
        }

        public long GetTasksProcessedCount() => processedTask;

        private sealed class MyWorker
        {
            private readonly MyThreadPool owner;
            public readonly Thread WorkingThread;

            private WorkStealingQueue<Action> localQueue;

            public MyWorker(MyThreadPool owner)
            {
                this.owner = owner;
                WorkingThread = new Thread(DoWork) {IsBackground = true};
            }

            private void DoWork()
            {
                localQueue = owner.tasks.Value = new WorkStealingQueue<Action>();
                Action action = null;
                while (true)
                {
                    while (localQueue.LocalPop(ref action))
                        action.Invoke();

                    if (owner.mainQueue.TryDequeue(out action)
                        || owner.workers.Any(x => x.localQueue?.TrySteal(ref action) == true))
                    {
                        action.Invoke();
                        continue;
                    }

                    lock (owner.taskWaiter)
                    {
                        Monitor.Wait(owner.taskWaiter);
                    }
                }
            }
        }
    }
}