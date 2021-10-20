using System;

namespace CustomThreadPoolTask.ThreadPools
{
    public interface IThreadPool
    {
        void EnqueueAction(Action action);
        long GetTasksProcessedCount();
    }
}