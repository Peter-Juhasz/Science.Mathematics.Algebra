using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like d/dx c to 0.
    /// </summary>
    internal sealed class ConstantDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            if (expression.GetConstantValue(cancellationToken) != null)
                return Zero;

            return expression;
        }
    }
}
