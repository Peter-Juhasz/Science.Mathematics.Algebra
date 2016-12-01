using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class CosineExpressionTests
    {
        [TestMethod]
        public void Cosine_Factory()
        {
            var expression = ExpressionFactory.Cosine(0);
            Assert.IsInstanceOfType(expression, typeof(CosineFunctionExpression));
        }
    }
}
