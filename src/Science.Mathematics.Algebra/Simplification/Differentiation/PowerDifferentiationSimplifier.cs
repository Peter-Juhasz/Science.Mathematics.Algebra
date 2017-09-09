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
            if (expression.Expression is PowerExpression power)
            {
                return (power.Base ^ (power.Exponent - 1)) * (
                    power.Exponent * power.Base.Differentiation(expression.RespectTo) +
                    power.Base * NaturalLogarithm(power.Base) * power.Exponent.Differentiation(expression.RespectTo)
                );
            }

            return expression;
        }
    }
}
