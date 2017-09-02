using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Represents a differentiation expression.
    /// </summary>
    public class IntegralExpression : AlgebraExpression, IEquatable<IntegralExpression>
    {
        public IntegralExpression(AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression from, AlgebraExpression to)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (respectTo == null)
                throw new ArgumentNullException(nameof(respectTo));

            if (from != null && to == null)
                throw new ArgumentNullException(nameof(to));

            if (from == null && to != null)
                throw new ArgumentNullException(nameof(from));

            this.Expression = expression;
            this.RespectTo = respectTo;
            this.From = from;
            this.To = to;
        }
        public IntegralExpression(AlgebraExpression expression, SymbolExpression respectTo)
            : this(expression, respectTo, null, null)
        { }


        /// <summary>
        /// Gets the expression to differentiate.
        /// </summary>
        public AlgebraExpression Expression { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public SymbolExpression RespectTo { get; private set; }

        /// <summary>
        /// Gets the expression to differentiate.
        /// </summary>
        public AlgebraExpression From { get; private set; }

        /// <summary>
        /// Gets the expression to differentiate.
        /// </summary>
        public AlgebraExpression To { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            return null;
        }
        
        public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement)
        {
            return this.WithExpression(this.Expression.Substitute(variable, replacement));
        }

        public override IEnumerable<AlgebraExpression> Children()
        {
            yield return this.Expression;
            yield return this.RespectTo;
        }
                

        #region Immutability
        public IntegralExpression WithRespectTo(SymbolExpression newVariable)
        {
            return Integrate(this.Expression, newVariable, this.From, this.To);
        }

        public IntegralExpression WithExpression(AlgebraExpression newExpression)
        {
            return Integrate(newExpression, this.RespectTo, this.From, this.To);
        }

        public IntegralExpression WithFrom(AlgebraExpression newExpression)
        {
            return Integrate(this.Expression, this.RespectTo, newExpression, this.To);
        }

        public IntegralExpression WithTo(AlgebraExpression newExpression)
        {
            return Integrate(this.Expression, this.RespectTo, this.From, newExpression);
        }
        #endregion


        public override string ToString()
        {
            return $"int_{From}^{To} {Expression} d{RespectTo}";
        }

        public bool Equals(IntegralExpression other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            return this.Expression.Equals(other.Expression)
                && this.RespectTo.Equals(other.RespectTo);
        }

        public override int GetHashCode()
        {
            return this.Expression.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IntegralExpression);
        }
    }

    public static partial class AlgebraExpressionExtensions
    {
        public static IntegralExpression Integrate(this AlgebraExpression expression, SymbolExpression respectTo)
        {
            return ExpressionFactory.Integrate(expression, respectTo);
        }

        public static IntegralExpression Integrate(this AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression from, AlgebraExpression to)
        {
            return ExpressionFactory.Integrate(expression, respectTo, from, to);
        }


        public static bool IsConstant(this AlgebraExpression expression, IntegralExpression integral)
        {
            return expression.IsConstant(integral.RespectTo);
        }
    }

    public static partial class ExpressionFactory
    {
        public static SymbolExpression IntegralConstant = Symbol("C");
        
        public static IntegralExpression Integrate(AlgebraExpression expression, SymbolExpression respectTo)
        {
            return new IntegralExpression(expression, respectTo);
        }

        public static IntegralExpression Integrate(AlgebraExpression expression, SymbolExpression respectTo, AlgebraExpression from, AlgebraExpression to)
        {
            return new IntegralExpression(expression, respectTo, from, to);
        }
    }
}
