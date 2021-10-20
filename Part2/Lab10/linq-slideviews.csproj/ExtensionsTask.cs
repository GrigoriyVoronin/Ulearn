using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		/// <summary>
		/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
		/// Медиана списка из четного количества элементов — это среднее арифметическое 
        /// двух серединных элементов списка после сортировки.
		/// </summary>
		/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
		public static double Median(this IEnumerable<double> items)
		{
			var elements = items.OrderBy(x => x).ToList();
			var count = elements.Count == 0 ? throw new InvalidOperationException() : elements.Count;
			return count % 2 == 0 ? (elements[count / 2] + elements[count / 2 - 1]) / 2 : elements[count / 2];
		}

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			var previousItem = default(T);
			var counter = 0;
			foreach (var a in items)
			{
				if (counter++ > 0)
					yield return Tuple.Create(previousItem, a);
				previousItem = a;
			}
		}
	}
}