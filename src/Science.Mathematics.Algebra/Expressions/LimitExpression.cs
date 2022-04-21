using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Represents a differentiation expression.
/// </summary>
public class LimitExpression : AlgebraExpression, IEquatable<LimitExpression>
{
	public LimitExpression(AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression to)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		if (respectTo == null)
			throw new ArgumentNullException(nameof(respectTo));

		if (to == null)
			throw new ArgumentNullException(nameof(to));

		this.Expression = expression;
		this.RespectTo = respectTo;
		this.To = to;
	}


	/// <summary>
	/// 
	/// </summary>
	public SymbolExpression RespectTo { get; private set; }

	public AlgebraExpression To { get; private set; }

	/// <summary>
	/// Gets the expression to differentiate.
	/// </summary>
	public AlgebraExpression Expression { get; private set; }


	public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
	{
		double? value = this.Expression.Substitute(this.RespectTo, this.To).GetConstantValue(cancellationToken);

		if (value != null)
			return value;

		return null;
	}

	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this
			.WithExpression(this.Expression.Substitute(variable, replacement))
			.WithTo(this.To.Substitute(variable, replacement))
		;

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return this.RespectTo;
		yield return this.To;
		yield return this.Expression;
	}


	#region Immutability
	public LimitExpression WithRespectTo(SymbolExpression newVariable) => Limit(this.Expression, newVariable, this.To);

	public LimitExpression WithExpression(AlgebraExpression newExpression) => Limit(newExpression, this.RespectTo, this.To);

	public LimitExpression WithTo(AlgebraExpression newTo) => Limit(this.Expression, this.RespectTo, newTo);
	#endregion


	public override string ToString() => $"lim {RespectTo} -> {To} ({Expression})";

	public bool Equals(LimitExpression other)
	{
		if (Object.ReferenceEquals(other, null)) return false;

		return this.Expression.Equals(other.Expression)
			&& this.RespectTo.Equals(other.RespectTo)
			&& this.To.Equals(other.To);
	}

	public override int GetHashCode() => this.Expression.GetHashCode();

	public override bool Equals(object obj) => this.Equals(obj as LimitExpression);
}

public static partial class ExpressionFactory
{
	public static LimitExpression Limit(AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression to) => new LimitExpression(expression, respectTo, to);
}
