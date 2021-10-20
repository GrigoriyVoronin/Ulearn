using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TodoApplication
{
    [TestFixture]
    class ListModel_PerformanceTest
    {
        [Test, Timeout(500)]
        [Description("Не нужно хранить все состояния модели")]
        public void AntiStupidTest()
        {
            int limit = 30000;
            var model = new ListModel<int>(limit);
            for (int i = 0; i < limit; ++i)
            {
                model.AddItem(0);
            }
            Assert.AreEqual(limit, model.Items.Count);
        }
    }
}
