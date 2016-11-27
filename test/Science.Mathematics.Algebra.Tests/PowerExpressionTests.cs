using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class PowerExpressionTests
    {
        [TestMethod]
        public void Power_WithBase()
        {
            const int reference = 3;

            var expression = ExpressionFactory.Exponentiate(1, 2);
            var result = expression.WithBase(reference);
            
            Assert.AreEqual(reference, result.Base.GetConstantValue());
        }

        [TestMethod]
        public void Power_WithExponent()
        {
            const int reference = 3;

            var expression = ExpressionFactory.Exponentiate(1, 2);
            var result = expression.WithExponent(reference);

            Assert.AreEqual(reference, result.Exponent.GetConstantValue());
        }

        [TestMethod]
        public void Power_Simplify_WhenExponentEqualsOne()
        {
            var @base = new VariableExpression("x");

            var expression = ExpressionFactory.Exponentiate(@base, 1);
            var simplifier = new ExponentOneSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual(@base, result);
        }

        [TestMethod]
        public void Power_Simplify_WhenExponentEqualsZero()
        {
            var @base = new VariableExpression("x");

            var expression = ExpressionFactory.Exponentiate(@base, 0);
            var simplifier = new ExponentZeroSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(1, result.GetConstantValue());
        }

        [TestMethod]
        public void Power_Simplify_WhenBaseEqualsZero()
        {
            var expression = ExpressionFactory.Exponentiate(0, 2);
            var simplifier = new BaseZeroSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(0, result.GetConstantValue());
        }

        [TestMethod]
        public void Power_Simplify_WhenBaseEqualsOne()
        {
            var exponent = new VariableExpression("x");

            var expression = ExpressionFactory.Exponentiate(1, exponent);
            var simplifier = new BaseOneSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(1, result.GetConstantValue());
        }

        [TestMethod]
        public void Power_GetConstantValue()
        {
            var expression = ExpressionFactory.Exponentiate(3, 5);
            Assert.AreEqual(243, expression.GetConstantValue());
        }

        [TestMethod]
        public void Power_ToString_Constants()
        {
            var expression = ExpressionFactory.Exponentiate(3, 5);
            Assert.AreEqual("3 ^ 5", expression.ToString());
        }
    }
}
