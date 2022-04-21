using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Mathematics.Algebra.Tests;

[TestClass]
public class FunctionInvocationExpressionTests
{
	[TestMethod]
	public void Function_ToString_NoArguments()
	{
		var expression = ExpressionFactory.Invoke("f");

		Assert.AreEqual("f()", expression.ToString());
	}

	[TestMethod]
	public void Function_ToString_OneArguments()
	{
		var expression = ExpressionFactory.Invoke("f", 2);

		Assert.AreEqual("f(2)", expression.ToString());
	}

	[TestMethod]
	public void Function_ToString_MultipleArguments()
	{
		var expression = ExpressionFactory.Invoke("f", 2, "x");

		Assert.AreEqual("f(2, x)", expression.ToString());
	}
}
