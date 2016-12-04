using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Represents an algebra expression. Serves as the base class of all expressions.
    /// </summary>
    public abstract class AlgebraExpression
    {
        /// <summary>
        /// Computes the constant value of the expression.
        /// </summary>
        /// <returns></returns>
        public abstract double? GetConstantValue(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Replaces every occurrences of <paramref name="subject"/> to <paramref name="replacement"/>.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public abstract AlgebraExpression Substitute(VariableExpression variable, AlgebraExpression replacement);

        public abstract IEnumerable<AlgebraExpression> Children();

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
        public static implicit operator AlgebraExpression(int value)
        {
            return ExpressionFactory.Constant(value);
        }

        public static implicit operator AlgebraExpression(double value)
        {
            return ExpressionFactory.Constant(value);
        }

        public static implicit operator AlgebraExpression(string name)
        {
            return ExpressionFactory.Variable(name);
        }

        public static implicit operator AlgebraExpression(char name)
        {
            return ExpressionFactory.Variable(name);
        }
        #endregion
    }

    public static partial class AlgebraExpressionExtensions
    {
        public static bool IsConstant(this AlgebraExpression expression, VariableExpression respectTo)
        {
            return !expression.DescendantsAndSelf().Contains(respectTo);
        }


        public static IEnumerable<AlgebraExpression> Descendants(this AlgebraExpression expression)
        {
            foreach (var child in expression.Children())
            {
                foreach (var descendant in child.DescendantsAndSelf())
                    yield return descendant;
            }
        }

        public static IEnumerable<AlgebraExpression> DescendantsAndSelf(this AlgebraExpression expression)
        {
            yield return expression;

            foreach (var descendant in expression.Descendants())
                yield return descendant;
        }
    }
}
