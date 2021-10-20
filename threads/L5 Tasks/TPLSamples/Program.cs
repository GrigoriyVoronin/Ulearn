using System;
using System.Threading;
using System.Threading.Tasks;

namespace TPLSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Continuation.Parent();
            //Continuation.TaskStatusWhenContinueWith();

            // Exceptions.Flattening();

            // TaskWait();
        }

        public static void TaskWait()
        {
            SleepAndPrint("Hello")
                .ContinueWith(_ =>
                {
                    Console.Write(", ");
                    var task = SleepAndPrint("world");
                    return task;
                })
                .ContinueWith(prev => prev.Result.ContinueWith(_ => Console.WriteLine("!"), TaskContinuationOptions.AttachedToParent))
                .Wait();
        }

        private static async Task SleepAndPrint(string message)
        {
            await Task.Delay(1000);
            Console.Write(message);
        }
    }
}
