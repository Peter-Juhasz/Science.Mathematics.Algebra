using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions.
    /// </summary>
    internal sealed class GenericFunctionInvocationSimplifier : ISimplifier<FunctionInvocationExpression>
    {
        public AlgebraExpression Simplify(FunctionInvocationExpression expression, CancellationToken cancellationToken)
        {
            return expression
                .WithArguments(expression.Arguments.Select(a => a.Simplify()).ToImmutableList())
            ;
        }
    }
}
