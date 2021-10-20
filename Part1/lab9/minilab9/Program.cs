using System;
using Microsoft.VisualBasic.CompilerServices;

namespace minilab9
{
    class Program
    {
        private static readonly Random random = new Random();
        public static int BinSearchLeftBorder(int[] array, int value, int left, int right)
        {
            if (right == 0) 
                return -1;
            if (left >= right)
                return left - 1;
            var m = (left + right) / 2;
            if (array[m] < value)
                return BinSearchLeftBorder(array, value, m + 1, right);
            return BinSearchLeftBorder(array, value, left, m);
        }

        public static void BubbleSortRange(int[] array, int left, int right)
        {
            for (int i = right; i > 0; i--)
            for (int j = left + 1; j <= i; j++)
                if (array[j - 1] > array[j])
                {
                    var t = array[j - 1];
                    array[j - 1] = array[j];
                    array[j] = t;
                }
        }

        private static int[] GenerateArray(int length)
        {
            var array = new int[length];
            for (int i = 0; i < array.Length; i++)
                array[i] = random.Next(10);
            return array;
        }
        static void Main()
        {
            var arr = GenerateArray(5);
            Console.WriteLine(BinSearchLeftBorder(arr, 5, -1, arr.Length));
            BubbleSortRange(arr,1,4);
            foreach(var number in arr)
                Console.WriteLine(number);
        }
    }
}
