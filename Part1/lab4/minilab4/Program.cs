using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minilab3
{

    class Program
    {
        
        public static int[] GetFirstEvenNumbers(int count)
        {
            var mas = new int[count];
            for (int i = 0; i < count; i++)
                mas[i] = (i + 1) * 2;
            return mas;
        }

        public static int MaxIndex1(double[] array)
        {
            if (array.Length == 0)
                return -1;
            var maxIndex = 0;
            var maxEl = array[0];
            for(int i=1;i<array.Length;i++)
            {
                if (array[i]>maxEl)
                {
                    maxEl = array[i];
                    maxIndex=i;
                }
            }
            return maxIndex;
            //return Array.IndexOf(array, array.Max());

        }

        public static int GetElementCount(int[] items, int itemToCount)
        {
            var count = 0;
            foreach (var element in items)
            {
                if (element == itemToCount)
                    count++;
            }
            return count;
        }
        public static int FindSubarrayStartIndex(int[] array, int[] subArray)
        {
            for (var i = 0; i < array.Length - subArray.Length + 1; i++)
                if (ContainsAtIndex(array, subArray, i))
                    return i;
            return -1;
        }

        public static bool ContainsAtIndex(int[] array, int [] subArray, int i)
        {
            bool isIt = true;
            for(int j = 0; j < subArray.Length; j++)
                isIt = isIt && array[i+j] == subArray[j];
            return isIt;
        }
        enum Suits
        {
            Wands,
            Coins,
            Cups,
            Swords
        }
        private static string GetSuit(Suits suit)
        {
            return new string[] { "жезлов", "монет", "кубков", "мечей" }[(int)suit];
        }

        public static bool CheckFirstElement(int[] array)
        {
            return array != null && array.Length != 0 && array[0] == 0;
        }

        public static int[] GetPoweredArray(int[] arr, int power)
        {
            return arr.Select(x => (int)Math.Pow(x, power)).ToArray();
            var mas = new int[arr.Length];
            for (var i = 0; i < arr.Length; i++)
                mas[i] = (int)Math.Pow(arr[i], power);
            return mas;
        }

        public enum Mark
        {
            Empty,
            Cross,
            Circle
        }

        public enum GameResult
        {
            CrossWin,
            CircleWin,
            Draw
        }
        private static void Run(string description)
        {
            Console.WriteLine(description.Replace(" ", Environment.NewLine));
            Console.WriteLine(GetGameResult(CreateFromString(description)));
            Console.WriteLine();
        }
        private static Mark[,] CreateFromString(string str)
        {
            var field = str.Split(' ');
            var ans = new Mark[3, 3];
            for (int x = 0; x < field.Length; x++)
                for (var y = 0; y < field.Length; y++)
                    ans[x, y] = field[x][y] == 'X' ? Mark.Cross : (field[x][y] == 'O' ? Mark.Circle : Mark.Empty);
            return ans;
        }
        public static GameResult GetGameResult(Mark[,] field)
        {
            bool isCircleWin = false;
            bool isCrossWin = false;
            for (int i =0; i<3;i++)
            {
                isCircleWin =  isCircleWin || IscomboByMark(field, i, Mark.Circle);
                isCrossWin = isCrossWin || IscomboByMark(field, i, Mark.Cross);
            }
            if (isCrossWin == isCircleWin)
                return GameResult.Draw;
            else if (isCircleWin)
                return GameResult.CircleWin;
            else
                return GameResult.CrossWin;
        }
        public static bool IscomboByMark (Mark [,] field, int i, Mark mark)
        {
            return (field[i, 0] == field[i, 1] && field[i, 1] == field[i, 2] && field[i, 2] == mark) ||
                (field[0, i] == field[1, i] && field[1, i] == field[2, i] && field[2, i] == mark) ||
                (field[0, 0] == field[1,1] && field[1,1]== field[2,2] && field[2,2] == mark) ||
                (field[0, 2] == field[1, 1] && field[1, 1] == field[2, 0] && field[2, 0] == mark);

        }

        public static void PrintArray (int [] arr)
        {
            foreach (var i in arr)
                Console.Write(i + " ");
           Console.WriteLine();
        }

        static void Main(string[] args)
        {
            //n1
            // PrintArray(GetFirstEvenNumbers(3));
            //n2
            //Console.WriteLine(MaxIndex1(new double [] {5,7,9 }));
            //n3
            //Console.WriteLine(GetElementCount(new int[] { 1, 1, 2, 2, 3 }, 1));
            //n4
            //Console.WriteLine(FindSubarrayStartIndex(new int[] { 1, 2, 3, 4, 3, 4 }, new int[] { 3, 4 }));
            //№5
            //Console.WriteLine(GetSuit(Suits.Wands));
            //Console.WriteLine(GetSuit(Suits.Coins));
            //Console.WriteLine(GetSuit(Suits.Cups));
            //Console.WriteLine(GetSuit(Suits.Swords));
            //N6
            //Console.WriteLine(CheckFirstElement(null));
            //Console.WriteLine(CheckFirstElement(new int[0]));
            //Console.WriteLine(CheckFirstElement(new[] { 1 }));
            //Console.WriteLine(CheckFirstElement(new[] { 0 }));
            //n7
            //var arrayToPower = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            //PrintArray(GetPoweredArray(arrayToPower, 1));
            //PrintArray(GetPoweredArray(arrayToPower, 2));
            //PrintArray(GetPoweredArray(arrayToPower, 3));
            //PrintArray(GetPoweredArray(new int[0], 1));
            //PrintArray(GetPoweredArray(new[] { 42 }, 0));
            //n8
            //Run("XXX OO. ...");
            //Run("OXO XO. .XO");
            //Run("OXO XOX OX.");
            //Run("XOX OXO OXO");
            //Run("... ... ...");
            //Run("XXX OOO ...");
            //Run("XOO XOO XX.");
            //Run(".O. XO. XOX");
        }
    }
}
