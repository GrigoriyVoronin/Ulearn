using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        static double[,] FindNeighborhood(double[,] g, int x, int y)
        {
            var result = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        result[i, j] = g[x - 1 + i, y - 1 + j];
                    }
                    catch
                    {
                        result[i, j] = 0;
                    }
                }
            return result;
        }

        static double CalculateValue(double[,] neighborhoodOfPoint, double[,] s)
        {
            var g = 0.0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    try
                    {
                        g += neighborhoodOfPoint[i, j] * s[i, j];
                    }
                }
            return g;
        }

        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var sy = new double[sx.GetLength(1), sx.GetLength(0)];
            for (int i = 0; i < sx.GetLength(0); i++)
                for (int j = 0; j < sx.GetLength(1); j++)
                {
                    sy[j, i] = sx[i, j];
                }
            var result = new double[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    // Вместо этого кода должно быть поэлементное умножение матриц sx и полученной транспонированием из неё sy на окрестность точки (x, y)
                    // Такая операция ещё называется свёрткой (Сonvolution)
                    var neighborhoodOfPoint = FindNeighborhood(g, x, y);
                    var gx = CalculateValue(neighborhoodOfPoint, sx);
                    var gy = CalculateValue(neighborhoodOfPoint, sy);
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);

                }
            return result;
        }
    }
}