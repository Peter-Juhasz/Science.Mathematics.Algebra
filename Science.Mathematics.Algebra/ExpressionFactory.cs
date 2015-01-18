using System.Collections.Generic;
using System.Collections.Immutable;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Creates algebra expression.
    /// </summary>
    public static class ExpressionFactory
    {
        public static ConstantExpression Constant(int value)
        {
            return new ConstantExpression(value);
        }
        public static ConstantExpression Constant(double value)
        {
            return new ConstantExpression(value);
        }

        public static VariableExpression Variable(string name)
        {
            return new VariableExpression(name);
        }
        
        public static AlgebraExpression Negate(AlgebraExpression expression)
        {
            return ExpressionFactory.Multiply(ConstantExpression.MinusOne, expression);
        }
        
        public static SumExpressionList Add(AlgebraExpression left, AlgebraExpression right)
        {
            return new SumExpressionList(ImmutableList.Create(left, right));
        }
        public static SumExpressionList Subtract(AlgebraExpression left, AlgebraExpression right)
        {
            return ExpressionFactory.Add(left, ExpressionFactory.Multiply(ConstantExpression.MinusOne, right));
        }

        public static SumExpressionList Sum(IEnumerable<AlgebraExpression> terms)
        {
            return new SumExpressionList(terms.ToImmutableList());
        }
        public static SumExpressionList Sum(params AlgebraExpression[] terms)
        {
            return new SumExpressionList(ImmutableList.Create(terms));
        }

        public static ProductExpressionList Multiply(AlgebraExpression left, AlgebraExpression right)
        {
            return new ProductExpressionList(ImmutableList.Create(left, right));
        }
        public static ProductExpressionList Divide(AlgebraExpression left, AlgebraExpression right)
        {
            return ExpressionFactory.Multiply(left, ExpressionFactory.Exponentiate(right, ConstantExpression.MinusOne));
        }

        public static ProductExpressionList Product(IEnumerable<AlgebraExpression> terms)
        {
            return new ProductExpressionList(terms.ToImmutableList());
        }
        public static ProductExpressionList Product(params AlgebraExpression[] terms)
        {
            return new ProductExpressionList(ImmutableList.Create(terms));
        }
        
        public static PowerExpression Exponentiate(AlgebraExpression @base, AlgebraExpression exponent)
        {
            return new PowerExpression(@base, exponent);
        }
        public static PowerExpression Root(AlgebraExpression @base, AlgebraExpression exponent)
        {
            return new PowerExpression(@base, ExpressionFactory.Divide(ConstantExpression.One, exponent));
        }
        public static PowerExpression Square(AlgebraExpression expression)
        {
            return new PowerExpression(expression, ExpressionFactory.Constant(2));
        }
        public static PowerExpression SquareRoot(AlgebraExpression expression)
        {
            return new PowerExpression(expression, ExpressionFactory.Divide(ConstantExpression.One, ExpressionFactory.Constant(2)));
        }
    }
}
