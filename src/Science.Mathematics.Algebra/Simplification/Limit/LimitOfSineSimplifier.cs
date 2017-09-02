using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit
{
    using static ExpressionFactory;

    public class LimitOfSineSimplifier : ISimplifier<LimitExpression>
    {
        public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken)
        {
            if (expression.Expression is SineFunctionExpression sine)
            {
                if (expression.To.IsConstant(expression.RespectTo) &&
                    !expression.To.IsInfinity() &&
                    sine.Argument.Equals(expression.RespectTo))
                    return Sine(expression.To);
            }

            return expression;
        }
    }
}
