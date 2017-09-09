using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    using static ExpressionFactory;

    [TestClass]
    public class AbsoluteValueExpressionTests
    {
        [TestMethod]
        public void AbsoluteValue_Positive()
        {
            const int reference = 5;

            var expression = AbsoluteValue(reference);
            var result = expression.GetConstantValue();

            Assert.AreEqual(reference, result);
        }

        [TestMethod]
        public void AbsoluteValue_Negative()
        {
            const int reference = -5;

            var expression = AbsoluteValue(reference);
            var result = expression.GetConstantValue();

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void AbsoluteValue_Match()
        {
            var x = Symbol("x");
            var inner = 2 * x + 4;
            var expression = AbsoluteValue(inner);

            var a = Symbol("a");
            var match = expression.Match(AbsoluteValue(a)).First();
            Assert.AreEqual(inner, match[a]);
        }

        [TestMethod]
        public void AbsoluteValue_ToString()
        {
            const int reference = 3;

            var expression = AbsoluteValue(reference);

            Assert.AreEqual("|3|", expression.ToString());
        }
    }
}
