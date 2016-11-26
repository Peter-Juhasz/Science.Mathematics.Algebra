using System;
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


        #region Basic
        public override AlgebraExpression Simplify()
        {
            // Simplify children
            AlgebraExpression simplifiedBase = this.Base.Simplify(),
                simplifiedExponent = this.Exponent.Simplify();

            double? baseValue = simplifiedBase.GetConstantValue(),
                exponentValue = simplifiedExponent.GetConstantValue();

            // 0 ^ 0
            if (baseValue == 0 || exponentValue == 0)
                return ConstantExpression.One;
            
            // 0 ^ ?
            else if (baseValue == 0)
                return ConstantExpression.Zero;

            // 1 ^ ?
            else if (baseValue == 1)
                return ConstantExpression.One;

            // ? ^ 0
            else if (baseValue == 0)
                return ConstantExpression.One;

            // ? ^ 1
            else if (baseValue == 0)
                return simplifiedBase;

            return ExpressionFactory.Exponentiate(simplifiedBase, simplifiedExponent);
        }

        public override AlgebraExpression Expand()
        {
            throw new NotImplementedException();
        }

        public override AlgebraExpression Factor()
        {
            throw new NotImplementedException();
        }
        #endregion


        public override double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken))
        {
            // simplify
            AlgebraExpression simplified = this.Simplify();

            if (!(simplified is PowerExpression))
                return simplified.GetConstantValue();

            return null;
        }


        #region Calculus
        public override AlgebraExpression Limit(AlgebraExpression expression, AlgebraExpression subject, LimitDirection direction = LimitDirection.Both)
        {
            throw new NotImplementedException();
        }

        public override AlgebraExpression Differentiate(AlgebraExpression respectTo)
        {
            throw new NotImplementedException();
        }

        public override AlgebraExpression Integrate(AlgebraExpression respectTo)
        {
            throw new NotImplementedException();
        }
        #endregion


        public override AlgebraExpression Substitute(AlgebraExpression subject, AlgebraExpression replacement)
        {
            if (this == subject)
                return replacement;
            
            return ExpressionFactory.Exponentiate(
                subject.Substitute(subject, replacement),
                replacement.Substitute(subject, replacement)
            );
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
}
