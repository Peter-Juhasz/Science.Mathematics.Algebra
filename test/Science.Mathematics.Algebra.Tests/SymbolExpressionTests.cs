using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests;

using static ExpressionFactory;

[TestClass]
public class SymbolExpressionTests
{
	[TestMethod]
	public void Symbol_Name()
	{
		const string reference = "x";

		var expression = ExpressionFactory.Symbol(reference);

		Assert.AreEqual(reference, expression.Name);
	}

	[TestMethod]
	public void Symbol_Substitute()
	{
		const string name = "x";
		const int referenceValue = 1;
		var reference = Constant(referenceValue);

		var variable = Symbol(name);
		var result = variable.Substitute("x", referenceValue);

		Assert.AreEqual(reference, result);
	}

	[TestMethod]
	public void Symbol_Equals()
	{
		const string reference = "x";

		var expression1 = Symbol(reference);
		var expression2 = Symbol(reference);

		Assert.AreEqual(expression1, expression2);
	}

	[TestMethod]
	public void Symbol_ToString()
	{
		const string reference = "x";

		var expression = Symbol(reference);

		Assert.AreEqual(reference, expression.ToString());
	}
}
