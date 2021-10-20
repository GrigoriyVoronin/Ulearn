using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Introduction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ThreadAndProcessDemo();

            // MonitorDemo();
            // SemaphoreDemo();
            // AutoResetEventDemo();
            // RwLockSlimDemo();

            // InterlockedDemo();

            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr) 1;
        }

        private static void ThreadAndProcessDemo()
        {
            var thread = new Thread(() => Console.WriteLine("Hello from another thread")) {IsBackground = true};
            thread.Start();
            thread.Join();

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\tree.com", 
                    RedirectStandardOutput = true, 
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
            Console.WriteLine("Output from another process:");
            Console.WriteLine(process.StandardOutput.ReadToEnd());
        }

        private static void MonitorDemo()
        {
            var lockObject = new object();

            var firstThread = new Thread(() =>
            {
                while (true)
                {
                    var lockTaken = false;
                    
                    // lock (lockObject)
                    {
                        try
                        {
                            Monitor.TryEnter(lockObject, ref lockTaken);
                            
                            Console.Write("{");
                            Thread.SpinWait(1_000_000);
                            Console.Write("}");
                        }
                        finally
                        {
                            if (lockTaken)
                                Monitor.Exit(lockObject);
                        }                        
                    }
                }
            }) {IsBackground = true};
            
            var secondThread = new Thread(() =>
            {
                while (true)
                {
                    // lock (lockObject)
                    {
                        Monitor.Enter(lockObject);
                        Console.Write("(");
                        Thread.SpinWait(1_000_000);
                        Console.Write(")");
                        Monitor.Exit(lockObject);
                    }
                }
            }) {IsBackground = true};
            
            firstThread.Start();
            secondThread.Start();

            firstThread.Join();
            secondThread.Join();
        }
        
        private static void SemaphoreDemo()
        {
            var semaphore = new SemaphoreSlim(5, 5);
            var threadsCount = 0;

            void ThreadAction()
            {
                while (true)
                {
                    semaphore.Wait();
                    Interlocked.Increment(ref threadsCount);
                    semaphore.Release();
                    Interlocked.Decrement(ref threadsCount);
                }
            }

            RunThreads(ThreadAction, 10);

            while (true)
            {
                Console.WriteLine(threadsCount);
                Thread.Sleep(500);
            }
        }

        private static void AutoResetEventDemo()
        {
            var are = new AutoResetEvent(false);

            void ThreadAction()
            {
                are.WaitOne();
                Console.WriteLine($"Hello from thread {Thread.CurrentThread.ManagedThreadId}");
            }
            
            RunThreads(ThreadAction, 10);

            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Open gate for next thread");
                are.Set();
            }
        }

        private static void RwLockSlimDemo()
        {
            var value = 0;
            var rwls = new ReaderWriterLockSlim();

            void ReaderThreadAction()
            {
                while (true)
                {
                    rwls.EnterReadLock();
                    
                    Console.WriteLine($"I read value {value}. Thread #{Thread.CurrentThread.ManagedThreadId}");
                    
                    rwls.ExitReadLock();
                }
            }

            void WriterThreadAction()
            {
                while (true)
                {
                    rwls.EnterWriteLock();
                    
                    Console.WriteLine($"Begin write. Current value {value}. Thread #{Thread.CurrentThread.ManagedThreadId}");
                    value = value + Thread.CurrentThread.ManagedThreadId;
                    Thread.SpinWait(100_000);
                    Console.WriteLine($"End write. Current value {value}. Thread #{Thread.CurrentThread.ManagedThreadId}");
                    
                    rwls.ExitWriteLock();
                    
                    Thread.SpinWait(1_000_000);
                }
            }
            
            RunThreads(ReaderThreadAction, 10);
            RunThreads(WriterThreadAction, 3);
            
            Thread.Sleep(500);
        }

        private static void InterlockedDemo()
        {
            var value = 0;

            void ThreadAction()
            {
                while (true)
                {
                    // var oldValue = Interlocked.Increment(ref value);

                    // var oldValue = Interlocked.Add(ref value, 10);

                    int CmpExch(ref int variable, int newValue, int oldValue)
                    {
                        if (variable != oldValue)
                            return oldValue;

                        variable = newValue;
                        return oldValue;
                    }
                    
                    // var oldValue = value;
                    // if (Interlocked.CompareExchange(ref value, oldValue + 1, oldValue) != oldValue)
                    // {
                    //     Console.WriteLine($"!!! Can't update value. Thread #{Thread.CurrentThread.ManagedThreadId}");
                    //     continue;
                    // }

                    var oldValue = Interlocked.Exchange(ref value, 42);
                    
                    Console.WriteLine($"Change value. Old value {oldValue}. Thread #{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(100);
                }
            }
            
            RunThreads(ThreadAction, 10);
            
            Thread.Sleep(-1);
        }
        
        private static void RunThreads(Action action, int count)
        {
            for (var i = 0; i < count; i++) 
                new Thread(() => action()) {IsBackground = true}.Start();
        }
    }
}