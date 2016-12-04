using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents a power expression.
    /// </summary>
    public class PowerExpression : AlgebraExpression
    {
        public PowerExpression(AlgebraExpression @base, AlgebraExpression exponent)
        {
            if (@base == null)
                throw new ArgumentNullException(nameof(@base));

            if (exponent == null)
                throw new ArgumentNullException(nameof(exponent));
            
            this.Base = @base;
            this.Exponent = exponent;
        }

        /// <summary>
        /// Gets the base of the power.
        /// </summary>
        public AlgebraExpression Base { get; private set; }

        /// <summary>
        /// Gets the exponent of the power.
        /// </summary>
        public AlgebraExpression Exponent { get; private set; }

        
        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            double? @base = this.Base.GetConstantValue(cancellationToken),
                exponent = this.Exponent.GetConstantValue(cancellationToken);

            if (@base != null && exponent != null)
                return Math.Pow(@base.Value, exponent.Value);

            return null;
        }
        

        public override AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement)
        {
            return this
                .WithBase(this.Base.Substitute(variable, replacement))
                .WithExponent(this.Exponent.Substitute(variable, replacement))
            ;
        }

        public override IEnumerable<AlgebraExpression> Children()
        {
            yield return this.Base;
            yield return this.Exponent;
        }


        #region Immutability
        public PowerExpression WithBase(AlgebraExpression newBase)
        {
            return ExpressionFactory.Exponentiate(newBase, this.Exponent);
        }
        public PowerExpression WithExponent(AlgebraExpression newExponent)
        {
            return ExpressionFactory.Exponentiate(this.Base, newExponent);
        }
        #endregion


        public override bool Equals(object obj)
        {
            return this.Equals(obj as PowerExpression);
        }

        public bool Equals(PowerExpression other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            return this.Base == other.Base && this.Exponent == other.Exponent;
        }

        public override int GetHashCode()
        {
            return this.Base.GetHashCode() ^ this.Exponent.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            var baseNeedsParenthesis = this.Base is ExpressionList;
            if (baseNeedsParenthesis)
                result.Append('(');

            result.Append(this.Base);

            if (baseNeedsParenthesis)
                result.Append(')');

            result.Append(' ');
            result.Append('^');
            result.Append(' ');

            var exponentNeedsParenthesis = this.Exponent is ExpressionList;
            if (exponentNeedsParenthesis)
                result.Append('(');

            result.Append(this.Exponent);

            if (exponentNeedsParenthesis)
                result.Append(')');

            return result.ToString();
        }
    }

    public static partial class AlgebraExpressionExtensions
    {
        public static PowerExpression AsPower(this AlgebraExpression expression)
        {
            if (expression is PowerExpression)
                return expression as PowerExpression;

            return ExpressionFactory.Exponentiate(expression, ExpressionFactory.One);
        }
    }

    public static partial class ExpressionFactory
    {
        public static PowerExpression Exponentiate(AlgebraExpression @base, AlgebraExpression exponent)
        {
            return new PowerExpression(@base, exponent);
        }
        public static PowerExpression Root(AlgebraExpression @base, AlgebraExpression exponent)
        {
            return Exponentiate(@base, Divide(ConstantExpression.One, exponent));
        }
        public static PowerExpression Square(AlgebraExpression expression)
        {
            return Exponentiate(expression, Constant(2));
        }
        public static PowerExpression SquareRoot(AlgebraExpression expression)
        {
            return Exponentiate(expression, Divide(ConstantExpression.One, Constant(2)));
        }
    }
}
