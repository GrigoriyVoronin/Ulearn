using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataParallelism
{
    public static class ProgramParallel
    {
        private static IEnumerable<int> Numbers => Enumerable.Range(0, 1_000_000);

        private static void Work(int num)
        {
            Thread.SpinWait(num % 100);
            if (num % 100_000 == 42)
                Console.WriteLine(num);
        }
        
        public static void Demo()
        {
            ParallelInvokeDemo1();
            ParallelInvokeDemo2();
            ParallelForDemo1();
            ParallelForDemo2();
            ParallelForDemo3();
            ParallelForeachDemo1();
            ParallelForeachDemo2();
        }

        private static void ParallelInvokeDemo1()
        {
            var tasks = Numbers.Select(n => (Action)(() => Work(n))).ToArray();
            Parallel.Invoke(new ParallelOptions{MaxDegreeOfParallelism = 10}, tasks);
        }

        private static void ParallelInvokeDemo2()
        {
            var tasks = Numbers.Select(n => (Action)(() =>
            {
                Work(n);
                
                if (n > 500_000)
                    throw new Exception("Oops");
            })).ToArray();
            
            try
            {
                Parallel.Invoke(new ParallelOptions{MaxDegreeOfParallelism = 10}, tasks);
            }
            catch (AggregateException e)
            {
                Console.WriteLine(string.Join("\n", e.InnerExceptions.Select(e => e.Message)));
            }
        }

        private static void ParallelForDemo1()
        {
            Parallel.For(0, 1_000_000, (n, state) => Work(n));
        }

        private static void ParallelForDemo2()
        {
            var result = Parallel.For(0, 1_000_000, (n, state) =>
            {
                if (n > 500_000)
                {
                    // state.Break();
                    state.Stop();
                }

                Work(n);
            });
            
            Console.WriteLine($"{result.IsCompleted}");
        }

        private static void ParallelForDemo3()
        {
            Parallel.For(0, 1_000_000, (n, state) =>
            {
                Work(n);
                
                if (n == 200_000)
                    throw new Exception("Oops");
                
                if (state.IsExceptional)
                    Console.WriteLine("I am doomed :(");
            });
        }

        private static void ParallelForeachDemo1()
        {
            var result = Parallel.ForEach(Numbers, new ParallelOptions{MaxDegreeOfParallelism = 32}, Work);
            Console.WriteLine($"{result.IsCompleted}");
        }

        private static void ParallelForeachDemo2()
        {
            var result = Parallel.ForEach(Numbers, (n, state) =>
            {
                if (n > 500_000)
                {
                    state.Break();
                }
                
                Work(n);
            });
            
            Console.WriteLine($"{result.IsCompleted}");
        }
    }
}