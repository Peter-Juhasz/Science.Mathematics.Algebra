using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests;

[TestClass]
public class AbsoluteValueExpressionTests
{
	[TestMethod]
	public void AbsoluteValue_Positive()
	{
		const int reference = 5;

		var expression = ExpressionFactory.AbsoluteValue(reference);
		var result = expression.GetConstantValue();

		Assert.AreEqual(reference, result);
	}

	[TestMethod]
	public void AbsoluteValue_Negative()
	{
		const int reference = -5;

		var expression = ExpressionFactory.AbsoluteValue(reference);
		var result = expression.GetConstantValue();

		Assert.AreEqual(5, result);
	}

	[TestMethod]
	public void AbsoluteValue_ToString()
	{
		const int reference = 3;

		var expression = ExpressionFactory.AbsoluteValue(reference);

		Assert.AreEqual("|3|", expression.ToString());
	}
}
