using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Simplifies expressions like int f'(x) / f(x) dx to ln | f(x) | + C.
/// </summary>
internal sealed class DerivativeInNumeratorSimplifier : ISimplifier<IntegralExpression>
{
	public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
	{
		if (expression.Expression is ProductExpressionList product)
		{
			var denominator = product.GetDenominator();
			if (product.GetNumerator().IsEquivalentTo(denominator.Differentiate(expression.RespectTo)))
				return NaturalLogarithm(AbsoluteValue(denominator)) + IntegralConstant;
		}

		return expression;
	}
}
