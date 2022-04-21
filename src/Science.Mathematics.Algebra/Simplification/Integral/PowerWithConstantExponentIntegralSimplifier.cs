using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Simplifies expressions like int x^a dx to x^(a+1) / (a+1) + C when a != -1.
/// </summary>
internal sealed class PowerWithConstantExponentIntegralSimplifier : ISimplifier<IntegralExpression>
{
	public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
	{
		if (expression.Expression is PowerExpression body)
		{
			var value = body.Exponent.GetConstantValue(cancellationToken);
			if (body.Base == expression.RespectTo && value != null && value != -1)
				return Exponentiate(body.Base, body.Exponent + 1) / (body.Exponent + 1) + IntegralConstant;
		}

		return expression;
	}
}
