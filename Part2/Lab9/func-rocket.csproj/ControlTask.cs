using System;

namespace func_rocket
{
	public class ControlTask
	{
		public static Turn ControlRocket(Rocket rocket, Vector target)
		{
			var predcitedVector = rocket.Location + rocket.Velocity + ForcesTask.GetThrustForce(10)(rocket);
			var predictedAngle = (target - rocket.Location).Angle - (predcitedVector - rocket.Location).Angle;
			return predictedAngle > 0 ? Turn.Right : Turn.Left;
		}
	}
}