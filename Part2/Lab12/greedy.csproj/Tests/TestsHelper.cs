using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy.Tests
{
	public static class TestsHelper
	{
		public static StateController LoadStateFromInputData_And_MoveThroughPath(string stateInputData,
			IPathFinder pathFinder, string stateName = "")
		{
			var isGreedyPathFinder = pathFinder is GreedyPathFinder;
			var path = FindPath(pathFinder, StatesLoader.LoadStateFromInputData(stateInputData, stateName), isGreedyPathFinder);
			return MoveThroughPath(isGreedyPathFinder, StatesLoader.LoadStateFromInputData(stateInputData, stateName), path);
		}

		public static StateController MoveThroughPath(bool isGreedyPathFinder, State stateForController, List<Point> path)
		{
			var controller = new StateController(stateForController, path, isGreedyPathFinder);
			controller.MoveThroughPathFast();
			return controller;
		}

		private static List<Point> FindPath(IPathFinder pathFinder, State stateForStudent, bool isGreedyPathFinder)
		{
			if (!isGreedyPathFinder)
			{
				stateForStudent.RemoveGoal();
			}
			var path = pathFinder.FindPathToCompleteGoal(stateForStudent);
			return path;
		}
	}
}