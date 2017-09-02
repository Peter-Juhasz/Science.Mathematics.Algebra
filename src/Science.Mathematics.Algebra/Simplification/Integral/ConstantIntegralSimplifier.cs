using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like int k dx to kx + C.
    /// </summary>
    internal sealed class ConstantIntegralSimplifier : ISimplifier<IntegralExpression>
    {
        public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression.IsConstant(expression))
                return expression.Expression * expression.RespectTo + IntegralConstant;

            return expression;
        }
    }
}
