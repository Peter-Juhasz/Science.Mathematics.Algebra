using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like x ^ 2 * x ^ 3 to x ^ 5.
    /// </summary>
    internal sealed class CollectExponentsInProductSimplifier : ISimplifier<ProductExpressionList>
    {
        public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken)
        {
            var results = expression.Terms
                .Select(t => t.AsPower())
                .GroupBy(p => p.Base)
                .ToList();

            if (results.Count != expression.Terms.Count)
            {
                return expression.WithTerms(
                    results
                        .Select(g =>
                            ExpressionFactory.Exponentiation(g.Key, ExpressionFactory.Sum(g.Select(p => p.Exponent).ToImmutableList()))
                        )
                        .Select(p => p.Simplify())
                        .ToImmutableList()
                );
            }

            return expression;
        }
    }
}
