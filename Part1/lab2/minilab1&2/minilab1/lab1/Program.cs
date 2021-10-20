using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace lab1
{
    class Program
    {
        public static string GetGreetingMessage( string name, double salary)
        {
            salary = Math.Ceiling(salary);
            return "Hello," + name + "your salary is " + salary.ToString();
        }
        public static int GetSquare (int number)
        {
            return (int)Math.Pow(number, 2);
        }
        public static void Print (int number)
        {
            Console.WriteLine(number);
        }
        public static string GetLastHalf(string text)
        {
            return text.Remove(0, (text.Length / 2)).Replace(" ", "");
        }
        public static string GetMinX (double a, double b, double c)
        {
            if (a > 0)
                return (-b / (2 * a)).ToString();
            else if (a == 0 && b == 0)
                return c.ToString();
            else return "Impossible";

        }
        static void Main()
        {
            //1 task
            //double num1 = +5.5e-2;
            //float num2 = 7.8f;
            //int num3 = 0;
            //long num4 = 2000000000000L;
            //Console.WriteLine(num1);
            //Console.WriteLine(num2);
            //Console.WriteLine(num3);
            //Console.WriteLine(num4);
            //2task
            //double pi = Math.PI;
            //int tenThousand = (int) 10000L;
            //float tenThousandPi = (float)pi * tenThousand;
            //int roundedTenThousandPi = (int)Math.Round(tenThousandPi);
            //int integerPartOfTenThousandPi = (int)tenThousandPi;
            //Console.WriteLine(integerPartOfTenThousandPi);
            //Console.WriteLine(roundedTenThousandPi);
            //task3
            //double amount = 1.11;
            //int peopleCount = 60;
            //int totalMoney = (int)Math.Round(amount * peopleCount);
            //Console.WriteLine(totalMoney);
            //task4
            //string doubleNumber = "894376,243643";
            //double number = double.Parse(doubleNumber);
            //Console.WriteLine(number+1);
            //task5
            //var a = 5.0;
            //a += 0.5;
            //Console.WriteLine(a);
            //task6
            //Console.WriteLine(GetGreetingMessage("Student", 10.01));
            //Console.WriteLine(GetGreetingMessage("Bill Gates", 1000000.5));
            //Console.WriteLine(GetGreetingMessage("Steve Jobs", 1));
            // task7
            //Print(GetSquare(42));
            //task 8
            //Console.WriteLine(GetLastHalf("I love CSharp!"));
            //Console.WriteLine(GetLastHalf("1234567890"));
            //Console.WriteLine(GetLastHalf("до ре ми фа сол ля си"));
            //task 9
            //Console.WriteLine("Hello, world!");
            //var number = 5.5;
            //number += 7;
            //Console.WriteLine(number);
            //task 10
            //Console.WriteLine(GetMinX(1,2,3));
            //Console.WriteLine(GetMinX(0, 3, 2));
            //Console.WriteLine(GetMinX(1, -2, -3));
            //Console.WriteLine(GetMinX(5, 2, 1));
            //Console.WriteLine(GetMinX(4, 3, 2));
            //Console.WriteLine(GetMinX(0, 4, 5));

            //Console.WriteLine(GetMinX(0, 0, 2) != "Impossible");
            //Console.WriteLine(GetMinX(0, 0, 0) != "Impossible");
        }
    }
}
