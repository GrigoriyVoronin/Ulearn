using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minilab3
{
    class Program
    {
        private static int GetMinPowerOfTwoLargerThan(int number)
        {
            int result = 1;
            while (number >= result)
                result *= 2;
            return result;
        }

        public static string RemoveStartSpaces(string text)
        {
            while (text.Length > 0 && char.IsWhiteSpace(text[0]) )
            {
                text = text.Substring(1);
            }

            return text;
        }

        private static void WriteTextWithBorder(string text)
        {
            string frame = "";

            for (int i = 0; i < text.Length + 4; i++)
            {
                if (i == 0 || i == text.Length + 3)
                    frame += '+';
                else
                    frame += '-';
            }

            Console.WriteLine(frame);
            Console.WriteLine($"| {text} |");
            Console.WriteLine(frame);
        }

        private static void WriteBoard(int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(((i + j) % 2 == 0) ? "#" : ".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            //n1
            //Console.WriteLine(GetMinPowerOfTwoLargerThan(2)); // => 4
            //Console.WriteLine(GetMinPowerOfTwoLargerThan(15)); // => 16
            //Console.WriteLine(GetMinPowerOfTwoLargerThan(-2)); // => 1
            //Console.WriteLine(GetMinPowerOfTwoLargerThan(-100));
            //Console.WriteLine(GetMinPowerOfTwoLargerThan(100));
            //n2
            Console.WriteLine(RemoveStartSpaces("a"));
            Console.WriteLine(RemoveStartSpaces(" b"));
            Console.WriteLine(RemoveStartSpaces(" cd"));
            Console.WriteLine(RemoveStartSpaces(" efg"));
            Console.WriteLine(RemoveStartSpaces(" text"));
            Console.WriteLine(RemoveStartSpaces(" two words"));
            Console.WriteLine(RemoveStartSpaces("  two spaces"));
            Console.WriteLine(RemoveStartSpaces("	tabs"));
            Console.WriteLine(RemoveStartSpaces("		two	tabs"));
            Console.WriteLine(RemoveStartSpaces("                             many spaces"));
            Console.WriteLine(RemoveStartSpaces("               "));
            Console.WriteLine(RemoveStartSpaces("\n\r line breaks are spaces too"));
            //n3
            //WriteTextWithBorder("Menu:");
            //WriteTextWithBorder("");
            //WriteTextWithBorder(" ");
            //WriteTextWithBorder("Game Over!");
            //WriteTextWithBorder("Select level:");
            //n4
            //WriteBoard(8);
            //WriteBoard(1);
            //WriteBoard(2);
            //WriteBoard(3);
            //WriteBoard(10);
        }
    }
}
