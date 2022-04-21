using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Simplifies expressions like int cos x dx to sin x + C.
/// </summary>
internal sealed class CosineIntegralSimplifier : ISimplifier<IntegralExpression>
{
	public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
	{
		var power = expression.Expression.AsPower();
		if (power.Base is CosineFunctionExpression cosine)
		{
			if (cosine.Argument == expression.RespectTo &&
				power.Exponent.GetConstantValue(cancellationToken) != -1)
			{
				var n = power.Exponent;
				var x = expression.RespectTo;
				var cosx = power.Base;
				return Exponentiate(cosx, n - 1) * Sine(x) / n + (n - 1) / n * Integrate(Exponentiate(cosx, n - 2), x) + IntegralConstant;
			}
		}

		return expression;
	}
}
