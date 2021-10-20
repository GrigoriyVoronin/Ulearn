using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace StructBenchmarking
{
    public class ChartBuilder
    {
        public Control CreateTimeOfObjectSizeChart(ChartData chartData)
        {
            var chart = new ZedGraphControl();
            chart.GraphPane.YAxis.Title.Text = "Time";
            chart.GraphPane.XAxis.Title.Text = "Size";
            chart.GraphPane.Title.Text = chartData.Title;
            chart.GraphPane.AddCurve(
                "Classes",
                chartData.ClassPoints.Select(z => (double) z.FieldsCount).ToArray(),
                chartData.ClassPoints.Select(z => z.AverageTime).ToArray(),
                Color.Red);
            chart.GraphPane.AddCurve(
                "Structures",
                chartData.StructPoints.Select(z => (double) z.FieldsCount).ToArray(),
                chartData.StructPoints.Select(z => z.AverageTime).ToArray(),
                Color.Blue);

            chart.GraphPane.XAxis.Scale.MinAuto = true;
            chart.GraphPane.XAxis.Scale.MaxAuto = true;
            chart.GraphPane.YAxis.Scale.MinAuto = true;
            chart.GraphPane.YAxis.Scale.MaxAuto = true;
            chart.AxisChange();
            chart.Invalidate();
            return chart;
        }
    }
}