using System;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Represents a variable.
/// </summary>
public record class SymbolExpression(string Name) : AtomicExpression, IEquatable<SymbolExpression>
{
	public override decimal? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken)) => null;


	public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement) => Name == variable.Name ? replacement : this;


	#region Conversions
	public static implicit operator SymbolExpression(char ch) => ExpressionFactory.Symbol(ch.ToString());

	public static implicit operator SymbolExpression(string name) => ExpressionFactory.Symbol(name);
	#endregion


	public override int GetHashCode() => Name.GetHashCode();

	public override string ToString() => Name;
}

public static partial class ExpressionFactory
{
	public static SymbolExpression Symbol(string name) => new(name);
	public static SymbolExpression Symbol(char name) => Symbol(name.ToString());
}
