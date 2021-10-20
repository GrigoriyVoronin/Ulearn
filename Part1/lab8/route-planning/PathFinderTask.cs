using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Diagnostics;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        private static double[][] lenAtoB;
        private static double[][] sortedLenAtoB;

        static int[] StandartOrder(int size)
        {
            var order = new int[size];
            for (int i = 0; i < size; i++)
                order[i] = i;
            return order;
        }

        static void Evaluate(int[] order, int[] bestOrder)
        {
            if (FindCurrentDistanse(bestOrder, order.Length) >
                FindCurrentDistanse(order, order.Length))
                order.CopyTo(bestOrder, 0);
        }

        static double FindCurrentDistanse(int[] order, int position)
        {
            var distanse = 0.0;
            for (int i = 1; i < position; i++)
                distanse += lenAtoB[order[i - 1]][ order[i]];
            return distanse;
         }


        private static void MakePermutation (int[] order, int position,
            int[] bestOrder, Stopwatch time)
        {
            if (FindCurrentDistanse(order,  position) >
                FindCurrentDistanse(bestOrder,  order.Length))
                return;

            if (position == order.Length)
            {
                Evaluate(order, bestOrder);
                return;
            }

            if (time.Elapsed.TotalSeconds > 1)
                return;

            var cloneArr = (double[])lenAtoB[order[position - 1]].Clone();

            for (int i = 0; i < order.Length; i++)
            {
                var subIndex = Array.IndexOf(cloneArr, sortedLenAtoB[order[position - 1]][i]);
                cloneArr[subIndex] = int.MaxValue;
                var index = Array.IndexOf(order, subIndex,0,position);
                if (index != -1)
                    continue;

                order[position] = subIndex;
                MakePermutation(order, position + 1, bestOrder, time);
            }
        }

        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var order = new int[checkpoints.Length];
            var bestOrder = StandartOrder(order.Length);
            lenAtoB = new double[checkpoints.Length][];
            CreateDataBase(checkpoints.Length, checkpoints);
            Stopwatch time = Stopwatch.StartNew();
            MakePermutation(order, 1, bestOrder,time);
            Console.WriteLine(time.Elapsed.TotalMilliseconds);
            return bestOrder;
        }

        private static void CreateDataBase(int size, Point[] checkpoints)
        {
            for (int i = 0; i < size; i++)
            {
                lenAtoB[i]=new double[size];
                for (int j = 0; j < i; j++)
                {
                    var len = PointExtensions.DistanceTo(checkpoints[i], checkpoints[j]);
                    lenAtoB[i][j] = len;
                    lenAtoB[j][i] = len;
                }
            }

            sortedLenAtoB=new double[size][];

            for (int i = 0; i < size; i++)
            {
                sortedLenAtoB[i] = (double[])lenAtoB[i].Clone();
                Array.Sort(sortedLenAtoB[i]);
            }
        }
    }
}