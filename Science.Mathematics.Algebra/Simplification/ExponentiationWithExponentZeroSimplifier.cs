using System.Threading;

namespace Science.Mathematics.Algebra.Simplification
{
    /// <summary>
    /// Simplifies expressions like x ^ 0 to x.
    /// </summary>
    public sealed class ExponentiationWithExponentZeroSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Exponent.GetConstantValue() == 0)
                return ConstantExpression.One;

            return expression;
        }
    }
}
