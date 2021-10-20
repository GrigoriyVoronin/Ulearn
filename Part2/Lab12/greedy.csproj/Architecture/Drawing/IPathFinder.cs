using System.Collections.Generic;
using System.Drawing;

namespace Greedy.Architecture.Drawing
{
	public interface IPathFinder
	{
		List<Point> FindPathToCompleteGoal(State state);
	}
}