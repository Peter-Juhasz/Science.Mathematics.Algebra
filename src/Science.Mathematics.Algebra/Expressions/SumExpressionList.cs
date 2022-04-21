using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents the sum of multiple terms.
/// </summary>
public record class SumExpressionList : ExpressionList, IEquatable<SumExpressionList>
{
	public SumExpressionList(IImmutableList<AlgebraExpression> Terms)
		: base(Terms)
	{
	}

	public override decimal? GetConstantValue(CancellationToken cancellationToken = default)
	{
		var values = Terms.Select(t => t.GetConstantValue()).Memoize();

		if (values.Contains(null))
			return null;

		return values.Sum();
	}


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this with
	{
		Terms = Terms
			.Select(t => t == variable ? replacement : variable)
			.Select(t => t.Substitute(variable, replacement))
			.ToImmutableList()
	};

	internal AlgebraExpression Normalize()
	{
		if (Terms.Count == 1)
			return Terms.Single();

		return this;
	}

	public virtual bool Equals(SumExpressionList? other) => other != null && Terms.Zip(other.Terms, (x, y) => x.Equals(y)).All(i => i);

	public override int GetHashCode() => Terms.Select(o => o.GetHashCode()).Aggregate((x, y) => x ^ y);

	public override string ToString() => String.Join(" + ", Terms
		.Select(t => NeedsParenthesis(t) ? $"({t})" : t.ToString())
	);

	private static bool NeedsParenthesis(AlgebraExpression expression) => expression is SumExpressionList;
}

public static partial class ExpressionFactory
{
	public static SumExpressionList Add(AlgebraExpression left, AlgebraExpression right) => new SumExpressionList(ImmutableList.Create(left, right));
	public static SumExpressionList Subtract(AlgebraExpression left, AlgebraExpression right) => Add(left, Negate(right));

	public static SumExpressionList Sum(IReadOnlyCollection<AlgebraExpression> terms) => new SumExpressionList(terms.ToImmutableList());
	public static SumExpressionList Sum(params AlgebraExpression[] terms) => Sum(terms as IReadOnlyCollection<AlgebraExpression>);
}
