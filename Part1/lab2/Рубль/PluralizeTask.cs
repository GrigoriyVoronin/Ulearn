namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
            //рублей рубля рубль
            // Напишите функцию склонения слова "рублей" в зависимости от предшествующего числительного count.
            bool ifCountIn11To19 = count % 100 < 20 && count % 100 > 10;
            if (count % 10 == 1 && !ifCountIn11To19)
                return "рубль";
            else if ((count % 10 == 2 || count % 10 == 3 || count % 10 == 4) && !ifCountIn11To19)
                return "рубля";
            else
                return "рублей";
		}
	}
}