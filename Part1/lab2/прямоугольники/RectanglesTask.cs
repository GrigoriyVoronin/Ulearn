using System;

namespace Rectangles
{
	public static class RectanglesTask
	{
        public static bool AreIntersectedFirstAndSecond (Rectangle r1, Rectangle r2)
        {
            return r2.Top <= r1.Bottom && r1.Top <= r2.Bottom && r2.Left <= r1.Right && r1.Left <= r2.Right;
        }

        public static bool IsRecnagleSecondInRectangleFirst (Rectangle r1, Rectangle r2)
        {
            return r1.Left <= r2.Left && r1.Right >= r2.Right && r1.Bottom >= r2.Bottom && r1.Top <= r2.Top;
        }

		public static bool AreIntersected(Rectangle r1, Rectangle r2)
		{
            return AreIntersectedFirstAndSecond(r1, r2) || AreIntersectedFirstAndSecond(r2, r1);
        }

        public static int IntersectionSquare(Rectangle r1, Rectangle r2)
        {
            if (AreIntersected(r1, r2))
            {
                int leftIntersection = Math.Max(r1.Left, r2.Left);
                int rightIntersection = Math.Min(r1.Right, r2.Right);
                int topIntersection = Math.Max(r1.Top, r2.Top);
                int bottomIntersection = Math.Min(r1.Bottom, r2.Bottom);
                return (bottomIntersection-topIntersection) * (rightIntersection-leftIntersection);
            }
            else
                return 0;
        }

		public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
		{
            if (IsRecnagleSecondInRectangleFirst(r1, r2))
                return 1;
            else if (IsRecnagleSecondInRectangleFirst(r2, r1))
                return 0;
            else
                return -1;
		}
	}
}