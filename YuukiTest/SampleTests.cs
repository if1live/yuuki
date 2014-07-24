using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Yuuki;

namespace YuukiTest
{
    public class SampleTests
    {
        [TestCase]
        public void TestFoo()
        {
            int a = Helper.Foo();
            Assert.AreEqual(a, 1);
        }
    }
}
