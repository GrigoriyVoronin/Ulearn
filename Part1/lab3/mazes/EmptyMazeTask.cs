namespace Mazes
{
	public static class EmptyMazeTask
	{
        public static void MoveDistanceTowards (Robot robot, int distanse, Direction direction)
        {
            for (int i =0;i < distanse; i++)
                robot.MoveTo(direction);
        }
		public static void MoveOut(Robot robot, int width, int height)
		{
            MoveDistanceTowards(robot, height - 3, Direction.Down);
            MoveDistanceTowards(robot, width - 3, Direction.Right);
        }
	}
}