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
    }
}
