using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minilab
{
    public class Point
    {
        public double X;
        public double Y;
        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
        }
    }

    //6
    public class ClockwiseComparer : IComparer
    {
        double CalculateDistanse(Point point)
        {
            var angle = Math.Atan2(point.Y, point.X);
            if (angle >= 0)
                return angle;
            else
                return Math.PI*2 + angle;
        }

        public int Compare(object x, object y)
        {
            var distX = CalculateDistanse((Point)x);
            var distY = CalculateDistanse((Point)y);
            return distX.CompareTo(distY);
        }
    }

    //5
    class Book : IComparable
    {
        public string Title;
        public int Theme;

        public int CompareTo(object obj)
        {
            return (Theme + Title).CompareTo(((Book)obj).Theme + ((Book)obj).Title);
        }
    }

    class Program
    {
        public static void Print(params object [] arguments)
        {
            for (var i = 0; i < arguments.Length;i++)
            {
                if (i > 0)
                    Console.Write(", ");
                Console.Write(arguments[i].ToString());
            }
            Console.WriteLine();
        }

        static void Print(Array array)
        {
            if (array == null)
            {
                Console.WriteLine("null");
                return;
            }
            for (int i = 0; i < array.Length; i++)
                Console.Write("{0} ", array.GetValue(i));
            Console.WriteLine();
        }

        static void Main()
        {
            //1
            Print(1, 2);
            Print("a", 'b');
            Print(1, "a");
            Print(true, "a", 1);
            //2
            var ints = new[] { 1, 2 };
            var strings = new[] { "A", "B" };

            Print(Combine(ints, ints));
            Print(Combine(ints, ints, ints));
            Print(Combine(ints));
            Print(Combine());
            Print(Combine(strings, strings));
            Print(Combine(ints, strings));
            //3
            Console.WriteLine(MiddleOfThree(2, 5, 4));
            Console.WriteLine(MiddleOfThree(3, 1, 2));
            Console.WriteLine(MiddleOfThree(3, 5, 9));
            Console.WriteLine(MiddleOfThree("B", "Z", "A"));
            Console.WriteLine(MiddleOfThree(3.45, 2.67, 3.12));
            //4
            Console.WriteLine(Min(new[] { 3, 6, 2, 4 }));
            Console.WriteLine(Min(new[] { "B", "A", "C", "D" }));
            Console.WriteLine(Min(new[] { '4', '2', '7' }));
            //6
            var array = new[]
                {
                    new Point { X = 1, Y = 0 },
                    new Point { X = -1, Y = 0 },
                    new Point { X = 0, Y = 1 },
                    new Point { X = 0, Y = -1 },
                    new Point { X = 0.01, Y = 1 }
                };
            Array.Sort(array, new ClockwiseComparer());
            foreach (Point e in array)
                Console.WriteLine("{0} {1}", e.X, e.Y);
            //7
            var triangle = new Triangle
            {
                A = new Point { X = 0, Y = 0 },
                B = new Point { X = 1, Y = 2 },
                C = new Point { X = 3, Y = 2 }
            };
            Console.WriteLine(triangle.ToString());
        }

        private static IComparable Min(Array arr)
        {
            IComparable min=(IComparable)arr.GetValue(0);
            for (int i=1; i<arr.Length;i++)
                if (min.CompareTo(arr.GetValue(i)) > 0)
                    min = (IComparable)arr.GetValue(i);
            return min;
        }

        static IComparable MiddleOfThree(IComparable a, IComparable b, IComparable c)
        {
            if (a.CompareTo(b) < 0)
            {
                if (b.CompareTo(c) < 0)
                    return b;
                else if (a.CompareTo(c) < 0)
                    return c;
                else
                    return a;
            }
            else
            {
                if (a.CompareTo(c) < 0)
                    return a;
                else if (b.CompareTo(c) < 0)
                    return c;
                else
                    return b;
            }
        }
        private static Array Combine(params Array[] arrays)
        {
            if (arrays.Length == 0) 
                return null;
            var isSame = true;
            for (int i = 1; i < arrays.Length; i++)
            {
                isSame &= arrays[i - 1].GetType().GetElementType() == arrays[i].GetType().GetElementType();
            }
            Type type;
            if (isSame)
                type = arrays[0].GetType().GetElementType();
            else
                return null;
            var len=0;
            for (int i = 0; i < arrays.Length; i++)
                len += arrays[i].Length;
            var arr = Array.CreateInstance(type, len);
            var index = 0;
            foreach(var arrSmall in arrays)
                foreach(var el in arrSmall)
                {
                    arr.SetValue(el, index);
                    index++;
                }
            return arr;
        }
    }

    internal class Triangle
    {
        public Point A, B, C;
        public override string ToString()
        {
            return string.Format("({0}) ({1}) ({2})", A, B, C);
        }
    }
}