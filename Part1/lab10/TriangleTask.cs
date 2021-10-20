using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        public static double GetABAngle(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c < 0)
                return double.NaN;
            return Math.Acos((-(c * c) + a * a + b * b) / (2 * a * b));
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(1,1,0,0)]
        [TestCase(0,1,1,double.NaN)]
        [TestCase(0, 0, 0, double.NaN)]
        [TestCase(-1,1,1,double.NaN)]
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            Assert.AreEqual(expectedAngle, TriangleTask.GetABAngle(a, b, c),1e-5);
        }
    }
}