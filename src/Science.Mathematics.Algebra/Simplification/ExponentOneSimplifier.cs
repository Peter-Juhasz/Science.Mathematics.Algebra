using System.Threading;

namespace Science.Mathematics.Algebra.Simplification
{
    /// <summary>
    /// Simplifies expressions like x ^ 1 to x.
    /// </summary>
    public sealed class ExponentOneSimplifier : ISimplifier<PowerExpression>
    {
        public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Exponent.GetConstantValue(cancellationToken) == 1)
                return expression.Base;

            return expression;
        }
    }
}
