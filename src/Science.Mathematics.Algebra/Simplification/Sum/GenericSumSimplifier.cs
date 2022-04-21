using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions.
/// </summary>
internal sealed class GenericSumSimplifier : ISimplifier<SumExpressionList>
{
	public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken) => expression with
	{
		Terms = expression.Terms.Select(t => t.Simplify(cancellationToken)).ToImmutableList()
	};
}
