using System;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents a constant expression.
    /// </summary>
    public class ConstantExpression : AtomicExpression,
        IEquatable<ConstantExpression>,
        IComparable<ConstantExpression>
    {
        public ConstantExpression(int value)
        {
            this.Value = value;
        }
        public ConstantExpression(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly ConstantExpression Zero = 0;

        /// <summary>
        /// 
        /// </summary>
        public static readonly ConstantExpression One = 1;

        /// <summary>
        /// 
        /// </summary>
        public static readonly ConstantExpression MinusOne = -1;


        /// <summary>
        /// Gets the value of the expression.
        /// </summary>
        public double Value { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.Value;
        }


        public override AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement)
        {
            return this;
        }


        #region Conversions
        public static implicit operator ConstantExpression(double value)
        {
            return ExpressionFactory.Constant(value);
        }
        #endregion


        public override string ToString()
        {
            return this.Value.ToString();
        }

        public bool Equals(ConstantExpression other)
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
            return this.Equals(obj as ConstantExpression);
        }

        public int CompareTo(ConstantExpression other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }
}
