using System;

namespace Expr68
{
    class Program
    {
        public static double LenghtOfSegment (string [] startCoordinates, string [] endCoordinates)
        {
            double x1 = double.Parse(startCoordinates[0]);
            double y1 = double.Parse(startCoordinates[1]);
            double x2 = double.Parse(endCoordinates[0]);
            double y2 = double.Parse(endCoordinates[1]);
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
        static void Main(string[] args)
        {
            //Expr6
            //Console.WriteLine("Введите коардинаты первой точки прямой:");
            //var a = Console.ReadLine().Split();
            //Console.WriteLine("Введите коардинаты второй точки прямой:");
            //var b = Console.ReadLine().Split();
            //Console.WriteLine("Введите коардинаты точки от которой нужно найти расстояние:");
            //var c = Console.ReadLine().Split();
            //double ac = LenghtOfSegment(a, c);
            //double bc = LenghtOfSegment(b, c);
            //double ab = LenghtOfSegment(a, b);
            //double halfOfPerimeter = 0.5 * (ac + bc + ab);
            //double lenght = (2 * Math.Sqrt(halfOfPerimeter * (halfOfPerimeter - ac) * (halfOfPerimeter - ab) * (halfOfPerimeter - bc))) / ab;
            //Console.Write("Расстояние от точки до прямой = ");
            //Console.Write(lenght);
            //Expr7
            //Console.WriteLine("Введите коэфинцеты прямой a и b, заданной уравнением y=ax+b,через пробел: ");
            //var userInput = Console.ReadLine().Split();
            //Console.Write("Коардинаты вектора параллельного прямой: ");
            //Console.Write("1"+" "+userInput[0]);
            //Console.WriteLine("Коардинаты вектора перпендекялрного прямой: ");
            //double x = -1/double.Parse(userInput[0]);
            //Console.Write(x.ToString()+" "+"1");
            //Expr8
            Console.WriteLine("Введите коэфинцеты прямой a и b, заданной уравнением y=ax+b,через пробел: ");
            var directСoefficients = Console.ReadLine().Split();
            double[] coordinatesOfNormalVector = new double[2];
            coordinatesOfNormalVector[0] = -1 / double.Parse(directСoefficients[0]);
            coordinatesOfNormalVector[1] = 1;
            Console.WriteLine("Введите коардинаты точки через которую проходит перпендекулярная прямая:");
            var a = Console.ReadLine().Split();
            double coefficientA = coordinatesOfNormalVector[0];
            double coefficientB = coordinatesOfNormalVector[0] * -1*double.Parse(a[0]) +  double.Parse(a[1]);
            Console.WriteLine("Коэфицент a = "+coefficientA.ToString());
            Console.WriteLine("Коэфицент b = "+coefficientB.ToString());
        }
    }
}
