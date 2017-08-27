using System.Threading;

namespace Science.Mathematics.Algebra
{
    using static ExpressionFactory;

    /// <summary>
    /// Simplifies expressions to their constant value, if possible.
    /// </summary>
    internal sealed class ConstantSimplifier : ISimplifier<AlgebraExpression>
    {
        public AlgebraExpression Simplify(AlgebraExpression expression, CancellationToken cancellationToken)
        {
            if (expression is NumberExpression)
                return expression;

            double? constantValue = expression.GetConstantValue(cancellationToken);

            if (constantValue != null)
                return Number(constantValue.Value);

            return expression;
        }
    }
}
