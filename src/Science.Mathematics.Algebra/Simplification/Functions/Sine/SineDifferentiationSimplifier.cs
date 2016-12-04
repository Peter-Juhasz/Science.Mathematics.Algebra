using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like sin(f(x)) to f'(x) * cos(f(x)).
    /// </summary>
    internal sealed class SineDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            var invocation = expression.Expression as FunctionInvocationExpression;
            if (invocation?.Name == SineFunctionExpression.PrimaryName)
            {
                var arg = invocation.Arguments.Single();
                return arg.Differentiate(expression.RespectTo) * Cosine(arg);
            }

            return expression;
        }
    }
}
