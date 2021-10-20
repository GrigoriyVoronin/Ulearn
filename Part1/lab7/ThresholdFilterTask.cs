using System;
using System.Collections.Generic;
using System.Linq;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        static double FindThreshold(double[,] original, double whitePixelsFraction)
        {
            var amountOfpixels = original.Length;
            var countOfWhitePixel = (int)(whitePixelsFraction * amountOfpixels);
            var values = new List<double>(amountOfpixels);
            foreach (var value in original)
                values.Add(value);
            values.Sort();
            try
            {
                return values[amountOfpixels - countOfWhitePixel];
            }
            catch
            {
                return Int32.MaxValue;
            }
        }

        static double[,] CreateRuseltImage(double[,] original, double treshold)
        {
            var x = original.GetLength(0);
            var y = original.GetLength(1);
            var pixels = new double[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    pixels[i, j] = original[i, j] < treshold ? 0.0 : 1.0;
            return pixels;
        }

        public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
        {
            var treshold = FindThreshold(original, whitePixelsFraction);
            return CreateRuseltImage(original, treshold);
        }
    }
}