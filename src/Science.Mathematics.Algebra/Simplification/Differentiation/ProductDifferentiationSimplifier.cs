using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like d/dx (f(x) * g(x) * ...) to f'(x) * g(x) * ... + f(x) * g'(x) * ... + ....
    /// </summary>
    internal sealed class ProductDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is ProductExpressionList)
            {
                var product = expression.Expression as ProductExpressionList;

                return ExpressionFactory.Sum(
                    product.Terms
                        .Select(t =>
                            ExpressionFactory.Product(
                                new[] { t.Differentiate(expression.RespectTo) }
                                    .Concat(product.Terms.Except(new[] { t }))
                                    .ToImmutableList()
                            )
                        )
                        .ToImmutableList()
                );
            }

            return expression;
        }
    }
}
