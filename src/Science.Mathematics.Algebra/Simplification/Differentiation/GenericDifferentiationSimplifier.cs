using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions.
    /// </summary>
    internal sealed class GenericDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            return expression
                .WithExpression(expression.Expression.Simplify())
            ;
        }
    }
}
