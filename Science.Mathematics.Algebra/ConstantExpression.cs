using System;

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

        /// <summary>
        /// Gets the value of the expression.
        /// </summary>
        public int Value { get; private set; }


        public override double? GetConstantValue()
        {
            return this.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        public static readonly ConstantExpression Zero = new ConstantExpression(0);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ConstantExpression One = new ConstantExpression(1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ConstantExpression MinusOne = new ConstantExpression(-1);


        #region Conversions
        public static implicit operator ConstantExpression(int value)
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
            if (other == null) return false;

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
