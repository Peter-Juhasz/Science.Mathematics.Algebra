using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class SumExpressionTests
    {
        [TestMethod]
        public void Sum_Zero()
        {
            var expression = ExpressionFactory.Sum(2, 0, "x");
            var simplifier = new AdditionWithZeroSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);
            
            Assert.AreEqual("2 + x", result.ToString());
        }

        [TestMethod]
        public void Sum_ToString()
        {
            var expression = ExpressionFactory.Sum(2, "x");

            Assert.AreEqual("2 + x", expression.ToString());
        }
    }
}
