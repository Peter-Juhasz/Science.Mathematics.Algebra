using System.Threading;

namespace Science.Mathematics.Algebra
{
    /// <summary>
    /// Simplifies expressions to their constant value, if possible.
    /// </summary>
    public sealed class ConstantSimplifier : ISimplifier<AlgebraExpression>
    {
        public AlgebraExpression Simplify(AlgebraExpression expression, CancellationToken cancellationToken)
        {
            if (expression is ConstantExpression)
                return expression;

            double? constantValue = expression.GetConstantValue(cancellationToken);

            if (constantValue != null)
                return ExpressionFactory.Constant(constantValue.Value);

            return expression;            
        }
    }
}
