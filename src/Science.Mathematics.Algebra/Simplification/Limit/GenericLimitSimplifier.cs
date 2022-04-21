using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit;

public class GenericLimitSimplifier : ISimplifier<LimitExpression>
{
	public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken) => expression
			.WithExpression(expression.Expression.Simplify())
			.WithTo(expression.To.Simplify())
		;
}
