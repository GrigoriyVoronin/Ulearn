using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        static double CalculateValue(double[,] pixels, double[,] s, int indent, int x, int y,int size)
        {
            var g = 0.0;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    g += pixels[x + i - indent, y + j - indent] * s[i, j];

            return g;
        }

        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var result = new double[width, height];
            var size = sx.GetLength(0);
            var sy = new double[size,size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    sy[j, i] = sx[i, j];

            var indent = size / 2;

            for (int x = indent; x < width - indent; x++)
                for (int y = indent; y < height - indent; y++)
                {
                    var gx = CalculateValue(g, sx, indent, x, y, size);
                    var gy = CalculateValue(g, sy, indent, x, y, size);
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }

            return result;
        }
    }
}