using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Science.Mathematics.Algebra
{
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


        public static SumExpressionList Polynomial(SymbolExpression variable, IReadOnlyList<AlgebraExpression> coefficients)
        {
            return Sum(
                coefficients
                    .Select((c, i) => Multiply(c, Exponentiation(variable, coefficients.Count - i)))
                    .ToImmutableList()
            );
        }
        public static SumExpressionList Polynomial(SymbolExpression variable, params AlgebraExpression[] coefficients)
        {
            return Polynomial(variable, coefficients as IReadOnlyList<AlgebraExpression>);
        }
    }
}
