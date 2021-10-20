using System.Collections.Generic;
using System.Numerics;

namespace Tickets
{
    public static class TicketsTask
    {
        public static BigInteger Solve(int halfLen, int totalSum)
        {
            var halfSum = totalSum / 2;
            if (totalSum == 0)
                return 1;
            if (totalSum % 2 != 0 || halfLen * 9 < halfSum)
                return 0;

            var opt = new LinkedList<BigInteger[]>();
            var arrFirst = new BigInteger[halfLen + 1];
            for (var i = 1; i <= halfLen; i++)
                arrFirst[i] = 1;
            opt.AddLast(arrFirst);
            for (var i = 1; i <= halfSum; i++)
                CalculateNExtLine(opt, i,halfLen);

            return opt.Last.Value[halfLen] * opt.Last.Value[halfLen];
        }

        private static void CalculateNExtLine(LinkedList<BigInteger[]> opt, int index, int halfLen)
        {
            var arr = new BigInteger[halfLen + 1];
            var shift = CalculateShift(arr, index);

            for (var j = shift; j <= halfLen; j++)
                arr[j] = CalculateVariants(arr, opt, j);
            opt.AddLast(arr);
            if (opt.Count > 10)
                opt.RemoveFirst();
        }

        private static int CalculateShift(BigInteger[] arr, int i)
        {
            if (i >= 10)
                return i % 9 == 0 ? i / 9 : i / 9 + 1;

            arr[1] = 1;
            return 2;
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