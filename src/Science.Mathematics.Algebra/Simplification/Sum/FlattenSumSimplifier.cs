using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions to their constant value, if possible.
    /// </summary>
    internal sealed class FlattenSumSimplifier : ISimplifier<SumExpressionList>
    {
        public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken)
        {
            var nestedSums = expression.Terms.OfType<SumExpressionList>();

            if (!nestedSums.Any())
                return expression;

            var newTerms = expression.Terms;

            foreach (var nested in nestedSums)
            {
                var index = newTerms.IndexOf(nested);
                newTerms = newTerms.RemoveAt(index).InsertRange(index, nested.Terms);
            }

            return expression.WithTerms(newTerms);
        }
    }
}
