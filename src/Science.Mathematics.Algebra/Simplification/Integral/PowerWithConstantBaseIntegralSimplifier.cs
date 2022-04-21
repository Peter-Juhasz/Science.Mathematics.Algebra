using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Simplifies expressions like int a^x dx to a^x / ln a + C.
/// </summary>
internal sealed class PowerWithConstantBaseIntegralSimplifier : ISimplifier<IntegralExpression>
{
	public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
	{
		if (expression.Expression is PowerExpression body)
		{
			if (body.Exponent == expression.RespectTo &&
				body.Base.IsConstant(expression))
				return expression.Expression / NaturalLogarithm(body.Base) + IntegralConstant;
		}

		return expression;
	}
}
