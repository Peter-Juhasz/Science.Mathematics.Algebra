using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Science.Mathematics.Algebra.Tests;

using static ExpressionFactory;

[TestClass]
public class ProductExpressionTests
{
	[TestMethod]
	public void Product_One()
	{
		var expression = Product(2, 1, "x");
		var simplifier = new MultiplicationByOneSimplifier();
		var result = simplifier.Simplify(expression, CancellationToken.None);

		Assert.AreEqual("2 * x", result.ToString());
	}

	[TestMethod]
	public void Product_Zero()
	{
		var expression = Product(2, 0, "x");
		var simplifier = new MultiplicationByZeroSimplifier();
		var result = simplifier.Simplify(expression, CancellationToken.None);

		Assert.AreEqual(Zero, result);
	}

	[TestMethod]
	public void Product_CollectConstants()
	{
		var x = Symbol("x");
		var expression = Product(2, 5, x, 6);
		var simplifier = new CollectNumbersInProductSimplifier();
		var result = simplifier.Simplify(expression, CancellationToken.None);

		Assert.AreEqual(60 * x, result);
	}

	[TestMethod]
	public void Product_CollectExponents_Single()
	{
		var x = Symbol("x");
		var expression = Product(x ^ 2, x ^ 3);
		var simplifier = new CollectExponentsInProductSimplifier();
		var result = simplifier.Simplify(expression, CancellationToken.None);

		Assert.AreEqual("x ^ (2 + 3)", result.ToString());
	}

	[TestMethod]
	public void Product_CollectExponents_Double()
	{
		var x = Symbol("x");
		var y = Symbol("y");
		var expression = Product(x ^ 2, y ^ 3, x ^ 3, y ^ 1);
		var simplifier = new CollectExponentsInProductSimplifier();
		var result = simplifier.Simplify(expression, CancellationToken.None);

		Assert.AreEqual("x ^ (2 + 3) * y ^ (3 + 1)", result.ToString());
	}

	[TestMethod]
	public void Product_CollectExponents_WithOneExponent()
	{
		var x = Symbol("x");
		var y = Symbol("y");
		var expression = Product(5, x ^ 2, y ^ 0, x ^ 3, y ^ 1);
		var simplifier = new CollectExponentsInProductSimplifier();
		var result = simplifier.Simplify(expression, CancellationToken.None);

		Assert.AreEqual("5 * x ^ (2 + 3) * y", result.ToString());
	}

	[TestMethod]
	public void Product_Numerator_General()
	{
		var x = Symbol("x");
		var expression = 2 * x;
		var result = expression.GetNumerator();

		Assert.AreEqual(expression, result);
	}

	[TestMethod]
	public void Product_Numerator()
	{
		var a = Symbol("a");
		var b = Symbol("b");
		var expression = a / b;
		var result = expression.GetNumerator();

		Assert.AreEqual(a, result);
	}

	[TestMethod]
	public void Product_Denominator()
	{
		var a = Symbol("a");
		var b = Symbol("b");
		var expression = a / b;
		var result = expression.GetDenominator();

		Assert.AreEqual(b, result);
	}

	[TestMethod]
	public void Product_IsInfinity()
	{
		var expression = 2 * Infinity();

		Assert.IsTrue(expression.IsInfinity());
	}

	[TestMethod]
	public void Product_ToString()
	{
		var x = Symbol("x");
		var expression = Product(2, x);

		Assert.AreEqual(2 * x, expression);
	}
}
