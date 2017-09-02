using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit
{
    using static ExpressionFactory;

    public class LimitOfCosineSimplifier : ISimplifier<LimitExpression>
    {
        public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is CosineFunctionExpression cosine)
            {
                if (expression.To.IsConstant(expression.RespectTo) &&
                    !expression.To.IsInfinity() &&
                    cosine.Argument.Equals(expression.RespectTo))
                    return Cosine(expression.To);
            }

            return expression;
        }
    }
}
