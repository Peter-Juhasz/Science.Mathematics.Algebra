using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Science.Mathematics.Algebra.Tests
{
    using System.Linq;
    using static ExpressionFactory;

    [TestClass]
    public class PowerExpressionTests
    {
        [TestMethod]
        public void Power_WithBase()
        {
            const int reference = 3;

            var expression = Exponentiate(1, 2);
            var result = expression.WithBase(reference);
            
            Assert.AreEqual(reference, result.Base.GetConstantValue());
        }

        [TestMethod]
        public void Power_WithExponent()
        {
            const int reference = 3;

            var expression = Exponentiate(1, 2);
            var result = expression.WithExponent(reference);

            Assert.AreEqual(reference, result.Exponent.GetConstantValue());
        }

        [TestMethod]
        public void Power_Simplify_WhenExponentEqualsOne()
        {
            var @base = new SymbolExpression("x");

            var expression = Exponentiate(@base, 1);
            var simplifier = new ExponentOneSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual(@base, result);
        }

        [TestMethod]
        public void Power_Simplify_WhenExponentEqualsZero()
        {
            var @base = new SymbolExpression("x");

            var expression = Exponentiate(@base, 0);
            var simplifier = new ExponentZeroSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(NumberExpression));
            Assert.AreEqual(1, result.GetConstantValue());
        }

        [TestMethod]
        public void Power_Simplify_WhenBaseEqualsZero()
        {
            var expression = Exponentiate(0, 2);
            var simplifier = new BaseZeroSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(NumberExpression));
            Assert.AreEqual(0, result.GetConstantValue());
        }

        [TestMethod]
        public void Power_Simplify_WhenBaseEqualsOne()
        {
            var exponent = new SymbolExpression("x");

            var expression = Exponentiate(1, exponent);
            var simplifier = new BaseOneSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(NumberExpression));
            Assert.AreEqual(1, result.GetConstantValue());
        }

        [TestMethod]
        public void Power_Simplify_Nested()
        {
            var expression = Exponentiate(Exponentiate("x", "y"), "z");
            var simplifier = new NestedPowerSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.AreEqual("x ^ (y * z)", result.ToString());
        }

        [TestMethod]
        public void Power_Simplify_Constant()
        {
            var expression = Exponentiate(2, 3);
            var simplifier = new ConstantPowerSimplifier();
            var result = simplifier.Simplify(expression, CancellationToken.None);

            Assert.IsInstanceOfType(result, typeof(NumberExpression));
            Assert.AreEqual(8, result.GetConstantValue());
        }

        [TestMethod]
        public void Power_GetConstantValue()
        {
            var expression = Exponentiate(3, 5);
            Assert.AreEqual(243, expression.GetConstantValue());
        }

        [TestMethod]
        public void Power_Base_IsInfinity()
        {
            var expression = Exponentiate(Infinity(), 1);

            Assert.IsTrue(expression.IsInfinity());
        }

        [TestMethod]
        public void Power_Exponent_IsInfinity()
        {
            var expression = Exponentiate(1, Infinity());

            Assert.IsTrue(expression.IsInfinity());
        }

        [TestMethod]
        public void Power_Match_x2_To_a2()
        {
            var x = Symbol("x");
            var expression = x ^ 2;

            var a = Symbol("a");
            var match = expression.Match(a ^ 2).First();
            Assert.AreEqual(x, match[a]);
        }

        [TestMethod]
        public void Power_Match_23_to_ab()
        {
            var expression = Number(2) ^ 3;

            var a = Symbol("a");
            var b = Symbol("b");
            var match = expression.Match(a ^ b).First();
            Assert.AreEqual(Number(2), match[a]);
            Assert.AreEqual(Number(3), match[b]);
        }

        [TestMethod]
        public void Power_Match_23_to_a3()
        {
            var expression = Number(2) ^ 3;

            var a = Symbol("a");
            var match = expression.Match(a ^ 3).First();
            Assert.AreEqual(Number(2), match[a]);
        }

        [TestMethod]
        public void Power_Match_xy_to_ab()
        {
            var x = Symbol("x");
            var y = Symbol("y");
            var expression = x ^ y;

            var a = Symbol("a");
            var b = Symbol("b");
            var match = expression.Match(a ^ b).First();
            Assert.AreEqual(x, match[a]);
            Assert.AreEqual(y, match[b]);
        }

        [TestMethod]
        public void Power_Match_33_to_aa()
        {
            var n = Number(3);
            var expression = n ^ n;

            var a = Symbol("a");
            var match = expression.Match(a ^ a).First();
            Assert.AreEqual(n, match[a]);
        }

        [TestMethod]
        public void Power_ToString_Constants()
        {
            var expression = Exponentiate(3, 5);
            Assert.AreEqual("3 ^ 5", expression.ToString());
        }
    }
}
