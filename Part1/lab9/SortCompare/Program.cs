using System;
using System.Diagnostics;
using System.Windows.Forms;
using NUnit.Framework;
using System.Drawing;
using System.Net.Mime;
using System.Windows.Forms.DataVisualization.Charting;

namespace SortCompare
{
	public class Program
	{
		static  readonly Random rnd = new Random();

        private static int[] GenerateArray(int length)
        {
            var array = new int[length];
            for (int i = 0; i < array.Length; i++)
                array[i] = rnd.Next(100000);
            return array;
        }

        private static void BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            for (int j = 0; j < array.Length - 1; j++)
                if (array[j] > array[j + 1])
                {
                    int t = array[j + 1];
                    array[j + 1] = array[j];
                    array[j] = t;
                }
        }

        static int[] temporaryArray;

        static void Merge(int[] array, int start, int middle, int end)
        {
            var leftPtr = start;
            var rightPtr = middle + 1;
            var length = end - start + 1;
            for (int i = 0; i < length; i++)
            {
                if (rightPtr > end || (leftPtr <= middle && array[leftPtr] < array[rightPtr]))
                {
                    temporaryArray[i] = array[leftPtr];
                    leftPtr++;
                }
                else
                {
                    temporaryArray[i] = array[rightPtr];
                    rightPtr++;
                }
            }
            for (int i = 0; i < length; i++)
                array[i + start] = temporaryArray[i];
        }

        static void MergeSort(int[] array, int start, int end)
        {
            if (start == end) return;
            var middle = (start + end) / 2;
            MergeSort(array, start, middle);
            MergeSort(array, middle + 1, end);
            Merge(array, start, middle, end);

        }

        static void MergeSort(int[] array)
        {
            temporaryArray = new int[array.Length];
            MergeSort(array, 0, array.Length - 1);
        }

        static void HoareSort(int[] array, int start, int end)
        {
            if (end == start) return;
            var pivot = array[end];
            var storeIndex = start;
            for (int i = start; i <= end - 1; i++)
                if (array[i] <= pivot)
                {
                    var t = array[i];
                    array[i] = array[storeIndex];
                    array[storeIndex] = t;
                    storeIndex++;
                }

            var n = array[storeIndex];
            array[storeIndex] = array[end];
            array[end] = n;
            if (storeIndex > start) HoareSort(array, start, storeIndex - 1);
            if (storeIndex < end) HoareSort(array, storeIndex + 1, end);
        }

        static void HoareSort(int[] array)
        {
            HoareSort(array, 0, array.Length - 1);
        }

		static void MeasureTime(int[] array, Action<int[]> sortProcedure, Series series)
		{
			var watch = new Stopwatch();
			watch.Start();
            sortProcedure(array);
			watch.Stop();
			series.Points.Add(new DataPoint(array.Length, watch.ElapsedTicks));
		}

		private static Chart MakeChart(Series bubbleGraph, Series mergeGraph, Series hoareGraph)
		{
			var chart = new Chart();
			chart.ChartAreas.Add(new ChartArea());

			bubbleGraph.ChartType = SeriesChartType.FastLine;
			bubbleGraph.Color = Color.Red;

			mergeGraph.ChartType = SeriesChartType.FastLine;
			mergeGraph.Color = Color.Green;

            hoareGraph.ChartType = SeriesChartType.FastLine;
            hoareGraph.Color = Color.Blue;

			chart.Series.Add(bubbleGraph);
			chart.Series.Add(mergeGraph);
            chart.Series.Add(hoareGraph);
			chart.Dock = DockStyle.Fill;
			return chart;
		}

		[Test]
		[Explicit]
		public static void Main()
		{
			var bubbleGraph = new Series();
			var mergeGraph = new Series();
            var hoareGraph = new Series();

			for (int i = 1000; i <= 10000; i += 1000)
			{
				var array = GenerateArray(i);
                GC.Collect();
                MeasureTime((int[])array.Clone(), BubbleSort, bubbleGraph);
                GC.Collect();
                MeasureTime((int[])array.Clone(), MergeSort,mergeGraph);
                GC.Collect();
                MeasureTime((int[])array.Clone(), HoareSort, hoareGraph);
			}

			var chart = MakeChart(bubbleGraph,mergeGraph,hoareGraph);
            var form = new Form();
			form.ClientSize = new Size(800, 600);
			form.Controls.Add(chart);
            Application.Run(form);
        }
	}
}
