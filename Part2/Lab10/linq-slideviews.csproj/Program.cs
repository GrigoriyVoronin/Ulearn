using System;
using System.IO;
using System.Linq;
using System.Text;

namespace linq_slideviews
{
	public class Program
	{
		private static void Main()
		{
			var encoding = Encoding.GetEncoding(1251);
			var slideRecords = ParsingTask.ParseSlideRecords(File.ReadAllLines("slides.txt", encoding));
			var visitRecords = ParsingTask.ParseVisitRecords(File.ReadAllLines("visits.txt", encoding), slideRecords).ToList();
			foreach (var slideType in new[] { SlideType.Theory, SlideType.Exercise, SlideType.Quiz })
			{
				var time = StatisticsTask.GetMedianTimePerSlide(visitRecords, slideType);
				Console.WriteLine("Median time per slide '{0}': {1} mins", slideType, time);
			}
		}
	}
}
