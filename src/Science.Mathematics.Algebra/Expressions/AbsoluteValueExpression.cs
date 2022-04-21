using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents an absolute value expression.
/// </summary>
public record class AbsoluteValueExpression(AlgebraExpression Expression) : AlgebraExpression, IEquatable<AbsoluteValueExpression>
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default) => Expression.GetConstantValue(cancellationToken) switch
	{
		decimal value => Math.Abs(value),
		_ => null
	};


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this;

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return Expression;
	}

	public override string ToString() => $"|{Expression}|";

	public override int GetHashCode() => Expression.GetHashCode();
}

public static partial class ExpressionFactory
{
	public static AbsoluteValueExpression AbsoluteValue(AlgebraExpression expression) => new AbsoluteValueExpression(expression);
}
