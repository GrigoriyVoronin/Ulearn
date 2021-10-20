using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowPos = new PointF( (float)Math.Cos(shoulder) * Manipulator.UpperArm,
                (float)Math.Sin(shoulder) * Manipulator.UpperArm);
            var piMinusElbowAngle = elbow - Math.PI + shoulder; 
            var wristPos = new PointF((float)Math.Cos(piMinusElbowAngle) * Manipulator.Forearm + elbowPos.X,
                (float)Math.Sin(piMinusElbowAngle) * Manipulator.Forearm + elbowPos.Y);
            var piMinusWristAngle = wrist - (Math.PI / 2 - piMinusElbowAngle);
            var palmEndPos = new PointF((float)Math.Sin(piMinusWristAngle) * Manipulator.Palm + wristPos.X,
                (float)-Math.Cos(piMinusWristAngle) * Manipulator.Palm + wristPos.Y);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]

        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }

        [TestCase(Math.PI / 2, -Math.PI / 2, Math.PI,
            -Manipulator.Forearm - Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, -Math.PI / 2, -Math.PI/2,
            -Manipulator.Forearm , Manipulator.UpperArm - Manipulator.Palm)]
        public void TestNegativeAngles(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }
}