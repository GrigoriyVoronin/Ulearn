using System.Drawing;
using System;

namespace Fractals
{
	internal static class DragonFractalTask
	{
        public static void CalculationNewValues (double x, double y, out double x1, out double y1, double angle, int shift)
        {
            x1 = (x * Math.Cos(angle) - y * Math.Sin(angle)) / Math.Sqrt(2) + shift;
            y1 = (x * Math.Sin(angle) + y * Math.Cos(angle)) / Math.Sqrt(2);
        }
		public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
		{
            var random = new Random(seed);
            double x = 1;
            double y = 0;
            double x1 = 0;
            double y1 = 0;
            var pi4 = Math.PI / 4;
            pixels.SetPixel(x, y);
            for (int i = 0; i < iterationsCount;i++)
            {
                if (random.Next(2)%2 == 0)
                    CalculationNewValues(x, y, out x1, out y1, pi4, 0);
                else
                    CalculationNewValues(x, y, out x1, out y1, 3*pi4, 1);
                x = x1;
                y = y1;
                pixels.SetPixel(x, y);
            }
        }
	}
}