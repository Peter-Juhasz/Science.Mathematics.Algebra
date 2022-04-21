using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions like log_b (b ^ x) to x.
/// </summary>
internal sealed class LogarithmOfPowerWithSameBaseSimplifier : ISimplifier<LogarithmFunctionExpression>
{
	public AlgebraExpression Simplify(LogarithmFunctionExpression expression, CancellationToken cancellationToken)
	{
		if (expression.Argument is PowerExpression)
		{
			var power = expression.Argument as PowerExpression;

			if (expression.Base.Equals(power.Base))
				return power.Exponent;
		}

		return expression;
	}
}
