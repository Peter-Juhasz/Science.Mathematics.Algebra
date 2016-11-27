using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like x ^ 0 to x.
    /// </summary>
    public sealed class ExponentZeroSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Exponent.GetConstantValue(cancellationToken) == 0)
                return ConstantExpression.One;

            return expression;
        }
    }
}
