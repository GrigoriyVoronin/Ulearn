using System.Collections.Generic;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			DataPoint previosDataP = null;
			foreach (var dataP in data)
			{
				if (previosDataP == null)
					dataP.ExpSmoothedY = dataP.OriginalY;
				else
					dataP.ExpSmoothedY = alpha * dataP.OriginalY + (1 - alpha) * previosDataP.ExpSmoothedY;
				previosDataP = dataP;
				yield return dataP;
			}
		}
	}
}