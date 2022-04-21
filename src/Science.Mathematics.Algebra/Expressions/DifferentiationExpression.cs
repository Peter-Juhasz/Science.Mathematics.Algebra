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
public class DifferentiationExpression : AlgebraExpression, IEquatable<DifferentiationExpression>
{
	public DifferentiationExpression(AlgebraExpression expression, SymbolExpression respectTo)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		if (respectTo == null)
			throw new ArgumentNullException(nameof(respectTo));

		this.Expression = expression;
		this.RespectTo = respectTo;
	}


	/// <summary>
	/// Gets the expression to differentiate.
	/// </summary>
	public AlgebraExpression Expression { get; private set; }

	/// <summary>
	/// 
	/// </summary>
	public SymbolExpression RespectTo { get; private set; }


	public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
	{
		double? value = this.Expression.GetConstantValue(cancellationToken);

		if (value != null)
			return 0;

		return null;
	}

	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this.WithExpression(this.Expression.Substitute(variable, replacement));

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return this.Expression;
		yield return this.RespectTo;
	}


	public LimitExpression ToLimit()
	{
		var delta = Symbol("h");
		return Limit((this.Expression.Substitute(this.RespectTo, this.RespectTo + delta) - this.Expression) / delta, delta, 0);
	}


	#region Immutability
	public DifferentiationExpression WithRespectTo(SymbolExpression newVariable) => Differentiate(this.Expression, newVariable);

	public DifferentiationExpression WithExpression(AlgebraExpression newExpression) => Differentiate(newExpression, this.RespectTo);
	#endregion


	public override string ToString() => $"d/d{RespectTo} {Expression}";

	public bool Equals(DifferentiationExpression other)
	{
		if (Object.ReferenceEquals(other, null)) return false;

		return this.Expression.Equals(other.Expression)
			&& this.RespectTo.Equals(other.RespectTo);
	}

	public override int GetHashCode() => this.Expression.GetHashCode();

	public override bool Equals(object obj) => this.Equals(obj as DifferentiationExpression);
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
