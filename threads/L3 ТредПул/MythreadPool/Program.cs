using System;
using System.Threading;

namespace CustomThreadPoolTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n----------=======DotNet ThreadPool tests=======----------", ConsoleColor.DarkCyan);
            new DotNetThreadPoolWrapperTests().RunTests();
            Console.WriteLine();
            Console.WriteLine("\n----------=======My ThreadPool tests=======----------", ConsoleColor.DarkMagenta);
            new MyThreadPoolTests().RunTests();
            Console.WriteLine();
        }
    }
}