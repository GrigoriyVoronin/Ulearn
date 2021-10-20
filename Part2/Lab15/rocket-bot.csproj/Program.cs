using System;
using System.Threading;
using System.Windows.Forms;

namespace rocket_bot
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var channel = new Channel<Rocket>();
            var random = new Random(223243);
            var level = LevelsFactory.CreateLevel(random);
            channel.AppendIfLastItemIsUnchanged(level.InitialRocket, null);
            var bot = new Bot(level.Clone(), channel, 45, 1000, random, 2);
            var thread = new Thread(() => bot.RunInfiniteLoop()) {IsBackground = true};
            thread.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new GameForm(level.Clone(), channel);
            Application.Run(form);
        }
    }
}