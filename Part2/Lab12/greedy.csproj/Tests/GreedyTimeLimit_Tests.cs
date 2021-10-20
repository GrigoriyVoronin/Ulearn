using System.Globalization;
using Greedy.Architecture;
using Greedy.Properties;
using NUnit.Framework;

namespace Greedy.Tests
{
    [TestFixture]
    public class GreedyTimeLimit_Tests : BasePerformanceTests
    {
        private const int timeLimit = 1000;

        [TestCase("no_walls_time_limit")]
        [TestCase("maze_time_limit")]
        public void Improved_GreedyPathFinder_Should_Not_Exceed_TimeLimit(string stateName)
        {
            string inputData = Resources.ResourceManager.GetString(stateName, CultureInfo.InvariantCulture);
            AssertWonWithTimout(timeLimit, new StateTestCase { InputData = inputData, Name = stateName }, new GreedyPathFinder());
        }
    }
}