using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit
{
    using static ExpressionFactory;

    public class LimitOfCosineSimplifier : ISimplifier<LimitExpression>
    {
        public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken)
        {
            var body = expression.Expression as FunctionInvocationExpression;
            if (body == null)
                return expression;

            if (body.Name != WellKnownFunctionNames.Cosine)
                return expression;

            if (expression.To.IsConstant(expression.RespectTo) &&
                !expression.To.IsInfinity() &&
                body.Arguments.Single().Equals(expression.RespectTo))
                return Cosine(expression.To);

            return expression;
        }
    }
}
