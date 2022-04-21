using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests;

using static ExpressionFactory;

[TestClass]
public class IntegralExpressionTests
{
	[TestMethod]
	public void Integral_Constant()
	{
		var x = Symbol("x");
		var k = Symbol("k");
		var expression = k;
		var result = expression.Integrate(x).Simplify();
		var expected = k * x + IntegralConstant;

		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void Integral_Sine_x()
	{
		var x = Symbol("x");
		var expression = Sine(x);
		var result = expression.Integrate(x).Simplify();
		var expected = -Cosine(x) + IntegralConstant;

		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void Integral_Cosine_x()
	{
		var x = Symbol("x");
		var expression = Cosine(x);
		var result = expression.Integrate(x).Simplify();
		var expected = Sine(x) + IntegralConstant;

		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void Integral_DerivativeOfDenominatorInNumerator()
	{
		var x = Symbol("x");
		var expression = (2 * x) / (x ^ 2);
		var result = expression.Integrate(x).Simplify();
		var expected = NaturalLogarithm(AbsoluteValue(x ^ 2)) + IntegralConstant;

		Assert.AreEqual(expected, result);
	}
}
