using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like 0 * x to 0.
    /// </summary>
    public sealed class MultiplicationByZeroSimplifier : ISimplifier<ProductExpressionList>
    {
        public AlgebraExpression Simplify(ProductExpressionList expression, CancellationToken cancellationToken)
        {
            if (expression.Terms.Any(t => t.GetConstantValue(cancellationToken) == 0))
                return ExpressionFactory.Zero;

            return expression;
        }
    }
}
