using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests;

using static ExpressionFactory;

[TestClass]
public class InfinityExpressionTests
{
	[TestMethod]
	public void Infinity_ToString()
	{
		var expression = Infinity();

		Assert.AreEqual("∞", expression.ToString());
	}

	[TestMethod]
	public void Infinity_IsInfinity()
	{
		var expression = Infinity();

		Assert.IsTrue(expression.IsInfinity());
	}
}
