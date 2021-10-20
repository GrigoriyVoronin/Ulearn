using System;

namespace Percents
{
    class Program
    {
        static void Main()
        {
            string userInput = Console.ReadLine();
            Console.WriteLine(Calculate(userInput));
        }
        public static double Calculate(string userInput)
        {
            var data = userInput.Split();
            double amount = double.Parse(data[0]);
            double percent = double.Parse(data[1]);
            int month = int.Parse(data[2]);
            return amount * Math.Pow((1 + (percent / 100) / 12), month);
        }
    }
}
