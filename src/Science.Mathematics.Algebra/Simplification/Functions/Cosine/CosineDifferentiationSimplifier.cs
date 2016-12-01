using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like cos(f(x)) to f'(x) * -sin(f(x)).
    /// </summary>
    internal sealed class CosineDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            var invocation = expression.Expression as FunctionInvocationExpression;
            if (invocation?.Name == CosineFunctionExpression.PrimaryName)
            {
                var arg = invocation.Arguments.Single();
                return arg.Differentiate(expression.RespectTo) * -ExpressionFactory.Sine(arg);
            }

            return expression;
        }
    }
}
