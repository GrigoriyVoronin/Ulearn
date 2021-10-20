using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
	internal static class MedianFilterTask
	{
        public static double FindMedianValue(double[,]original, int x, int y)
        {
            var values = new List<double>();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        values.Add(original[x + 1 - i, y + 1 - j]);
                    }
                    catch
                    {
                        continue;
                    }
                }
            values.Sort();
            var index = values.Count / 2;
            return values.Count % 2 == 1 ? values[index] : (values[index] + values[values.Count / 2 - 1])/2;
        }
		public static double[,] MedianFilter(double[,] original)
		{
            var x = original.GetLength(0);
            var y = original.GetLength(1);
            var pixels = new double[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    pixels[i, j] = FindMedianValue(original, i, j);
			return pixels;
		}
	}
}