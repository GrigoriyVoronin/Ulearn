using System;

namespace lab2
{
    class Program
    {
        public static bool IsLeapYear(int year)
        {
            return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
        }
        public static void TestMove(string from, string to)
        {
            Console.WriteLine("{0}-{1} {2}", from, to, IsCorrectMove(from, to));
        }
        public static bool IsCorrectMove(string from, string to)
        {
            var dx = Math.Abs(to[0] - from[0]); 
            var dy = Math.Abs(to[1] - from[1]); 
            return ((dx == dy) && dx != 0) || (dx == 0 && dy != 0) || (dx != 0 && dy == 0);
        }
        public static int MiddleOf(int a, int b, int c)
        {
            return a + b + c - Math.Min(Math.Min(a, b), c) - Math.Max(Math.Max(a, b), c);
        }
        static void Main(string[] args)
        {
            //n1
            //Console.WriteLine(IsLeapYear(2014));
            //Console.WriteLine(IsLeapYear(1999));
            //Console.WriteLine(IsLeapYear(8992));
            //Console.WriteLine(IsLeapYear(1));
            //Console.WriteLine(IsLeapYear(14));
            //Console.WriteLine(IsLeapYear(400));
            //Console.WriteLine(IsLeapYear(600));
            //Console.WriteLine(IsLeapYear(3200));
            //n2
            //TestMove("a1", "d4");
            //TestMove("f4", "e7");
            //TestMove("a1", "a4");
            //n3
            //Console.WriteLine(MiddleOf(5, 0, 100)); // => 5
            //Console.WriteLine(MiddleOf(12, 12, 11)); // => 12
            //Console.WriteLine(MiddleOf(1, 1, 1)); // => 1
            //Console.WriteLine(MiddleOf(2, 3, 2));
            //Console.WriteLine(MiddleOf(8, 8, 8));
            //Console.WriteLine(MiddleOf(5, 0, 1));
            //n4
            //static bool ShouldFire2(bool enemyInFront, string enemyName, int robotHealth)
            //{
            //    return enemyInFront && (enemyName != "boss" || robotHealth >= 50);
            //}
        }
    }
}
