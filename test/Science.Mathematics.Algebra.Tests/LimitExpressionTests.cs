using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    using static ExpressionFactory;

    [TestClass]
    public class LimitExpressionTests
    {
        [TestMethod]
        public void Limit_ConstantValue_Constant()
        {
            const int reference = 5;

            var expression = Limit(reference, Variable("x"), Infinity());
            var result = expression.GetConstantValue();

            Assert.AreEqual(reference, result);
        }

        [TestMethod]
        public void Limit_ConstantValue_SubstituteFiniteAmount()
        {
            const int reference = 5;

            var @var = Variable("x");
            var expression = Limit(@var, @var, reference);
            var result = expression.GetConstantValue();

            Assert.AreEqual(reference, result);
        }

        [TestMethod]
        public void Limit_Simplify_Constant()
        {
            const int reference = 5;

            var expression = Limit(reference, Variable("x"), Infinity());
            var result = expression.Simplify();

            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(reference, result);
        }

        [TestMethod]
        public void Limit_Simplify_SubstituteFiniteAmount()
        {
            const int reference = 5;

            var @var = Variable("x");
            var expression = Limit(@var, @var, reference);
            var result = expression.Simplify();

            Assert.IsInstanceOfType(result, typeof(ConstantExpression));
            Assert.AreEqual(reference, result);
        }

        [TestMethod]
        public void Limit_Simplify_SumWithConstant()
        {
            const int reference = 5;

            var x = Variable("x");
            var fx = Invoke("f", x);
            var expression = Limit(reference + fx, x, Infinity());
            var result = expression.Simplify();

            Assert.AreEqual(Limit(fx, x, Infinity()) + reference, result);
        }

        [TestMethod]
        public void Limit_Simplify_ProductWithConstant()
        {
            const int reference = 5;

            var x = Variable("x");
            var fx = Invoke("f", x);
            var expression = Limit(reference * fx, x, Infinity());
            var result = expression.Simplify();

            Assert.AreEqual(reference * Limit(fx, x, Infinity()), result);
        }
    }
}
