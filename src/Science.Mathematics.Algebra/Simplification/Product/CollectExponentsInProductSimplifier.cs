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
        private static readonly ExponentOneSimplifier exponentSimplifier = new ExponentOneSimplifier();

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
                            ExpressionFactory.Exponentiate(g.Key, ExpressionFactory.Sum(g.Select(p => p.Exponent).ToImmutableList()))
                        )
                        .Select(p => exponentSimplifier.Simplify(p, cancellationToken))
                        .ToImmutableList()
                );
            }

            return expression;
        }
    }
}
