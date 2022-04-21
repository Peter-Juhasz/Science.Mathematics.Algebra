using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents a constant expression.
    /// </summary>
    public class NumberExpression : AtomicExpression,
        IEquatable<NumberExpression>,
        IComparable<NumberExpression>
    {
        public NumberExpression(int value)
        {
            this.Value = value;
        }
        public NumberExpression(double value)
        {
            this.Value = value;
        }

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


        /// <summary>
        /// Gets the value of the expression.
        /// </summary>
        public double Value { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default)
        {
            return this.Value;
        }


        public override AlgebraExpression Substitute(SymbolExpression variable, AlgebraExpression replacement)
        {
            return this;
        }

        public override IEnumerable<PatternMatch> MatchTo(AlgebraExpression expression, MatchContext context, CancellationToken cancellationToken = default)
        {
            if (expression.GetConstantValue(cancellationToken) == Value)
                yield return new PatternMatch(this);
        }


        #region Conversions
        public static implicit operator NumberExpression(double value)
        {
            return ExpressionFactory.Number(value);
        }
        #endregion


        public override string ToString()
        {
            return this.Value.ToString();
        }

        public bool Equals(NumberExpression other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            return this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as NumberExpression);
        }

        public int CompareTo(NumberExpression other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }

    public static partial class ExpressionFactory
    {
        public static NumberExpression Zero => NumberExpression.Zero;

        public static NumberExpression One => NumberExpression.One;

        public static NumberExpression MinusOne => NumberExpression.MinusOne;


        public static NumberExpression Constant(int value)
        {
            return new NumberExpression(value);
        }
        public static NumberExpression Number(double value)
        {
            return new NumberExpression(value);
        }
    }
}
