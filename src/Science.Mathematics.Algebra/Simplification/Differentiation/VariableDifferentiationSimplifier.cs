using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions like d/dx x to 1.
    /// </summary>
    internal sealed class VariableDifferentiationSimplifier : ISimplifier<DifferentiationExpression>
    {
        public AlgebraExpression Simplify(DifferentiationExpression expression, CancellationToken cancellationToken)
        {
            if ((expression.Expression as VariableExpression)?.Equals(expression.RespectTo) ?? false)
                return ExpressionFactory.One;

            return expression;
        }
    }
}
