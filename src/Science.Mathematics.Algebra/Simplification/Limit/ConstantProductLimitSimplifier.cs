using System.Linq;
using System.Threading;

namespace Science.Mathematics.Algebra.Simplification.Limit;

using static ExpressionFactory;

public class ConstantProductLimitSimplifier : ISimplifier<LimitExpression>
{
	public AlgebraExpression Simplify(LimitExpression expression, CancellationToken cancellationToken)
	{
		var body = expression.Expression as ProductExpressionList;
		if (body == null)
			return expression;

		var constants = body.Terms.Where(t => t.IsConstant(expression.RespectTo)).ToList();
		if (!constants.Any())
			return expression;

		return Product(constants) * expression with { Expression = Product(body.Terms.Except(constants).ToList()) };
	}
}
