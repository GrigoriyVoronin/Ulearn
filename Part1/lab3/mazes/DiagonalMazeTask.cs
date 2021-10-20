namespace Mazes
{
	public static class DiagonalMazeTask
	{
        public static void MoveTroughtDiagonal(Robot robot,int bigMove,Direction bigDir,
            Direction smallDir, int numberOfBending)
        {
            for (int i = 0; i < numberOfBending; i++)
            {
                EmptyMazeTask.MoveDistanceTowards(robot, bigMove, bigDir);
                if (i != numberOfBending - 1)
                    EmptyMazeTask.MoveDistanceTowards(robot, 1, smallDir);
            }
        }

        public static void MoveOut(Robot robot, int width, int height)
		{
            if (width > height)
                {
                int bigMove = (int)System.Math.Round((double)width / height);
                MoveTroughtDiagonal(robot, bigMove, Direction.Right, Direction.Down, height - 2);
                }
            else
                {
                int bigMove = (int)System.Math.Round((double)height / width);
                MoveTroughtDiagonal(robot, bigMove, Direction.Down, Direction.Right, width - 2);
                }
        }
	}
}