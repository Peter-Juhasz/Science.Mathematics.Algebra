using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like d/dx f(x) ^ g(x) to f(x)^(g(x) - 1) (g(x) f'(x) + f(x) log(f(x)) g'(x)).
    /// </summary>
    internal sealed class PowerDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is PowerExpression)
            {
                var power = expression.Expression as PowerExpression;
                
                return (power.Base ^ (power.Exponent - 1)) * (
                    power.Exponent * power.Base.Differentiate(expression.RespectTo) +
                    power.Base * Logarithm(power.Base) * power.Exponent.Differentiate(expression.RespectTo)
                );
            }

            return expression;
        }
    }
}
