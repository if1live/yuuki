using System.Collections;
using NUnit.Framework;
using Yuuki;

namespace YuukiTest
{
    [TestFixture]
    [Category("MathHelper")]
    internal class MathHelperTests
    {

        [Test]
        public void TestIsPowerOfTwo()
        {
            //true
            Assert.AreEqual(true, MathHelper.IsPowOfTwo(2));
            Assert.AreEqual(true, MathHelper.IsPowOfTwo(4));
            Assert.AreEqual(true, MathHelper.IsPowOfTwo(8));

            Assert.AreEqual(false, MathHelper.IsPowOfTwo(3));
            Assert.AreEqual(false, MathHelper.IsPowOfTwo(5));
            Assert.AreEqual(false, MathHelper.IsPowOfTwo(7));
        }
    }

}
