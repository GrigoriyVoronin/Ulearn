using NUnit.Framework;

namespace TodoApplication
{
    [TestFixture]
    public class LimitedSizeStack_PerformanceTest
    {
        [Test, Timeout(500), Description("Push должен работать быстро, даже при большом лимите на размер стека")]
        public void Push_ShouldTakeConstantTime()
        {
            int limit = 100000;
            var stack = new LimitedSizeStack<int>(limit);
            for (int i = 0; i < 5 * limit; ++i)
            {
                stack.Push(0);
            }
            Assert.AreEqual(limit, stack.Count);
        }

        [Test, Timeout(500), Description("Pop должен работать быстро, даже при большом лимите на размер стека")]
        public void Pop_ShouldTakeConstantTime()
        {
            int limit = 200000;
            var stack = new LimitedSizeStack<int>(limit);
            for (int i = 0; i < limit; ++i)
            {
                stack.Push(0);
            }
            Assert.AreEqual(limit, stack.Count);
            for (int i = 0; i < limit; ++i)
            {
                stack.Pop();
            }
            Assert.AreEqual(0, stack.Count);
        }
    }
}
