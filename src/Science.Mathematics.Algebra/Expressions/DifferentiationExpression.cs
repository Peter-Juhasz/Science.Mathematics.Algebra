using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Represents a differentiation expression.
/// </summary>
public record class DifferentiationExpression(AlgebraExpression Expression, SymbolExpression RespectTo) : AlgebraExpression, IEquatable<DifferentiationExpression>
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default) => Expression.GetConstantValue(cancellationToken) switch
	{
		decimal => 0,
		_ => null
	};

	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this with
	{
		Expression = Expression.Substitute(variable, replacement)
	};

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return Expression;
		yield return RespectTo;
	}


	public LimitExpression ToLimit()
	{
		var delta = Symbol("h");
		return Limit((Expression.Substitute(RespectTo, RespectTo + delta) - Expression) / delta, delta, 0);
	}

	public override string ToString() => $"d/d{RespectTo} {Expression}";

	public override int GetHashCode() => Expression.GetHashCode();
}

public static partial class AlgebraExpressionExtensions
{
	public static DifferentiationExpression Differentiate(this AlgebraExpression expression, SymbolExpression respectTo) => ExpressionFactory.Differentiate(expression, respectTo);

	public static DifferentiationExpression PartialDerivative(this AlgebraExpression expression, SymbolExpression respectTo) => Differentiate(expression, respectTo);

	public static AlgebraExpression TotalDerivative(this AlgebraExpression function, IReadOnlyCollection<SymbolExpression> parameters, SymbolExpression respectTo)
	{
		if (!parameters.Contains(respectTo))
			throw new ArgumentException($"Parameter '{nameof(respectTo)}' must be one of the variables provided in parameter '{nameof(parameters)}'.", nameof(respectTo));

		return Sum(
			parameters
				.Select(p => function.PartialDerivative(p) * p.Differentiate(respectTo))
				.ToImmutableList()
		);
	}


	public static bool IsConstant(this AlgebraExpression expression, DifferentiationExpression differentiation) => expression.IsConstant(differentiation.RespectTo);
}

public static partial class ExpressionFactory
{
	public static DifferentiationExpression Differentiate(AlgebraExpression expression, SymbolExpression respectTo) => new DifferentiationExpression(expression, respectTo);
}
