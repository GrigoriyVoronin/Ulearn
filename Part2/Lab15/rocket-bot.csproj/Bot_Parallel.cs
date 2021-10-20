using System;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var tasks = new Task<Tuple<Turn, double>>[threadsCount];
            for (var i = 0; i < threadsCount; i++)
                tasks[i] = Task.Run(()
                    => SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount));
            for (var i = 0; i < threadsCount; i++)
                tasks[i].Wait();
            var max = double.MinValue;
            var turn = Turn.None;
            for (var i = 0; i < threadsCount; i++)
                if (tasks[i].Result.Item2 > max)
                {
                    max = tasks[i].Result.Item2;
                    turn = tasks[i].Result.Item1;
                }

            return rocket.Move(turn, level);
        }
    }
}