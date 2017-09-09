using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents an absolute value expression.
    /// </summary>
    public class AbsoluteValueExpression : AlgebraExpression, IEquatable<AbsoluteValueExpression>
    {
        public AbsoluteValueExpression(AlgebraExpression expression)
        {
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }
        

        /// <summary>
        /// Gets the expression.
        /// </summary>
        public AlgebraExpression Expression { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default)
        {
            double? value = this.Expression.GetConstantValue(cancellationToken);

            if (value != null)
                return Math.Abs(value.Value);

            return null;
        }


        public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement)
        {
            return this;
        }

        public override IEnumerable<AlgebraExpression> Children()
        {
            yield return this.Expression;
        }

        public override IEnumerable<PatternMatch> MatchTo(AlgebraExpression expression, CancellationToken cancellationToken = default)
        {
            if (expression is AbsoluteValueExpression absoluteValue)
                return absoluteValue.Expression.Match(this.Expression, cancellationToken);

            return Enumerable.Empty<PatternMatch>();
        }

        #region Immutability
        public AbsoluteValueExpression WithExpression(AlgebraExpression newExpression)
        {
            return ExpressionFactory.AbsoluteValue(newExpression);
        }
        #endregion


        public override string ToString()
        {
            return $"|{Expression}|";
        }

        public bool Equals(AbsoluteValueExpression other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            return this.Expression.Equals(other.Expression);
        }

        public override int GetHashCode()
        {
            return this.Expression.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AbsoluteValueExpression);
        }
    }

    public static partial class ExpressionFactory
    {
        public static AbsoluteValueExpression AbsoluteValue(AlgebraExpression expression)
        {
            return new AbsoluteValueExpression(expression);
        }
    }
}
