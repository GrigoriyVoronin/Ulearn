using System;

namespace Cond1_7
{
    class Program
    {
        public static bool IsItcorrectMove
        {

        }
        static void Main(string[] args)
        {
            //cond1
            Console.WriteLine("Введите коардинаты начальной клетки и конечной клетки, через пробел: ");
            var userInput = Console.ReadLine().Split();
            var strat = userInput[0];
            var end = userInput[1];
            Console.WriteLine("Введите название фигуры: слон/король/ферзь/конь/ладья");
            var name = Console.ReadLine();

        }
    }
}
