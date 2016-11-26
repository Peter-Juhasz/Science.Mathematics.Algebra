using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class ConstantExpressionTests
    {
        [TestMethod]
        public void Constant_ConstantValue()
        {
            const int reference = 3;

            var expression = ExpressionFactory.Constant(reference);
            
            Assert.AreEqual(reference, expression.Value);
            Assert.AreEqual(reference, expression.GetConstantValue());
        }

        [TestMethod]
        public void Constant_ToString()
        {
            const int reference = 3;

            var expression = ExpressionFactory.Constant(reference);

            Assert.AreEqual("3", expression.ToString());
        }
    }
}
