namespace Mazes
{
	public static class SnakeMazeTask
	{
        public static void MoveTroughOneBend(Robot robot, int width, int height, int numberOfBends, int bendingNumber)
        {
            EmptyMazeTask.MoveDistanceTowards(robot, width - 3, Direction.Right);
            EmptyMazeTask.MoveDistanceTowards(robot, 2, Direction.Down);
            EmptyMazeTask.MoveDistanceTowards(robot, width - 3, Direction.Left);
            if (bendingNumber != numberOfBends - 1)
                EmptyMazeTask.MoveDistanceTowards(robot, 2, Direction.Down);
        }
        public static void MoveOut(Robot robot, int width, int height)
        {
            int numberOfBends = (height - 1) / 4;
            for (int bendingNumber = 0; bendingNumber < numberOfBends; bendingNumber++)
            {
                MoveTroughOneBend(robot, width, height, numberOfBends, bendingNumber);
            }
        }
	}
}