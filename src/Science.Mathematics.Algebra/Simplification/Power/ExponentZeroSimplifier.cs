using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like x ^ 0 to x.
    /// </summary>
    internal sealed class ExponentZeroSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Exponent.GetConstantValue(cancellationToken) == 0)
                return One;

            return expression;
        }
    }
}
