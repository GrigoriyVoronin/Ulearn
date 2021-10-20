using System;
using System.Diagnostics;
using NUnit.Framework;

namespace rocket_bot
{
    [TestFixture]
    public class BotTests
    {
        [SetUp]
        public void Init()
        {
            random = new Random(223243);
            channel = new Channel<Rocket>();
            rocket = new Rocket(new Vector(100, 100), Vector.Zero, -0.5 * Math.PI);
        }

        private Random random;
        private Channel<Rocket> channel;
        private Rocket rocket;
        private const int PerformanceIterations = 200;
        private const int PerformanceMoves = 35;
        private const int CorrectnessIterations = 50;
        private const int CorrectnessMoves = 4;


        private void CheckAlmostAlwaysEquals(Rocket expectedResult, Bot bot, Rocket initialRocket, int times = 1000)
        {
            var successes = 0;
            for (var i = 0; i < times; ++i)
            {
                var actualResult = bot.GetNextMove(initialRocket);
                if (actualResult.Equals(expectedResult)) ++successes;
            }

            Assert.GreaterOrEqual(successes, 0.8 * times);
        }

        private void ComparePerformances(Bot singleThreadBot, Bot multiThreadBot, Rocket initialRocket, int times = 40)
        {
            var successes = 0;
            var stopWatch = new Stopwatch();
            for (var i = 0; i < times; ++i)
            {
                stopWatch.Restart();
                singleThreadBot.GetNextMove(initialRocket);
                stopWatch.Stop();
                var singleThreadBotTime = stopWatch.Elapsed;
                stopWatch.Restart();
                multiThreadBot.GetNextMove(initialRocket);
                stopWatch.Stop();
                var multiThreadBotTime = stopWatch.Elapsed;
                if (multiThreadBotTime < singleThreadBotTime) ++successes;
            }

            Assert.GreaterOrEqual(successes, 0.8 * times,
                $"Ваше решение в два потока должно работать быстрее решения в один поток в {times * 0.8} случаев из {times}");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(0)]
        public void TestCanTakeTarget(int threadCount)
        {
            var level = new Level(rocket, new[] {new Vector(100, 90), new Vector(0, 0)}, LevelsFactory.StandardPhysics);
            var expectedResult = level.InitialRocket.Move(Turn.Left, level); // поворачиваем к (0,0), через лево короче.
            RunTestCase(threadCount, level, expectedResult);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(0)]
        public void TestCannotTakeTarget(int threadCount)
        {
            var level = new Level(rocket, new[] {new Vector(0, 100), new Vector(0, 0)}, LevelsFactory.StandardPhysics);
            var expectedResult = level.InitialRocket.Move(Turn.Left, level); // поворачиваем к (0, 100)
            RunTestCase(threadCount, level, expectedResult);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(0)]
        public void TestGoToNextTarget(int threadCount)
        {
            var level = new Level(rocket, new[] {new Vector(99, 79), new Vector(120, 79)},
                LevelsFactory.StandardPhysics);
            var expectedResult =
                level.InitialRocket.Move(Turn.Right, level); // первый возьмем по инерции, поворачиваем к (120, 79)
            RunTestCase(threadCount, level, expectedResult);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(0)]
        public void TestGoStraight(int threadCount)
        {
            var level = new Level(rocket, new[] {new Vector(100, 90), new Vector(100, 0)},
                LevelsFactory.StandardPhysics);
            var expectedResult =
                level.InitialRocket.Move(Turn.None, level); // следующие два чекпоинта прямо. Летим прямо.
            RunTestCase(threadCount, level, expectedResult);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(0)]
        public void TestGoToNextCheckpoint(int threadCount)
        {
            rocket = rocket.IncreaseCheckpoints();
            var level = new Level(rocket, new[] {new Vector(200, 100), new Vector(0, 100)},
                LevelsFactory.StandardPhysics);
            var expectedResult =
                level.InitialRocket.Move(Turn.Left, level); // Нам к (0, 100), потому что первый уже взят.
            RunTestCase(threadCount, level, expectedResult);
        }

        private void RunTestCase(int threadCount, Level level, Rocket expectedResult)
        {
            channel.AppendIfLastItemIsUnchanged(level.InitialRocket, null);
            if (threadCount == 0)
            {
                ComparePerformances(new Bot(level, channel, PerformanceMoves, PerformanceIterations, random, 1),
                    new Bot(level, channel, PerformanceMoves, PerformanceIterations, random, 2), level.InitialRocket);
            }
            else
            {
                var bot = new Bot(level, channel, CorrectnessMoves, CorrectnessIterations, random, threadCount);
                CheckAlmostAlwaysEquals(expectedResult, bot, level.InitialRocket);
            }
        }
    }
}