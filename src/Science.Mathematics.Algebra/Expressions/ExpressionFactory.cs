using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Science.Mathematics.Algebra.Tests")]

namespace Science.Mathematics.Algebra;

/// <summary>
/// Creates algebra expressions.
/// </summary>
public static partial class ExpressionFactory
{
	/// <summary>
	/// Represents the base of the natural logarithm.
	/// </summary>
	public static readonly SymbolExpression e = Symbol("e");

	/// <summary>
	/// Represents the mathematical constant π.
	/// </summary>
	public static readonly SymbolExpression Pi = Symbol("π");


	public static SumExpressionList Polynomial(SymbolExpression variable, IReadOnlyList<AlgebraExpression> coefficients) => Sum(
			coefficients
				.Select((c, i) => Multiply(c, Exponentiate(variable, coefficients.Count - i)))
				.ToImmutableList()
		);
	public static SumExpressionList Polynomial(SymbolExpression variable, params AlgebraExpression[] coefficients) => Polynomial(variable, coefficients as IReadOnlyList<AlgebraExpression>);
}
