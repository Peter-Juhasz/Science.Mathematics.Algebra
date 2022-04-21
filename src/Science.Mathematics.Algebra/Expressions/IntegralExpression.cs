using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Represents a differentiation expression.
/// </summary>
public record class IntegralExpression(AlgebraExpression Expression, SymbolExpression RespectTo, AlgebraExpression? From, AlgebraExpression? To) : AlgebraExpression, IEquatable<IntegralExpression>
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken)) => null;

	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this with 
	{ 
		Expression = Expression.Substitute(variable, replacement) 
	};

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return Expression;
		yield return RespectTo;
	}

	public override string ToString() => $"int_{From}^{To} {Expression} d{RespectTo}";

	public override int GetHashCode() => Expression.GetHashCode();
}

public static partial class AlgebraExpressionExtensions
{
	public static IntegralExpression Integrate(this AlgebraExpression expression, SymbolExpression respectTo) => ExpressionFactory.Integrate(expression, respectTo);

	public static IntegralExpression Integrate(this AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression from, AlgebraExpression to) => ExpressionFactory.Integrate(expression, respectTo, from, to);


	public static bool IsConstant(this AlgebraExpression expression, IntegralExpression integral) => expression.IsConstant(integral.RespectTo);
}

public static partial class ExpressionFactory
{
	public static readonly SymbolExpression IntegralConstant = Symbol("C");

	public static IntegralExpression Integrate(AlgebraExpression expression, SymbolExpression respectTo) => Integrate(expression, respectTo, null, null);

	public static IntegralExpression Integrate(AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression from, AlgebraExpression to) => new(expression, respectTo, from, to);
}
