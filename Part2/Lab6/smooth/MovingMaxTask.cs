using System.Collections.Generic;

namespace yield
{
	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var potentialMaxs = new LinkedList<DataPoint>();
			var pointsInWindow = new Queue<DataPoint>();
			foreach (var dataP in data)
			{
				pointsInWindow.Enqueue(dataP);
				if (pointsInWindow.Count > windowWidth)
					potentialMaxs.Remove(pointsInWindow.Dequeue());
				var curDataP = potentialMaxs.Last;
				while (curDataP!=null && dataP.OriginalY > curDataP.Value.OriginalY)
				{
					curDataP = curDataP.Previous;
					potentialMaxs.RemoveLast();
				}
				potentialMaxs.AddLast(dataP);
				dataP.MaxY = potentialMaxs.First.Value.OriginalY;
				yield return dataP;
			}
		}
	}
}