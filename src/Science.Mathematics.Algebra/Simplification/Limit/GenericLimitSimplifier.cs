using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit;

public class GenericLimitSimplifier : ISimplifier<LimitExpression>
{
	public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken) => expression with
	{
		Expression = expression.Expression.Simplify(cancellationToken),
		To = expression.To.Simplify(cancellationToken)
	};
}
