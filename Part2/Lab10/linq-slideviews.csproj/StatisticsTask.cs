using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			try
			{
				return visits
					.OrderBy(x => x.DateTime).GroupBy(x => x.UserId)
					.Select(x => x
						.Bigrams()
						.Where(
							y => y.Item1.SlideType == slideType
							&& (y.Item2.DateTime - y.Item1.DateTime) >= TimeSpan.FromMinutes(1)
							&& (y.Item2.DateTime - y.Item1.DateTime) <= TimeSpan.FromHours(2))
						.Select(y => (y.Item2.DateTime - y.Item1.DateTime).TotalMinutes))
					.SelectMany(x => x)
					.Median();
			}
			catch
			{
				return 0;
			}
		}
	}
}