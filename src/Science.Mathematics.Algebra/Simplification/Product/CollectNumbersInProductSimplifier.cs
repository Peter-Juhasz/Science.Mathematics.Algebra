﻿using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Simplifies expressions like 2 * 3 * x to 6 * x.
/// </summary>
internal sealed class CollectNumbersInProductSimplifier : ISimplifier<ProductExpressionList>
{
	public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken)
	{
		var constants = expression.Terms
			.ToDictionary(t => t, t => t.GetConstantValue(cancellationToken));

		if (constants.Values.Any(c => c.HasValue))
		{
			// collect coefficients
			IImmutableList<AlgebraExpression> terms = expression.Terms;
			decimal coefficient = 1m;

			foreach (var kv in constants.Where(kv => kv.Value.HasValue))
			{
				coefficient *= kv.Value.Value;
				terms = terms.Remove(kv.Key);
			}

			// append result
			terms = terms.Insert(0, coefficient);

			return Product(terms);
		}

		return expression;
	}
}
