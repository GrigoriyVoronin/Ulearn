using System.Collections.Generic;

namespace yield
{
	public static class MovingAverageTask
	{
		public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var numbersInWindow = new Queue<double>();
			var previusAvr = 0.0;
			foreach (var dataP in data)
			{
				if (numbersInWindow.Count<windowWidth)
				{
					numbersInWindow.Enqueue(dataP.OriginalY);
					previusAvr += dataP.OriginalY;
				}
				else
				{
					numbersInWindow.Enqueue(dataP.OriginalY);
					previusAvr += dataP.OriginalY - numbersInWindow.Dequeue();
				}
				dataP.AvgSmoothedY = previusAvr / numbersInWindow.Count;
				yield return dataP;
			}
		}
	}
}