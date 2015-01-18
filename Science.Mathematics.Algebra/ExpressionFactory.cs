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


        public static SumExpressionList Add(AlgebraExpression left, AlgebraExpression right)
        {
            return new SumExpressionList(ImmutableList.Create(left, right));
        }
        public static SumExpressionList Sum(params AlgebraExpression[] terms)
        {
            return new SumExpressionList(ImmutableList.Create(terms));
        }

        public static ProductExpressionList Multiply(AlgebraExpression left, AlgebraExpression right)
        {
            return new ProductExpressionList(ImmutableList.Create(left, right));
        }
        public static ProductExpressionList Product(params AlgebraExpression[] terms)
        {
            return new ProductExpressionList(ImmutableList.Create(terms));
        }
    }
}
