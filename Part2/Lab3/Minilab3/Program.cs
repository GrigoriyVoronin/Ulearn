using System;
using System.Globalization;
//5
namespace ReadOnlyVectorTask
{
    public class ReadOnlyVector
    {
        public readonly double X;
        public readonly double Y;

        public ReadOnlyVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public ReadOnlyVector Add(ReadOnlyVector vec)
        {
            return new ReadOnlyVector(X + vec.X, Y + vec.Y);
        }

        public ReadOnlyVector WithX(double x)
        {
            return new ReadOnlyVector(x, Y);
        }

        public ReadOnlyVector WithY(double y)
        {
            return new ReadOnlyVector(X, y);
        }
    }
}

namespace Minilab3
{
    public class Ratio
    {
        public Ratio(int num, int den)
        {
            Numerator = num;
            Denominator = den > 0 ? den : throw new ArgumentException();
            Value = (double)Numerator / Denominator;
        }

        public readonly int Numerator; //числитель
        public readonly int Denominator; //знаменатель
        public readonly double Value; //значение дроби Numerator / Denominator
    }

    public class Student
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value ?? throw new ArgumentException(); }
        }
    }

    public class Vector
    {
        public double X;
        public double Y;
        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y);
            }
        }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}) with length: {2}", X, Y, Length);
        }
    }

    public class Book
    {
        public string Title { get; set; }
    }

    class Program
    {
        public static void Check2()
        {
            Vector vector = new Vector(3, 4);
            Console.WriteLine(vector.ToString());

            vector.X = 0;
            vector.Y = -1;
            Console.WriteLine(vector.ToString());

            vector = new Vector(9, 40);
            Console.WriteLine(vector.ToString());

            Console.WriteLine(new Vector(0, 0).ToString());
        }

        private static void WriteStudent()
        {
            var student = new Student { Name = "Vasya" };
            Console.WriteLine("student " + FormatStudent(student));
        }

        private static string FormatStudent(Student student)
        {
            return student.Name.ToUpper();
        }

        public static void Check1()
        {
            var book = new Book();
            book.Title = "Structure and interpretation of computer programs";
            Console.WriteLine(book.Title);
        }

        public static void Check3(int num, int den)
        {
            var ratio = new Ratio(num, den);
            Console.WriteLine("{0}/{1} = {2}",
                ratio.Numerator, ratio.Denominator,
                ratio.Value.ToString(CultureInfo.InvariantCulture));
        }

        static void Main(string[] args)
        {
            //1
            WriteStudent();
            //2
            Check1();
            //3
            Check2();
            //4
            Check3(1,2);
        }
    }
}
