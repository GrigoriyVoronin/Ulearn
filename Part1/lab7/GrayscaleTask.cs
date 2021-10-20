namespace Recognizer
{
	public static class GrayscaleTask
	{
		public static double[,] ToGrayscale(Pixel[,] original)
		{
            var x = original.GetLength(0);
            var y = original.GetLength(1);
            var graysycle = new double[x,y];
            for (int i=0; x>i; i++)
                for (int j=0; y>j; j++)
                    graysycle[i, j] = (0.299 * original[i, j].R + 0.587 * original[i, j].G + 0.114 * original[i, j].B) / 255;

			return graysycle;
		}
	}
}