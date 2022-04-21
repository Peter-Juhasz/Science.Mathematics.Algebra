using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Represents a differentiation expression.
/// </summary>
public record class LimitExpression(AlgebraExpression Expression, SymbolExpression RespectTo, AlgebraExpression To) : AlgebraExpression, IEquatable<LimitExpression>
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default) => Expression.GetConstantValue(cancellationToken) switch
	{
		decimal value => value,
		_ => null
	};

	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this with
	{
		Expression = Expression.Substitute(variable, replacement),
		To = To.Substitute(variable, replacement)
	};

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return RespectTo;
		yield return To;
		yield return Expression;
	}

	public override string ToString() => $"lim {RespectTo} -> {To} ({Expression})";

	public override int GetHashCode() => Expression.GetHashCode();
}

public static partial class ExpressionFactory
{
	public static LimitExpression Limit(AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression to) => new(expression, respectTo, to);
}
