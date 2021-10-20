using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vec)
        {
            return Geometry.Add(this, vec);
        }

        public bool Belongs(Segment seg)
        {
            return Geometry.IsVectorInSegment(this, seg);
        }
    }

    class Geometry
    {
        public static double GetLength(Vector vec)
        {
            return Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
        }

        public static double GetLength(Segment seg)
        {
            return GetLength(new Vector { X = seg.End.X - seg.Begin.X, Y = seg.End.Y - seg.Begin.Y });
        }

        public static Vector Add(Vector vec1, Vector vec2)
        {
            return new Vector { X = vec1.X + vec2.X, Y = vec1.Y + vec2.Y };
        }

        public static bool IsVectorInSegment(Vector vec, Segment seg)
        {
             return CheckStraight (vec,seg) && CheckXY(vec,seg);
        }

        private static bool CheckStraight (Vector vec, Segment seg)
        {
            return (vec.X - seg.Begin.X) * (seg.End.Y - seg.Begin.Y) ==
                (vec.Y - seg.Begin.Y) * (seg.End.X - seg.Begin.X);
        }

        private static bool CheckXY(Vector vec, Segment seg)
        {
            return ((vec.X >= seg.Begin.X && vec.X <= seg.End.X) || (vec.X <= seg.Begin.X && vec.X >= seg.End.X))&&
                ((vec.Y >= seg.Begin.Y && vec.Y <= seg.End.Y) || (vec.Y <= seg.Begin.Y && vec.Y >= seg.End.Y));
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains (Vector vec)
        {
            return Geometry.IsVectorInSegment(vec, this);
        }
    }
}
