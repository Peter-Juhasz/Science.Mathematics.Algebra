using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions to their constant value, if possible.
    /// </summary>
    internal sealed class FlattenProductSimplifier : ISimplifier<ProductExpressionList>
    {
        public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken)
        {
            var nestedProducts = expression.Terms.OfType<ProductExpressionList>();

            if (!nestedProducts.Any())
                return expression;

            var newTerms = expression.Terms;

            foreach (var nested in nestedProducts)
            {
                var index = newTerms.IndexOf(nested);
                newTerms = newTerms.RemoveAt(index).InsertRange(index, nested.Terms);
            }

            return expression.WithTerms(newTerms);
        }
    }
}
