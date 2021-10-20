using System;
using System.Threading;

namespace CustomThreadPoolTask.ThreadPools
{
    public class DotNetThreadPoolWrapper : IThreadPool
    {
        private long processedTask;

        public void EnqueueAction(Action action)
        {
            ThreadPool.UnsafeQueueUserWorkItem(delegate
            {
                action.Invoke();
                Interlocked.Increment(ref processedTask);
            }, null);
        }

        public long GetTasksProcessedCount() => processedTask;
    }
}