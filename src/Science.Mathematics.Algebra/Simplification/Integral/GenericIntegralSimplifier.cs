using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions.
    /// </summary>
    internal sealed class GenericIntegralSimplifier : ISimplifier<IntegralExpression>
    {
        public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
        {
            return expression
                .WithExpression(expression.Expression.Simplify())
            ;
        }
    }
}
