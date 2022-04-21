using System;
using System.Threading;

namespace Science.Mathematics.Algebra;

/// <summary>
/// Simplifies expressions.
/// </summary>
internal sealed class ConstantPowerSimplifier : ISimplifier<PowerExpression>
{
	public AlgebraExpression Simplify(PowerExpression expression, CancellationToken cancellationToken)
	{
		var @base = expression.Base.GetConstantValue(cancellationToken);
		var exponent = expression.Exponent.GetConstantValue(cancellationToken);
		
		if (@base != null && exponent != null)
			return (decimal)Math.Pow((double)@base.Value, (double)exponent.Value);

		return expression;
	}
}
