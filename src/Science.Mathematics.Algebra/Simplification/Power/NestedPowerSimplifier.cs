using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions (a ^ b) ^ c to a ^ (b * c).
    /// </summary>
    internal sealed class NestedPowerSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Base is PowerExpression)
            {
                var @base = expression.Base as PowerExpression;
                return Exponentiate(@base.Base, @base.Exponent * expression.Exponent);
            }

            return expression;
        }
    }
}
