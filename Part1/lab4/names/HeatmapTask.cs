using System;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            var x = new string[30];
            var y = new string[12];
            var intense = new double[30, 12];
            for (int i=0;i<names.Length;i++)
            {
                if (names[i].BirthDate.Day!=1)
                    intense[names[i].BirthDate.Day-2, names[i].BirthDate.Month-1]++;
            }

            for(int i = 0; i < 30; i++)
            {
                x[i] = (i + 2).ToString();
            }
            for (int j = 0; j < 12; j++)
            {
                y[j] = (j + 1).ToString();
            }
            return new HeatmapData( "Рождаемость в число в зависимости от месяца", intense, x, y);
        }
    }
}