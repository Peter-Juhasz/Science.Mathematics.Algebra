using System;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents a constant expression.
/// </summary>
public record class NumberExpression(decimal Value) : AtomicExpression,
	IEquatable<NumberExpression>,
	IComparable<NumberExpression>
{
	/// <summary>
	/// 
	/// </summary>
	public static readonly NumberExpression Zero = 0;

	/// <summary>
	/// 
	/// </summary>
	public static readonly NumberExpression One = 1;

	/// <summary>
	/// 
	/// </summary>
	public static readonly NumberExpression MinusOne = -1;


	public override decimal? GetConstantValue(CancellationToken cancellationToken = default) => Value;


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => this;


	#region Conversions
	public static implicit operator NumberExpression(double value) => ExpressionFactory.Number(value);
	public static implicit operator NumberExpression(int value) => ExpressionFactory.Number(value);
	#endregion


	public override string ToString() => Value.ToString();

	public override int GetHashCode() => Value.GetHashCode();

	public int CompareTo(NumberExpression other) => Value.CompareTo(other.Value);
}

public static partial class ExpressionFactory
{
	public static NumberExpression Zero => NumberExpression.Zero;

	public static NumberExpression One => NumberExpression.One;

	public static NumberExpression MinusOne => NumberExpression.MinusOne;


	public static NumberExpression Number(int value) => new(value);
	public static NumberExpression Number(decimal value) => new(value);
	public static NumberExpression Number(double value) => Number((decimal)value);
}
