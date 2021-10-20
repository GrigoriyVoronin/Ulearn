using System;
using System.Windows.Forms;

namespace StructBenchmarking
{
    //1
    //public static void WriteAllNumbersFromText(string text)
    //{
    //    var pos = 0;
    //    while (true)
    //    {
    //        SkipSpaces(text, ref pos);
    //        var num = ReadNumber(text, ref pos);
    //        if (num == "")
    //            break;
    //        Console.Write(num + " ");
    //    }
    //    Console.WriteLine();
    //}
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var arraysData = Experiments.BuildChartDataForArrayCreation(new Benchmark(), 100);
            var callsData = Experiments.BuildChartDataForMethodCall(new Benchmark(), 1000000);
            var form = CreateChartForm(arraysData, callsData);
            Application.Run(form);
        }

        private static Form CreateChartForm(ChartData arraysData, ChartData callsData)
        {
            var form = new Form {WindowState = FormWindowState.Maximized};
            var chartBuilder = new ChartBuilder();
            var arraysChart = chartBuilder.CreateTimeOfObjectSizeChart(arraysData);
            var callsChart = chartBuilder.CreateTimeOfObjectSizeChart(callsData);
            form.Controls.Add(arraysChart);
            arraysChart.Dock = DockStyle.Top;
            form.Controls.Add(callsChart);
            callsChart.Dock = DockStyle.Bottom;
            form.Resize += (sender, args) => ResizeCharts(form, arraysChart, callsChart);
            form.Shown += (sender, args) => ResizeCharts(form, arraysChart, callsChart);
            return form;
        }

        private static void ResizeCharts(Form form, Control chartArrays, Control chartCalls)
        {
            chartArrays.Height = form.ClientSize.Height / 2;
            chartCalls.Height = form.ClientSize.Height / 2;
        }
    }
}