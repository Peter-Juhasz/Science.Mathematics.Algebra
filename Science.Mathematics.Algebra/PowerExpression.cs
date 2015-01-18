using System;
using System.Text;

namespace Science.Mathematics.Algebra
{
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

        public AlgebraExpression Base { get; private set; }

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


        public override double? GetConstantValue()
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
            return obj is PowerExpression ? this.Equals(obj as PowerExpression) : false;
        }

        public bool Equals(PowerExpression other)
        {
            if (other == null) return false;

            return this.Base.Equals(other.Base) && this.Exponent.Equals(other.Exponent);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            var baseNeedsParenthesis = this.Base is ExpressionList || this.Base is ExpressionList;
            if (baseNeedsParenthesis)
                sb.Append('(');

            sb.Append(this.Base);

            if (baseNeedsParenthesis)
                sb.Append(')');

            sb.Append(' ');
            sb.Append('^');
            sb.Append(' ');

            var exponentNeedsParenthesis = this.Exponent is ExpressionList || this.Exponent is ExpressionList;
            if (exponentNeedsParenthesis)
                sb.Append('(');

            sb.Append(this.Exponent);

            if (exponentNeedsParenthesis)
                sb.Append(')');

            return sb.ToString();
        }
    }
}
