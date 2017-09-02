using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions like int cos x dx to sin x + C.
    /// </summary>
    internal sealed class CosineIntegralSimplifier : ISimplifier<IntegralExpression>
    {
        public AlgebraExpression Simplify(IntegralExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is CosineFunctionExpression sine)
            {
                if (sine.Argument == expression.RespectTo)
                    return Sine(sine.Argument) + IntegralConstant;
            }

            return expression;
        }
    }
}
