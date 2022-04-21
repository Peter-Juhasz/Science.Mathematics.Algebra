using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions.
/// </summary>
internal sealed class NestedAbsoluteValueSimplifier : ISimplifier<AbsoluteValueExpression>
{
	public AlgebraExpression Simplify(AbsoluteValueExpression expression, CancellationToken cancellationToken)
	{
		if (expression.Expression is AbsoluteValueExpression)
			return expression.Expression;

		return expression;
	}
}
