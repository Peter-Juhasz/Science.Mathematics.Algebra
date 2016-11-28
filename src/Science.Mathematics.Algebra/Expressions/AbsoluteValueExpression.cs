using System;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents a constant expression.
    /// </summary>
    public class AbsoluteValueExpression : AlgebraExpression, IEquatable<AbsoluteValueExpression>
    {
        public AbsoluteValueExpression(AlgebraExpression expression)
        {
            this.Expression = expression;
        }
        

        /// <summary>
        /// Gets the value of the expression.
        /// </summary>
        public AlgebraExpression Expression { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            double? value = this.Expression.GetConstantValue(cancellationToken);

            if (value != null)
                return Math.Abs(value.Value);

            return null;
        }


        public override AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement)
        {
            return this;
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
            return this.Equals(obj as AlgebraExpression);
        }
    }
}
