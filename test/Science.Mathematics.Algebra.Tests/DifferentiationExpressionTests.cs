using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class DifferentiationExpressionTests
    {
        [TestMethod]
        public void Differentiation_Power_x2()
        {
            var x = ExpressionFactory.Variable("x");
            var expression = x ^ 2;
            var result = expression.Differentiate(x).Simplify();

            Assert.AreEqual(2 * x, result);
        }

        [TestMethod]
        public void Differentiation_Power_ex()
        {
            var x = ExpressionFactory.Variable("x");
            var expression = ExpressionFactory.E ^ x;
            var result = expression.Differentiate(x).Simplify();

            Assert.AreEqual(expression, result);
        }
    }
}
