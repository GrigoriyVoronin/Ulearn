using System;
using System.Drawing;

namespace func_rocket
{
	public class Physics
	{
		private readonly double mass;
		private readonly double maxVelocity;
		private readonly double maxTurnRate;

		public Physics() : this(1.0, 300.0, 0.15)
		{
		}

		public Physics(double mass, double maxVelocity, double maxTurnRate)
		{
			this.mass = mass;
			this.maxVelocity = maxVelocity;
			this.maxTurnRate = maxTurnRate;
		}

		public Rocket MoveRocket(Rocket rocket, RocketForce force, Turn turn, Size spaceSize, double dt)
		{
			var turnRate = turn == Turn.Left ? -maxTurnRate : turn == Turn.Right ? maxTurnRate : 0;
			var dir = rocket.Direction + turnRate * dt;
			var velocity = rocket.Velocity + force(rocket) * dt / mass;
			if (velocity.Length > maxVelocity) velocity = velocity.Normalize() * maxVelocity;
			var location = rocket.Location + velocity * dt;
			if (location.X < 0) velocity = new Vector(Math.Max(0, velocity.X), velocity.Y);
			if (location.X > spaceSize.Width) velocity = new Vector(Math.Min(0, velocity.X), velocity.Y);
			if (location.Y < 0) velocity = new Vector(velocity.X, Math.Max(0, velocity.Y));
			if (location.Y > spaceSize.Height) velocity = new Vector(velocity.X, Math.Min(0, velocity.Y));
			return new Rocket(location.BoundTo(spaceSize), velocity, dir);
		}
	}
}