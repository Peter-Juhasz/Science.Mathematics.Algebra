using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class ProductExpressionTests
    {
        [TestMethod]
        public void Product_One()
        {
            var expression = ExpressionFactory.Product(2, 1, "x");
            var simplifier = new MultiplicationByOneSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);
            
            Assert.AreEqual("2 * x", result.ToString());
        }

        [TestMethod]
        public void Product_Zero()
        {
            var expression = ExpressionFactory.Product(2, 0, "x");
            var simplifier = new MultiplicationByZeroSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual(ExpressionFactory.Zero, result);
        }

        [TestMethod]
        public void Product_CollectConstants()
        {
            var expression = ExpressionFactory.Product(2, 5, "x", 6);
            var simplifier = new CollectConstantsInProductSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("60 * x", result.ToString());
        }

        [TestMethod]
        public void Product_CollectExponents_Single()
        {
            var x = ExpressionFactory.Variable("x");
            var expression = ExpressionFactory.Product(x ^ 2, x ^ 3);
            var simplifier = new CollectExponentsInProductSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("x ^ (2 + 3)", result.ToString());
        }

        [TestMethod]
        public void Product_CollectExponents_Double()
        {
            var x = ExpressionFactory.Variable("x");
            var y = ExpressionFactory.Variable("y");
            var expression = ExpressionFactory.Product(x ^ 2, y ^ 3, x ^ 3, y ^ 1);
            var simplifier = new CollectExponentsInProductSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("x ^ (2 + 3) * y ^ (3 + 1)", result.ToString());
        }

        [TestMethod]
        public void Product_CollectExponents_WithOneExponent()
        {
            var x = ExpressionFactory.Variable("x");
            var y = ExpressionFactory.Variable("y");
            var expression = ExpressionFactory.Product(5, x ^ 2, y ^ 0, x ^ 3, y ^ 1);
            var simplifier = new CollectExponentsInProductSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("5 * x ^ (2 + 3) * y", result.ToString());
        }

        [TestMethod]
        public void Product_ToString()
        {
            var expression = ExpressionFactory.Product(2, "x");

            Assert.AreEqual("2 * x", expression.ToString());
        }
    }
}
