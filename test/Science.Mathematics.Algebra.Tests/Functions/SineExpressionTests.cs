using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class SineExpressionTests
    {
        [TestMethod]
        public void Sine_Factory()
        {
            var expression = ExpressionFactory.Sine(0);
            Assert.IsInstanceOfType(expression, typeof(SineFunctionExpression));
        }

        [TestMethod]
        public void Sine_Differentiate_Variable()
        {
            var expression = ExpressionFactory.Sine("x");
            var result = expression.Differentiation("x").Simplify();

            Assert.AreEqual("cos(x)", result.ToString());
        }
    }
}
