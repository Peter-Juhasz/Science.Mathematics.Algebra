using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like d/dx (f(x) + g(x) + ...) to d/dx f(x) + d/dx g(x) + ....
    /// </summary>
    internal sealed class SumDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is SumExpressionList)
            {
                var sum = expression.Expression as SumExpressionList;

                return Sum(
                    sum.Terms
                        .Select(t => t.Differentiate(expression.RespectTo))
                        .ToImmutableList()
                );
            }

            return expression;
        }
    }
}
