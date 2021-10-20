using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			return lines
				.Skip(1)
				.Where(
				x =>
				{
					var data = x.Split(';');
					return data.Length == 3 && int.TryParse(data[0], out int b) && Enum.TryParse(data[1], true, out SlideType c);
				}
				).ToDictionary(
				x => int.Parse(x.Split(';')[0]),
				x =>
				{
					var data = x.Split(';');
					Enum.TryParse(data[1], true, out SlideType slideType);
					return new SlideRecord(int.Parse(data[0]), slideType, data[2]);
				}
				);
		}

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines
				.Skip(1)
				.Select(
				x =>
				{
					var a = x.Split(';');
					if (a.Length == 4 && int.TryParse(a[0], out int b) && int.TryParse(a[1], out int c) &&
					DateTime.TryParse(a[2] + " " + a[3], out DateTime date) && slides.ContainsKey(c))
						return new VisitRecord(b, c, date, slides[c].SlideType);
					throw new FormatException($"Wrong line [{x}]");
				}
				);
		}
	}
}