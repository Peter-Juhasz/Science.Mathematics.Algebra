using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like log_x (x).
    /// </summary>
    internal sealed class BaseEqualsToArgumentLogarithmSimplifier : ISimplifier<LogarithmFunctionExpression>
    {
        public AlgebraExpression Simplify(LogarithmFunctionExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Argument.Equals(expression.Base))
                return One;

            return expression;
        }
    }
}
