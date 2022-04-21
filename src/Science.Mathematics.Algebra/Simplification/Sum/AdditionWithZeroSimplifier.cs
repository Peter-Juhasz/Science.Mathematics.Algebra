using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions like 0 + x to x.
/// </summary>
internal sealed class AdditionWithZeroSimplifier : ISimplifier<SumExpressionList>
{
	public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken)
	{
		var otherTerms = expression.Terms
			.Where(t => t.GetConstantValue(cancellationToken) != 0)
			.ToImmutableList();

		if (otherTerms.Count != expression.Terms.Count)
			return expression.WithTerms(otherTerms);

		return expression;
	}
}
