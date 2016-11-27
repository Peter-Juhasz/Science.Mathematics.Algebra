using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests
{
    [TestClass]
    public class VariableExpressionTests
    {
        [TestMethod]
        public void Variable_Name()
        {
            const string reference = "x";

            var expression = ExpressionFactory.Variable(reference);

            Assert.AreEqual(reference, expression.Name);
        }

        [TestMethod]
        public void Variable_Equals()
        {
            const string reference = "x";

            var expression1 = ExpressionFactory.Variable(reference);
            var expression2 = ExpressionFactory.Variable(reference);

            Assert.AreEqual(expression1, expression2);
        }

        [TestMethod]
        public void Variable_ToString()
        {
            const string reference = "x";

            var expression = ExpressionFactory.Variable(reference);

            Assert.AreEqual(reference, expression.ToString());
        }
    }
}
