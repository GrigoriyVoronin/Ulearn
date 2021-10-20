using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Tickets
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var a = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
            var halfLen = a[0];
            var totalSum = a[1];
            var halfSum = totalSum / 2;
            if (totalSum == 0)
            {
                Console.WriteLine(1);
                return;
            }

            if (totalSum % 2 != 0 || halfLen * 9 < halfSum)
            {
                Console.WriteLine(0);
                return;
            }


            var opt = new LinkedList<BigInteger[]>();
            var arrFirst = new BigInteger[halfLen + 1];
            for (var i = 1; i <= halfLen; i++)
                arrFirst[i] = 1;
            opt.AddLast(arrFirst);

            for (var i = 1; i <= halfSum; i++)
            {
                var arr = new BigInteger[halfLen + 1];
                var shift = 0;
                if (i < 10)
                {
                    arr[1] = 1;
                    shift = 2;
                }
                else
                {
                    if (i % 9 == 0)
                        shift = i / 9;
                    else
                        shift = i / 9 + 1;
                }

                for (var j = shift; j <= halfLen; j++)
                    arr[j] = CalculateVariants(arr, opt, j);
                opt.AddLast(arr);
                if (opt.Count > 10)
                    opt.RemoveFirst();
            }

            Console.WriteLine(opt.Last.Value[halfLen] * opt.Last.Value[halfLen]);
        }

        private static BigInteger CalculateVariants(BigInteger[] arr, LinkedList<BigInteger[]> opt, int i)
        {
            var index = 0;
            var variants = arr[i - 1];
            var currentLine = opt.Last;
            while (++index < 10 && currentLine != null)
            {
                variants += currentLine.Value[i - 1];
                currentLine = currentLine.Previous;
            }

            return variants;
        }
    }
}