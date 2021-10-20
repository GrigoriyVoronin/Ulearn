using System.Drawing;

namespace rocket_bot
{
    public class Level
    {
        public readonly Vector[] Checkpoints;
        public readonly Rocket InitialRocket;
        public readonly int MaxTicksCount = 1000;
        public readonly Physics Physics;

        public readonly Size SpaceSize = new Size(800, 600);

        public Level(Rocket rocket, Vector[] checkpoints, Physics physics)
        {
            InitialRocket = rocket;
            Checkpoints = checkpoints;
            Physics = physics;
        }

        public Level Clone() => new Level(InitialRocket, Checkpoints, Physics);
    }
}