using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like int (sin x)^n dx to -(sin x)^n * (cos x) / n + (n - 1) / n * int (sin x)^(n - 2) dx
    /// </summary>
    internal sealed class SineIntegralSimplifier : ISimplifier<IntegralExpression>
    {
        public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
        {
            var power = expression.Expression.AsPower();
            if (power.Base is SineFunctionExpression sine)
            {
                if (sine.Argument == expression.RespectTo &&
                    power.Exponent.GetConstantValue(cancellationToken) != -1)
                {
                    var n = power.Exponent;
                    var x = expression.RespectTo;
                    var sinx = power.Base;
                    return -(Exponentiate(sinx, n - 1) * Cosine(x)) / n + (n - 1) / n * Integrate(Exponentiate(sinx, n - 2), x) + IntegralConstant;
                }
            }

            return expression;
        }
    }
}
