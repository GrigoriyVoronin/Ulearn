using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdvancedConcurrency
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            // CompareAndSetDemo();
            // var stack = new ConcurrencyStackDemo<int>();
            // PulseWaitDemo();
            ThreadLocalDemo1();
            // ThreadLocalDemo2();
            // ThreadLocalDemo3();
        }

        private static void CompareAndSetDemo()
        {
            long CompareExchange(ref long variable, long newValue, long expectedValue)
            {
                if (variable != expectedValue) 
                    return variable;
                
                variable = newValue;
                return expectedValue;
            }

            long variable = 0;

            CompareExchange(ref variable, 2, 1);
            Interlocked.CompareExchange(ref variable, 2, 1);
        }
        
        private static void PulseWaitDemo()
        {
            var random = new Random(); // Not thread safe!
            var queue = new Queue<int>();
            
            void Worker()
            {
                while (true)
                {
                    // Monitor.Enter(queue);

                    var number = 0;
                    lock (queue)
                    {
                        if (!queue.Any())
                        {
                            Monitor.Wait(queue);
                        }

                        number = queue.Dequeue();
                    }
                    // Monitor.Exit(queue);
                    
                    Thread.SpinWait(1_000);
                    Console.WriteLine(number);
                }
            }
            new Thread(Worker) {IsBackground = false}.Start();

            while (true)
            {
                var sleepTime = random.Next(500, 2000);
                Thread.Sleep(sleepTime);
                
                Monitor.Enter(queue);
                queue.Enqueue(sleepTime);
                Monitor.Pulse(queue);
                Monitor.Exit(queue); // Только после этой строки Worker выйдет из метода Wait
            }
        }

        private static int Variable1 = 0;
        private static void ThreadLocalDemo1()
        {
            void FirstThread()
            {
                Variable1 = 1;
                while (true)
                {
                    Variable1 += 2;
                    Console.WriteLine($"NonEven:\t{Variable1}");
                    Thread.Sleep(500);
                }
            }
            
            void SecondThread()
            {
                Variable1 = 0;
                while (true)
                {
                    Variable1 += 2;
                    Console.WriteLine($"Even:\t{Variable1}");
                    Thread.Sleep(500);
                }
            }

            new Thread(FirstThread) {IsBackground = true}.Start();
            new Thread(SecondThread) {IsBackground = true}.Start();
            Thread.Sleep(-1);
        }
        
        private static ConcurrentDictionary<int, int> Variable2 = new ConcurrentDictionary<int, int>();
        private static void ThreadLocalDemo2()
        {
            void FirstThread()
            {
                while (true)
                {
                    var n = Variable2.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (_, old) => old + 2);
                    Console.WriteLine($"NonEven:\t{n}");
                    Thread.Sleep(500);
                }
            }
            
            void SecondThread()
            {
                while (true)
                {
                    var n = Variable2.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 2, (_, old) => old + 2);
                    Console.WriteLine($"Even:\t{n}");
                    Thread.Sleep(500);
                }
            }

            new Thread(FirstThread) {IsBackground = true}.Start();
            new Thread(SecondThread) {IsBackground = true}.Start();
            Thread.Sleep(-1);
        }
        
        private static ThreadLocal<int> Variable3 = new ThreadLocal<int>();
        private static void ThreadLocalDemo3()
        {
            void FirstThread()
            {
                Variable3.Value = -1;
                while (true)
                {
                    Variable3.Value += 2;
                    Console.WriteLine($"NonEven:\t{Variable3.Value}");
                    Thread.Sleep(500);
                }
            }
            
            void SecondThread()
            {
                Variable3.Value = 0;
                while (true)
                {
                    Variable3.Value += 2;
                    Console.WriteLine($"Even:\t{Variable3.Value}");
                    Thread.Sleep(500);
                }
            }

            new Thread(FirstThread) {IsBackground = true}.Start();
            new Thread(SecondThread) {IsBackground = true}.Start();
            Thread.Sleep(-1);
            
            Variable3.Dispose();
        }
        
        [ThreadStatic] private static int Variable4 = 0;
        private static void ThreadLocalDemo4()
        {
            void FirstThread()
            {
                Variable4 = -1;
                while (true)
                {
                    Variable4 += 2;
                    Console.WriteLine($"NonEven:\t{Variable4}");
                    Thread.Sleep(500);
                }
            }
            
            void SecondThread()
            {
                Variable4 = 0;
                while (true)
                {
                    Variable4 += 2;
                    Console.WriteLine($"Even:\t{Variable4}");
                    Thread.Sleep(500);
                }
            }

            new Thread(FirstThread) {IsBackground = true}.Start();
            new Thread(SecondThread) {IsBackground = true}.Start();
            Thread.Sleep(-1);

            MyRandom2.Random.Next();
        }


        public static class MyRandom2
        {
            public static readonly Random Random = new Random();
        } 
        
        
        
        
        
        public static class MyRandom
        {
            [ThreadStatic] private static Random random;

            public static Random Random => random = (random ?? new Random());
        }
    }

    public interface IConcurrentQueue<T>
    {
        void Enqueue(T value);
        bool TryDequeue(out T value);
        bool TryPeek(out T value);
        int Count { get; }
    }
}