using System;

namespace DistanceTask
{
	public static class DistanceTask
	{
		// Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
        public static double CutLength (double ax, double ay, double bx, double by)
        {
            return Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
        }

		public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
		{
            double ab = CutLength(ax, ay, bx, by);
            double bc = CutLength(bx, by, x, y);
            double ac = CutLength(ax, ay, x, y);
            double p = 0.5 * (ab + bc + ac);
            if (ab * ab + bc * bc - ac * ac >= 0 && ab * ab + ac * ac - bc * bc >= 0 && ab != 0)
                return (2 / ab) * Math.Sqrt(p * (p - ab) * (p - bc) * (p - ac));
            else 
                return Math.Min(bc, ac);
		}
	}
}