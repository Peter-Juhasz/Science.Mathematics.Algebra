using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like b ^ log_b(x) to x.
    /// </summary>
    internal sealed class ExponentiatedToLogarithmWithSameBaseSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Exponent is LogarithmFunctionExpression)
            {
                var log = expression.Exponent as LogarithmFunctionExpression;

                if (expression.Base.Equals(log.Base))
                    return log.Argument;
            }

            return expression;
        }
    }
}
