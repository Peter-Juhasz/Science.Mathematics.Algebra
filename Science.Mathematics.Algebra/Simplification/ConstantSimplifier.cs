using System.Threading;

namespace Science.Mathematics.Algebra.Simplification
{
    /// <summary>
    /// Simplifies expressions to their constant value, if possible.
    /// </summary>
    public sealed class ConstantSimplifier : ISimplifier<AlgebraExpression>
    {
        public AlgebraExpression Simplify(AlgebraExpression expression, CancellationToken cancellationToken)
        {
            double? constantValue = expression.GetConstantValue();

            if (constantValue != null)
                return ExpressionFactory.Constant(constantValue.Value);

            return expression;            
        }
    }
}
