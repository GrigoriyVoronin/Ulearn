using System;

namespace Expr1013
{
    class Program
    {
        static void Main()
        {
            //Expr10
            //int sum = 0;
            //for (int i = 999; i > 0; i--)
            //{
            //    if (i%5 == 0 || i%3 == 0)
            //    sum += i;
            //}
            //Console.WriteLine(sum);
            //Expr11
            //Console.WriteLine("Введите часы:");
            //int hours = int.Parse(Console.ReadLine());
            //Console.WriteLine("Введите минуты:");
            //int minutes = int.Parse(Console.ReadLine());
            //double hoursInMinutes = hours%12 * 5;
            //double distanceBetweenHoursAndMinutes = Math.Abs(hoursInMinutes - minutes);
            //double angleInDegrees = 0;
            //if (distanceBetweenHoursAndMinutes < 30)
            //    angleInDegrees = distanceBetweenHoursAndMinutes * 6;
            //else if (hoursInMinutes + 30 < minutes)
            //    angleInDegrees = (60 - minutes + hoursInMinutes) * 6;
            //else
            //    angleInDegrees = (60 - hoursInMinutes + minutes) * 6;
            //Console.WriteLine("Угол между стрелками в градусах ="+angleInDegrees);
            //Expr12
            //string userInput = Console.ReadLine();
            //var data = userInput.Split();
            //double h = double.Parse(data[0]);
            //double t = double.Parse(data[1]);
            //double v = double.Parse(data[2]);
            //double x = double.Parse(data[3]);
            //double minTime = 0;
            //double maxTime = 0;
            //double minSpeed = h / t;
            //if (minSpeed < x)  // <=x
            //{
            //    Console.Write(minTime);
            //    maxTime = Math.Round((h / x), 6);  // (x+1)
            //    Console.Write(' ');
            //    Console.Write(maxTime);
            //}
            //else
            //{
            //    minTime = Math.Round(((h - t * x) / (v - x)), 6);
            //    maxTime = Math.Round((h / minSpeed), 6);
            //    Console.Write(minTime);
            //    Console.Write(' ');
            //    Console.Write(maxTime);
            //}
            //Expr13
            string userInput = Console.ReadLine();
            var data = userInput.Split();
            double sideOfSquare = double.Parse(data[0]);
            double ropeLenght = double.Parse(data[1]);
            double areaOfGrass = 0;
            double pi = Math.PI;
            double sqrOfRopeLenght = ropeLenght * ropeLenght;
            if (ropeLenght <= sideOfSquare / 2)
                areaOfGrass = pi * sqrOfRopeLenght;
            else if (Math.Sqrt(2) * 0.5 * sideOfSquare <= ropeLenght)
                areaOfGrass = sideOfSquare*sideOfSquare;
            else
            {
                double angle = 2 * Math.Acos(sideOfSquare / (2 * ropeLenght));
                areaOfGrass = pi * sqrOfRopeLenght - 4 * (0.5 * sqrOfRopeLenght * (angle - Math.Sin(angle)));
            }
            areaOfGrass = Math.Round(areaOfGrass, 3);
            Console.Write(areaOfGrass);
        }
    }
}
