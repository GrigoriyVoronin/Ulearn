using System;
using NUnit.Framework;
using System.Drawing;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            double wristX = x + Manipulator.Palm * Math.Cos(Math.PI - alpha);
            double wristY = y + Manipulator.Palm * Math.Sin(Math.PI - alpha);
            var distanseFromShoulderToWrist = Math.Sqrt(wristX * wristX + wristY * wristY);
            var elbow = TriangleTask.GetABAngle(Manipulator.Forearm, Manipulator.UpperArm, distanseFromShoulderToWrist);
            var shoulder = TriangleTask.GetABAngle(Manipulator.UpperArm, distanseFromShoulderToWrist, Manipulator.Forearm)
                + Math.Atan2(wristY, wristX);
            var wrist = -alpha - shoulder - elbow;
            if (double.IsNaN(shoulder) || double.IsNaN(elbow) || double.IsNaN(wrist))
                return new[] { double.NaN, double.NaN, double.NaN };
            return new[] { shoulder, elbow, wrist };
        }
    }

    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            var rnd = new Random();
            for (int i=0; i<1000;i++)
            {
                var x = rnd.Next(-300, 300);
                var y = rnd.Next(-300, 300);
                var al = rnd.NextDouble() * Math.PI;
                var angles = ManipulatorTask.MoveManipulatorTo(x, y, al);
                
                if (!double.IsNaN(angles[0]))
                {
                    var coord = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2]);
                    Assert.AreEqual(x, coord[2].X, 0.001);
                    Assert.AreEqual(y, coord[2].Y, 0.001);
                }
            }
        }
    }
}