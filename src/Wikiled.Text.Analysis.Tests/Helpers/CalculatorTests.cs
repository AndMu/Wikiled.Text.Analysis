using NUnit.Framework;
using Wikiled.Text.Analysis.Helpers;

namespace Wikiled.Text.Analysis.Tests.Helpers
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void Add()
        {
            object value1 = 1;
            object value2 = 2;
            var result = Calculator<object>.Add(value1, value2);
            Assert.AreEqual(3, result);
        }
    }
}
