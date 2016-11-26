using System;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents an algebra expression. Serves as the base class of all expressions.
    /// </summary>
    public abstract class AlgebraExpression
    {
        #region Simple algebraic transformations
        /// <summary>
        /// Simplifies the expression.
        /// </summary>
        /// <returns></returns>
        public abstract AlgebraExpression Simplify();

        /// <summary>
        /// Expands the expression.
        /// </summary>
        /// <returns></returns>
        public abstract AlgebraExpression Expand();

        /// <summary>
        /// Factors the expression.
        /// </summary>
        /// <returns></returns>
        public abstract AlgebraExpression Factor();
        #endregion

        /// <summary>
        /// Computes the constant value of the expression.
        /// </summary>
        /// <returns></returns>
        public abstract double? GetConstantValue();
        
        #region Calculus
        /// <summary>
        /// Computes the limit as <paramref name="expression"/> approaches <paramref name="subject"/>.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public abstract AlgebraExpression Limit(AlgebraExpression expression, AlgebraExpression subject, LimitDirection direction = LimitDirection.Both);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="respectTo"></param>
        /// <returns></returns>
        public virtual AlgebraExpression Differentiate(AlgebraExpression respectTo)
        {
            VariableExpression limitVariable = new VariableExpression("h");

            return (
                this.Substitute(respectTo, respectTo - limitVariable) / (respectTo - limitVariable)
            ).Limit(limitVariable, ConstantExpression.Zero);
        }

        /// <summary>
        /// Constructs the primitive function the given expression respect to <paramref name="respectTo"/>.
        /// </summary>
        /// <param name="respectTo"></param>
        /// <returns></returns>
        public abstract AlgebraExpression Integrate(AlgebraExpression respectTo);
        #endregion
        
        /// <summary>
        /// Replaces every occurrences of <paramref name="subject"/> to <paramref name="replacement"/>.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public abstract AlgebraExpression Substitute(AlgebraExpression subject, AlgebraExpression replacement);

        #region Operators
        public static AlgebraExpression operator +(AlgebraExpression left, AlgebraExpression right)
        {
            return ExpressionFactory.Add(left, right);
        }
        public static AlgebraExpression operator +(double left, AlgebraExpression right)
        {
            return ExpressionFactory.Add(ExpressionFactory.Constant(left), right);
        }
        public static AlgebraExpression operator +(AlgebraExpression left, double right)
        {
            return ExpressionFactory.Add(left, ExpressionFactory.Constant(right));
        }

        public static AlgebraExpression operator -(AlgebraExpression left, AlgebraExpression right)
        {
            return ExpressionFactory.Subtract(left, right);
        }
        public static AlgebraExpression operator -(double left, AlgebraExpression right)
        {
            return ExpressionFactory.Subtract(ExpressionFactory.Constant(left), right);
        }
        public static AlgebraExpression operator -(AlgebraExpression left, double right)
        {
            return ExpressionFactory.Subtract(left, ExpressionFactory.Constant(right));
        }

        public static AlgebraExpression operator *(AlgebraExpression left, AlgebraExpression right)
        {
            return ExpressionFactory.Multiply(left, right);
        }
        public static AlgebraExpression operator *(double left, AlgebraExpression right)
        {
            return ExpressionFactory.Multiply(ExpressionFactory.Constant(left), right);
        }
        public static AlgebraExpression operator *(AlgebraExpression left, double right)
        {
            return ExpressionFactory.Multiply(left, ExpressionFactory.Constant(right));
        }

        public static AlgebraExpression operator /(AlgebraExpression left, AlgebraExpression right)
        {
            return ExpressionFactory.Divide(left, right);
        }
        public static AlgebraExpression operator /(double left, AlgebraExpression right)
        {
            return ExpressionFactory.Divide(ExpressionFactory.Constant(left), right);
        }
        public static AlgebraExpression operator /(AlgebraExpression left, double right)
        {
            return ExpressionFactory.Divide(left, ExpressionFactory.Constant(right));
        }

        public static AlgebraExpression operator ^(AlgebraExpression left, AlgebraExpression right)
        {
            return ExpressionFactory.Exponentiate(left, right);
        }
        public static AlgebraExpression operator ^(double left, AlgebraExpression right)
        {
            return ExpressionFactory.Exponentiate(ExpressionFactory.Constant(left), right);
        }
        public static AlgebraExpression operator ^(AlgebraExpression left, double right)
        {
            return ExpressionFactory.Exponentiate(left, ExpressionFactory.Constant(right));
        }

        public static AlgebraExpression operator -(AlgebraExpression expr)
        {
            return ExpressionFactory.Negate(expr);
        }

        public static bool operator ==(AlgebraExpression left, AlgebraExpression right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(AlgebraExpression left, AlgebraExpression right)
        {
            return !(left == right);
        }
        #endregion

        #region Conversions
        public static implicit operator AlgebraExpression(double value)
        {
            return ExpressionFactory.Constant(value);
        }
        #endregion
    }

    /// <summary>
    /// Contains extensions for <see cref="AlgebraExpression"/>s.
    /// </summary>
    public static class AlgebraExpressionExtensions
    {
        /// <summary>
        /// Differentiates a given expression <paramref name="degree"/> times.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="respectTo"></param>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static AlgebraExpression Differentiate(this AlgebraExpression subject, AlgebraExpression respectTo, int degree)
        {
            if (degree < 0)
                throw new ArgumentException("Degree of differentiation must be a non-negative integer.", nameof(degree));

            AlgebraExpression result = subject;

            for (int i = 0; i < degree; i++)
                result = result.Differentiate(respectTo);
            
            return respectTo;
        }

        /// <summary>
        /// Calculates the Riemann Integral of the expression in a given interval.
        /// </summary>
        /// <param name="respectTo"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <returns></returns>
        public static AlgebraExpression Integrate(this AlgebraExpression subject, AlgebraExpression respectTo, AlgebraExpression lowerBound, AlgebraExpression upperBound)
        {
            AlgebraExpression primitiveFunction = subject.Integrate(respectTo);

            return primitiveFunction.Substitute(respectTo, upperBound) - primitiveFunction.Substitute(respectTo, lowerBound);
        }
    }
}
