using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions to their constant value, if possible.
    /// </summary>
    public sealed class SingleSumSimplifier : ISimplifier<SumExpressionList>
    {
        public AlgebraExpression Simplify(SumExpressionList expression, CancellationToken cancellationToken)
        {
            if (expression.Terms.Count == 1)
                return expression.Terms.Single();

            return expression;
        }
    }
}
