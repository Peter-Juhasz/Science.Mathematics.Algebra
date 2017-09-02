using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like int log_a x dx to x log_a x - x / ln a + C.
    /// </summary>
    internal sealed class LogatirhmIntegralSimplifier : ISimplifier<IntegralExpression>
    {
        public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is LogarithmFunctionExpression logarithm)
            {
                if (logarithm.Argument == expression.RespectTo &&
                    logarithm.Base.IsConstant(expression.RespectTo))
                    return logarithm.Argument * logarithm - logarithm.Argument / NaturalLogarithm(logarithm.Base) + IntegralConstant;
            }

            return expression;
        }
    }
}
