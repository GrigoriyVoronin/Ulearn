using System;
using System.Linq;
using System.Windows.Forms;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var initialStateName = args.Any() ? args[0] : "not_greedy_no_walls_3";

            Application.Run(new GreedyWindow(initialStateName));
        }
    }
}