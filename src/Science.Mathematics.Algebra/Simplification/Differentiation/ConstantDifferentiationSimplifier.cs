using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like d/dx c to 0.
    /// </summary>
    internal sealed class ConstantDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            if (expression.GetConstantValue(cancellationToken) == null)
                return ExpressionFactory.Zero;

            return expression;
        }
    }
}
