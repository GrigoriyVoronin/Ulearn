namespace Mazes
{
	public static class PyramidMazeTask
	{
        public static void MoveTroughOneBend(Robot robot, int width, int height, int numberOfBends, int bendingNumber,int counter)
        {
            EmptyMazeTask.MoveDistanceTowards(robot, width - 3-counter*2, Direction.Right);
            EmptyMazeTask.MoveDistanceTowards(robot, 2, Direction.Up);
            EmptyMazeTask.MoveDistanceTowards(robot, width - 3 -(counter+1)*2, Direction.Left);
            if (bendingNumber != numberOfBends - 1)
                EmptyMazeTask.MoveDistanceTowards(robot, 2, Direction.Up);
        }
        public static void MoveOut(Robot robot, int width, int height)
		{
            int numberOfBends = (height - 1) / 4;
            int counter = 0;
            for (int bendingNumber = 0; bendingNumber < numberOfBends; bendingNumber++)
            {
                MoveTroughOneBend(robot, width, height, numberOfBends, bendingNumber,counter);
                counter += 2;
            }
        }
	}
}