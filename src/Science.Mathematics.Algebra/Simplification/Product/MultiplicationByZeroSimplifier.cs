using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like 0 * x to 0.
    /// </summary>
    internal sealed class MultiplicationByZeroSimplifier : ISimplifier<ProductExpressionList>
    {
        public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken)
        {
            if (expression.Terms.Any(t => t.GetConstantValue(cancellationToken) == 0))
                return Zero;

            return expression;
        }
    }
}
