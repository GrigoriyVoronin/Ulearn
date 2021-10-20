using System;
using System.Collections.Generic;

namespace func_rocket
{
	public class LevelsTask
	{
		private static readonly Physics standardPhysics = new Physics();

		private static double GetDistance(Vector r, Vector t) => Math.Sqrt(Math.Pow(t.X - r.X, 2) + Math.Pow(t.Y - r.Y, 2));

		private static Rocket GetRocket() => new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

		private static Vector GetTarget(double x = 600, double y = 200) => new Vector(x, y);

		private static double GetHoleForce(double k, double distance) => k * distance / (distance * distance + 1);

		private static Gravity GetGravityHole(double k, Vector vector, bool isWhite) =>
			(size, v) => 
			((isWhite ? 1 : -1) * (v - GetTarget(vector.X, vector.Y))).Normalize() *
			GetHoleForce(k, GetDistance(v, GetTarget(vector.X, vector.Y)));

		private static Level GetNewLevel(string name, Vector target, Gravity gravity) =>
			new Level(name, GetRocket(), target, gravity, standardPhysics);

		public static IEnumerable<Level> CreateLevels()
		{
			Gravity whiteGravity = GetGravityHole(140, GetTarget(), true);
			Gravity blackGravity = GetGravityHole(300, GetTarget(400, 350), false);

			yield return GetNewLevel("Zero", GetTarget(), (size, v) => Vector.Zero);
			yield return GetNewLevel("Heavy", GetTarget(), (size, v) => new Vector(0, 0.9));
			yield return GetNewLevel("Up", GetTarget(700, 500), 
				(size, v) => new Vector(0, -300 / (size.Height - v.Y + 300.0)));
			yield return GetNewLevel("WhiteHole", GetTarget(), whiteGravity);
			yield return GetNewLevel("BlackHole", GetTarget(), blackGravity);
			yield return GetNewLevel("BlackAndWhite", GetTarget(), 
				(size, v) => (blackGravity(size, v) + whiteGravity(size, v)) / 2);
		}
	}
}