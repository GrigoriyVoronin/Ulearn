using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataParallelism
{
    public static class ProgramPlinq
    {
        private static IEnumerable<int> GetNumbers(int count) => Enumerable.Range(0, count);

        private static int Work(int n)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}\t{n}");
            return n;
        }
        
        public static void Demo()
        {
            SimpleDemo1();
            SimpleDemo2();
            SimpleDemo3();
            PartitioningDemo1();
        }

        private static void SimpleDemo1()
        {
            var arr = GetNumbers(50)
                .AsParallel()
                // .AsUnordered()
                // .AsOrdered() //Не гарантирует порядок выполнения, но гарантирует порядок результата
                .Select(Work)
                .ToArray();
            
            Console.WriteLine(string.Join(" ", arr));
        }

        private static void SimpleDemo2()
        {
            GetNumbers(10)
                .AsParallel()
                // .WithCancellation(new CancellationTokenSource().Token)
                // .WithDegreeOfParallelism(10)
                // .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(Work)
                // .ForAll(n => Work(n));
                .AsSequential()
                .Select(Work)
                .ToArray();
        }

        private static void SimpleDemo3()
        {
            try
            {
                GetNumbers(10)
                    .AsParallel()
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .ForAll(n =>
                    {
                        if (n >= 2)
                            throw new InvalidCastException(n.ToString());
                        throw new ArgumentException(n.ToString());
                    });
            }
            catch (AggregateException e)
            {
                
            }
        }

        private static void PartitioningDemo1()
        {
            var partitioner = Partitioner.Create(GetNumbers(12));

            // partitioner
            //     .AsParallel()
            //     ...;
        }

        public class MyPartitioner : Partitioner<int>
        {
            private readonly IEnumerable<int> foo;

            public MyPartitioner(IEnumerable<int> foo)
            {
                this.foo = foo;
            }
            
            public override IList<IEnumerator<int>> GetPartitions(int partitionCount)
            {
                
            }
        }
    }
}