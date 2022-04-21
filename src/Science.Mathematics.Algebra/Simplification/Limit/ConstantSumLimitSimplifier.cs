using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit;

using static ExpressionFactory;

public class ConstantSumLimitSimplifier : ISimplifier<LimitExpression>
{
	public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken)
	{
		var body = expression.Expression as SumExpressionList;
		if (body == null)
			return expression;

		var constants = body.Terms.Where(t => t.IsConstant(expression.RespectTo)).ToList();
		if (!constants.Any())
			return expression;

		return Sum(constants) + expression with { Expression = Sum(body.Terms.Except(constants).ToList()) };
	}
}
