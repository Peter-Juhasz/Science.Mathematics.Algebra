using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents an absolute value expression.
/// </summary>
public class AbsoluteValueExpression : AlgebraExpression, IEquatable<AbsoluteValueExpression>
{
	public AbsoluteValueExpression(AlgebraExpression expression)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		this.Expression = expression;
	}


	/// <summary>
	/// Gets the expression.
	/// </summary>
	public AlgebraExpression Expression { get; private set; }


	public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
	{
		double? value = this.Expression.GetConstantValue(cancellationToken);

		if (value != null)
			return Math.Abs(value.Value);

		return null;
	}


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this;

	public override IEnumerable<AlgebraExpression> Children()
	{
		yield return this.Expression;
	}


	#region Immutability
	public AbsoluteValueExpression WithExpression(AlgebraExpression newExpression) => ExpressionFactory.AbsoluteValue(newExpression);
	#endregion


	public override string ToString() => $"|{Expression}|";

	public bool Equals(AbsoluteValueExpression other)
	{
		if (Object.ReferenceEquals(other, null)) return false;

		return this.Expression.Equals(other.Expression);
	}

	public override int GetHashCode() => this.Expression.GetHashCode();

	public override bool Equals(object obj) => this.Equals(obj as AbsoluteValueExpression);
}

public static partial class ExpressionFactory
{
	public static AbsoluteValueExpression AbsoluteValue(AlgebraExpression expression) => new AbsoluteValueExpression(expression);
}
