using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var userInput = Console.ReadLine();
            var firstHalfSum = int.Parse(userInput[0].ToString()) + int.Parse(userInput[1].ToString()) + int.Parse(userInput[2].ToString());
            var secondHalfSum = int.Parse(userInput[3].ToString()) + int.Parse(userInput[4].ToString()) + int.Parse(userInput[5].ToString());
            if ((firstHalfSum > secondHalfSum) && (userInput[5] != '9'))
                Console.WriteLine("Yes");
            else if ((secondHalfSum > firstHalfSum) && (userInput[5] != '0'))
                Console.WriteLine("Yes");
            else
                Console.WriteLine("No");
        }
    }
}
