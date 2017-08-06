using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit
{
    using static ExpressionFactory;

    public class LimitOfSineSimplifier : ISimplifier<LimitExpression>
    {
        public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken)
        {
            var body = expression.Expression as FunctionInvocationExpression;
            if (body == null)
                return expression;

            if (body.Name != WellKnownFunctionNames.Sine)
                return expression;

            if (expression.To.IsConstant(expression.RespectTo) &&
                !expression.To.IsInfinity() &&
                body.Arguments.Single().Equals(expression.RespectTo))
                return Sine(expression.To);

            return expression;
        }
    }
}
