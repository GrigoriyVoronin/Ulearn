using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLSamples
{
    public static class Continuation
    {
        public static void Parent()
        {
            //for (int i = 0; i < 100; i++)
                Console.WriteLine(".");
            
            var parent = Task.Factory
                .StartNew(() =>
                          {
                              Console.WriteLine("Outer task executing.");
                              var child = Task.Factory.StartNew(() =>
                                                    {
                                                        Console.WriteLine("Nested task executing.");
														Thread.Sleep(1000);
                                                        Console.WriteLine("Nested task completing.");
                                                    }, TaskCreationOptions.AttachedToParent);
                              //Task.WaitAll(child);
                              //child.Wait();
                          }/*, TaskCreationOptions.DenyChildAttach*/);
            parent.Wait();
            //for (int i = 0; i < 500; i++)
            //{
            //    Thread.Sleep(10);
            //    Console.WriteLine(parent.Status);
            //}
            Console.WriteLine("Outer has completed.");
        }
        
        public static void ContinueWith()
        {
            for (int i = 0; i < 100; i++)
                Console.WriteLine(".");

            var firstTask = Task.Run(() =>
            {
                Console.WriteLine("Starting in thread #{0}", Thread.CurrentThread.ManagedThreadId);
                // throw new Exception();
            });

            var success = firstTask.ContinueWith(_ => { }, TaskContinuationOptions.OnlyOnRanToCompletion);
            var faulted = firstTask.ContinueWith(_ => { }, TaskContinuationOptions.OnlyOnFaulted);
            var canceled = firstTask.ContinueWith(_ => { }, TaskContinuationOptions.OnlyOnCanceled);
            
            var tasksChain = Task.Run(() =>
                {
                    Console.WriteLine("Starting in thread #{0}", Thread.CurrentThread.ManagedThreadId);
                    // throw new Exception();
                })
                .ContinueWith(previousTask =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Slept in thread #{0}", Thread.CurrentThread.ManagedThreadId);
                }, TaskContinuationOptions.OnlyOnFaulted);
                // .ContinueWith(previousTask =>
                // {
                //     Console.WriteLine("SleepingTask status is {0}", previousTask.Status);
                // });

                try
                {
                    tasksChain.Wait();
                }
                catch (Exception e)
                {
                }
            Console.WriteLine(tasksChain.Status);
        }
        
        public static void TaskStatusWhenContinueWith()
        {
            var task = Task.Run(() =>
                                {
                                    Console.WriteLine("Sleping in thread #{0}", Thread.CurrentThread.ManagedThreadId);
                                    Thread.Sleep(1000);
                                });

            var continuationTask = task.ContinueWith(previousTask => Console.WriteLine("Finished sleeping"));

            Console.WriteLine("ContinuationTask status is {0}", continuationTask.Status);
        }
    }
}