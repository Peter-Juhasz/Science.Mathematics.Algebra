using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra;

using static ExpressionFactory;

/// <summary>
/// Represents an algebra expression. Serves as the base class of all expressions.
/// </summary>
public abstract record class AlgebraExpression
{
	/// <summary>
	/// Computes the constant value of the expression.
	/// </summary>
	/// <returns></returns>
	public abstract decimal? GetConstantValue(CancellationToken cancellationToken = default);

	/// <summary>
	/// Replaces every occurrences of <paramref name="subject"/> to <paramref name="replacement"/>.
	/// </summary>
	/// <param name="variable"></param>
	/// <param name="replacement"></param>
	/// <returns></returns>
	public abstract AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement);

	public abstract IEnumerable<AlgebraExpression> Children();

	public virtual bool IsInfinity() => this.DescendantsAndSelf().OfType<InfinityExpression>().Any();

	#region Operators
	public static SumExpressionList operator +(AlgebraExpression left, AlgebraExpression right) => Add(left, right);
	public static SumExpressionList operator +(decimal left, AlgebraExpression right) => Add(Number(left), right);
	public static SumExpressionList operator +(AlgebraExpression left, decimal right) => Add(left, Number(right));

	public static SumExpressionList operator -(AlgebraExpression left, AlgebraExpression right) => Subtract(left, right);
	public static SumExpressionList operator -(decimal left, AlgebraExpression right) => Subtract(Number(left), right);
	public static SumExpressionList operator -(AlgebraExpression left, decimal right) => Subtract(left, Number(right));

	public static ProductExpressionList operator *(AlgebraExpression left, AlgebraExpression right) => Multiply(left, right);
	public static ProductExpressionList operator *(decimal left, AlgebraExpression right) => Multiply(Number(left), right);
	public static ProductExpressionList operator *(AlgebraExpression left, decimal right) => Multiply(left, Number(right));

	public static ProductExpressionList operator /(AlgebraExpression left, AlgebraExpression right) => Divide(left, right);
	public static ProductExpressionList operator /(decimal left, AlgebraExpression right) => Divide(Number(left), right);
	public static ProductExpressionList operator /(AlgebraExpression left, decimal right) => Divide(left, Number(right));

	public static PowerExpression operator ^(AlgebraExpression left, AlgebraExpression right) => Exponentiate(left, right);
	public static PowerExpression operator ^(decimal left, AlgebraExpression right) => Exponentiate(Number(left), right);
	public static PowerExpression operator ^(AlgebraExpression left, decimal right) => Exponentiate(left, Number(right));

	public static ProductExpressionList operator -(AlgebraExpression expr) => Negate(expr);
	#endregion

	public override int GetHashCode() => ToString().GetHashCode();

	public bool IsEquivalentTo(AlgebraExpression other)
	{
		if (Equals(other))
			return true;

		return this.Simplify().Equals(other.Simplify());
	}

	#region Conversions
	public static implicit operator AlgebraExpression(int value) => Number(value);
	public static implicit operator AlgebraExpression(double value) => Number(value);
	public static implicit operator AlgebraExpression(decimal value) => Number(value);

	public static implicit operator AlgebraExpression(char ch) => Symbol(ch.ToString());
	public static implicit operator AlgebraExpression(string name) => Symbol(name);
	#endregion
}

public static partial class AlgebraExpressionExtensions
{
	public static bool IsConstant(this AlgebraExpression expression, SymbolExpression respectTo) => !expression.DependsUpon(respectTo);

	public static bool DependsUpon(this AlgebraExpression expression, SymbolExpression respectTo) => expression.DescendantsAndSelf().Contains(respectTo);


	public static IEnumerable<AlgebraExpression> Descendants(this AlgebraExpression expression)
	{
		foreach (var child in expression.Children())
		{
			foreach (var descendant in child.DescendantsAndSelf())
				yield return descendant;
		}
	}

	public static IEnumerable<AlgebraExpression> DescendantsAndSelf(this AlgebraExpression expression)
	{
		yield return expression;

		foreach (var descendant in expression.Descendants())
			yield return descendant;
	}
}
