using System.Globalization;
using Greedy.Architecture;
using Greedy.Properties;
using NUnit.Framework;

namespace Greedy.Tests
{
    [TestFixture]
    public class GreedyPathFinder_Should
    {
        [TestCase("no_walls_1")]
        [TestCase("no_walls_2")]
        [TestCase("no_walls_3")]
        [TestCase("maze_1")]
        [TestCase("maze_2")]
        [TestCase("maze_3")]
        [TestCase("maze_4")]
        public void WinGame_With_State(string stateName)
        {
            var controller =
                TestsHelper.LoadStateFromInputData_And_MoveThroughPath(
                    Resources.ResourceManager.GetString(stateName, CultureInfo.InvariantCulture),
                    new GreedyPathFinder());

            
            Assert.True(controller.GameIsWon, controller.GameLostMessage);
        }

        [TestCase("maze_no_energy")]
        [TestCase("maze_no_chests")]
        public void ReturnEmptyPath_WhenCantWinGame(string stateName)
        {
            var state = StatesLoader.LoadStateFromInputData(Resources.ResourceManager.GetString(stateName, CultureInfo.InvariantCulture));
            var path = new GreedyPathFinder().FindPathToCompleteGoal(state);
            Assert.IsEmpty(path);
        }
    }
}