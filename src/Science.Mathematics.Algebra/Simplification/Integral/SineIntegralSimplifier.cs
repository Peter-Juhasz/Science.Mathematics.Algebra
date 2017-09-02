using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like int sin x dx to -cos x + C.
    /// </summary>
    internal sealed class SineIntegralSimplifier : ISimplifier<IntegralExpression>
    {
        public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is SineFunctionExpression sine)
            {
                if (sine.Argument == expression.RespectTo)
                    return -Cosine(sine.Argument) + IntegralConstant;
            }

            return expression;
        }
    }
}
