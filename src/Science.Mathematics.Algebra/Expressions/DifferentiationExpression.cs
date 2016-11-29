using System;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents a differentiation expression.
    /// </summary>
    public class DifferentiationExpression : AlgebraExpression, IEquatable<DifferentiationExpression>
    {
        public DifferentiationExpression(AlgebraExpression expression, VariableExpression respectTo)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (respectTo == null)
                throw new ArgumentNullException(nameof(respectTo));

            this.Expression = expression;
            this.RespectTo = respectTo;
        }
        

        /// <summary>
        /// Gets the expression to differentiate.
        /// </summary>
        public AlgebraExpression Expression { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public VariableExpression RespectTo { get; private set; }


        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            double? value = this.Expression.GetConstantValue(cancellationToken);

            if (value != null)
                return 0;

            return null;
        }


        public override AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement)
        {
            return this.WithExpression(this.Expression.Substitute(variable, replacement));
        }


        #region Immutability
        public DifferentiationExpression WithRespectTo(VariableExpression newVariable)
        {
            return ExpressionFactory.Differentiate(this.Expression, newVariable);
        }

        public DifferentiationExpression WithExpression(AlgebraExpression newExpression)
        {
            return ExpressionFactory.Differentiate(newExpression, this.RespectTo);
        }
        #endregion


        public override string ToString()
        {
            return $"d/d{RespectTo} {Expression}";
        }

        public bool Equals(DifferentiationExpression other)
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
            return this.Equals(obj as AlgebraExpression);
        }
    }

    public static partial class AlgebraExpressionExtensions
    {
        public static AlgebraExpression Differentiate(AlgebraExpression expression, VariableExpression respectTo)
        {
            return ExpressionFactory.Differentiate(expression, respectTo);
        }
    }
}
