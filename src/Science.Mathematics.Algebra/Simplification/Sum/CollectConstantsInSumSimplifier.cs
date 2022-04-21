using System;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Collects constants in a sum expression.
/// </summary>
internal sealed class CollectConstantsInSumSimplifier : ISimplifier<SumExpressionList>
{
	public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken)
	{
		// collect constants
		var constants = expression.Terms.ToDictionary(e => e, e => e.GetConstantValue());

		// all constant: 1 + 2 + 3  =>  6
		if (constants.Values.All(v => v != null))
			return Number(constants.Values.Cast<decimal>().Sum(v => v));

		// collect constants: 1 + ? + 3  =>  (1 + 3) + ?  =>  4 + ?
		var sum = constants.Values.Where(v => v != null).Cast<decimal>().Sum(v => v);
		var newTerms = expression.Terms.RemoveAll(e => constants[e].HasValue);

		// do not add if zero
		if (sum != 0)
			newTerms = newTerms.Add(Number(sum));

		if (newTerms.Count == 1)
			return newTerms.Single();

		return expression with { Terms = newTerms };
	}
}
