using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GeometryTasks;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        public static  Dictionary<Segment,Color> Dic = new Dictionary<Segment, Color>();
        public static Color GetColor(this Segment seg)
        {
            if (Dic.ContainsKey(seg))
                return Dic[seg];
            else
                return Color.Black;
        }

        public static void SetColor(this Segment seg, Color color)
        {
            Dic[seg]=color;
        }
    }
}
