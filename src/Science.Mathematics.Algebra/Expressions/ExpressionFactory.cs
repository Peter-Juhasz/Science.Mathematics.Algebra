using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Creates algebra expressions.
    /// </summary>
    public static partial class ExpressionFactory
    {
        public static ConstantExpression Zero => ConstantExpression.Zero;

        public static ConstantExpression One => ConstantExpression.One;

        public static ConstantExpression MinusOne => ConstantExpression.MinusOne;


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
        public static VariableExpression Variable(char name)
        {
            return new VariableExpression(name);
        }
        
        public static AlgebraExpression Negate(AlgebraExpression expression)
        {
            return Multiply(ConstantExpression.MinusOne, expression);
        }
        
        public static SumExpressionList Add(AlgebraExpression left, AlgebraExpression right)
        {
            return new SumExpressionList(ImmutableList.Create(left, right));
        }
        public static SumExpressionList Subtract(AlgebraExpression left, AlgebraExpression right)
        {
            return Add(left, Negate(right));
        }

        public static SumExpressionList Sum(IReadOnlyCollection<AlgebraExpression> terms)
        {
            return new SumExpressionList(terms.ToImmutableList());
        }
        public static SumExpressionList Sum(params AlgebraExpression[] terms)
        {
            return Sum(terms as IReadOnlyCollection<AlgebraExpression>);
        }

        public static ProductExpressionList Multiply(AlgebraExpression left, AlgebraExpression right)
        {
            return new ProductExpressionList(ImmutableList.Create(left, right));
        }
        public static ProductExpressionList Divide(AlgebraExpression left, AlgebraExpression right)
        {
            return Multiply(left, Exponentiate(right, ConstantExpression.MinusOne));
        }

        public static ProductExpressionList Reciprocal(AlgebraExpression expression)
        {
            return Divide(ConstantExpression.One, expression);
        }

        public static ProductExpressionList Product(IReadOnlyCollection<AlgebraExpression> terms)
        {
            return new ProductExpressionList(terms.ToImmutableList());
        }
        public static ProductExpressionList Product(params AlgebraExpression[] terms)
        {
            return Product(terms as IReadOnlyCollection<AlgebraExpression>);
        }
        
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

        public static SumExpressionList Polynomial(VariableExpression variable, IReadOnlyList<AlgebraExpression> coefficients)
        {
            return new SumExpressionList(
                coefficients
                    .Select((c, i) => Multiply(c, Exponentiate(variable, coefficients.Count - i)))
                    .ToImmutableList()
            );
        }
        public static SumExpressionList Polynomial(VariableExpression variable, params AlgebraExpression[] coefficients)
        {
            return Polynomial(variable, coefficients as IReadOnlyList<AlgebraExpression>);
        }

        public static FunctionInvocationExpression Invoke(string name, IReadOnlyList<AlgebraExpression> arguments)
        {
            return new FunctionInvocationExpression(name, arguments.ToImmutableList());
        }
        public static FunctionInvocationExpression Invoke(string name, params AlgebraExpression[] arguments)
        {
            return Invoke(name, arguments as IReadOnlyList<AlgebraExpression>);
        }

        public static AbsoluteValueExpression AbsoluteValue(AlgebraExpression expression)
        {
            return new AbsoluteValueExpression(expression);
        }

        public static DifferentiationExpression Differentiate(AlgebraExpression expression, VariableExpression respectTo)
        {
            return new DifferentiationExpression(expression, respectTo);
        }
    }
}
