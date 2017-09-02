using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Collects terms in a sum expression.
    /// </summary>
    internal sealed class CollectCoefficientsInSumSimplifier : ISimplifier<SumExpressionList>
    {
        private static readonly MultiplicationByOneSimplifier simplifier = new MultiplicationByOneSimplifier();

        public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken)
        {
            var groups = expression.Terms
                .Where(t => t.GetConstantValue() == null)
                .Select(t => t.AsProduct())
                .GroupBy(t =>
                    Product(
                        t.Terms
                            .Where(r => r.GetConstantValue() == null)
                            .ToImmutableList()
                    )
                )
                .Where(g => g.Key.Terms.Any()) // exclude constants
                .Where(g => g.Count() != 1) // exclude single terms
            ;

            var newTerms = expression.Terms
                .RemoveAll(e => groups.Any(g => g.Contains(e.AsProduct())))
                .InsertRange(0, 
                    groups.Select(g =>
                        simplifier.Simplify(
                            Multiply(
                                g
                                    .SelectMany(p => // calculate coefficients
                                        p.Terms
                                            .Select(t => t.GetConstantValue())
                                            .Where(c => c != null)
                                            .DefaultIfEmpty(1)
                                    )
                                    .Sum(),
                                Normalize(g.Key)
                            ), cancellationToken
                        )
                    )
                );

            if (newTerms.Count == 1)
                return newTerms.Single();

            return expression.WithTerms(newTerms);
        }


        private static AlgebraExpression Normalize(ProductExpressionList expression)
        {
            if (expression.Terms.Count == 1)
                return expression.Terms.Single();

            return expression;
        }

        private sealed class ProductExpressionListComparer : IEqualityComparer<ProductExpressionList>
        {
            public bool Equals(ProductExpressionList x, ProductExpressionList y)
            {
                if (x.Terms.Count != y.Terms.Count)
                    return false;

                return x.Terms.All(t => y.Terms.Contains(t));
            }

            public int GetHashCode(ProductExpressionList obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
