using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like d/dx log_f(x) g(x) to (log(f(x)) g'(x) / g(x) - f'(x) log(g(x)) / f(x)) / log^2(f(x)).
    /// </summary>
    internal sealed class LogarithmDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is LogarithmFunctionExpression logarithm)
            {
                return (
                    (NaturalLogarithm(logarithm.Base) * Differentiate(logarithm.Argument, expression.RespectTo)) / logarithm.Argument -
                    (Differentiate(logarithm.Base, expression.RespectTo) * NaturalLogarithm(logarithm.Argument)) / logarithm.Base
                )
                    /
                    Square(NaturalLogarithm(logarithm.Base));
            }

            return expression;
        }
    }
}
