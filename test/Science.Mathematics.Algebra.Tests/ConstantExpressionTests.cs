﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void Constant_Substitute()
        {
            const int reference = 5;

            var constant = ExpressionFactory.Constant(reference);
            var result = constant.Substitute("x", 1);

            Assert.AreEqual(constant, result);
        }

        [TestMethod]
        public void Constant_Equals()
        {
            const int reference = 5;

            var expression1 = ExpressionFactory.Constant(reference);
            var expression2 = ExpressionFactory.Constant(reference);

            Assert.AreEqual(expression1, expression2);
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
