using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions.
/// </summary>
internal sealed class GenericProductSimplifier : ISimplifier<ProductExpressionList>
{
	public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken) => expression with
	{
		Terms = expression.Terms.Select(t => t.Simplify(cancellationToken)).ToImmutableList()
	};
}
