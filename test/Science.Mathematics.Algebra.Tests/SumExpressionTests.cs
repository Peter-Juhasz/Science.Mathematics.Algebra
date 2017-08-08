using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Science.Mathematics.Algebra.Tests
{
    using static ExpressionFactory;

    [TestClass]
    public class SumExpressionTests
    {
        [TestMethod]
        public void Sum_Zero()
        {
            var x = Symbol("x");
            var expression = Sum(2, 0, x);
            var simplifier = new AdditionWithZeroSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);
            
            Assert.AreEqual(2 + x, result);
        }

        [TestMethod]
        public void Sum_CollectConstants()
        {
            var x = Symbol("x");
            var expression = Sum(2, 5, x);
            var simplifier = new CollectConstantsInSumSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual(x + 7, result);
        }

        [TestMethod]
        public void Sum_CollectConstants_OnlyConstants()
        {
            var expression = Sum(2, 5, 7);
            var simplifier = new CollectConstantsInSumSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(14, result.GetConstantValue());
        }

        [TestMethod]
        public void Sum_CollectConstants_SingleExpressionRemains()
        {
            var expression = Sum(2, 5, -7, "x");
            var simplifier = new CollectConstantsInSumSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(SymbolExpression));
        }

        [TestMethod]
        public void Sum_CollectCoefficients()
        {
            var x = Symbol("x");
            var expression = Sum(5 * x, 8 * x);
            var simplifier = new CollectCoefficientsInSumSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("13 * x", result.ToString());
        }

        [TestMethod]
        public void Sum_CollectCoefficients_WithoutCoefficient()
        {
            var x = Symbol("x");
            var expression = Sum(x, 8 * x);
            var simplifier = new CollectCoefficientsInSumSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("9 * x", result.ToString());
        }

        [TestMethod]
        public void Sum_CollectCoefficients_WithConstant()
        {
            var x = Symbol("x");
            var expression = Sum(5 * x, 8 * x, 3);
            var simplifier = new CollectCoefficientsInSumSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("13 * x + 3", result.ToString());
        }

        [TestMethod]
        public void Sum_CollectCoefficients_Multiple()
        {
            var x = Symbol("x");
            var y = Symbol("y");
            var expression = Sum(5 * x, 8 * x, 4 * y, 3 * x * y);
            var simplifier = new CollectCoefficientsInSumSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("13 * x + 4 * y + 3 * x * y", result.ToString());
        }

        [TestMethod]
        public void Sum_IsInfinity()
        {
            var expression = 2 + Infinity();

            Assert.IsTrue(expression.IsInfinity());
        }

        [TestMethod]
        public void Sum_ToString()
        {
            var x = Symbol("x");
            var expression = Sum(2, x);

            Assert.AreEqual(2 + x, expression);
        }
    }
}
