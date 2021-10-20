using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tickets
{
	[TestFixture]
    public class TicketsTask_should
    {
        public void MakeTest(int halfLen, int totalSum, string answer)
        {
            var result = BigInteger.Parse(answer);
            Assert.AreEqual(result, TicketsTask.Solve(halfLen, totalSum));
        }

        [Test]
        public void Test11() { MakeTest(2, 2, "4"); }
        [Test]
        public void Test21() { MakeTest(2, 3, "0"); }
        [Test]
        public void Test12() { MakeTest(2, 5, "0"); }
        [Test]
        public void Test16() { MakeTest(10, 10, "4008004"); }
        [Test]
        public void Test17() { MakeTest(20, 20, "401200499400100"); }
        [Test]
        public void Test1() { MakeTest(2, 2, "4"); }
        [Test]
        public void Test31() { MakeTest(2, 4, "9"); }
        [Test]
        public void Test32() { MakeTest(2, 6, "16"); }
        [Test]
        public void Test33() { MakeTest(2, 8, "25"); }
        [Test]
        public void Test34() { MakeTest(4, 4, "100"); }
        [Test]
        public void Test35() { MakeTest(4,6, "400"); }
        [Test]
        public void Test36() { MakeTest(4, 8, "1225"); }
        [Test]
        public void Test3() { MakeTest(1, 18, "1"); }
        [Test]
        public void Test4() { MakeTest(2, 20, "81"); }
        [Test]
        public void Test40() { MakeTest(3, 4, "36"); }
        [Test]
        public void Test41() { MakeTest(50, 250, "41409732703108026572448942476041207839411119408173570127621420289526490759097327483136"); }
        [Test]
        public void Test42() { MakeTest(50, 300, "3660144008320861404705392514284143308670326732628084621145897204106027006076468826991924409"); }
        [Test]
        public void Test5() { MakeTest(3, 54, "1"); }
        [Test]
        public void Test6() { MakeTest(10, 10, "4008004"); }
        [Test]
        public void Test7() { MakeTest(20, 20, "401200499400100"); }
        [Test]
        public void Test9() { MakeTest(50, 50, "1228576490354119571810717714807944388004"); }
    }
}
