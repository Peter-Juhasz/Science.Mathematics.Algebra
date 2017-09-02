using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like int ln x dx to x ln x - x + C.
    /// </summary>
    internal sealed class NaturalLogatirhmIntegralSimplifier : ISimplifier<IntegralExpression>
    {
        public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is LogarithmFunctionExpression logarithm)
            {
                if (logarithm.Argument == expression.RespectTo)
                    return expression.RespectTo * expression.Expression - expression.RespectTo + IntegralConstant;
            }

            return expression;
        }
    }
}
