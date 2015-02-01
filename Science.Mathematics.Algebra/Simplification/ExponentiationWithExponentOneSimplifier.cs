using System.Threading;

namespace Science.Mathematics.Algebra.Simplification
{
    /// <summary>
    /// Simplifies expressions like x ^ 1 to x.
    /// </summary>
    public sealed class ExponentiationWithExponentOneSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Exponent.GetConstantValue() == 1)
                return expression.Base;

            return expression;
        }
    }
}
