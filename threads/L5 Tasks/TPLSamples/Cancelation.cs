using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TPLSamples
{
    public static class Cancelation
    {
        public static void CancelationExample()
        {
            var cts = new CancellationTokenSource();
            var task = new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);

					// if (cts.Token.IsCancellationRequested)
					// 	throw new OperationCanceledException(cts.Token);

					cts.Token.ThrowIfCancellationRequested();
				}
            }, cts.Token);

			Console.WriteLine($"Before start: {task.Status}");
			cts.Cancel();
			task.Start();
            Console.WriteLine($"After start: {task.Status}");
            // cts.Cancel();
            Thread.Sleep(1000);
            Console.WriteLine($"After 100 ms: {task.Status}");
            // cts.Cancel();
            Console.WriteLine($"After cancel: {task.Status}");
            try
            {
                task.Wait();
                Console.WriteLine($"After wait: {task.Status}.");
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"After wait: {task.Status}. Ex={ex.InnerExceptions.Single().GetType().Name}");
            }
        }
    }
}