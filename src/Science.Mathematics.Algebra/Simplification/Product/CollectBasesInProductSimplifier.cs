using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Simplifies expressions like x ^ 2 * y ^ 2 to (x * y) ^ 2.
/// </summary>
internal sealed class CollectBasesInProductSimplifier : ISimplifier<ProductExpressionList>
{
	public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken)
	{
		var groups = expression.Terms
			.OfType<PowerExpression>()
			.GroupBy(p => p.Exponent)
			.Where(g => !g.Key.Equals(One)) // exclude simple terms
			.Where(g => g.Count() > 1) // exclude simple groups
			.ToList();

		if (groups.Any())
		{
			return expression.WithTerms(
				expression.Terms
					.RemoveRange(groups.SelectMany(g => g))
					.AddRange(
						groups.Select(g => Product(g.Select(g2 => g2.Base).ToImmutableList()) ^ g.Key)
					)
			);
		}

		return expression;
	}
}
