using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace TPLSamples
{
    public static class CreationAndWaiting
    {
        public static void QueueUserWorkItem()
        {
            ThreadPool.QueueUserWorkItem(state =>
                                         {
                                             Console.WriteLine("Starting...");
                                             Thread.Sleep(1000);
                                             Console.WriteLine("... finished!");
                                         });
        }

        public static void QueueUserWorkItemWaitingToFinish()
        {
            var methodFinishedEvent = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(state =>
                                         {
                                             Console.WriteLine("Starting...");
                                             Thread.Sleep(1000);
                                             Console.WriteLine("... finished!");
                                             methodFinishedEvent.Set();
                                         });
            methodFinishedEvent.WaitOne(); 
        }

        public static void TaskWaitingToFinish()
        {
            var task = new Task(() =>
                                {
                                    Console.WriteLine("Starting...");
                                    Thread.Sleep(1000);
                                    Console.WriteLine("... finished!");
                                });
            task.Start();
            task.Wait();
        }

        public static void Statuses()
        {
	        var task = new Task(() =>
	        {
		        Console.WriteLine("Starting...");
		        Thread.Sleep(1000);
		        Console.WriteLine("... finished!");
		        // throw new Exception();
	        });

	        Console.WriteLine("Task1 status: {0}", task.Status);
	        task.Start();
	        Console.WriteLine("Task1 status: {0}", task.Status);
	        Thread.Sleep(500);
	        Console.WriteLine("Task1 status: {0}", task.Status);
	        // task.Wait();
            Thread.Sleep(1000);
	        Console.WriteLine("Task1 status: {0} {1}", task.Status, task.IsCompleted && !task.IsFaulted);
        }

		public static void TaskRun()
        {
            Action action = () =>
                            {
                                Console.WriteLine("Starting...");
                                Thread.Sleep(1000);
                                Console.WriteLine("... finished!");
                            };

            var task = Task.Run(action);
            task.Wait();

            //the code above is equal to

            task = Task.Factory.StartNew(action, TaskCreationOptions.DenyChildAttach);
            task.Wait();
        }
        
        public static void ParametrizedTask()
        {
            var task = new Task<int>(() =>
            {
                Console.WriteLine("1");
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 42;
            });
            task.Start();
            Console.WriteLine("Before");
            Console.WriteLine(task.Result);
            Console.WriteLine("After");
        }

        public static void TaskFromResult()
        {
            Task<int> task = Task.FromResult(42);
            Console.WriteLine(task.Result);
        }

        public static void WaitAllWaitAny()
        {
            var firstTask = Task.Run(() =>
                                     {
                                         Console.WriteLine("Task 0 starting...");
                                         Thread.SpinWait(10000000);
                                         Console.WriteLine("Task 0 finishing...");
                                     });
            var secondTask = Task.Run(() =>
                                      {
                                          Console.WriteLine("Task 1 starting...");
                                          Thread.SpinWait(1000000000);
                                          Console.WriteLine("Task 1 finishing...");
                                      });

            int finishedTaskIndex = Task.WaitAny(firstTask, secondTask);
            Console.WriteLine("Task {0} finished", finishedTaskIndex);
 
            Task.WaitAll(firstTask, secondTask);
            Console.WriteLine("All tasks finished");
        }

        public static void WhenAllWhenAny()
        {
            Task.Run(() =>  Thread.SpinWait(1000000000));
            var firstTask = Task.Run(() =>
                                     {
                                         Console.WriteLine("Task 0 starting...");
                                         Thread.SpinWait(10000000);
                                         Console.WriteLine("Task 0 finishing...");
                                         return 0;
                                     });
            var secondTask = Task.Run(() =>
                                      {
                                          Console.WriteLine("Task 1 starting...");
                                          Thread.SpinWait(1000000000);
                                          Console.WriteLine("Task 1 finishing...");
                                          return 1;
                                      });
            
            Task<Task<int>> whenAnyTask = Task.WhenAny(firstTask, secondTask);
            whenAnyTask.Wait();
            Console.WriteLine("Task {0} finished", whenAnyTask.Result == firstTask ? "0" : "1");

            Task<int[]> aggregationTask = Task.WhenAll(firstTask, secondTask);
            aggregationTask.Wait();
            Console.WriteLine("All tasks finished: " + string.Join(", ", aggregationTask.Result));
        }
    }
}