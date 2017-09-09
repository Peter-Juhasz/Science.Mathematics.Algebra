using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    using System.Linq;
    using static ExpressionFactory;

    [TestClass]
    public class NumberExpressionTests
    {
        [TestMethod]
        public void Number_ConstantValue()
        {
            const int reference = 3;

            var expression = ExpressionFactory.Constant(reference);
            
            Assert.AreEqual(reference, expression.Value);
            Assert.AreEqual(reference, expression.GetConstantValue());
        }

        [TestMethod]
        public void Number_Match()
        {
            var expression = Number(3);
            Assert.IsTrue(expression.Match(3).Any());
        }

        [TestMethod]
        public void Number_Match_Sum()
        {
            var expression = Number(3) + Number(5);
            Assert.IsTrue(expression.Match(8).Any());
        }

        [TestMethod]
        public void Number_Substitute()
        {
            const int reference = 5;

            var constant = ExpressionFactory.Constant(reference);
            var result = constant.Substitute("x", 1);

            Assert.AreEqual(constant, result);
        }

        [TestMethod]
        public void Number_Equals()
        {
            const int reference = 5;

            var expression1 = ExpressionFactory.Constant(reference);
            var expression2 = ExpressionFactory.Constant(reference);

            Assert.AreEqual(expression1, expression2);
        }

        [TestMethod]
        public void Number_ToString()
        {
            const int reference = 3;

            var expression = ExpressionFactory.Constant(reference);

            Assert.AreEqual("3", expression.ToString());
        }
    }
}
